using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Zoo
{
    class ZooTask:Tasks.Tasks
    {
        public void say(string word)
        {
            this.baseSpeech.robotSpeak(word);
        }

        public override void ArmGet()
        {
            ArmAction armAction0 = new ArmAction(888, "stage");
            armCtrl.getThing(armAction0);
        }

    }
}
