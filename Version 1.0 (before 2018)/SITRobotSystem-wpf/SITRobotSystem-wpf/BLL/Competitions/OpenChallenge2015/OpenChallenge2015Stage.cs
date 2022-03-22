using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.OpenChallenge2015
{

    //开放性挑战简要步骤：
    //1.机器人移动到ticket point为访客取票
    //2.机器人检测到招手，移动至sofa说：“here's your registration”
    //3.给了票后问“Would you want some water？” Yes则移至water point 倒水，No则回取票点
    class OpenChallenge2015Stage:Stage
    {
        public OpenChallenge2015Task octask;

        public OpenChallenge2015Stage()
        {
            octask = new OpenChallenge2015Task();
        }
        

        public void Start()     //初始化位置：取票点
        {
            if (true)
                //     octask.ArmGet();
                //     octask.moveToPlaceByName("ticketpoint");
                ;
        }

        public void findPerson()        //看到人招手后到sofa送票
        {
            octask.findPeople();
        }

        public void askCommandAndAction()       //询问指令并且执行操作
        {
            octask.askCommand();
        }

        public override void init()
        {
            octask.initSurfFrmae();
            octask.initSpeech();
            octask.initBodyDetect();
        }


    }
}
