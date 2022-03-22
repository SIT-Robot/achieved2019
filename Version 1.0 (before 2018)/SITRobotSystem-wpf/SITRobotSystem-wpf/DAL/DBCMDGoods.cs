using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.DAL
{
    public  static class  DBCMDGoods
    {
        /// <summary>
        /// 插入一个物品
        /// </summary>
        /// <param name="goods">物品</param>
        /// <returns></returns>
        public static string insertGoods(Goods goods)
        {
            string Sqlcmd = "insert into Goods values('" + goods.No + "','" + goods.Name + "','" + goods.Category +
                            "'";
            return Sqlcmd;
        }



        /// <summary>
        /// 取出所有物品
        /// </summary>
        /// <returns></returns>
        public static string selectAllGoods()
        {
            string Sqlcmd = "select * from Goods";
            return Sqlcmd;
        }

        /// <summary>
        /// 更新物体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string updateGoods(Goods goods)
        {
            string Sqlcmd = "update Goods set Gno = '" + goods.No + "',GName = '" + goods.Name + "',GCategory = '" + goods.Category +
                "' where Gno='" + goods.No + "'";
            return Sqlcmd;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Gno"></param>
        /// <returns></returns>
        public static string delGoods(string Gno)
        {
            string Sqlcmd = "delete from Goods where Gno='"+Gno+"' ";
            return Sqlcmd;
        }
        /// <summary>
        /// 取出指定物体
        /// </summary>
        /// <param name="Pno"></param>
        /// <returns></returns>
        public static string selectGoodsByGno(string Gno)
        {
            string Sqlcmd = "select * from Goods where Gno='" + Gno + "'";
            return Sqlcmd;
        }

        public static string selectGoodsByGname(string Gname)
        {
            string Sqlcmd = "select * from Goods where Gname='" + Gname + "'";
            return Sqlcmd;            
        }
        /// <summary>
        /// 增加一个物体的图片路径
        /// </summary>
        /// <param name="Gno"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string InsertGoodsImgPath(string Ino,string Gno, string Path)
        {
            string Sqlcmd = "insert into GImgPath values('" +Ino+ "'" +Gno+"'"+Path+"')" ;
            return Sqlcmd;

        }

        /// <summary>
        /// 通过Gno提取物体路径
        /// </summary>
        /// <param name="Gno"></param>
        /// <returns></returns>
        public static string selectGoodsImgPathsByGno(string Gno)
        {
            string Sqlcmd = "select * from GImgPath where Gno = '" + Gno + "'";
            return Sqlcmd;
        }
        /// <summary>
        /// 通过名字提取物体路径
        /// </summary>
        /// <param name="GName"></param>
        /// <returns></returns>
        public static string selectGoodsImgPathsByGname(string GName)
        {
            string Sqlcmd = "select Gpath from GImgPath where Gno =(select Gno from Goods where GName='" + GName + ",)";
            return Sqlcmd;
        }

        /// <summary>
        /// 通过路径名删除路径
        /// </summary>
        /// <param name="GImgPathName"></param>
        /// <returns></returns>
        public static string deleteGoodsImgPathsByImgPath(string GImgPath)
        {
            string Sqlcmd = "delete from GImgPath where Gpath='"+GImgPath+"'";
            return Sqlcmd;
        }

        /// <summary>
        /// 通过物体的no删除该物体所有的路径名
        /// </summary>
        /// <param name="Gno"></param>
        /// <returns></returns>
        public static string deleteALLGoodsImgPathsByGno(string Gno)
        {
            string Sqlcmd = "delete from GImgPath where Gno='"+Gno+"'";
            return Sqlcmd;
        }
        /// <summary>
        /// 通过物体的Name删除该物体所有的路径名
        /// </summary>
        /// <param name="GImgPathName"></param>
        /// <returns></returns>
        public static string deleteALLGoodsImgPathsByGname(string GImgPathName)
        {
            string Sqlcmd = "delete from GImgPath where Gno=(select Gno from Goods where GName='" +GImgPathName+ ",)";
            return Sqlcmd;
        }


        public static string getALLGoodsName()
        {
            string Sqlcmd = "select GName from Goods";
            return Sqlcmd;
        }

       

    }
}
