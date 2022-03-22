using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i_Shit_Core.Core.Drivers;
using System.Threading;

namespace i_Shit_Core.Core.Functions
{
    public static partial class Function
    {
        public static void Move_SetSpeedSmooth(TriPoint speedPoint)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {
                Driver.Emulator_Speed = speedPoint;
                return;
            }
            TriPoint sp = MathPloblems.smoothSpeed(speedPoint);
            Driver.Ros_Send("#movevel@" + (sp.X) + "@" + (sp.Y) + "@" + (sp.Z) + "#");

        }
        public static void Move_SetSpeed(float forwardSpeed, float rightSpeed, float rotateSpeed)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {

                Driver.Emulator_Speed.X = forwardSpeed;
                Driver.Emulator_Speed.Y = rightSpeed;
                Driver.Emulator_Speed.Z = rotateSpeed;
                return;
            }
            Driver.Ros_Send("#movevel@" + forwardSpeed + "@" + rightSpeed + "@" + rotateSpeed + "#");
        }

        public static void Move_SetSpeed(TriPoint speedPoint)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {
                Driver.Emulator_Speed = speedPoint;
                return;
            }
            Driver.Ros_Send("#movevel@" + (speedPoint.X).ToString() + "@" + (speedPoint.Y).ToString() + "@" + (speedPoint.Z) + "#");
        }
        public static void Move_Navigate(LocationInfo targetLocation)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {
                Console.WriteLine("Emlator: Navigate to: " + targetLocation._locationName);
                Driver.Emulator_Move_ArrowFakeNavgate();
                return;
            }
            Driver.Ros_Send("#movetogo" + "@map@" + targetLocation._positionX + "@" + targetLocation._positionY + "@" + targetLocation._positionZ + "@" + targetLocation._orientationX + "@" + targetLocation._orientationY + "@" + targetLocation._orientationZ + "@" + targetLocation._orientationW + "#");
            Thread.Sleep(500);
        }

        /// <summary>
        /// 只能跑前后左右或转，如果要旋转forwardDistance和rightDistance要=0。rotateAngle是角度制。
        /// </summary>
        /// <param name="forwardDistance"></param>
        /// <param name="rightDistance"></param>
        /// <param name="rotateAngle"></param>
        public static void Move_Distance(float forwardDistance, float rightDistance, float rotateAngle)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_Move_Enable == true)
            {
                Console.WriteLine("Emulator: Move_Distance(" + forwardDistance + "," + rightDistance + "," + rotateAngle + ")");
                Driver.Emulator_Move_ArrowGo(forwardDistance, rightDistance);
                Driver.Emulator_Move_ArrowRotate(rotateAngle);
                Console.WriteLine("Emulator: Move_Distance Action Finished.");
                return;
            }
            if (forwardDistance == 0 & rightDistance == 0) { Driver.Ros_Send("#movevel&z@" + (Math.PI * rotateAngle / 180) + "#"); }
            else
            {
                Driver.Ros_Send("#movevel&s@" + forwardDistance + "@" + rightDistance + "#");
                //有时会有停不下来的bug，setspeed确保停下
                Function.Move_SetSpeed(0, 0, 0);
                Thread.Sleep(100);
                Function.Move_SetSpeed(0, 0, 0);
                Thread.Sleep(100);
            }

        }

    }
}
