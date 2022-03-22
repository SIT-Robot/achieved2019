using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.DAL
{
    static class DBCMDPlace
    {
        public static string  selectAllPlace()
        {
            string Sqlcmd = "select * from Place";
            return Sqlcmd;
        }
        public static string insertPlace(Place pla)
        {
            string Sqlcmd =
                "insert into Place(Pno,ProomName,PName,PCategory,PpositionX,PpositionY,PpositionZ,POrientationX,POrientationY,POrientationZ,POrientationW) values('" +
                pla.No + "','" + pla.ProomName + "','" + pla.Name + "','" + pla.PCategory + "','" +
                pla.position.X + "','" + pla.position.Y + "','" + pla.position.Z + "','" + pla.oritation.X +
                "','" + pla.oritation.Y + "','" + pla.oritation.Z + "','" + pla.oritation.W + "')";
            return Sqlcmd;
        }
        public static string delPlace(string Pno) 
        {
            string Sqlcmd = "delete from Place where Pno='" + Pno + "'";
            return Sqlcmd;
        }
        public static string getPlace(string Pno) 
        {
            string Sqlcmd = "select * from Place where Pno='" + Pno + "'";
            return Sqlcmd;
        }

        public static string getALLPlaceName()
        {
            string Sqlcmd = "select Pname from Place";
            return Sqlcmd;
        }

        public static string getPlaceByName(string Pname)
        {
            string Sqlcmd = "select * from Place where Pname='" + Pname + "'";
            return Sqlcmd;
        }

        public static string getPlaceByGood(string GName)       //说出物品名自动查询那货的地点详情
        {
            string Sqlcmd = "select * from Place where  Pno=( select Pno from GoodsPlace where Gno=(select Gno from Goods where GName='"+ GName+"'))";
            return Sqlcmd;
        }


        public static string updatePlace(Place pla)
        {
            string Sqlcmd = "update Place set ProomName = '" + pla.ProomName
               + "',PName = '" + pla.Name +
                "',PCategory='" + pla.PCategory +
                "',PpositionX='" + pla.position.X +
                "',PpositionY='" + pla.position.Y +
                "',PpositionZ='" + pla.position.Z +
                "',PorientationX='" + pla.oritation.X +
                "',PorientationY='" + pla.oritation.Y +
                "',PorientationZ='" + pla.oritation.Z +
                "',PorientationW='" + pla.oritation.W +
             "'where Pno='" + pla.No+"'";
            return Sqlcmd;
        }

        public static string updatePlace(char Pno,Place pla)
        {
            string Sqlcmd = "update Place set ProomName = '" + pla.ProomName + "','" + pla.Name + "','" + pla.PCategory +
                            "','" +
                            pla.position.X + "','" + pla.position.Y + "','" + pla.position.Z + "','" + pla.oritation.X +
                            "','" + pla.oritation.Y + "','" + pla.oritation.Z + "," + pla.oritation.W + "'";
            return Sqlcmd;
        }
    }
}
