using System.Collections.Generic;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.BaseFunction
{
    class BaseFunctionCompetition : Competition
    {
        private BaseFunctionStage baseFunctionStage;

        public string NavPlace1;
        public string NavPlace2;
        public string Obj;
        public string GoPlace1;
        public string GoPlace2;

        public override void init()
        {
            baseFunctionStage = new BaseFunctionStage();
            baseFunctionStage.init();
        }

        public override void process()
        {
            baseFunctionStage.BaseGetObj(GoPlace1,Obj,GoPlace2);
            
            baseFunctionStage.BaseQuestion();

            baseFunctionStage.Nav(NavPlace1, NavPlace2);
            

        }

        public void runNav()
        {
            
            baseFunctionStage.Nav(NavPlace1, NavPlace2);
        }

        public void runBaseQuestion()
        {
            baseFunctionStage.BaseQuestion();
        }

        public void runGetObj()
        {
            baseFunctionStage.BaseGetObj(GoPlace1,Obj,GoPlace2);
        }

    }
}
