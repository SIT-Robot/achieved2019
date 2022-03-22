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
using SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows;

namespace SITRobotSystem_wpf.SITRobotWindow.Other
{
    /// <summary>
    /// ZotherComputerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ZotherComputerWindow : Window
    {
        public ZotherComputerWindow()
        {
            InitializeComponent();
        }

        private stateWindow sWindow;
        private BaseFunction bFunWindow;
        private FollowWindow followWindow;
        private shoppingWindow ShoppingWindow;


        private void BtnBaseFunction_Click(object sender, RoutedEventArgs e)
        {
            sWindow = new stateWindow();
            sWindow.Show();
            bFunWindow = new BaseFunction();
            bFunWindow.Show();
        }

        private void BtnFollow_Click(object sender, RoutedEventArgs e)
        {
            stateWindow sWindow = new stateWindow();
            sWindow.Show();
            followWindow = new FollowWindow();
            followWindow.Show();
        }

        private void BtnShopping_Click(object sender, RoutedEventArgs e)
        {
            stateWindow sWindow = new stateWindow();
            sWindow.Show();
            ShoppingWindow = new shoppingWindow();
            ShoppingWindow.Show();
        }

        private GpsrWindow gpsrWindow;
        private void BtnGpsr_Click(object sender, RoutedEventArgs e)
        {
            stateWindow sWindow = new stateWindow();
            sWindow.Show();
            gpsrWindow = new GpsrWindow();
            gpsrWindow.Show();
        }
        private WhoisWhoWindow whoisWhoWindow;
        private void BtnWhoisWho_Click(object sender, RoutedEventArgs e)
        {
            whoisWhoWindow = new WhoisWhoWindow();
            whoisWhoWindow.Show();
        }

        private HomeAccidentWindow haWindows;
        private void BtnHomeAccident_OnClick(object sender, RoutedEventArgs e)
        {
            haWindows = new HomeAccidentWindow();
            haWindows.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //private ZooWindow zooWindow;

        //private void BtnZoo_Click(object sender, RoutedEventArgs e)
        //{
        //    zooWindow = new ZooWindow();
        //    zooWindow.Show();
        //}

        private void BtnLongGpsr_Click(object sender, RoutedEventArgs e)
        {
            LongGpsrWindow longGpsr = new LongGpsrWindow();
            longGpsr.Show();
        }

        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            HumburgerWindow humburger = new HumburgerWindow();
            humburger.Show();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InnovationWindows innovation = new InnovationWindows();
            innovation.Show();
        }

        private void BtnBSR_Click(object sender, RoutedEventArgs e)
        {
            BSRWindow bsrWindow = new BSRWindow();
            bsrWindow.Show();
        }

        private void BtnDemonstrate_Click(object sender, RoutedEventArgs e)
        {
            new DemonstrateWindow().Show();
        }
    }
}
