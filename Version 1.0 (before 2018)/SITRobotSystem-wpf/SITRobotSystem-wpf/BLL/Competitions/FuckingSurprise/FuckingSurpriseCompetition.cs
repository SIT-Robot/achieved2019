using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SITRobotSystem_wpf.BLL.Competitions;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Competitions.FuckingSurprise
{
    class FuckingSurpriseCompetition : Competition
    {
        public static CameraSpacePoint targetCSP;

        private FuckingSurpriseMethods fuckingsurprisemethods;
        public FuckingSurpriseCompetition()
        {

            fuckingsurprisemethods = new FuckingSurpriseMethods();

        }
        public override void init()
        {
            this.ThreadNameStr = "FuckingSurprise";
            fuckingsurprisemethods.init();
        }
        public override void process()//开始你的表演
        {
            //fuckingsurprisemethods.userSpeakToRobot();       // 人讲话  "i need your help" "i need someting to drink" 
            fuckingsurprisemethods.funnyrobot();                                              // fuckingsurprisemethods.movetoPlaceByName("teatable"); //go to teatable
           // fuckingsurprisemethods.robotReaction();          // 机器人的反应

        }
    }
}
