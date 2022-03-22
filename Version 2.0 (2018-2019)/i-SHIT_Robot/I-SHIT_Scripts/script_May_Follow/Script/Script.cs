using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System.Collections;

namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
       
        public override void InitScript()
        {
            Driver.Kinect_InitKinect();
        }
        
        public override void Script_Process()
        {
            Function.Speech_TTS("I am ready", false);
      
            Function.BodyDetect_ShowBodyDetectWindow();
            while (true)
            {
            
                ArrayList speech = new ArrayList();
                speech.Add(new string[] { "Follow me" });
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                if (resultInt[0] == 0)
                {
                    Function.Speech_TTS("Start Follow", true);
                    Function.BodyDetect_StartFollow();
                    Function.Speech_TTS("End Follow", true);
                    Function.Move_SetSpeed(0, 0, 0);
                }
            }
        }
    }
}
