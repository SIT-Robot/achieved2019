using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using Microsoft.Kinect;

namespace i_Shit_Scirpt.Script {

    public class MyScript : Scripts { 

    /// <summary>
    /// Write Init Code Here. 在这里写初始化Script的代码
    /// Run in UI Thread. 在UI线程中运行
    /// Such as Init Kinect. 比如Init Kinect. 
    /// </summar
    public override void InitScript() {
        //Function.Hand_Init();
        Driver.Kinect_InitKinect();
    }

    /// <summary>
    /// 在这里写Script的过程，可随意阻塞。
    /// </summary>
    public override void Script_Process()

    {
         try
                        {
                            Function.BodyDetect_ShowBodyDetectWindow();
                            List<User> fuckers = Function.BodyDetect_GetAllusers();
                            //float angle = 5;
                            int num = 0;
                            while (fuckers.Count != 1)
                            {
                                /*
                                 * code
                                 */
                                Function.Move_Distance(0, 0, 25);
                            };
                            FaceInfo[] faceinfo = Function.Vision_FaceDetect();
                            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "\\0.jpg");
                            CameraSpacePoint cameraSpacePoint = new CameraSpacePoint();
                            //CameraSpacePoint cameraSpacePoint 

                            //cameraSpacePoint.Z = fuckers[0].BodyCenter.Z - 0.45f;
                            //cameraSpacePoint.X = fuckers[0].BodyCenter.X - 0.02f;

                            double angle = Math.Atan(fuckers[0].BodyCenter.X/fuckers[0].BodyCenter.Z);
                            Function.Move_Distance(0, 0,(float)angle);
                            float Distancepow2= (float) (Math.Pow(fuckers[0].BodyCenter.X,2)+Math.Pow(fuckers[0].BodyCenter.Z,2)); 
                            float Distance = (float) Math.Pow(Distancepow2,0.5);
                            // 走斜边  0.5f 防止怼上去
                            Function.Move_Distance(Distance-0.5f,0,0) ;
                            Thread.Sleep(1000);
                        }
                        catch { }
                        finally { Function.BodyDetect_CloseBodyDetectWindow(); }
        }
    }
}


