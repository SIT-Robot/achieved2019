using SITRobotSystem_wpf.BLL.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Non_finite
{
    class Non_finiteTask:Tasks.Tasks
    {
        public Non_finiteTask()
        {
            
           
        }
        public string OcrRead()
        {
            string s=visionCtrl.GetOcrWords();

            return s;
        }
        public void Ocrspeech(string speechcmd)
        {

        }

        public void getCommand()
        {
            baseSpeech.readerRecognize();
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            // return res;
        }

        public void initOCR()
        {
            visionCtrl.readyVisionHub();
        }
    }
}
