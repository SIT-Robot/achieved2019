using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Innovation
{
    class InnovationStages:Stage
    {
        public InnovationTask ITask;

        public InnovationStages()
        {
            ITask = new InnovationTask();
        }

        public override void init()
        {
            ITask.initSurfFrmae();
            ITask.initBodyDetect();
            ITask.initSpeech();
        }

        //public void findpeople()
        //{
        //    gpsrTask.moveToPlaceByName(command.thing.Name);
        //    User user = new User();
            
        //    while (true)
        //    {
        //        user = gpsrTask.findUserMiddleRasingHand(gpsrTask.getAllUser());
        //        if (user.ID != 0)
        //        {
        //            break;
        //        }
        //    }
        //    gpsrTask.GetCloseToUser(user);
            
        //}

        public void follow()
        {
            
            ITask.followEasy();
        }
        [DllImport("winmm.dll")]
        public static extern bool PlaySound(String Filename, int Mod, int Flags);  
        public void trackingUser()
        {
            ITask.speak("hello,please stay here.");
            Thread.Sleep(5);
            System.Media.SoundPlayer sp = new SoundPlayer();
            //sp.SoundLocation = @"B:\programs\SITRobotSystem-wpf\SITRobotSystem-wpf\bin\x64\Debug\1.mp3";
            //` sp.PlayLooping();  
            while (true)
            {
                List<User> users = ITask.getAllUser();
                if (users.Count>0)
                {
                    foreach (var user in users)
                    {
                        user.trackingJump();
                        if (user.isJumping)
                        {
                            ITask.speak("Don't jump!!!");
                            Thread.Sleep(5000);
                        }
                    }
                }
            }

            
        }

        public void say(String word)
        {
            ITask.speak(word);
        }


        public void StartCare()
        {
            ITask.CareForCommand();
            //ITask.moveToPlaceByName("bed room");
        }
    }
}
