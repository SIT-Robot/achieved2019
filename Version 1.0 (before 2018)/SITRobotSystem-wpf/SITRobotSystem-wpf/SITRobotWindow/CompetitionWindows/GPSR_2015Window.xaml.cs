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
using SITRobotSystem_wpf.BLL.Competitions.GPSR_2015;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// GPSR_2015Window.xaml 的交互逻辑
    /// </summary>
    public partial class GPSR_2015Window : Window
    {

        private GPSR_2015Competition gPSR_2015Competition;
        public GPSR_2015Window()
        {
            gPSR_2015Competition = new GPSR_2015Competition();
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            gPSR_2015Competition.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BaseCtrl baseCtrl = new BaseCtrl();
            baseCtrl.moveToDirection(0, 0, 0);
        }
    }
}
