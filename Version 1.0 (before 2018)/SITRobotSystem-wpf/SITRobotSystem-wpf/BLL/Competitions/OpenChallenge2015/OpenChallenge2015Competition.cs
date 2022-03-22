using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.OpenChallenge2015
{
    class OpenChallenge2015Competition : Competition
    {
        public OpenChallenge2015Stage ocstage;

        public OpenChallenge2015Competition()
        {
            ocstage = new OpenChallenge2015Stage();
        }

        public override void init()
        {
            this.ThreadNameStr = "OpenChallenge";
          ocstage.init();
        }

        public override void process()
        {
            ocstage.Start();
            ocstage.findPerson();
            ocstage.askCommandAndAction();
        }
    }
}
