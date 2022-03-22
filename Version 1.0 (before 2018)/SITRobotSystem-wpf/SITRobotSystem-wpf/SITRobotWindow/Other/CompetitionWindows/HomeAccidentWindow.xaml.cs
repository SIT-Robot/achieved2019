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
using SITRobotSystem_wpf.BLL.Competitions.HomeAccident;
using SITRobotSystem_wpf.BLL.Competitions.MOrecognition;
using SITRobotSystem_wpf.BLL.Competitions.nonFinite;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// HomeAccidentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HomeAccidentWindow : Window
    {
        private HomeAccidentCompetition haCompetition;
        private MOCompetition moCompetition;
        public HomeAccidentWindow()
        {
            InitializeComponent();
            haCompetition = new HomeAccidentCompetition();
            moCompetition = new MOCompetition();
        }

        private void BtnHAS_Click(object sender, RoutedEventArgs e)
        {
            //haCompetition.Start();
            moCompetition.Start();
        }
    }
}
