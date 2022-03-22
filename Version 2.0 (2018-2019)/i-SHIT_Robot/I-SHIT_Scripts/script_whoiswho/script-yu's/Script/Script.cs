﻿using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System.Collections;
using System;
using Microsoft.Kinect;//1
using System.Threading;
using System.Collections.Generic;


namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        LocationInfo gpsr_start = Function.Location_GetLocationInfoByName("startpoint");
        string place_name = "";
        string stuff_name = "";
        string people_name = "";
        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        public override void InitScript()
        {
            Driver.Kinect_InitKinect();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            Function.Vision_WaitForDoor();
            Function.Move_Distance(2f, 0, 0);//2
            for (int i = 0; i < 1; i++)//3 means how many tasks
            {
                Function.Move_Navigate(gpsr_start);
                FuckGPSR();
            }
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("startpoint"));
            Function.Hand_Open(0xFF);
            Function.Speech_TTS("Task Finished", false);
        }
        public void FuckGPSR()
        {
            Function.Speech_TTS("I am ready", false);
            string sentence = "";
            while (true)
            {
                ArrayList speech = new ArrayList();

                string[] s0 = new string[]
                {
                    //-------1
                        "take the|take",
                        //-------2
                        "go to the|go to","reach|reach the","navigate to the|navigate to",
                        //-------3
                        "find the| find","look for the|look for","search for the|search for"
                };
                speech.Add(s0);

                string[] s1 = new string[]
                {
                    //-------1
                        "milk","chips","safeguard","toothpaste","laoganma",
                        "cola","icetea","coffee","noodle","water","porridge","napkin","floralwater",
                        //-------2
                        "kitchen", "diningroom", "bedroom", "livingroom",
                        //-------3
                        "tom","angle","paul","jamie","green","fisher","kevin","shirley","tracy","robin","john","daniel"
                };
                speech.Add(s1);

                string[] s2 = new string[]
                {
                     "from|from the" ,
                        "and find|and find the","and look for|and look for the","and search for|and search for the",
                        "in the|in"
                };
                speech.Add(s2);

                /*string[] s3 = {//---------1  & 3 
                    "kitchen", "diningroom", "bedroom", "livingroom",
                    //---------2
                    "coffee","chips","rollpaper",
                    "water","toothpaste","soap ","noodles","cake","milk",
                    "cola","biscuit" };*/

                speech.Add(s1);
                speech.Add(new string[] {//=========4
                    "and go back here", "and answer a question" });

                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);

                if (resultInt[0] == 0)//grammar 1
                {//take something from someplace and go back here

                    stuff_name = s1[resultInt[1]];//set goble value stuff_name;
                    place_name = s1[resultInt[3]];//set goble value place_name;
                    sentence = "take " + stuff_name + " from " + place_name + " and go back here? ";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, false);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        Function.Speech_TTS("I arrived " + place_name, false);



                        Function.Speech_TTS("I am finding " + stuff_name, false);
                        //FuckStuff(stuff_name,place_name);
                        Console.WriteLine("End of look for.");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }

                }

                else if (resultInt[0] == 1 || resultInt[0] == 2 || resultInt[0] == 3)//grammer 2;
                {
                    //(go to)/(reach)(navigate to) someplace 
                    //and (find)/(look for)/(search for) something and go back here;
                    string word0 = "";
                    string word2 = "";
                    word0 = s0[resultInt[0]];
                    stuff_name = s1[resultInt[3]];//get stuff name from speech
                    place_name = s1[resultInt[1]];//get place_name from speech;

                    word2 = s2[resultInt[2]];
                    sentence = "go to " + place_name + " and look for " + stuff_name + " and go back here?";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, true);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        Function.Speech_TTS("I arrived " + place_name, false);
                        //Function.Speech_TTS("I am finding " + stuff_name, true);
                        try
                        {
                            //Function.Vision_FindObjectByMachineLearning(stuff_name);
                            //Function.Speech_TTS("I found " + stuff_name, false);
                        }
                        catch { Function.Speech_TTS("can not find " + stuff_name, false); }
                        Console.Write("Successed");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
                else if (resultInt[0] == 1 || resultInt[0] == 2 || resultInt[0] == 3)//grammer 2;
                {
                    //(go to)/(reach)(navigate to) someplace 
                    //and (find)/(look for)/(search for) something and go back here;
                    string word0 = "";
                    string word2 = "";
                    word0 = s0[resultInt[0]];
                    stuff_name = s1[resultInt[3]];//get stuff name from speech
                    place_name = s1[resultInt[1]];//get place_name from speech;

                    word2 = s2[resultInt[2]];
                    sentence = "go to " + place_name + " and look for " + stuff_name + " and go back here?";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, true);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        Function.Speech_TTS("I arrived " + place_name, false);
                        //Function.Speech_TTS("I am finding " + stuff_name, true);
                        try
                        {
                            //Function.Vision_FindObjectByMachineLearning(stuff_name);
                            //Function.Speech_TTS("I found " + stuff_name, false);
                        }
                        catch { Function.Speech_TTS("can not find " + stuff_name, false); }
                        Console.Write("Successed");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
                else if (resultInt[0] == 4 || resultInt[0] == 5 || resultInt[0] == 6)//grammer 3;
                {
                    //(find)/(look for)/(search for) somebody in someplace and answer a question
                    string word0 = "";
                    word0 = s0[resultInt[0]];
                    people_name = s1[resultInt[1]];
                    place_name = s1[resultInt[3]];

                    sentence = "find " + people_name + " " + "in " + place_name + " and answer a question";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will go to " + place_name, true);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        Thread.Sleep(1000);
                        //   Function.Move_Distance(0, 0, 90);
                        if (place_name == "livingroom")
                        {
                            Function.Move_Distance(0, 0, 90);
                        }

                        else if (place_name == "diningroom")
                        {
                            Function.Move_Distance(0, 0, 70);
                        }
                        else if (place_name == "kitchen")
                        {
                            Function.Move_Distance(0, 0, 90);
                        }
                        Function.Speech_TTS("I arrived " + place_name, false);


                        try
                        {
                            Function.BodyDetect_ShowBodyDetectWindow();
                            List<User> fuckers = Function.BodyDetect_GetAllusers();
                            CameraSpacePoint cameraSpacePoint = fuckers[0].BodyCenter;
                            cameraSpacePoint.Z = cameraSpacePoint.Z - 0.45f;
                            cameraSpacePoint.X = cameraSpacePoint.X - 0.02f;

                            Function.Move_Distance(0, cameraSpacePoint.X, 0);
                            Thread.Sleep(1000);
                            Function.Move_Distance(cameraSpacePoint.Z, 0, 0);
                        }
                        catch { }
                        finally { Function.BodyDetect_CloseBodyDetectWindow(); }
                        Function.Speech_TTS("what is your questions", true);
                        AskQuestions();
                        Console.Write("Successed");
                        break;
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
            }
        }
        public void FuckStuff(string stuff, string _place_name)
        {

        }

        public void AskQuestions()
        {

        }
    }
}
