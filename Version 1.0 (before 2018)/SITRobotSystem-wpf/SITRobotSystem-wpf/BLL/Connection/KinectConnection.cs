using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.BLL.Connection
{
    public class KinectConnection
    {

        public VisionHub visionHub;


        private Publisher NewSurfWindowPublisher;
        private Publisher EndSurfWindowPublisher;
        private Publisher ShowSurfImgPublisher;
        private Publisher IsKinectReadyPublisher;
        private Publisher NewBodyWindowPublisher;
        private Publisher startCoordinateMappingPublisher;
        private Publisher endCoordinateMappingPublisher;
        private Publisher startDepthReaderPublisher;
        private Publisher endDepthReaderPublisher;

        private Publisher NewMachineLearningPublisher;
        private Publisher EndMachineLearningPublisher;
        private Publisher startMachineLearningCoordinateMappingPublisher;
        private Publisher endMachineLearningCoordinateMappingPublisher;

        private KinectConnection()
        {

            NewSurfWindowPublisher = new Publisher();
            NewSurfWindowPublisher.OnPublish += WindowCtrl.ShowSurfWindow;
            NewBodyWindowPublisher=new Publisher();
            NewBodyWindowPublisher.OnPublish+=WindowCtrl.startBodyDetect;
            startCoordinateMappingPublisher=new Publisher();
            startCoordinateMappingPublisher.OnPublish += WindowCtrl.startCoordinateMapping;
            endCoordinateMappingPublisher=new Publisher();
            endCoordinateMappingPublisher.OnPublish += WindowCtrl.stopCoordinateMapping;

            NewMachineLearningPublisher = new Publisher();
            NewMachineLearningPublisher.OnPublish += WindowCtrl.StartMachineLearingTestWindow;
            //EndMachineLearningPublisher = new Publisher();
            //EndMachineLearningPublisher.OnPublish += WindowCtrl.EndMachineLearning;
            startMachineLearningCoordinateMappingPublisher = new Publisher();
            startMachineLearningCoordinateMappingPublisher.OnPublish +=WindowCtrl.StartMachineLearningMapping;
            endMachineLearningCoordinateMappingPublisher = new Publisher();
            endMachineLearningCoordinateMappingPublisher.OnPublish += WindowCtrl.EndMachineLearningMapping;

            //EndSurfWindowPublisher=new Publisher();
            //EndSurfWindowPublisher.OnPublish += new Publisher.PublishEventHander(WindowCtrl.CloseSurfWindow);
            //ShowSurfImgPublisher=new Publisher();
            //ShowSurfImgPublisher.OnPublish+=new Publisher.PublishEventHander(WindowCtrl.ShowSurfImg);
            IsKinectReadyPublisher=new Publisher();
            IsKinectReadyPublisher.OnPublish += WindowCtrl.SyncKinectCorrentState;
            startDepthReaderPublisher=new Publisher();
            startDepthReaderPublisher.OnPublish += WindowCtrl.startDepthScan;
            endDepthReaderPublisher = new Publisher();
            endDepthReaderPublisher.OnPublish += WindowCtrl.endDepthScan;


        } //构造函数在其它地方不可实例化了


        public void initConnection()
        {
            visionHub = new VisionHub("127.0.0.1", 4500);
            visionHub.readyCommand();
        }
        private static volatile KinectConnection _instance; //声明类，只能在自己内部实现了

        public static KinectConnection GetInstance() //判断是否有实例化过，确保只有一个
        {
            if (_instance == null)
            {
                _instance = new KinectConnection();
            }
            return _instance;
        }


       
        public BodyFrameReader getBodyPublisher()
        {
            return WindowCtrl.bodyWindow.bodyFrameReader;
        }

        public CameraSpacePoint findUser(string Name)
        {
            CameraSpacePoint res = new CameraSpacePoint();
            return res;
        }

        public User recognizeUser(int type)
        {
            User resUser = new User();

            return resUser;
        }

        public List<User> getAllUsers()
        {
            List<User> users=new List<User>();
            while (!UserTracker.isBodyReady)
            {
                users = UserTracker.users;
            }
            //List<User> users = UserTracker.users;
            return users;
        }

        

        public void StartMutipleFrame()
        {
            NewSurfWindowPublisher.issue();
        }

        public void StartMutipleFrameMachineLearning()
        {
            NewMachineLearningPublisher.issue();
        }

        public void EndMutipleFrameMachineLearning()
        {
            NewMachineLearningPublisher.issue();
        }

        public BitmapSource surfImgBitmapSource;
        private delegate void ShowSurfImgDelegate(BitmapSource bitmapSource);

        public void ShowSurfImg(Image<Bgr, byte> img,string surfResStr)
        {
            WindowCtrl.surfWindow.showImg(img, surfResStr);
            //ShowSurfImgDelegate showSurfImgDelegate =WindowCtrl.ShowSurfImg;
            //showSurfImgDelegate(bitmapSource);
        }

        public void setSurfDistance(float dis)
        {
            WindowCtrl.surfWindow.setSurfDistance(dis);
        }
        public string ScreenShot()
        {

            var screenShotPath = WindowCtrl.KinectScreenShortForSurf();

            return screenShotPath;
        }

        public string ScreenShotForMachineLearning()
        {
            return WindowCtrl.KinectScreenShortForMachineLearning();
        }

        public void endSURF()
        {
            WindowCtrl.surfWindow.SetSurfFlag(false);
        }

        public bool isKinectReady()
        {
            IsKinectReadyPublisher.issue();
            return WindowCtrl.KinectState;
        }


        public void startBodyDetect()
        {
            NewBodyWindowPublisher.issue();
            
        }

        public bool isBodyDetectReady()
        {
            IsKinectReadyPublisher.issue();
            return WindowCtrl.BodyState;
        }

        public void SetDepthReaderXrange(int Xrange)
        {
            WindowCtrl.depthWindow.setXline(Xrange);
        }

        //设置机器人的
        public void SetDepthReaderRobotInfo(float KINECT_HIGHT, float TOTAL_HIGHT, float SAFE_DISTANCE)
        {
            WindowCtrl.depthWindow.SetRobotInfo(KINECT_HIGHT, TOTAL_HIGHT, SAFE_DISTANCE);
        }
        public int GetDepthReaderUnSafeCount()
        {
            return WindowCtrl.depthWindow.getUnsafeCount();
        }

        /// <summary>
        /// 用作mutipleframe中取出color点的坐标的delegate
        /// </summary>
        /// <param name="colorSpacePoint"></param>
        /// <returns></returns>

        private delegate CameraSpacePoint MutipleFrameGetCameraPointDelegate(ColorSpacePoint colorSpacePoint);

        public CameraSpacePoint GetCameraSpacePoint(ColorSpacePoint colorSpacePoint)
        {
            MutipleFrameGetCameraPointDelegate GetCameraPointDelegate = WindowCtrl.GetCameraSpacePoint;
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            cameraPoint=GetCameraPointDelegate(colorSpacePoint);
            return cameraPoint;
        }

        private delegate CameraSpacePoint MutipleFrameGetCameraPointForMachieLearningDelegate(float  X, float  Y);

        public CameraSpacePoint GetCameraSpacePointForMachineLearning(float X,float  Y)
        {
            MutipleFrameGetCameraPointForMachieLearningDelegate GetCameraPointDelegate = WindowCtrl.GetCameraSpacePointForMachineLearning;
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            cameraPoint = GetCameraPointDelegate(X,Y);
            return cameraPoint;
        }

        public ColorSpacePoint GetColorSpacePoint(CameraSpacePoint cameraSpacePoint)
        {
            ColorSpacePoint resColorSpacePoint= KinectProcesser.GetInstance().CameraToColor(cameraSpacePoint);
            return resColorSpacePoint;
        }

        public void startCoordinateMapping()
        {
            startMachineLearningCoordinateMappingPublisher.issue();
        }

        public void endCoordinateMapping()
        {
            startMachineLearningCoordinateMappingPublisher.issue();
        }

        public void startMachineLearningCoordinateMapping()
        {
            startMachineLearningCoordinateMappingPublisher.issue();
        }

        public void endMachineLearningCoordinateMapping()
        {
            endMachineLearningCoordinateMappingPublisher.issue();
        }

        public void flashCamShift()
        {
            visionHub.readyCommand();
        }
        public void SendBodyPoints(ColorSpacePoint[] bodyPoint)
        {
            
            visionHub.SetBody(bodyPoint);
        }
        public ColorSpacePoint[] GetResultPoints()
        {
            ColorSpacePoint[] colorSpacePoints = new ColorSpacePoint[2];
            string s= visionHub.GetRes();
            string[] s1=s.Split('#');
            if (s1[0]=="body")
            {
                colorSpacePoints[0].X = int.Parse(s1[1].Split('@')[0]);
                colorSpacePoints[0].X = int.Parse(s1[1].Split('@')[1]);
                colorSpacePoints[1].X = int.Parse(s1[1].Split('@')[0]);
                colorSpacePoints[1].X = int.Parse(s1[1].Split('@')[1]);


            }
            return colorSpacePoints;
        }

        public void StartOCR()
        {
            visionHub.startOCR();
        }

        public string  readOCR()
        {
            string s = visionHub.ReadOCR();
            return s;
        }

        public string startface()
        {
            string s = visionHub.facedetect();
            return s;
        }
        public string Tcp_exit()
        {
            string s = visionHub.tcp_exit();
            return s;
        }
        public void startDepthReader()
        {
           startDepthReaderPublisher.issue();
        }
        public void endDepthReader()
        {
            endDepthReaderPublisher.issue();
        }

        internal void StartSurf()
        {
            WindowCtrl.surfWindow.SetSurfFlag(true);
        }

        internal void StartSurf(bool surFlag, bool bgFlag)
        {
            WindowCtrl.surfWindow.SetSurfFlag(surFlag,bgFlag);
        }
    }
}

