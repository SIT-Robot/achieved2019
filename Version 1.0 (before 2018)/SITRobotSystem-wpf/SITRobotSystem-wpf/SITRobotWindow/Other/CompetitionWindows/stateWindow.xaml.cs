using System.Windows;
using SITRobotSystem_wpf.BLL.State;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// stateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class stateWindow : Window
    {
        public stateWindow()
        {
            ArmState.armStatePublisher.OnPublish+=SyncArmState;
            BaseState.baseStatePublisher.OnPublish += SyncBaseState;
            VisionState.visionStatePublisher.OnPublish+= SyncVisionState;
            VoiceState.voiceStatePublisher.OnPublish+=SyncVoiceState;
            InitializeComponent();
        }

        /// <summary>
        /// 同步手臂状态
        /// </summary>
        public static void SyncArmState()
        {
            
        }

        /// <summary>
        /// 同步语音状态
        /// </summary>
        public void SyncVoiceState()
        {
            Dispatcher.Invoke(() =>{
                                       RichTBlockVoice.AppendText(VoiceState.showText);
            });  
            
        }

        /// <summary>
        /// 同步底盘状态
        /// </summary>
        public static void SyncBaseState()
        {

        }
        /// <summary>
        /// 同步图像状态
        /// </summary>
        public static void SyncVisionState()
        {

        }


    }
}
