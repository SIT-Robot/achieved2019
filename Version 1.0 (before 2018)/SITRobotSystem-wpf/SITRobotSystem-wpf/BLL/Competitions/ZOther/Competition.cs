using System.Collections.Generic;
using System.Threading;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions
{
    public abstract class Competition
    {
        public Thread CompetitionThread;
        public string ThreadNameStr = "";

        /// <summary>
        /// 覆盖初始化地点
        /// </summary>
        public abstract void init();
        /// <summary>
        /// 异步执行入口
        /// </summary>
        public void Start()
        {
            init();
            CompetitionThread = new Thread(process);
            CompetitionThread.SetApartmentState(ApartmentState.STA);
            CompetitionThread.IsBackground = true;
            CompetitionThread.Name = ThreadNameStr;
            CompetitionThread.Start();
        }

        /// <summary>
        /// 实现次方法，用作之心任务
        /// </summary>
        public abstract void process();
    }
}
