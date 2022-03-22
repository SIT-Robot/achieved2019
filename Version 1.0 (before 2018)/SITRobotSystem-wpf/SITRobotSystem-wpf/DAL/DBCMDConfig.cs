namespace SITRobotSystem_wpf.DAL
{
    public class DBCMDConfig
    {
        public static string updateAddress(string address)
        {
            string Sqlcmd = "update Config set CHubAddress = '" + address + "' where CNo='1'";
            return Sqlcmd;
        }

        public static string updatePort(string port)
        {
            string Sqlcmd = "update Config set CHubPort = '" + port + "' where CNo='1'";
            return Sqlcmd;
        }

        public static string selectHubAddress()
        {
            string Sqlcmd = "select CHubAddress from Config where CNo ='1'";
            return Sqlcmd;
        }

        public static string selectHubPort()
        {
            string Sqlcmd = "select CHubPort from Config where CNo ='1'";
            return Sqlcmd;
        }
    }
}
