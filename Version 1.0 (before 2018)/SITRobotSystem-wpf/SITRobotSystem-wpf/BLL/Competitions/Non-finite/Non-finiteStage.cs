using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
namespace SITRobotSystem_wpf.BLL.Competitions.Non_finite
{
    class Non_finiteStage:Stage
    {
        private Non_finiteTask non_finiteTask;
        public Non_finiteStage()
        {
            non_finiteTask = new Non_finiteTask();
        }
        public override void init()
        {
            non_finiteTask.initSpeech();
        }
        public void Ocrspeech(string cmd)
        {
            non_finiteTask.speak(non_finiteTask.OcrRead());
            
        }
    }
}
