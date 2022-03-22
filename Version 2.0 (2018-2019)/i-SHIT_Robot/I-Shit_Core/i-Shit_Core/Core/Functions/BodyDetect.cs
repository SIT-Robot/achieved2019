/*  
     I am using the naming rules below;
        all value remain lower case   value_name;
        formal parameter: parameter_name_;
        class fields:    _property_name;
        function:    FunctionName();
    I will express the main idea within the function;
    I will express how to use the function outside the function;
    carefully using static value, and write the actual path of the static value;
    static value： value_name_static;
    //special type of value: value_name_type;
    // special type like static,const,
    name space using upper case SPACE_NAME;
    我使用以下命名规则；
        全部变量小写：value_name;
        形参：parameter_name_;
        字段：  _property_name;
        函数：FunctionName();
    函数内部解释函数的思路；
    函数外部解释函数的用法；
    小心使用静态变量，而且注明静态变量的具体位置；
    静态变量：value_name_static;
    命名空间全部大写：SPACE_NAME;
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;//using Kinect body;
using i_Shit_Core.Library;
using System.Threading;
using i_Shit_Core.Core.Drivers;
using System.Collections;

namespace i_Shit_Core.Core.Functions
{

    public static partial class Function
    {
        /// <summary>
        /// 显示BodyDetect可视化窗口
        /// </summary>
        public static void BodyDetect_ShowBodyDetectWindow()
        {
            Driver.UIThreadOperator.Send(delegate
            {


                Driver.bodyDetectWindow.Visibility = System.Windows.Visibility.Visible;
                Driver.bodyDetectWindow.Show();
            }, null);


        }
        /// <summary>
        /// 关闭BodyDetect可视化窗口
        /// </summary>
        public static void BodyDetect_CloseBodyDetectWindow()
        {

            Driver.UIThreadOperator.Send(delegate
            {
                Driver.bodyDetectWindow.Visibility = System.Windows.Visibility.Hidden;
                Driver.bodyDetectWindow.Close();
            }, null);



        }

        /// <summary>
        /// 查找一个手举起来的User（在图像中央的优先）。
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static User BodyDetect_FindUserMiddleRasingHand(List<User> users)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true)
            {
                Thread.Sleep(5);
                bool RaiseHand = false;
                User returnUser = new User();
                Driver.UIThreadOperator.Send(delegate
                {
                    RaiseHand = (bool)Driver.Emulator_BodyDetectWindow.RaiseHandCheckBox.IsChecked;

                }, null);
                if (RaiseHand == true)
                {
                    Driver.UIThreadOperator.Send(delegate
                    {
                        returnUser = Driver.Emulator_BodyDetectWindow.fakeUser;
                    }, null);
                    return returnUser;
                }
                else
                {
                    returnUser = new User();
                    returnUser.BodyCenter.Z = -1;

                    return returnUser;
                }
            }
            try
            {
                foreach (var user in users)
                {
                    user.trackingHand();
                }
            }
            catch { }
            List<User> handOnUsers = users.FindAll(user => user.isRaisingHand);
            User resUser = new User();
            resUser.BodyCenter.Z = -1;
            if (handOnUsers.Count != 0)
            {
                int MinX = 0;
                resUser = handOnUsers[0];
                foreach (var user in handOnUsers)
                {
                    if (Math.Abs(resUser.BodyCenter.X) - Math.Abs(user.BodyCenter.X) > 0)
                    {
                        resUser = user;
                        Console.WriteLine("BodyDetect: Found a User Middle Rasing Hand!");
                    }
                }
            }
            return resUser;
        }

        /// <summary>
        /// 得到一个和一个User的距离的TriPoint，这个TriPoint可以直接扔给Move_Distance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static TriPoint BodyDetect_GetDistancePointToUser(User user)
        {
            TriPoint p = new TriPoint();
            p.X = user.BodyCenter.Z - 0.5f;
            p.Y = user.BodyCenter.X;
            p.Z = (float)(Math.Atan((user.BodyCenter.X / user.BodyCenter.Z) * 180 / Math.PI));
            return p;
        }




        /// <summary>
        /// 通过USERID找到一个User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static User BodyDetect_FindUserByUserID(int userId)
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true)
            {
                Thread.Sleep(5);
                User returnUser = new User();
                Driver.UIThreadOperator.Send(delegate
                {
                    returnUser = Driver.Emulator_BodyDetectWindow.fakeUser;
                }, null);

                return returnUser;
            }
            User LastUser = new User();
            List<User> allUsersList = BodyDetect_GetAllusers();
            User[] allUser = allUsersList.ToArray();

            User u = null;
            for (int i = 0; i < allUser.Length; i++)
            {
                if (allUser[i].ID == userId)
                {
                    u = allUser[i];
                }
            }
            if (allUser.Length == 0)
            {

                return u;
            }
            if (u != null)
            {
                u.confidence = 1;
                LastUser = u;
            }
            else
            {
                for (int i = 0; i < allUser.Length; i++)
                {
                    User user = allUser[i];

                    if (user.BodyCenter.Z > LastUser.BodyCenter.Z)
                    {
                        user.trackingHeight();
                        if (MathPloblems.Distance3D(user.BodyCenter, LastUser.BodyCenter) < 0.5)
                        {
                            user.confidence += 0.3f;
                        }
                        else
                        {
                            user.confidence += 0.1f;
                        }
                        if (user.BodyCenter.Z > 3)
                        {
                            user.confidence -= 0.1f;
                        }
                        if (Math.Abs(LastUser.HeightCharacteristic - user.HeightCharacteristic) > 0.1)
                        {
                            user.confidence += 0.1f;
                        }
                        else
                        {
                            user.confidence -= 0.1f;
                        }
                        Console.WriteLine("BodyDetect: FindUserByUserID: Now ID:" + user.ID.ToString() + "  Height:" + user.UserHeight.ToString() + "  X:" +
      user.BodyCenter.X.ToString() + "  Z:" + user.BodyCenter.Z.ToString() + "  confidence:" +
      user.confidence.ToString());

                    }
                }
                float maxConfidence = 0;
                for (int i = 0; i < allUser.Length; i++)
                {
                    if (maxConfidence < allUser[i].confidence)
                    {
                        maxConfidence = allUser[i].confidence;
                    }
                }
                for (int i = 0; i < allUser.Length; i++)
                {
                    if (Math.Abs(allUser[i].confidence - maxConfidence) < 0.05)
                    {
                        u = allUser[i];
                    }
                }
            }
            Console.WriteLine("BodyDetect: FindUserByUserID: Fonund User By ID");
            return u;
        }



        /// <summary>
        /// 得到所有的User，用List<User>接收
        /// </summary>
        /// <returns></returns>
        public static List<User> BodyDetect_GetAllusers()
        {
            if (Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true)
            {
                Thread.Sleep(5);
                List<User> fakeUsers = new List<User>();
                User returnUser = new User();
                Driver.UIThreadOperator.Send(delegate
                {
                    returnUser = Driver.Emulator_BodyDetectWindow.fakeUser;
                }, null);

                fakeUsers.Add(returnUser);
                return fakeUsers;
            }
            List<User> correntUsers = new List<User>();
            while (true)
            {
                Thread.Sleep(500);
                if (Driver.Kinect_BodyDetect_DataReceived)
                {
                    while (Driver.Kinect_BodyDetect_isBodyReady)
                    {
                        bool usedFlag = false;
                        foreach (var body in Driver.Kinect_BodyDetect_CorrentBodies)
                        {
                            User user = new User();
                            user.body = body;
                            user.sync();
                            user.trackingHand();
                            user.trackingHeight();
                            correntUsers.Add(user);
                            usedFlag = true;
                        }
                        if (usedFlag)
                        {
                            break;
                        }
                    }
                    break;
                }

            }

            return UserTracker.users;
        }

        public static void BodyDetect_StartFollow()
        {
            BodyDetect_StartFollow(false);
        }


        /// <summary>
        /// 总是跟踪视野中的一个人，视野中的人数=1则开始follow，不为1则不follow，不需要举手等任何手势，推手停止
        /// </summary>
        public static void BodyDetect_StartFollowEx()
        {
            float LastRotateSpeed = 0;
            bool FollowFlag = true;
            bool isUserReady = false;
            User user = new User();
            int userId = 0;
            while (FollowFlag)
            {
                List<User> usersList = BodyDetect_GetAllusers();
                try
                {
                    foreach (User userPush in usersList)
                    {
                        userPush.trackingHandLeftPush();
                        userPush.trackingHandRightPush();
                        if (userPush.isHandLeftPush || userPush.isHandRightPush)
                        {
                            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                            Console.WriteLine("BodyDetect: Follow: Hand Pushed! STOP Follow!");
                            FollowFlag = false;
                            break;
                        }

                    }
                }
                catch { }
                while (true)
                {
                    try
                    {
                        foreach (User userPush in usersList)
                        {
                            if (!(Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true))
                            {
                                userPush.trackingHandLeftPush();
                                userPush.trackingHandRightPush();
                            }
                            if (userPush.isHandLeftPush || userPush.isHandRightPush)
                            {
                                new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                                Console.WriteLine("BodyDetect: Follow: Hand Pushed! Stop Follow!");
                                FollowFlag = false;

                            }

                        }
                    }
                    catch { }

                    if (isUserReady || !FollowFlag)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    if (usersList.Count != 0)
                        if (usersList.Count == 1)
                        {
                            user = usersList[0];
                        }
                        else
                        {
                            Console.WriteLine("BodyDetect: Follow: Users Count = " + usersList.Count + " ! It's not 1, So pause follow!");
                        }
                    if (user.ID != 0)
                    {
                        Console.WriteLine("BodyDetect: Follow: Found 1 People, Continue Follow !");
                        userId = user.ID;
                        isUserReady = true;
                        break;
                    }
                }
                User u = usersList.Find(us => us.ID == userId);
                if (u == null)
                {
                    isUserReady = false;
                    Console.WriteLine("BodyDetect: Follow: Lost People, Pause Follow!");



                    new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();

                    userId = 0;

                }
                else
                {
                    TriPoint twist = MathPloblems.twistCompute(u.BodyCenter.X, u.BodyCenter.Y);
                    if (!(Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true))
                    {
                        u.trackingHandLeftPush();
                        u.trackingHandRightPush();
                    }
                    else
                    {
                        Thread.Sleep(100);//很多Emulator里面的Sleep都是让UI线程歇一歇，不然直接卡死。
                    }
                    if ((!u.isHandPush) && (usersList.Count == 1))
                    {
                        Console.WriteLine("BodyDetect: Following...");
                        LastRotateSpeed = (float)twist.Z;
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeedSmooth(twist); })).Start();
                    }
                    else
                    {
                        Console.WriteLine("BodyDetect: Pause Follow!");
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                    }
                    if (u.isHandLeftPush && u.isHandRightPush)
                    {
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                        Console.WriteLine("BodyDetect: Follow: Hand Pushed! Stop Follow!");
                        FollowFlag = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 跟踪中间举手的人。推手停止（跳出这个函数）
        /// </summary>
        public static void BodyDetect_StartFollow(bool isAutoRotateWhenLostPeople)
        {
            float LastRotateSpeed = 0;
            bool PausedAndRotating = false;
            bool FollowFlag = true;
            bool isUserReady = false;
            User user = new User();
            int userId = 0;
            while (FollowFlag)
            {
                List<User> usersList = BodyDetect_GetAllusers();
                try
                {
                    foreach (User userPush in usersList)
                    {
                        userPush.trackingHandLeftPush();
                        userPush.trackingHandRightPush();
                        if (userPush.isHandLeftPush || userPush.isHandRightPush)
                        {
                            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                            Console.WriteLine("BodyDetect: Follow: Hand Pushed! STOP Follow!");
                            FollowFlag = false;
                            break;
                        }

                    }
                }
                catch { }
                while (true)
                {
                    try
                    {
                        foreach (User userPush in usersList)
                        {
                            if (!(Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true))
                            {
                                userPush.trackingHandLeftPush();
                                userPush.trackingHandRightPush();
                            }
                            if (userPush.isHandLeftPush || userPush.isHandRightPush)
                            {
                                new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                                Console.WriteLine("BodyDetect: Follow: Hand Pushed! Stop Follow!");
                                FollowFlag = false;

                            }

                        }
                    }
                    catch { }

                    if (isUserReady || !FollowFlag)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    if (usersList.Count != 0)
                        user = BodyDetect_FindUserMiddleRasingHand(usersList);

                    if (user.ID != 0)
                    {
                        Console.WriteLine("BodyDetect: Follow: Found People, Continue Follow !");
                        userId = user.ID;
                        isUserReady = true;
                        break;
                    }
                }
                User u = usersList.Find(us => us.ID == userId);
                if (u == null)
                {
                    isUserReady = false;
                    Console.WriteLine("BodyDetect: Follow: Lost People, Pause Follow!");

                    if (isAutoRotateWhenLostPeople == true)
                    {
                        if (PausedAndRotating == false)
                        {
                            PausedAndRotating = true;
                            Console.WriteLine("BodyDetect: Auto Rotate Enabled");
                            Thread.Sleep(500);
                            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, LastRotateSpeed); })).Start();
                        }
                    }
                    else
                    {
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                    }
                    userId = 0;

                }
                else
                {
                    TriPoint twist = MathPloblems.twistCompute(u.BodyCenter.X, u.BodyCenter.Y);
                    if (!(Driver.Emulator_Mode == true && Driver.Emulator_BodyDetect_Enable == true))
                    {
                        u.trackingHandLeftPush();
                        u.trackingHandRightPush();
                    }
                    else
                    {
                        Thread.Sleep(100);//很多Emulator里面的Sleep都是让UI线程歇一歇，不然直接卡死。
                    }
                    if (!u.isHandPush)
                    {
                        Console.WriteLine("BodyDetect: Following...");
                        PausedAndRotating = false;//找到人之後重設這個
                        LastRotateSpeed = (float)twist.Z;
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeedSmooth(twist); })).Start();
                    }
                    else
                    {
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                    }
                    if (u.isHandLeftPush && u.isHandRightPush)
                    {
                        new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
                        Console.WriteLine("BodyDetect: Follow: Hand Pushed! Stop Follow!");
                        FollowFlag = false;
                        break;
                    }
                }
            }

        }//end ,,

        /// <summary>
        /// Start a follow progess with voice command (Follow me, stop follow)
        /// 启动一次完整的带语音的Follow进程(Follow me, Stop follow)
        /// </summary>
        public static void BodyDetect_StartFullFollow()
        {
            Console.WriteLine("Started a full voice command Follow.");
            while (true)
            {
                //一直跟随，意外停止while继续。
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "Follow me", "stop follow" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    Function.Speech_TTS("Start Follow", true);
                    Function.BodyDetect_StartFollow();
                    Function.Speech_TTS("End Follow", true);
                    Function.Move_SetSpeed(0, 0, 0);
                }
                else if (resultInt[0] == 1)
                {
                    Console.WriteLine("Follow progress over.");
                    break;
                }
            }

        }
        public static void BodyDetect_FaceToUser(User user_)
        {
            //计算机器人和人的角度，并面向人。
            float right_angle = 0.00f;
            right_angle = (float)(90 - (Math.Tan(user_.BodyCenter.Z / user_.BodyCenter.X)));
            Function.Move_Distance(0, 0, right_angle);
        }//end BodyDetect_FaceToUser

    }
}