using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions;
using SITRobotSystem_wpf.SITRobotWindow;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.Huawei
{
    public class HuaweiCompetition:Competition
    {
        public HuaweiStage hwStages;
        
        public HuaweiCompetition()
        {
            hwStages = new HuaweiStage();
        }

        public override void init()
        {
            this.ThreadNameStr = "Huawei";
            hwStages.init();
        }

        public override void process()
        {
            hwStages.Point1();
            hwStages.Video();
            hwStages.hwTask.RightTurn();

            hwStages.Point2();
            //hwStages.hwTask.LeftTurn();
            hwStages.Video();
            hwStages.hwTask.RightTurn();

            hwStages.Point3();
            hwStages.Video();
            hwStages.hwTask.RightTurn();

            hwStages.Point4();
            hwStages.Video();
        }
    }
}
