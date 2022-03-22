using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Demonstrate
{
    class DemonstrateCompetition : Competition
    {
        DemonstrateStages DeStage;

        public DemonstrateCompetition()
        {
            DeStage = new DemonstrateStages();
        }

        public override void init()
        {
            this.ThreadNameStr = "Demonstrate";
            DeStage.init();
        }

        public override void process()
        {

            DeStage.Introduce();

            DeStage.GoToStartPoint();
            DeStage.AskCommond();
           
            DeStage.GoToWater();
            Console.WriteLine("3");
            DeStage.CatchObject();

            DeStage.GoToPeople();

            DeStage.AskPour();

            DeStage.PourWater();


            
        }
    }
}
