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
using SITRobotSystem_wpf.BLL.Competitions.Speech_Recognition_and_Audio_Detection_Test;

namespace SITRobotSystem_wpf.SITRobotWindow.Other.CompetitionWindows
{
    /// <summary>
    /// SpeechRecognitionAudioDetectionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SpeechRecognitionAudioDetectionWindow : Window
    {
        public SpeechRecognitionAudioDetectionWindow() 
        {
            InitializeComponent();
        }

        private SRAD_Competition SradCompetition;
        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            SradCompetition = new SRAD_Competition();
            SradCompetition.Start();
        }
    }
}
