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
using SITRobotSystem_wpf.BLL.Competitions.OpenChallenge2015;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// OpenChallengeWindows.xaml 的交互逻辑
    /// </summary>
    public partial class OpenChallengeWindows : Window
    {
        public OpenChallengeWindows()
        {
            InitializeComponent();
        }

        private OpenChallenge2015Competition OC2015;

        private void Btn_OC_Click(object sender, RoutedEventArgs e)
        {
            OC2015=new OpenChallenge2015Competition();
            OC2015.Start();
        }
    }
}
