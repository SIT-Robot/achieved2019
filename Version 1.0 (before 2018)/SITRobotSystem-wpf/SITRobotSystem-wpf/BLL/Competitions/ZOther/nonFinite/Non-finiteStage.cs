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
            non_finiteTask.initBodyDetect();
            non_finiteTask.initOCR();

        }
        public void Ocrspeech(string cmd)
        {
            User u;
            while (true)
            {
                List<User> users = non_finiteTask.getAllUser();
                u=non_finiteTask.findUserMiddleRasingHand(users);
                if (u.ID!=null)
                {
                    break;
                }
            }
            non_finiteTask.getCommand();
            string ocrStr = non_finiteTask.OcrRead();
            non_finiteTask.speak(ocrStr);
            //确定语音
            non_finiteTask.moveToPlaceByName("living room");
            non_finiteTask.speak("here is your command:");
            non_finiteTask.speak(ocrStr);
        }
    }
}
