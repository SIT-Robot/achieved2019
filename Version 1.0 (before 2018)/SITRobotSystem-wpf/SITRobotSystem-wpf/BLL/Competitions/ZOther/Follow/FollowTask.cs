using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Follow
{
    class FollowTask : Tasks.Tasks
    {
        public FollowTask()
        {
            
        }


        public int TrackedPeopleID = 0;

        /// <summary>
        /// 通过手势的形式记住人
        /// </summary>
        /// <returns></returns>
        public int RememberUserByHandState()
        {
            List<User> users = getAllUser();
            User handOnUser = findUserMiddleRasingHand(users);
            return handOnUser.ID;
        }

        public User findUser(int userId)
        {
            User user = findCorrectUser(userId);
            return user;
        }

        public User findUserByCamShiftResult(ColorSpacePoint bodyPoint)
        {
            List<User> users = getAllUser();
            float minDetaX = 5000;
            int ID=0;
            foreach (var user in users)
            {
                ColorSpacePoint userBodyColorPoint= visionCtrl.MapCamera2Color(user.BodyCenter);
                float detaX = Math.Abs(userBodyColorPoint.X - bodyPoint.X);
                if (detaX<=minDetaX)
                {
                    ID = user.ID;
                }
            }
            User resUser =users.Find(u => u.ID == ID);


            return resUser;
        }
        public User findUserByLeg(Leg userleg)
        {
            List<User> users = getAllUser();
            float minDeta = 5000;
            int ID = 0;
            foreach (var user in users)
            {
                float deta = (float)Math.Sqrt(Math.Pow(userleg.X, 2) + Math.Pow(user.BodyCenter.Z,2));
                if (deta <= minDeta)
                {
                    ID = user.ID;
                }
            }
            User resUser = users.Find(u => u.ID == ID);

            return resUser;
        }

        public void followUser(User user)
        {
            Console.WriteLine("Tracking   ID:" + user.ID.ToString() + "  Height:" + user.UserHeight.ToString() + "  X:" +
                              user.BodyCenter.X.ToString() + "  Z:" + user.BodyCenter.Z.ToString() + "  confidence:" +
                              user.confidence.ToString());
            Point twist = ComputSpeed(user.BodyCenter);

            //followTask.SendSpeedSmooth(twist);
        }

        public Point computeSpeedCamShift(ColorSpacePoint colorSpacePoint)
        {
            Point twist=MathPloblems.twistComputeColorPoint(colorSpacePoint);
            return twist;
        }
        public void WaitUser(User user)
        {
           // findCorrectUser(user.ID);
            SpeedStopSmooth();
        }

        public void lostUser()
        {
            
            //用SURF找回User
            return;
        }


        public void followCamshiftUser(ColorSpacePoint bodyColorSpacePoint)
        {
            SendSpeedSmooth(computeSpeedCamShift(bodyColorSpacePoint));
        }

        public void SendSpeed(Point twist)
        {

            SendSpeedSmooth (twist);
        }

        public Leg GetLeg(Leg leg)
        {
            Leg resLeg = baseCtrl.findLeg(leg.PeopleID);
            return resLeg;
        }

        public bool FindSimilar(User user , Leg UserLeg)
        {
            if (Math.Abs(UserLeg.X - user.BodyCenter.Z) < 0.2 && Math.Abs(UserLeg.Y - user.BodyCenter.Y) < 0.2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Leg findLegByUser(User user)
        {
            Leg leg=new Leg();
            string legIDstr = baseCtrl.getLegIDByUser(user.BodyCenter.Z,-user.BodyCenter.X);
            leg = baseCtrl.findLeg(legIDstr);
            return leg;
        }

        public void goback()
        {
            baseCtrl.moveToDirectionSpeed(-2.0f,0);
        }
    }
}
