using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.IO;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.Follow;

/*
 *   1.路径点1
 *       打开门并继续导航不是规划新路线，到达地点1            60分
 *   2.路径点2
 *      发现并要求人让开路                               10分
 *      移动障碍物来到达地点，到达地点2                     50分
 *   3.路径点3
 *      开始跟随导航者，到达地点4
 *       重新进入区域后重新到达地点3                       40分
 *   4.路径点4
 *       避开盒状障碍物和3D障碍物                         20分
 *   5.离开区域                                        10分
 *   6.
 *      未到达                                        -50分
 *      出色表现                                        20分
 *      
 *                                                    200分
*/

namespace SITRobotSystem_wpf.BLL.Competitions.Navigation_Test
{

    class NavigationTestStages:Stage
    {
        private NavigationTestTask navigationTestTask;
        private FollowCompetition followCompetition;

        public NavigationTestStages()
        {
            navigationTestTask = new NavigationTestTask();
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public override void init()
        {
            navigationTestTask.initBodyDetect();
            navigationTestTask.initSpeech();
            navigationTestTask.initSurfFrmae();
        }

        /// <summary>
        /// 等待门开启
        /// </summary>
        public void WaitForDoor()
        {
            navigationTestTask.WaitForDoor();
        }

        /// <summary>
        /// 打开门到达点1
        /// </summary>
        public virtual void Point1ToDoor()
        {
            if(true)
            {
                navigationTestTask.moveToPlaceByName("door1");
                if(navigationTestTask.IsDoorOpened())
                {
                    navigationTestTask.moveToPlaceByName("door2");
                }
                    if(navigationTestTask.IsDoorOpened())
                    {
                        navigationTestTask.moveToPlaceByName("door3");
                    }
            }

            //手臂开门
            //navigationTestTask.HandOpenDoor();
            
            navigationTestTask.moveToPlaceByName("waypoint1");
            
        }

        /// <summary>
        /// 不开门到达点1
        /// </summary>
        public virtual void Point1NoDoor()
        {
           // bool res;
            navigationTestTask.moveToPlaceByName("waypoint1");
            //if (res)
                navigationTestTask.speak("I have reached waypoint one");
            //else
            //    navigationTestTask.speak("Sorry,I can not reach way point one");
        }

        public virtual void ToPoint2()
        {
            bool PeoFou=false;
            DateTime dt, dtbegin;
            dt = dtbegin = DateTime.Now;

            //在此点检测障碍物到底是物体还是人
            navigationTestTask.moveToPlaceByName("beforewaypoint2");

            PeoFou=navigationTestTask.IsPeople();

            if (PeoFou)
            {
                navigationTestTask.FindPeople();
                navigationTestTask.speak("Please stand aside");
                while (true)
                {
                    PeoFou = false;
                    dt = DateTime.Now;
                    PeoFou = navigationTestTask.IsPeople();
                    if (PeoFou == false) break;
                    if((dt-dtbegin).Seconds>10) break;
                }
                navigationTestTask.moveToPlaceByName("waypoint2");
                navigationTestTask.speak("i have reached waypoint two");
            }
            else
            {
                navigationTestTask.MoveObject();
            }
        }

        public virtual void ToPoint3()
        {
            navigationTestTask.moveToPlaceByName("waypoint3");
            navigationTestTask.speak("i have reached waypoint three");
        }

        public void Follow()
        {
            //navigationTestTask.followEasy();
            followCompetition = new FollowCompetition();
            followCompetition.Start();
        }

        public virtual void ToPoint4()
        {
            navigationTestTask.moveToPlaceByName("waypoint3");
            navigationTestTask.speak("i have reached waypoint three");
        }

        public virtual void Exit()
        {
            navigationTestTask.moveToPlaceByName("exit");
        }
    }
}
