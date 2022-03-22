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

        public override void InitScript()
        {
            Driver.Kinect_InitKinect();
            Function.Hand_Init();
        }

        public override void Script_Process()
        {
            Function.Vision_WaitForDoor();
            Function.Move_Distance(2f, 0, 0);
            Thread.Sleep(1000); 
            Fuckshopping();
        }
        public void Fuckshopping()
        {
            Function.Speech_TTS("I am ready", false);
            Function.BodyDetect_ShowBodyDetectWindow();
            while (true)
            {
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "Follow me" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    Function.Speech_TTS("ok", true);
                    Function.BodyDetect_StartFollow();
                }
                Fuckmain();
            }
        }
        public void   Fuckmain()
        {
            LocationInfo[] locate = new LocationInfo[4];
            LocationInfo barlocate = new LocationInfo();
            ArrayList speech = new ArrayList();
            string[] obj = new string[4];
            for (int i = 1; i < 5; i++)
            {
                string[] objs = new string[] { "cola", "milk", "water", "noddle"};
                speech.Add(objs);
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                obj[i] = objs[resultInt[i - 1]];
                locate[i] = Function.Location_GetCurrectLocationFromRos();
                Function.Speech_TTS("ok,I have remembered" + obj[i] , false);
                ArrayList speech1 = new ArrayList();
                speech1.Add(new string[] { "Follow me" });
                int[] resultInt1 = Function.Speech_Recognize_StartSimpleRecognize(speech1);
                if (resultInt1[0] == 0)
                    Function.Speech_TTS("ok", true);
                Function.BodyDetect_StartFollow();
            }

           
            ArrayList speech2 = new ArrayList();
            speech2.Add(new string[] { "here is bar" });
            int[] resultInt2 = Function.Speech_Recognize_StartSimpleRecognize(speech2);
            barlocate = Function.Location_GetCurrectLocationFromRos();
            Function.Speech_TTS("ok,I have remembered", true);
               
                for (int j = 1; j < 4; j++)
                {
                ArrayList speech3 = new ArrayList();
                speech3.Add(new string[] { "take the" + obj[j] + "and go back here" });
                    int[] resultInt3 = Function.Speech_Recognize_StartSimpleRecognize(speech3);
                    Function.Speech_TTS("ok,I will take the " + obj[j] + "and go back here", true);
                    ArrayList speech4 = new ArrayList();
                    speech4.Add(new string[] { "yes", "no" });
                    int[] resultInt4 = Function.Speech_Recognize_StartSimpleRecognize(speech4);
                    if (resultInt4[0] == 0) { break; }
                    Function.Move_Navigate(Function.Location_GetLocationInfoByName(obj[j]));
                    Function.Move_Navigate(locate[j]);
                    Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
                    CameraSpacePoint csp = new CameraSpacePoint();
                    csp.X = 10;
                    try
                    {
                        csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(obj[j]));
                    }
                    catch
                    {
                        Console.WriteLine("Can not find obj " + obj[j]);
                        Function.Speech_TTS("Can not find " + obj[j], true);
                        return;
                    }
                    Console.WriteLine("Original CameraSpacePoint.Z=" + csp.Z + " X=" + csp.X);
                    if (csp.Z > 3 | csp.Z < 0)
                    {
                        Console.WriteLine("Can not find obj " + obj[j]);
                        Function.Speech_TTS("Can not find " + obj[j], true);
                        return;
                    }
                    else
                        csp.Z = csp.Z - 0.405f;
                    csp.X = csp.X + 0.022f;
                    Function.Speech_TTS("I found " + obj[j] + ", I will get it.", true);
                    Function.Move_Distance(0, csp.X, 0);
                    Thread.Sleep(500);
                    Function.Move_Distance(csp.Z, 0, 0);
                    if (obj[j] == "cola")
                    {
                        Function.Hand_Close(2);
                    }
                    else if (obj[j] == "milk")
                    {
                        Function.Hand_Close(3);
                    }
                    else if (obj[j] == "noddle")
                    {
                        Function.Hand_Close(3);
                    }
                    else if (obj[j] == "water")
                    {
                        Function.Hand_Close(3);
                    }
                    Function.Speech_TTS("I have found it", false);
                    Function.Hand_GoUp(5);
                    Thread.Sleep(2000);
                    Function.Move_Distance(-0.5f, 0, 0);
                    Function.Hand_GoDown(50);
                    Function.Move_Navigate(barlocate);
                    Function.Hand_Open(150);





                }
            }
        }

    }



 