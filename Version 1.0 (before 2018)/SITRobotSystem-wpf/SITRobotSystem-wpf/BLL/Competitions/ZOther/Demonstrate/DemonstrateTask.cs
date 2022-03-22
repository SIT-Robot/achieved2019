using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Demonstrate
{

    class DemonstrateTask:Tasks.Tasks
    {
        public void AskPeople()
        {

            this.baseSpeech.showSpeechRecognize();
        }

        public override void ArmPut()
        {
            ArmAction armAction1 = new ArmAction(51, "put");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);
           
        }

        public void ArmPour()
        {
            ArmAction armAction1 = new ArmAction(52, "pour");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);

        }

        
    }
}
