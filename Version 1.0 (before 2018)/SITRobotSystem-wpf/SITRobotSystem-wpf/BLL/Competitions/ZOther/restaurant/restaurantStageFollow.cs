using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.shopping;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.restaurant
{
    class restaurantStageFollow:Stage
    {
        public ShoppingTask task = new ShoppingTask();
        public Place[] place;
        public string[] objects;
        public string[] direction;

        public restaurantStageFollow(string[] objects)
        {
            this.objects = objects;
            this.place = new Place[objects.Length];
            this.direction = new string[objects.Length];
        }

        int userID = 0;
        public bool isStartFollow = false;

        public User ensureUser()
        {
            //确认人阶段
            while (isStartFollow == false)
            {
                userID = this.RememberUserByHandState();
                if (userID != 0)
                {
                    isStartFollow = true;
                }
            }
            User user = this.findUser(userID);
            return user;
        }

        public void followTrackingUser(User user)
        {
            int times = 0;
            bool run = true;
            //task.baseSpeech = new SitRobotSpeech();
            //task.speechGetRecognize(objects);

            

            while (times<6&&run)
            {
                //丢失人
                if (user == null)
                {
                    this.lostUser();
                }
                //未丢失人
                else if (user.ID != 0 && user.body.IsTracked)
                {
                    user.trackingHandPush();
                    if (!user.isHandPush)
                    {
                        //跟踪人
                        this.followUser(user);
                        //userID = user.ID;
                    }
                    else
                    {
                        this.WaitUser(user);
                        task.baseSpeech = new SitRobotSpeech();
                        task.speechRemeber(objects);

                        //task.speak("i am ready");
                        Console.WriteLine("0");
                        while (true)
                        {
                            //Console.WriteLine("1");
                            if (task.baseSpeech.ReturnCommand != null)
                            {
                                break;
                            }
                        }
                        int i = 0;
                        for (; i < objects.Length; i++)
                        {
                            if (objects[i]==task.baseSpeech.ReturnCommand)
                            {
                                break;
                            }
                        }
                        if (i == objects.Length-1)
                        {
                            place[i] = task.GetPlace();
                            
                            run = false;
                            times++;
                        }
                        else if(i < objects.Length-1)
                        {
                            place[i] = task.GetPlace();
                            direction[i] = task.baseSpeech.ReturnD;
                            Console.WriteLine("direction:" + direction[i]);
                            
                            times++;
                        }

                        Console.WriteLine("times:"+times);
                        Console.WriteLine("object:" + times);
                        //else
                        //{
                        //    task.baseSpeech.ReturnCommand = null;
                        //    task.baseSpeech.ReturnD = null;
                        //}
                        
                    }
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void init()
        {
            task.initBodyDetect();
        }


        public int TrackedPeopleID=0;

        /// <summary>
        /// 通过手势的形式记住人
        /// </summary>
        /// <returns></returns>
        public int RememberUserByHandState()
        {
            List<User> users= task.getAllUser();
            User handOnUser = task.findUserMiddleRasingHand(users);
            return handOnUser.ID;
        }

        public User findUser(int userId)
        {
            User user = task.findCorrectUser(userId);
            return user;
        }

        public void followUser(User user)
        {
            Console.WriteLine("Tracking   ID:" + user.ID.ToString() + "  Height:" + user.UserHeight.ToString() + "  X:" +
                              user.BodyCenter.X.ToString() + "  Z:" + user.BodyCenter.Z.ToString() + "  confidence:" +
                              user.confidence.ToString());
            
            Point twist= task.ComputSpeed(user.BodyCenter);

            task.SendSpeed(twist);
        }

        public void WaitUser(User user)
        {
            task.findCorrectUser(user.ID);
            task.SpeedStop();
        }

        public void lostUser()
        {
            //用SURF找回User
            return;
        }
    }
}
