// @sunnysab
// 
// Whoiswho 项目， 2019，中国机器人大赛 · 青岛
// 
// 比赛规则
//
//5.1 场地准备
//3个客人进入场地，面向门，离门4米左右。机器人位于门外，等待开门。
//比赛开始，机器人进入房间，离门3-4米，先进行自我介绍。

//5.2 客人介绍给机器人
//    机器人识别客人，并依次走向每个客人，在此过程中，客人应面向机器人，并向机器人介绍自己，
//    以及自己希望机器人帮自己拿的物品。在此学习阶段，关于要客人如何做，机器人可以给予客人一些指示，
//    但绝对不能触碰机器人。物品从物品清单指定（由裁判抽取），客人的名字从名字清单指定（由裁判抽取）。
//    客人告诉机器人自己的名字和所需物品后，机器人必须说出它所理解到的名字和物品。如果机器人没有听懂，
//    可以要求客人再次重复，不扣分。如果在这个阶段机器人没有正确的识别名字，它仍还可以用该错误的名字继
//    续下一阶段的识别。这是机器人与客人相互认识的阶段。这个阶段客人明确为志愿者，但与机器人之间的对话
//    可以由参赛队指定。

//5.3 获取物品
//    机器人导航到指定位置（该位置与客人不在同一个房间，比如一个房间为客厅，则另一个房间为餐厅）获得客
//    人所需物品，机器人得到所需的一个或几个物品后回到客人所在房间，此时客人应面向机器人。

//5.4 识别人
//    机器人回到房间后找到相应的客人，给予他所需的物品，并询问是否满足要求。

//离开场地
//    当机器人已经为所有客人拿好物品后，或者决定停止寻找时，它从另一个门离开。机器人可以通过自主开门来
//    离开（需事先告诉裁判）。如果机器人不做任何事，直接离开场地，则离场分不能给。

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
        // 生成本次运行时唯一 hash
        // 用作日志记录
        string FilePrxStr = "Record_" + MakePrefixString();

        public override void InitScript()
        {
            // 调试时候注释掉
            Driver.Kinect_InitKinect();
        }


        public override void Script_Process()
        {
            // 场地准备
            //     3个客人进入场地，面向门，离门4米左右。机器人位于门外，等待开门。
            //     比赛开始，机器人进入房间，离门3 - 4米，先进行自我介绍。

            // 进入房间
            Thread.Sleep(6000);
            // Function.Vision_WaitForDoor();
            Function.Move_Distance(3.5f, 0, 0);

            //5.2 客人介绍给机器人
            //    机器人识别客人，并依次走向每个客人，在此过程中，客人应面向机器人，并向机器人介绍自己，
            //    以及自己希望机器人帮自己拿的物品。在此学习阶段，关于要客人如何做，机器人可以给予客人一些指示，
            //    但绝对不能触碰机器人。物品从物品清单指定（由裁判抽取），客人的名字从名字清单指定（由裁判抽取）。
            //    客人告诉机器人自己的名字和所需物品后，机器人必须说出它所理解到的名字和物品。如果机器人没有听懂，
            //    可以要求客人再次重复，不扣分。如果在这个阶段机器人没有正确的识别名字，它仍还可以用该错误的名字继
            //    续下一阶段的识别。这是机器人与客人相互认识的阶段。这个阶段客人明确为志愿者，但与机器人之间的对话
            //    可以由参赛队指定。

            // 走点、面向人
            LocationInfo whoiswhoPlace = Function.Location_GetLocationInfoByName("l1");
            Function.Move_Navigate(whoiswhoPlace);

            // 自我介绍
            Function.Speech_TTS("I'm wali.", true);
            Function.Speech_TTS("I'm from Shanghai Institute of Technology.", true);
            Function.Speech_TTS("I am ready to remember your name.", false);
            for (int i = 0; i < 3; i++)
            {
                // 记住名字和物品
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
            // Item.
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

        // 生成实验记录
        public static string MakePrefixString()
        {
            // const string prefix_string = "Record_";
            return System.DateTime.Now.GetHashCode().ToString() + "_";
        }


        // 记住人员名称和物品
        void FirstMeet()
        {
            string CurrentName;
            ArrayList speech = new ArrayList();
            ArrayList NameList = new ArrayList();
            string[] AllNames = { "Jack", "Tom", "Robot", "White" };

            NameList.Add(new string[]
                {
                    "My name is"
                }
            );
            NameList.Add(AllNames);

            Function.Speech_TTS("What's your name?", false);

            // 记住人员
            while (true)
            {
                int[] resultint = Function.Speech_Recognize_StartSimpleRecognize(NameList);

                if (resultint[0] == 0)
                {
                    CurrentName = AllNames[resultint[1]];

                    Function.Speech_TTS("Are you " + CurrentName + "?", false);
                    speech = new ArrayList();
                    speech.Add(new string[]
                        {
                            "yes", "no"
                        });
                    if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)
                    {
                        Function.Speech_TTS("You are " + CurrentName, false);

                        int i = 0;
                        // 试三次，实在记不住就算了
                        // 可能的情况有：拍不到人脸或人脸数量大于 1
                        while (i < 3)
                        {
                            string _file_name = FilePrxStr + MakePrefixString();
                            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), _file_name);

                            int BD_ErrCode = 0;
                            if (SavePeople(CurrentName, _file_name, ref BD_ErrCode))
                            {
                                Console.Write("Successed!");
                                Function.Speech_TTS("hold still, I should momerize you " + CurrentName, false);

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

            // 记住物品
            string[] AllItems = { "banana", "apple" };

            Function.Speech_TTS("What do you want?", true);

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
                if (Convert.ToDecimal(result["result"]["user_list"][0]["score"]) > 80)
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


        string[] AllNames = { "Jack", "Tom", "Robot", "White" };
        string[] AllItems = { "banana", "apple" };

        ArrayList WantList = new ArrayList();

        void s511waitOpenDoor()
        {
            // 等待开门
            Thread.Sleep(6000);
            // Function.Vision_WaitForDoor();
        }
        void s512enterRoom()
        {
            // 进入房间
            Function.Move_Distance(3.5f, 0, 0);

            // 走点、面向人
            LocationInfo whoiswhoPlace = Function.Location_GetLocationInfoByName("l1");
            Function.Move_Navigate(whoiswhoPlace);
        }
        void s513selfIntro()
        {
            // 自我介绍
            Function.Speech_TTS("I'm wali.", true);
            Function.Speech_TTS("I'm from Shanghai Institute of Technology.", true);
            Function.Speech_TTS("I am ready to remember your name.", false);
        }
        void s521scanMan()
        {
            // 不需要 直接定点
        }

        int s52211waitName()
        {
            // 问名字
            ArrayList NameList = new ArrayList();
            NameList.Add(new string[] { "My name is" });
            NameList.Add(AllNames);
            int[] resultint = Function.Speech_Recognize_StartSimpleRecognize(NameList);
            return resultint[0] == 0 ? resultint[1] : -1;
        }
        bool s52212checkName(int nameIndex)
        {
            // 确认名字
            if (nameIndex == -1)
            {
                return false;
            }
            string CurrentName = AllNames[nameIndex];
            Function.Speech_TTS("Are you " + CurrentName + "?", false);
            ArrayList speech = new ArrayList();
            speech.Add(new string[] { "yes", "no" });
            return Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0;
        }
        int s52231waitItem()
        {
            // 问要什么东西
            ArrayList ItemList = new ArrayList();
            ItemList.Add(new string[] { "I want" });
            ItemList.Add(AllItems);
            int[] resultint = Function.Speech_Recognize_StartSimpleRecognize(ItemList);
            return resultint[0] == 0 ? resultint[1] : -1;
        }
        bool s52232checkItem(int ItemIndex)
        {
            // 确认要什么东西
            if (ItemIndex == -1)
            {
                return false;
            }
            string CurrentItem = AllItems[ItemIndex];
            Function.Speech_TTS("Do you want" + CurrentItem + "?", false);
            ArrayList speech = new ArrayList();
            speech.Add(new string[] { "yes", "no" });
            return Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0;
        }
        int s5221askName_checkName()
        {
            int k = 0;
            int NameIndex = -1;
            Function.Speech_TTS("What's your name?", false);
            NameIndex = s52211waitName();
            while (!s52212checkName(NameIndex) && ++k <= 5)
            {
                Function.Speech_TTS("Maybe I am wrong.", false);
                Function.Speech_TTS("Can you say your name again?", false);
                NameIndex = s52211waitName();
            }
            // 5次没识别到就假设为0
            if (NameIndex == -1) { NameIndex = 0; }
            string CurrentName = AllNames[NameIndex];
            Function.Speech_TTS("Okay, Now I know that you are " + CurrentName, false);
            return NameIndex;
        }
        int s5221askItem_checkItem()
        {
            int k = 0;
            int ItemIndex = -1;
            Function.Speech_TTS("What do you want?", true);
            ItemIndex = s52231waitItem();
            while (s52232checkItem(ItemIndex) && ++k <= 5)
            {
                ItemIndex = s52231waitItem();
            }
            // 5次没识别到就假设为0        
            if (ItemIndex == -1) { ItemIndex = 0; }
            string CurrentItem = AllItems[ItemIndex];
            Function.Speech_TTS("Okay, Now I know that you want " + CurrentItem, false);
            return ItemIndex;
        }
        string s52221captureMan()
        {
            string file_name = FilePrxStr + MakePrefixString();
            Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), file_name);
            return file_name;
        }
        bool s52222saveMan(string CurrentName, string _file_name)
        {
            int BD_ErrCode = 0;
            if (SavePeople(CurrentName, _file_name, ref BD_ErrCode))
            {
                Console.Write("Successed!");
                Function.Speech_TTS("hold still, I should momerize you " + CurrentName, false);
                return true;
            }
            else
            {
                // Error code page:
                // https://cloud.baidu.com/doc/FACE/Face-Csharp-SDK.html#.E9.94.99.E8.AF.AF.E7.A0.81
                if (BD_ErrCode == 222202) // 没有人脸
                {
                    Function.Speech_TTS("I can't see you clearly, could you stand in front of me?", false);
                    Thread.Sleep(1000);
                    return true;
                }
                // 其他错误
                return true;
            }
            // 返回是否成功            
        }
        bool s5222captureMan_saveMan(string CurrentName)
        {
            int k = 0;
            string file_name;
            do
            {
                file_name = s52221captureMan();
            } while (!s52222saveMan(CurrentName, file_name) && ++k <= 3);
            // 返回是否成功
            return k <= 3;
        }
        void s522walkToEachMan_AskName_CheckName_AskItem_CheckItem()
        {
            for (int i = 0; i < 3; i++)
            {
                int NameIndex = s5221askName_checkName();
                string CurrentName = AllNames[NameIndex];
                s5222captureMan_saveMan(CurrentName);
                int ItemIndex = s5221askItem_checkItem();

                WantList.Add(new int[] { NameIndex, ItemIndex });
            }
        }
        void s5311walkToKitchen()
        {
            LocationInfo loc = Function.Location_GetLocationInfoByName("Kitchen");
            Function.Move_Navigate(loc);
        }
        void s5312getItem(int ItemIndex)
        {
            // string CurrentItem = AllItems[ItemIndex];
            // LocationInfo loc = Function.Location_GetLocationInfoByName(CurrentItem);
            // Function.Move_Navigate(loc);
            // 物品不用识别，不管拿什么东西，跑柜子前面就可以
            LocationInfo loc = Function.Location_GetLocationInfoByName("ItemsLocation");
            Function.Move_Navigate(loc);
        }
        void s5313walkToLivingRoom()
        {
            LocationInfo loc = Function.Location_GetLocationInfoByName("LivingRoom");
            Function.Move_Navigate(loc);
        }
        void s5314giveMen(int NameIndex)
        {
            string CurrentName = AllNames[NameIndex];
            LocationInfo loc = Function.Location_GetLocationInfoByName(CurrentName);
            Function.Move_Navigate(loc);

        }
        void s531getEachItem_walkToKitchen_getItem_giveMen()
        {
            foreach (int[] want in WantList)
            {
                int NameIndex = want[0];
                int ItemIndex = want[1];

                s5311walkToKitchen();
                s5312getItem(ItemIndex);
                s5313walkToLivingRoom();
                s5314giveMen(NameIndex);
            }
        }
        void s551tellExit()
        {

            Function.Speech_TTS("I want to leave here, may some one open this door for me?", false);
            Thread.Sleep(1000);
        }
        void s551goExit()
        {
            //离开场地
            Function.Move_Navigate(Function.Location_GetLocationInfoByName("ExitPoint"));
        }
        void Start()
        {
            //5.1 场地准备
            //3个客人进入场地，面向门，离门4米左右。机器人位于门外，等待开门
            s511waitOpenDoor();
            s512enterRoom();
            //比赛开始，机器人进入房间，离门3-4米，先进行自我介绍。
            s513selfIntro();

            //5.2 客人介绍给机器人
            //    机器人识别客人，并依次走向每个客人，在此过程中，客人应面向机器人，并向机器人介绍自己，
            // s521scanMan(); // list mans, items
            //    以及自己希望机器人帮自己拿的物品。在此学习阶段，关于要客人如何做，机器人可以给予客人一些指示，
            //    但绝对不能触碰机器人。物品从物品清单指定（由裁判抽取），客人的名字从名字清单指定（由裁判抽取）。
            //    客人告诉机器人自己的名字和所需物品后，机器人必须说出它所理解到的名字和物品。如果机器人没有听懂，
            //    可以要求客人再次重复，不扣分。如果在这个阶段机器人没有正确的识别名字，它仍还可以用该错误的名字继
            //    续下一阶段的识别。这是机器人与客人相互认识的阶段。这个阶段客人明确为志愿者，但与机器人之间的对话
            //    可以由参赛队指定。
            s522walkToEachMan_AskName_CheckName_AskItem_CheckItem();

            //5.3 获取物品
            //    机器人导航到指定位置（该位置与客人不在同一个房间，比如一个房间为客厅，则另一个房间为餐厅）获得客
            //    人所需物品，机器人得到所需的一个或几个物品后回到客人所在房间，此时客人应面向机器人。

            //5.4 识别人
            //    机器人回到房间后找到相应的客人，给予他所需的物品，并询问是否满足要求。
            s531getEachItem_walkToKitchen_getItem_giveMen();

            //离开场地
            //    当机器人已经为所有客人拿好物品后，或者决定停止寻找时，它从另一个门离开。机器人可以通过自主开门来
            //    离开（需事先告诉裁判）。如果机器人不做任何事，直接离开场地，则离场分不能给。
            s551tellExit();
            s551goExit();
        }
    }// end of namespace.
}