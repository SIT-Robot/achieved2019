using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.ZOther.WhoisWho
{
    public class ClientSocket
    {
        private static byte[] result = new byte[1024];
        Socket clientSocket;
        private ClientSocket()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }
        }
        private static volatile ClientSocket _instance;
        public static ClientSocket GetInstance()        //判断是否有实例化过，确保只有一个
        {
            if (_instance == null)
            {
                _instance = new ClientSocket();
            }
            return _instance;
        }
        public void SendMessagg(string message)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes(message));
            System.Console.WriteLine("已发送" + message);
        }
        public string ReceiveMessage()
        {
            string message = "";
            while (true)
            {
                message = Encoding.ASCII.GetString(result, 0, clientSocket.Receive(result));
                if (message != "")
                {
                    System.Console.WriteLine("成功接收" + message);
                    return message;
                }
            }
        }
    }
}
