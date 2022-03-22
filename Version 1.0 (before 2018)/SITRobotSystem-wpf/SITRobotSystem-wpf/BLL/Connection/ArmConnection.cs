using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Connection
{
    class ArmConnection
    {

        private SitRobotHub sitRobotHub;

        public ArmConnection()
        {
            sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
        }
        /// <summary>
        /// 传输手臂字符串
        /// </summary>
        /// <param name="armStr"></param>
        /// <returns></returns>
        public bool sendArmAction(ArmAction action)
        {

            sitRobotHub.HandAction(action.ID);
            return true;
        }
    }
}