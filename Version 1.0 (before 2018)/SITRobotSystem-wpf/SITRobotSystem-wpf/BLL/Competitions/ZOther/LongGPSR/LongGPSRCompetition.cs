using System;
using System.Collections.Generic;
using SITRobotSystem_wpf.BLL.Competitions.GPSR;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.LongGPSR
{
    class LongGpsrCompetition:Competition
    {
        private LongGPSRStages longGPSRStages;
        public LongGpsrCompetition()
        {
            longGPSRStages = new LongGPSRStages();
           
        }

        public override void init()
        {
            this.ThreadNameStr = "GPSR";
            longGPSRStages.init();
        }

        public override void process()
        {
            while (true)
            {
                longGPSRStages.ComeIn();
                List<Command> commandlist = longGPSRStages.SpeechReconigizeCommand();
                foreach (var command in commandlist)
                {
                    Console.WriteLine(command.action + command.thing.ToString());
                    longGPSRStages.ProcessCommand(command);
                }
                longGPSRStages.BackToGoal();
            }

        }
    }
}
