using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Competitions.Follow;
using SITRobotSystem_wpf.BLL.Competitions.shopping;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2015
{
    class Restaurant2015StageFollow:Stage
    {
        public Restaurant2015Task task;
        private RestaurantFollowTask followTask;
        public Place[] place;
        public string[] objects;
        public string[] direction;

        public Restaurant2015StageFollow(string[] objects)
        {
            task = new Restaurant2015Task();
            followTask = new RestaurantFollowTask();
            this.objects = objects;
            this.place = new Place[objects.Length];
            this.direction = new string[objects.Length];
        }

        //int userID = 0;
        //public bool isStartFollow = false;

       
        //public User ensureUser()
        //{
        //    //确认人阶段
        //    while (isStartFollow == false)
        //    {
        //        userID = this.RememberUserByHandState();
        //        if (userID != 0)
        //        {
        //            isStartFollow = true;
        //        }
        //    }
        //    User user = this.findUser(userID);
        //    return user;
        //}


        //public Publisher handUpPublisher = new Publisher();

        public int followTrackingUser(User u)
        {
            int times = 0;
            bool run = true;
            while (times < 3 && run)
            {
                bool isRaisingHand=false;

                //followTask.FlashCamShift();
                ColorSpacePoint bodyColorSpacePoint = followTask.findCamShiftRes();

                User user = followTask.findUser(u.ID);
                User userCam = followTask.findUserByCamShiftResult(bodyColorSpacePoint);
                Point twist = followTask.computeSpeedCamShift(bodyColorSpacePoint);

                //丢失人
                if (userCam == null)
                {
                    followTask.lostUser();
                }
                //未丢失人
                else if (userCam.ID != 0 && userCam.body.IsTracked)
                {
                    user.trackingHand();
                    user.trackingHandPush();
                    if (!user.isHandPush)
                    {
                        twist = followTask.ComputSpeed(user.BodyCenter);
                        //if (user.isHandLeftPush && user.isHandRightPush)
                        //{
                        //    followTask.SpeedStop();
                        //    return -1;
                        //}
                    }
                    else
                    {
                        isRaisingHand = true;
                        twist = new Point(0, 0, 0);
                        followTask.WaitUser(user);
                    }
                }
                followTask.SendSpeed(twist);

                if (isRaisingHand)
                {
                    followTask.SpeedStop();
                    task.baseSpeech = new SitRobotSpeech();
                    task.speechRemeber(objects);

                    task.speak("i am ready");
                    Console.WriteLine("0");
                    while (task.baseSpeech.ReturnCommand == null)
                    {
                        Console.WriteLine("1");
                    }
                    int i = 0;
                    for (; i < objects.Length; i++)
                    {
                        if (objects[i] == task.baseSpeech.ReturnCommand)
                        {
                            break;
                        }
                    }
                    if (i == objects.Length - 2)
                    {
                        place[i] = task.GetPlace();

                        run = false;
                    }
                    else if (i < objects.Length - 2)
                    {
                        place[i] = task.GetPlace();
                        direction[i] = task.baseSpeech.ReturnD;
                        Console.WriteLine("direction:" + direction[i]);
                        times++;
                    }

                    Console.WriteLine("times:" + times);
                    Console.WriteLine("object:" + times);
                }
            }
            return 1;
        }
        //public void followTrackingUser(User user)
        //{
        //    int times = 0;
        //    bool run = true;
        //    //task.baseSpeech = new SitRobotSpeech();
        //    //task.speechGetRecognize(objects);

            

        //    while (times<5&&run)
        //    {
        //        //丢失人
        //        if (user == null)
        //        {
        //            this.lostUser();
        //        }
        //        //未丢失人
        //        else if (user.ID != 0 && user.body.IsTracked)
        //        {
        //            user.trackingHandPush();
        //            if (!user.isHandPush)
        //            {
        //                //跟踪人
        //                this.followUser(user);
        //                //userID = user.ID;
        //            }
        //            else
        //            {
        //                this.WaitUser(user);
        //                task.baseSpeech = new SitRobotSpeech();
        //                task.speechRemeber(objects);

        //                //task.speak("i am ready");
        //                Console.WriteLine("0");
        //                while (true)
        //                {
        //                    //Console.WriteLine("1");
        //                    if (task.baseSpeech.ReturnCommand != null)
        //                    {
        //                        break;
        //                    }
        //                }
        //                int i = 0;
        //                for (; i < objects.Length; i++)
        //                {
        //                    if (objects[i]==task.baseSpeech.ReturnCommand)
        //                    {
        //                        break;
        //                    }
        //                }
        //                if (i == objects.Length-2)
        //                {
        //                    place[i] = task.GetPlace();
                            
        //                    run = false;
        //                }
        //                else if(i < objects.Length-2)
        //                {
        //                    place[i] = task.GetPlace();
        //                    direction[i] = task.baseSpeech.ReturnD;
        //                    Console.WriteLine("direction:" + direction[i]);
                            
        //                    times++;
        //                }

        //                Console.WriteLine("times:"+times);
        //                Console.WriteLine("object:" + times);
        //                //else
        //                //{
        //                //    task.baseSpeech.ReturnCommand = null;
        //                //    task.baseSpeech.ReturnD = null;
        //                //}
                        
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 初始化
        /// </summary>
        public override void init()
        {
            followTask.initBodyDetect();
            //followTask.initCamshift();
        }


        //public int TrackedPeopleID=0;

        ///// <summary>
        ///// 通过手势的形式记住人
        ///// </summary>
        ///// <returns></returns>
        //public int RememberUserByHandState()
        //{
        //    List<User> users= task.getAllUser();
        //    User handOnUser = task.findUserMiddleRasingHand(users);
        //    return handOnUser.ID;
        //}

        //public User findUser(int userId)
        //{
        //    User user = task.findCorrectUser(userId);
        //    return user;
        //}

        //public void followUser(User user)
        //{
        //    Console.WriteLine("Tracking   ID:" + user.ID.ToString() + "  Height:" + user.UserHeight.ToString() + "  X:" +
        //                      user.BodyCenter.X.ToString() + "  Z:" + user.BodyCenter.Z.ToString() + "  confidence:" +
        //                      user.confidence.ToString());
            
        //    Point twist= task.ComputSpeed(user.BodyCenter);

        //    task.SendSpeedSmooth(twist);
        //}

        //public void WaitUser(User user)
        //{
        //    task.findCorrectUser(user.ID);
        //    task.SpeedStopSmooth();
        //}

        //public void lostUser()
        //{
        //    //用SURF找回User
        //    return;
        //}




        int userID = 0;
        public bool isStartFollow = false;
        private bool stage2Flag = false;
        private int stage1Flag = 0;
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

        public Publisher handUpPublisher = new Publisher();

        private bool peopleFlag = false;
        private bool legFlag = false;
        private bool simliarFlag = false;

        private bool sendspeedFlag = true;
        public void followTrackingUser(ref User findingUser, ref Leg userleg)
        {
            int times = 0;
            bool run = true;
            while (times<3&&run)
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
                    Console.WriteLine("have leg but lost user");
                    twist = followTask.ComputSpeed(userLeg);
                    user = followTask.findUserByLeg(userLeg);
                }
                //只剩人， 腿丢失
                if (peopleFlag && !legFlag)
                {
                    Console.WriteLine("have user but lost leg");
                    followTask.findLegByUser(findingUser);
                    twist = new Point(0, 0, 0);
                }
                if (peopleFlag && legFlag)
                {
                    Console.WriteLine("have user and leg");
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
                            task.SpeedStop();
                            //sendspeedFlag = false;

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
                            if (i == objects.Length-2)
                            {
                                place[i] = task.GetPlace();

                                run = false;
                            }
                            else if(i < objects.Length-2)
                            {
                                place[i] = task.GetPlace();
                                direction[i] = task.baseSpeech.ReturnD;
                                Console.WriteLine("direction:" + direction[i]);

                                times++;
                            }

                            Console.WriteLine("times:"+times);
                            Console.WriteLine("object:" + times);
                            //单手推-暂停

                        }
                        if ((user.isHandLeftPush && user.isHandRightPush))
                        {
                            task.SpeedStop();
                            //sendspeedFlag = false;
                            //双手手推-暂停
                          //  return 1;
                        }

                    }
                    if (!peopleFlag && !legFlag)
                    {
                        Console.WriteLine("lost user and leg");
                    }
                }
                //Point  twist=followTask.computeSpeedCamShift(bodyColorSpacePoint);
                if (sendspeedFlag)
                {
                    followTask.SendSpeed(twist);
                }




               // return 1;
            }
            task.SpeedStop();
                
            }

    }
}
