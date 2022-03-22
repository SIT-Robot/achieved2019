using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using PointF = System.Drawing.PointF;
using System.Drawing;

#if !IOS

#endif
namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{
    /// <summary>
    /// SURFWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SURFWindow : Window
    {

        private MutipleFrame mutipleFrame;
        private string objPathStr;
        private string scenePathStr;
        private int GracutCount;

        //protected VisionCtrl visionCtrl = new VisionCtrl();
        public SURFWindow()
        {
            //rect = new Rectangle(300, 500, 1300, 580);
            mutipleFrame = new MutipleFrame();
            GracutCount = 0;
            //mutipleFrame.multiFrameSourceReader.MultiSourceFrameArrived += showImg;
            InitializeComponent();
            tx1.Text = "300";
            tx2.Text = "500";
            tx3.Text = "1300";
            tx4.Text = "400";
        }

        private Image<Bgr, byte> colorFrameImg;
        private byte[] pixels = null;

        public bool SurfFlag=false;

        public void SetSurfFlag(bool flag)
        {
            SurfFlag = flag;
            mutipleFrame.NeedBackGroundClear = flag;
        }

        public void SetSurfFlag(bool surFlag,bool bgFlag)
        {
            SurfFlag = surFlag;
            mutipleFrame.NeedBackGroundClear = bgFlag;
        }

        public void showImg(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            //this.SurfImg.Source = mutipleFrame.GetBitmapSource();
        }


        public bool IsKinectRunning()
        {
            return mutipleFrame.isKinectReady;
        }

        public void startMutipleFrame()
        {
            
        }
        public CameraSpacePoint FindObj(Goods goods, int type)
        {
            
            BitmapSource surfresBitmapSource;
            if (!mutipleFrame.KinectReady())
            {
                return new CameraSpacePoint();
            }

            ColorSpacePoint resColorCenterPoint = new ColorSpacePoint();
            CameraSpacePoint resCameraCenterPoint = new CameraSpacePoint();
            CameraSpacePoint[] resCameraPoints =new CameraSpacePoint[4];
            
            //获取图像
            string KinethImgPath = mutipleFrame.getscreenShortPath();
            if (KinethImgPath == "")
            {
                return resCameraCenterPoint;
            }
            while (FileManager.getFileSize(KinethImgPath) < 200)
            {
                KinethImgPath = mutipleFrame.getscreenShortPath();
                Thread.Sleep(500);
            }

            string objPathStr = goods.imgPath[0];
            if (string.IsNullOrWhiteSpace(objPathStr))
            {
                return resCameraCenterPoint;
            }

            string scenePathStr = KinethImgPath;
            if (string.IsNullOrWhiteSpace(scenePathStr))
            {
                return resCameraCenterPoint;
            }            

            long matchTime = 0;

            Image<Bgr, Byte> kinectImage = colorFrameImg;
            using (Image<Gray, Byte> modelImage = new Image<Gray, byte>(objPathStr))
            using (Image<Gray, Byte> observedImage = new Image<Gray, byte>(scenePathStr))
            {
                Image<Bgr, byte> result = SurfFeature.Draw(modelImage, observedImage, out matchTime);
                SurfImg.Source= surfresBitmapSource = BitmapSourceConvert.ToBitmapSource(result);

                string resString = String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime);
                resString += ("  find" + SurfFeature.matchPoints);

                if (SurfFeature.matchPoints > 10)
                {
                    resColorCenterPoint.X = (SurfFeature.ObjectPts[0].X + SurfFeature.ObjectPts[1].X +
                                        SurfFeature.ObjectPts[2].X + SurfFeature.ObjectPts[3].X)/4;
                    resColorCenterPoint.Y = (SurfFeature.ObjectPts[0].Y + SurfFeature.ObjectPts[1].Y +
                                        SurfFeature.ObjectPts[2].Y + SurfFeature.ObjectPts[3].Y)/4;
                }
                resCameraCenterPoint = mutipleFrame.GetCameraSpacePoint(resColorCenterPoint);
                //resCameraPoints[0]=
                

                resString += "Color:" + resColorCenterPoint.X + "  " + resColorCenterPoint.Y;
                resString += "CAM:" + resCameraCenterPoint.X +"  "+ resCameraCenterPoint.Y + "  "+resCameraCenterPoint.Z;
                TBSURFres.Text = resString;

            }
            
            return resCameraCenterPoint;
        }

        /// <summary>
        /// 判断四个点所组成的图形是否近似为方形
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private bool judgeRect(PointF[] points)
        {
            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mutipleFrame.setDistance(float.Parse(distanceTB.Text));
            Thread thread = new Thread(findObject);
            thread.Start();

        }
        
        private void findObject()
        {
            StartCoordinateMapping();
            
            if (!FileManager.isFileExist(objPathStr))
            {
                return;
            }
            VisionCtrl vision = new VisionCtrl();
            startMutipleFrame();
            ObjCameraPosition campoPoint = new ObjCameraPosition();
            Goods goods = new Goods();
            DBCtrl dbCtrl = new DBCtrl();

            //goods = dbCtrl.GetGoodsByName("green tea");
            goods.imgPath.Add(objPathStr);
            SurfResult surfResult = vision.findObject(goods);



            stopCoordinateMapping();
        }
        public void ShowBitmap(BitmapSource bitmapSource)
        {
            SurfImg.Source = bitmapSource;
        }

        public string GetScreenShort()
        {
            string screenShotPath = null;
            mutipleFrame.screenShotFlag=true;
            while (mutipleFrame.isScreenShotSuccess)
            {
                screenShotPath= mutipleFrame.screenShotPath;
                mutipleFrame.isScreenShotSuccess = false;
                
            }
            return screenShotPath;
        }

        public CameraSpacePoint GetCameraSpacePoint(ColorSpacePoint colorSpacePoint)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();

            cameraPoint=mutipleFrame.GetCameraSpacePoint(colorSpacePoint);
            return cameraPoint;
        }

        private BitmapSource surfBitmapSource = null;
        public void showImg(Image<Bgr, byte> img,string res)
        {
                Dispatcher.Invoke(() =>
                {
                    TBSURFres.Text = "find";
                    SurfImg.Source = BitmapSourceConvert.ToBitmapSource(img);
                });
        }

        private void BtnloadObjPath_Click(object sender, RoutedEventArgs e)
        {
            TBObjPath.Text=FileManager. OpenImgFile();
            objPathStr = TBObjPath.Text.Length.Equals(0) ? "greentea1.png" : TBObjPath.Text;
        }



        private void BtnLoadSencePath_Click(object sender, RoutedEventArgs e)
        {
            TBSencePath.Text = FileManager.OpenImgFile();
            scenePathStr = TBSencePath.Text.Length.Equals(0) ? "KinectScreenshot.png" : TBSencePath.Text;
        }

 
        private void BtnOnlySurf_Click(object sender, RoutedEventArgs e)
        {
            VisionCtrl vision = new VisionCtrl();
            objPathStr = TBObjPath.Text.Length.Equals(0) ? "greentea1.png" : TBObjPath.Text;
            scenePathStr = TBSencePath.Text.Length.Equals(0) ? "KinectScreenshot.png" : TBSencePath.Text;

            int x = 0, y = 0, weight = 0, high = 0;
            Rectangle rect = new Rectangle();
            if (!tx1.Text.Length.Equals(0) ||
                !tx2.Text.Length.Equals(0) ||
                !tx3.Text.Length.Equals(0) ||
                !tx4.Text.Length.Equals(0))
            {
                x = int.Parse(tx1.Text);
                y = int.Parse(tx2.Text);
                weight = int.Parse(tx3.Text);
                high = int.Parse(tx4.Text);
            }
            if (x > 0 && y > 0 && weight > 0 && high > 0)
                rect = new Rectangle(x, y, weight, high);
            else
                rect = new Rectangle(300, 500, 1300, 580);

            long matchTime;
            Image<Bgr, Byte> resultImg = vision.GrabcutImg(scenePathStr, rect);
            
            using (Image<Gray, Byte> modelImage = new Image<Gray, byte>(objPathStr))
            using(Image<Gray, Byte> observedImage = resultImg.Convert<Gray, Byte>())
            {
                Image<Bgr, byte> result = SurfFeature.Draw(modelImage, observedImage, out matchTime);
                SurfImg.Source = BitmapSourceConvert.ToBitmapSource(result);

                string resString = String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime);
                resString += ("  find" + SurfFeature.matchPoints);
                TBSURFres.Text = resString;
            }
        }

        public void stopCoordinateMapping()
        {
            mutipleFrame.coordinateMappingFlag = false;
        }
        public void StartCoordinateMapping()
        {
            mutipleFrame.coordinateMappingFlag = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mutipleFrame.stop();
        }

        public void setSurfDistance(float dis)
        {
            mutipleFrame.setDistance(dis);
        }

        private void Grabcut_Click(object sender, RoutedEventArgs e)
        {
            GracutCount++;
            int x=0, y=0, weight=0, high=0;
            Rectangle rect = new Rectangle();
            if (!tx1.Text.Length.Equals(0) ||
                !tx2.Text.Length.Equals(0) ||
                !tx3.Text.Length.Equals(0) ||
                !tx4.Text.Length.Equals(0))
            {
                x = int.Parse(tx1.Text);
                y = int.Parse(tx2.Text);
                weight = int.Parse(tx3.Text);
                high = int.Parse(tx4.Text);
            }
            if (x > 0 && y > 0 && weight > 0 && high > 0)
                rect = new Rectangle(x, y, weight, high);
            else
                rect = new Rectangle(300, 500, 1300, 580);
            scenePathStr = TBSencePath.Text.Length.Equals(0) ? "KinectScreenshot.png" : TBSencePath.Text;
            VisionCtrl vision = new VisionCtrl();
            Image<Bgr, byte> result = vision.GrabcutImg(scenePathStr, rect, GracutCount);
            SurfImg.Source = BitmapSourceConvert.ToBitmapSource(result);
        }

        private void SurfWithKinect2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
