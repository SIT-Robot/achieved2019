using System.Windows;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.SITRobotWindow.testWindows;
using SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows;
using SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows;
using System.Threading;

namespace SITRobotSystem_wpf.SITRobotWindow.MainWindows
{
    /// <summary>
    /// TestMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestMainWindow : Window
    {
        private DBTestWindow dbTestwin;
        //private WindowCtrl windowCtrl;
        public TestMainWindow()
        {
            //windowCtrl = new WindowCtrl();
            InitializeComponent();
        }

        private void BTN_DBTEST_Click(object sender, RoutedEventArgs e)
        {
            dbTestwin=new DBTestWindow();
            dbTestwin.Show();
        }

        private void btnSURF_Click(object sender, RoutedEventArgs e)
        {
            KinectConnection.GetInstance().StartMutipleFrame();

        }

        private HandTest handTest;
        private void BtnHand_Click(object sender, RoutedEventArgs e)
        {
            handTest=new HandTest();
            handTest.Show();
            
        }

        private SpeechTestForm speechTestForm;
        private void BtnSpeech_Click(object sender, RoutedEventArgs e)
        {
            speechTestForm=new  SpeechTestForm();
            speechTestForm.Show();
        }

        private BaseTestWindow baseTestWindow;
        private void BtnBase_Click(object sender, RoutedEventArgs e)
        {
            baseTestWindow=new BaseTestWindow();
            baseTestWindow.Show();
        }

        private GoodsTestWindow goodsTestWindow;
        private void BtnGoodsTest_Click(object sender, RoutedEventArgs e)
        {
            goodsTestWindow=new GoodsTestWindow();
            goodsTestWindow.Show();
        }

        private ColorFrameCVwindow colorFrameCVwindow;
        private void BtnColorCV_Click(object sender, RoutedEventArgs e)
        {
            colorFrameCVwindow=new ColorFrameCVwindow();
            colorFrameCVwindow.Show();
        }

        private RobotConfigWindow robotConfigWindow;
        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            robotConfigWindow = new RobotConfigWindow();
            robotConfigWindow.Show();

        }

        private void BtnImagePDF_Click(object sender, RoutedEventArgs e)
        {
            ImageTestWindow im = new ImageTestWindow();
            im.Show();
        }

        private FaceTextWindow faceTextWindow;
        private void Face_Click(object sender, RoutedEventArgs e)
        {
            faceTextWindow = new FaceTextWindow();
            faceTextWindow.Show();
        }

        private void AudioDetection_Click(object sender, RoutedEventArgs e)
        {
            AudioAngleTest angleWindow = new AudioAngleTest();
            angleWindow.Show();
        }

        private void MachLearn_Click(object sender, RoutedEventArgs e)
        {
            //Thread thread = new Thread(KinectConnection.GetInstance().StartMutipleFrameMachineLearning);
            //thread.Start();
            KinectConnection.GetInstance().StartMutipleFrameMachineLearning();
        }

        private void MutipelColor_Click(object sender, RoutedEventArgs e)
        {
            MachineLearingWindow machineLearningWindow = new MachineLearingWindow();
            machineLearningWindow.Show();
        }
    }
}
