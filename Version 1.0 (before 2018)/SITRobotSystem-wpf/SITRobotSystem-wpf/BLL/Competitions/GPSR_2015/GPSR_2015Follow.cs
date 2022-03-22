using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2015
{
    class GPSR_2015Follow:FollowStage
    {
        public GPSR_2015Follow()
        {
        }

        public void GPSR_Follow()
        {
            User followuser=new User();
            Leg leg=new Leg();
            ensureUser(ref followuser, ref leg);
            followTrackingUser(ref followuser, ref leg);
            //Console.WriteLine("456");
        }
    }
}
