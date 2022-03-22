/* 相机空间坐标 通过解析深度学习传来的json文件，保存物体的实际位置。
 * 用到 Microsoft.Kinect.CameraSpacePoint 位置用米为单位；
 * X，Y, Z 横纵深；
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
namespace i_Shit_Core.Library
{
    class CameraSpaceInfo
    {
        public CameraSpacePoint[] _camera_space_point;
        public float[] _confidence;
        public int _stuff_number;
        public string _stuff_name;

        public CameraSpaceInfo(int stuff_number_)
        {
            _camera_space_point = new Microsoft.Kinect.CameraSpacePoint[stuff_number_];//有多少个物体就有多少物体空间坐标
            _stuff_number = stuff_number_;//保存物体数量
            _confidence = new float[stuff_number_]; //每个物体都需要一个可信度
        }
    }
}
