using System.Windows;
using SITRobotSystem_wpf.BLL.State;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// RobotConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RobotConfigWindow : Window
    {
        public RobotConfigWindow()
        {

            InitializeComponent();
            TBIP.Text = SitRobotConfig.getHubAddress();
            TBPort.Text = SitRobotConfig.getHubPort().ToString();
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            SitRobotConfig.setHubAddress(TBIP.Text);
            SitRobotConfig.setHubPort((ushort) int.Parse(TBPort.Text));
        }

        private void TBIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }
    }
}
