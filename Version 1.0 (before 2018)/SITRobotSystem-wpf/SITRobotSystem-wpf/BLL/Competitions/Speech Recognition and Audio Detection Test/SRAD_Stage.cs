using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf .BLL.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Speech_Recognition_and_Audio_Detection_Test
{
       class SRAD_Stage : Stage
       {
           public SRAD_Task STask;
           public BaseCtrl baseCtrl;
           public SRAD_Stage()
           {
               STask=new SRAD_Task();
           }


        public override void init()
        {
            this.STask.initSpeech();
            this.STask.initSurfFrmae();
            this.STask.initBodyDetect();
            //this.personrecognitionTask.visionCtrl.readyVisionHub();
        }

        public void ReadyToAnswer()                              //说话示意准备好并准备答题
        {
            int NumofFace;
            STask.baseSpeech.robotSpeak("I want to play riddle");
            System.Threading.Thread.Sleep(10000);

            STask.moveDirectionBySpeedW(180);

            //通过人脸识别来判断站在机器人面前的人头数
            //NumofFace = STask.getfaceNum();
            //NumofFace = STask.getAllUser();
            NumofFace = STask.getfaceNum();
            Console.WriteLine("There are " + NumofFace + "people in front of me");
            STask.baseSpeech.robotSpeak("There are " + NumofFace + " people in front of me");

            STask.baseSpeech.robotSpeak("Who want to play riddle with me?");
             
            //开始进行语音识别，即一问一答的形式
            STask.showSRADRecognized();
            STask.moveToPlaceByName("table");
            //baseCtrl.moveToGoal();

            Console.WriteLine("123");
            /*while (this.STask .baseSpeech .ReturnCommand  != "1haha")
            {
            这个循环暂时没什么卵用
            }*/
            Console.WriteLine("23");
            this.STask.baseSpeech.ReturnCommand = "";
        }

    }
}
