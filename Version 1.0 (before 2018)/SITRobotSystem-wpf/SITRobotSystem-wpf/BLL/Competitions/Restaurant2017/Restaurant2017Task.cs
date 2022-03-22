using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2017
{
    class Restaurant2017Task : Tasks.Tasks
    {
        string commandStrs;
        List<string> listorder;
        public Restaurant2017Task()
        {
            baseSpeech = new SitRobotSpeech();
            baseSpeech.setConfidenceThreshold(0.2);
        }
        //接近呼叫的客户
        public User GetAskingCoustomer()
        {
            List<User> usersList;
            User userCoustomer = new User();
            usersList = this.getAllUser();
            do
            {
                Thread.Sleep(500);
                foreach (User user in usersList)
                {
                    if (user.isRaisingHand)
                    {
                        userCoustomer = user;
                    }
                }
            } while (!userCoustomer.isRaisingHand);
            return userCoustomer;
        }

        //听到呼叫
        public void waitForAsk()
        {
            string[] commands = { "waiter", "please" };
            commandStrs = "";
            baseSpeech.ReturnCommand = "";
            //(commands) take the order
            baseSpeech.SetTable2017Recognize(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
            }
        }
        //等待下单命令
        public string waitForOrder(bool isFirst = true)
        {
            baseSpeech = new SitRobotSpeech();
            //机器人的名字，在语法里面写了所以这个没用
            string[] commands = { "sakiya" };
            if (isFirst)
                baseSpeech.robotSpeak("I heard " + commandStrs);
            else
                baseSpeech.robotSpeak("I have finished one task. What should I do next");
            commandStrs = "";
            baseSpeech.ReturnCommand = "";
            //(commands) take the order
            commandStrs = baseSpeech.Restaurant2017Recognize(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
            }
            return commandStrs;
        }

        //下单
        public string AskForOrder()
        {
            //人对机器说的话，可以是多个选择
            string[] commands = { "water", "cola", "Hamburger", "fries", "juice" };
            Thread.Sleep(2000);
            baseSpeech = new SitRobotSpeech();
            baseSpeech.robotSpeak("what do you want");
            commandStrs = "";
            //下面为2种语法
            //please give me the (commands)
            //please give me the (commands) and (commands)
            baseSpeech.Restaurant2017Recognize(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
            }
            Thread.Sleep(1500);
            //string[] strs = commandStrs.Split(' ');
            //string goodsStr = StrProcesser.FindInSequential(strs, "the", ".");
            baseSpeech.robotSpeak("please wait a moment");
            return commandStrs;
        }

        //获得订单中的物体(未完成)
        public void GetObject()
        {
            ArmAction armAction1 = new ArmAction(1000, "get");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);
        }

        //把物体放桌子上(未完成)
        public void PlaceObject()
        {
            this.moveDirectionBySpeed(0.3f, 0);
            ArmAction armAction1 = new ArmAction(301, "put");
            armCtrl.putThing(armAction1);
            this.moveDirectionBySpeed(-0.3f, 0);
            this.moveDirectionBySpeedW(180f);
            Thread.Sleep(800);
        }

        //复述订单命令
        public void RepeatOrder(string order, string PlaceName)
        {
            string speakStr = null;
            listorder = this.TranslatecommandStrs(order);
            foreach (string ord in listorder)
            {
                speakStr = speakStr + ord + ' ';
            }
            speakStr = speakStr + "for " + PlaceName;
            baseSpeech.robotSpeak(speakStr);
        }

        public bool JudgeIsEnd(string commandstr)
        {
            List<string> command = TranslatecommandStrs(commandstr);
            bool result = false;
            if (command.Count == 2)
            {
                if (command[0].Equals("sakiya") && command[1].Equals("stop"))
                    result = true;
            }
            if (!result)
            {
                baseSpeech = new SitRobotSpeech();
                baseSpeech.setConfidenceThreshold(0.2);
            }
            return result;
        }

        //字符串分割函数
        public List<string> TranslatecommandStrs(string commandStrs)
        {
            List<string> commands = new List<string>();
            string[] strs = commandStrs.Split(' ');
            //语法一
            if (strs.Count() == 4)
            {
                commands.Add(strs[0]);
                commands.Add(strs[1]);
            }
            else if (strs.Count() == 2)
            {
                commands.Add(strs[0]);
                commands.Add(strs[1]);
            }
            //语法二
            else if (strs.Count() == 5)
                commands.Add(strs[4]);
            //语法三
            else if (strs.Count() == 7)
            {
                commands.Add(strs[4]);
                commands.Add("and");
                commands.Add(strs[6]);
            }
            return commands;
        }
    }
}
