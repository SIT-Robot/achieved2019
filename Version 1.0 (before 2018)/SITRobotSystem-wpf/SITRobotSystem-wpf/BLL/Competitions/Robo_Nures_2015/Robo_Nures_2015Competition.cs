using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Robo_Nures_2015
{
    class Robo_Nures_2015Competition : Competition
    {

        private RoboNures_2015stage roboNures_2015stage;

        public Robo_Nures_2015Competition()
        {
            roboNures_2015stage = new RoboNures_2015stage();
        }

        public override void init()
        {
            this.ThreadNameStr = "RoboNurses_2015";
            roboNures_2015stage.init();
        }

        public override void process()
        {
            //roboNures_2015stage.WaitForDoor();
            //roboNures_2015stage.Start();
            roboNures_2015stage.ToPeople();

            roboNures_2015stage.AskPill();
            roboNures_2015stage.FindPill();
            roboNures_2015stage.DeliverPill();

            roboNures_2015stage.ActivityRecognition();
        }
    }
}
