using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

/*
 * 1．正确理解命令(机器人完整复述相应的任务) …………………………200 分
2．正确完成执行第一个任务……………………………………………………200 分
3．正确完成执行第一和第二个任务…………………………………………300 分
5
4．正确完成执行所有的任务……………………………………………………300 分
5．使用开始开关……………………………………………………………………-50 分
6．由非队员下达任务并被机器人理解………………………………………200 分
7．机器人没正确理解完整的命令，但是理解了部分，并完成该任务，每完
成一个……………………………………………………………………………………………200 分
 */

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR
{
    class GpsrStages:Stage
    {
        private GpsrTask gpsrTask;
        public GpsrStages()
        {
            gpsrTask = new GpsrTask();
        }

        /// <summary>
        /// 初始化开启窗口服务等
        /// </summary>
        public override void init()
        {
            gpsrTask.initSurfFrmae();
            gpsrTask.initBodyDetect();
            gpsrTask.initSpeech();
        }

        public void testSurf()
        {
            gpsrTask.FaceToGoods(gpsrTask.GetGoodsByName("water"));
        }
        /// <summary>
        /// 等待门
        /// </summary>
        public void WaitForDoor()
        {
            gpsrTask.WaitForDoor();
        }

        /// <summary>
        /// 自主进场
        /// </summary>
        public virtual void ComeIn()
        {
            if (true)
            {
                gpsrTask.moveToPlaceByName("center");
            }
        }

        /// <summary>
        /// 正确理解命令(机器人完整复述相应的任务) …………………………200 分
        /// </summary>
        public virtual List<Command>  SpeechReconigizeCommand()
        {
            gpsrTask.speakReady();
            string commandStrs="";
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = gpsrTask.speechGetCommandWithDataBase();
            }
            List<Command > commands= gpsrTask.commandTranslate(commandStrs);
            return commands;
        }

        /// <summary>
        /// 完成执行任务
        /// </summary>
        /// <param name="command"></param>
        public void ProcessCommand(Command command)
        {
            //执行命令
            switch (command.action)
            {
                    //执行放置物品的任务
                case ActionType.put:
                    gpsrTask.speak("I will put the thing on table.");
                    Thread.Sleep(1000);
                    gpsrTask.ArmPut();
                    break;
                    //移动到某个地点
                case ActionType.move:
                    
                    //gpsrTask.speak("I will go to "+command.thing.Name);

                    gpsrTask.moveToPlaceByName(command.thing.Name);
                    break;
                    //抓取物体  移动
                case ActionType.get:
                    Goods g = new Goods();
                    //取出物体
                    g = gpsrTask.GetGoodsByName(command.thing.Name);
                    //识别并面对物体
                     bool res=gpsrTask.FaceToGoods(g);
                    Thread.Sleep(1000);
                    //根据物体选择机械臂手势
                    if(res )
                    if (g.Name=="paper")
                    {
                        gpsrTask.ArmGetBig();
                    }
                    else
                    {
                        gpsrTask.ArmGet();
                    }
                    
                    break;
                    //寻找人
                case ActionType.find:
                    if (command.thing.Name=="people")
                    {
                        //gpsrTask.moveToPlaceByName(command.thing.Name);
                        User user = new User();
                        while (true)
                        {
                            user = gpsrTask.findUserMiddleRasingHand(gpsrTask.getAllUser());
                            if (user.ID != 0)
                            {
                                break;
                            }
                        }
                        gpsrTask.speak("Ok,I find you");
                        gpsrTask.GetCloseToUser(user);
                    }
                    else
                    {
                        gpsrTask.speak("start finding");
                        Goods g1 = new Goods();
                        g1 = gpsrTask.GetGoodsByName(command.thing.Name);
                        gpsrTask.speak("Ok,I have finished");
                    }                                        
                    break;
                case ActionType.tell:
                    gpsrTask.speak("the time is "+DateTime.Now.ToString("HH:mm"));
                    break;
                case ActionType.ask:
                    gpsrTask.baseSpeech = new SitRobotSpeech();
                    gpsrTask.baseSpeech.askName();
                    while (gpsrTask.baseSpeech.ReturnName!=null)
                    {                       
                        break;
                    }
                    break;
                case ActionType.count:
                    gpsrTask.speak("I find one "+command.thing.Name+" at here");
                    break;
                case ActionType.say:
                    gpsrTask.speak(command.thing.Name);
                    break;
                case ActionType.introduce:
                    gpsrTask.speak("Hello,my name is Well-E,Nice to meet you!");
                    break;
                case ActionType.follow:
                    gpsrTask.followEasy();
                    break;
                case ActionType.give:
                    gpsrTask.followEasy();
                    break;
                default:
                    break;

            }
        }

        public void goOut()
        {
            gpsrTask.moveToPlaceByName("door");
        }

        public void findPeople()
        {
            gpsrTask.speak("start finding you!");
            List<User> users = new List<User>();
            List<User> finalUsers = new List<User>();
            while (users.Count == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    users = gpsrTask.getAllUser();
                    foreach (var user in users)
                    {
                        if (finalUsers.Find(us => us.ID == user.ID) == null)
                        {
                            finalUsers.Add(user);
                        }

                    }
                }
                users = gpsrTask.getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            int numofbody = 0;
            foreach (var user in finalUsers)
            {
                if (user.ID != 0)
                {
                    numofbody++;
                    usersposition.Add(user.BodyCenter);
                }
            }
            gpsrTask.speak("I find you");
            foreach (var position in usersposition)
            {
                gpsrTask.moveToDirection(position.Z - 0.7f, -position.X, 0);
                gpsrTask.speak("here you are.");
                gpsrTask.ArmGive();
                gpsrTask.moveToPlaceByName("startpoint");
            }

        }
    }
}
