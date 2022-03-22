using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.Navigation_Test
{
    class NavigationTestFollow:FollowStage
    {
        public NavigationTestFollow()
        {

        }

        /// <summary>
        /// 双手张开往前推
        /// </summary>
        public virtual void BothHandPush()
        {
            this.followTask.SpeedStop();
            this.EndFollow();
        }
    }
}
