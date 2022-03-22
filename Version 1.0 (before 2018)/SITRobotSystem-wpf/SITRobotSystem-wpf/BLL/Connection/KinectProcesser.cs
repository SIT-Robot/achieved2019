using System;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Connection
{
    public class KinectProcesser
    {
        /// <summary>
        /// 构造函数私有化,禁止在其他地方实例化改类
        /// </summary>
        private KinectProcesser()
        {
            kinectSensor = KinectSensor.GetDefault();

        }               //构造函数在其它地方不可实例化了

        private static volatile KinectProcesser instance;  //声明类，只能在自己内部实现了

        public static KinectProcesser GetInstance()        //判断是否有实例化过，确保只有一个
        {
            if (instance == null)
            {
                instance = new KinectProcesser();
            }
            return instance;
        }

        public CoordinateMapper coordinateMapper;

        public KinectSensor kinectSensor;

        public void Init(ref KinectSensor _kinectSensor)
        {
            kinectSensor = _kinectSensor;
        }
        public ColorSpacePoint CameraToColor(CameraSpacePoint cameraPoint)
        {
            if (kinectSensor != null)
            {
                ColorSpacePoint colorPoint = kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(cameraPoint);
                return colorPoint;
            }
            throw new Exception("KinectProcesser未初始化");
        }

        public DepthSpacePoint CameraToDepth(CameraSpacePoint cameraPoint)
        {
            if (kinectSensor != null)
            {
                DepthSpacePoint depthPoint = kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(cameraPoint);
                return depthPoint;
            }
            throw new Exception("KinectProcesser未初始化");
        }

        public CameraSpacePoint ColorToCamera(ColorSpacePoint colorPoint)
        {
            if (kinectSensor != null)
            {
                CameraSpacePoint cameraPoint=new CameraSpacePoint();
                
                //kinectSensor.CoordinateMapper.MapColorFrameToCameraSpace();
                CameraSpacePoint cameraSpacePoint;
                
                return cameraPoint;
            }
            throw new Exception("KinectProcesser未初始化");
        }

        public void depth2color()
        {

        }

    }
}
