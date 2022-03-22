using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR
{
    class GpsrCompetition:Competition
    {
        private GpsrStages gpsrStages;
        public GpsrCompetition()
        {
            gpsrStages=new GpsrStages();
           
        }

        public override void init()
        {
            this.ThreadNameStr = "GPSR";
            gpsrStages.init();
        }

        public override void process()
        {
            //gpsrStages.ComeIn();
            gpsrStages.testSurf();
            List< Command> commandlist = gpsrStages.SpeechReconigizeCommand();
            //Thread.Sleep();
            foreach (var command in commandlist)
            {
                Console.WriteLine(command.action+command.thing.ToString());
                gpsrStages.ProcessCommand(command);
            }
            //gpsrStages.goOut();
        }


    }
}
