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
using SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows;
using SITRobotSystem_wpf.SITRobotWindow.Other.CompetitionWindows;
using MORecognitionWindow = SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows.MORecognitionWindow;
using SITRobotSystem_wpf.BLL.Competitions.Huawei;
using System.Threading;

namespace SITRobotSystem_wpf.SITRobotWindow.MainWindows
{
    /// <summary>
    /// CompetitionMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompetitionMainWindow : Window
    {
        public CompetitionMainWindow()
        {
            InitializeComponent();
        }

        private void BtnNavigationTest_Click(object sender, RoutedEventArgs e)
        {
            //stateWindow sWindow = new stateWindow();
            //sWindow.Show();
            NavigationTestWindow navigationTestWindow = new NavigationTestWindow();
            navigationTestWindow.Show();
        }

        private void BtnMORecognition_Click(object sender, RoutedEventArgs e)
        {
            new MORecognitionWindow().Show();
        }

        private void BtnRobo_Nures_2015_Click(object sender,RoutedEventArgs e)
        {
            Robo_Nures_2015Window robo_Nurse_2015Window = new Robo_Nures_2015Window();
            robo_Nurse_2015Window.Show();
        }

        private void BtnGPSR_2015_Click(object sender, RoutedEventArgs e)
        {
            GPSR_2015Window gPSR_2015Window = new GPSR_2015Window();
            gPSR_2015Window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpeechRecognitionAudioDetectionWindow SRAD_2015Window = new SpeechRecognitionAudioDetectionWindow();
            SRAD_2015Window.Show();
        }

        private void Btn_Restaurant_Click(object sender, RoutedEventArgs e)
        {
            Restaurant2015Window R2015Window=new Restaurant2015Window();
            R2015Window.Show();
        }

        private void Btn_shopping_Click(object sender, RoutedEventArgs e)
        {
            shoppingWindow R2016Window = new shoppingWindow();
            R2016Window.Show();
        }

        private void BtnWakeMeUp_Click(object sender, RoutedEventArgs e)
        {
            new WakeMeUpWindow().Show();
        }

        private void BtnPerson_Click(object sender, RoutedEventArgs e)
        {
            new PersonRecognitionWindow().Show();
        }

        private void Btn_OC_Click(object sender, RoutedEventArgs e)
        {
            OpenChallengeWindows OCWindow=new OpenChallengeWindows();
            OCWindow.Show();
        }

        private void Btn_Zoo_Click(object sender, RoutedEventArgs e)
        {
            RoboZoo2015Windows zoo2015=new RoboZoo2015Windows();
            zoo2015.Show();
        }

        public HuaweiWindow hw;
        //public delegate void Start_Video_Del(string command);
        public delegate void Endt_Video_Del(object sender, RoutedEventArgs e);
        private void Btn_Huawei_Click(object sender,RoutedEventArgs e)
        {
            hw = new HuaweiWindow();
            hw.hwCompetition.hwStages.hwTask.Video_Event += new Start_Video_Del(hw.PlayVideo_Handler);
            hw.Media.MediaEnded += hw.hwCompetition.hwStages.hwTask.Ended_Handler;
            //hw.Media.MediaEnded += new Endt_Video_Del(hw.EndVideo_Handler);
            hw.Show();
            hw.Topmost = true;
        }


        //FollowWindow fw;
        //private void follow_Click(object sender, RoutedEventArgs e)
        //{
        //    fw = new FollowWindow();
        //    fw.Show();
        //}
        FollowWindow fw;
        private void follow_Click_1(object sender, RoutedEventArgs e)
        {
            fw = new FollowWindow();
            fw.Show();
        }
        WhoisWhoWindow whoisWhoWindow;
        private void WhoisWho_Click(object sender, RoutedEventArgs e)
        {
            whoisWhoWindow = new WhoisWhoWindow();
            whoisWhoWindow.Show();
        }

        private void SetTable_Click(object sender, RoutedEventArgs e)
        {
            SetTable2017Window setTable2017Window = new SetTable2017Window();
            setTable2017Window.Show();
        }

        private void HelpMeCarry_Click(object sender, RoutedEventArgs e)
        {
            HelpMeCarryWindow helpMeCarryWindow = new HelpMeCarryWindow();
            helpMeCarryWindow.Show();
        }

        private void FuckingSurprise_Click(object sender, RoutedEventArgs e)
        {
            FuckingSurprise fuckingsurprise = new FuckingSurprise();
            fuckingsurprise.Show();
        }
    }
}
