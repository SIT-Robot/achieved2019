using System;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;

namespace SITRobotSystem_wpf.entity
{
    public class BodyCharacteristic
    {
        public ColorSpacePoint ShoulderLeftCol, ShoulderRightCol, HipLeftCol, HipRightCol;

    };

    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID;
        /// <summary>
        /// face
        /// </summary>
        public string face;
        /// <summary>
        /// 名字
        /// </summary>
        public string name;
        /// <summary>
        /// 身高
        /// </summary>
        public double UserHeight;
        /// <summary>
        /// 高度特征
        /// </summary>
        public double HeightCharacteristic;
        /// <summary>
        /// 身体特征点
        /// </summary>
        public BodyCharacteristic bodyCharacteristic = new BodyCharacteristic();
        /// <summary>
        /// 身体特征string
        /// </summary>
        public string bodyCharacteristicStr;
        /// <summary>
        /// 检测状态
        /// </summary>
        public int State;
        /// <summary>
        /// 身体骨架信息
        /// </summary>
        public Body body;

        public float confidence=0.5f;
        public CameraSpacePoint BodyCenter=new CameraSpacePoint();

        public bool isLeanLeft;

        public bool isLeanRight;

        public bool isLeanMiddle;

        public bool isJumping;

        public bool isSeated;

        public double seatedConfidence;
        public bool isRaisingHand;

        public bool isSquat;

        public float footConstHeight;
        
        public float hipConstHeight;

        /// <summary>
        /// 性别
        /// </summary>
        public Sex sex = Sex.UnKnown;

        /// <summary>
        /// 检测性别用，人体脖子到胸部距离差
        /// </summary>
        public float breastsize;

        public void sync()
        {
            ID = (int)body.TrackingId;
            BodyCenter = body.Joints[JointType.SpineMid].Position;
            //Head = body.Joints[JointType.Head].Position;
        }
        private static double Length(CameraSpacePoint p1, CameraSpacePoint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.X - p2.X, 2) +
                Math.Pow(p1.Y - p2.Y, 2) +
                Math.Pow(p1.Z - p2.Z, 2));
        }

        public void processUser()
        {
            trackingHeight();
            trackingHand();
            trackingHandSimple();
            //trackingJump();
            //trackingLean();
        }

        public void trackingHeight()
        {

            HeightCharacteristic =
                Length(body.Joints[JointType.Head].Position, body.Joints[JointType.Neck].Position) +
                Length(body.Joints[JointType.Neck].Position, body.Joints[JointType.SpineShoulder].Position) +
                Length(body.Joints[JointType.SpineShoulder].Position, body.Joints[JointType.SpineMid].Position) +
                Length(body.Joints[JointType.SpineMid].Position, body.Joints[JointType.SpineBase].Position);

            bodyCharacteristicStr = "A:" + bodyCharacteristic.HipLeftCol.X + "," + bodyCharacteristic.HipLeftCol.Y + ";"
                + "B:" + bodyCharacteristic.ShoulderRightCol.X + "," + bodyCharacteristic.ShoulderRightCol.Y + ";"
                + "C:" + bodyCharacteristic.HipLeftCol.X + "," + bodyCharacteristic.HipLeftCol.Y + ";"
                 + "D:" + bodyCharacteristic.HipRightCol.X + "," + bodyCharacteristic.HipRightCol.Y;
        }


        public void trackingLean()
        {
            if (body.IsTracked)
            {
                //左倾斜
                if (
                    MathPloblems.Angel(body.Joints[JointType.HipRight].Position, body.Joints[JointType.HipLeft].Position) >
                    4
                    && (body.Joints[JointType.HipRight].Position.Y > body.Joints[JointType.HipLeft].Position.Y))
                {
                    isLeanLeft = true;
                }
                else
                {
                    isLeanLeft = false;
                }

                //右倾斜
                if (
                    MathPloblems.Angel(body.Joints[JointType.HipRight].Position, body.Joints[JointType.HipLeft].Position) >
                    4
                    && body.Joints[JointType.HipLeft].Position.Y > (body.Joints[JointType.HipRight].Position.Y))
                {
                    isLeanRight = true;
                }
                else
                {
                    isLeanRight = false;
                }
                //回到中间
                if (
                    MathPloblems.Angel(body.Joints[JointType.HipRight].Position, body.Joints[JointType.HipLeft].Position) <
                    4)
                {
                    //cout<<"MMMMMMMMiddle!!!"<<endl;
                    isLeanMiddle = true;
                    isLeanLeft = false;
                    isLeanRight = false;
                }
                //俯身
                if (body.Joints[JointType.FootLeft].TrackingState == TrackingState.Tracked)
                {
                    isSquat = Math.Abs(body.Joints[JointType.Head].Position.Z - body.Joints[JointType.FootLeft].Position.Z) > 0.2;
                }
                else
                {
                    isSquat = false;
                }
            }
        }

        public void trackingHand()
        {
            if (body.IsTracked)
            {
                if (body.HandLeftState==HandState.Closed&&body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.Head].Position.Y ||
                    body.HandRightState==HandState.Closed&&body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y) 
                {
                    isRaisingHand = true;
                }
                else
                {
                    isRaisingHand = false;
                }
            }

        }

        public void trackingHandSimple()
        {
            if (body.IsTracked)
            {
                if ( body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.Head].Position.Y ||
                    body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y)
                {
                    isRaisingHand = true;
                }
                else
                {
                    isRaisingHand = false;
                }
            }

        }


        public void trackingJump()
        {

            if (body.IsTracked)
            {
                if (body.Joints[JointType.FootLeft].TrackingState == TrackingState.Tracked)
                {
                    isJumping = body.Joints[JointType.FootLeft].Position.Y - KinectInfo.FloorHeight > 0.05 ||
                                body.Joints[JointType.FootRight].Position.Y - KinectInfo.FloorHeight > 0.05;
                }
                else
                {
                    isJumping = false;
                }
            }

        }
        public bool isHandPush = false;
        public bool isHandLeftPush = false;
        public bool isHandRightPush = false;
        public void trackingHandRightPush()
        {
            if (body.IsTracked)
            {
                if ((body.Joints[JointType.HandLeft].TrackingState == TrackingState.Tracked&&body.HandLeftState==HandState.Open))
                {
                    if ((body.Joints[JointType.SpineMid].Position.Z-body.Joints[JointType.HandLeft].Position.Z>0.3))
                    {
                        isHandLeftPush = true;
                    }
                }
                else
                {
                    isHandLeftPush = false;
                }
                isHandPush = isHandLeftPush || isHandRightPush;
            }
        }
        public void trackingHandLeftPush()
        {
            if (body.IsTracked)
            {
                if ((body.Joints[JointType.HandRight].TrackingState == TrackingState.Tracked && body.HandRightState == HandState.Open))
                {
                    if ((body.Joints[JointType.SpineMid].Position.Z - body.Joints[JointType.HandRight].Position.Z > 0.3))
                    {
                        isHandRightPush = true;
                    }
                }
                else
                {
                    isHandRightPush = false;
                }
            }
            isHandPush = isHandLeftPush || isHandRightPush;
        }

        public bool ensureUser ()
        {
            bool res = true;
            if (body.IsTracked)
            {
                if (body.Joints[JointType.FootLeft].TrackingState == TrackingState.Tracked)
                {
                    footConstHeight = body.Joints[JointType.FootLeft].Position.Y;
                    
                }
                else
                {
                    res = false;
                }
                if (body.Joints[JointType.HipLeft].TrackingState == TrackingState.Tracked)
                {
                    hipConstHeight = body.Joints[JointType.HipLeft].Position.Y;
                }
                else
                {
                    res = false;
                }
            }
            else
            {
                res = false;
            }
            return res;
        }

        public void trackingHandPush()
        {
            trackingHandLeftPush();
            trackingHandRightPush();
        }
    }

}
