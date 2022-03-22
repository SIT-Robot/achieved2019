using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2017
{
    class Restaurant2017Competition : Competition
    {
        Restaurant2017Stage restaurant2017Stage;

        public Restaurant2017Competition()
        {
            restaurant2017Stage = new Restaurant2017Stage();
        }
        public override void init()
        {
            ThreadNameStr = "Restaurant2017Task";
            restaurant2017Stage.init();
        }
        public override void process()
        {
            restaurant2017Stage.Restaurant2017StageStart();
        }
    }
}
