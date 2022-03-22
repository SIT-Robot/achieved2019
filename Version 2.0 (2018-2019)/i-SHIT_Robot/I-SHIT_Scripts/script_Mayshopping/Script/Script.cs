using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;
using System;
using System.Collections;
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
            LocationInfo barlocate = new LocationInfo();
            Function.Speech_TTS("I am ready", false);
            Function.BodyDetect_ShowBodyDetectWindow();
            LocationInfo[] locate = new LocationInfo[4];
            string[] obj = new string[4];
            string[] objs = new string[] {       "milk","chips","safeguard","toothpaste","laoganma",
                        "cola","icetea","coffee","noodle","water","porridge","napkin","floralwater" };
            bool[] isLeft = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                while (true)
                {
                    //一直跟随，意外停止while继续。
                    ArrayList speech = new ArrayList();
                    speech.Add(new string[] { "Follow me", "stop follow" });
                    int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                    if (resultInt[0] == 0)
                    {
                        Function.Speech_TTS("Start Follow", true);
                        Function.BodyDetect_StartFollow();
                        Function.Speech_TTS("End Follow", true);
                        Function.Move_SetSpeed(0, 0, 0);
                    }
                    else if (resultInt[0] == 1)
                    {
                        break;
                    }
                }
                Function.Speech_TTS("what's this", false);
                ArrayList speech3 = new ArrayList();
                speech3.Add(objs);
                int[] resultInt55 = Function.Speech_Recognize_StartSimpleRecognize(speech3);
                obj[i] = objs[resultInt55[0]];
                locate[i] = Function.Location_GetCurrectLocationFromRos();
                Function.Speech_TTS("ok", false);

            }//end for

            while (true)
            {
                //一直跟随，意外停止while继续。
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "Follow me", "stop follow" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    Function.Speech_TTS("Start Follow", true);
                    Function.BodyDetect_StartFollow();
                    Function.Speech_TTS("End Follow", true);
                    Function.Move_SetSpeed(0, 0, 0);
                }
                else if (resultInt[0] == 1)
                {
                    break;
                }
            }

            Function.Speech_TTS("ok", false);
            ArrayList speech2 = new ArrayList();
            speech2.Add(new string[] { "here is bar" });
            int[] resultInt2 = Function.Speech_Recognize_StartSimpleRecognize(speech2);
            barlocate = Function.Location_GetCurrectLocationFromRos();

            Function.Speech_TTS("what you want", false);
            int[] resultInt3;
            while (true)
            {
                ArrayList speech3 = new ArrayList();
                speech3.Add(objs);
                speech3.Add(new string[] { "and" });
                speech3.Add(objs);
                speech3.Add(new string[] { "and" });
                speech3.Add(objs);
                resultInt3 = Function.Speech_Recognize_StartSimpleRecognize(speech3);
                Function.Speech_TTS(objs[resultInt3[0]] + " and " + objs[resultInt3[2]] + " and " + objs[resultInt3[4]] + ", right?", true);
                ArrayList speech4 = new ArrayList();
                speech4.Add(new string[] { "yes", "no" });
                int[] resultInt4 = Function.Speech_Recognize_StartSimpleRecognize(speech4);
                if (resultInt4[0] == 0) { break; }
            }


            Function.Move_Navigate(locate[3]);
            Thread.Sleep(200);
            FuckStuff(objs[resultInt3[0]]);
            Function.Move_Navigate(barlocate);
            Function.Hand_Open(150);

            Function.Move_Navigate(locate[1]);
            Function.Speech_TTS("I arrived, I can't found the object", false);
            Function.Move_Navigate(locate[2]);
            Function.Speech_TTS("I arrived, I can't found the object", false);
            //Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), @"Z:\0.jpg");
        }



        public void FuckStuff(string stuff)
        {

            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
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
            csp.Z = csp.Z - 0.405f;
            csp.X = csp.X + 0.022f;
            Function.Speech_TTS("I found " + stuff + ", I will get it.", true);
            Function.Move_Distance(0, csp.X, 0);
            Thread.Sleep(500);
            Function.Move_Distance(csp.Z, 0, 0);
            if (stuff == "icetea")
            {
                Function.Hand_Close(150);

            }
            else if (stuff == "toothpaste")
            {
                Function.Hand_Close(155);

            }
            else if (stuff == "napkin")
            {
                Function.Hand_Close(145);

            }
            else if (stuff == "coffee")
            {
                Function.Hand_Close(160);

            }
            else if (stuff == "water")
            {
                Function.Hand_Close(150);

            }
            else if (stuff == "porridge")
            {
                Function.Hand_Close(140);

            }
            else if (stuff == "cola")
            {
                Function.Hand_Close(143);

            }
            else if (stuff == "noodle")
            {
                Function.Hand_Close(128);

            }
            else if (stuff == "floralwater")
            {
                Function.Hand_Close(163);

            }
            else if (stuff == "safeguard")
            {
                Function.Hand_Close(158);

            }
            else if (stuff == "chips")
            {
                Function.Hand_Close(140);

            }
            else if (stuff == "milk")
            {
                Function.Hand_Close(155);

            }
            else if (stuff == "laoganma")
            {
                Function.Hand_Close(144);

            }
            Function.Hand_GoUp(50);
            Thread.Sleep(3000);
            Function.Move_Distance(-0.5f, 0, 0);
            Function.Hand_GoDown(50);
        }
    }


}
