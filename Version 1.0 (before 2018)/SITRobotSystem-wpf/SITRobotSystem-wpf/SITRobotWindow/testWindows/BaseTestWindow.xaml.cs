using System;
using System.CodeDom;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Emgu.CV.Util;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using Point = SITRobotSystem_wpf.entity.Point;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// BaseTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BaseTestWindow : Window
    {
        private BaseCtrl baseCtrl;
        public BaseTestWindow()
        {
            baseCtrl = new BaseCtrl();
            InitializeComponent();
        }

        private void BtnVGo_Click(object sender, RoutedEventArgs e)
        {
            
            float X = float.Parse(TBVX.Text);
            float Y = float.Parse(TBVY.Text);
            float W = float.Parse(TBVW.Text);
            baseCtrl.SendSpeed(new Point(X,Y,W));
        }

        private void BtnDGo_Click(object sender, RoutedEventArgs e)
        {
            float X = float.Parse(TBDX.Text);
            float Y = float.Parse(TBDY.Text);
            float A = float.Parse(TBDA.Text);

            baseCtrl.moveToDirection(X, Y, A);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            TBVX.Text = TBVY.Text = TBVW.Text = "0";

            float X = float.Parse(TBVX.Text);
            float Y = float.Parse(TBVY.Text);
            float W = float.Parse(TBVW.Text);
            baseCtrl.SendSpeed(new Point(X, Y, W));
        }


        private void BtnDGoSpeed_Click(object sender, RoutedEventArgs e)
        {
            delay();

            float X = float.Parse(TBDXS.Text);
            float Y = float.Parse(TBDYS.Text);

            baseCtrl.moveToDirectionSpeed(X,Y);
        }

        private float linearSpeed=0;
        private float roundSpeed=0;



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            linearSpeed =(float) SpeedSlider.Value;
            TBspeed.Text = SpeedSlider.Value.ToString();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SpeedSlider.Value = float.Parse(TBspeed.Text);

        }

        private void roundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            roundSpeed = (float)roundSlider.Value;
            TBRound.Text = roundSlider.Value.ToString();
        }

        private void TBRound_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            roundSlider.Value = float.Parse(TBRound.Text);
        }

        private bool Sending = false;

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.W:
                    baseCtrl.SendSpeed(new Point(linearSpeed,0,0));
                    break;
                case Key.A:
                    baseCtrl.SendSpeed(new Point(0,linearSpeed,0));
                    break;
                case Key.S:
                    baseCtrl.SendSpeed(new Point(0,0,0));
                    break;
                case Key.D:
                    baseCtrl.SendSpeed(new Point(0,-linearSpeed,0));
                    break;     
                case Key.X:
                    baseCtrl.SendSpeed(new Point(-linearSpeed,0,0));
                    break;     
                case Key.Q:
                    baseCtrl.SendSpeed(new Point(0,0,roundSpeed));
                    break;        
                case Key.E:
                    baseCtrl.SendSpeed(new Point(0,0,-roundSpeed));
                    break; 
            }

        }


        private void btnGoSpeedW_Click(object sender, RoutedEventArgs e)
        {   

            delay();
            float w = float.Parse(TBDWS.Text);
            float rad = (float) (Math.PI*w / 180);

            baseCtrl.moveToDirectionSpeedW(rad);

        }

        public void delay()
        {

            int delayTime = int.Parse(this.tbDelay.Text);
            Console.WriteLine("SLEEP for" + delayTime + "times!");
            if (delayTime>0)
            {
                Thread.Sleep(delayTime);
            }
            
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            delay();

            float X = float.Parse(TBDXS.Text);
            float Y = float.Parse(TBDYS.Text);

            baseCtrl.moveToDirectionSpeed(X*1.01f, Y);
        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            delay();

            float X = float.Parse(TBDXS.Text);
            float Y = float.Parse(TBDYS.Text);

            baseCtrl.moveToDirectionSpeed(X * 1.015f, Y);
        }

        private void btn20_Click(object sender, RoutedEventArgs e)
        {
            delay();

            float X = float.Parse(TBDXS.Text);
            float Y = float.Parse(TBDYS.Text);

            baseCtrl.moveToDirectionSpeed(X * 1.02f, Y);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            baseCtrl.moveToDirection(0, 0, 0);
        }

    }
}
