using SITRobotSystem_wpf.DAL;

namespace SITRobotSystem_wpf.BLL.State
{
    public static class SitRobotConfig
    {

        private static string HubAddress = "192.168.1.111";
        private static ushort HubPort = 2150;

        public static void setHubAddress(string Address)
        {
            DBCtrl dbCtrl=new DBCtrl();
            HubAddress = Address;
            dbCtrl.updateHubAddress(Address);

        }

        public static void setHubPort(ushort Port)
        {
            DBCtrl dbCtrl = new DBCtrl();
            HubPort = Port;
            dbCtrl.updateHubPort(Port);

        }
        public static string getHubAddress()
        {
            DBCtrl dbCtrl = new DBCtrl();
            return dbCtrl.GetHubAddress();
        }

        public static ushort getHubPort()
        {
            DBCtrl dbCtrl = new DBCtrl();
            return dbCtrl.GetHubPort();
        }
    }

  

}
