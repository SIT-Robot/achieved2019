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
            Function.Vision_WaitForDoor();
            Function.Move_Distance(1.8f, 0, 0);
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("diningroom"));
            try { Function.Vision_FindObjectByMachineLearning("tea");

                Function.Speech_TTS("I found tea",false);
            }
            catch { }
            try
            {
                Function.Vision_FindObjectByMachineLearning("cup");

                Function.Speech_TTS("I found cup", false);
            }
            catch { }


        }

        public void FuckGetTea()
        {
            //拍完照片后找物品，找到物品后移动对准。
            string obj_position_z = "0";
            string obj_position_x = "0";
            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
            string objname = "tea";
            CameraSpacePoint csp = new CameraSpacePoint();
            try
            {
                csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(objname));
            }
            catch { }
            obj_position_z = csp.Z.ToString();
            obj_position_x = csp.X.ToString();
            Function.Move_Distance(0, float.Parse(obj_position_z), 0);
            Function.Move_Distance(float.Parse(obj_position_x), 0, 0);
            Function.Hand_Close(100);
        }
    }
}
