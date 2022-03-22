using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i_Shit_Core.Core.Drivers;


namespace i_Shit_Core.Core.Functions
{
    //ROSLocation数据库操作
    public static partial class Function
    {
        /// <summary>
        /// 得到一个相对当前位置的LocationInfo，用于导航。
        /// </summary>
        /// <param name="X">前后相对位置。前正，后负</param>
        /// <param name="Y">左右相对位置。左正，右负</param>
        /// <param name="Angle">自转角度。逆时针（往左）正，顺时针（往右）负</param>
        /// <returns></returns>
        public static LocationInfo Location_GetRelativeLocationInfo(float X, float Y, float Angle, LocationInfo sourceLocation)
        {
            LocationInfo li = new LocationInfo();
            li._locationName = "RelativeLocation";
            li._positionX = sourceLocation._positionX;
            li._positionY = sourceLocation._positionY;
            li._positionZ = sourceLocation._positionZ;
            double fCosHRoll = Math.Cos(0 * .5f);
            double fSinHRoll = Math.Sin(0 * .5f);
            double fCosHPitch = Math.Cos(0 * .5f);
            double fSinHPitch = Math.Sin(0 * .5f);
            double fCosHYaw = Math.Cos(Angle * .5f);
            double fSinHYaw = Math.Sin(Angle * .5f);

            li._orientationW = fCosHRoll * fCosHPitch * fCosHYaw + fSinHRoll * fSinHPitch * fSinHYaw;
            li._orientationX = fCosHRoll * fSinHPitch * fCosHYaw + fSinHRoll * fCosHPitch * fSinHYaw;
            li._orientationY = fCosHRoll * fCosHPitch * fSinHYaw - fSinHRoll * fSinHPitch * fCosHYaw;
            li._orientationZ = fSinHRoll * fCosHPitch * fCosHYaw - fCosHRoll * fSinHPitch * fSinHYaw;
            return li;
        }

        /// <summary>
        /// 在数据库中获得一个位置的信息；
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static LocationInfo Location_GetLocationInfoByName(string name)
        {
            SQLiteConnection sc = Driver.SQLite_OpenDB("RosLocationDB");

            DataSet resultDataSet = Driver.SQLite_Query(sc, "Location", new string[] { "name" }, new string[] { name });
            LocationInfo resultLocationInfo = new LocationInfo();
            resultLocationInfo._positionX = Double.Parse(resultDataSet.Tables[0].Rows[0]["x"].ToString());
            resultLocationInfo._positionY = Double.Parse(resultDataSet.Tables[0].Rows[0]["y"].ToString());
            resultLocationInfo._positionZ = Double.Parse(resultDataSet.Tables[0].Rows[0]["z"].ToString());
            resultLocationInfo._orientationX = Double.Parse(resultDataSet.Tables[0].Rows[0]["ox"].ToString());
            resultLocationInfo._orientationY = Double.Parse(resultDataSet.Tables[0].Rows[0]["oy"].ToString());
            resultLocationInfo._orientationZ = Double.Parse(resultDataSet.Tables[0].Rows[0]["oz"].ToString());
            resultLocationInfo._orientationW = Double.Parse(resultDataSet.Tables[0].Rows[0]["ow"].ToString());
            sc.Close();
            return resultLocationInfo;
        }
        public static void Location_DeleteLocationInfoByName(string name)
        {
            SQLiteConnection sc = Driver.SQLite_OpenDB("RosLocationDB");
            Driver.SQLite_Delete(sc, "Location", new string[] { "name" }, new string[] { name });
            sc.Close();
        }
        public static void Location_UpdateLocationInfo(LocationInfo li)
        {
            SQLiteConnection sc = Driver.SQLite_OpenDB("RosLocationDB");
            bool isExisted = false;
            try
            {
                isExisted = Driver.SQLite_Query(sc, "Location", new string[] { "name" }, new string[] { li._locationName }).Tables[0].Rows.Count != 0;
            }
            catch { }
            if (isExisted == false)
            {//没有这条name，就insert

                Driver.SQLite_Insert(sc, "Location",
                    new string[] { "name", "x", "y", "z", "ox", "oy", "oz", "ow" },
                    new string[] { li._locationName, li._positionX.ToString(), li._positionY.ToString(), li._positionZ.ToString(), li._orientationX.ToString(), li._orientationY.ToString(), li._orientationZ.ToString(), li._orientationW.ToString() }
                    );
            }
            else
            {//有了这条name，就update
                Driver.SQLite_Update(sc, "Location",
                    new string[] { "name", "x", "y", "z", "ox", "oy", "oz", "ow" },
                    new string[] { li._locationName, li._positionX.ToString(), li._positionY.ToString(), li._positionZ.ToString(), li._orientationX.ToString(), li._orientationY.ToString(), li._orientationZ.ToString(), li._orientationW.ToString() },
                    new string[] { "name" },
                    new string[] { li._locationName }
                    );
            }
            sc.Close();
        }

        public static LocationInfo Location_GetCurrectLocationFromRos()
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {
                LocationInfo LI2 = new LocationInfo();
                LI2._positionX = 10f;
                LI2._positionY = 11f;
                LI2._positionZ = 0;
                LI2._orientationX = 0;
                LI2._orientationY = 0;
                LI2._orientationZ = 0;
                LI2._orientationW = 0;
                return LI2;
            }
            string receivedData = Driver.Ros_Send("#getLocation#");
            string[] splitedData = receivedData.Split('@');
            LocationInfo LI = new LocationInfo();
            LI._positionX = Double.Parse(splitedData[3]);
            LI._positionY = Double.Parse(splitedData[4]);
            LI._positionZ = Double.Parse(splitedData[5]);
            LI._orientationX = Double.Parse(splitedData[6]);
            LI._orientationY = Double.Parse(splitedData[7]);
            LI._orientationZ = Double.Parse(splitedData[8]);
            LI._orientationW = Double.Parse(splitedData[9]);
            return LI;
        }
        //--ROSLocation数据库操作
    }
}
