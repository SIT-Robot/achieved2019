using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Zoo
{
    class ZooCompetition:Competition
    {
        private ZooStages zooStages;

        public ZooCompetition()
        {
            zooStages = new ZooStages();
        }

        public override void init()
        {
            this.ThreadNameStr = "Robot-Zoo";
            zooStages.init();
        }

        public override void process()
        {
            while (true)
            {
                zooStages.say();
                zooStages.get();
                Thread.Sleep(3000);
                zooStages.rotate();
            }
        }
    }
}
