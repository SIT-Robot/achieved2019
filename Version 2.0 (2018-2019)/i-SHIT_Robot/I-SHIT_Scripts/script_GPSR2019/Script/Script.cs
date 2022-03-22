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
        //globle value
     
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
            Driver.Emulator_Enable();
            // Driver.Ros_DisableRosForDebug();
            //Function.Hand_Init();
        }



        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            LocationInfo gpsr_start = Function.Location_GetLocationInfoByName("start");
            //Function.Vision_WaitForDoor();
            Thread.Sleep(5000);           //wait for open the door. 
            Function.Move_Distance(1.3f, 0, 0);//the door to the startpoint. 
            for (int i = 0; i < 3; i++)//3 means how many tasks
            {

                //Entering and command retrieval
                Function.Move_Navigate(gpsr_start);    //gpsr

                //Command generation
                FuckGPSR();
                //
            }
            Function.Move_Navigate(gpsr_start);
            //Function.Hand_Open(0xFF);
            Function.Speech_TTS("Task Finished", false);
        }


        public void FuckGPSR()
        {
            //grammer one
            //take something from someplace and go back here
            //(go to)/(reach)(navigate to) someplace and (find)/(look for)/(search for) something and go back here;
            //(find)/(look for)/(search for) somebody in someplace and answer a question  
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

                string[] s1 = new string[] {//=======1
                        //-------1
                        // "milk","chips","safeguard","toothpaste","laoganma",
                        // "cola","icetea","coffee","noodle","water","porridge","napkin","floralwater", change in 4.18
                        "cola", "milktea","ice tea","grape juice","beer","toothpaste",
                        "soap","pear","apple","orange","biscuit",
                        "green shampoo","blue shampoo","yellow shampoo",
                        "green chips","yellow chips","red chips",
                        //-------2
                        "kitchen", "diningroom", "bedroom", "livingroom",
                        //-------3
                        //"tom","angle","paul","jamie","green","fisher","kevin","shirley","tracy","robin","john","daniel"}; change in 4.18. 
                        "James","Alex","Ryan","John","Eric","Adam","Carter","Jack","David","Tyler","Lily","Mary","Anna","Zoe","Sara","Sofia","Faith","Julia","Paige","Jessica"};
                speech.Add(s1);

                string[] s2 = new string[]
                {
                        "from|from the" ,
                        "and find|and find the","and look for|and look for the","and search for|and search for the",
                        "in the|in"
                };
                speech.Add(s2);//2

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
                            // Function.BodyDetect_ShowBodyDetectWindow();
                            // List<User> fuckers = Function.BodyDetect_GetAllusers();
                            FaceInfo[] faceinfo = Function.Vision_FaceDetect();
                            float angle = 5;
                            int num = 0;
                            while (faceinfo.Length != 1)
                            {
                                Function.Move_Distance(0, 0, num * angle);
                                num++;
                            }
                            ColorSpacePoint color = new ColorSpacePoint();
                            color.X = faceinfo[0].FaceLocation.X + faceinfo[0].FaceLocation.Width / 2;
                            color.Y = faceinfo[0].FaceLocation.Y + faceinfo[0].FaceLocation.Height / 2;

                            CameraSpacePoint cameraSpacePoint = Function.Vision_GetCameraSpacePoint(color);
                            //CameraSpacePoint cameraSpacePoint 
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
        }//end fuckorder






        public void FuckStuff(string stuff, string _place_name) //grasp the objects
        {
            if (_place_name == "bedroom")
            {
                Function.Hand_GoUp(440);
            }
            else if (_place_name == "livingroom")
            {
                Function.Hand_GoUp(400);
            }
            else if (_place_name == "kitchen")
            {
                Function.Hand_GoUp(1500);
            }
            else if (_place_name == "diningroom")
            {
                Function.Hand_GoUp(1000);
            }
            //Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
            CameraSpacePoint csp = new CameraSpacePoint();
            csp.X = 10;
            try
            {
                // csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff));
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

        public void AskQuestions()
        {
            //try
            //{
            //    int sum = 0;
            //    for (int i = 0; i < 10; i++)
            //    {
            //        int count = Function.Vision_FaceDetect().Length;
            //        sum = count > sum ? count : sum;

            //    }
            //    Function.Speech_TTS("Here are" + sum.ToString() + "people", false);
            //}
            //catch
            //{
            //    Function.Speech_TTS("Here is a problem,sit back and relax", false);
            //}

            ArrayList userProblems = new ArrayList();
            userProblems.Add(new string[]{"what is the capital of china?",
                "how many hours in a day?",
                "how many season are there in one year?",
                "how many seconds in one minute?",
                "what is the biggest province of china ?",
                "how large is the area of china",
                "what is the word biggest island",
                "What is China's national animal?",
                "Who was the first president of the USA?",
                "How many children did Queen Victoria have?",
                "What was the former name of New York?"  });
            string[] answer = new string[] { "beijing",
                "24",
                "4",
                "60",
                "xinjiang",
                "960 square kilometer",
                "Greenland",
                "Panda. ",
                "George Washington",
                "Nine children",
                "New Amsterdam"};

            int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(userProblems);
            Function.Speech_TTS(answer[resultInt[0]], false);

        }

    }//end
}

