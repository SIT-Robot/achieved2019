using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;
using System.Threading;
using System.Drawing;

namespace SITRobotSystem_wpf.BLL.Competitions.SetTable2017
{
    class SetTable2017Task : Tasks.Tasks
    {
        string commandStrs;
        public Rectangle rect;
        public SetTable2017Task()
        {
            rect = new Rectangle(300, 500, 1300, 400);
        }
        public string AskforMile()
        {
            //人对机器说的话，可以是多个选择
            string[] commands= { "bread","cookies" };

            baseSpeech = new SitRobotSpeech();
            baseSpeech.robotSpeak("whether you prefer bread or cookies");
            commandStrs = "";
            baseSpeech.SetTable2017Recognize(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
            }
            baseSpeech.robotSpeak("I will take the " + commandStrs +　" later");
            return commandStrs;
        }

        public string waitForClean()
        {
            //人对机器说的话，可以是多个选择
            string[] commands = { "clean the table" };

            baseSpeech = new SitRobotSpeech();
            baseSpeech.robotSpeak("I am waitting for cleaning table");
            commandStrs = "";
            baseSpeech.SetTable2017Recognize(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
            }
            baseSpeech.robotSpeak("OK! I will clean it");
            return commandStrs;
        }

        public virtual void ArmGet()
        {
            ArmAction armAction0 = new ArmAction(300, "get");
            armCtrl.getThing(armAction0);
            baseCtrl.moveToDirectionSpeed(-0.3f, 0f);
        }

        public virtual void ArmPlace(int id = 301)
        {
            ArmAction armAction1 = new ArmAction(id, "put");
            armCtrl.putThing(armAction1);
            baseCtrl.moveToDirectionSpeed(-0.1f, 0f);
            ArmAction armAction2 = new ArmAction(1000, "put");
            armCtrl.putThing(armAction2);
        }

        public void moveToAdjust(List<SurfResult> SearchsurfResultList, List<int> SearchObjectID,int NowObjectID)
        {
            baseCtrl.moveToDirectionSpeed(-0.3f, 0f);
        }

        public void SpeakFindObject(string objName)
        {
            this.speak("I will go to fook for" + objName);
            Thread.Sleep(1000);
        }
    }
}
