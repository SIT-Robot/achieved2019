using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.shopping
{
    class ShoppingTask : Tasks.Tasks
    {

        public void speechRemeber(string[] objects)
        {
            baseSpeech=new SitRobotSpeech();
            baseSpeech.robotSpeak("I'm ready");
            //rtbStageOutput.Text += "已选择Restaurant项目语音识别\n";
            baseSpeech.restaurantRecognize(objects);
            //rtbStageOutput.Text += "启动完毕\n";
        }

        public void speechCommand(string[] objects)
        {
            baseSpeech.robotSpeak("I'm ready");
            baseSpeech.restaurantCommand(objects);
        }

        public void followStart()
        {

        }


        public bool moveToDirection(string direction)
        {
            int res;
            float X = 0, Y = 0;          
            if (direction == "left")
            {
                float Angel = -90; 
                res = baseCtrl.moveToDirection(X, Y, Angel);
            }
            else
            {
                float Angel = 90; 
                res = baseCtrl.moveToDirection(X, Y, Angel);
            }


            if (res==1)
            {
            return true;
        }
            else
            {
                return false;
            }
            
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
                        speak("I Find you !");
                    }
                }
            }

            return resUser;
        }


        internal void RestaurantRecognize(string[] objects, string[] thing)
        {
            baseSpeech = new SitRobotSpeech();
            baseSpeech.intlResRecognize(objects, thing);
        }
    }
}
