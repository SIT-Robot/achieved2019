using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Connection
{
    public class MachineLearningConnection
    {
        public static string IPADDRESS = "192.168.1.100";//IP地址
        public static int PORT = 666;//端口
        Socket s = null;
        string resultString = "";
        private void ReceiveMessage(object form1)
        {
            byte[] result = new byte[1048576];
            int receiveNumber = 0;
            Socket myClientSocket = s;
            while (true)
            {
                try
                {
                    receiveNumber = myClientSocket.Receive(result);
                    resultString = Encoding.ASCII.GetString(result, 0, receiveNumber);
                }
                catch { break; }

            }
        }


        private string GOGOGOGO()
        {





            IPAddress ip = IPAddress.Parse(IPADDRESS);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            s.Connect(ip, PORT);
            if (s.Connected == true)
            {
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(this);
                s.Send(System.Text.Encoding.Default.GetBytes("start"));
                while (resultString == "") ;
                return resultString;
            }
            else { return resultString; }
        }
        public static string StartAndGetResult()
        {
            MachineLearningConnection m = new MachineLearningConnection();
            string RESULT = m.GOGOGOGO();
            Console.WriteLine("ML JSON:"+RESULT);
            return RESULT;
        }

  

    }
}
