using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using SITRobotSystem_wpf.entity;
using Point = SITRobotSystem_wpf.entity.Point;

namespace SITRobotSystem_wpf.BLL.Connection
{
    public class SitRobotHub
    {
        private String _address;
        private UInt16 _port;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">设置网络主机地址</param>
        /// <param name="port">设置端口号</param>
        public SitRobotHub(string address, ushort port)
        {
            _address = address;
            _port = port;
        }

        public String testConnection()
        {
            string testStr = "#TESTCONNECT#";
            String res = TcpClient(testStr);
            return res;
        }


        /// <summary>
        /// 字符串转Position
        /// </summary>
        /// <param name="temp">被处理的字符串</param>
        /// <returns>转换后的对象</returns>
        private PoseStamped StringToPosition(string temp)  //处理字符串
        {
            ////#movetogo@1@1@1@1@1@1@1#
            PoseStamped pose = new PoseStamped();
            string[] spilted = temp.Split('@');
            pose.header.PlaceName = spilted[0].Split('#')[1];
            pose.position.X = Convert.ToDouble(spilted[1]);
            pose.position.Y = Convert.ToDouble(spilted[2]);
            pose.position.Z = Convert.ToDouble(spilted[3]);
            pose.oritation.X = Convert.ToDouble(spilted[4]);
            pose.oritation.Y = Convert.ToDouble(spilted[5]);
            pose.oritation.Z = Convert.ToDouble(spilted[6]);
            pose.oritation.W = Convert.ToDouble(spilted[7].Split('#')[0]);
            return pose;      //Point对象
        }

        /// <summary>
        /// Position对象转字符串
        /// </summary>
        /// <param name="position">Position对象</param>
        /// <param name="funtion">字符串标识符</param>
        /// <returns>转换后的字符串</returns>
        private string PositionToString(PoseStamped position, String funtion)
        {
            String result = null;
            String[] tem = new String[8];
            tem[0] = position.header.FrameId;
            tem[1] = (position.position.X).ToString();
            tem[2] = (position.position.Y).ToString();
            tem[3] = (position.position.Z).ToString();
            tem[4] = (position.oritation.X).ToString();
            tem[5] = (position.oritation.Y).ToString();
            tem[6] = (position.oritation.Z).ToString();
            tem[7] = (position.oritation.W).ToString();
            result += "#" + funtion;
            for (int i = 0; i <= 7; i++)
            {
                result += "@";
                result += tem[i];
            }
            result += "#";
            return result;
        }
        /// <summary>
        /// 向机器人发送目标点
        /// </summary>
        /// <param name="pose">PoseStamped对象</param>
        /// <returns>机器人执行情况0为未完成，1为完成</returns>
        public int Movetogoal(PoseStamped pose)
        {
            String req = PositionToString(pose, "movetogo");

            String res = TcpClient(req);
            Console.Write(res);
            return isRight(res);
        }
        /// <summary>
        /// 验证返回值的正确性
        /// </summary>
        /// <param name="res">返回值</param>
        /// <returns>成功与否</returns>
        private int isRight(String res)
        {
            int result = 0;
            if (res.Substring(1, 1) == "1")
            {
                result = 1;
            }
            return result;
        }
        /// <summary>
        /// 手臂操作函数
        /// </summary>
        /// <returns>执行情况</returns>
        public int HandAction(int type)
        {
            String req = "#handaction@" + type + "#";
            String res = TcpClient(req);
            return isRight(res);
        }
        /// <summary>
        /// 获取机器人当前位置
        /// </summary>
        /// <returns>机器人当前位置坐标对象</returns>
        public PoseStamped GetCurrPosition()
        {
            PoseStamped currPosition = new PoseStamped();
            String res = TcpClient("#getLocation#");
            string[] temp = res.Split('@');
            if (temp[1] != "getLocation")
            {
                throw new Exception("返回值错误");
            }
            currPosition.header.FrameId = temp[2];
            currPosition.position.X = Convert.ToDouble(temp[3]);
            currPosition.position.Y = Convert.ToDouble(temp[4]);
            currPosition.position.Z = Convert.ToDouble(temp[5]);
            currPosition.oritation.X = Convert.ToDouble(temp[6]);
            currPosition.oritation.Y = Convert.ToDouble(temp[7]);
            currPosition.oritation.Z = Convert.ToDouble(temp[8]);
            currPosition.oritation.W = Convert.ToDouble(temp[9]);
            return currPosition;
        }


        public void movevel(Point twist)
        {
            String req = "#movevel@"+twist.X+"@"+twist.Y+"@"+twist.Z+"#";
            String res = TcpClient(req);
        }

        public int movevel(double x,double y)
        {
            String req = "#movevel&s@" + x + "@" + y + "#";
            String res = TcpClient(req);
            return isRight(res);
        }

        /// <summary>
        /// 弧度转四元数
        /// </summary>
        /// <param name="ea">角度对象</param>
        /// <returns>四元数对象</returns>
        public Quaternion AngleToOritation(EulerAngle ea)
        {
            Quaternion oritation = new Quaternion();
            double fCosHRoll = Math.Cos(ea.FRoll * .5f);
            double fSinHRoll = Math.Sin(ea.FRoll * .5f);
            double fCosHPitch = Math.Cos(ea.FPitch * .5f);
            double fSinHPitch = Math.Sin(ea.FPitch * .5f);
            double fCosHYaw = Math.Cos(ea.FYaw * .5f);
            double fSinHYaw = Math.Sin(ea.FYaw * .5f);

            oritation.W = fCosHRoll * fCosHPitch * fCosHYaw + fSinHRoll * fSinHPitch * fSinHYaw;
            oritation.X = fCosHRoll * fSinHPitch * fCosHYaw + fSinHRoll * fCosHPitch * fSinHYaw;
            oritation.Y = fCosHRoll * fCosHPitch * fSinHYaw - fSinHRoll * fSinHPitch * fCosHYaw;
            oritation.Z = fSinHRoll * fCosHPitch * fCosHYaw - fCosHRoll * fSinHPitch * fSinHYaw;
            return oritation;
        }

        /// <summary>
        /// 四元数转角度
        /// </summary>
        /// <param name="oritation">四元数对象</param>
        /// <returns>角度对象</returns>
        public EulerAngle OritationToAngle(Quaternion oritation)
        {
            EulerAngle ea = new EulerAngle();
            ea.FRoll = Math.Atan2(2 * (oritation.W * oritation.Z + oritation.X * oritation.Y),
                1 - 2 * (oritation.Z * oritation.Z + oritation.X * oritation.X));
            ea.FPitch = Math.Asin(CLAMP(2 * (oritation.W * oritation.X - oritation.Y * oritation.Z), -1.0f, 1.0f));
            ea.FYaw = Math.Atan2(2 * (oritation.W * oritation.Y + oritation.Z * oritation.X),
                1 - 2 * (oritation.X * oritation.X + oritation.Y * oritation.Y));
            return ea;
        }

        private double CLAMP(double x, double min, double max)
        {
            return (x) > (max) ? (max) : ((x) < (min) ? (min) : x);
        }

        public MemoryStream getEM()
        {
            Client client = new Client(_address, _port);
            return client.SendImmessage("#EM#");
        }

        public MemoryStream getPM()
        {
            Client client = new Client(_address, _port);
            return client.SendImmessage("#PM#");
        }

        private string TcpClient(string request)
        {
            Client client = new Client(_address, _port);
            String res = client.Sendmessage(request);
            return res;
        }

        public Leg GetLeg(string legId)
        {
            Client client = new Client(_address, _port);
            string request = "#Peopleid@" + legId+"#";
            String res = client.Sendmessage(request);
            string[] temp = res.Split('@');
            if (temp[1] == "null")
            {
                //throw new Exception("返回值错误");
            }
            Leg resLeg=new Leg();   
            resLeg.PeopleID = temp[1];
            resLeg.X = float.Parse(temp[2]) ;
            resLeg.Y =- float.Parse(temp[3]);
            Console.WriteLine("get leg");
            return resLeg;

        }

        public string GetLegIDByUser(float f, float f1)
        {
            Client client = new Client(_address, _port);
            string request = "#Peoplexy@"+f+"@"+f1+"#";
            String res = client.Sendmessage(request);
            string[] temp = res.Split('@');
            if (temp[1] == "null")
            {
                Console.WriteLine("返回值错误");
            }
            string PeopleID = temp[1];
            return PeopleID;
        }

        public int movevelW(float w)
        {
            String req = "#movevel&z@" + w+ "#";
            String res = TcpClient(req);
            return isRight(res);
        }

        static int IsLightOpen = 0;
        //全开BDFHJLNP
        public int OpenLight(int opt)
        {
            if (IsLightOpen == 0)
            {
                Client client = new Client(_address, _port);
                string request = "BDFHJLNP";
                switch (opt)
                {
                    case 1:
                        request = "BDEGIKMO";
                        break;
                    case 2:
                        request = "ADEGIKMO";
                        break;
                }
                string res = client.Sendmessage(request);
                IsLightOpen = opt;
            }
            return 1;
        }
        //全关ACEGIKMO
        public int CloseLight(int opt)
        {
            if (IsLightOpen != 0)
            {
                Client client = new Client(_address, _port);
                string request = "ADEGIKMO";
                switch (opt)
                {
                    case 1:
                        request = "ADEGIKMO";
                        break;
                    case 2:
                        request = "BDEGIKMO";
                        break;
                }
                string res = client.Sendmessage(request);
                IsLightOpen = 0;
            }
            return 1;
        }

        public int PeopleNum(int length)
        {
            Client client = new Client(_address, _port);
            string num = client.Sendmessage("#PeopleNum@" + length + "#");
            string[] temp = num.Split('@');
            return int.Parse(temp[1]);
        }


        public void FaceRecognitionTrain(string name)
        {
            Client client=new Client(_address,_port);
            string req = "#FaceRecognitionTrain@" + 2 + "@" + name + "#";
            client.Sendmessage(req);
            Thread.Sleep(10000);
            req = "#FaceRecognitionTrain@" + 3 + "@none#";
            client.Sendmessage(req);
        }

        public string FaceRecognition()
        {
            Client client = new Client(_address, _port);
            string req = "#FaceRecognition#";
            string res=client.Sendmessage(req);
            return res;
        }
    }
}
