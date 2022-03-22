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
            VisionHub Ocr = new VisionHub("127.0.0.1", 4500);
            string OcrRes=Ocr.OCRResult();
            return OcrRes;
        }
        public void Ocrspeech(string speechcmd)
        {

        }
    }
}
