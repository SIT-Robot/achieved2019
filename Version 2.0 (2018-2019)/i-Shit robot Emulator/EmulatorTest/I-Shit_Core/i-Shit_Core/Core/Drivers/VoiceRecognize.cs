using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Core.Drivers
{
    //--语音识别
    public static partial class Driver
    {
        public static RecognizerInfo Voice_Recognize_GetCommonRecognizer()
        {

            CultureInfo myCIintl = new CultureInfo("en-US");
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())//获取所有语音引擎  
            {
                if (config.Culture.Equals(myCIintl))
                {
                    Console.WriteLine("语音识别:Found Reconizer:" + config.Description.ToString());
                    return config;
                }//选择识别引擎
            }
            Console.WriteLine("语音识别:Error: No Specch Regconizer Found ! Please check your system settings.");
            return null;

        }

        public static RecognizerInfo Voice_Recognize_GetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // catch预期没有安装识别器的情况。
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("语音识别: Get Kinect Recognizer成功！");
                    return recognizer;
                }
            }
            Console.WriteLine("语音识别: 未找到KinectRecognizer，返回CommonRecognizer");
            return Voice_Recognize_GetCommonRecognizer();//没有Kinect Recoginzer就返回普通的
        }


        public static Grammar Voice_Recognize_CreatGrammar(ArrayList _commandsStringsList, RecognizerInfo _recognizer, string grammerName)
        {

            GrammarBuilder CommandsBuilder = new GrammarBuilder();

            CommandsBuilder.Culture = _recognizer.Culture;
            Console.Write("语音识别: Build a New Grammer: ");
            for (int i = 0; i < (_commandsStringsList.Count); i++)
            {
                Console.Write(Environment.NewLine + i.ToString() + ".{");
                Choices c = new Choices();
                for (int ii = 0; ii < ((string[])_commandsStringsList[i]).Length; ii++)
                {
                    string[] splitedString = (((string[])_commandsStringsList[i])[ii]).Split('|');
                    for (int iii = 0; iii < splitedString.Length; iii++)
                    {
                        Console.Write("\"" + splitedString[iii] + "\"");
                        if (iii != splitedString.Length - 1)
                        {
                            Console.Write(" or ");
                        }
                        c.Add(new SemanticResultValue(splitedString[iii], ii.ToString()));
                    }

                    if (ii != ((string[])_commandsStringsList[i]).Length - 1)
                    {
                        Console.Write(" , ");
                    }
                }
                CommandsBuilder.Append(new SemanticResultKey(i.ToString(), c));
                Console.Write("} ");

            }
            Console.Write(Environment.NewLine);
            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = grammerName;
            Commands.Priority = 1;
            return Commands;
        }



        public static void Voice_Recognize_StartRecognize(SpeechRecognitionEngine _speechEngine, Grammar _grammar, EventHandler<SpeechRecognizedEventArgs> _speechRecognized, EventHandler<SpeechRecognitionRejectedEventArgs> _speechRejected, EventHandler<RecognizerUpdateReachedEventArgs> _recognizerUpdateReached)
        {

            ////fState = new fStateOutput();
            ////fState.Show();
            // 只支持一个传感器



            _speechEngine.LoadGrammar(_grammar);

            _speechEngine.SpeechRecognized += _speechRecognized;
            _speechEngine.SpeechRecognitionRejected += _speechRejected;
            _speechEngine.RecognizerUpdateReached += _recognizerUpdateReached;

            // 为AudioLevelUpdated事件添加一个事件处理程序。ShowRecognized;

            // 让convertStream知道speech活动状态


            // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
            // 随着时间的推移这将防止识别精度降低。
            ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

            _speechEngine.SetInputToDefaultAudioDevice();
            _speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("语音识别: Speech Recognition Engine Started!");

        }
    }
    //语音识别



}
