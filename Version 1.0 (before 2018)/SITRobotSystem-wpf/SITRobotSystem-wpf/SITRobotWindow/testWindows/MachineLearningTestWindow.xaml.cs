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
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Service;
using System.Threading;
using SITRobotSystem_wpf.BLL.FunctionClass;
using System.IO;
using SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows;
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.entity;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// Interaction logic for MachineLearningTestWindow.xaml
    /// </summary>
    public partial class MachineLearningTestWindow : Window
    {
        public MutipleFrame mutipleFrame;
        VisionCtrl visionCtrl;
        string path1;

        string Thingtofindtext;
        public MachineLearningTestWindow()
        {

            InitializeComponent();
            ipbox.Text = MachineLearningConnection.IPADDRESS;

            portbox.Text = MachineLearningConnection.PORT.ToString();
            mutipleFrame = new MutipleFrame();

        }

        public CameraSpacePoint GetCameraSpacePointForML(float pixels_X, float pixels_Y)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            ColorSpacePoint csp = new ColorSpacePoint();
            csp.X = pixels_X;
            csp.Y = pixels_Y;
            cameraPoint = mutipleFrame.GetCameraSpacePoint(csp);
            return cameraPoint;
        }

        public void startMapping()
        {
            mutipleFrame.coordinateMappingFlag = true;
        }

        public void endMapping()
        {
            mutipleFrame.coordinateMappingFlag = false;
        }

        public string GetScreenShotForMachineLearning()
        {
            string screenShotPath = null;
            mutipleFrame.screenShotFlag = true;
            while (screenShotPath == null)
            {
                while (mutipleFrame.isScreenShotSuccess)
                {
                    screenShotPath = mutipleFrame.screenShotPath;
                    mutipleFrame.isScreenShotSuccess = false;

                }
            }
            return screenShotPath;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void getjsonbutton_Click(object sender, RoutedEventArgs e)
        {
            MachineLearningConnection.IPADDRESS = ipbox.Text;
            MachineLearningConnection.PORT = int.Parse(portbox.Text);
            string RESULT = MachineLearningConnection.StartAndGetResult();
            Console.Write(RESULT);
            MessageBox.Show("Succeed,JSON(Also showed in Console):" + System.Environment.NewLine + RESULT);

        }

        private void kinectbutton_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// 拍照交给linux处理
            /// </summary>
            /// 
            //visionCtrl = new VisionCtrl();
            Thread thread = new Thread(GetScreenShort);
            thread.Start();
            //morecognitionTask.moveDirectionBySpeed(0f, -0.8f);
            //path2 = morecognitionTask.getCorrentImg();
            //morecognitionTask.visionCtrl.ChangePath(path1, "2.png");
            //ImgPath.Add(path2);

        }

        public void GetScreenShort()
        {

            while (path1 == null)
            {
                mutipleFrame.screenShotFlag = true;
                while (mutipleFrame.isScreenShotSuccess)
                {
                    path1 = mutipleFrame.screenShotPath;
                    mutipleFrame.screenShotFlag = false;
                    mutipleFrame.isScreenShotSuccess = false;
                }
            }
            Console.WriteLine("IMG PATH=" + path1);


            //Thread.Sleep(1000);
            List<string> ImgPath = new List<string>();
            //path1 = mutipleFrame.screenShort();
            visionCtrl = new VisionCtrl();

            visionCtrl.ChangePath(path1, "Y:\\0.jpg");
            MessageBox.Show("Take Photo Finished");
            //morecognitionTask.visionCtrl.ChangePath("D:\\作业\\机协\\大赛用代码\\捕捉\\2017-05-19\\P70519-184929.jpg", "Z:\\1.jpg");
            ImgPath.Add(path1);
            path1 = null;

        }

        private void processJSON_Click(object sender, RoutedEventArgs e)
        {
            VisionCtrl vc = new VisionCtrl();
            MachineLearingResult result = vc.FindObjectMachineLearing(THINGTOFINDtextBox.Text);
            MessageBox.Show("name:" + result.Name + " X=" + result.CameraSpacePoint[result.ThingsNumber - 1].X + " Y=" + result.CameraSpacePoint[result.ThingsNumber - 1].Y + " Z=" + result.CameraSpacePoint[result.ThingsNumber - 1].Z);
        }

        private void TestFind_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start");
            Thingtofindtext = thingName.Text;

            Thread thread = new Thread(lookfor);
            thread.Start();

        }

        public void lookfor()
        {
            Tasks task = new Tasks();
            task.lookToGoodsMachineLearing(Thingtofindtext);
        }

        public void findtest()
        {
            Tasks task = new Tasks();
            bool success = task.lookToGoodsMachineLearing(Thingtofindtext, this);
        }

        public bool findForGPSR(string thing)
        {
            Tasks task = new Tasks();
            return task.lookToGoodsMachineLearing(thing, this);
        }

    }
}
