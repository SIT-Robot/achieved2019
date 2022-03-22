using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR
{
    class ZiZhuZhuTiStages:Stage
    {
        private ZiZhuZhuTiTask ziZhuZhuTiTask;
        public ZiZhuZhuTiStages()
        {
            ziZhuZhuTiTask = new ZiZhuZhuTiTask();
        }

        /// <summary>
        /// 初始化开启窗口服务等
        /// </summary>
        public override void init()
        {
            ziZhuZhuTiTask.initSurfFrmae();
            ziZhuZhuTiTask.initSpeech();
        }

        /// <summary>
        /// 自主进场
        /// </summary>
        public void ComeIn()
        {
            if (true)
            {
                ziZhuZhuTiTask.moveToPlaceByName("startpoint");
            }
        }

        /// <summary>
        /// 正确理解命令(机器人完整复述相应的任务) …………………………200 分
        /// </summary>
        public List<Command>  SpeechReconigizeCommand()
        {
            ziZhuZhuTiTask.speakReady();
            string commandStrs="";
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = ziZhuZhuTiTask.speechGetCommand();
            }
            List<Command> commands = ziZhuZhuTiTask.commandTranslate(commandStrs);
            return commands;
        }

        /// <summary>
        /// 完成执行任务
        /// </summary>
        /// <param name="command"></param>
        public void ProcessCommand(Command command)
        {
            switch (command.action)
            {
                case ActionType.put:
                    Thread.Sleep(1000);
                    ziZhuZhuTiTask.ArmPut();
                    break;

                case ActionType.move:
                    ziZhuZhuTiTask.moveToPlaceByName(command.thing.Name);
                    break;

                case ActionType.get:
                    Goods g = new Goods();
                    g = ziZhuZhuTiTask.GetGoodsByName(g.Name);
                    ziZhuZhuTiTask.FaceToGoods(g);
                    Thread.Sleep(1000);
                    ziZhuZhuTiTask.ArmGet();
                    break;

                default:
                    break;

            }
        }
    }
}
