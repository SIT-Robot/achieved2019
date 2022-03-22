using System.Windows;
using SITRobotSystem_wpf.BLL.State;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// TestingConnectionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestingConnectionWindow : Window
    {
        public TestingConnectionWindow()
        {
            InitializeComponent();
            LBPort.Content = SitRobotConfig.getHubPort();
            LBAddress.Content = SitRobotConfig.getHubAddress();
        }

        public void setLBSucceed(string str)
        {
            LBSuccess.Content = str;
        }
    }
}
