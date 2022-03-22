using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.SetTable2017
{
    class SetTable2017Competition : Competition
    {
        private SetTable2017Stage setTable2017Stage;
        public bool IsNeedRestart = false;
        public bool IsNeedStop = false;
        public SetTable2017Competition()
        {
            setTable2017Stage = new SetTable2017Stage();
        }
        public override void init()
        {
            ThreadNameStr = "SetTable2017Task";
            setTable2017Stage.init();
        }
        public override void process()
        {
            setTable2017Stage.SetTable2017StageStart();
        }
    }
}
