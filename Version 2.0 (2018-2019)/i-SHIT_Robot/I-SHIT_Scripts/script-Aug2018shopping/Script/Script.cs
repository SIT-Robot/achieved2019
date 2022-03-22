using System;
using System.Collections;
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
        LocationInfo barlocate = new LocationInfo();
        int[] ObjectPosition = new int[4];
        public override void InitScript()
        {
            // Driver.Emulator_Enable();
            Driver.Kinect_InitKinect();
            //Function.Hand_Init();
        }

        public override void Script_Process()
        {

            barlocate = Function.Location_GetCurrectLocationFromRos();
            Thread.Sleep(1000);
            Fuckshopping();
        }
        public void Fuckshopping()
        {
            Function.Speech_TTS("I am ready", false);
            Function.BodyDetect_ShowBodyDetectWindow();

            while (true)
            {
                //一直跟随，意外停止while继续。
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "Follow me", "stop follow" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    Function.Speech_TTS("Start Follow", true);
                    Function.BodyDetect_StartFollow(true);
                    Function.Speech_TTS("End Follow", true);
                    Function.Move_SetSpeed(0, 0, 0);
                }
                else if (resultInt[0] == 1)
                {
                    break;
                }
            }
            Fuckmain();

        }

        public void Fuckmain()
        {
            LocationInfo[] locate = new LocationInfo[4];
            string[] obj = new string[4];
            int i = 0;
            while (i < 4)
            {
                Function.Speech_TTS("What's this", false);
                ArrayList speech = new ArrayList();
                string[] objs = new string[] { "chips", "safeguard", "napkin", "sprite", "porridge", "milk", "laoganma", "icetea", "cola", "water", "toothpaste", "coffee" };
                speech.Add(objs);
                string[] positionString = new string[] { "on your right", "on your left", "on your front" };
                speech.Add(positionString);
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                obj[i] = objs[resultInt[0]];
                ObjectPosition[i] = resultInt[1];
                Function.Speech_TTS(obj[i] + " " + positionString[resultInt[1]] + "?", false);
                ArrayList speech0 = new ArrayList();
                speech0.Add(new string[] { "yes", "no" });
                int[] resultInt0 = Function.Speech_Recognize_StartSimpleRecognize(speech0);
                if (resultInt0[0] == 0)
                {
                    Function.Speech_TTS("ok", false);
                    locate[i] = Function.Location_GetCurrectLocationFromRos();
                    i++;
                    while (true)
                    {
                        //一直跟随，意外停止while继续。
                        speech = new ArrayList();
                        speech.Add(new string[] { "Follow me", "stop follow" });
                        resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                        if (resultInt[0] == 0)
                        {
                            Function.Speech_TTS("Start Follow", true);
                            Function.BodyDetect_StartFollow(true);
                            Function.Speech_TTS("End Follow", true);
                            Function.Move_SetSpeed(0, 0, 0);
                        }
                        else if (resultInt[0] == 1)
                        {
                            break;
                        }
                    }

                }
                else { }
            }
            Function.BodyDetect_CloseBodyDetectWindow();
            ArrayList speech2 = new ArrayList();
            speech2.Add(new string[] { "Here is bar" });
            int[] resultInt2 = Function.Speech_Recognize_StartSimpleRecognize(speech2);
            barlocate = Function.Location_GetCurrectLocationFromRos();
            Function.Speech_TTS("Ok, I have remembered. ", true);
            int[] TargetNum = new int[3];
            string[] Target = new String[3];

            for (int j = 0; j < 3;)
            {
                ArrayList speech3 = new ArrayList();
                speech3.Add(new string[] { "take the" });
                speech3.Add(obj);
                speech3.Add(new string[] { "and go back here" });
                int[] resultInt3 = Function.Speech_Recognize_StartSimpleRecognize(speech3);
                Function.Speech_TTS("ok,I will take the " + obj[resultInt3[1]] + " and go back here", false);
                ArrayList speech4 = new ArrayList();
                speech4.Add(new string[] { "yes", "no" });
                int[] resultInt4 = Function.Speech_Recognize_StartSimpleRecognize(speech4);
                if (resultInt4[0] == 0)
                {
                    Function.Speech_TTS("ok", true); Target[j] = obj[resultInt3[1]];
                    TargetNum[j] = resultInt3[1];
                    j++;
                }
            }


            for (int k = 0; k < 3; k++)
            {



                //plan B:
                //locate[0] = Function.Location_GetLocationInfoByName("desk1");
                //locate[1] = Function.Location_GetLocationInfoByName("shelf");
                //locate[2] = Function.Location_GetLocationInfoByName("desk2");
                //locate[3] = Function.Location_GetLocationInfoByName("desk3");
                //--plan B
                Function.Move_Navigate(locate[TargetNum[k]]);
                //if (TargetNum[k] == 0)
                //{
                //    Function.Hand_GoUp(820);
                //}
                //else if (TargetNum[k] == 1)
                //{ Function.Hand_GoUp(700); }
                //else if (TargetNum[k] == 2)
                //{ Function.Hand_GoUp(850); }
                //else
                //{
                //    Function.Hand_GoUp(850);
                //}

                //GraspStuff(obj, TargetNum[k]);
                Function.Speech_TTS("I have found " + Target[k], false);
                Thread.Sleep(2000);
                Function.Move_Distance(-0.5f, 0, 0);
                Function.Hand_GoDown(50);
                Function.Move_Navigate(barlocate);
                Function.Hand_Open(150);
            }
            Console.WriteLine(obj);
            Console.WriteLine(Target);
            Console.WriteLine(TargetNum);
        }
        public void GraspStuff(string[] obj, int j)
        {

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
                csp.Z = csp.Z - 0.42f;
            csp.X = csp.X + 0.022f;
            Function.Speech_TTS("I found " + obj[j] + ", I will get it.", true);
            Function.Move_Distance(0, csp.X, 0);
            Thread.Sleep(500);
            Function.Move_Distance(csp.Z, 0, 0);
            if (obj[j] == "milk")
            {

                Function.Hand_Close(155);
            }
            else if (obj[j] == "chips")
            {

                Function.Hand_Close(140);
            }
            else if (obj[j] == "safeguard")
            {
                Function.Hand_Close(158);
            }
            else if (obj[j] == "toothpaste")
            {
                Function.Hand_Close(155);
            }
            else if (obj[j] == "laoganma")
            {
                Function.Hand_Close(144);
            }
            else if (obj[j] == "cola")
            {
                Function.Hand_Close(143);
            }
            else if (obj[j] == "sprite")
            {
                Function.Hand_Close(130);
            }
            else if (obj[j] == "coffee")
            {
                Function.Hand_Close(160);
            }
            else if (obj[j] == "icetea")
            {
                Function.Hand_Close(150);
            }
            else if (obj[j] == "water")
            {
                Function.Hand_Close(150);
            }
            else if (obj[j] == "porridge")
            {
                Function.Hand_Close(140);
            }
            else if (obj[j] == "napkin")
                Function.Hand_Close(145);
        }




    }
}


