/*串口通信*/
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    public static partial class Driver
    {
        public class COMPort
        {
            string port_name;
            int baud_rate;
            SerialPort sp1;

            /// <summary>
            /// 建立串口通信，不自行处理接受委托
            /// 会有个阻塞(sync)的SendBytesAndWaitReceive给你发还带收的，一条龙服务，放心。
            /// </summary>
            /// <param name="_port_name"></param>
            /// <param name="_baud_rate"></param>
            public COMPort(string _port_name, int _baud_rate)
            {
                port_name = _port_name.TrimEnd();
                baud_rate = _baud_rate;
                sp1 = new SerialPort();
            }

            private void OpenCOMPort()
            {
                try
                {
                    if (!sp1.IsOpen)
                    {
                        sp1.PortName = port_name;
                        sp1.BaudRate = baud_rate;
                        sp1.Open();
                    }
                }
                catch { }
            }

            public void SendBytesOnly(byte[] bytes)
            {
                OpenCOMPort();
                if (!sp1.IsOpen)
                {
                    Console.WriteLine("Sorry!串口" + port_name + "写数据失败！请检查是不是GG了，有没有插♂好");
                    return;
                }
                Console.WriteLine("COM Port: Send Bytes(NoRead): " + BitConverter.ToString(bytes, 0).Replace("-", " ").ToUpper());
                sp1.Write(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// <para>发个数据然后读，请注意设好你要读多少个，读不到这么多就卡死。请确保那边会发回来多少个bytes，就读几个。</para>
            /// <para>不然真的会GG的！不骗你不开玩乐。</para>
            /// </summary>
            /// <param name="bytes">What fucking bytes you want to send.</param>
            /// <param name="readBytesCount">How many fucking bytes you want to read. FBI WARNNING!!! CAREFUL!!! Please ensure it will retuen the count of bytes you set.</param>
            /// <returns></returns>
            public byte[] SendBytesAndWaitReceive(byte[] bytes, int readBytesCount)
            {
                OpenCOMPort();
                if (!sp1.IsOpen)
                {
                    Console.WriteLine("串口" + port_name + "打不开！请有没有插♂好。因此串口拒绝了你的Write&Read请求，并给你返回了一堆全是0x02的bytes，好让程序继续走下去");
                    byte[] errorreturnBytes = new byte[readBytesCount];
                    for (int i = 0; i < readBytesCount; i++) { errorreturnBytes[i] = 0x02; }
                    return errorreturnBytes;
                }
                try
                {
                    Console.WriteLine("COM Port: Send Bytes(WaitReadBack): " + BitConverter.ToString(bytes, 0).Replace("-", " ").ToUpper());
                    sp1.Write(bytes, 0, bytes.Length);
                }
                catch
                {
                    Console.WriteLine("Sorry!串口" + port_name + "用着用着就GG了（刚刚是好的）。看样子是掉了。请有没有插♂好");
                    Console.WriteLine("所以串口拒绝了你的Write&Read请求，并给你返回了一堆全是0x02的bytes，好让程序继续走下去");
                    byte[] errorreturnBytes = new byte[readBytesCount];
                    for (int i = 0; i < readBytesCount; i++) { errorreturnBytes[i] = 0x02; }
                    return errorreturnBytes;
                }
                byte[] returnBytes = new byte[readBytesCount];
                for (int i = 0; i < readBytesCount; i++) { returnBytes[i] = (byte)sp1.ReadByte(); }
                Console.WriteLine("COM Port: Received: " + BitConverter.ToString(returnBytes, 0).Replace("-", " ").ToUpper());
                return returnBytes;
            }

        }
    }
}
