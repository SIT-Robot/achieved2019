using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Stages
{
    class FollowStage:Stage
    {
        public FollowTask followTask;

        public FollowStage()
        {
            followTask = new FollowTask();
        }
        public override void init()
        {
            
            followTask.initBodyDetect();
        }


        int userID = 0;
        public bool isStartFollow = false;
        private bool stage2Flag = false;
        private int stage1Flag = 0;
        /// <summary>
        /// 确认人：动作为单手握拳举过头顶
        /// </summary>
        /// <param name="user"></param>
        /// <param name="leg"></param>
        public void ensureUser(ref User user, ref Leg leg)
        {
            //确认人阶段
            while (isStartFollow == false)
            {
                //followTask.FlashCamShift();

                userID = followTask.RememberUserByHandState();
                Leg userLeg = new Leg();
                user = followTask.findUser(userID);

                if (userID != 0)
                {
                    userLeg = followTask.findLegByUser(user);
                    if (userLeg.PeopleID != "null")
                    {
                        leg.PeopleID = userLeg.PeopleID;
                        isStartFollow = true;
                    }
                }
            }
        }

        private bool peopleFlag = false;
        private bool legFlag = false;
        private bool simliarFlag = false;

        private bool sendspeedFlag = true;
        bool run = true;
        public virtual void followTrackingUser(ref User findingUser, ref Leg userleg)
        {
            int times = 0;
            
            while (run)
            {
                Point twist = new Point(0, 0, 0);

                //人体识别
                User user = followTask.findUser(findingUser.ID);
                if (user != null)
                {
                    peopleFlag = user.ID != 0;
                }
                else
                {
                    peopleFlag = false;
                }

                //leg识别
                Leg userLeg = followTask.GetLeg(userleg);
                legFlag = userLeg.PeopleID != "null";

                //相似识别
                if (user != null)
                {
                    bool simliarFlag = followTask.FindSimilar(user, userLeg);
                }


                if (stage1Flag == 1)
                {
                    if (user == null)
                    {
                        stage1Flag = 1;
                    }
                }
                Console.WriteLine(legFlag.ToString() + " " + peopleFlag.ToString());
                //人丢失，腿存在
                if (!peopleFlag && legFlag)
                {
                    twist = followTask.ComputSpeed(userLeg);
                    user = followTask.findUserByLeg(userLeg);
                }
                //只剩人， 腿丢失
                if (peopleFlag && !legFlag)
                {
                    followTask.findLegByUser(findingUser);
                    twist = new Point(0, 0, 0);
                }
                if (peopleFlag && legFlag)
                {
                    user.trackingHand();
                    user.trackingHandPush();
                    if (!user.isHandPush)
                    {
                        twist = followTask.ComputSpeed(user.BodyCenter);
                    }
                    else
                    {
                        if ((!user.isHandLeftPush && user.isHandRightPush) || (user.isHandLeftPush && !user.isHandRightPush))
                        {
                            
                            SingleHandPush();
                            //单手推-暂停

                        }
                        if ((user.isHandLeftPush && user.isHandRightPush))
                        {
                            BothHandPush();

                            //双手推
                        }

                    }
                }
                //Point  twist=followTask.computeSpeedCamShift(bodyColorSpacePoint);
                if (sendspeedFlag)
                {
                    followTask.SendSpeed(twist);
                }
                // return 1;
            }
            followTask.SpeedStop();

        }
        /// <summary>
        /// 单手张开往前推
        /// </summary>
        public virtual void SingleHandPush()
        {
            followTask.SpeedStop();
        }
        /// <summary>
        /// 双手张开往前推
        /// </summary>
        public virtual void BothHandPush()
        {
            followTask.SpeedStop();
        }

        public void EndFollow()
        {
            run = false;           
        }
    }
}
