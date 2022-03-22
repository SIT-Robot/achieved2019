using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System.Collections;
using System;
using Microsoft.Kinect;
using System.Threading;
using System.Collections.Generic;

namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
        //globle value


        LocationInfo barman;

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

            Function.Speech_TTS("IN THE BARMAN'S RIGHT", true);

            for (int i = 0; i < 6; i++)
            {
                Function.BodyDetect_ShowBodyDetectWindow();
                barman = Function.Location_GetCurrectLocationFromRos();
                AudioDetection ad = new AudioDetection();
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "hello" });

                Function.Speech_Recognize_StartSimpleRecognize(speech);
                Function.Move_Distance(0, 0, ad.nowangle * 1.2f);

                List<User> users = Function.BodyDetect_GetAllusers();
                User hand_user = Function.BodyDetect_FindUserMiddleRasingHand(users);
                while (true)
                {
                    users = Function.BodyDetect_GetAllusers();
                    hand_user = Function.BodyDetect_FindUserMiddleRasingHand(users);
                    if (hand_user.BodyCenter.Z != -1) { break; }
                }
                //LocationInfo userlocation = Function.Location_GetRelativeLocationInfo(hand_user.BodyCenter.Z, hand_user.BodyCenter.X, 0, Function.Location_GetCurrectLocationFromRos());
                //Function.Move_Navigate(userlocation);
                Function.Move_Distance((hand_user.BodyCenter.Z - 1.4f), 0, 0);
                Thread.Sleep(1000);
                Function.Move_Distance(0, hand_user.BodyCenter.X, 0);
                Thread.Sleep(1000);
                Function.Speech_TTS("Could i get you anything", false);
                //Function.Move_Distance((hand_user.BodyCenter.Z - 1.5f) / 2, 0, 0);
                speech = new ArrayList();
                speech.Add(new string[] { "I want" });
                string[] stuffs = new string[] {//=======1

                        "coffee","chips","rollpaper","water","toothpaste",
                        "soap","noodles","cake","milk","cola","biscuit",
                };

                speech.Add(stuffs);
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                LocationInfo userlocation = Function.Location_GetCurrectLocationFromRos();
                Function.Move_Navigate(Function.Location_GetLocationInfoByName("diningroom"));
                FuckStuff(stuffs[resultInt[1]]);
                Function.Move_Navigate(userlocation);
                Function.Hand_Open(0xFF);
                Function.Hand_Close(16);
                Function.Hand_GoDown(50);
                Function.Move_Navigate(barman);
            }
        }


        public void FuckStuff(string stuff)
        {

            CameraSpacePoint csp = new CameraSpacePoint();
            csp.X = 10;
            try
            {
                csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff));
            }
            catch
            {
                Function.Move_Navigate(barman);
                Console.WriteLine("Can not find obj " + stuff);
                Function.Speech_TTS("Plase give me " + stuff, true);
                return;
            }
            Console.WriteLine("Original CameraSpacePoint.Z=" + csp.Z + " X=" + csp.X);
            if (csp.Z > 2 | csp.Z < 0)
            {
                Function.Move_Navigate(barman);
                Console.WriteLine("Plase give me " + stuff);
                Function.Speech_TTS("Plase give me " + stuff, true);
                return;
            }
            csp.Z = csp.Z - 0.39f;
            csp.X = csp.X + 0.022f;
            Function.Speech_TTS("I found " + stuff + " I will get it.", true);
            Function.Move_Distance(0, csp.X, 0);
            Thread.Sleep(500);
            Function.Move_Distance(csp.Z, 0, 0);
            if (stuff == "noodles")
            {
                Function.Hand_Close(135);

            }
            else if (stuff == "cake")
            {
                Function.Hand_Close(150);

            }
            else if (stuff == "toothpaste")
            {
                Function.Hand_Close(162);

            }
            else if (stuff == "coffee")
            {
                Function.Hand_Close(152);

            }
            else if (stuff == "cola")
            {
                Function.Hand_Close(150);

            }
            else if (stuff == "soap")
            {
                Function.Hand_Close(163);

            }
            else if (stuff == "chips")
            {
                Function.Hand_Close(149);

            }
            else if (stuff == "biscuit")
            {
                Function.Hand_Close(140);

            }
            else if (stuff == "rollpaper")
            {
                Function.Hand_Close(142);

            }
            else if (stuff == "milk")
            {
                Function.Hand_Close(161);

            }
            else if (stuff == "water")
            {
                Function.Hand_Close(148);

            }
            Function.Hand_GoUp(50);
            Function.Move_Distance(-0.5f, 0, 0);

        }







    }//end

}


/*test grammer:
 * sentence                                         is successed        times
 * take coffee from kitchen and go back here;       1                   1
 * go to kitchen and find coffee and go back here;  0   1               2  1
 * find tom in kitchen and answer a question        0   1               4  1 (tom&paul)
 * 
 * take chips from dining room and go back here     1                   4
 * reach dining room and look for chips and go back here    1           4
 * look for Angel in dining room and answer a question      1           3
 * 
 * take roll paper from bedroom and go back here    1                   1
 * navigate to bedroom and search for roll paper and go back here   0   2
 * search for Paul in bedroom and answer a question     0 1              10 1
 * 
 * 
*/
