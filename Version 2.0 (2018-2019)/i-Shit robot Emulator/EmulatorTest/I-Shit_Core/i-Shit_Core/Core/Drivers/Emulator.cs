using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace i_Shit_Core.Core.Drivers
{
    public static partial class Driver
    {
        internal static bool Emulator_Mode = false;
        internal static TriPoint Emulator_Speed = new TriPoint();
        internal static bool Emulator_Ready = false;
        internal static bool Emulator_Move_Enable = false;
        internal static bool Emulator_Hand_Enable = false;
        internal static bool Emulator_MLCSP_Enable = false;
        internal static bool Emulator_BodyDetect_Enable = false;
        internal static EmulatorMainWindow Emulator_MainWindow = null;
        internal static EmulatorFootprintWindow Emulator_FootprintWindow = null;
        internal static EmulatorHandWindow Emulator_HandWindow = null;
        internal static EmulatorMLCSPWindow Emulator_MLCSPWindow = null;
        internal static EmulatorBodyDetectWindow Emulator_BodyDetectWindow = null;

        public static void Emulator_Enable()
        {
            Emulator_MainWindow = new EmulatorMainWindow();
            Emulator_FootprintWindow = new EmulatorFootprintWindow();
            Emulator_HandWindow = new EmulatorHandWindow();
            Emulator_BodyDetectWindow = new EmulatorBodyDetectWindow();
            Emulator_MLCSPWindow = new EmulatorMLCSPWindow();
            Emulator_MainWindow.Show();
            Emulator_EnableSpeeder();
            Emulator_Mode = true;
        }

        internal static void ThroughEdge()
        {

            if (Emulator_FootprintWindow.odomArrow.Margin.Top <-356)
            {
                Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, 202, 0, 0);
            }
            if (Emulator_FootprintWindow.odomArrow.Margin.Left > 518)
            {
                Emulator_FootprintWindow.odomArrow.Margin = new Thickness(-467, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
            }

            if (Emulator_FootprintWindow.odomArrow.Margin.Top >202)
            {
                Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, -356, 0, 0);
            }
            if (Emulator_FootprintWindow.odomArrow.Margin.Left < -467)
            {
                Emulator_FootprintWindow.odomArrow.Margin = new Thickness(518, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
            }
        }
        internal static void Emulator_EnableSpeeder()
        {
            Emulator_Speed.X = 0;
            Emulator_Speed.Y = 0;
            Emulator_Speed.Z = 0;
            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    Thread.Sleep(5);
                    double cosUP = Math.Cos(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);
                    double sinUP = Math.Sin(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);
                    double cosRIGHT = Math.Cos(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);
                    double sinRIGHT = -Math.Sin(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);

                    UIThreadOperator.Send(delegate
                    {
                        double top = Emulator_FootprintWindow.odomArrow.Margin.Top - Emulator_Speed.X * cosUP * 2;
                        Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                        double left = Emulator_FootprintWindow.odomArrow.Margin.Left + Emulator_Speed.X * sinUP * 2;
                        Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                        ThroughEdge();
                    }, null);
                    UIThreadOperator.Send(delegate
                    {
                        double left = Emulator_FootprintWindow.odomArrow.Margin.Left + Emulator_Speed.Y * cosRIGHT * 2;
                        Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                        double top = Emulator_FootprintWindow.odomArrow.Margin.Top - Emulator_Speed.Y * sinRIGHT * 2;
                        Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                        ThroughEdge();
                    }, null);
                    UIThreadOperator.Send(delegate
                    {
                        if (Emulator_Speed.Z != 0)
                        {
                            double angle = Emulator_Speed.Z;
                            RotateTransform rotateTransform = new RotateTransform(Emulator_FootprintWindow.ArrowRotateAngle);

                            bool minus = false;
                            if (angle < 0)
                            {
                                minus = true;
                            }
                            int absang = (int)Math.Abs(angle);
                            if (minus == true)
                            {
                                Emulator_FootprintWindow.ArrowRotateAngle += 1;
                                rotateTransform.Angle += 1;
                            }
                            else
                            {
                                Emulator_FootprintWindow.ArrowRotateAngle -= 1;
                                rotateTransform.Angle -= 1;
                            }
                            Emulator_FootprintWindow.odomArrow.RenderTransform = rotateTransform;

                        }
                    }, null);
                }

            })).Start();
        }
        internal static void Emulator_Move_ArrowFakeNavgate()
        {
            Emulator_Move_ArrowGo(1, 0);
        }
        internal static void Emulator_Move_ArrowGo(float up, float right)
        {
            if (Emulator_Mode == false || Emulator_MainWindow == null)
            {
                Console.WriteLine("Emulator Error: Emulator Not Enabled !");
                return;
            }
            //Going UP
            double cosUP = Math.Cos(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);
            double sinUP = Math.Sin(Emulator_FootprintWindow.ArrowRotateAngle * Math.PI / 180);
            double upTimes = (up * 10) * cosUP;
            double rightTimes = (up * 10) * sinUP;
            if (upTimes > rightTimes)
            {
                double multi = upTimes / rightTimes;
                if (upTimes >= 0)
                {
                    for (int i = 0; i < upTimes; i++)
                    {
                        Thread.Sleep(50);
                        UIThreadOperator.Send(delegate
                        {
                            double top = Emulator_FootprintWindow.odomArrow.Margin.Top - 1;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                            double left = Emulator_FootprintWindow.odomArrow.Margin.Left + 1 / multi;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                            ThroughEdge();
                        }, null);
                    }
                }
                else
                {
                    for (int i = 0; i > upTimes; i--)
                    {
                        Thread.Sleep(50);
                        UIThreadOperator.Send(delegate
                        {
                            double top = Emulator_FootprintWindow.odomArrow.Margin.Top + 1;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                            double left = Emulator_FootprintWindow.odomArrow.Margin.Left - 1 / multi;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                            ThroughEdge();
                        }, null);
                    }

                }

            }
            else
            {
                double multi = rightTimes / upTimes;
                if (rightTimes >= 0)
                {
                    for (int i = 0; i < rightTimes; i++)
                    {
                        Thread.Sleep(50);
                        UIThreadOperator.Send(delegate
                        {
                            double top = Emulator_FootprintWindow.odomArrow.Margin.Top - 1 / multi;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                            double left = Emulator_FootprintWindow.odomArrow.Margin.Left + 1;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                            ThroughEdge();
                        }, null);
                    }
                }
                else
                {
                    for (int i = 0; i > rightTimes; i--)
                    {
                        Thread.Sleep(50);
                        UIThreadOperator.Send(delegate
                        {
                            double top = Emulator_FootprintWindow.odomArrow.Margin.Top + 1 / multi;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(Emulator_FootprintWindow.odomArrow.Margin.Left, top, 0, 0);
                            double left = Emulator_FootprintWindow.odomArrow.Margin.Left - 1;
                            Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);
                            ThroughEdge();
                        }, null);
                    }
                }
            }





            //--GoingUP

            for (int i = 0; i < (right * 10); i++)
            {
                Thread.Sleep(50);
                UIThreadOperator.Send(delegate
                {
                    double left = Emulator_FootprintWindow.odomArrow.Margin.Left + 1;

                    Emulator_FootprintWindow.odomArrow.Margin = new Thickness(left, Emulator_FootprintWindow.odomArrow.Margin.Top, 0, 0);

                }, null);
            }


        }
        internal static void Emulator_Move_ArrowRotate(float angle)
        {
            bool minus = false;
            if (angle < 0)
            {
                minus = true;
            }
            int absang = (int)Math.Abs(angle);

            for (int i = 0; i < absang; i++)
            {
                Thread.Sleep(50);
                UIThreadOperator.Send(delegate
                {

                    RotateTransform rotateTransform = new RotateTransform(Emulator_FootprintWindow.ArrowRotateAngle);

                    if (minus == true)
                    {
                        Emulator_FootprintWindow.ArrowRotateAngle += 1;
                        rotateTransform.Angle += 1;
                    }
                    else
                    {
                        Emulator_FootprintWindow.ArrowRotateAngle -= 1;
                        rotateTransform.Angle -= 1;
                    }
                    Emulator_FootprintWindow.odomArrow.RenderTransform = rotateTransform;


                }, null);

            }
        }

        //Hand 
        internal static void Emulator_Hand_Init()
        {
            UIThreadOperator.Send(delegate
            {
                Emulator_HandWindow.handslider.Value = 0;
                Emulator_HandWindow.handheight.Text = "0";
                Emulator_HandWindow.handup.RenderTransform = new RotateTransform(45, 232, 63);
                Emulator_HandWindow.handdown.RenderTransform = new RotateTransform(-45, 232, 0);
                Emulator_HandWindow.HandAngle = 45;
                Emulator_HandWindow.handdown.Visibility = Visibility.Visible;
                Emulator_HandWindow.handup.Visibility = Visibility.Visible;
                Emulator_HandWindow.notinitlabel.Visibility = Visibility.Hidden;
                Emulator_HandWindow.handslider.Visibility = Visibility.Visible;
                Emulator_HandWindow.handheight.Visibility = Visibility.Visible;
                Emulator_HandWindow.handheight.Text = "0";
            }, null);
            Console.WriteLine("Emulator: Hand Inited.");
        }


        internal static void Emulator_Hand_GoUp(int distance)
        {
            UIThreadOperator.Send(delegate
            {
                Emulator_HandWindow.handslider.Value += distance;
                Emulator_HandWindow.handheight.Text = (int.Parse(Emulator_HandWindow.handheight.Text) + distance).ToString();
            }, null);
            Thread.Sleep(1000);
        }

        internal static void Emulator_Hand_GoDown(int distance)
        {
            UIThreadOperator.Send(delegate
            {
                Emulator_HandWindow.handslider.Value -= distance;
                Emulator_HandWindow.handheight.Text = (int.Parse(Emulator_HandWindow.handheight.Text) - distance).ToString();
            }, null);
            Thread.Sleep(1000);

        }

        internal static void Emulator_Hand_Close(byte distance)
        {
            UIThreadOperator.Send(delegate
            {
                float newangle = distance * 0.2419f;
                Emulator_HandWindow.HandAngle = Emulator_HandWindow.HandAngle - newangle;

                Emulator_HandWindow.handup.RenderTransform = new RotateTransform(Emulator_HandWindow.HandAngle, 232, 63);
                Emulator_HandWindow.handdown.RenderTransform = new RotateTransform(-Emulator_HandWindow.HandAngle, 232, 0);

            }, null);
            Thread.Sleep(1000);
        }

        internal static void Emulator_Hand_Open(byte distance)
        {
            UIThreadOperator.Send(delegate
            {
                float newangle = distance * 0.2419f;
                if ((Emulator_HandWindow.HandAngle + newangle) > 45)
                {
                    Emulator_HandWindow.HandAngle = 45;
                }
                else
                {
                    Emulator_HandWindow.HandAngle = Emulator_HandWindow.HandAngle + newangle;
                }




                Emulator_HandWindow.handup.RenderTransform = new RotateTransform(Emulator_HandWindow.HandAngle, 232, 63);
                Emulator_HandWindow.handdown.RenderTransform = new RotateTransform(-Emulator_HandWindow.HandAngle, 232, 0);
            }, null);
            Thread.Sleep(1000);
        }
        //--Hand

    }
}
