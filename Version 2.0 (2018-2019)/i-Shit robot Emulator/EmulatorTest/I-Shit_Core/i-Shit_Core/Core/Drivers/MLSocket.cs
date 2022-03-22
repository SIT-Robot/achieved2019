using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    public static partial class Driver
    {
        /// <summary>
        /// Socket连接MachineLearing服务器发送start命令，获得结果JSON String
        /// </summary>
        /// <returns>返回个JSON String，然后可以拿去解析</returns>
        public static string MachineLearningSocket_StartAndGetResult()
        {

            String _address = FileOperation.ReadTXT("MLServer.txt").Split(':')[0];
            int _uport = int.Parse(FileOperation.ReadTXT("MLServer.txt").Split(':')[1]);
            Socket s = null;
            string resultString = "";


            IPAddress ip = IPAddress.Parse(_address);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            s.Connect(ip, _uport);
            if (s.Connected == true)
            {
                Thread receiveThread = new Thread(new ThreadStart(delegate
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
                }));
                receiveThread.Start();
                s.Send(System.Text.Encoding.Default.GetBytes("start"));
                while (resultString == "") ;
                return resultString;
            }
            else { return resultString; }
        }
    }
}

