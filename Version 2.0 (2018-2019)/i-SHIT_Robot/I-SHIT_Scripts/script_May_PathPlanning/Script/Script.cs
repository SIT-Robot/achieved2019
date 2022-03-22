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
            //Driver.Ros_DisableRosForDebug();
            //Function.Hand_Init();
        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            Function.Vision_WaitForDoor();
            Function.Move_Distance(2f, 0, 0);
            //走到起始点，大喊一声“苟”，然后开始跑5个点，跑到的每个点都分别说到“利”，“国”，“家”，“生”，"死”；
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("startpoint"));
            Function.Speech_TTS("I am ready",false);//“苟”

            string[] place_name = {"livingroom",
            "bedroom",
            "diningroom",
            "kitchen",
            "exit"};

            LocationInfo[] location_info =
            {
                Function.Location_GetLocationInfoByName("livingroom"),     //“利”
                Function.Location_GetLocationInfoByName("bedroom"), //“国”
                Function.Location_GetLocationInfoByName("diningroom"), //“家”，
                Function.Location_GetLocationInfoByName("kitchen"),    //“生”，
                Function.Location_GetLocationInfoByName("exit")         //"死”；
            };
            for (int i = 0; i < 5; i++)
            {
                Function.Speech_TTS("I will go to " + place_name[i], false);

                Function.Move_Navigate(location_info[i]);
                Function.Speech_TTS("I am at " + place_name[i],false);
            }
        }



    }
}