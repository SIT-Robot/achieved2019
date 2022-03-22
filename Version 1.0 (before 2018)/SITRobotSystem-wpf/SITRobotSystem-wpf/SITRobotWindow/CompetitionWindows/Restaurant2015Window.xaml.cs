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
using SITRobotSystem_wpf.BLL.Competitions.restaurant;
using SITRobotSystem_wpf.BLL.Competitions.Restaurant2015;
using SITRobotSystem_wpf.UI;
using SITRobotSystem_wpf.BLL.Competitions.Restaurant2017;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// Restaurant2015Window.xaml 的交互逻辑
    /// </summary>
    public partial class Restaurant2015Window : Window
    {
        public Restaurant2015Window()
        {
            InitializeComponent();
            WindowCtrl windowCtrl = new WindowCtrl();
        }

        private Restaurant2015Competition RC2015;
        private Restaurant2017Competition RC2017;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RC2015 = new Restaurant2015Competition();
            RC2015.Start();
        }

        private void Btn_Restaurant2017_Click(object sender, RoutedEventArgs e)
        {
            RC2017 = new Restaurant2017Competition();
            RC2017.Start();
        }
    }
}
