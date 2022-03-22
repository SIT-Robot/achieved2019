namespace SITRobotSystem_wpf.BLL.State
{
    public enum VoiceStateType
    {
        Stop = -1,
        Reconigizing =1,
        Waiting=0,
        Saying=2
    }
    class VoiceState:State
    {
        public static  VoiceStatePublisher voiceStatePublisher=new VoiceStatePublisher();
        public static  VoiceStateType nowState;
        public static string showText;
        public void SetNowState(VoiceStateType State)
        {
            nowState = State;
            
            voiceStatePublisher.sync();
        }

        public void PublishText(string text)
        {
            showText = text;
            voiceStatePublisher.sync();
        }
    }
}
