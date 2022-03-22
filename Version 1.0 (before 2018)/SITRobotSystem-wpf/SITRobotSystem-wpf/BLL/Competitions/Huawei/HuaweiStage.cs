using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using System.Windows;

namespace SITRobotSystem_wpf.BLL.Competitions.Huawei
{
    public class HuaweiStage:Stage
    {
        public HuaweiTask hwTask;
        

        public HuaweiStage()
        {
            hwTask = new HuaweiTask();
        }
        public override void init()
        {
            hwTask.initBodyDetect();
            hwTask.initSpeech();
            hwTask.initSurfFrmae();
        }

        public void Point1()
        {
            hwTask.moveToPlaceByName("Point1");
            hwTask.speak("i have reached waypoint 1");
            hwTask.LeftTurn();
        }

        public void Point2()
        {
            hwTask.moveToPlaceByName("Point2");
            hwTask.speak("i have reached waypoint 2");
            hwTask.LeftTurn();
        }

        public void Point3()
        {
            hwTask.moveToPlaceByName("Point3");
            hwTask.speak("i have reached waypoint 3");
            hwTask.LeftTurn();
        }

        public void Point4()
        {
            hwTask.moveToPlaceByName("Point4");
            hwTask.speak("i have reached waypoint 4");
            hwTask.LeftTurn();
        }

        public void Video()
        {
            hwTask.VideoPlay();
            hwTask.Loop();
        }

    }
}
