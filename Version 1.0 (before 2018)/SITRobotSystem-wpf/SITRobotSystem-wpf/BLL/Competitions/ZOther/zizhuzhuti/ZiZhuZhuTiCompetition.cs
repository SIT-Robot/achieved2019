using System;
using System.Collections.Generic;
using SITRobotSystem_wpf.BLL.Competitions.GPSR;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.zizhuzhuti
{
    class zizhuzhuti:Competition
    {
        private ZiZhuZhuTiStages ziZhuZhuTiStages;
        public zizhuzhuti()
        {
            ziZhuZhuTiStages = new ZiZhuZhuTiStages();
           
        }


        public override void init()
        {
            this.ThreadNameStr = "ZiZhuZhuTi";
            ziZhuZhuTiStages.init();
        }

        public override void process()
        {
            List<Command> commandlist = ziZhuZhuTiStages.SpeechReconigizeCommand();
            foreach (var command in commandlist)
            {
                Console.WriteLine(command.action+command.thing.ToString());
                ziZhuZhuTiStages.ProcessCommand(command);
            }
        }
    }
}
