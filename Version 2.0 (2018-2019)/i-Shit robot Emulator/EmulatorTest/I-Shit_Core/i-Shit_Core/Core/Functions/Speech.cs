using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using i_Shit_Core.Core.Drivers;

namespace i_Shit_Core.Core.Functions
{
    //语音识别
    public static partial class Function
    {
        public static double ConfidenceThreshold = 0;

        /// <summary>
        /// Grammer为传入一个ArrayList，其中里面放一个个string[]为一个个句子成份，string[]里放一个个枚举，每个枚举中的同义词用|分开。
        /// 详见Dev Guide 2，里面还有Example。
        /// </summary>
        /// <param name="_commandsStringsList"></param>
        /// <returns></returns>
        public static int[] Speech_Recognize_StartSimpleRecognize(ArrayList _commandsStringsList)
        {
            int[] returnStringArray = null;
            RecognizerInfo ri = Driver.Voice_Recognize_GetKinectRecognizer();
            SpeechRecognitionEngine speechEngine = new SpeechRecognitionEngine(ri.Id);
            Grammar gm = Driver.Voice_Recognize_CreatGrammar(_commandsStringsList, ri, "grammer");
            Driver.Voice_Recognize_StartRecognize(speechEngine, gm,
            delegate (Object sender, SpeechRecognizedEventArgs e)//用于回调(识别到)，外部调这个没有意义
            {
                if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "grammer")
                {
                    Console.WriteLine("语音识别结果: Recognized: " + e.Result.Text.ToString());
                    int[] sr = new int[_commandsStringsList.Count];
                    for (int i = 0; i < _commandsStringsList.Count; i++)
                    {
                        sr[i] = int.Parse(e.Result.Semantics[i.ToString()].Value.ToString());
                    }
                    returnStringArray = sr;
                }
            },

            delegate (Object sender, SpeechRecognitionRejectedEventArgs e)//用于回调(识别到)，外部调这个没有意义
            {
                Console.WriteLine("语音识别结果: Rejected！！");
            },

            delegate (Object sender, RecognizerUpdateReachedEventArgs e)//用于回调(xxx)，外部调这个没有意义
            {
                Console.WriteLine("语音识别结果: Updated");
            }

            );
            while (returnStringArray == null) ;
            speechEngine.RecognizeAsyncStop();
            speechEngine.Dispose();
            return returnStringArray;
        }

        /// <summary>
        /// 不阻塞，自己搞定degelate，以便做一些骚操作。\n
        /// Grammer为传入一个ArrayList，其中里面放一个个string[]为一个个句子成份，string[]里放一个个枚举，每个枚举中的同义词用|分开。\n
        /// 详见Dev Guide 2，里面还有Example。\n
        /// delegate写法见Dev Guide 2 。当然也可以参考上面的StartSimpleRecognize里面的delegate写法。\n
        /// 返回SpeechRecognitionEngine是为了方便让你关掉它。记住！不用的时候一定要调用返回的SpeechRecognitionEngine.Stop。不然会一直识别。\n
        /// 当然你在delegate里面关也行。
        /// </summary>
        /// <param name="_commandsStringsList"></param>
        /// <param name="_speechRecognizedDelegate"></param>
        public static SpeechRecognitionEngine Speech_Recognize_StartCustomDelegateRecognize(ArrayList _commandsStringsList, EventHandler<SpeechRecognizedEventArgs> _speechRecognizedDelegate)
        {
            RecognizerInfo ri = Driver.Voice_Recognize_GetKinectRecognizer();
            SpeechRecognitionEngine speechEngine = new SpeechRecognitionEngine(ri.Id);
            Grammar gm = Driver.Voice_Recognize_CreatGrammar(_commandsStringsList, ri, "grammer");
            Driver.Voice_Recognize_StartRecognize(speechEngine, gm,
           _speechRecognizedDelegate,

            delegate (Object sender, SpeechRecognitionRejectedEventArgs e)//用于回调(识别到)，外部调这个没有意义
            {
                Console.WriteLine("语音识别结果: Rejected！！");
            },

            delegate (Object sender, RecognizerUpdateReachedEventArgs e)//用于回调(xxx)，外部调这个没有意义
            {
                Console.WriteLine("语音识别结果: Updated");
            }

            );
            return speechEngine;
        }
    }




    //--语音识别



    //TTS
    public static partial class Function
    {
        public static void Speech_TTS(string _whatYouWantToSay, bool _isWaitingFinished)
        {
            if (_isWaitingFinished)
            {
                new Thread(new ParameterizedThreadStart(delegate (object str) { Driver.TTS_Speak((string)str); })).Start(_whatYouWantToSay);
                //这操作很骚，简单解释下为什么这么做。这是为了在新线程里调TTS以不阻塞，
                //【ParameterizedThreadStart】才能传参数但接受类型为object的，为了不重新写一个函数或把Driver层的TTS函数接受参数改object，这里匿名实现一个参数为obj的方法。就这样。
            }
            else
            {
                Driver.TTS_Speak(_whatYouWantToSay);
            }
        }
    }
}
//--TTS
