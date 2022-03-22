using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Functions
{
    public static partial class Function
    {
        public static bool Hand_IsReady = false;
        public static bool Hand_IsRunInit = false;
        private static byte Hand_RotationPosition = 0x0B;//旋转舵机水平位置为0x0B
        private static Driver.COMPort HandCOMPort;

        /// <summary>
        /// Init手臂。目的是让手臂回到初始位置，好算坐标。
        /// </summary>
        public static void Hand_Init()
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Driver.Emulator_Hand_Init();
                return;
            }
            Console.WriteLine("Hand Initing.....");
            Hand_IsReady = false;
            Hand_IsRunInit = true;
            HandCOMPort = new Driver.COMPort(FileOperation.ReadTXT(@"..\Data\HandCOMPortName.txt"), int.Parse(FileOperation.ReadTXT(@"..\Data\HandCOMPortBaud.txt")));
            new Thread(new ThreadStart(delegate
            {
                Console.WriteLine("Hand Init: 爪子张开..........");
                HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x01, 0x03, 0x00, 0xFF }, 1);
                Console.WriteLine("Hand Init: 爪子已Init到限位.");
                bool whileCtrl = true;
                Console.WriteLine("Hand Init: 升降杆向下..........");
                while (whileCtrl)
                {

                    byte[] receiveBytes = HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x01, 0xFF, 0XFF }, 1);
                    if (receiveBytes[0] == 0x02) { whileCtrl = false; Console.WriteLine("Hand Init: 升降杆到底."); }
                }
                Console.WriteLine("Hand Init: 升降杆已Init到限位.");
                Thread.Sleep(100);
                Console.WriteLine("Hand Init: 升降杆上升(离开限位)0x10距离.");
                HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x02, 0x10, 0x00 }, 1);
                Console.WriteLine("Hand Init: 爪子收拢(离开限位)0x10距离.");
                HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x02, 0x03, 0x00, 0x10 }, 1);
                Thread.Sleep(1500);
                Hand_IsReady = true;
                Console.WriteLine("Hand Inited.");
            })).Start();
        }

        /// <summary>
        /// 设置旋转舵机位置。范围0x03~0x15（16进制，见MCU文档，改过的记得改这里）
        /// </summary>
        /// <param name="_position"></param>
        public static void Hand_SetRotationPosition(byte _position)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Console.WriteLine("Emulator: Hand Rotation Position: " + _position);
                return;
            }
            if (Hand_IsRunInit == false)
            {
                Console.WriteLine("Hand not init!! For Debug" + " 舵机应该会动 Position:" + _position);
                return;
            }
            Hand_RotationPosition = _position;
            Console.WriteLine("Hand: 手臂旋转舵机 位置" + BitConverter.ToString(new byte[] { _position }, 0).Replace("-", " ").ToUpper());
            HandCOMPort.SendBytesOnly(new byte[] { Hand_RotationPosition, 0x03, 0x03, 0x00, 0x00 });
            Thread.Sleep(1000);
        }

        /// <summary>
        /// 手臂升降丝杆上升
        /// </summary>
        /// <param name="_distance">幅度，从最底到最顶最大2400（10进制）</param>
        public static void Hand_GoUp(int _distance)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Driver.Emulator_Hand_GoUp(_distance);
                return;
            }


            if (Hand_IsRunInit == false)
            {
                Console.WriteLine("Hand not init!! For Debug" + " 丝杆应上升" + _distance);
                return;
            }
            while (!Hand_IsReady) ;
            Console.WriteLine("Hand: 手臂升降杆上升 距离" + _distance);
            byte modDistance = (byte)(_distance % 255);
            double FFCount = Math.Floor(Convert.ToDouble(_distance / 255));
            for (int i = 0; i < FFCount; i++)
            {
                HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x02, 0xFF, 0x00 }, 1);
            }
            HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x02, modDistance, 0x00 }, 1);
            Console.WriteLine("Hand: 手臂升降杆上升结束.");
        }

        /// <summary>
        /// 手臂升降丝杆下降
        /// </summary>
        /// <param name="_distance">幅度，从最底到最顶最大2400(10进制)</param>
        public static void Hand_GoDown(int _distance)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Driver.Emulator_Hand_GoDown(_distance);
                return;
            }
            if (Hand_IsRunInit == false)
            {
                Console.WriteLine("Hand not init!! For Debug" + " 丝杆应该会下降 Position:" + _distance);
                return;
            }
            while (!Hand_IsReady) ;
            Console.WriteLine("Hand: 手臂升降杆下降 距离" + _distance);
            byte modDistance = (byte)(_distance % 255);
            double FFCount = Math.Floor(Convert.ToDouble(_distance / 255));
            for (int i = 0; i < FFCount; i++)
            {
                HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x02, 0xFF, 0x00 }, 1);
            }
            HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x03, 0x01, modDistance, 0x00 }, 1);
            Console.WriteLine("Hand: 手臂升降杆下降结束.");
        }

        /// <summary>
        /// 手臂爪子合拢（Close）
        /// </summary>
        /// <param name="_distance">幅度，最大0xBA（16进制）(一定要小心不然收拢过头电机会炸！)</param>
        public static void Hand_Close(byte _distance)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Driver.Emulator_Hand_Close(_distance);
                return;
            }
            if (Hand_IsRunInit == false)
            {
                Console.WriteLine("Hand not init!! For Debug" + " 爪子应该会Close:" + _distance);
                return;
            }
            while (!Hand_IsReady) ;
            Console.WriteLine("Hand: 手臂爪子收拢 距离" + BitConverter.ToString(new byte[] { _distance }, 0).Replace("-", " ").ToUpper());
            HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x02, 0x03, 0x00, _distance }, 1);
        }

        /// <summary>
        /// 手臂爪子张开（Open）
        /// </summary>
        /// <param name="_distance">幅度，最大0xFF（16进制）(发大的数据比如FF可以直接归到最头打限位，但请不要这么做！这样相对位置容易乱，请Close了多少就Open多少)</param>
        public static void Hand_Open(byte _distance)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Hand_Enable == true)
            {
                Driver.Emulator_Hand_Open(_distance);
                return;
            }
            if (Hand_IsRunInit == false)
            {
                Console.WriteLine("Hand not init!! For Debug" + " 爪子应该会Open:" + _distance);
                return;
            }
            while (!Hand_IsReady) ;
            Console.WriteLine("Hand: 手臂爪子张开 距离" + BitConverter.ToString(new byte[] { _distance }, 0).Replace("-", " ").ToUpper());
            HandCOMPort.SendBytesAndWaitReceive(new byte[] { Hand_RotationPosition, 0x01, 0x03, 0x00, _distance }, 1);
        }
    }
}
