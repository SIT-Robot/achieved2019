using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Connection;
using Microsoft.Kinect;

/*
  1.到达需求
 *      在被呼叫后到达病人               20分
 *      等待拿药命令                    10分
 * 
  2.描述药品
 *      真实表述时间                    50分
 *      小于5秒                        30分
 *      大于5秒小于15秒                 20分
 *      大于15秒小于30秒                10分
 *      大于30秒                       0分
 *      
 *3.取药
 *      选择正确的药品                   40分
 *      抓取正确的药品                   20分
 *      抓取错误的药品                   5分
 *      
 * 4.递送药品
 *      自然的递送                       20分
 *      有帮助的递送                     10分
 *      
 * 5.动作识别
 *      Granny尝试到达掉落的地毯          50分
 *      Granny摔倒                      50分
 *      Granny站起、走远和坐下            50分
 *      
 * 6.对动作的回应
 *      捡起地毯和递送地毯                 40分
 *      抓起、递送手机                     40分
 *      照顾走路                          40分
 *      
 * 7.加分
 *      拨打电话替代递送手机                10分
 *      描述未知的药瓶                     30分
 *      打开药瓶（拧转瓶盖）                40分
 *      从药瓶中得到一颗药品                100分
 *      
 * 8.
 *      未到达                           -50分
 *      出色表现                          25分
 *      
 * 总分：                                 250分
*/

namespace SITRobotSystem_wpf.BLL.Competitions.Robo_Nures_2015
{
    class RoboNures_2015stage : Stage
    {
        private Robo_Nures_2015Task roboNures2015Task;

        public RoboNures_2015stage()
        {
            roboNures2015Task=new Robo_Nures_2015Task();
        }

        public override void init()
        {
            roboNures2015Task.initBodyDetect();
            roboNures2015Task.initSpeech();
            roboNures2015Task.initSurfFrmae();
        }

        public void WaitForDoor()
        {
            roboNures2015Task.WaitForDoor();
        }

        public virtual void Start()
        {
            roboNures2015Task.moveToPlaceByName("startpoint");
        }

        public void ToPeople()
        {
            bool res=false;
            roboNures2015Task.moveToPlaceByName("bed");
            while(!res)
                res=roboNures2015Task.FindPeople();
        }

        public virtual void AskPill()
        {
            roboNures2015Task.speak("what can i do for you");
            roboNures2015Task.AskForPill();
            while (this.roboNures2015Task.baseSpeech.ReturnCommand != "1haha")
            {
            }
            this.roboNures2015Task.baseSpeech.ReturnCommand = "";
            //this.roboNures2015Task.baseSpeech.ReturnCommand = "";

            roboNures2015Task.moveToPlaceByName("shelf");
            roboNures2015Task.speak("i have reached shelf");
        }

        public virtual void FindPill()
        {
            //List<Goods> gs = new List<Goods>();

            ////gs=roboNures2015Task
            ////Goods g = new Goods();
            ////g.Name = "firstpill";

            //while (this.roboNures2015Task.baseSpeech.ReturnCommand != "1haha")
            //{
            //    //roboNures2015Task.SearchObject(g)  
            //}
            //this.roboNures2015Task.baseSpeech.ReturnCommand = "";
            roboNures2015Task.speak("i am finding the pill");
            Goods g = new Goods();
            g = roboNures2015Task.GetGoodsByName("pill");
            roboNures2015Task.SearchObject(g);
            roboNures2015Task.ArmGet();
            roboNures2015Task.speak("i have got the pill");
        }

        public virtual void DeliverPill()
        {
            roboNures2015Task.moveToPlaceByName("bedbefore");
            roboNures2015Task.FindPeople();
            roboNures2015Task.ArmGive();
            roboNures2015Task.speak("Please take the pill");
            roboNures2015Task.speak("it is my pleasure to help you");
        }

        public virtual void ActivityRecognition()
        {
            bool res;
            roboNures2015Task.FindPeople();
        }

        public void end()
        {
            roboNures2015Task.moveToPlaceByName("exit");
        }
    }
}
