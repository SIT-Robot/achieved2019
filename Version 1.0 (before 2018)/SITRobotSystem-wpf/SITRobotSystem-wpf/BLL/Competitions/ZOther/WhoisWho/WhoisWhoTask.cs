using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.ZOther.WhoisWho;

namespace SITRobotSystem_wpf.BLL.Competitions.WhoisWho
{
    class WhoisWhoTask : Tasks.Tasks
    {
        public string whoisWhoName = "";
        public string whoisWhoCommand = "";
        public ClientSocket face;
        DBCtrl dbCtrl = new DBCtrl();

        public static List<string> userName = new List<string>();
        public static List<string> stuffName = new List<string>();  //存物品名字
        public Place[] p = new Place[5];

        /// <summary>
        /// 根据语音的结果字符串翻译命令
        /// </summary>
        /// <param name="commandStr"></param>
        /// <returns></returns>
        public  List<Command> commandTranslate(string commandStr)
        { 
            
            List<Command> resList=new List<Command>();
            Place p1 = new Place();
            p1.Name = "shelf";
            Command cmd1=new Command(ActionType.move,p1);
            Goods g = new Goods();
            g.Name = commandStr;
            Command comd2 = new Command(ActionType.get, g);
            Place p2 = new Place();
            p2.Name = "shelf";
            Command cmd3=new Command(ActionType.move,p2);
            Command cmd4 = new Command(ActionType.put, g);

            /*
            string[] commandsStrings = commandStr.Split('@');
            List<Command> commands = new List<Command>();
            //foreach (var comstr in commandsStrings)
            {
                string[] strs = commandStr.Split(',');
                int act;
                if (strs[0] == "catch")
                {
                    Goods g = new Goods();
                    g.Name = strs[1];
                    Command command = new Command(ActionType.get, g);
              //      commands.Add(command);
                }
                else if (strs[0] == "put")
                {
                    Place p = new Place();
                    p.Name = strs[1];
                    Command command1 = new Command(ActionType.move, p);
              //      commands.Add(command1);

                    Goods g = new Goods();
                    //g.Name = strs[1];
                    Command command2 = new Command(ActionType.put, g);
              //      commands.Add(command2);
                }
                else if (strs[0] == "move")
                {
                    Place p = new Place();
                    p.Name = strs[1];
                    Command command = new Command(ActionType.move, p);
              //      commands.Add(command);
                }
            }
             * */
            return resList;
        }



        /// <summary>
        /// 记住客人的名字和脸
        /// </summary>
        /// <returns>名字列表</returns>
        public List<string> RememberName()
        {
            List<string> CommandList = new List<string>();  //创建一个保持人名的链表
            //人对机器说的话:My name is (commands)
            string[] names = { "Tom",   "Angel", "jamie",
                               "Kevin", "Tracy",  };  //指定人名
            string[] things = { "milktea","biscuit",
                                "pie","toothpaste", "coffee"};//?
            string commandStrs = "";
            for (int i = 0; i < 5; i++)//问三次人名，并记住他们的脸 20170815 i=5
            {
                p[i] = new Place();
                baseSpeech = new SitRobotSpeech();
                baseSpeech.robotSpeak("Hello My Name Is Alice");
                commandStrs = "";
                baseSpeech.whoIsWhoRecogine(names, things);     //语言识别             
                while (string.IsNullOrEmpty(baseSpeech.ReturnCommand)) 
                {
                    commandStrs = baseSpeech.ReturnName;   //将人名加入到commandStrs
                }              
                CommandList.Add(commandStrs);  //将保存的人名加入到链表CommandList
                                               /*TrainFace 记住人脸 并保存到C:\Users\ASS\Documents\SIT服务机器人Windows\SITRobotSystem-wpf\SITRobotSystem-wpf\
                                                                                bin\x64\Debug\trainedFaces*/
                                               //Thread.Sleep(2000);
                                               //speak("face me");
                this.moveDirectionBySpeed(-1.1f, 0f);//向后跑1.1米
                //p[i] = this.GetPlace();   // 获取机器人当前位置
                try { 
               this.TrainFace(commandStrs);
                }
                catch { }
                this.moveDirectionBySpeed(0f, -0.5f);//向右移动0.4米
                 this.moveDirectionBySpeed(1.1f, 0f);//向前走1.1米
                baseSpeech.speakStop(); baseSpeech = null;//关掉这层语言
            }
            
            return CommandList;   //返回给上一层的 name 
        }




        /// <summary>
        /// 寻找人
        /// </summary>
        /// <returns></returns>
        public User findPeople(int type)
        {
            List<User> persons = visionCtrl.getAllusers();
            //根据需求选择
            return persons[0];
        }


        public string speechGetCommand()
        {
            baseSpeech.gpsrRecogize();
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            return res;
        }


        public User findUserMiddleRasingHand(List<User> users)
        {
            List<User> handOnUsers = users.FindAll(user => user.isRaisingHand);
            User resUser = new User();
            if (handOnUsers.Count != 0)
            {
                int MinX = 0;
                resUser = handOnUsers[0];
                foreach (var user in handOnUsers)
                {
                    if (Math.Abs(resUser.BodyCenter.X) - Math.Abs(user.BodyCenter.X) > 0)
                    {
                        resUser = user;
                        //speechSay("I Find you !");
                    }
                }
            }

            return resUser;
        }

        public void whoIsWhoRec()
        {
            
        }
        public void ask()
        {
            
            baseSpeech = new SitRobotSpeech();
            string[] name = new string[]
            {"Gray","David","Daniel","Jack","Jenny","Michael","Lucy","Peter","Tom","Jorden"};
            DBCtrl dbCtrl=new DBCtrl();
            string[] things = new []{ "milk", "redbull", "sprite", "rollpaper", "biscuit", "juice", "tea", "chips", "coffee", "toothpaste" };
            baseSpeech.whoIsWhoRecogine(name,things);
            while (whoisWhoName == ""|| whoisWhoCommand=="")
            {
                if (!string.IsNullOrWhiteSpace(baseSpeech.ReturnCommand) && !string.IsNullOrWhiteSpace(baseSpeech.ReturnName))
                {
                    whoisWhoName = baseSpeech.ReturnName;
                    //face.SendMessagg("recogize the people " + whoisWhoName);
                    //string who;
                    //who=face.ReceiveMessage();
                    //Console.WriteLine("Whoiswho"+who);
                    //whoisWhoTask.speak(who);
                    whoisWhoCommand = baseSpeech.ReturnCommand;
                    break;
                }
            }
        }

        public void getout()
        {
            baseSpeech.robotSpeak("Good bye.");
            moveToPlaceByName("exitpoint");
        }


        //拿东西
        public void getStuff(string stuff)
        {
            bool whetherFindStuff;
            Goods obj = GetGoodsByName(stuff);    //milk 为数据库中的数据
            //whetherFindStuff = FaceToGoods2(obj);  //是否找到东西 
            whetherFindStuff = this.lookToGoodsMachineLearing(stuff);//用深度学习找
            if (whetherFindStuff)
            {
                Console.Write("found: " + obj);
            }
            armCtrl.getThing(new ArmAction(600,"get"));//手臂动作
            moveDirectionBySpeedW(180);
            armCtrl.getThing(new ArmAction(1000, "get"));
            moveDirectionBySpeedW(-180);
            
        }

        //放手
        public void free_hand()
        {
            armCtrl.getThing(new ArmAction(1000, "shit"));
        }

    }
}
