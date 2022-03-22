namespace SITRobotSystem_wpf.BLL.FunctionClass
{
    /// <summary>
    /// 单态模式基类
    /// </summary>
    public class Singleton
    {
        /// <summary>
        /// 构造函数私有化,禁止在其他地方实例化改类
        /// </summary>
        private Singleton() { }               //构造函数在其它地方不可实例化了

        private static volatile Singleton _instance;  //声明类，只能在自己内部实现了

        public static Singleton GetInstance()        //判断是否有实例化过，确保只有一个
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }

    }
}
