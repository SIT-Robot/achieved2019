using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using SITRobotSystem_wpf.BLL.Competitions.Huawei;
using SITRobotSystem_wpf.UI;
using System.Threading;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// HuaweiWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HuaweiWindow : Window
    {
        public HuaweiCompetition hwCompetition;
        public static readonly RoutedEvent MediaEndedEvent;
        //private WindowCtrl wc;
        public HuaweiWindow()
        {
            hwCompetition = new HuaweiCompetition();
            //wc = new WindowCtrl();
            InitializeComponent();
        }

        //事件处理器
        public void PlayVideo_Handler(string commmand)
        {
            if (commmand == "start")
            {
                Thread th = new Thread(new ThreadStart(Start_Video));
                th.Start();
            }
        }

        public void Start_Video()
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    string filepath = @"E:\TrackPad2.mp4";
                    //FileStream fs = File.Open(filepath, FileMode.Append);
                    Media.Source = new Uri(filepath, UriKind.Relative);
                    //this.WindowState = WindowState.Maximized;
                    //this.WindowStyle = WindowStyle.None;
                    //Media.HorizontalAlignment = HorizontalAlignment.Stretch;
                    //Media.VerticalAlignment = VerticalAlignment.Stretch;
                    btn.Visibility = Visibility.Collapsed;
                    Media.Play();
                }));
            }).Start();
        }

        public void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            hwCompetition.Start();
        }
    }
}
