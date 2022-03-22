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
using System.Windows.Shapes;
using SITRobotSystem_wpf.BLL.Competitions.WakeMeUp;
using SITRobotSystem_wpf.UI;
namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// WakeMeUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WakeMeUpWindow : Window
    {
        WakeMeUpCompetition wakemeup;
        public WakeMeUpWindow()
        {
            wakemeup= new WakeMeUpCompetition();
            InitializeComponent();
            WindowCtrl windowCtrl = new WindowCtrl();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            wakemeup.Start();
        }

        private void BtnAwake_OnClick(object sender, RoutedEventArgs e)
        {
            while (!wakemeup.wakemeupStage.wakemeupTask.IsAwake())
            {
                wakemeup.wakemeupStage.wakemeupTask.speak("no wake");
                Thread.Sleep(1000);
            }
        }
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            wakemeup.wakemeupStage.TakeOrder();
        }
        private void BtnServe_Click(object sender, RoutedEventArgs e)
        {
            wakemeup.wakemeupStage.ServeBreakfast();
        }
        private void BtnDeliver_Click(object sender, RoutedEventArgs e)
        {
            wakemeup.wakemeupStage.DeliverBreakfast();
        }
    }
}
