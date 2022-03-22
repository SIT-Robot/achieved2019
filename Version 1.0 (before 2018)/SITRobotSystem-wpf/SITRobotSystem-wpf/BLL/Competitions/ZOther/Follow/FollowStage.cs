using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Follow
{
    class FollowStage:Stage
    {

        private FollowTask followTask;

        public FollowStage()
        {
            followTask = new FollowTask();
        }

        int userID = 0;
        public bool isStartFollow = false;
        private bool stage2Flag=false;
        private int stage1Flag = 0;
        public void ensureUser(ref User user,ref Leg leg)
        {
            //确认人阶段
            while (isStartFollow == false)
            {
                //followTask.FlashCamShift();
                Console.WriteLine("ensure User");
                userID = followTask.RememberUserByHandState();
                Leg userLeg=new Leg();
                user = followTask.findUser(userID);
                
                if (userID != 0)
                {
                    userLeg = followTask.findLegByUser(user);
                    if (userLeg.PeopleID!="null")
                    {
                        leg.PeopleID = userLeg.PeopleID;
                        isStartFollow = true;
                    }
                    
                }
                Thread.Sleep(50);
            }
        }

        public Publisher handUpPublisher=new Publisher();

        private bool peopleFlag = false;
        private bool legFlag = false;
        private bool simliarFlag = false;

        private bool sendspeedFlag = true;
        public int followTrackingUser(ref User findingUser,ref Leg userleg)
        {

            Point twist=new Point(0,0,0);

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


            if (stage1Flag==1)
            {
                if (user == null)
                {
                    stage1Flag = 1;
                }
            }
            Console.WriteLine(legFlag.ToString()+" "+peopleFlag.ToString());
            //人丢失，腿存在
            if (!peopleFlag && legFlag )
            {
                twist = followTask.ComputSpeed(userLeg);
                user = followTask.findUserByLeg(userLeg);
            }
            //只剩人， 推丢失
            if (peopleFlag && !legFlag )
            {
                followTask.findLegByUser(findingUser);
                twist = new Point(0, 0, 0);
            }
            if (peopleFlag && legFlag )
            {
                user.trackingHand();
                user.trackingHandPush();
                if (!user.isHandPush)
                {
                    twist = followTask.ComputSpeed(user.BodyCenter);
                }
                else
                {
                    if ((user.isHandLeftPush && user.isHandRightPush))
                    {
                        followTask.SpeedStop();
                        sendspeedFlag =false;
                        //followTask.speak("start going through");
                        stage1Flag = 0;
                    }
                    if(user.isRaisingHand)
                    {
                        stage1Flag = 1;
                    }
                    //if (stage1Flag==1)
                    //{
                    //    //followTask.SpeedStop();
                    //    sendspeedFlag = true ;
                    //    followTask.speak("continue follow");
                    //    stage1Flag = -1;
                    //}
                    //if (stage1Flag==-1)
                    //{
                    //    if (user.isRaisingHand && user.isHandLeftPush)
                    //    {
                    //        followTask.speak("OK,i will go to the lift");
                    //        stage2Flag = true;
                    //        //followTask.goback();
                    //        sendspeedFlag = false;
                    //    }
                    //    if (stage2Flag)
                    //    {
                    //        followTask.SendSpeed(new Point(0,0,-0.2));
                    //        Thread.Sleep(7850);
                    //        followTask.SendSpeed(new Point(0,0,0));
                    //        followTask.SendSpeed(new Point(0.5, 0, 0));
                    //        Thread.Sleep(10000);
                    //        followTask.SendSpeed(new Point(0, 0, 0));
                    //        followTask.SendSpeed(new Point(0, 0, +0.2));
                    //        Thread.Sleep(7850);
                    //        followTask.SendSpeed(new Point(0, 0, 0));
                    //    }                  
                    //}
                    ////twist = new Point(0, 0, 0);
                    ////followTask.WaitUser(user);
                    //handUpPublisher.issue();
                }
            }
            //Point  twist=followTask.computeSpeedCamShift(bodyColorSpacePoint);
            if (sendspeedFlag)
            {
                followTask.SendSpeed(twist);
            }




            return 1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void init()
        {
            followTask.initBodyDetect();
            
            //followTask.initCamshift();
        }


    }
}
