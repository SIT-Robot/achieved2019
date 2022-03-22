using i_Shit_Core.Core;
using i_Shit_Core.Core.Functions;
using i_Shit_Scirpt.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_Init();
            initButton.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00));
            initButton.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
            initButton.Content = "Initing.....";
            DispatcherTimer t = new DispatcherTimer();
            t.Tick += delegate
            {
                if (Function.Hand_IsReady == true)
                {
                    initButton.Background = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00));
                    FBIWARNING.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00));
                    initButton.Content = "Inited";
                    siganPostionBox.Text = "0";
                    zhuaziPositionBox.Text = "0";
                    t.Stop();

                }
            };
            t.Interval = TimeSpan.FromMilliseconds(100);
            t.Start();
        }

        private void zhuaziOpenButton_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_Open((byte)int.Parse(zhuaziAdjBox.Text));
            zhuaziPositionBox.Text = (int.Parse(zhuaziPositionBox.Text) - int.Parse(zhuaziAdjBox.Text)).ToString();
        }

        private void zhuaziCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_Close((byte)int.Parse(zhuaziAdjBox.Text));
            zhuaziPositionBox.Text = (int.Parse(zhuaziPositionBox.Text) + int.Parse(zhuaziAdjBox.Text)).ToString();
        }

        private void siganUpButton_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_GoUp(int.Parse(siganAdjBox.Text));
            siganPostionBox.Text = (int.Parse(siganPostionBox.Text) + int.Parse(siganAdjBox.Text)).ToString();
        }

        private void siganDownButton_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_GoDown(int.Parse(siganAdjBox.Text));
            siganPostionBox.Text = (int.Parse(siganPostionBox.Text) - int.Parse(siganAdjBox.Text)).ToString();
        }

        private void setRotate_Click(object sender, RoutedEventArgs e)
        {
            Function.Hand_SetRotationPosition((byte)Convert.ToInt16(rotateBox.Text.TrimEnd(),16));
        }
    }
}
