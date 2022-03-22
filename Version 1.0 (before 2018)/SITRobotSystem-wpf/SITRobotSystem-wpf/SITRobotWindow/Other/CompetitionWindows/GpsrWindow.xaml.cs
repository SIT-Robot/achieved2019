using System.Windows;
using SITRobotSystem_wpf.BLL.Competitions.GPSR;
using SITRobotSystem_wpf.BLL.Competitions.LongGPSR;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// Gpsr.xaml 的交互逻辑
    /// </summary>
    public partial class GpsrWindow : Window
    {
        public GpsrWindow()
        {
            gpsrCompetition = new GpsrCompetition();
            windowCtrl = new WindowCtrl();
            InitializeComponent();
        }

        private GpsrCompetition gpsrCompetition;
        private WindowCtrl windowCtrl;
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            gpsrCompetition.Start();

        }
    }
}
