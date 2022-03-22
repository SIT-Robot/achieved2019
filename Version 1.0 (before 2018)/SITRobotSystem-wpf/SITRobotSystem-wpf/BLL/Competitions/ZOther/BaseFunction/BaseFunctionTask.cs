using System;
using System.Collections.Generic;
using System.Threading;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.BaseFunction
{
    public class BaseFunctionTask : Tasks.Tasks
    {

        public void BaseQuestion()
        {
            baseSpeech.baseRecognize();
        }

        public string speechGetCommand()
        {
            baseSpeech.gpsrRecogize();
            string res="";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            return res;
        }
        public string introduce()
        {
            baseSpeech.gpsrRecogize();
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            return res;
        }



    }
}
