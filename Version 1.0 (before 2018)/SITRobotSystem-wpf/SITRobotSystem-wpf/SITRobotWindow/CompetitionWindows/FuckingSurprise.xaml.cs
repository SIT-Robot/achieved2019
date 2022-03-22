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

using SITRobotSystem_wpf.BLL.Competitions.FuckingSurprise;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// Interaction logic for FuckingSurprise.xaml
    /// </summary>
    public partial class FuckingSurprise : Window
    {
        private FuckingSurpriseCompetition fuckingsurprisecompetition;
        public FuckingSurprise()
        {
            fuckingsurprisecompetition = new FuckingSurpriseCompetition();
            InitializeComponent();
        }

        private void FuckingSurprise_Click(object sender, RoutedEventArgs e)
        {
            fuckingsurprisecompetition.Start();
        }
    }
}
