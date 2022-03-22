using System;

namespace SITRobotSystem_wpf.BLL.State
{
    public enum ArmStateType
    {
        Waiting = 0,
        Running = 1,
        Stop = -1
    }
    public class ArmState:State
    {
        public ArmState()
        {
            armStatePublisher=new ArmStatePublisher();
        }
        public static ArmStatePublisher armStatePublisher=new ArmStatePublisher();

        private static String nowArmAction;

        private static ArmStateType nowState;

        public void SetNowArmAction(string armAction)
        {
            nowArmAction = armAction;
            armStatePublisher.sync();
        }
        public void SetNowState(ArmStateType armState)
        {
            nowState = armState;
            armStatePublisher.sync();
        }

    }
}
