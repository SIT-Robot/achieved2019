using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.entity
{
    public class SurfResult
    {
        public string name;
        public ColorSpacePoint centerColorPoint=new ColorSpacePoint();
        public CameraSpacePoint centerCameraPoint=new CameraSpacePoint();
        public ColorSpacePoint[] cornerColorPoints=new ColorSpacePoint[4];
        public CameraSpacePoint[] cornerCameraPoints=new CameraSpacePoint[4];
        public long usingTime=0;
        public string resultStr = "";
        public bool isSuccess = false;
        public bool isReachable=false;
        public int numOfGoodPoint = 0;
        public string ImgPath = "";
    }
}
