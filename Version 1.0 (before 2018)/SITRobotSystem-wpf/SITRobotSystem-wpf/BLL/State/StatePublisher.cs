namespace SITRobotSystem_wpf.BLL.State
{
    public class StatePublisher
    {
        public delegate void PublishEventHander();

        public event PublishEventHander OnPublish;

        public void sync()
        {
            //如果有人注册了这个事件，也就是这个事件不是空
            if (OnPublish != null)
            {
                issue();
                OnPublish();
            }
        }

        public virtual void issue()
        {

        }
    }
}
