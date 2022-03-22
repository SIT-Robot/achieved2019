using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Connection
{
    public class VisionHub
    {

        private String _address;
        private UInt16 _port;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">设置网络主机地址</param>
        /// <param name="port">设置端口号</param>
        public VisionHub(string address, ushort port)
        {
            _address = address;
            _port = port;
        }
        public string OCRResult()
        {
            String req = "#read#";
            String res = TcpClient(req);
            return res;
        }
        public void readyCommand()
        {
            String req = "#ready#";
            String res = TcpClient(req);
            //return 1;            
        }

        private string TcpClient(string request)
        {
            Client client = new Client(_address, _port);
            String res = client.Sendmessage(request);
            return res;
        }

        public int SetBody(ColorSpacePoint[] bodyPoints)
        {
            //"#select#100@200#800@500#";
            String req = "#select#" + bodyPoints[0].X + "@" + bodyPoints[0].Y + "#" +
                         bodyPoints[1].X + "@" + bodyPoints[1].Y + "#";
            String res = TcpClient(req);
            return isRight(res);
        }

        public string GetRes()
        {
            String req = "#body#";
            String res = TcpClient(req);
            return res;
        }

        public void startOCR()
        {
            String req = "#startOCR#";
            String res = TcpClient(req);
            //return res;
        }
        public string ReadOCR()
        {
            String req = "#readOCR#";
            String res = TcpClient(req);
            return res;
        }
        public string facedetect()
        {
            String req = "#face#";
            String res = TcpClient(req);
            return res;
        }
        public string tcp_exit()
        {
            String req = "#exit#";
            String res = TcpClient(req);
            return res;
        }
        private int isRight(String res)
        {
            int result = 0;
            if (res.Substring(1, 1) == "1")
            {
                result = 1;
            }
            return result;
        }


    }
}
