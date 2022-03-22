using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Org.BouncyCastle.Utilities.Collections;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.Navigation_Test
{
    class NavigationTestTask:Tasks.Tasks
    {
        public virtual bool IsDoorOpened()
        {
            bool res=false;
            int num=1;
            while (num<=5)
            {
                CameraSpacePoint centerPoint = visionCtrl.getCenterCameraSpacePoint();
                res = (centerPoint.Z > 1.5||centerPoint.Z<0);
                if (res)
                {
                    break;
                }
                Thread.Sleep(100);
                num++;
            }
            return res;
        }


        /// <summary>
        /// 使用手臂开启门
        /// </summary>
        public virtual void HandOpenDoor()
        {
            DateTime dt, dtbegin;
            dtbegin = DateTime.Now;
            dt = DateTime.Now;
            //举起手臂开门
            while ((dt-dtbegin).Seconds<15)
            {
                dt = DateTime.Now;
                ArmAction armAction0 = new ArmAction(11, "get");
                armCtrl.getThing(armAction0);
            }

            //放下手臂
            ArmAction armAction1 = new ArmAction(1000, "reset");
            armCtrl.send(armAction1);
        }


        /// <summary>
        /// 让人让开道路以到达点2
        /// </summary>
        public virtual void PersonAside()
        {
            List<User> users =getAllUser();
            GetCloseToUser(users[0]);
            speak("Please step aside, thank you");
            while(true)
            {
                users = getAllUser();
                if (users.Count == 0)
                    break;
            }
        }


        /// <summary>
        /// 移动障碍物到达点2
        /// </summary>
        public virtual void MoveObject()
        {
            speak("sorry, i can not move object.i will go to waypoint three");
        }

        public bool IsPeople()
        {
            DateTime dt, dtbegin;
            dtbegin = DateTime.Now;
            bool res=false;
            //speak("start finding people!");
            List<User> users = new List<User>();
            List<User> finalUsers = new List<User>();
            while (true)
            {
                dt = DateTime.Now;
                if ((dt - dtbegin).Seconds>5||users.Count!=0) break;
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    users = getAllUser();
                    foreach (var user in users)
                    {
                        if (finalUsers.Find(us => us.ID == user.ID) == null)
                        {
                            finalUsers.Add(user);
                        }

                    }
                }
                users = getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            //int numofbody = 0;
            //foreach (var user in finalUsers)
            //{
            //    if (user.ID != 0)
            //    {
            //        numofbody++;
            //        usersposition.Add(user.BodyCenter);
            //    }
            //}
            //roboNures2015Task.speak("I find " + numofbody + "people");
            //foreach (var position in usersposition)
            //{
            //    //moveToDirection(position.Z - 1.0f, -position.X, 0);
            //    speak("i found you.");
            //    //roboNures2015Task.ask();
            //    //CommandStrList.Add(roboNures2015Task.whoisWhoCommand);
            //}
            if (users.Count != 0) res = true;
            return res;
        }
    }
}
