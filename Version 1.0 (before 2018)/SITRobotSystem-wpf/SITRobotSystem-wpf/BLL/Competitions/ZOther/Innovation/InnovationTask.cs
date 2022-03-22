using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Innovation
{
    class InnovationTask:Tasks.Tasks
    {
        public string CareForCommand()
        {
            baseSpeech.readerRecognize();
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }


            return res;
        }
    }
}
