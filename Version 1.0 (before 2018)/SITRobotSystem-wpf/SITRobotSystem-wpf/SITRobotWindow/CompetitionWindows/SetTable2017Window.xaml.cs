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

using SITRobotSystem_wpf.BLL.Competitions.SetTable2017;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// SetTable2017.xaml 的交互逻辑
    /// </summary>
    public partial class SetTable2017Window : Window
    {
        SetTable2017Competition setTable2017Competition;
        public SetTable2017Window()
        {
            InitializeComponent();
            setTable2017Competition = new SetTable2017Competition();
            WindowCtrl windowCtrl = new WindowCtrl();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            setTable2017Competition.Start();
        }
    }
}
