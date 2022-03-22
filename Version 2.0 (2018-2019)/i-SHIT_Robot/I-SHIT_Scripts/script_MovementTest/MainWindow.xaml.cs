using i_Shit_Core.Core;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using i_Shit_Scirpt.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace i_Shit_Scirpt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Core(new MyScript());
        }



        private void BtnDGo_Click(object sender, RoutedEventArgs e)
        {
            float X = float.Parse(TBDX.Text);
            float Y = float.Parse(TBDY.Text);
            float A = float.Parse(TBDA.Text);

            LocationInfo li = Function.Location_GetRelativeLocationInfo(X, Y, A,Function.Location_GetCurrectLocationFromRos());
            new Thread(new ThreadStart(delegate { Function.Move_Navigate(li); })).Start();
        }


        private void BtnDGoSpeed_Click(object sender, RoutedEventArgs e)
        {

            float X = float.Parse(TBDXS.Text);
            float Y = float.Parse(TBDYS.Text);

            new Thread(new ThreadStart(delegate { Function.Move_Distance(X, Y, 0); })).Start();
        }

        private float linearSpeed = 0;
        private float roundSpeed = 0;



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            linearSpeed = (float)SpeedSlider.Value;
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



        private void btnGoSpeedW_Click(object sender, RoutedEventArgs e)
        {

            float w = float.Parse(TBDWS.Text);
            new Thread(new ThreadStart(delegate { Function.Move_Distance(0, 0, w); })).Start();

        }






        private void StopMoveBtn_Click(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
        }

        private void StopNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo li = new LocationInfo();
            li._orientationW = 0;
            li._orientationX = 0;
            li._orientationY = 0;
            li._orientationZ = 0;
            li._positionX = 0;
            li._positionY = 0;
            li._positionZ = 0;
            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
            new Thread(new ThreadStart(delegate { Function.Move_Navigate(li); })).Start();
            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SpeedSlider.Value = 0.13;
            roundSlider.Value = 0.2;

        }





        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.I:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(linearSpeed, 0, 0); })).Start();
                    break;
                case Key.J:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, linearSpeed, 0); })).Start();
                    break;
                case Key.K:

                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                    break;
                case Key.L:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, -linearSpeed, 0); })).Start();
                    break;
                case Key.OemComma:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(-linearSpeed, 0, 0); })).Start();
                    break;
                case Key.U:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, roundSpeed); })).Start();
                    break;
                case Key.O:
                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, -roundSpeed); })).Start();
                    break;
            }
        }
    }
}
