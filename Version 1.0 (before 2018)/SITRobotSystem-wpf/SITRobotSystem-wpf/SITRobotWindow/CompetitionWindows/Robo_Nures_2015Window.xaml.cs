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
using SITRobotSystem_wpf.BLL.Competitions.Robo_Nures_2015;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// Robo_Nures_2015Window.xaml 的交互逻辑
    /// </summary>
    public partial class Robo_Nures_2015Window : Window
    {

        private Robo_Nures_2015Competition robo_Nures_2015Competition;

        public Robo_Nures_2015Window()
        {
            robo_Nures_2015Competition = new Robo_Nures_2015Competition();
            InitializeComponent();
        }

        private void BtnStart_Click(object sender,RoutedEventArgs e)
        {
            robo_Nures_2015Competition.Start();
        }
    }
}
