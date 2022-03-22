using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Connection
{
    class BaseConnection
    {
        private SitRobotHub sitRobotHub;

        public BaseConnection()
        {
            sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
        }


        public Place getPosition()
        {
            PoseStamped poseStamped=sitRobotHub.GetCurrPosition();
            Place resPlace = Place.change(poseStamped);
            return resPlace;
        }

        /// <summary>
        /// 传送至某一地点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool sendMoveGoal(Place p)
        {
            
            PoseStamped poseStamped = new PoseStamped();
            poseStamped.oritation.X = p.oritation.X;
            poseStamped.oritation.Y = p.oritation.Y;
            poseStamped.oritation.Z = p.oritation.Z;
            poseStamped.oritation.W = p.oritation.W;
            poseStamped.header.FrameId =p.header.FrameId;
            poseStamped.header.PlaceName = p.header.PlaceName;
            poseStamped.position.X = p.position.X;
            poseStamped.position.Y = p.position.Y;
            poseStamped.position.Z = p.position.Z;
            int res=sitRobotHub.Movetogoal(poseStamped);
            return res==1;
        }

        /// <summary>
        /// 传送移动至相对目标位置
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool sendMoveDirection(Place p)
        {
            //跑点5秒
            PoseStamped poseStamped = new PoseStamped();
            poseStamped.oritation.X = p.oritation.X;
            poseStamped.oritation.Y = p.oritation.Y;
            poseStamped.oritation.Z = p.oritation.Z;
            poseStamped.oritation.W = p.oritation.W;
            poseStamped.header.FrameId ="base_link";
            poseStamped.header.PlaceName = p.header.PlaceName;
            poseStamped.position.X = p.position.X;
            poseStamped.position.Y = p.position.Y;
            poseStamped.position.Z = p.position.Z;
            sitRobotHub.Movetogoal(poseStamped);
            return true;
        }
    }
}
