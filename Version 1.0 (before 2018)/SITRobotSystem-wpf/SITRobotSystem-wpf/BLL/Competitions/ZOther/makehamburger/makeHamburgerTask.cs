using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.makehamburger
{
    public class makeHamburgerTask : Tasks.Tasks
    {
        public void HandCatch()
        {
            ArmAction armAction0 = new ArmAction(111, "catch");//1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void HandTurn()
        {
            ArmAction armAction0 = new ArmAction(222, "turn");//1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void HandDown()
        {
            ArmAction armAction0 = new ArmAction(333, "down");//1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void SelectHumburger()
        {
            this.baseSpeech = new SitRobotSpeech();
            this.baseSpeech.robotSpeak("Which hamburger do you need?");
            this.baseSpeech.hamburgerRecognize();
        }
    }
}
