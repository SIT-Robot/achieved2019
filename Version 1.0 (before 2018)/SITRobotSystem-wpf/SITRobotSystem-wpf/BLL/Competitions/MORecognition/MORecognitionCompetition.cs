using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math.EC;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.MORecognition
{
    class MORecognitionCompetition : Competition
    {
        private MORecognitionStage morecognitionStage;
        public bool IsNeedRestart=false;
        public bool IsNeedStop = false;
        public MORecognitionCompetition()
        {
            morecognitionStage=new MORecognitionStage();
        }
        public override void init()
        {
            ThreadNameStr = "MORecognition";
            morecognitionStage.init();
        }
        public override void process()
        {
            morecognitionStage.MOStageStart();
        }
    }
}
