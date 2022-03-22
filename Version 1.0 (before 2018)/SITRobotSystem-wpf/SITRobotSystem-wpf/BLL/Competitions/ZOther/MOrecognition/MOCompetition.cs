using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.MOrecognition;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.nonFinite
{
    class MOCompetition:Competition
    {
        MOStage stage = new MOStage();
        string[] things=new string[]{""};
        public override void init()
        {
            stage.init();
        }

        public override void process()
        {
            
            stage.start(things);
            //stage.PdfTest();
        }
    }
}
