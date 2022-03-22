using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR
{
    public class ZiZhuZhuTiTask :Tasks.Tasks
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
                    //g.Name = strs[1];
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
            }
            return commands;
        }


        public string speechGetCommandWithDataBase()
        {
            string[] strPlace = new string[10];
            string[] strThing = new string[10];
            string[] strName = new string[10];
            baseSpeech.gpsrRecogize(strPlace, strThing);
            string res = "";
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }
            return res;
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

