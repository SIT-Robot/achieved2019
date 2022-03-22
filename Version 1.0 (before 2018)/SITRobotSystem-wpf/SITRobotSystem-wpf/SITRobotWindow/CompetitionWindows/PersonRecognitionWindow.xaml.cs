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
using SITRobotSystem_wpf.BLL.Competitions.PersonRecognition;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// PersonRecognitionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PersonRecognitionWindow : Window
    {
        private PersonRecognitionCompetition competition;
        public PersonRecognitionWindow()
        {
            competition=new PersonRecognitionCompetition();
            InitializeComponent();
            WindowCtrl windowCtrl = new WindowCtrl();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            competition.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            competition.personrecognitionStage.StateCroSize();
        }
    }
}
