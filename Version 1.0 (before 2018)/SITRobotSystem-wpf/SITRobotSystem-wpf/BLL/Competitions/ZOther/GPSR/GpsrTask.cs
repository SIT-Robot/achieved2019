using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Kinect;
using Org.BouncyCastle.Utilities.Collections;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR
{
    public class GpsrTask : Tasks.Tasks
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
                    Command command = new Command(ActionType.get, g);
                    commands.Add(command);
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
                else if(strs[0] == "tell")
                {
                    Thing t = new Thing();
                    t.Name = strs[1];
                    Command command = new Command(ActionType.tell,t);
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
                Console.WriteLine(com.action.ToString()+"  "+com.thing.Name.ToString());    
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

        /// <summary>
        /// 跟随
        /// </summary>
        public void follow()
        {

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

