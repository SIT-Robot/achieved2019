using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.FunctionClass;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2015
{
    /// <summary>
    /// 
    /// </summary>
    class GPSR_2015Task:Tasks.Tasks
    {
        /// <summary>
        /// 根据语音的结果字符串翻译命令
        /// </summary>
        /// <param name="commandStr"></param>
        /// <returns></returns>
        public List<Command> commandTranslate(string commandStr)
        {
            string[] commandsStrings = commandStr.Split('@');
            List<Command> commands = new List<Command>();
            foreach (var comstr in commandsStrings)
            {
                string[] strs = comstr.Split(',');
                int act;
                if (strs[0] == "catch")
                {
                    Goods g = new Goods();
                    g.Name = strs[1];
                    Command command2 = new Command(ActionType.get, g);
                    commands.Add(command2);

                    Place p = new Place();
                    DBCtrl dbCtrl = new DBCtrl();
                    p.Name = dbCtrl.GetPlaceByGood(g).Name;

                    Command command1 = new Command(ActionType.move, p);
                    commands.Add(command1);
                }
                else if (strs[0] == "put")
                {
                    Place p = new Place();
                    p.Name = strs[1];
                    Command command1 = new Command(ActionType.move, p);
                    commands.Add(command1);

                    Goods g = new Goods();
                    g.Name = strs[1];
                    Command command2 = new Command(ActionType.put, g);
                    commands.Add(command2);
                }
                else if (strs[0] == "move")
                {
                    Place p = new Place();
                    p.Name = strs[1];
                    Command command = new Command(ActionType.move, p);
                    commands.Add(command);
                }
                else if (strs[0] == "find")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.find, t);
                    commands.Add(command);
                }
                else if (strs[0] == "say")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.say, t);
                    commands.Add(command);
                }
                else if (strs[0] == "introduce")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.introduce, t);
                    commands.Add(command);
                }
                else if (strs[0] == "follow")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.follow, t);
                    commands.Add(command);
                }
                else if (strs[0] == "tell")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.tell, t);
                    commands.Add(command);
                }
                else if (strs[0] == "ask")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.ask, t);
                    commands.Add(command);
                }
                else if (strs[0] == "count")
                {
                    Goods g = new Goods();
                    g.Name = strs[1];
                    Command command = new Command(ActionType.ask, g);
                    commands.Add(command);
                }
                else if (strs[0] == "give")
                {
                    Goods g = new Goods();
                    g.Name = strs[1];
                    Command command = new Command(ActionType.give, g);
                    commands.Add(command);
                }
            }
            foreach (var com in commands)
            {
                Console.WriteLine(com.action.ToString() + "  " + com.thing.Name.ToString());
            }

            return commands;

        }

        public List<Command> commandTranslate2015(string commandStr)
        {
            commandStr += " .";
            string[] commandsStrings = commandStr.Split('@');
            List<Command> commands = new List<Command>();
            if (commandsStrings[0] == "1.1")
            {// go to the sp and find the sth and go back here
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                string goodsStr= StrProcesser.FindInSequential(strs, "the", 2, "and", 2);

                    //到地点
                    Place p = new Place();
                    p.Name = placeStr;
                    commands.Add(new Command(ActionType.move, p));

                //取得goods
                Goods g = new Goods();
                g.Name = goodsStr;
                commands.Add(new Command(ActionType.get, g));
                //返回
                commands.Add(new Command(ActionType.back, new Place()));
            }
            else if (commandsStrings[0] == "1.2")
            {// 1.2:get(take/grasp) the 物品 from 地点 and bring(carry/deliver/take) it to 人名 in(at/which is in) the 地点
                string[] strs = commandsStrings[1].Split(' ');
                string goodsStr = StrProcesser.FindInSequential(strs, "the", "from");
                string placeStr = StrProcesser.FindInSequential(strs, "the", 2, "and", 1);
                string peopleStr = StrProcesser.FindInSequential(strs, "to", "in");
                if (peopleStr == "null")
                {
                    peopleStr = StrProcesser.FindInSequential(strs, "to", "at");
                    if (peopleStr == "null")
                    {
                        peopleStr = StrProcesser.FindInSequential(strs, "to", "which");
                    }
                }
                string goalStr = StrProcesser.FindInReverse(strs, "the", ".");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //取得物品
                Goods g = new Goods();
                g.Name = goodsStr;
                commands.Add(new Command(ActionType.get, g));
                //移动至终点
                Place pgoal = new Place();
                pgoal.Name = goalStr;
                commands.Add(new Command(ActionType.move, pgoal));
                //找到某人
                Person per = new Person();
                per.Name = peopleStr;
                per.place = pgoal;
                commands.Add(new Command(ActionType.find, per));
                //伸手给
                commands.Add(new Command(ActionType.give, new Thing()));
            }
            else if (commandsStrings[0] == "1.3")
            {// 1.2:get(take/grasp) the 物品 from 地点 and bring(carry/deliver/take) it to the 地点
                string[] strs = commandsStrings[1].Split(' ');
                string goodsStr = StrProcesser.FindInSequential(strs, "the", "from");
                string placeStr = StrProcesser.FindInSequential(strs, "the", 2, "and", 1);
                string goalStr = StrProcesser.FindInReverse(strs, "the", ".");

                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));

                //取得物品
                Goods g = new Goods();
                g.Name = goodsStr;
                commands.Add(new Command(ActionType.get, g));
                //移动至终点
                Place pgoal = new Place();
                pgoal.Name = goalStr;
                commands.Add(new Command(ActionType.move, pgoal));
                //找到某人

                commands.Add(new Command(ActionType.put, new Thing()));

                //回到start
                commands.Add(new Command(ActionType.back2,new Place()));
            }

            else if (commandsStrings[0] == "2.1")
            {//2.1:go to(navigate to/reach/get into) the 房间名 find(look for) the 物品名
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "find");
                string goodsStr = StrProcesser.FindInReverse(strs, "the", ".");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找物品,找的物品的地点放在g.place中
                Goods g = new Goods();
                g.Name = goodsStr;
                g.PlaceName = placeStr;
                commands.Add(new Command(ActionType.lookfor, g));

            }
            else if (commandsStrings[0] == "3.1")
            {//3.1:find(look for) a person in the 房间名 and answer a question //问题随机出
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
               Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.answeraquestion, new Thing()));


            }
            else if (commandsStrings[0] == "3.2")
            {//3.2:find(look for) a person in the 房间名  and tell(say/speak) your name
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.tellyourname, new Thing()));

            }
            else if (commandsStrings[0] == "3.3")
            {//3.3:find(look for) a person in the 房间名  and tell(say/speak) the name of your team
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.tellthenameofyourteam, new Thing()));

            }
            else if (commandsStrings[0] == "3.4")
            {//3.4:find(look for) a person in the 房间名  and tell(say/speak) the time
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.tellthetime, new Thing()));

            }
            else if (commandsStrings[0] == "3.5")
            {//3.5:find(look for) a person in the 房间名  and tell(say/speak) what time is it
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.tellwhattimeisit, new Thing()));

            }
            else if (commandsStrings[0] == "3.6")
            {//3.6:find(look for) a person in the 房间名  and tell(say/speak) tell the date //你没有看错 可能是两个tell连着
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                commands.Add(new Command(ActionType.telltellthedate, new Thing()));


            }
            else if (commandsStrings[0] == "3.7")
            {//3.7:find(look for) a person in the 房间名  and tell(say/speak) what day is today(tomorrow)
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                string dayStr = StrProcesser.FindInReverse(strs, "is", ".");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                if (dayStr == "today")
                {
                    commands.Add(new Command(ActionType.tellwhatdayistoday, new Thing()));
                }
                else { commands.Add(new Command(ActionType.tellwhatdayistomorrow, new Thing())); }



            }
            else if (commandsStrings[0] == "3.8")
            {//3.8:find(look for) a person in the 房间名  and tell(say/speak) tell the day of the month(week) //又是两个tell
                string[] strs = commandsStrings[1].Split(' ');
                string placeStr = StrProcesser.FindInSequential(strs, "the", "and");
                string dayStr = StrProcesser.FindInReverse(strs, "the", ".");
                //到地点
                Place p = new Place();
                p.Name = placeStr;
                commands.Add(new Command(ActionType.move, p));
                //找到某人
                Person per = new Person();
                per.place = p;
                commands.Add(new Command(ActionType.find, per));
                //回答问题
                if (dayStr == "month")
                {
                    commands.Add(new Command(ActionType.tellthedayofthemonth, new Thing()));
                }
                else { commands.Add(new Command(ActionType.tellthedayoftheweek, new Thing())); }

            }

            return commands;
        }

        public void recognizeQuestion()
        {
            baseSpeech.baseRecognize();
        }

        public string speechGetCommandWithDataBase()
        {
            DBCtrl dbCtrl = new DBCtrl();
            string[] strPlace = dbCtrl.GetAllPlaceName();
            string[] strThing = dbCtrl.GetAllGoodsName();
            baseSpeech.gpsrRecogize(strPlace, strThing);
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            return res;
        }

        public void AnswerQue()
        {

        }

        public string GPSR2015()
        {
            DBCtrl dbCtrl = new DBCtrl();
            //string[] strPlace = { "startpoint", "entrance", "diningroom", "bedroom", "book cabinet", "livingroom", "teatable", "kitchen", "kitchentable", "exit" };
            //string[] strObject = { "startpoint", "entrance","diningroom", "bedroom", "book cabinet", "livingroom", "teatable", "kitchen", "kitchentable", "exit" }; 
            string[] strPlace = { "startpoint", "table", "desk" };

            string[] strObject = { "startpoint","table","desk"};

            string[] strName = { "Tom", "Paul", "Kevin", "Tracy", "John", "Angel", "Jamie", "Fisher", "Shirley", "Robin", "Daniel","Green" };
            string[] strThing = { "tea",  "yellowbottle" };
            //string[] strThing = dbCtrl.GetAllGoodsName();
            baseSpeech.GPSR2015Test(strThing, strObject, strPlace, strName);
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            Console.WriteLine(res);
            return res;
        }

        /// <summary>
        /// 跟随
        /// </summary>
        public void Follow()
        {
            Console.WriteLine("123");
            //gpsrFollow.GPSR_Follow();
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
    }
}
