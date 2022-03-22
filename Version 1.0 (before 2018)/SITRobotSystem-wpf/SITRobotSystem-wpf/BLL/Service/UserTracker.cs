using System.Collections.Generic;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Service
{

    public enum TrackerType
    {
        middleUser = 1
    }
    /// <summary>
    /// 用作人体跟踪的捕获算法
    /// </summary>
    public  class UserTracker 
    {

        public static bool isBodyReady;

        public int TrackingUserID;
        public void init()
        {

        }
        public static List<User> users= new List<User>();

        public void cleanUser()
        {
            users.Clear();
            isBodyReady = false;
        }
        public void bodyReady()
        {
            isBodyReady = true;
        }
        public void addUserBody(Body body)
        {
            User user = new User();
            user.body = body;
            user.sync();
            user.processUser();
            users.Add(user);
        }

        public void addUserBody(Body body,bool isSeated,double seatedConfidence)
        {
            User user = new User();
            user.isSeated = isSeated;
            user.seatedConfidence = seatedConfidence;
            user.body = body;
            user.sync();
            user.processUser();
            users.Add(user);
        }

        /// <summary>
        /// 确认人
        /// </summary>
        public void ensureUser()
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].isRaisingHand)
                {
                    TrackingUserID = users[i].ID;
                    if (users[i].body.Joints[JointType.FootLeft].TrackingState==TrackingState.Tracked)
                    {
                        KinectInfo.FloorHeight= users[i].body.Joints[JointType.FootLeft].Position.Y;
                    }
                }
            }
        }

        public User getTrackingUser()
        {

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].ID == TrackingUserID)
                {
                    return users[i];
                }
            }
            return new User();
            
        }
    }

}
