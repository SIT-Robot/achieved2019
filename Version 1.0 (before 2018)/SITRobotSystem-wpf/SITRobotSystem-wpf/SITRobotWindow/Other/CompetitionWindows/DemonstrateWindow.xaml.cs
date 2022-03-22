using SITRobotSystem_wpf.BLL.Competitions.Demonstrate;
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

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// DemonstrateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DemonstrateWindow : Window
    {
        public DemonstrateWindow()
        {
            InitializeComponent();
        }

        private DemonstrateCompetition deCompetition;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            deCompetition = new DemonstrateCompetition();
            deCompetition.Start();
        }
    }
}
