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
using SITRobotSystem_wpf.BLL.Competitions.LongGPSR;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// LongGpsrWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LongGpsrWindow : Window
    {
        public LongGpsrWindow()
        {
            InitializeComponent();
        }

        private LongGpsrCompetition longGpsrCompetition;
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            longGpsrCompetition = new LongGpsrCompetition();
            longGpsrCompetition.Start();
        }
    }
}
