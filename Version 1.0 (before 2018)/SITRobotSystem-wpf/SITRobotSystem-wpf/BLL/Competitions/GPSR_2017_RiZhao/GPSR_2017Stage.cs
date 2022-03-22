/*
 用于实现各个步骤的得分点。
 1.正确理解指令（完整复述相应的任务)     150分 有3个 每个50分。
 2.正确完成第一个任务                   200分
 3.正确完成第二个任务                   200分
 4.正确完成第二个任务                   200分
 5.由非队员下达任务并被理解             100分

 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//add
using System.Threading;
using SITRobotSystem_wpf.BLL.Stages;
using Microsoft.Kinect;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.GPSR_2017_RiZhao;
using SITRobotSystem_wpf.BLL.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2017_RiZhao
{
    class GPSR_2017Stage : Stage
    {
        //initialization
        private GPSR_2017Task gPSR_2017Task;
        //private WhoisWhoTask whoisWhoTask;
        public SitRobotSpeech useSpeech;
        public SitRobotSpeech baseSpeech = new SitRobotSpeech();
        public BaseCtrl baseCtrl = new BaseCtrl();
        public GPSR_2017Stage()
        {
            gPSR_2017Task = new GPSR_2017Task();
        }

        public override void init()
        {
        }


        //理解指令
        public virtual List<Command> SpeechReconigizeCommand()   // return a object with two value  action & thing
        {

            baseSpeech.robotSpeak("My Lord?");             // robot will speak "My Lord?" at here
            string commandStrs = "";                       //keep a string from user
            while (string.IsNullOrEmpty(commandStrs))      
            {
                commandStrs = gPSR_2017Task.GPSR2017();     //process the string spoken by user , return string res
            }
            List<Command> commands = gPSR_2017Task.commandTranslate2017(commandStrs);
            return commands;
        }

        //处理指令
        public void ProcessCommand2017(Command command)//处理命令啊
        {
            //看gPSR_2015Task.commandTranslate2015中的所有任务需求写
            switch (command.action)
            {
                case ActionType.answeraquestion:
                    {
                        useSpeech.robotSpeakOwner("please ask me a question");
                        //useSpeech.SRADRecognize_ver2();
                    }
                    break;         
                case ActionType.move:
                    gPSR_2017Task.speak("My Lord I will go to " + command.thing.Name);
                    //  gPSR_2015Task.speak("I will go to bookcase");
                    gPSR_2017Task.moveToPlaceByName(command.thing.Name);
                    //  gPSR_2015Task.moveToPlaceByName("bookcase");
                    break;
                //抓取物体  移动
                case ActionType.get:
                    Goods g = new Goods();
                    //取出物体
                    g = gPSR_2017Task.GetGoodsByName(command.thing.Name);
                    //识别并面对物体
                    bool res = gPSR_2017Task.FaceToGoods(g);
                    Thread.Sleep(1000);
                    //根据物体选择机械臂手势
                    if (res)
                        if (g.Name == "paper")
                        {
                            gPSR_2017Task.ArmGetBig();
                        }
                        else
                        {
                            gPSR_2017Task.ArmGet();
                        }
                    //baseCtrl.moveToDirectionSpeed(-0.05f, 0);
                    break;
                case ActionType.back:
                    gPSR_2017Task.moveToPlaceByName("startpoint");
                    gPSR_2017Task.speak("here，my lord");
                    //改过
                    gPSR_2017Task.ArmGive();
                    break;
                case ActionType.give:
                    gPSR_2017Task.ArmGive();
                    break;
                case ActionType.put:
                    gPSR_2017Task.ArmPut();
                    break;
                case ActionType.lookfor:
                    //gPSR_2015Task.speak("start finding");
                    Goods g1 = new Goods();
                    g1 = gPSR_2017Task.GetGoodsByName(command.thing.Name);
                    bool lookres = gPSR_2017Task.LookForGoods(g1);
                    if (lookres)
                    {
                        gPSR_2017Task.speak("here is " + g1.Name + "!");
                    }
                    else
                    {
                        gPSR_2017Task.speak("I cannot find " + g1.Name + "!");
                    }


                    break;
            }
        }

    }
}//namespace
