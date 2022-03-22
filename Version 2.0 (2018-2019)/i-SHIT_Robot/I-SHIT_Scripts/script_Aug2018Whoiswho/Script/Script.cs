using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;
namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {

        int Num = 4;//人数-1

        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        public override void InitScript()
        {
            //  Function.Hand_Init ();
            Driver.Kinect_InitKinect();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        /// 


        public override void Script_Process()
        {

            // Function.Vision_WaitForDoor();
            Function.BodyDetect_ShowBodyDetectWindow();
            Thread.Sleep(5000);
            List<User> users = new List<User>();
            CameraSpacePoint[] facecsp = new CameraSpacePoint[Num + 1];
            while (true)
            {
                try
                {

                    //users = Function.BodyDetect_GetAllusers();
                    //break;
                    FaceInfo[] faces = Function.Vision_FaceDetect();
                    for (int faceIndex = Num; faceIndex > 0; faceIndex--)
                    {
                        ColorSpacePoint colorCSP = new ColorSpacePoint();

                        Console.WriteLine("FaceIndex = " + faceIndex);
                        Console.WriteLine("FacenUMBER = " + faces.Length);

                        colorCSP.X = faces[faceIndex].FaceLocation.X + (faces[faceIndex].FaceLocation.Width / 2);
                        colorCSP.Y = faces[faceIndex].FaceLocation.Y + (faces[faceIndex].FaceLocation.Height / 2);
                        facecsp[faceIndex] = Function.Vision_GetCameraSpacePoint(colorCSP);
                        Console.WriteLine("FACE " + faceIndex + " COLOR X= " + colorCSP.X + " Y=" + colorCSP.Y);
                        Console.WriteLine("FACE " + faceIndex + " CAMERA X= " + facecsp[faceIndex].X + " Z=" + facecsp[faceIndex].Z);

                    }
                    break;
                }
                catch { }
            }//得到当前所有人
            Function.Move_Distance(facecsp[Num].Z - 0.5f, facecsp[Num].X, 0);//怼到第一个人上
            //LocationInfo user1 = Function.Location_GetCurrectLocationFromRos();

            string[] name = new string[Num];
            string[] stuff = new string[Num];
            int i = 0;
            while (i <= Num)
            {
                Function.Speech_TTS("Hello my name is wali. What is your name", false);
                ArrayList voiceArrayList = new ArrayList();
                voiceArrayList.Add(new string[] { "my name is" });
                string[] names = new string[] { "Daniel", "Michael", "Jack", "Fisher", "Kevin", "Rose", "John", "Mary", "Adam", "Tom" };
                voiceArrayList.Add(names);
                int[] voiceResult = Function.Speech_Recognize_StartSimpleRecognize(voiceArrayList);
                name[i] = names[voiceResult[1]];

                Function.Speech_TTS("What you want", false);
                voiceArrayList = new ArrayList();
                voiceArrayList.Add(new string[] { "I want a" });
                string[] stuffs = new string[] { "chips", "safeguard", "napkin", "sprite", "porridge", "milk", "laoganma", "icetea", "cola", "water", "toothpaste", "coffee" };
                voiceArrayList.Add(stuffs);
                voiceResult = Function.Speech_Recognize_StartSimpleRecognize(voiceArrayList);
                stuff[i] = stuffs[voiceResult[1]];

                voiceArrayList = new ArrayList();
                voiceArrayList.Add(new string[] { "yes", "no" });
                Function.Speech_TTS("U are " + name[i] + " and U want the " + stuff[i] + ". Am I right? ", false);
                voiceResult = Function.Speech_Recognize_StartSimpleRecognize(voiceArrayList);



                if (voiceResult[0] == 0)
                {
                    Function.Speech_TTS("这是第 " + (i + 1) + " 个人 ", false);

                    Function.Speech_TTS("The next. ", true);
                    if (i < Num)
                    {
                        float nextDistance = facecsp[Num - i].X - facecsp[Num - i - 1].X;
                        Function.Move_Distance(0, -nextDistance, 0);//移动的距离  下面须同时修改
                    }
                    i++;
                }
            }
            Function.BodyDetect_CloseBodyDetectWindow();
            Function.Speech_TTS("Task Over", false);
            //LocationInfo EndPoint = Function.Location_GetLocationInfoByName("EndPoint");
            //Function.Move_Navigate(EndPoint);

        }
        public void FindAndBack(String[] name, String[] stuff)
        {

            LocationInfo Desk = new LocationInfo();
            int i = 0;
            while (i < Num)
            {
                LocationInfo Deskplace = new LocationInfo();
                Deskplace = Function.Location_GetLocationInfoByName(stuff[i]);
                Function.Move_Navigate(Deskplace);
                Function.Speech_TTS("I have got  the " + stuff[i], false);
                GraspStuff(name, stuff, i);    //抓物品
                                               //Function.Move_Navigate(Function.Location_GetLocationInfoByName("facepoint"));
                                               //Function.Move_Distance(0, -1f * i, 0);
                                               //     Function.Speech_TTS("There are five people.",false);
                                               //Function.Speech_TTS("Hi" + name[i] + ". this is the " + stuff[i] + "U want. ", false);
                                               //Function.Speech_TTS("Am I Right?", false);
                                               //ArrayList voiceArrayList = new ArrayList();
                                               // voiceArrayList.Add(new string[] { "yes", "no" });//不识别回答是否正确
                i++;
            }


            while (true)
            {
                try
                {

                    //users = Function.BodyDetect_GetAllusers();
                    //break;
                    FaceInfo[] faces = Function.Vision_FaceDetect();
                    for (int faceIndex = Num; faceIndex > 0; faceIndex--)
                    {
                        ColorSpacePoint colorCSP = new ColorSpacePoint();

                        Console.WriteLine("FaceIndex = " + faceIndex);
                        Console.WriteLine("FacenUMBER = " + faces.Length);

                        colorCSP.X = faces[faceIndex].FaceLocation.X + (faces[faceIndex].FaceLocation.Width / 2);
                        colorCSP.Y = faces[faceIndex].FaceLocation.Y + (faces[faceIndex].FaceLocation.Height / 2);
                        facecsp[faceIndex] = Function.Vision_GetCameraSpacePoint(colorCSP);
                        Console.WriteLine("FACE " + faceIndex + " COLOR X= " + colorCSP.X + " Y=" + colorCSP.Y);
                        Console.WriteLine("FACE " + faceIndex + " CAMERA X= " + facecsp[faceIndex].X + " Z=" + facecsp[faceIndex].Z);

                    }
                    break;
                }
                catch { }
            }//得到当前所有人



        }
        public void GraspStuff(String[] name, String[] stuff, int i)
        {
            CameraSpacePoint csp = new CameraSpacePoint();
            csp.X = 10;
            try
            {
                csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(stuff[i]));
            }
            catch
            {
                Console.WriteLine("Can not find obj " + stuff[i]);
                Function.Speech_TTS("Can not find " + stuff[i], true);
                return;
            }
            Console.WriteLine("Original CameraSpacePoint.Z=" + csp.Z + " X=" + csp.X);
            if (csp.Z > 3 | csp.Z < 0)
            {
                Console.WriteLine("Can not find obj " + stuff);
                Function.Speech_TTS("Can not find " + stuff, true);
                return;
            }
            /*需调试修改数值
             * csp.Z = csp.Z - 0.405f;
             * csp.X = csp.X + 0.022f;
            */
            Function.Speech_TTS("I found " + stuff + ", I will get it.", true);
            Function.Move_Distance(0, csp.X, 0);
            Thread.Sleep(500);
            Function.Move_Distance(csp.Z, 0, 0);
            if (stuff[i] == "icetea")//参数须手动设定
            {
                Function.Hand_Close(150);

            }
            else if (stuff[i] == "toothpaste")
            {
                Function.Hand_Close(155);

            }
            else if (stuff[i] == "napkin")
            {
                Function.Hand_Close(145);

            }
            else if (stuff[i] == "coffee")
            {
                Function.Hand_Close(160);

            }
            else if (stuff[i] == "water")
            {
                Function.Hand_Close(150);

            }
            else if (stuff[i] == "porridge")
            {
                Function.Hand_Close(140);

            }
            else if (stuff[i] == "cola")
            {
                Function.Hand_Close(143);

            }
            else if (stuff[i] == "noodle")
            {
                Function.Hand_Close(128);

            }
            else if (stuff[i] == "floralwater")
            {
                Function.Hand_Close(163);

            }
            else if (stuff[i] == "safeguard")
            {
                Function.Hand_Close(158);

            }
            else if (stuff[i] == "chips")
            {
                Function.Hand_Close(140);

            }
            else if (stuff[i] == "milk")
            {
                Function.Hand_Close(155);

            }
            else if (stuff[i] == "laoganma")
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
