using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using Microsoft.Kinect;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.GPSR_2015;
using SITRobotSystem_wpf.BLL.Tasks;

using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.UI;

/*
 * 1.得到指令
 *      第一次尝试成功理解指令             40分
 *      第二次尝试成功理解指令             20分
 *      第三次尝试成功理解指令             10分
 *      
 * 2.任务表现：科目1
 *      正确完成第一个任务                   10分
 *      正确完成第二个任务                   10分
 *      成功处理完整指令                    30分
 *      
 * 特殊奖励和处罚
 *      未到达                             -50分
 *      出色表现                            25分
 */

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2015
{
    class GPSR_2015Stage : Stage
    {
        private GPSR_2015Task gPSR_2015Task;
        //private WhoisWhoTask whoisWhoTask;
        public SitRobotSpeech useSpeech;
        public BaseCtrl baseCtrl = new BaseCtrl();
        public GPSR_2015Stage()
        {
            gPSR_2015Task = new GPSR_2015Task();
        }

        public override void init()
        {
            gPSR_2015Task.initBodyDetect();
            gPSR_2015Task.initSpeech();
            gPSR_2015Task.initSurfFrmae();
            gPSR_2015Task.initMachineLearning();
        }

        public void WaitForDoor()
        {
            gPSR_2015Task.WaitForDoor();
        }

        public void Start()
        {
            //if (true)
            //   //gPSR_2015Task.moveToPlaceByName("startpoint");
                
                baseCtrl.moveToDirectionSpeed(2f,0);
        }

        public virtual List<Command> SpeechReconigizeCommand()
        {
            
            gPSR_2015Task.speakReady();
            string commandStrs = "";
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = gPSR_2015Task.GPSR2015();
            }
            List<Command> commands = gPSR_2015Task.commandTranslate2015(commandStrs);
            return commands;
            /*
            gPSR_2015Task.speakReady();
            string commandStrs = "";
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = gPSR_2015Task.speechGetCommandWithDataBase();
            }
            List<Command> commands = gPSR_2015Task.commandTranslate(commandStrs);
            return commands;*/
 
        }

        public void ProcessCommand2015(Command command)
        {
            //看gPSR_2015Task.commandTranslate2015中的所有任务需求写
            switch(command.action)
            {
                case ActionType.tellwhatdayistomorrow:
                     DateTime dt8 = DateTime.Now;
                    gPSR_2015Task.speak(dt8.AddDays(1.0).ToString());
                    break;
                case ActionType.answeraquestion:
                    {
                        useSpeech = new SitRobotSpeech();
                        useSpeech.robotSpeakOwner("please ask me a question");
                        useSpeech.GPSR_SRADRecognize();
                    }
                    break;
                case ActionType.tellyourname:
                    gPSR_2015Task.speak("Hello, my name is wali, nice to meet you");
                    break;
                case ActionType.tellthenameofyourteam:
                    gPSR_2015Task.speak("Hello, our team name is chuang zhi feng");
                    break;
                case ActionType.tellwhattimeisit:
                case ActionType.tellthetime:
                    DateTime dt = DateTime.Now;
                    gPSR_2015Task.speak(dt.ToString("HH:mm:ss"));
                    break;
                case ActionType.telltellthedate:
                    DateTime dt2 = DateTime.Now;
                    gPSR_2015Task.speak(dt2.ToString("yyyy-MM-dd"));
                    break;
                case ActionType.tellwhatdayistoday:
                    DateTime dt3 = DateTime.Now;
                    gPSR_2015Task.speak(dt3.DayOfWeek.ToString());
                    break;
                case ActionType.tellthedayofthemonth:
                    DateTime dt4 = DateTime.Now;
                    gPSR_2015Task.speak(dt4.Day.ToString());
                    break;
                case ActionType.move:

                    gPSR_2015Task.speak("I will go to " + command.thing.Name);
                    //  gPSR_2015Task.speak("I will go to bookcase");
                    
                        gPSR_2015Task.moveToPlaceByName(command.thing.Name);
                    
                  //  gPSR_2015Task.moveToPlaceByName("bookcase");
                    break;
                  //抓取物体  移动
                case ActionType.get:
                    Goods g = new Goods();
                    //取出物体
                    g = gPSR_2015Task.GetGoodsByName(command.thing.Name);
                    //识别并面对物体

                     
                    //bool res = gPSR_2015Task.FaceToGoods(g);
                    gPSR_2015Task.speak("I am finding " + g.Name);
                    //WindowCtrl.StartMachineLearingTestWindow();
                    //bool res = WindowCtrl.machineLearningTestWindow.findForGPSR(g.Name);
                    bool res = gPSR_2015Task.lookToGoodsMachineLearing(g.Name);

                    Thread.Sleep(1000);
                    //根据物体选择机械臂手势
                    if (res)
                    {
                        gPSR_2015Task.ArmGet();
                    }
                    //baseCtrl.moveToDirectionSpeed(-0.05f, 0);
                    break;
                case ActionType.back:
                    gPSR_2015Task.moveToPlaceByName("startpoint");
                    gPSR_2015Task.speak("Here you are");
                    //改过
                    gPSR_2015Task.ArmGive();
                    break;

                case ActionType.back2:
                    gPSR_2015Task.moveToPlaceByName("startpoint");
                    gPSR_2015Task.speak("I have finished your task");
                    break;

                case ActionType.give:
                    gPSR_2015Task.ArmGive();
                    break;
                case ActionType.put:
                    gPSR_2015Task.ArmPut();
                    break;
                case ActionType.lookfor:
                    //gPSR_2015Task.speak("start finding");
                    Goods g1 = new Goods();
                    g1 = gPSR_2015Task.GetGoodsByName(command.thing.Name);
                    bool lookres=gPSR_2015Task.lookToGoodsMachineLearing(g1.Name);
                    if (lookres)
                    {
                        gPSR_2015Task.speak("here is "+g1.Name+"!");    
                    }
                    else
                    {
                        gPSR_2015Task.speak("I cannot find "+ g1.Name + "!"); 
                    }

                    
                    break;

                case ActionType.find:
                    User user = new User();
                    while (true)
                    {
                        user = gPSR_2015Task.findUserMiddleRasingHand(gPSR_2015Task.getAllUser());
                        if (user.ID != 0)
                        {
                            break;
                        }
                    }
                    gPSR_2015Task.speak("Ok,I find you");
                    gPSR_2015Task.GetCloseToUser(user);
                    break;
                case ActionType.follow:
                    //gPSR_2015Task.followEasy();
                    gPSR_2015Task.Follow();
                    break;
            }
        }

        public void ProcessCommand(Command command)
        {
            //执行命令
            switch (command.action)
            {
                //执行放置物品的任务
                case ActionType.put:
                    gPSR_2015Task.speak("I will put the thing on table.");
                    Thread.Sleep(1000);
                    gPSR_2015Task.ArmPut();
                    break;
                //移动到某个地点
                case ActionType.move:

                    //gPSR_2015Task.speak("I will go to "+command.thing.Name);

                    gPSR_2015Task.moveToPlaceByName(command.thing.Name);
                    break;
                //抓取物体  移动
                case ActionType.get:
                    Goods g = new Goods();
                    //取出物体
                    g = gPSR_2015Task.GetGoodsByName(command.thing.Name);
                    //识别并面对物体
                    //bool res = gPSR_2015Task.FaceToGoods(g);

                    bool res = gPSR_2015Task.lookToGoodsMachineLearing(g.Name);

                    Thread.Sleep(1000);
                    //根据物体选择机械臂手势
                    if (res)
                        if (g.Name == "paper")
                        {
                            gPSR_2015Task.ArmGetBig();
                        }
                        else
                        {
                            gPSR_2015Task.ArmGet();
                        }

                    break;
                //寻找人
                case ActionType.find:
                    if (command.thing.Name == "0")
                    {
                        //gPSR_2015Task.moveToPlaceByName(command.thing.Name);
                        User user = new User();
                        while (true)
                        {
                            user = gPSR_2015Task.findUserMiddleRasingHand(gPSR_2015Task.getAllUser());
                            if (user.ID != 0)
                            {
                                break;
                            }
                        }
                        gPSR_2015Task.speak("Ok,I find you");
                        gPSR_2015Task.GetCloseToUser(user);
                    }
                    else
                    {
                        gPSR_2015Task.speak("start finding");
                        Goods g1 = new Goods();
                        g1 = gPSR_2015Task.GetGoodsByName(command.thing.Name);
                        gPSR_2015Task.speak("Ok,I have finished");
                    }
                    break;
                case ActionType.tell:
                    gPSR_2015Task.speak("the time is " + DateTime.Now.ToString("HH:mm"));
                    break;
                case ActionType.ask:
                    gPSR_2015Task.baseSpeech = new SitRobotSpeech();
                    gPSR_2015Task.baseSpeech.askName();
                    while (gPSR_2015Task.baseSpeech.ReturnName != null)
                    {
                        break;
                    }
                    break;
                case ActionType.count:
                    gPSR_2015Task.speak("I find one " + command.thing.Name + " at here");
                    break;
                case ActionType.say:
                    gPSR_2015Task.speak(command.thing.Name);
                    break;
                case ActionType.introduce:
                    gPSR_2015Task.speak("Hello,my name is Well-E,Nice to meet you!");
                    break;
                case ActionType.follow:
                    //gPSR_2015Task.followEasy();
                    gPSR_2015Task.Follow();
                    break;
                case ActionType.give:
                    //gPSR_2015Task.followEasy();
                    gPSR_2015Task.Follow();
                    break;
                default:
                    break;
            }
        }

        public void Exit()
        {
            gPSR_2015Task.moveToPlaceByName("exit");
        }
    }
}
