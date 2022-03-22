using System;
using System.Collections.Generic;

using SITRobotSystem_wpf.entity;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SITRobotSystem_wpf.DAL
{
    public class DBConnection
    {
        string dbconntn;
        SqlConnection myConnection = new SqlConnection();              //db连接对象
        SqlCommand myCommandR;                                         //Sql命令集（select）
        SqlCommand myCommand;  
         public DBConnection(string strlink)
        {
            dbconntn = strlink;
            myConnection.ConnectionString = dbconntn;
        }
          ~DBConnection()
         {
             dbclose();
         }
        public bool dbopen()
        {

            try
            {
                myConnection.Open();    // open database
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect Error:"+ ex);
                return false;
            }
            return true;
        }
        public bool dbclose()
        {
            try
            {
                myConnection.Close();    // close database
            }
            catch (Exception ex)
            {
                Console.WriteLine("Close Error:" + ex);
                return false;
            }
            return true;
        }
        //Sqlcmd (create update insert )
        public bool Sqlcmd(string strSql)           
        {
            try
            {
                myCommand = myConnection.CreateCommand();            //创建Sqlcmd对象
                myCommand.CommandText = "" + strSql + "";            //提供Sqlcmd(string)
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                return false;
            }

            myCommand.Connection = myConnection;
            myCommand.ExecuteNonQuery();                             //执行Sqlcmd(string)
            return true;
        }
       // Sqlcmd (select)
        public List<Place> SqlcmdReadAllPlacePlace()
        {
            List<Place> places=new List<Place>();
            
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDPlace.selectAllPlace() + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader

            while (myReader.Read()) // 读取DataReader内容
            {
                Place p = new Place();
                string pno = myReader[0].ToString();
                string pRoomName = myReader[1].ToString();
                string pName = myReader[2].ToString();
                string pCategory = myReader[3].ToString();
                Point point = new Point(float.Parse(myReader[4].ToString()), float.Parse(myReader[5].ToString()),
                    float.Parse(myReader[6].ToString()));
                Quaternion quaternion = new Quaternion(float.Parse(myReader[7].ToString()),
                    float.Parse(myReader[8].ToString()), float.Parse(myReader[9].ToString()),
                    float.Parse(myReader[10].ToString()));

                Header header=new Header(pName,"map");

                p = new Place(pno, pName, pRoomName, pCategory, quaternion, point,header);
                places.Add(p);
            }
            
            myReader.Close();
            return places;
        }


        public bool SqlCmdInsertPlace(Place pla)
        {
            bool res = true;
            try
            {
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "" + DBCMDPlace.insertPlace(pla) + "";
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            myCommand.Connection = myConnection;
            myCommand.ExecuteNonQuery();                             //执行Sqlcmd(string)

            return res;
        }

        public bool SqlCmdUpdatePlace(Place pla)
        {
            bool res = true;
            try
            {
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "" + DBCMDPlace.updatePlace(pla) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            myCommand.Connection = myConnection;
            myCommand.ExecuteNonQuery();                             //执行Sqlcmd(string)

            return res;
        }

        public Place SqlCmdGetPlace(string Pno)
        {
            Place resPlace = new Place();
            bool res = true;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDPlace.getPlace(Pno) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader

            while (myReader.Read()) // 读取DataReader内容
            {
                string pno = myReader[0].ToString();
                string pRoomName = myReader[1].ToString();
                string pName = myReader[2].ToString();
                string pCategory = myReader[3].ToString();
                Point point = new Point(float.Parse(myReader[4].ToString()), float.Parse(myReader[5].ToString()),
                    float.Parse(myReader[6].ToString()));
                Quaternion quaternion = new Quaternion(float.Parse(myReader[7].ToString()),
                    float.Parse(myReader[8].ToString()), float.Parse(myReader[9].ToString()),
                    float.Parse(myReader[10].ToString()));

                Header header = new Header(pName, "map");

                resPlace = new Place(pno, pName, pRoomName, pCategory, quaternion, point, header);
                
            }
            return resPlace;
        }

        public Place SqlCmdGetPlaceByName(string name)
        {
            Place resPlace = new Place();
            bool res = true;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDPlace.getPlaceByName(name) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader

            while (myReader.Read()) // 读取DataReader内容
            {
                string pno = myReader[0].ToString();
                string pRoomName = myReader[1].ToString();
                string pName = myReader[2].ToString();
                string pCategory = myReader[3].ToString();
                Point point = new Point(float.Parse(myReader[4].ToString()), float.Parse(myReader[5].ToString()),
                    float.Parse(myReader[6].ToString()));
                Quaternion quaternion = new Quaternion(float.Parse(myReader[7].ToString()),
                    float.Parse(myReader[8].ToString()), float.Parse(myReader[9].ToString()),
                    float.Parse(myReader[10].ToString()));

                Header header = new Header(pName, "map");

                resPlace = new Place(pno, pName, pRoomName, pCategory, quaternion, point, header);

            }
            return resPlace;
        }

        public bool SqlCmdUpdateAddress(String address)
        {
            bool res = true;
            try
            {
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "" + DBCMDConfig.updateAddress(address) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            myCommand.Connection = myConnection;
            myCommand.ExecuteNonQuery();                             //执行Sqlcmd(string)

            return res;
        }


        public bool SqlCmdUpdatePort(string port)
        {
            bool res = true;
            try
            {
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "" + DBCMDConfig.updatePort(port) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            myCommand.Connection = myConnection;
            myCommand.ExecuteNonQuery();                             //执行Sqlcmd(string)

            return res;
        }

        public string SqlCmdGetAddress()
        {
            bool res = true;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDConfig.selectHubAddress() + "";
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader
            string resAddress="";
            while (myReader.Read()) // 读取DataReader内容
            {
                resAddress = myReader[0].ToString();

            }
            return resAddress;
        }
        public ushort SqlCmdGetPort()
        {
            bool res = true;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDConfig.selectHubPort() + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader
            ushort resPort = 0;
            while (myReader.Read()) // 读取DataReader内容
            {
                resPort =ushort.Parse(myReader[0].ToString());

            }
            return resPort;
        }


        public Goods SqlCmdgetGoodsByNO(string gno)
        {
            Goods resGoods = new Goods(); ;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDGoods.selectGoodsByGno(gno) + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader


            while (myReader.Read())                                  // 读取DataReader内容
            {
                try
                {
                    string Gno = myReader[0].ToString();
                    string GName = myReader[1].ToString();
                    string GCategory = myReader[2].ToString();
                    resGoods.Name = GName;
                    resGoods.No = Gno;
                    resGoods.Category = GCategory;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read Error:" + ex);
                }
            }
            myReader.Close();
            return resGoods;
        }
        public List<string> SqlCmdgetGoodsImgpathListByGno(string gno)
        {
            List<string> imgPathList = new List<string>(); ;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDGoods.selectGoodsImgPathsByGno(gno) + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader


            while (myReader.Read())                                  // 读取DataReader内容
            {
                try
                {
                    imgPathList.Add(myReader["Gpath"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read Error:" + ex);
                }
            }
            myReader.Close();
            return imgPathList;
        }


        public Goods SqlCmdgetGoodsByName(string gname)
        {
            Goods resGoods = new Goods(); ;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDGoods.selectGoodsByGname(gname) + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader


            while (myReader.Read())                                  // 读取DataReader内容
            {
                try
                {
                    string Gno = myReader[0].ToString();
                    string GName = myReader[1].ToString();
                    string GCategory = myReader[2].ToString();
                    resGoods.Name = GName;
                    resGoods.No = Gno;
                    resGoods.Category = GCategory;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read Error:" + ex);
                }
            }
            myReader.Close();
            return resGoods;
        }

        public List<string> SqlCmdGetAllPlaceNames()
        {
            List<string> PlaceNameList = new List<string>(); ;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDPlace.getALLPlaceName() + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader


            while (myReader.Read())                                  // 读取DataReader内容
            {
                try
                {
                    PlaceNameList.Add(myReader["PName"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read Error:" + ex);
                }
            }
            myReader.Close();
            return PlaceNameList;
        }

        public List<string> SqlCmdGetAllGoodsNames()
        {
            List<string> GoodNameList = new List<string>(); ;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDGoods.getALLGoodsName() + "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
            }

            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader


            while (myReader.Read())                                  // 读取DataReader内容
            {
                try
                {
                    GoodNameList.Add(myReader["GName"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read Error:" + ex);
                }
            }
            myReader.Close();
            return GoodNameList;
        }

        public Place SqlCmdGetPlaceByGood(Goods g)
        {
            string GName = g.Name;
            Place resPlace = new Place();
            bool res = true;
            try
            {
                myCommandR = myConnection.CreateCommand();
                myCommandR.CommandText = "" + DBCMDPlace.getPlaceByGood(GName) + "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlCommand Error:" + ex);
                res = false;
            }
            SqlDataReader myReader = myCommandR.ExecuteReader();    //返回结果指派给DataReader

            while (myReader.Read()) // 读取DataReader内容
            {
                string pno = myReader[0].ToString();
                string pRoomName = myReader[1].ToString();
                string pName = myReader[2].ToString();
                string pCategory = myReader[3].ToString();
                Point point = new Point(float.Parse(myReader[4].ToString()), float.Parse(myReader[5].ToString()),
                    float.Parse(myReader[6].ToString()));
                Quaternion quaternion = new Quaternion(float.Parse(myReader[7].ToString()),
                    float.Parse(myReader[8].ToString()), float.Parse(myReader[9].ToString()),
                    float.Parse(myReader[10].ToString()));

                Header header = new Header(pName, "map");

                resPlace = new Place(pno, pName, pRoomName, pCategory, quaternion, point, header);

            }
            return resPlace;
        }



    }
}
