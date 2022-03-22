using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    //TTS部分
    public static partial class Driver
    {
        public static void TTS_Speak(string _whatYouWantToSay)
        {
            Console.WriteLine("TTS:" + _whatYouWantToSay);
            SpeechSynthesizer synth = new SpeechSynthesizer();

            synth.SetOutputToDefaultAudioDevice();//设置播放语音设备，为当前默认

            synth.Rate = -3;//设置语速
            synth.Volume = 100;//设置音量
            // 选择输出语音
            //synth.SelectVoice("Microsoft David Desktop - English (United States)");
            // Build a prompt.
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText(_whatYouWantToSay);

            // Speak the prompt.
            synth.Speak(builder);

            //fState.state(state);
        }
    }
    //--TTS部分
}
