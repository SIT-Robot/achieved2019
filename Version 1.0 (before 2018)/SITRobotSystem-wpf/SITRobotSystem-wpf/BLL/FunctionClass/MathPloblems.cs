using System;
using Microsoft.Kinect;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.FunctionClass
{

    /// <summary>
    /// 数学问题计算
    /// </summary>
    public class MathPloblems
    {
        /// <summary>
        /// 平面距离计算相机空间
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Distance2D(CameraSpacePoint p1, CameraSpacePoint p2)
        {
	        double d=0;
            d = Math.Sqrt(Math.Pow((p1.Y - p2.Y), 2) + Math.Pow((p1.X - p2.X), 2));
	        return d;
        }
        /// <summary>
        /// 平面距离计算彩色图像空间
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Distance2D(ColorSpacePoint p1, ColorSpacePoint p2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((p1.Y - p2.Y), 2) + Math.Pow((p1.X - p2.X), 2));
            return d;
        }
        /// <summary>
        /// 空间距离计算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Distance3D(CameraSpacePoint p1, CameraSpacePoint p2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((p1.Y - p2.Y), 2) + Math.Pow((p1.X - p2.X), 2)+ Math.Pow((p1.Z - p2.Z), 2));
            return d;
        }

        /// <summary>
        /// 平面角度计算相机空间
        /// </summary>
        /// <param name="PMove"></param>
        /// <param name="PStand"></param>
        /// <returns></returns>
        public static double Angel(CameraSpacePoint PMove, CameraSpacePoint PStand)
        {
	        double a;
	        double b;
	        double d;
	        a = Math.Abs(PStand.Y - PMove.Y);
	        b = Math.Abs(PStand.X-PMove.X);
	        d = Math.Atan(a/b)/3.14*100;
	        return d;
        }
        /// <summary>
        /// 平面角度计算
        /// </summary>
        /// <param name="PMove"></param>
        /// <param name="PStandX"></param>
        /// <param name="PStandY"></param>
        /// <returns></returns>
        public static double Angel(CameraSpacePoint PMove, double PStandX, double PStandY)
        {
	        double a;
	        double b;
	        double d;
	        a = Math.Abs(PStandY - PMove.Y);
	        b = Math.Abs(PStandX - PMove.X);
	        d = Math.Atan(a/b)/3.14*100;
	        return d;
        }

        /// <summary>
        /// 跟踪速度计算
        /// </summary>
        /// <param name="rPos"></param>
        /// <param name="V_A"></param>
        /// <param name="V_B"></param>
        /// <param name="V_C"></param>
        public static void SpeedCompute(CameraSpacePoint rPos,ref short V_A,ref short V_B,ref short V_C )
        {
	        const double L = 0.20;
	        if (rPos.X>1500)
	        {
	            V_A =
	                ((short)
	                    (350*
	                     ((-Math.Cos(Math.Atan((rPos.X)/rPos.Z))*(rPos.X)) +
	                      (Math.Sin(Math.Atan((rPos.X)/(rPos.Z)))*(rPos.Z)) +
	                      (20*L*(Math.Atan((rPos.X)/(rPos.Z)))))));
	            V_B =
	                ((short)
	                    (350*
	                     ((-Math.Sin(Math.Atan((rPos.X)/(rPos.Z)) - 3.1415926/6)*(rPos.X)) -
	                      (Math.Cos(Math.Atan((rPos.X)/(rPos.Z)) - 3.1415926/6)*(rPos.Z)) +
	                      (10*L*(Math.Atan((rPos.X)/(rPos.Z)))))));
		        V_C = 
                    ((short)
                    (350 * 
                    ((Math.Sin(Math.Atan((rPos.X) / (rPos.Z))) + 3.1415926/ 6) * (rPos.X)) +
                    (Math.Cos(Math.Atan((rPos.X) /(rPos.Z)) +3.1415926 / 6) *(rPos.Z)) +
                    (10 * L * (Math.Atan((rPos.X) /(rPos.Z))))));


	        }
	        else{
		        V_A=0;
		        V_B=0;
		        V_C=0;
	        }

        }


        /// <summary>
        /// 四元数与平面坐标转换
        /// </summary>
        /// <param name="X">横向距离</param>
        /// <param name="Z">纵向距离</param>
        /// <returns></returns>
        public static Point twistCompute(float X ,float Z)
        {
            //Point resTwist = new Point();
            //float BufX = Z - 0.6f;
            //resTwist.X = BufX / 10;
            //if (BufX < 0.3 && BufX > -0.3)
            //{
            //    resTwist.X = 0;
            //}

            //if (Math.Abs(resTwist.X) > 0.2)
            //{
            //    resTwist.X = 0.2 * resTwist.X / Math.Abs(resTwist.X);
            //}

            //double angel = 0;
            ////角度左正右负
            //angel = Math.Atan(X / Z) * 180 / Math.PI;
            //if (Math.Abs(angel) > 10)
            //{
            //    resTwist.Z = angel / 10;
            //    if (Math.Abs(resTwist.Z) > 0.1)
            //    {
            //        resTwist.Z = 0.1 * resTwist.Z / Math.Abs(resTwist.Z);
            //    }
            //}
            //else
            //{
            //    resTwist.Z = 0;
            //}
            //Console.WriteLine("UserX:" + X + "Z:" + Z + "angel:" + angel);
            //return resTwist;
            Point resTwist=new Point();
            resTwist.X=-1.5+1.5*Z;
            if(Z>0.8&&Z<1.2)//距离保持在0.8-1.2
            {
                resTwist.X = 0.1;
            }
            if(resTwist.X>0.3)
            {
                resTwist.X = 0.3;
            }
            double angel = 0;
            //角度左正右负
            angel = Math.Atan(X/Z);
            if (Math.Abs(angel) > 0.1745)//当偏角大于10 度时，转动
            {
                resTwist.Z = angel*0.8;
                if (Math.Abs(resTwist.Z) > 0.4)
                {
                    resTwist.Z = 0.4 ;
                }
            }
            else
            {
                resTwist.Z = 0;
            }
            Console.WriteLine("UserX:"+X+"Z:"+Z+"angel:"+angel);
            return resTwist;
        }
        /// <summary>
        /// 根据图像点计算速度
        /// </summary>
        /// <param name="rPos"></param>
        /// <returns></returns>
        public static Point twistComputeColorPoint(ColorSpacePoint rPos)
        {
            Point resTwist = new Point();
            resTwist.X = 0.12;
            resTwist.Y = 0;
            if (rPos.X<-50)
            {
                resTwist.Z = 0.1;
            }
            else if (rPos.X > 50)
            {
                resTwist.Z =- 0.1;
            }
            else
            {
                resTwist.Z = 0;
            }
            
            return resTwist;
        }


        private static entity.Point CorrentSpeed = new Point(0, 0, 0);
        private static float datea = 0.03f;
        /// <summary>
        /// 平滑速度处理
        /// </summary>
        /// <param name="twist">目标速度</param>
        public static entity.Point smoothSpeed(entity.Point twist)
        {
            entity.Point detaSpeed = new Point(twist.X - CorrentSpeed.X
                , twist.Y - CorrentSpeed.Y
                , twist.Z - CorrentSpeed.Z);
            //处理X方向速度
            if (detaSpeed.X < -datea)
            {
                CorrentSpeed.X -= datea;
            }
            else if (detaSpeed.X < datea)
            {
                CorrentSpeed.X = twist.X;
            }
            if (detaSpeed.X > datea)
            {
                CorrentSpeed.X += datea;
            }

            //处理Y方向速度
            if (detaSpeed.Y < -datea)
            {
                CorrentSpeed.Y -= datea;
            }
            else if (detaSpeed.Y < datea)
            {
                CorrentSpeed.Y = twist.Y;
            }
            if (detaSpeed.Y > datea)
            {
                CorrentSpeed.Y += datea;
            }
            //处理Y方向速度
            if (detaSpeed.Y < -datea)
            {
                CorrentSpeed.Y -= datea;
            }
            else if (detaSpeed.Y < datea)
            {
                CorrentSpeed.Y = twist.Y;
            }
            if (detaSpeed.Y > datea)
            {
                CorrentSpeed.Y += datea;
            }
            //处理Z方向的速度
            CorrentSpeed.Z = twist.Z;
            return CorrentSpeed;
        }

        /// <summary>
        /// 求三个点构成的三角形的夹角
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="X3"></param>
        /// <returns></returns>
        public static float pointAngel(ColorSpacePoint X1, ColorSpacePoint X2, ColorSpacePoint X3)
        {
            float ResAngel = 0;
            float c =(float)Distance2D(X1, X2);
            float b = (float)Distance2D(X2, X3);
            float a = (float)Distance2D(X1, X3);
            float cosa = (float)((Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2))/(2*b*c));
            ResAngel =(float) (Math.Acos(cosa) * 180 / Math.PI);
            return ResAngel;
        }

    }
}
