namespace SITRobotSystem_wpf.BLL.FunctionClass
{
    /// <summary>
    /// 委托publisher发布者
    /// </summary>
    public class Publisher
    {
        //声明一个委托
        public delegate void PublishEventHander();
        //在委托的机制下我们建立以个事件
        public event PublishEventHander OnPublish;

        //事件必须要在方法里去触发，触发方法
        public void issue()
        {
            //如果有人注册了这个事件，也就是这个事件不是空
            if (OnPublish != null)
            {
                detonate();
                OnPublish();
            }
        }
        /// <summary>
        /// 触发委托事件
        /// </summary>
        public virtual void detonate()
        {

        }
    }
}
