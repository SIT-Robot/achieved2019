using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.ServiceCtrl
{
    /// <summary>
    /// 手臂控制
    /// </summary>
    public class ArmCtrl
    {
        private ArmState state=new ArmState();
        private ArmConnection armConnection;
        public ArmCtrl()
        {
              state.SetNowState(ArmStateType.Stop);
              armConnection = new ArmConnection();
        }
        
        /// <summary>
        /// 拿东西
        /// </summary>
        /// <param name="type"></param>
        public void getThing(ArmAction action)
        {
            armConnection.sendArmAction(action);

            //switch (action.ID)
            //{
            //    case 0:
            //        System.Threading.Thread.Sleep(1000);
            //        break;
            //    case 1:
            //        System.Threading.Thread.Sleep(2000);
            //        break;
            //    case 2:
            //        System.Threading.Thread.Sleep(3000);
            //        break;
            //    default:
            //        break;
            //}
        }
        /// <summary>
        /// 放置
        /// </summary>
        /// <param name="action"></param>
        public void putThing(ArmAction action)
        {
            armConnection.sendArmAction(action);
        }
        /// <summary>
        /// 发送手臂指令
        /// </summary>
        /// <param name="action"></param>
        public void send(ArmAction action)
        {
            armConnection.sendArmAction(action);
        }
    }
}
