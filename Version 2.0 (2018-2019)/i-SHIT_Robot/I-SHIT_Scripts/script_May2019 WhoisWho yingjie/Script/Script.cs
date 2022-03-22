using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        }

        /// <summary>
        /// 1.（运动）进入场地
        /// 2.识别人脸，给予语音提示：“请告诉我你的名字”
        ///   2（1）.收到名字后，再读出确认，确认完毕后，拍照，与名字字符串绑定。
        ///   2（2）.说出“下一个”
        /// 3.重复2（3遍）
        /// 4.（运动）人脸识别，靠近
        /// 5.完成后，离场
        ///
        /// </summary>
        public override void Script_Process()
        {
            // Function.Vision_WaitForDoor();
            Thread.Sleep(6000);

            LocationInfo whoiswhoPlace = Function.Location_GetLocationInfoByName("l1");
            Function.Move_Navigate(whoiswhoPlace);
            Function.Speech_TTS("I am ready to remember your name.", false);
            for (int i = 0; i < 3; i++)
            {
                FirstMeet();
                Thread.Sleep(5000);
            }
        // Then, go to the second scene.
        GO_SCENCE:
            Function.Speech_TTS("Go to kitchen?", false);

            ArrayList speech00 = new ArrayList();
            speech00.Add(new string[] { "yes", "no" });
            int[] temp0 = Function.Speech_Recognize_StartSimpleRecognize(speech00);

            if (temp0[0] == 1)
            {
                goto GO_SCENCE;
            }

            GoScene2();
            // todo.
            Function.Move_Distance(0.7f, 0, 0);


            // Third, go to the front of each people and tell its name.
            Thread.Sleep(8000);

        ready_quesiton:

            Thread.Sleep(2000);

            int nRetry1 = 0;
            FaceInfo[] Persons = null;
            ArrayList speech0 = new ArrayList();

            speech0.Add(new string[] { "yes", "no" });
            Function.Speech_TTS("Are you ready?", false);
            int[] temp = Function.Speech_Recognize_StartSimpleRecognize(speech0);

            if (temp[0] == 1)
            {
                goto ready_quesiton;
            }

            while (nRetry1 < 3)
            {
                List<FaceInfo> _p = Function.Vision_BaiduFaceDetect();
                Persons = _p.ToArray();

                if (Persons.Length == 5)
                {
                    break;
                }
                nRetry1++;
                Thread.Sleep(3000);
            }

            if (Persons.Length == 5)
            {
                Function.Speech_TTS("I found five people.", false);
                Console.WriteLine("Find five people successfully.");
            }
            else
            {
                // 一个都没找到肯定是出了问题，为避免后面访问越界，重新询问是否准备
                if (Persons.Length == 0)
                {
                    Function.Speech_TTS("Are there anyone here?", false);
                    Console.WriteLine("没找到人");

                    goto ready_quesiton;
                }

                Function.Speech_TTS("I found " + Persons.Length + " people", false);
                Console.WriteLine("Could not find five people.");

                int nL = Persons[0].FaceLocation.X + Persons[0].FaceLocation.Width / 2;
                int nR = Persons[Persons.Length - 1].FaceLocation.X + Persons[Persons.Length - 1].FaceLocation.Width / 2;

                // 考虑到五个人不是均匀分布
                // 比较哪侧边的人脸离中心最远
                // 图像分辨率 1920*1080，只算 X 轴部分
                if (Math.Abs(nR - 1920 / 2) > Math.Abs(nL - 1920 / 2))
                {
                    // 旋转角度左正右负。右转 20+°
                    // 当前情况应该是已经看到了 1 ~ 4 个人
                    // 稍微偏一点就行了.
                    // 闭环的.
                    // 下同。
                    Function.Move_Distance(0, 0, (float)-Math.PI / 8);
                }
                else
                {
                    Function.Move_Distance(0, 0, (float)Math.PI / 8);
                }
                goto ready_quesiton;
            }

            // Find five person successfully.
            // Calculate the distances among them.

            // 假设现在和人的距离是 2 米.
            double fCameraMan = 2.0f;

            // Who is who.
            //  -   o    o    o    o    o  -
            //  |                          |  1 meter.
            //  |   [---- a - | -----------| 
            //  |       \     |        |
            //  |        \    |        |
            //  |         \   |        |- (fCameraMan - 1) meter.
            //  |          \  |        |
            //  |           \ |        |
            //  |            O         -
            //  double a = GetRealDistanceByPX(fCameraMan, Math.Abs(Persons[0].FaceLocation.X + Persons[0].FaceLocation.Width / 2 - 1920 / 2));

            double a = 0.86f;
            // 跑到第一个人面前
            Function.Move_Distance((float)(fCameraMan - 1), (float)a, 0);

            for (int k = 0; k < 4; ++k)
            {
                Function.Speech_TTS("I found people!!", true);
                Function.Move_Distance(0, -0.55f, 0);
            }

            Function.Speech_TTS("I found people!!", true);

            /*
            for (nRetry1 = 0; nRetry1 < Persons.Length; ++nRetry1)
            {
                Thread.Sleep(2000);

                int nRetry2 = 0;
                string Name = "";

                // 每个人脸尝试 2 次，避免角度和光线问题带来的遗漏
                // 如果还是无法识别，就当做不认识
                while (nRetry2 < 2)
                {
                    Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "OnePerson.jpg");
                    Name = RecognizeMan("OnePerson.jpg");

                    if (Name != "")
                    {
                        break;
                    }
                    else // Name == ""
                    {
                        Thread.Sleep(1000);
                        nRetry2++;
                        continue;
                    }
                }

                if (Name == "") // 不认识
                {
                    Function.Speech_TTS("I have never seen you.", false);
                }
                else
                {
                    Function.Speech_TTS("I have seen you, you are " + Name + ".", false);
                    Thread.Sleep(1000);
                }

                if (nRetry1 < Persons.Length - 1)
                {
                    // Go to next person.
                    double xRight = (Persons[nRetry1 + 1].FaceLocation.X + Persons[nRetry1 + 1].FaceLocation.Width / 2)
                        - (Persons[nRetry1].FaceLocation.X + Persons[nRetry1].FaceLocation.Width / 2);

                    Function.Move_Distance(0, (float)-Math.Abs(GetRealDistanceByPX(1f, xRight)), 0);
                }

            }*/

            Console.WriteLine("Go to the end point.");
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("out"));

        }

        // 通过人和摄像机的距离，以及照片中两个像素点的距离
        // 按比例计算两个像素点实际物体距离（同一平面下）
        // 参数 fCameraMan 是摄像机和人（物体）之间的距离, 单位是米
        // fPX 表示两个像素点间的距离
        // 注意，这么算下来， fCameraMan必须小于 1.5m.
        private double GetRealDistanceByPX(double fCameraMan, double fPX)
        {
            // fCameraMan  PX/cm
            // 1.2         9.38
            // 1.5         7.375
            // 1           11
            // 0.5         23
            // 拟合之后,PX/cm = 15.339x^2 - 46.116x + 42.168 

            // return (fPX / (15.339 * Math.Pow(fCameraMan, 2)  - 46.116 * fCameraMan + 42.168)) / 100.0;
            return (fPX / (Math.Pow(fCameraMan, -1.033f) * 11.193)) / 100;
        }


        void FirstMeet()
        {
            string Name;
            ArrayList speech = new ArrayList();
            ArrayList NameList = new ArrayList();

            Function.Speech_TTS("What's your name?", false);

            string[] Names = { "Jack", "Tom", "Robot", "White" };
            NameList.Add(new string[]
            {
                "My name is"
            }
            );
            NameList.Add(Names);

            while (true)
            {
                int[] resultint = Function.Speech_Recognize_StartSimpleRecognize(NameList);

                if (resultint[0] == 0)
                {
                    Name = Names[resultint[1]];

                    Function.Speech_TTS("Are you " + Name + "?", false);
                    speech = new ArrayList();
                    speech.Add(new string[]
                        {
                            "yes", "no"
                        });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("You are " + Name, false);


                        int i = 0;
                        // 试三次，实在记不住就算了。。。。
                        // 可能的情况有：拍不到人脸或人脸数量大于 1
                        while (i < 3)
                        {
                            File.Delete("temp.jpg"); // 防止文件创建失败
                            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "temp.jpg");

                            int BD_ErrCode = 0;
                            if (SavePeople(Name, "temp.jpg", ref BD_ErrCode))
                            {
                                Console.Write("Successed!");
                                Function.Speech_TTS("hold still, I should momerize you " + Name, false);

                                break;
                            }
                            else
                            {
                                // Error code page:
                                // https://cloud.baidu.com/doc/FACE/Face-Csharp-SDK.html#.E9.94.99.E8.AF.AF.E7.A0.81
                                if (BD_ErrCode == 222202) // 没有人脸
                                {
                                    Function.Speech_TTS("I can't see you clearly, could you stand in front of me?", false);
                                    Thread.Sleep(1000);
                                    continue;
                                }
                            }

                            ++i;
                        }
                        break;
                    }
                    else
                    {
                        Function.Speech_TTS("Maybe I am wrong.", false);
                        Function.Speech_TTS("Can you say your name again?", false);

                    }
                }
            }
        }

        void GoScene2()
        {
            LocationInfo Findpeople = Function.Location_GetLocationInfoByName("k1");
            // Vision_WaitForDoor();
            Function.Move_Navigate(Findpeople);
            // Function.Move_Distance(2.5, 0, 0);
            //如果找到人脸，跑到人脸1m前

            if (Function.Vision_FaceDetect().Length < 5) // 少识别到了人
            {
                // 退后
                Function.Move_Distance(-0.5f, 0, 0);
            }
            else
            {
                ;
            }
        }
        void Byebye()
        {
            //离开场地
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("out"));
        }


        private bool SavePeople(string PersonName, string JPGPhotoPath, ref int ErrCode)
        {
            var APP_ID = "16370422";
            var API_KEY = "TM57LeqKwgUsYcRS0LYyErlW";
            var SECRET_KEY = "G7LAuBaVYRGFpOpYHDuxxQK3LT5NAdQI";

            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 20000;  // 修改超时时间
            var imageType = "BASE64";

            // 如果有可选参数
            var options = new Dictionary<string, object>{
                        {"face_field", "age"},
                        {"max_face_num", 1},
                        {"face_type", "LIVE"}
                    };
            FileStream filestream = new FileStream(JPGPhotoPath, FileMode.Open);

            byte[] bt = new byte[filestream.Length];

            // 调用read读取方法
            filestream.Read(bt, 0, bt.Length);
            var image = Convert.ToBase64String(bt);
            filestream.Close();

            // 添加人脸信息, 注意, 每次测试完需要进入管理页面删除人脸信息
            var result = client.UserAdd(image, imageType, "Competition", PersonName);

            if (Convert.ToDecimal(result["error_code"]) == 0) // Success.
            {
                return true;
            }
            else  // 没有人脸等情况.
            {
                ErrCode = Convert.ToInt32(result["error_code"]);
                Console.WriteLine("BaiduAPI returned error code: " + ErrCode);

                // Thread.Sleep(900);
                return false;
            }
        }
        // End of function.

        string RecognizeMan(string PhotoPath)
        {
            var APP_ID = "16370422";
            var API_KEY = "TM57LeqKwgUsYcRS0LYyErlW";
            var SECRET_KEY = "G7LAuBaVYRGFpOpYHDuxxQK3LT5NAdQI";

            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 20000;  // 修改超时时间
            var imageType = "BASE64";

            var options = new Dictionary<string, object>{
                        {"face_field", "age"},
                        {"max_face_num", 1},
                        {"face_type", "LIVE"}
                    };
            FileStream filestream = new FileStream(PhotoPath, FileMode.Open);

            byte[] bt = new byte[filestream.Length];

            filestream.Read(bt, 0, bt.Length);
            var image = Convert.ToBase64String(bt);
            filestream.Close();

            var result = client.Search(image, imageType, "Competition", options);
            Console.WriteLine("RecognizePeople: ");
            Console.WriteLine(result);


            if (Convert.ToDecimal(result["error_code"]) == 0) // Success.
            {
                // 判定标准 Score > 80.
                if (Convert.ToDouble(result["result"]["user_list"][0]["score"]) > 80)
                {
                    return Convert.ToString(result["result"]["user_list"][0]["user_id"]);
                }
                return "";
            }
            else
            {
                return "";
            }
        }
    }// end of namespace.
}