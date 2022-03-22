using System.Collections.Generic;
using Org.BouncyCastle.Math.EC;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Follow
{
    class FollowCompetition:Competition
    {
        private FollowStage followStage;

        public FollowCompetition()
        {
            followStage = new FollowStage();
        }
        public override void init()
        {
            ThreadNameStr = "Follow";
            followStage.init();
        }

        public override void process()
        {
            User followUser=new User();
            Leg leg=new Leg();
            followStage.ensureUser(ref followUser,ref leg);
            while (true)
            {
                followStage.followTrackingUser(ref followUser,ref leg);
            }
        }
    }
}
