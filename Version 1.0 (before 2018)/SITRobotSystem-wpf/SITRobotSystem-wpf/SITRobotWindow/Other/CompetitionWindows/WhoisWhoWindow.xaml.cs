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
using SITRobotSystem_wpf.BLL.Competitions.WhoisWho;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// WhoisWhoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WhoisWhoWindow : Window
    {
        public WhoisWhoWindow()
        {
            InitializeComponent();
        }

        private WhoisWhoCompetition whoisWhoCompetition;
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            whoisWhoCompetition=new WhoisWhoCompetition();
            whoisWhoCompetition.Start();
        }
    }
}
