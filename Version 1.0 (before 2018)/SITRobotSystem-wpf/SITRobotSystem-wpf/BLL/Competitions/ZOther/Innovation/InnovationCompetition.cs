using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Innovation
{
    class InnovationCompetition:Competition
    {

        public InnovationStages IStages;

        public InnovationCompetition()
        {
            IStages = new InnovationStages();
        }

        public override void init()
        {
            IStages.init();
        }

        public override void process()
        {
            //IStages.findpeople();
            IStages.StartCare();
            Thread.Sleep(5000);
            IStages.trackingUser();
            
            
        }
    }
}
