using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
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
        LocationInfo barman;
        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            //PlanA();//垃圾分类；
            PlanB();//restraunt;
        }



        public void PlanB()
        {
            //机器人找到 分类员， 分类员可以说“hello” 使机器人面向他，
            //分类员告诉机器人 "take this garbage to somewhere",手持一物。
            //机器识别物品然后并说出。然后放到机器人上，然后机器人导航到somewhere。
            for (int i = 0; i < 2; i++)
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
                Function.Move_Distance(0, -hand_user.BodyCenter.X, 0);
                Thread.Sleep(1000);

                Function.Speech_TTS("can I help you ", false);
                //Function.Move_Distance((hand_user.BodyCenter.Z - 1.5f) / 2, 0, 0);

                //"take this garbage to somewhere"
                speech = new ArrayList();
                speech.Add(new string[] { "take this garbage" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);

                //这里识别物品
                Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), @"Z://0.jpg");
                try
                {
                    Function.Vision_FindObjectByMachineLearning("tea");
                    Function.Speech_TTS("I found " + " tea", false);
                    Function.Hand_Close(150);
                    Function.Move_Navigate(Function.Location_GetLocationInfoByName("bedroom"));//去到该去的地方

                }
                catch
                {
                    Function.Speech_TTS("Plase give me " + "noddles", true);
                    Function.Hand_Close(150);
                    Function.Move_Navigate(Function.Location_GetLocationInfoByName("kitchen"));//去到该去的地方
                }

                Function.Hand_Open(186);//throw it away
                Function.Hand_Close(16);
                Function.Move_Navigate(barman);
            }
        }

        public void PlanA()
        {
            //从startpoint点到table点找垃圾，并将垃圾1拿至垃圾场1。垃圾2至垃圾场2.循环
            LocationInfo land_fill_one = Function.Location_GetLocationInfoByName("bedroom");//垃圾场一
            LocationInfo land_fill_two = Function.Location_GetLocationInfoByName("kitchen");//垃圾场二
            LocationInfo obj_table = Function.Location_GetCurrectLocationFromRos();             //table点

            string[] garbage_one = { "tea" }; //drinks
            string[] garbage_two = { "noodles" };//foods
            //Function.Move_Navigate(Function.Location_GetLocationInfoByName("startpoing"));
            for (int i = 0; i < 1; i++)
            {
                //## Get garbage_one
                Function.Move_Navigate(obj_table);//走到
                
                FuckStuff(garbage_one[i]);          //拿tea
                Function.Move_Navigate(land_fill_one);//
                Function.Hand_Close(186);// throw that 

                //## Get garbage_two
                Function.Move_Navigate(obj_table);
                FuckStuff(garbage_two[i]);
                Function.Move_Navigate(land_fill_two);
                Function.Hand_Close(186);// throw that      
            }
        }

        public void GoGetFuckingStuff(string stuff_name_)
        {
            //获得物品的空间坐标后，给地盘发距离。
            CameraSpacePoint camera_space = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff_name_));
            camera_space.Z = camera_space.Z - 0.45f;
            camera_space.X = camera_space.X - 0.02f;
            Function.Move_Distance(0, camera_space.X, 0);
            Thread.Sleep(1000);
            Function.Move_Distance(camera_space.Z, 0, 0);
            Function.Hand_Close(150);
            Function.Hand_GoUp(50);
            Function.Move_Distance(-0.5f, 0, 0);
        }

        public void FuckStuff(string stuff)
        {
            Function.Hand_GoUp(500);

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
