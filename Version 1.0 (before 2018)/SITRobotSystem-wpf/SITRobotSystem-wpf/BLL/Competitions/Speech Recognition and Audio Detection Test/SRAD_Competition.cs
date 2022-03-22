using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.Speech_Recognition_and_Audio_Detection_Test
{
    class SRAD_Competition : Competition
    {
        SRAD_Stage SRADSTAGE;

        public SRAD_Competition()
        {
            SRADSTAGE = new SRAD_Stage();
        }
        public override void init()
        {
            this.ThreadNameStr = "SRAD";
            SRADSTAGE.init();
        }

    

        public override void process()
        {
            SRADSTAGE.ReadyToAnswer() ;
        }
    }
}
