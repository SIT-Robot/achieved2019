using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.State;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Competitions.Robo_Nures_2015
{
    class Robo_Nures_2015Task:Tasks.Tasks
    {

        public void AskForPill()
        {
            this.baseSpeech.Ro_nurse_2015SpeechRecognize();
        }

        public List<Goods> GetGoodsName()
        {
            List<Goods> gs=new List<Goods>();
            DBCtrl db = new DBCtrl();
            
            //gs=db.GetGoodsByName(db.GetAllGoodsName());
            return gs;
        }

        public User findUserMiddleRasingHand(List<User> users)
        {
            List<User> handOnUsers = users.FindAll(user => user.isRaisingHand);
            User resUser = new User();
            if (handOnUsers.Count != 0)
            {
                int MinX = 0;
                resUser = handOnUsers[0];
                foreach (var user in handOnUsers)
                {
                    if (Math.Abs(resUser.BodyCenter.X) - Math.Abs(user.BodyCenter.X) > 0)
                    {
                        resUser = user;
                        //speechSay("I Find you !");
                    }
                }
            }
            return resUser;
        }

        //surf识别 obj
        public SurfResult SearchObject(Goods obj)
        {
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("searching for " + obj.Name + i + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
            }
            return result;
        }

        public bool GetSeaded()
        {
            List<User> users=new List<User>();
            users=getAllUser();
            return users[0].isSeated;
        }
    }
}
