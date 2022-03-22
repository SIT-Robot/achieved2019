using SITRobotSystem_wpf.BLL.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Demonstrate
{
    /*
        1.到初始位置
        2.语音识别指令
        3.到达物品地点，语音说话（我到了这个地方）
        4.图像识别（抓取物品）（11、12），语音说话（成功抓取物品），后退25cm，再给手臂发送1000（归位用）
        5.到底放置地点，手臂发送抬手，语音说话（请喝水）
        6.确认倒水，然后倒水（手臂发送命令），手臂归位
        7.自己介绍
    */
    class DemonstrateStages:Stage
    {
        public DemonstrateTask deTask;
        public DemonstrateStages()
        {
            deTask = new DemonstrateTask();
        }

        /// <summary>
        /// 初始化开启窗口服务等
        /// </summary>
        public override void init()
        {
            deTask.initSurfFrmae();
            deTask.initSpeech();
            deTask.initBodyDetect();
        }

        public void GoToStartPoint()
        {
            deTask.moveToPlaceByName("people");
        }

        public void AskCommond()
        {
            deTask.baseSpeech.robotSpeak("what can i do for you");
            deTask.AskPeople();
            Console.WriteLine("123");
            while (this.deTask.baseSpeech.ReturnCommand != "1haha")
            {
            }
            Console.WriteLine("23");
            this.deTask.baseSpeech.ReturnCommand = "";
        }

        private string command;


        public void UnderStandCommond(string res)
        {
            command = res;
        }

        public void GoToWater()
        {
            deTask.moveToPlaceByName("water");
            deTask.baseSpeech.robotSpeak("i am here");
        }

        //抓取物品
        public void CatchObject()
        {
            bool res=deTask.FaceToGoods(deTask.GetGoodsByName("water"));
            //if (res)
            {
                deTask.ArmGet();
                
            }
            //else
           /* {
                deTask.baseSpeech.robotSpeak("sorry, i cannot catch water");
            }*/
            
            
            
         
        }

        public  void GoToPeople()
        {
            deTask.moveToPlaceByName("people");
            deTask.ArmPut();
            deTask.baseSpeech.robotSpeak("Have water,please");
        }

        public void PourWater()
        {
            deTask.ArmPour();
            
        }

        public void Introduce()
        {
            deTask.baseSpeech.robotSpeak("i am wali,i come from shanghai institute of technology,nice to meet you");
        }

        public void AskPour()
        {
            deTask.baseSpeech.robotSpeak("Can i pour water");
            deTask.AskPeople();
            while (this.deTask.baseSpeech.ReturnCommand != "1haha")
            {
            }
            Console.WriteLine("23");
            this.deTask.baseSpeech.ReturnCommand = "";
        }
    }
}
