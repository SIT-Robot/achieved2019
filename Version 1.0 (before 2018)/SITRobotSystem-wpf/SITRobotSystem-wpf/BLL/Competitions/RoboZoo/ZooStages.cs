using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.Zoo
{
    class ZooStages:Stage
    {
        private ZooTask zooTask;

        public ZooStages()
        {
            zooTask = new ZooTask();
        }

        public override void init()
        {
            zooTask.initSpeech();
            zooTask.initSurfFrmae();
        }

        /// <summary>
        /// 拿
        /// </summary>
        public void get()
        {
            zooTask.ArmGet();
        }

        /// <summary>
        /// zoo说话
        /// </summary>
        public void say()
        {
            zooTask.say("Hello everyone,my name is Well-e,nice to meet you.I come from Shanghai institute of technology college.i am four years old." +
                        "i love all of you very so much.i am sooooooooooooooo exciting.Let me show some actions for you");
        }

        public void rotate()
        {
            zooTask.moveDirectionBySpeedW(360);
        }
    }
}
