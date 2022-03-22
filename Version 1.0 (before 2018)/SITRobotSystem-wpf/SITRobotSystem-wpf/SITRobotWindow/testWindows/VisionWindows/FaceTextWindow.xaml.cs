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

using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Microsoft.Kinect;

using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{
    /// <summary>
    /// faceText.xaml 的交互逻辑
    /// </summary>
    public partial class FaceTextWindow : Window
    {
        private MutipleFrame mutipleFrame;
        private string objPathStr;
        private string scenePathStr;
        private string KinethImgPath;
        private VisionCtrl vision;
        private Image<Gray, Byte> objImg;
        SurfResult[] surfResult;

        public FaceTextWindow()
        {
            mutipleFrame = new MutipleFrame();
            vision = new VisionCtrl();
            InitializeComponent();
            GetScreenShort();
            btSave.IsEnabled = false;
            btRecognition.IsEnabled = false;
        }

        private void face_Click(object sender, RoutedEventArgs e)
        {
            surfResult = vision.FindFace(mutipleFrame.screenShotPath);
            //surfResult = vision.FindFace("D:\\VS2010旗舰版\\program\\人脸识别\\222.png");
            if (surfResult[0].isSuccess)
                objImg = new Image<Gray, byte>(surfResult[0].ImgPath);
            else
                objImg = new Image<Gray, byte>(mutipleFrame.screenShotPath);
            objImg._EqualizeHist();
            faceImg.Source = BitmapSourceConvert.ToBitmapSource(objImg);
            btSave.IsEnabled = true;
            btRecognition.IsEnabled = true;
            GetScreenShort();
        }

        public void GetScreenShort()
        {
            mutipleFrame.screenShotFlag = true;
            while (mutipleFrame.isScreenShotSuccess)
            {
                KinethImgPath = mutipleFrame.screenShotPath;
                mutipleFrame.isScreenShotSuccess = false;
            }
        }

        public bool IsKinectRunning()
        {
            return mutipleFrame.isKinectReady;
        }

        private void recognition_Click(object sender, RoutedEventArgs e)
        {
            String name = vision.FindFaceName(objImg);
            txfaceName.Text = name;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (txfaceName.Text != null)
                vision.SaveTrainedFace(objImg, txfaceName.Text);
            else
                System.Console.WriteLine("请输入名字");
        }
    }
}
