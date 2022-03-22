namespace SITRobotSystem_wpf.BLL.State
{

    public enum BaseStateType
    {
        Waiting=0,
        Running=1,
        Stopping=-1
    }
    
    public class BaseState:State
    {
        public static BaseStatePublisher baseStatePublisher=new BaseStatePublisher();

        private static BaseStateType nowState;

        private static float speedX;

        private static float speedY;

        private static float wheelSpeed;
        public void SetNowState(BaseStateType baseState)
        {
            nowState = baseState;
            baseStatePublisher.sync();
        }



    }
}
