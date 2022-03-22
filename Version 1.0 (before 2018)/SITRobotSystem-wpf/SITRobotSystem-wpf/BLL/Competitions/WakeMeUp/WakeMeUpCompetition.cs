using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.WakeMeUp
{
    class WakeMeUpCompetition:Competition
    {
        public WakeMeUpStage wakemeupStage;

        public WakeMeUpCompetition()
        {
            wakemeupStage = new WakeMeUpStage();
        }
        public override void init()
        {
            ThreadNameStr = "WakeMeUp";
            wakemeupStage.init();
        }
        public override void process()
        {
            this.wakemeupStage.AwakeHuman();
            this.wakemeupStage.TakeOrder();
            this.wakemeupStage.ServeBreakfast();
            //this.wakemeupStage.DeliverBreakfast();
        }
    }
}
