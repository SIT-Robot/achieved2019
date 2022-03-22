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
            //Function.Hand_Init();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            planA();
        }

        public void planA()
        {
            //使用者告诉机器人 想要去的地方，然后机器导航引导使用者 到某地
            string sentence = "";
            Function.Speech_TTS("Your command?", false);
            //"take me to somewhere"
            ArrayList speech = new ArrayList();

            string[] place_name = new string[]
            {
                   "kitchen", "diningroom", "bedroom", "livingroom",
            };

            speech.Add("take me to");
            speech.Add(place_name);
            int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
            while (true)
            {
                if (resultInt[0] == 0)
                {
                    sentence = "Do you want me take you to" + place_name[resultInt[1]] + " ? ";
                    Function.Speech_TTS(sentence, false);
                    speech = new ArrayList();
                    speech.Add(new string[] { "yes", "no" });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        string placename = place_name[resultInt[1]];
                        Function.Speech_TTS("OK I know, I will go to " + place_name, false);
                        Function.Move_Navigate(Function.Location_GetLocationInfoByName(placename));
                    }
                    else { Function.Speech_TTS("Maybe I am wrong.", false); }
                }
            }//end while
        }

        public void planB()
        {
            Function.Move_SetSpeed(1f, 0, 0);
            Function.Speech_TTS("Stop! ", false);
        }



    }
}
