using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using SITRobotSystem_wpf.SITRobotWindow.testWindows;
using SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows;

namespace SITRobotSystem_wpf.UI
{
    class WindowCtrl
    {
        /// <summary>
        /// 用户信息界面
        /// </summary>
        public static UsersInfoWindow userInfoWindow;

        /// <summary>
        /// 骨架信息界面
        /// </summary>
        public static BodyWindow bodyWindow;

        /// <summary>
        /// 图像界面
        /// </summary>
        public static ColorWindow colorWindow;

        /// <summary>
        /// 深度图像界面
        /// </summary>
        public static DepthWindow depthWindow;


        /// <summary>
        /// 是否开启骨架检测flag
        /// </summary>
        protected static bool isBodyStart;

        /// <summary>
        /// 是否开启图像检测flag
        /// </summary>
        protected bool isColortart;

        /// <summary>
        /// 是否开启深度检测flag
        /// </summary>
        protected static bool isDepthStart;

        /// <summary>
        /// 开始身体检测
        /// </summary>
        public static void startBodyDetect()
        {
            bodyWindow = new BodyWindow();
            userInfoWindow = new UsersInfoWindow();
            bodyWindow.BodyPublisher.OnPublish += userInfoWindow.UsersArrived;
            isBodyStart = true;
            bodyWindow.Show();
            userInfoWindow.Show();
        }

        /// <summary>
        /// 结束身体检测
        /// </summary>
        public void endBodyDetect()
        {
            isBodyStart = false;
            bodyWindow.Close();
            userInfoWindow.Close();
        }

        /// <summary>
        /// 开启彩色图像
        /// </summary>
        public void startColorScan()
        {
            colorWindow.Show();
            isColortart = true;
        }

        /// <summary>
        /// 结束彩色图像
        /// </summary>
        public void endcolorScan()
        {
            isColortart = false;
            colorWindow.Close();
        }

        /// <summary>
        /// 开启深度图像
        /// </summary>
        public static void startDepthScan()
        {

            isDepthStart = true;
            if (depthWindow == null) { depthWindow = new DepthWindow(); depthWindow.Show(); }
            else { depthWindow.Show(); }
            
        }

        /// <summary>
        /// 结束深度图像
        /// </summary>
        public static void endDepthScan()
        {
            isDepthStart = false;
            depthWindow.Close();
        }




        /// <summary>
        /// surf
        /// </summary>
        public static SURFWindow surfWindow;
        public static void ShowSurfWindow()
        {
            surfWindow = new SURFWindow();
            surfWindow.Show();
            
        }

        public static MachineLearningTestWindow machineLearningTestWindow; 
        public static void StartMachineLearingTestWindow()
        {
            machineLearningTestWindow = new MachineLearningTestWindow();
            machineLearningTestWindow.Show();
        }

        public static void StartMachineLearningMapping()
        {
            machineLearningTestWindow.startMapping();
        }

        public static void EndMachineLearningMapping()
        {
            machineLearningTestWindow.endMapping();
        }


        public static void CloseSurfWindow()
        {
            surfWindow.Close();
        }
        public static void ShowSurfImg(Image<Bgr, byte> img )
        {
            //surfWindow.showImg(img);
        }

        public static string screenShotPath ;

        public static string KinectScreenShortForSurf()
        {

            return surfWindow.GetScreenShort();
        }

        public static string KinectScreenShortForMachineLearning()
        {
            return machineLearningTestWindow.GetScreenShotForMachineLearning();
        }

        public static bool KinectState;
        public static bool BodyState = false;
        public static void SyncKinectCorrentState()
        {
            KinectState = surfWindow.IsKinectRunning();
            //BodyState = bodyWindow.IsBodyReady;
        }

        public static CameraSpacePoint GetCameraSpacePoint(ColorSpacePoint colorSpacePoint)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            cameraPoint=surfWindow.GetCameraSpacePoint(colorSpacePoint);   
            return cameraPoint;
        }

        public static CameraSpacePoint GetCameraSpacePointForMachineLearning(float X,float Y)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            cameraPoint = machineLearningTestWindow.GetCameraSpacePointForML(X,Y);
            return cameraPoint;
        }

        public static void startCoordinateMapping()
        {
            surfWindow.StartCoordinateMapping();
        }

        public static void stopCoordinateMapping()
        {
            surfWindow.stopCoordinateMapping();
        }


    }
}
