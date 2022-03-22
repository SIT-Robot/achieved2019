using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.MORecognition
{
    /*  Stage流程
     *  1.到达书架点
     *  2.识别已知物品，按strThing中存储顺序优先识别
     *  3.根据识别结果抓取
     *  4.抓住一段时间，handtime=10秒
     *  5.原地放下（暂）
     *  6.识别成功次数未达到5次时，重复2-6
     */

    /* 分数标准
     * 抓取目标                 5*10
     * 放置目标                 5*10
     * 正确识别目标             5*10
     * 错误识别                 5*-5
     * 隐藏物品                  50
     * Not attending            -50
     * 杰出表现                  20
     * 除特殊奖惩外           共计200
     */
    class MORecognitionStage : Stage
    {
        public const float MaxDistance = 10;
        private MORecognitionTask morecognitionTask;
        private int MOStageOKCount;
        private int NowObjectCount;
        private bool IsReachBookcase;
        private bool IsSearchObject;
        private bool IsGraspObject;
        private bool IsPlaceObject;
        private bool IsHandObject;

        private int UnReachBookcaseCount;
        private int UnSearchObjectCount;
        private int UnGraspObjectCount;
        private int UnPlaceObjectCount;
        private int UnHandObjectCount;
        int handtime=10000;
        string strplace;

        /*物品名称，按数组中的先后顺序抓取*/
        //string[] strThing = new[] { "oreo","palm milk","potato chips","rejoice","soap","tomato sauce",
        //    "toothpaste","want milk","coconut juice",
        //    "cookie","fruit milk","gum","juice milk","milk tea","oatmeal" };
        string[] strThing = new[] { "green bottle","purple bottle","milk tea", "my bottle", "long bottle" };
        Goods obj;
        SurfResult surfres;
        public bool IsStageReady;
        public MORecognitionStage()
        {
            morecognitionTask = new MORecognitionTask();
            MOStageOKCount=0;
            NowObjectCount = 0;
            UnReachBookcaseCount =0;
            UnSearchObjectCount = 0;
            UnGraspObjectCount = 0;
            UnPlaceObjectCount = 0;
            UnHandObjectCount = 0;
            IsStageReady = false;
            IsReachBookcase=false;
            IsSearchObject=false;
            IsGraspObject=false;
            IsPlaceObject=false;
            IsHandObject=false;
        }
        bool Ready()
        {
            if (morecognitionTask.Ready())
            {
                morecognitionTask = new MORecognitionTask();
                obj = new Goods();
                MOStageOKCount = 0;
                UnReachBookcaseCount = 0;
                UnSearchObjectCount = 0;
                UnGraspObjectCount = 0;
                UnPlaceObjectCount = 0;
                UnHandObjectCount = 0;
                IsStageReady = false;
                IsReachBookcase = false;
                IsSearchObject = false;
                IsGraspObject = false;
                IsPlaceObject = false;
                IsHandObject = false;

                IsStageReady = true;
                return true;
            }
            return false;
        }
        List<string> sPath = new List<string>();
        List<string> sName = new List<string>();
        public bool MOStageStart()
        {
            while (MOStageOKCount<5 && NowObjectCount < 4 )
            {
                NowObjectCount = UnSearchObjectCount + MOStageOKCount;
                obj = new Goods();
                obj = this.morecognitionTask.GetGoodsByName(strThing[NowObjectCount]);
                //1 reach bookcase
                //  morecognitionTask.ReachBookcase("bookcase");

                //this.morecognitionTask.PDFMaker();
                if (morecognitionTask.FaceToGoods2(obj))
                //if (morecognitionTask.SearchObject(obj))
                {
                    //sPath.Add(this.morecognitionTask.getCorrentImg());
                    //sName.Add(obj.Name);
                    //3 get object from surf result
                    morecognitionTask.GraspObject(surfres);
                    morecognitionTask.moveDirectionBySpeed(-0.2f, 0);
                    morecognitionTask.moveDirectionBySpeedW(180);
                    morecognitionTask.moveDirectionBySpeed(1f, 0);
                    if(NowObjectCount % 2 == 0)
                        morecognitionTask.PlaceObject();
                    else
                        morecognitionTask.PlaceObject2();
                    morecognitionTask.moveDirectionBySpeedW(180);
                    morecognitionTask.moveDirectionBySpeed(1.1f, 0);
                    MOStageOKCount++;
                }
                else
                {
                   UnSearchObjectCount++;
                }
            }
          //  this.morecognitionTask.PdfTest(sPath.ToArray(), sName.ToArray());
          //  this.morecognitionTask.speak("I found");
            Thread.Sleep(1000);
            foreach (var sn in sName)
            {
                this.morecognitionTask.speak(sn);
                Thread.Sleep(800);
            }
            return true;
        }
        bool MOStageReStart()
        {
            morecognitionTask = new MORecognitionTask();
            MOStageOKCount = 0;
            UnReachBookcaseCount = 0;
            UnSearchObjectCount = 0;
            UnGraspObjectCount = 0;
            UnPlaceObjectCount = 0;
            UnHandObjectCount = 0;
            IsStageReady = false;
            IsReachBookcase = false;
            IsSearchObject = false;
            IsGraspObject = false;
            IsPlaceObject = false;
            IsHandObject = false;
            return true;
        }
        
        
        bool UnSearchObject()
        {
            for (float i = 0; i < MaxDistance; i+=0.5f)
            {
                float X = (float)this.morecognitionTask.GetPlace().position.X;
                float Y = (float) this.morecognitionTask.GetPlace().position.Y - i;
                this.morecognitionTask.moveToDirection(X, Y, 0);
                if (morecognitionTask.SearchObject(obj))
                    return true;
            }
            for (float i = 0; i < MaxDistance; i += 0.5f)
            {
                float X = (float)this.morecognitionTask.GetPlace().position.X;
                float Y = (float)this.morecognitionTask.GetPlace().position.Y + i;
                this.morecognitionTask.moveToDirection(X, Y, 0);
                if (morecognitionTask.SearchObject(obj))
                    return true;
            }
            return false;
        }
        bool UnGraspObject()
        {
            
            return false;
        }
        bool UnPlaceObject()
        {
            
            return false;
        }
        bool UnHandObject()
        {
            return false;
        }
        public override void init()
        {
            morecognitionTask.initSpeech();
            morecognitionTask.initSurfFrmae();
        }
    }
}
