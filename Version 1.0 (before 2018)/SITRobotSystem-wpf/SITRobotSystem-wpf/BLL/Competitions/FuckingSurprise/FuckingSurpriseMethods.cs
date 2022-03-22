using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//add
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;  //用语音
using SITRobotSystem_wpf.BLL.Stages;       //using state
using SITRobotSystem_wpf.entity;
using System.Collections;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.FuckingSurprise
{
    class FuckingSurpriseMethods : Stage
    {
        public SitRobotSpeech sitrobotspeech = new SitRobotSpeech();
        public ArmCtrl armctrl = new ArmCtrl();
        public Tasks.Tasks task = new Tasks.Tasks();
        public override void init()
        {
            initMachineLearning();
            VisionCtrl.initSurf();
        }

        /// <summary>
        /// 人对着机器人讲话
        /// </summary>
        public void userSpeakToRobot()
        {
            //string something = "something";
           // sitrobotspeech.FuckingSurpriseRecognition(something);       //人说话      
        }
        public void funnyrobot()
        {
            sitrobotspeech.robotSpeak("hello my name is Alice");
            //task.TrainFace("tom");
            // Thread.Sleep(1);
            // string src = "KinectScreenshot-Color-10-47-47.png";
            //string foundname=  task.FindName(src);
            // while (true)
            // {
            //     task.moveToPlaceByName("startpoint");
            //     //task.moveDirectionBySpeed(1.6f, 0);
            //     for (int i = 0; i < 4; i++)
            //     {

            //         task.moveDirectionBySpeed(0, -0.5f);
            //     }
            //     sitrobotspeech.robotSpeak("hello my name is Alice");
            //     task.moveToPlaceByName("endpoint");
            //     for (int i = 0; i < 4; i++)
            //     {
            //         sitrobotspeech.robotSpeak("hello my name is Alice");
            //         task.moveDirectionBySpeed(0, 0.5f);
            //     }
            // }
        }

        public void movetoPlaceByName(string placeName)
        {
            
        }

        public void robotReaction()
        {

            while(true)
            {
                bool findedthings = false;
                task.moveToPlaceByName("desk");
                //task.moveDirectionBySpeed(5f, 0);
                findedthings = task.lookToGoodsMachineLearing("tea");
                armctrl.getThing(new ArmAction(601, "get"));
                task.adjustMoveDirection(FuckingSurpriseCompetition.targetCSP);
                armctrl.getThing(new ArmAction(600, "get"));
                task.moveToPlaceByName("table");
                armctrl.getThing(new ArmAction(700, "put"));
                task.moveToPlaceByName("startpoint");

            }
            bool whetherFindStuff;
            Random randomIndexOfStuff = new Random();
            int indexOfShit;
            string[] pileOfShit = {"milktea", "soap","shampoo", "pie", "biscuit",
                                   "paper", "nodles", "pie", "water", "toothpaste",
                                    "coffee", "redbull", "cola","tea"};

            indexOfShit = randomIndexOfStuff.Next(0, 13);
            string stuff = pileOfShit[indexOfShit];
            Console.WriteLine("indexOfShit:" + indexOfShit);
            task.moveToPlaceByName("fucking0");
            whetherFindStuff = task.lookToGoodsMachineLearing(stuff);//用深度学习找 stuff
            if (whetherFindStuff)
            {
                task.moveToPlaceByName("fuckingstart");
                task.speak("I found: " + stuff + ", follow me.");
                task.moveToPlaceByName("fucking0");
                task.speak("Here is the surprise " + stuff + " for you");
            }
            else
            {
                task.moveToPlaceByName("fuckinga");
                whetherFindStuff = task.lookToGoodsMachineLearing(stuff);
                if (whetherFindStuff)
                {
                    task.moveToPlaceByName("fuckingstart");
                    task.speak("I found: " + stuff + ", follow me.");
                    task.moveToPlaceByName("fuckinga");
                    task.speak("Here is the surprise " + stuff + " for you");
                }
                else
                {
                    task.moveToPlaceByName("fuckingb");
                    whetherFindStuff = task.lookToGoodsMachineLearing(stuff);
                    if (whetherFindStuff)
                    {
                        task.moveToPlaceByName("fuckingstart");
                        task.speak("I found: " + stuff + ", follow me.");
                        task.moveToPlaceByName("fuckingb");
                        task.speak("Here is the surprise " + stuff + " for you");
                    }
                    else
                    {
                        task.moveToPlaceByName("fuckingc");
                        whetherFindStuff = task.lookToGoodsMachineLearing(stuff);
                        if (whetherFindStuff)
                        {
                            task.moveToPlaceByName("fuckingstart");
                            task.speak("I found: " + stuff + ", follow me.");
                            task.moveToPlaceByName("fuckingc");
                            task.speak("Here is the surprise " + stuff + " for you");
                        }
                        else
                        {
                            task.moveToPlaceByName("fuckingd");
                            whetherFindStuff = task.lookToGoodsMachineLearing(stuff);
                            if (whetherFindStuff)
                            {
                                task.moveToPlaceByName("fuckingstart");
                                task.speak("I found: " + stuff + ", follow me.");
                                task.moveToPlaceByName("fuckingd");
                                task.speak("Here is the surprise " + stuff + " for you");
                            }
                            else
                            {
                                task.moveToPlaceByName("fuckinge");
                                whetherFindStuff = task.lookToGoodsMachineLearing(stuff);
                                if (whetherFindStuff)
                                {
                                    
                                    task.moveToPlaceByName("fuckingstart");
                                    task.speak("I found: " + stuff + ", follow me.");
                                    task.moveToPlaceByName("fuckinge");
                                    task.speak("Here is the surprise " + stuff + " for you");
                                }
                                else
                                {
                                    task.moveToPlaceByName("fuckingstart");
                                    task.speak("I found nothing. This is some air for you");
                                }
                            }
                        }
                    }
                }
            }
            armctrl.getThing(new ArmAction(600, "get"));//手臂动作
        }
        public void initMachineLearning()
        {
            VisionCtrl.initMachineLearning();
        }
    }
}
