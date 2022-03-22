using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.HomeAccident
{
    class HomeAccidentStages:Stage
    {
        public HomeAccidentTask haTask;

        public HomeAccidentStages()
        {
            haTask = new HomeAccidentTask();
        }

        /// <summary>
        /// 初始化开启窗口服务等
        /// </summary>
        public override void init()
        {
            haTask.initSurfFrmae();
            haTask.initSpeech();
            haTask.initBodyDetect();
        }

        /// <summary>
        /// 驾驶进入“受害者”所处的公寓 
        ///观察紧急事件并且意识到时间已经发生（计时开始） 
        ///接近人物
        /// 自动检测事故-----------------------------------------200 
        /// </summary>
        public void ComeIn()
        {
            // 驾驶进入“受害者”所处的公寓
            if (true)
            {
                haTask.moveToPlaceByName("startpoint");
            }
            //观察紧急事件并且意识到时间已经发生（计时开始）
            //接近人物
            haTask.findPeople();
        }

        private Place patient;
        /// <summary>
        /// 询问人物的情况
        ///记录人物的地点
        ///存储收集到的信息并且汇报到救护车
        /// 出事故的人被发现，并且询问他/她的状态---------------100 
        /// </summary>
        public void AskCommond()
        {
            haTask.AskPeople();
            patient = haTask.GetPlace();
        }

        private string command;

        /// <summary>
        /// 询问人物需要拿什么（水，急救箱，手机）
        /// 正确地理解出事故的人要的东西 -----------------------100 
        /// </summary>
        public void UnderStandCommond(string res)
        {
            command =  res;
            //开多线程处理PDF
            //haTask.info = res;
            //Thread thread = new Thread(haTask.PDFThread);
            //thread.IsBackground = true;
            //thread.Start();
        }

        /// <summary>
        /// 带来人物所需的物品
        /// 抓住正确的物品并且提起它超过5秒钟-------------------100 
        /// 向出事故的人提供正确的物品------------------------- 150 
        /// </summary>
        public void BringObject()
        {
            //code
            Place p = new Place();

            p.Name = command;

            Command com1 = new Command(ActionType.move, p);
            
            haTask.ProcessCommand(com1);

            Command com = new Command(ActionType.get, haTask.GetGoodsByName(command));

            haTask.ProcessCommand(com);
    
            haTask.moveToPlace(patient);

            haTask.ArmPut();

        }

        /// <summary>
        /// 去公寓的入口 
        /// 等待救护车或者叫朋友 
        /// 引导救护车/朋友到病人的位置（停止计时）
        /// 在入口等待救护车或者是被通知的朋友，然后引导他们找到出事故的人-------150 
        /// </summary>
        public void Helppeople()
        {
            //code
            haTask.moveToPlaceByName("living room door");

            //检测有人
            User user = new User();
            while (true)
            {
                user = haTask.findUserMiddleRasingHand(haTask.getAllUser());
                if (user.ID != 0)
                {
                    break;
                }
            }
            haTask.GetCloseToUser(user);

            haTask.speak("Please follow me!");

            haTask.moveToPlace(patient);

        }
    }
}
