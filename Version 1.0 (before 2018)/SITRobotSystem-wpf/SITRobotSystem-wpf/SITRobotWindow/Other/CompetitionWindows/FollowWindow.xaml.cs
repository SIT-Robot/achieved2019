using System.Windows;
using SITRobotSystem_wpf.BLL.Competitions.Follow;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// FollowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FollowWindow : Window
    {
        private FollowCompetition followComp;
        private WindowCtrl windowCtrl;


        public FollowWindow()
        {
            followComp=new FollowCompetition();
            windowCtrl=new WindowCtrl();
            InitializeComponent();
        }

        private void BtnFollowStart_Click(object sender, RoutedEventArgs e)
        {
            followComp.Start();
        }
    }
}
