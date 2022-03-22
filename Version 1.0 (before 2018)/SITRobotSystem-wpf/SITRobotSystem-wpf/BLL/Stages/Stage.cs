using System.Threading;
using SITRobotSystem_wpf.BLL.FunctionClass;

namespace SITRobotSystem_wpf.BLL.Stages
{
    public enum StageType
    {
        Stop=0,
        Running=1,
        Pause=-1
    }
    public abstract class Stage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void init();

    }
}
