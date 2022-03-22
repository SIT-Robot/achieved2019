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
           // Driver.Ros_DisableRosForDebug();
            // Function.Hand_Init();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            Function.Vision_WaitForDoor();
            Function.Move_Distance(1.8f, 0, 0);

            Function.Move_Navigate(Function.Location_GetLocationInfoByName("diningroom"));
            Function.Speech_TTS("I arrived", false);
            try
            {
                Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), @"Z:\0.jpg");
                Function.Vision_FindObjectByMachineLearning("tea");
            }
            catch { }
            Function.Speech_TTS("I found an object", false);
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("livingroom"));
            Function.Speech_TTS("I arrived. can not find object", false);
            Thread.Sleep(1000);
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("exit"));

        }
        public void FuckOrders()
        {
            //grammer 1 I guess: find something in somewhere
            string sentence = "";
            string stuff_name = "";
            string place_name = "";
            while (true)
            {
                ArrayList speech = new ArrayList();

                string[] s0 = new string[]
                {

                        "find","go to"
                };

                speech.Add(s0);

                string[] s1 = new string[] {

                        "coffee","chips","rollpaper","water","toothpaste",
                        "soap","noodles","cake","milk","cola","biscuit","tea"
                };
                speech.Add(s1);
                string[] s2 = new string[]
                {
                        "in"
                };
                speech.Add(s2);

                string[] s3 =
                {
                    "kitchen", "diningroom", "bedroom", "livingroom"
                };

                speech.Add(s3);
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)//grammar 1
                {//take something from someplace and go back here

                    stuff_name = s1[resultInt[0]];//获得 sl 里面的 第一个语言识别到的 下标
                    place_name = s3[resultInt[1]];
                    sentence = "take " + stuff_name + " from " + place_name + " and go back here? ";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, false);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        //FuckStuff(stuff_name);//Get stuff and said i have found the stuff name;
                        Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
                        Function.Speech_TTS("I have remembered it", false);//假装识别

                        Console.Write("Successed");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }//end grammer1(if)

                if (resultInt[0] == 1)
                {
                    place_name = s3[resultInt[1]];
                    sentence = "go to " + place_name + "? ";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, false);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        Function.Speech_TTS("I have found the " + stuff_name, true);
                        FuckStuff(stuff_name);//Get stuff and said i have found the stuff name;
                        Console.Write("Successed");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }

                else { Function.Speech_TTS("Maybe I am wrong.", false); }
            }//while
        }//end void

        public void FuckStuff(string stuff_name_)
        {
            CameraSpacePoint cameraSpace = new CameraSpacePoint();
            try
            {
                cameraSpace = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff_name_));
            }
            catch
            {
                try
                {
                    cameraSpace = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff_name_));
                }
                catch { Function.Move_Navigate(Function.Location_GetLocationInfoByName("exit")); }
            }
            cameraSpace.Z = cameraSpace.Z - 0.45f;
            cameraSpace.X = cameraSpace.X - 0.02f;

            Function.Move_Distance(0, cameraSpace.X, 0);
            Thread.Sleep(1000);
            Function.Move_Distance(cameraSpace.Z, 0, 0);
            Function.Hand_Close(150);
            Function.Hand_GoUp(50);
            Function.Move_Distance(-0.5f, 0, 0);
        }

    }

}

