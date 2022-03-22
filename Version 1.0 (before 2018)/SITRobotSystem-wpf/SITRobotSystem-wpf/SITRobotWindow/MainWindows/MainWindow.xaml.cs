using System.Threading;
using System.Windows;
using System.Windows.Media;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.SITRobotWindow.testWindows;
using SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows;

namespace SITRobotSystem_wpf.SITRobotWindow.MainWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompetitionMainWindow competitionWindow;
        private TestMainWindow testMainWindow;
        public MainWindow()
        {

            InitializeComponent();
            btnCompetition.Background=Brushes.Red;
        

        }

        private void testConnection()
        {
            TestingConnectionWindow testingConnectionWindow = new TestingConnectionWindow();
            testingConnectionWindow.Show();
            //DBCtrl dbCtrl
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            sitRobotHub.testConnection();
            Dispatcher.Invoke(() =>
            {
                btnCompetition.Background = Brushes.GreenYellow;
            });
            
            testingConnectionWindow.setLBSucceed("链接成功");
            Thread.Sleep(1000);
            testingConnectionWindow.Close();

        }
        private void btnCompetition_Click(object sender, RoutedEventArgs e)
        {
            competitionWindow=new CompetitionMainWindow();
            competitionWindow.Show();
            
        }
        
        private void BTN_Test_Click(object sender, RoutedEventArgs e)
        {
            testMainWindow=new TestMainWindow();
            testMainWindow.Show();
        }

        private void BtnTestConnection_Click(object sender, RoutedEventArgs e)
        {

            Thread testThread = new Thread(testConnection);
            testThread.Name = "testThread";
            testThread.SetApartmentState(ApartmentState.STA);
            testThread.IsBackground = true;    
            testThread.Start();    
            
        }

    }
}
