using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;

using SITRobotSystem_wpf.entity;
using System.Configuration;
namespace SITRobotSystem_wpf.DAL
{
    public class DBCtrl
    {
        private DBConnection dbConnection;
        public DBCtrl()
        {
            //这里有问题！链接字符串无法自动获取
            dbConnection = new DBConnection(DBConfig.connectString);
            
        }//
  
        public bool InsertPlace(Place pla)
        {
            bool res = true;
            if (dbConnection.dbopen())
            {
                dbConnection.SqlCmdInsertPlace(pla);
                dbConnection.dbclose();
             }
            else
            {
                res = false;
            }
            return res;
        }


        public Place GetPlaceByID(string Pno)
        {
            Place resPlace =new Place();
            if (dbConnection.dbopen())
            {
                resPlace = dbConnection.SqlCmdGetPlace(Pno);
                dbConnection.dbclose();
            }
            return resPlace;            
        }

        public Place GetPlaceByName(string pname)
        {
            Place resPlace = new Place();
            if (dbConnection.dbopen())
            {
                resPlace = dbConnection.SqlCmdGetPlaceByName(pname);
                dbConnection.dbclose();
            }
            return resPlace;                
        }

        public Place GetPlaceByGood(Goods g)
        {
            
            Place resPlace = new Place();
            if (dbConnection.dbopen())
            {
                resPlace = dbConnection.SqlCmdGetPlaceByGood(g);
                dbConnection.dbclose();
            }
            return resPlace;
        }

        public bool UpdatePlace(Place place)
        {
            bool res=false;
            if (dbConnection.dbopen())
            {
                res = dbConnection.SqlCmdUpdatePlace(place);
                dbConnection.dbclose();
                res = true;
            }
            return res;
        }

        public bool updateHubAddress(string HubAddress)
        {
            bool res = true;
            if (dbConnection.dbopen())
            {
                dbConnection.SqlCmdUpdateAddress(HubAddress);
                dbConnection.dbclose();
            }
            else
            {
                res = false;
            }
            return res;
        }

        public bool updateHubPort(ushort port)
        {
            bool res = true;
            if (dbConnection.dbopen())
            {
                dbConnection.SqlCmdUpdatePort(port.ToString());
                dbConnection.dbclose();
            }
            else
            {
                res = false;
            }
            return res;
        }
        public string GetHubAddress()
        {
            string resStr="";
            if (dbConnection.dbopen())
            {
                resStr = dbConnection.SqlCmdGetAddress();
                dbConnection.dbclose();
            }
            return resStr;
        }
        public ushort GetHubPort()
        {
            ushort resPort = 0;
            if (dbConnection.dbopen())
            {
                resPort = dbConnection.SqlCmdGetPort();
                dbConnection.dbclose();
            }
            return resPort;
        }

        /// <summary>
        /// 从数据库中取出指定物体的对象
        /// </summary>
        /// <param name="gname">物体名</param>
        /// <returns></returns>
        public Goods GetGoodsByNo(string gno)
        {
            Goods resGoods = new Goods();
            if (dbConnection.dbopen())
            {
                resGoods = dbConnection.SqlCmdgetGoodsByNO(gno);
                resGoods.imgPath = dbConnection.SqlCmdgetGoodsImgpathListByGno(resGoods.No);
                dbConnection.dbclose();
            }
            return resGoods;  
        }


        public Goods GetGoodsByName(string gname)
        {
            Goods resGoods = new Goods();
            if (dbConnection.dbopen())
            {
                resGoods = dbConnection.SqlCmdgetGoodsByName(gname);
                resGoods.imgPath = dbConnection.SqlCmdgetGoodsImgpathListByGno(resGoods.No);
                dbConnection.dbclose();
            }
            return resGoods;
        }

        public string[] GetAllGoodsName()
        {
            List<string> resGoodsNameList=new List<string>();
            string[] resNames;
            Goods resGoods = new Goods();
            if (dbConnection.dbopen())
            {
                resGoodsNameList = dbConnection.SqlCmdGetAllGoodsNames();
                dbConnection.dbclose();
            }
            resNames = resGoodsNameList.ToArray();
            return resNames;           
        }
        public string[] GetAllPlaceName()
        {
            List<string> rePlaceNameList = new List<string>();
            string[] resNames;
            Goods resGoods = new Goods();
            if (dbConnection.dbopen())
            {
                rePlaceNameList = dbConnection.SqlCmdGetAllPlaceNames();
                dbConnection.dbclose();
            }
            resNames = rePlaceNameList.ToArray();
            return resNames;
        }

        
        

    }
}
