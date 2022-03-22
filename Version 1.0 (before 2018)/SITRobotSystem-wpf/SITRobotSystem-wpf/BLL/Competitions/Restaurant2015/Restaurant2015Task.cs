using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2015
{
    class Restaurant2015Task : Tasks.Tasks
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

        public bool moveToDirection(float x,float y,float angel)
        {
            int res;
            res = baseCtrl.moveToDirection(x, y, angel);
            


            if (res == 1)
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

        //restaurant回到初始位置询问顾客在哪张桌子
        public int WhichTable(string[] objects)
        {
            baseSpeech = new SitRobotSpeech();
            baseSpeech.robotSpeak("Which Table should I go?");
            baseSpeech.Res2015Stage2Recognize(objects);
            int table;
            while (true)
            {
               
                table = baseSpeech.ReturnNo;
                if (table != -1)
                {
                    return table;
                }
                    
            }
        }

        public int[] WitchNeed(string[] things)
        {
            baseSpeech = new SitRobotSpeech();
            baseSpeech.robotSpeak("What can I do for you?");
            baseSpeech.Res2015Stage2Recognize(things);
            int[] objects = null;
            while (true)
            {
                Thread.Sleep(100);
                objects = baseSpeech.rCommand;
                if (objects != null)
                {
                    return objects;
                }
            }
        }


    }
}


