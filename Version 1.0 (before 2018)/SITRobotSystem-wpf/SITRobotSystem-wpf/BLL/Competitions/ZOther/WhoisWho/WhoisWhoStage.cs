using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.ZOther.WhoisWho;
//add
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.BLL.Connection;

namespace SITRobotSystem_wpf.BLL.Competitions.WhoisWho
{
    class WhoisWhoStage : Stage
    {

        private List<string> name; //name链表保存rememberName()返回的CommandList
        public WhoisWhoStage()
        {
            whoisWhoTask = new WhoisWhoTask();
        }
        private WhoisWhoTask whoisWhoTask;
        public BaseCtrl baseCtrl = new BaseCtrl();
        System.DateTime currentTime = new System.DateTime();
        public KinectSensor kinectSensor;
        public SitRobotSpeech baseSpeech = new SitRobotSpeech();

        public override void init()
        {
            whoisWhoTask.initSurfFrmae();
            whoisWhoTask.initSpeech();
            whoisWhoTask.initBodyDetect(); //
            whoisWhoTask.initMachineLearning();
        }
        /// <summary>
        /// 进门
        /// </summary>
        public void ComeIn()
        {
            //if (whoisWhoTask.WaitForDoor())
            {
                baseCtrl.moveToDirectionSpeed(2f, 0);
                //whoisWhoTask.moveToPlaceByName("who");
            }
        }


        public List<string> NameList = new List<string>();
        public List<string> CommandStrList = new List<string>();
        /// <summary>
        /// 走进人并记住人
        /// </summary>
        /// <returns></returns>
        public List<Command> rememberPeople()
        {
            whoisWhoTask.speak("start finding people!");
            List<User> users = new List<User>();
            List<User> finalUsers = new List<User>();
            while (users.Count == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(200);
                    users = whoisWhoTask.getAllUser();
                    foreach (var user in users)
                    {
                        if (finalUsers.Find(us => us.ID == user.ID) == null)
                        {
                            finalUsers.Add(user);
                            Console.WriteLine("users++" + users.Count);
                        }

                    }
                }
                users = whoisWhoTask.getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            int numofbody = 0;
            foreach (var user in finalUsers)
            {
                if (user.ID != 0)
                {
                    numofbody++;
                    usersposition.Add(user.BodyCenter);
                }
            }
            //whoisWhoTask.speak("I find "+numofbody+"people");
            foreach (var position in usersposition)
            {
                whoisWhoTask.moveToDirection(position.Z - 1.0f, -position.X, 0);
                whoisWhoTask.speak("nice to meet you.");
                ClientSocket.GetInstance();
                Thread.Sleep(3000);
                while (true)
                {
                    whoisWhoTask.ask();
                    ClientSocket.GetInstance().SendMessagg("recognize the people " + whoisWhoTask.whoisWhoName);
                    if (ClientSocket.GetInstance().ReceiveMessage() == "OK")
                        break;
                }

                NameList.Add(whoisWhoTask.whoisWhoName);
                foreach (var Name in NameList)
                {
                    Console.WriteLine(Name);
                }
                CommandStrList.Add(whoisWhoTask.whoisWhoCommand);
                foreach (var Commond in CommandStrList)
                {
                    Console.WriteLine(Commond);
                }
                whoisWhoTask.moveToPlaceByName("who");
            }

            return new List<Command>();
        }

        /// <summary>
        /// 移动到人的面前
        /// </summary>
        public void moveToPeople()
        {
           
            for (int i = 0; i < 3; i++)
            {
                List<User> users = whoisWhoTask.getAllUser(); //寻找场景中的人
                try
                {
                    whoisWhoTask.moveToPlaceByUser(users[0]);   //改过距离 0.9 到 1.5
                    Console.Write("found.");
                }
                catch (Exception e)
                {

                    Console.Write("damn it:" + e);
                }
            }
        }

        /// <summary>
        /// 记住3个客人的名字和脸
        /// </summary>
        public void RememberPeople()
        {

            name = whoisWhoTask.RememberName();  //这里的name保存了3个人名
        }

        public void getstuff(int j)
        {
            whoisWhoTask.speak("I am finding " + WhoisWhoTask.stuffName[j]);
            whoisWhoTask.getStuff(WhoisWhoTask.stuffName[j]);
        }
        public void freeHand()
        {
            whoisWhoTask.free_hand();
        }



        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="command"></param>
        public void processCommand(Command command)
        {

        }

        public IList<List<Command>> commandTra()
        {
            IList<List<Command>> resCommandListList = new ArraySegment<List<Command>>();
            foreach (var cmdstr in this.CommandStrList)
            {
                resCommandListList.Add(whoisWhoTask.commandTranslate(cmdstr));
            }
            return resCommandListList;
        }

        public void getout()
        {

            whoisWhoTask.getout();
        }

        public void moveCatch()
        {
            whoisWhoTask.moveToPlaceByName("teatable");
        }

        public void moveLiving()
        {
            whoisWhoTask.moveToPlaceByName("diningroom");
        }

        public void moveToRoom()
        {
            whoisWhoTask.moveToPlaceByName("bedroom");
        }

        public void findPeople()
        {
            whoisWhoTask.speak("start finding people!");
            List<User> users = new List<User>();
            List<User> finalUsers = new List<User>();
            while (users.Count == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    users = whoisWhoTask.getAllUser();
                    foreach (var user in users)
                    {
                        if (finalUsers.Find(us => us.ID == user.ID) == null)
                        {
                            finalUsers.Add(user);
                        }

                    }
                }
                users = whoisWhoTask.getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            int numofbody = 0;
            foreach (var user in finalUsers)
            {
                if (user.ID != 0)
                {
                    numofbody++;
                    usersposition.Add(user.BodyCenter);
                }
            }

            whoisWhoTask.speak("I find " + numofbody + "people");

            foreach (var position in usersposition)
            {
                whoisWhoTask.moveToDirection(position.Z - 1.0f, -position.X, 0);

                whoisWhoTask.speak("I am back.");

                //whoisWhoTask.ask();
                NameList.Add(whoisWhoTask.whoisWhoName);
                CommandStrList.Add(whoisWhoTask.whoisWhoCommand);
                whoisWhoTask.moveToPlaceByName("startpoint");
            }

        }

        public void LookForPeople()
        {
            int i;
            for (int j = 0; j < 6; j++)   //旋转6次
            {
                for (i = 0; i < 5; i++) //寻找5次，3个客人1个队员1个不认识的。
                {
                    whoisWhoTask.speak("I am looking for People.");//提示
                    List<User> users = whoisWhoTask.getAllUser(); //寻找场景中的人
                    //if(users.Count != 0)
                    try   //处理异常
                    {
                        whoisWhoTask.moveToPlaceByUser(users[0]);   //改过距离 0.9 到 1.1
                        whoisWhoTask.moveDirectionBySpeed(-0.9f, 0f);
                    }
                    catch (Exception e)
                    {
                        whoisWhoTask.speak("there is no one in sight!");//
                        break;
                    }
                    List<string> names = whoisWhoTask.FindNames();//寻找名字
                    whoisWhoTask.speak("I find " + names[0]);  //提示
                    Console.Write(Environment.NewLine + "找到这些人");
                    for (int iii = 0; iii < names.Count - 1; iii++) { Console.WriteLine(names[iii]); }
                    Console.WriteLine("===找到人===:", names[0]);

                }//end for with value i
                if (i == 4) //has recognized 5 times
                {
                    break;
                }
                whoisWhoTask.moveDirectionBySpeedW(60);  //旋转60度
            }// end for with value j
        }//end lookforpeople

        public void findOnePeople(int peopleindex)//这里需要先认出 WhoisWhoTask.userName 这里面的第一个人
        {
            if (WhoisWhoTask.userName.Count == 0)
            {
                WhoisWhoTask.userName.Add("Tom");
            }
            whoisWhoTask.speak("I am looking for " + WhoisWhoTask.userName[peopleindex]);//提示
            List<User> users = whoisWhoTask.getAllUser(); //寻找场景中的人
                                                          //if(users.Count != 0)
            List<string> names = whoisWhoTask.FindNames();//寻找名字

            whoisWhoTask.speak("I find " + names[0]);  //提示
            
            Console.Write(Environment.NewLine + "找到这些人");
            //for (int iii = 0; iii < names.Count - 1; iii++) { Console.WriteLine(names[iii]); }
            Console.WriteLine("===找到人===:", names[0]);
        }

        public void speakError(Exception err)
        {
            Console.WriteLine("Error:" + err);
            whoisWhoTask.speak("Sorry. There is an error occured.  let's move to next step. ");
        }
        public void speakFindningpeople()
        {
            
            whoisWhoTask.speak("I am finding people.");
        }
    }
}

