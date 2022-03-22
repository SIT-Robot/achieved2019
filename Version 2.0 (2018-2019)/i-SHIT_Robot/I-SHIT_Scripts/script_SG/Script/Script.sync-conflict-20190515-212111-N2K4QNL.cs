using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;
using System;
using System.Threading;

namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        public override void InitScript()
        {
            Driver.Kinect_InitKinect();
            Function.Hand_Init();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
          
            Function.Hand_GoUp(1300);

            FuckStuff("chips");

            FuckStuff("cola");

            FuckStuff("rollpaper");

            FuckStuff("cake");


            FuckStuff("noodles");


            FuckStuff("biscuit");



            FuckStuff("soap");


            FuckStuff("coffee");


            FuckStuff("toothpaste");


            FuckStuff("milk");

            FuckStuff("water");
        }

        public void StartFucking()
        {

            Function.Move_Navigate(Function.Location_GetLocationInfoByName("table"));

            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
        }

        public void FuckStuff(string stuff)
        {
            StartFucking();
            CameraSpacePoint csp = new CameraSpacePoint();
            csp.X = 10;
            try
            {
                csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff));
            }
            catch
            {
                Console.WriteLine("Can not find obj " + stuff);
                Function.Speech_TTS("Can not find " + stuff, true);
                return;
            }
            Console.WriteLine("Original CameraSpacePoint.Z=" + csp.Z + " X=" + csp.X);
            if (csp.Z > 3 | csp.Z < 0)
            {
                Console.WriteLine("Can not find obj " + stuff);
                Function.Speech_TTS("Can not find " + stuff, true);
                return;
            }
            csp.Z = csp.Z - 0.43f;
            csp.X = csp.X + 0.022f;
            Function.Speech_TTS("I found " + stuff + " I will get it.", true);
            Function.Move_Distance(0, csp.X, 0);
            Thread.Sleep(500);
            Function.Move_Distance(csp.Z, 0, 0);
            if (stuff == "cola")
            {
                Function.Hand_Close(120);

            }
            else if (stuff == "milktea")
            {
                Function.Hand_Close(123);

            }
            else if (stuff == "toothpaste")
            {
                Function.Hand_Close(110);

            }
            else if (stuff == "beer")
            {
                Function.Hand_Close(117);

            }
            else if (stuff == "icetea")
            {
                Function.Hand_Close(128);

            }
            else if (stuff == "soap")
            {
                Function.Hand_Close(120);

            }
            else if (stuff == "red chips")
            {
                Function.Hand_Close(120);

            }
            else if (stuff == "biscuit")
            {
                Function.Hand_Close(120);

            }
            else if (stuff == "yellow chips")
            {
                Function.Hand_Close(120);

            }
            else if (stuff == "apple")
            {
                Function.Hand_Close(113);

            }
            else if (stuff == "orange")
            {
                Function.Hand_Close(140);
            }
            else if (stuff == "grapejuice")
            {
                Function.Hand_Close(128);
            }
            else if (stuff == "green shampoo")
            {
                Function.Hand_Close(140);
            }
            else if (stuff == "blue shampoo")
            {
                Function.Hand_Close(140);
            }
            else if (stuff == "yellow shampoo")
            {
                Function.Hand_Close(140);
            }
            else if (stuff == "pear")
            {
                Function.Hand_Close(113);
            }
            Function.Hand_GoUp(50);
            Thread.Sleep(3000);

            Function.Move_Distance(-0.5f, 0, 0);
            Function.Move_Distance(0, 0, 180);
            Function.Hand_Open(0xFF);
            Function.Hand_Close(16);
            Function.Hand_GoDown(50);
        }
    }
}
