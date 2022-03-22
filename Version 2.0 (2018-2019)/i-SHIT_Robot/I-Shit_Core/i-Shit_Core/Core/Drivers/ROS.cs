using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using i_Shit_Core.Library;
namespace i_Shit_Core.Core.Drivers
{
    //Ros部分
    public static partial class Driver
    {
        private static bool isRosDisabled = false;
        public static void Ros_DisableRosForDebug()
        {
            Console.WriteLine("FBI WARNING!!!!!!!!!!!!");
            Console.WriteLine("ROS DISABLEDDDDDDDDDDDDD!!!");
            Console.WriteLine("FBI WARNING!!!!!!!!!!!!");
            isRosDisabled = true;

        }
        /// <summary>
        /// initialize ROS address
        /// 初始化ros端的地址
        /// </summary>
        /// <param name="ROSIP_">ROS IP Address such as string:"192.168.5.5"</param>
        /// <param name="ROSPORT_">ROS port such as int 1080</param>

        //给 address：port 传递消息（message_) 并执行动作。
        //函数返回 address：port 传来的消息；
        public static String Ros_Send(String message_)
        {
            if (isRosDisabled)
            {
                Console.WriteLine("ROS DEBUG MODE!! NOT SEND:" + message_);
                return "@0@";
            }
            String _address = FileOperation.ReadTXT("RosServer.txt").Split(':')[0];
            UInt16 _uport = UInt16.Parse(FileOperation.ReadTXT("RosServer.txt").Split(':')[1]);
            String responseData = null;
            TcpClient client = null;    //windows api
            NetworkStream stream = null;//windows api
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                client = new TcpClient(_address, _uport);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.ASCII.GetBytes(message_);//transform argument to byte[]信息类型转换

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);//给tcpserver传递message***

                Console.WriteLine("Sent: {0}", message_);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes. 
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);//adrress:port 传回来的消息
                Console.WriteLine("Received: {0}", responseData);
                // Close everything.
                stream.Close();
                client.Close();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("ROS SERVER ERROR! NO Response!!", e);
            }
            return responseData;
        }
    }
    //--Ros部分
}
