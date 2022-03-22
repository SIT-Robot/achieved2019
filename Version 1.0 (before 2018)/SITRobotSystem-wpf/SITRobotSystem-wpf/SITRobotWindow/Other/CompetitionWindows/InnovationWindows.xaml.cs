﻿using System;
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
using SITRobotSystem_wpf.BLL.Competitions.Innovation;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// InnovationWindows.xaml 的交互逻辑
    /// </summary>
    public partial class InnovationWindows : Window
    {
        private InnovationCompetition innovation;
        public InnovationWindows()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            innovation = new InnovationCompetition();
            innovation.Start();
        }
    }
}
