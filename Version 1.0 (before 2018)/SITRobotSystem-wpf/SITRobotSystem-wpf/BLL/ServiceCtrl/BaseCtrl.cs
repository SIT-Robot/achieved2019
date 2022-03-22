using System;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.ServiceCtrl
{
    /// <summary>
    /// 底盘移动控制
    /// </summary>
    public class BaseCtrl
    {
        /// <summary>
        /// 移动至某一地点
        /// </summary>
        /// <param name="p">地点</param>
        /// <returns>返回是否成功</returns>
        public bool moveToGoal(Place p)
        {
            BaseConnection connection=new BaseConnection();

            bool res=connection.sendMoveGoal(p);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X">前+X后-X</param>
        /// <param name="Y">左+Y右-Y</param>
        /// <param name="angel">顺+angel 逆-angel,角度</param>
        /// <returns></returns>
        public int moveToDirection(float X,float Y,float angel)//发角度,90度：90.0   右正左负
        {
            BaseConnection conn=new BaseConnection();
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            EulerAngle eulerAngle=new EulerAngle(angel*Math.PI/180,0,0);
            Quaternion orientation = sitRobotHub.AngleToOritation(eulerAngle);
            Point point=new Point(X,Y,0);
            Header header=new Header("","base_link");
            PoseStamped pose = new PoseStamped(header, point, orientation);
            int res = 0;
            res = sitRobotHub.Movetogoal(pose);
            return res;
        }

        public void SendSpeed(Point twist)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            sitRobotHub.movevel(twist);
        }
        /// <summary>
        /// 发送距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int SendDisplacemet(float x, float y)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            return sitRobotHub.movevel(x, y);
        }
        /// <summary>
        /// 获取当前位置
        /// </summary>
        /// <returns></returns>
        public Place getNowPlace()
        {
            BaseConnection baseConnection=new BaseConnection();
            Place place = baseConnection.getPosition();
            return place;
        }
        /// <summary>
        /// 发送停止速度
        /// </summary>
        public void SpeedStop()
        {
            SendSpeed(new entity.Point(0, 0, 0));
        }
        /// <summary>
        /// 通过控制速度延迟移动（直接移动，精确但没避障）
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void moveToDirectionSpeed(float X,float Y)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            sitRobotHub.movevel(X, Y);
        }
        /// <summary>
        /// 寻找指定ID的腿
        /// </summary>
        /// <param name="LegID"></param>
        /// <returns></returns>
        public Leg findLeg(string LegID)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            Leg resLeg = sitRobotHub.GetLeg(LegID);
            return resLeg;
        }
        /// <summary>
        /// 寻找指定位置附近的腿
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public string getLegIDByUser(float X,float Y)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            string resLeg = sitRobotHub.GetLegIDByUser(X - 0.2f, Y);
            return resLeg;
        }

        /// <summary>
        /// 弧度制原地转向
        /// </summary>
        /// <param name="w"></param>
        public void moveToDirectionSpeedW(float w)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            sitRobotHub.movevelW(w);
        }
        /// <summary>
        /// 打开灯（1为厨房灯，2为卧室灯）
        /// </summary>
        /// <param name="lightId">灯的id</param>
        public void OpenLight(int lightId)
        {
            SitRobotHub sitRobotHub = new SitRobotHub("10.10.100.254", 8899);
            sitRobotHub.OpenLight(lightId);
        }

        public void CloseLight(int lightId)
        {
            SitRobotHub sitRobotHub = new SitRobotHub("10.10.100.254", 8899);
            sitRobotHub.CloseLight(lightId);
        }

        public int PeopleNum(int length)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            return sitRobotHub.PeopleNum(length);
        }

    }
}
