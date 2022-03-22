using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Non_finite
{
    class Non_finiteCompetition:Competition
    {
        private Non_finiteStage non_finiteStage;
        public Non_finiteCompetition()
        {
            non_finiteStage=new Non_finiteStage();
        }
        public override void init()
        {
            ThreadNameStr = "Non_finite";
            non_finiteStage.init();
        }

        public override void process()
        {
            throw new NotImplementedException();
        }
    }
}
