/*
 * The robot’s owner went shopping for groceries and needs help carrying the groceries 
 * from the car into the home
 */
using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Kinect;


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

            // Driver.Emulator_Enable();
            Driver.Kinect_InitKinect();

        }

        LocationInfo car_place;
        LocationInfo kitchen_table;
        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            Function.Hand_Init();
            new Thread(delegate () { Function.Hand_GoUp(700); }).Start();
            // 2019.4.19 0:44
            // 1. 人和机器走到车旁
            // 2. 记录位置
            // 3. 机器提物品回，放下物品，寻找人像
            // 4. 提示跟随,走到上述记录的位置
            //start:  The operator steps in front of the robot and tells it to follow
            //Following the operater: 
            Thread.Sleep(500);
            FuckFollow();

            // Bring the groceries in
            FuckOrders();
            //Bag delivery            

            // Asking for help
            Function.BodyDetect_GetAllusers();
            Function.Speech_TTS("Hello, could you help me carrying groceries into the house", false);

        }

        /// <summary>
        /// robot wait for the operator saying "follow me"-signal;
        /// operator must raised hand to start follow
        /// by saying "here is the car " to set a location to sql;
        /// </summary>
        public void FuckFollow()
        {
            Function.Speech_TTS("I am ready", false);
            while (true)
            {
                ArrayList speech = new ArrayList();

                speech.Add(new string[] { "Follow me", "here is the car|stop following me" });

                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    // 1. 跟随
                    Function.BodyDetect_ShowBodyDetectWindow();
                    Function.Speech_TTS("Start Follow", true);
                    Function.BodyDetect_StartFollowEx();
                    Function.Speech_TTS("End Follow", true);
                    Function.Move_SetSpeed(0, 0, 0);
                }
                else if (resultInt[0] == 1)
                {
                    car_place = Function.Location_GetCurrectLocationFromRos();//must;
                    break;
                }
            }
        }//end fuckfollow

        public void FuckOrders()
        {

            while (true)
            {


                Function.Speech_TTS("please hand me the blanket", false);//operator hand bag 
                ArrayList speech = new ArrayList();
                //Function.Hand_GoUp(700);
                Function.Hand_Open(0xFF);
                
                speech.Add(new string[] { "This is the blanket" });
                Function.Speech_Recognize_StartSimpleRecognize(speech);
                Function.Hand_Close(0x8C);

                speech = new ArrayList();

                speech.Add(new string[] { "Take this " });
                speech.Add(new string[] { "bag" });
                speech.Add(new string[] { "to the" });
                string[] places = new string[]
                {
                     "bedroom","living room","dining room","kitchen"
                };
                speech.Add(places);
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    string stuff_bag = "blanket";
                    string place_name = places[resultInt[3]];

                    Function.Speech_TTS("Do you want me to take this " + stuff_bag + " to the " + place_name, false);

                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("OK I know, I will deliver it to " + place_name, true);

                        // Location variable: car_place.

                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(place_name));
                        
                        Function.Move_Distance(0.1f, 0, 0);
                        Function.Move_Distance(0, -0.1f, 0);
                        Function.Move_Distance(0.2f, 0, 0);
                        Function.Move_Distance(0, -0.1f, 0);
                        Function.Hand_Open(0xFF);
                        Function.Move_Distance(-0.25f, 0, 0);
                        //Function.BodyDetect_ShowBodyDetectWindow();
                        //List<User> fuckers = Function.BodyDetect_GetAllusers();
                        List<User> fuckers = null;
                        Function.BodyDetect_ShowBodyDetectWindow();
                        Thread.Sleep(3000);
                        Function.Move_SetSpeed(0, 0, 0.1f);
                        while (true)
                        {
                            fuckers = Function.BodyDetect_GetAllusers();
                            //float angle = 5;

                            if (fuckers.Count == 1)
                            {
                                if (Math.Abs(fuckers[0].BodyCenter.X) < 0.5)
                                {
                                    break;

                                }

                            };

                        }
                        Function.Move_SetSpeed(0, 0, 0);


                        FaceInfo[] faces = Function.Vision_FaceDetect();
                        int rotatedAngle = 0;
                        while ((faces.Length != 1) | (rotatedAngle == 360))
                        {
                            rotatedAngle += 10;
                            Function.Move_Distance(0, 0, 10);
                            faces = Function.Vision_FaceDetect();
                        }
                        string ld = Function.Vision_Kinect_Shot();
                        System.Console.WriteLine(ld);

                        System.Console.Write("Successed");
                        Function.Speech_TTS("I find you,follow me", true);
                         Function.Move_Navigate(car_place);
                        break;//must!!

                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }


            }
        }//end fuckorderd

    }
}
