using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Navigation_Test
{
    class NavigationTestcompetition : Competition
    {

        private NavigationTestStages navigationTestStages;
        private NavigationTestFollow navigationTestFollow;

        public NavigationTestcompetition()
        {
            navigationTestStages=new NavigationTestStages();
            navigationTestFollow = new NavigationTestFollow();
        }

        public override void init()
        {
            this.ThreadNameStr = "NavigationTest";
            navigationTestStages.init();
        }

        public override void process()
        {
            navigationTestStages.WaitForDoor();
            navigationTestStages.Point1NoDoor();
            navigationTestStages.ToPoint2();
            navigationTestStages.ToPoint3();

            #region//follow
            User followuser = new User();
            Leg leg = new Leg();
            navigationTestFollow.ensureUser(ref followuser, ref leg);
            navigationTestFollow.followTrackingUser(ref followuser, ref leg);
            #endregion

            navigationTestStages.ToPoint4();
            navigationTestStages.Exit();
        }
    }

}
