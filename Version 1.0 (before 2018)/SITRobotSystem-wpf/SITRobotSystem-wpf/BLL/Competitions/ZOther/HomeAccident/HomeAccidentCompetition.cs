using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.HomeAccident
{
    class HomeAccidentCompetition:Competition
    {

        private HomeAccidentStages haStages;

        public HomeAccidentCompetition()
        {
            haStages = new HomeAccidentStages();
        }

        public override void init()
        {
            this.ThreadNameStr = "HomeAccident";
            haStages.init();
        }

        public override void process()
        {
            haStages.ComeIn();
            haStages.AskCommond();
            while (true)
            {
                if (haStages.haTask.baseSpeech.ReturnCommand != null)
                {
                    haStages.UnderStandCommond(haStages.haTask.baseSpeech.ReturnCommand);
                    break;
                }
            }
            haStages.BringObject();
            haStages.Helppeople();
        }
    }
}
