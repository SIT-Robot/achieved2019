using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.makehamburger
{
    class makehamburgerCompetition: Competition
    {
        makeHamburgerStage Hamburger = new makeHamburgerStage();
        public override void init()
        {
            ThreadNameStr = "makeHamburger";
            Hamburger.init();
        }

        public override void process()
        {
            Hamburger.MakingHamburger();
        }
    }
}
