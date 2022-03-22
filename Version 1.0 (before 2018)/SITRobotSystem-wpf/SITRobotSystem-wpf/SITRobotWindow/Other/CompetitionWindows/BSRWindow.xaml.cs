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
using SITRobotSystem_wpf.BLL.Competitions.BSR;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// BSRWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BSRWindow : Window
    {
        private BSRCompetition bsrcomp;
        public BSRWindow()
        {
            bsrcomp=new BSRCompetition();
            bsrcomp.init();
            InitializeComponent();
        }

        private void BtnS1_Click(object sender, RoutedEventArgs e)
        {
            /*
            Quaternion pos=new Quaternion();
            pos.X = float.Parse(this.TBX.Text);
            pos.Y = float.Parse(this.TBY.Text);
            pos.Y = float.Parse(this.TBW.Text);
            bsrcomp.runStage1(pos);*/
        }

        private void BTNS2_Click(object sender, RoutedEventArgs e)
        {
            bsrcomp.Start();
        }

        private void btnGodoor_Click(object sender, RoutedEventArgs e)
        {
            bsrcomp.setIsThroughDoor(true);
            bsrcomp.Start();
        }

        private void btnNotGoDoor_Click(object sender, RoutedEventArgs e)
        {
            bsrcomp.setIsThroughDoor(false);
            bsrcomp.Start();
        }
        

    }
}
