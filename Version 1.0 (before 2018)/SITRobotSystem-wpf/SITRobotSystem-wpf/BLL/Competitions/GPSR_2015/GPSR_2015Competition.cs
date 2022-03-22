using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2015
{
    class GPSR_2015Competition:Competition
    {
        private GPSR_2015Stage gPSR_2015Stage;
         
        public GPSR_2015Competition()
        {
            gPSR_2015Stage = new GPSR_2015Stage();
        }

        public override void init()
        {
            this.ThreadNameStr = "GPSR_2015";
            gPSR_2015Stage.init();
        }

        public override void process()
        {
            //注释用于调试

          // gPSR_2015Stage.WaitForDoor();
          // gPSR_2015Stage.Start();

            List<Command> commandlist = gPSR_2015Stage.SpeechReconigizeCommand();
            //Thread.Sleep();
            foreach (var command in commandlist)
            {

                Console.WriteLine(command.action + command.thing.ToString());
            }
            foreach (var command in commandlist)
            {
                Console.WriteLine(command.action + command.thing.ToString());
                gPSR_2015Stage.ProcessCommand2015(command);
            }

            //gPSR_2015Stage.Exit();
        }
    }
}
