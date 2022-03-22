using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using Org.BouncyCastle.Asn1.Ocsp;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows;
using SITRobotSystem_wpf.BLL.Tasks;

//using System.Speech.Recognition;
//add
using System.Collections;
using SITRobotSystem_wpf.BLL.Competitions.WhoisWho;

namespace SITRobotSystem_wpf.BLL.ServiceCtrl
{
    /// <summary>
    /// 语音模块底层类
    /// </summary>
    public class SitRobotSpeech
    {
        //11111111111111111111111
        private SpeechRecognitionEngine speechEngine;//语音识别引擎
        SpeechSynthesizer synth;//语音合成引擎
        RecognizerInfo ri;

        protected double ConfidenceThreshold = 0.3;//识别结果可信度标准
        public string state;//状态信息
        //fStateOutput //fState = new fStateOutput();//状态显示窗口

        SpeechRecognizedEventArgs TemporaryResult;//暂存识别结果
        public string ReturnCommand;//返回指令
        public int ReturnNo;        //返回指令号
        public string ReturnD;//返回指令
        public int[] rCommand;//返回任务索引
        public string ReturnName;

        private KinectSensor kinectSensor;//kinect传感器
        private KinectAudioStream convertStream;//kinect传来的音频流

        string[] Objects;//外部调用传入地点
        string[] Places;//外部传入房间
        string[] Things;//外部调用传入物品
        string[] Names;//外部调用传入人名
        string[] Words = { "yourself", "hello" };//说话内容
        public string[] ReIntlRes;
        private string outSpeech;
        public int RecognitedCount = 0;//记录SRAD识别次数

        public string[] point;//restaurant记点      
        public DBCtrl s_DBCtrl;

        //---------ysh series------------------

        string something;
  

        /// <summary>
        /// 语音模块底层类构造函数
        /// </summary>
        public SitRobotSpeech()
        {
            state = "";
            ////fState.Show();

            kinectSensor = null;
            convertStream = null;
            ReturnCommand = null;
            ReturnD = null;
            rCommand = null;
            ReturnName = null;
            outSpeech = "";
            ReIntlRes = null;


        }


        /// <summary>
        ///设置可信度 
        /// </summary>
        /// /// <param name="value">参数类型：double类型；内容：标准可信度</param>
        public void setConfidenceThreshold(double value = 0.3)
        {
            ConfidenceThreshold = value;
        }


        /// <summary>
        ///语音合成入口 
        /// </summary>
        /// <param name="words">参数类型：string类型；内容：机器人要说的话</param>
        public void robotSpeakOwner(string words)
        {
            if (speechEngine != null)
            {
                speechPause();
            }
            synth = new SpeechSynthesizer();

            //对所有已安装的声音输出信息。
            Console.WriteLine("Installed voices -");
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                Console.WriteLine(" Voice Name: " + info.Name);
                state = " Voice Name: " + info.Name;
                //fState.state(state);
            }

            synth.SpeakStarted += synth_SpeakStarted;
            synth.SpeakProgress += synth_SpeakProgress;
            synth.SpeakCompleted += synth_SpeakCompleted;
            synth.BookmarkReached += synth_BookmarkReached;

            synth.SetOutputToDefaultAudioDevice();//设置播放语音设备，为当前默认

            synth.Rate = -3;//设置语速
            synth.Volume = 100;//设置音量
            // 选择输出语音
            synth.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, ZiraPro)");

            // Build a prompt.
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText(words);

            // Speak the prompt.
            synth.Speak(builder);


            Console.WriteLine("speak over");
            state = "speak over";
            //fState.state(state);
            if (speechEngine != null)
            {
                speechContinue();
            }
        }

        public void robotSpeak(string words)
        {

            synth = new SpeechSynthesizer();

            //对所有已安装的声音输出信息。
            /*Console.WriteLine("Installed voices -");
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;5
                Console.WriteLine(" Voice Name: " + info.Name);
                state = " Voice Name: " + info.Name;
                //fState.state(state);
            }*/

            synth.SpeakStarted += synth_SpeakStarted;
            synth.SpeakProgress += synth_SpeakProgress;
            synth.SpeakCompleted += synth_SpeakCompleted;
            synth.BookmarkReached += synth_BookmarkReached;


            synth.SetOutputToDefaultAudioDevice();//设置播放语音设备，为当前默认

            synth.Rate = -3;//设置语速
            synth.Volume = 100;//设置音量
            // 选择输出语音
            synth.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, ZiraPro)");

            // Build a prompt.
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText(words);

            // Speak the prompt.
            synth.Speak(builder);


            Console.WriteLine("speak over");
            state = "speak over";
            //fState.state(state);
            if (speechEngine != null)
            {
                speechContinue();
            }
        }
        /// <summary>
        /// 开始说话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void synth_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            Console.WriteLine("Speak started");
            state = "Speak started";
            //fState.state(state);
        }
        /// <summary>
        /// 即将说出的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            Console.WriteLine("Speak progress: " + e.Text);
            state = "Speak progress: " + e.Text;
            //fState.state(state);
        }
        /// <summary>
        /// 完成发音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Console.WriteLine("Speak completed");
            state = "Speak completed";
            //fState.state(state);
        }

        /// <summary>
        /// 到达发音标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void synth_BookmarkReached(object sender, BookmarkReachedEventArgs e)
        {
            Console.WriteLine("Bookmark reached: " + e.Bookmark);
            state = "Bookmark reached: " + e.Bookmark;
            //fState.state(state);
        }
        /// <summary>
        /// base赛项入口
        /// </summary>
        public void baseRecognize()
        {
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                robotSpeak("I am ready");
                createBaseGrammars();

                speechEngine.SpeechRecognized += baseSpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        /// <summary>
        /// 尝试分配识别引擎
        /// </summary>
        /// <returns></returns>
        private static RecognizerInfo TryGetKinectRecognizer()
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
                    return recognizer;
                }
            }

            return null;
        }

        /// <summary>
        /// 建立base项目语法
        /// </summary>
        private void createBaseGrammars()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;


            Choices head = new Choices();
            head.Add(new SemanticResultValue("what", "what"));
            head.Add(new SemanticResultValue("what is", "what"));
            head.Add(new SemanticResultValue("what's", "what"));
            head.Add(new SemanticResultValue("how many", "how"));
            head.Add(new SemanticResultValue("how", "how"));
            head.Add(new SemanticResultValue("which", "which"));
            head.Add(new SemanticResultValue("in which", "which"));
            head.Add(new SemanticResultValue("who", "who"));

            gb.Append(new SemanticResultKey("head", head));

            Choices body = new Choices();
            body.Add(new SemanticResultValue("time is it", 1));
            body.Add(new SemanticResultValue("your name", 2));
            body.Add(new SemanticResultValue("the capital", 3));
            body.Add(new SemanticResultValue("year was RoboCup founded", 4));
            body.Add(new SemanticResultValue("rings has the Olympic flag", 5));
            body.Add(new SemanticResultValue("the answer to the ultimate question about life the universe and every thing", 6));
            body.Add(new SemanticResultValue("oldest drug used on earth", 7));
            body.Add(new SemanticResultValue("insect has the best evesight", 8));
            body.Add(new SemanticResultValue("lives in a pineapple under the sea", 10));

            gb.Append(new SemanticResultKey("body", body));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }
        /// <summary>
        /// 识别结果处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">识别到的结果</param>
        private void baseSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    string a = e.Result.Semantics["head"].Value.ToString();
                    int b = (int)e.Result.Semantics["body"].Value;

                    switch (a)
                    {
                        case "what":
                            switch (b)
                            {
                                case 1:
                                    robotSpeakOwner("the answer is " + DateTime.Now.ToString("HH:mm"));
                                    break;
                                case 2:
                                    robotSpeakOwner("the answer is well-e");
                                    break;
                                case 3:
                                    robotSpeakOwner("the answer is Beijing");
                                    break;
                                case 6:
                                    robotSpeakOwner("the answer is 42");
                                    break;
                            }
                            break;

                        case "which":
                            switch (b)
                            {
                                case 4:
                                    robotSpeakOwner("the answer is 1997");
                                    break;
                                case 7:
                                    robotSpeakOwner("the answer is Alcohol");
                                    break;
                                case 8:
                                    robotSpeakOwner("the answer is Dragonfly");
                                    break;
                            }
                            break;

                        case "how":
                            switch (b)
                            {
                                case 5:
                                    robotSpeakOwner("the answer is 5");
                                    break;
                            }
                            break;

                        case "who":
                            switch (b)
                            {
                                case 10:
                                    robotSpeakOwner("the answer is Spongebob Squarepants");
                                    break;
                            }
                            break;

                    }
                }
            }
            else
            {
                robotSpeak("sorry I miss heard, please repeat your question.");
                Console.WriteLine("sorry I miss heard, please repeat your question");
            }

        }
        /// <summary>
        /// 识别被驳回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            string massge1 = "未能识别:" + e.Result.Text;         //识别被驳回后所说的话
            Console.WriteLine(massge1);
            ListGrammars(this.speechEngine);
            //TemporaryResult = null;
            //fState.state(state);
        }

        /// <summary>
        /// 暂停更新时间发生时，输出名字和启用状态当前加载的语法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recognizerUpdateReached(object sender, RecognizerUpdateReachedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Update reached:");

            List<Grammar> grammars = new List<Grammar>(speechEngine.Grammars);
            foreach (Grammar g in grammars)
            {
                Thread.Sleep(1000);
                var qualifier = (g.Enabled) ? "enabled" : "disabled";
                string massage4 = g.Name + " grammar is loaded and ." + qualifier;
                Console.WriteLine(massage4);
                state = massage4;
                //fState.state(massage4);
            }
        }

        /// <summary>
        /// 语音识别模块暂停
        /// </summary>
        public void speechPause()
        {
            speechEngine.RecognizeAsyncCancel();
        }
        /// <summary>
        /// 语音识别模块继续运行
        /// </summary>
        public void speechContinue()
        {
            try
            {
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }

        }
        /// <summary>
        /// 语音识别模块停止
        /// </summary>
        public void speechStop()
        {
            speechEngine.RecognizeAsyncStop();
            //speechEngine.UnloadAllGrammars();
            //this.speechEngine.Dispose();
            //kinectSensor.Close();
            convertStream.Close();
            //ReturnCommand = null;
            state = null;
            //speechEngine = null;
            //fState.Close();
        }
        //ysh
        public void speakStop()
        {
            speechEngine = new SpeechRecognitionEngine();
            speechEngine.RecognizeAsyncStop();
            speechEngine.UnloadAllGrammars();
            //this.speechEngine.Dispose();
            //kinectSensor.Close();
            convertStream.Close();
            //ReturnCommand = null;
            state = null;
            speechEngine = null;
            //fState.Close();
        }
        /// <summary>
        /// GPSR识别（无参数）
        /// </summary>
        /// <returns></returns>
        public string gpsrRecogize()
        {
            Objects = new string[17];

            Objects[0] = "couch";
            Objects[1] = "TV";
            Objects[2] = "round table";
            Objects[3] = "sofa";
            Objects[4] = "teapoy";
            Objects[5] = "dresser";
            Objects[6] = "desk";
            Objects[7] = "bed";
            Objects[8] = "bookshelf";
            Objects[9] = "book cabinet";
            Objects[10] = "dinning table";
            Objects[11] = "tea table";
            Objects[12] = "cupboard";
            Objects[13] = "bookstand";
            Objects[14] = "kitchen table";
            Objects[15] = "washbasin";
            Objects[16] = "cooking table";


            Things = new string[5];
            Things[0] = "water";
            Things[1] = "ice tea";
            Things[2] = "green tea";
            Things[3] = "juice";
            Things[4] = "flower tea";

            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
                return "-1";
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                createGpsrGrammars1();

                speechEngine.SpeechRecognized += gpsrSpeechRecognized1;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                return "-1";
                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            Console.WriteLine("no speech recognizer!");
            state = "no speech recognizer!";
            //fState.state(state);
            robotSpeakOwner("no speech recognizer!");
            return "-1";
        }

        private void createGpsrGrammars1()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;


            Choices Action = new Choices();
            Action.Add(new SemanticResultValue("move to", "move"));
            //Action.Add(new SemanticResultValue("move to the", "moveToGoal"));
            Action.Add(new SemanticResultValue("go to the", "move"));
            //Action.Add(new SemanticResultValue("say", "say"));
            //Action.Add(new SemanticResultValue("find", "find"));
            //Action.Add(new SemanticResultValue("follow", "follow"));
            Action.Add(new SemanticResultValue("get the", "catch"));
            Action.Add(new SemanticResultValue("get", "catch"));
            Action.Add(new SemanticResultValue("catch the", "catch"));
            Action.Add(new SemanticResultValue("put it on", "put"));

            Choices place = new Choices();
            //place.Add(new SemanticResultValue(Objects[0], 0));
            //place.Add(new SemanticResultValue("hello", 2));
            //place.Add(new SemanticResultValue("jack", 3));
            //place.Add(new SemanticResultValue(Things[0], 1));

            for (int i = 0; i < Objects.Length; i++)
            {
                place.Add(new SemanticResultValue(Objects[i], i));
            }

            for (int k = 0; k < Things.Length; k++)
            {
                place.Add(new SemanticResultValue(Things[k], k));
            }

            CommandsBuilder.Append(new SemanticResultKey("firtAction", Action));
            CommandsBuilder.Append(new SemanticResultKey("firstObject", place));
            CommandsBuilder.Append(new SemanticResultKey("secondAction", Action));
            CommandsBuilder.Append(new SemanticResultKey("secondObject", place));
            CommandsBuilder.Append(new SemanticResultKey("thirdAction", Action));
            CommandsBuilder.Append(new SemanticResultKey("thirdObject", place));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);

            Choices YesOrNo = new Choices("please intruduce your self");
            GrammarBuilder CheckBuilder = new GrammarBuilder(YesOrNo);
            CheckBuilder.Culture = ri.Culture;
            Grammar CheckGrammar = new Grammar(CheckBuilder);
            CheckGrammar.Name = "introduce";
            CheckGrammar.Priority = 2;
            speechEngine.LoadGrammar(CheckGrammar);

        }

        public void createJudgeGrammars()
        {
            speechEngine.UnloadAllGrammars();

            Choices YesOrNo = new Choices("yes", "no");
            GrammarBuilder CheckBuilder = new GrammarBuilder(YesOrNo);
            CheckBuilder.Culture = ri.Culture;
            Grammar CheckGrammar = new Grammar(CheckBuilder);
            CheckGrammar.Name = "check";
            CheckGrammar.Priority = 2;
            speechEngine.LoadGrammar(CheckGrammar);

        }

        // ReSharper disable once InconsistentNaming
        private void gpsrSpeechRecognized1(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    TemporaryResult = e;
                    switch (e.Result.Semantics["firtAction"].Value.ToString())
                    {
                        case "move":
                            if (e.Result.Semantics["secondAction"].Value.ToString() == "catch" &&
                                e.Result.Semantics["thirdAction"].Value.ToString() == "put")
                            {
                                robotSpeakOwner("would you want me " + e.Result.Semantics["firtAction"].Value + " to "
                                                                + Objects[(int)e.Result.Semantics["firstObject"].Value] + " "
                                                                + e.Result.Semantics["secondAction"].Value + " the "
                                                                + Things[(int)e.Result.Semantics["secondObject"].Value] + " "
                                                                + e.Result.Semantics["thirdAction"].Value + " it on "
                                                                + Objects[(int)e.Result.Semantics["thirdObject"].Value] + "?");
                            }

                            else
                            {
                                string massge7 = "sorry I miss heard, please repeat your command";
                                Console.WriteLine(massge7);
                                state = massge7;
                                //fState.state(massge7);
                                robotSpeakOwner(massge7);
                                TemporaryResult = null;
                            }
                            break;
                        default:
                            string massge1 = "sorry I miss heard, please repeat your command";
                            Console.WriteLine(massge1);
                            state = massge1;
                            //fState.state(massge1);
                            robotSpeakOwner(massge1);
                            TemporaryResult = null;
                            break;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "introduce")
            {
                ReturnCommand = "introduce";
                //robotSpeakOwner("ok,I know");
                speechEngine.RecognizeAsyncStop();

            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    if (TemporaryResult.Result.Semantics["firtAction"].Value.ToString() == "move")
                    {
                        string massge2 = "Ok I know";
                        Console.WriteLine(massge2);
                        state = massge2;
                        //fState.state(massge2);
                        robotSpeakOwner(massge2);
                        ReturnCommand = TemporaryResult.Result.Semantics["firtAction"].Value + ","
                                            + Objects[(int)TemporaryResult.Result.Semantics["firstObject"].Value]
                                        + "@" + TemporaryResult.Result.Semantics["secondAction"].Value + ","
                                            + Things[(int)TemporaryResult.Result.Semantics["secondObject"].Value]
                                        + "@" + TemporaryResult.Result.Semantics["thirdAction"].Value + ","
                                            + Objects[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];

                        speechEngine.RecognizeAsyncStop();
                    }
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    speechEngine.UnloadAllGrammars();
                    createGpsrGrammars1();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }
        }

        /// <summary>
        /// GPSR识别
        /// </summary>
        /// <param name="room">房间string数组</param>
        /// <param name="place">地点string数组</param>
        /// <param name="thing">物品string数组</param>
        /// <returns></returns>
        public string gpsrRecogize(string[] place, string[] thing)
        {
            //fState.Close();
            this.//fState = new fStateOutput(place, thing, name);
                 //fState.Show();
            Objects = place;
            Things = thing;
            //Objects[0] = "kitchen table";
            //Objects[1] = "conference table";
            //Objects[2] = "bed";
            //Objects[3] = "living room chair";

            //Things[0] = "green tea";
            //Things[1] = "juice";
            //Things[2] = "water";
            //Things[3] = "sprite";

            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
                return "-1";
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                createGpsrGrammars();

                this.speechEngine.SpeechRecognized += this.GPSR2015TestRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                return "-1";
                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
                return "-1";
            }
        }

        private void createGpsrGrammars()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;


            Choices Action1 = new Choices();
            Action1.Add(new SemanticResultValue("go to the", "move"));
            Action1.Add(new SemanticResultValue("go to", "move"));
            Action1.Add(new SemanticResultValue("navigate to", "move"));
            Action1.Add(new SemanticResultValue("reach", "move"));
            Action1.Add(new SemanticResultValue("get into", "move"));

            Choices Action2 = new Choices();
            Action2.Add(new SemanticResultValue("get the", "catch"));
            Action2.Add(new SemanticResultValue("catch the", "catch"));
            Action2.Add(new SemanticResultValue("grasp the", "catch"));

            Choices Action3 = new Choices();
            Action3.Add(new SemanticResultValue("and put it on", "put"));
            //Action3.Add(new SemanticResultValue("and tell him", "tell"));
            //Action3.Add(new SemanticResultValue("and ask his", "ask"));
            //Action3.Add(new SemanticResultValue("and count the", "count"));



            //Action1.Add(new SemanticResultValue("move to the", "move"));
            //Action1.Add(new SemanticResultValue("say", "say"));
            //Action1.Add(new SemanticResultValue("introduce", "introduce"));
            //Action1.Add(new SemanticResultValue("find the", "find"));
            //Action1.Add(new SemanticResultValue("find", "find"));
            //Action1.Add(new SemanticResultValue("follow", "follow"));
            //Action1.Add(new SemanticResultValue("and follow", "follow"));
            //Action1.Add(new SemanticResultValue("get the", "catch"));
            //Action1.Add(new SemanticResultValue("get", "catch"));
            //Action1.Add(new SemanticResultValue("catch the", "catch"));
            //Action1.Add(new SemanticResultValue("put it on", "put"));
            //Action1.Add(new SemanticResultValue("and put it on", "put"));





            //place.Add(new SemanticResultValue(Objects[0], 0));
            //place.Add(new SemanticResultValue("hello", 2));
            //place.Add(new SemanticResultValue("jack", 3));
            //place.Add(new SemanticResultValue(Things[0], 1));
            Choices place = new Choices();
            for (int i = 0; i < Objects.Length; i++)
            {
                place.Add(new SemanticResultValue(Objects[i], i));
            }


            Choices thing = new Choices();
            for (int j = 0; j < Things.Length; j++)
            {
                thing.Add(new SemanticResultValue(Things[j], j));
            }

            //for (int k = 0; k < Names.Length; k++)
            //{
            //    place.Add(new SemanticResultValue(Names[k], k));
            //}

            thing.Add(new SemanticResultValue("person", 11));

            Choices third = new Choices();
            for (int k = 0; k < Objects.Length; k++)
            {

                third.Add(new SemanticResultValue(Objects[k], k));
            }
            for (int l = 0; l < Objects.Length; l++)
            {
                third.Add(new SemanticResultValue(Objects[l], l));
            }

            third.Add(new SemanticResultValue("time", 12));
            third.Add(new SemanticResultValue("name and repeat", 13));

            CommandsBuilder.Append(new SemanticResultKey("firstAction", Action1));

            CommandsBuilder.Append(new SemanticResultKey("firstObject", place));

            CommandsBuilder.Append(new SemanticResultKey("secondAction", Action2));
            CommandsBuilder.Append(new SemanticResultKey("secondObject", thing));
            CommandsBuilder.Append(new SemanticResultKey("thirdAction", Action3));

            CommandsBuilder.Append(new SemanticResultKey("thirdObject", third));

            var Commands = new Grammar(CommandsBuilder);

            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);

        }

        private void gpsrSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        outSpeech = "";
                        string massge1;
                        switch (e.Result.Semantics["firstAction"].Value.ToString())
                        {
                            //case "move":
                            //if (e.Result.Semantics["secondAction"].Value.ToString() == "catch" &&
                            //    e.Result.Semantics["thirdAction"].Value.ToString() == "put")
                            //{
                            //    robotSpeakOwner("would you want me " + e.Result.Semantics["firtAction"].Value+" to "
                            //                                    + Objects[(int)e.Result.Semantics["firstObject"].Value] + " "
                            //                                    + e.Result.Semantics["secondAction"].Value + " the "
                            //                                    + Objects[(int)e.Result.Semantics["secondObject"].Value] + " "
                            //                                    + e.Result.Semantics["thirdAction"].Value + " it on "
                            //                                    + Objects[(int)e.Result.Semantics["thirdObject"].Value] + "?");
                            //}
                            //break;

                            case "move":
                                outSpeech += "would you want me go to the "
                                           + Objects[(int)e.Result.Semantics["firstObject"].Value] + " ";
                                break;
                            case "catch":
                                outSpeech += "would you want me catch the "
                                           + Things[(int)e.Result.Semantics["firstObject"].Value] + " ";
                                break;
                            case "find":
                                outSpeech += "would you want me find the people ";

                                break;
                            case "follow":
                                outSpeech += "would you want me follow the people ";

                                break;
                            //case "find":
                            //    outSpeech += "would you want me find the "
                            //               + Names[(int)e.Result.Semantics["firstObject"].Value] + " ";
                            //case "follow":
                            //    outSpeech += "would you want me follow "
                            //               + Names[(int)e.Result.Semantics["firstObject"].Value] + " ";
                            //    break;
                            case "say":
                                outSpeech += "would you want me say "
                                           + Words[(int)e.Result.Semantics["firstObject"].Value] + " ";
                                break;
                            case "introduce":
                                outSpeech += "would you want me introduce myself ";
                                break;
                            default:
                                massge1 = "sorry I miss heard, please repeat your command";
                                Console.WriteLine(massge1);
                                state = massge1;
                                //fState.state(massge1);
                                robotSpeakOwner(massge1);
                                TemporaryResult = null;
                                break;
                        }
                        switch (e.Result.Semantics["secondAction"].Value.ToString())
                        {
                            case "move":
                                outSpeech += "move to the "
                                           + Objects[(int)e.Result.Semantics["secondObject"].Value] + " ";
                                break;
                            case "catch":
                                outSpeech += "grasp the "
                                           + Things[(int)e.Result.Semantics["secondObject"].Value] + " ";
                                break;
                            case "find":
                                if ((int)e.Result.Semantics["secondObject"].Value == 11)
                                {
                                    outSpeech += "find the people ";
                                }
                                else
                                {
                                    outSpeech += "find the "
                                        + Things[(int)e.Result.Semantics["secondObject"].Value] + " ";
                                }

                                break;
                            case "follow":
                                outSpeech += "follow him ";

                                break;
                            //case "find":
                            //    outSpeech += "would you want me find the "
                            //               + Names[(int)e.Result.Semantics["firstObject"].Value] + " ";
                            //case "follow":
                            //    outSpeech += "would you want me follow "
                            //               + Names[(int)e.Result.Semantics["firstObject"].Value] + " ";
                            //    break;
                            case "say":
                                outSpeech += "say "
                                           + Words[(int)e.Result.Semantics["secondObject"].Value] + " ";
                                break;
                            case "introduce":
                                outSpeech += "introduce myself ";

                                break;
                            default:
                                massge1 = "sorry I miss heard, please repeat your command";
                                Console.WriteLine(massge1);
                                state = massge1;
                                //fState.state(massge1);
                                robotSpeakOwner(massge1);
                                TemporaryResult = null;
                                break;
                        }
                        switch (e.Result.Semantics["thirdAction"].Value.ToString())
                        {
                            case "move":
                                outSpeech += "and move to the "
                                           + Objects[(int)e.Result.Semantics["thirdObject"].Value] + " ";
                                break;
                            case "catch":
                                outSpeech += "and catch the "
                                           + Things[(int)e.Result.Semantics["thirdObject"].Value] + " ";
                                break;
                            case "put":
                                outSpeech += "and put it on  "
                                           + Objects[(int)e.Result.Semantics["thirdObject"].Value] + " ";
                                break;
                            case "tell":
                                outSpeech += "and tell him time";
                                break;
                            case "ask":
                                outSpeech += "and ask his name and repeat";
                                break;
                            case "count":
                                outSpeech += "and count the "
                                    + Things[(int)e.Result.Semantics["thirdObject"].Value] + " ";
                                break;
                            case "find":
                                outSpeech += "and find the people";
                                break;
                            case "follow":
                                outSpeech += "follow him";
                                break;
                            case "say":
                                outSpeech += "and say "
                                           + Words[(int)e.Result.Semantics["thirdObject"].Value] + " ";
                                break;
                            case "introduce":
                                outSpeech += "introduce myself ";

                                break;
                            default:
                                massge1 = "sorry I miss heard, please repeat your command";
                                Console.WriteLine(massge1);
                                state = massge1;
                                //fState.state(massge1 + " :third false");
                                robotSpeakOwner(massge1);
                                TemporaryResult = null;
                                break;
                        }
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    string buffers = null;
                    //if (TemporaryResult.Result.Semantics["firtAction"].Value.ToString() == "move")
                    //{
                    //    buffers = "@" + TemporaryResult.Result.Semantics["firtAction"].Value + ","
                    //                        + Objects[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@"
                    //                    + "@" + TemporaryResult.Result.Semantics["secondAction"].Value + ","
                    //                        + Things[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@"
                    //                    + "@" + TemporaryResult.Result.Semantics["thirdAction"].Value + ","
                    //                        + Objects[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] + "@";
                    //    this.speechEngine.RecognizeAsyncStop();
                    //}
                    switch (TemporaryResult.Result.Semantics["firstAction"].Value.ToString())
                    {
                        case "move":
                            buffers = "move,"
                                       + Objects[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@";
                            break;
                        case "catch":
                            buffers = "catch,"
                                       + Things[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@";
                            break;
                        case "find":
                            buffers = "find,people@";
                            break;
                        case "follow":
                            buffers = "follow,people@";
                            break;
                        //case "find":
                        //    buffers = "find,"
                        //               + Names[(int)TemporaryResult.Result.Semantics["firtObject"].Value] + "@";
                        //    break;
                        //case "follow":
                        //    buffers += "follow,";
                        //    if (Names[(int)TemporaryResult.Result.Semantics["firtObject"].Value] != "him")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["firtObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["secondAction"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["thirdObject"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] + "@";
                        //    }
                        //    else
                        //    {
                        //        string massge3 = "Maybe I am worry";
                        //        Console.WriteLine(massge3);
                        //        state = massge3;
                        //        //fState.state(massge3 + " :first");
                        //        robotSpeakOwner(massge3);
                        //        TemporaryResult = null;
                        //        createGpsrGrammars();
                        //        buffers = null;
                        //        return;
                        //    }
                        //    break;
                        case "say":
                            buffers = "say,"
                                       + Words[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@";
                            break;
                        case "introduce":
                            buffers = "introduce,myself@";
                            break;
                    }
                    switch (TemporaryResult.Result.Semantics["secondAction"].Value.ToString())
                    {
                        case "move":
                            buffers += "move,"
                                       + Objects[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                            break;
                        case "catch":
                            buffers += "catch,"
                                       + Things[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                            break;
                        case "find":
                            if (TemporaryResult.Result.Semantics["secondObject"].Value.ToString() == "people")
                            {
                                buffers += "find,people@";
                            }
                            else
                            {
                                buffers += "find,"
                                    + Things[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@"; ;
                            }
                            break;
                        case "follow":
                            buffers += "follow,people@";
                            break;
                        //case "find":
                        //    buffers += "find,"
                        //               + Names[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                        //    break;
                        //case "follow":
                        //    buffers += "follow,";
                        //    if (Names[(int)TemporaryResult.Result.Semantics["secondObject"].Value] != "him")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["firstAction"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["thirdAction"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] + "@";
                        //    }
                        //    else
                        //    {
                        //        string massge3 = "Maybe I am worry";
                        //        Console.WriteLine(massge3);
                        //        state = massge3;
                        //        //fState.state(massge3 + " :second");
                        //        robotSpeakOwner(massge3);
                        //        TemporaryResult = null;
                        //        createGpsrGrammars();
                        //        buffers = null;
                        //        return;
                        //    }
                        //    break;
                        case "say":
                            buffers += "say,"
                                       + Words[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                            break;
                        case "introduce":
                            buffers += "introduce,myself@";
                            break;
                    }
                    switch (TemporaryResult.Result.Semantics["thirdAction"].Value.ToString())
                    {
                        case "move":
                            buffers += "move,"
                                       + Objects[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                            break;
                        case "catch":
                            buffers += "catch,"
                                       + Things[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                            break;

                        case "put":
                            buffers += "put,"
                                       + Objects[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                            break;
                        case "tell":
                            buffers += "tell,time";
                            break;
                        case "ask":
                            buffers += "ask,name";
                            break;
                        case "count":
                            buffers += "count,"
                                + Things[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                            break;
                        case "find":
                            buffers += "find,people";
                            break;
                        case "follow":
                            buffers += "follow,people";
                            break;
                        //case "find":
                        //    buffers += "find,"
                        //               + Names[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] + "@";
                        //    break;
                        //case "follow":
                        //    buffers += "follow,";
                        //    if (Names[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] != "him")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["thirdObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["secondAction"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["secondObject"].Value] + "@";
                        //    }
                        //    else if (TemporaryResult.Result.Semantics["firtAction"].Value.ToString() == "find")
                        //    {
                        //        buffers += Names[(int)TemporaryResult.Result.Semantics["firstObject"].Value] + "@";
                        //    }
                        //    else
                        //    {
                        //        string massge3 = "Maybe I am worry";
                        //        Console.WriteLine(massge3);
                        //        state = massge3;
                        //        //fState.state(massge3 + " :third");
                        //        robotSpeakOwner(massge3);
                        //        TemporaryResult = null;
                        //        createGpsrGrammars();
                        //        buffers = null;
                        //        return;
                        //    }
                        //    break;
                        case "say":
                            buffers += "say,"
                                       + Words[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                            break;
                        case "introduce":
                            buffers += "introduce,myself";
                            break;
                    }
                    ReturnCommand = buffers;
                    string massge2 = "Ok I know.I will " + outSpeech.Substring(18, outSpeech.Length - 18);
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    createGpsrGrammars();
                }
            }
            else
            {
                string massge1 = "sorry I miss heard, please repeat your command";
                Console.WriteLine(massge1);
                state = massge1;
                robotSpeakOwner(massge1);
                TemporaryResult = null;
            }
        }

        public void ChangeConfidenceThreshold(double newConfidenceThreshold)
        {
            this.ConfidenceThreshold = newConfidenceThreshold;
        }

        public string restaurantRecognize(string[] newPoint)
        {
            ReturnCommand = null;
            ////fState = new fStateOutput();
            ////fState.Show();
            point = newPoint;
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatRestaurantGrammar();

                this.speechEngine.SpeechRecognized += this.restaurantRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ListGrammars(this.speechEngine);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }

            return ReturnCommand;
        }

        private void creatRestaurantGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Points = new Choices();
            for (int i = 0; i < point.Length; i++)
            {
                Points.Add(new SemanticResultValue(point[i], i));
            }

            CommandsBuilder.Append(new Choices(new string[] { "here is", "this is" }));
            CommandsBuilder.Append(new SemanticResultKey("Point", Points));

            CommandsBuilder.Append("on your");

            Choices direction = new Choices();
            direction.Add(new SemanticResultValue("left", "left"));
            direction.Add(new SemanticResultValue("right", "right"));
            CommandsBuilder.Append(new SemanticResultKey("direction", direction));


            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
            Console.WriteLine("2016" + Commands);
        }

        private void restaurantRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Is there " + point[(int)TemporaryResult.Result.Semantics["Point"].Value] + " on My"
                            + TemporaryResult.Result.Semantics["direction"].Value.ToString() + " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = point[(int)TemporaryResult.Result.Semantics["Point"].Value];
                    ReturnD = TemporaryResult.Result.Semantics["direction"].Value.ToString();
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatRestaurantGrammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void restaurantCommand(string[] newPoint)
        {
            ReturnCommand = null;
            ReturnD = null;

            rCommand = null;

            //fState = new fStateOutput();
            //fState.Show();
            point = newPoint;
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatRestaurantCommandGrammar();

                this.speechEngine.SpeechRecognized += this.restaurantCommandRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatRestaurantCommandGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Points = new Choices();
            for (int i = 0; i < point.Length; i++)
            {
                Points.Add(new SemanticResultValue(point[i], i));
            }

            CommandsBuilder.Append("give me the");
            CommandsBuilder.Append(new SemanticResultKey("first", Points));
            CommandsBuilder.Append(new SemanticResultKey("second", Points));
            CommandsBuilder.Append("and");
            CommandsBuilder.Append(new SemanticResultKey("third", Points));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);

            var CommandsBuilder2 = new GrammarBuilder();
            CommandsBuilder2.Culture = ri.Culture;
            CommandsBuilder2.Append("move to initial position");
            var Commands2 = new Grammar(CommandsBuilder2);
            Commands2.Name = "end";
            Commands2.Priority = 2;
            speechEngine.LoadGrammar(Commands2);

        }

        private void restaurantCommandRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you want me get the " +
                                           point[(int)TemporaryResult.Result.Semantics["first"].Value] + ",the "
                                           + point[(int)TemporaryResult.Result.Semantics["second"].Value] + " and the " +
                                           point[(int)TemporaryResult.Result.Semantics["third"].Value] + "?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Thread.Sleep(3000);
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }

            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "end")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you want me go to all location?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Thread.Sleep(3000);
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check" && TemporaryResult.Result.Grammar.Name == "end")
            {
                if (e.Result.Text == "yes")
                {

                    ReturnCommand = "end";
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatRestaurantCommandGrammar();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check" && TemporaryResult.Result.Grammar.Name == "commands")
            {
                if (e.Result.Text == "yes")
                {
                    rCommand = new int[3];
                    rCommand[0] = (int)TemporaryResult.Result.Semantics["first"].Value;
                    rCommand[1] = (int)TemporaryResult.Result.Semantics["second"].Value;
                    rCommand[2] = (int)TemporaryResult.Result.Semantics["third"].Value;
                    string massge2 = "Ok I know";

                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatRestaurantCommandGrammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }
        }

        private void creatWhoIsWhoName()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices namesChoices = new Choices();


            for (int i = 0; i < Names.Length; i++)
            {
                namesChoices.Add(new SemanticResultValue(Names[i], i));
            }

            CommandsBuilder.Append("my name is");
            CommandsBuilder.Append(new SemanticResultKey("name", namesChoices));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "getName";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void creatWhoIsWhoCommand()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Choices = new Choices();
            for (int i = 0; i < Things.Length; i++)
            {
                Choices.Add(new SemanticResultValue(Things[i], i));
            }

            CommandsBuilder.Append("please give a");
            CommandsBuilder.Append(new SemanticResultKey("thing", Choices));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "getCommand";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void whoIsWhoRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "getName")
            {
                Console.WriteLine("===理解名字===：" + e.Result.Text);//得分证明

                state = "result is :" + e.Result.Text;
                ////fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Are you " + Names[(int)TemporaryResult.Result.Semantics["name"].Value] +
                                           " ?";


                        Console.WriteLine("----");
                        Console.WriteLine("outSpeech: " + outSpeech);
                        state = outSpeech;
                        ////fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        ////fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check" && TemporaryResult.Result.Grammar.Name == "getName")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnName = Names[(int)TemporaryResult.Result.Semantics["name"].Value];
                    WhoisWhoTask.userName.Add((Names[(int)TemporaryResult.Result.Semantics["name"].Value]));//保存名字
                    //ReturnD = TemporaryResult.Result.Semantics["direction"].Value.ToString();
                    string massge2 = "Ok I know.And what can I do for you?";
                    Console.WriteLine(massge2);
                    state = massge2;
                    ////fState.state(massge2);
                    robotSpeakOwner(massge2);
                    creatWhoIsWhoCommand();
                    //this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am wrong.";
                    Console.WriteLine(massge3);
                    state = massge3;
                    ////fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnName = null;
                    creatWhoIsWhoName();
                }
            }

            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "getCommand")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                Console.WriteLine("===理解物品===:" + e.Result.Text);

                state = "result is :" + e.Result.Text;
                ////fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you need the " + Things[(int)TemporaryResult.Result.Semantics["thing"].Value] +
                                           " ?";


                        Console.WriteLine(WhoisWhoTask.stuffName);
                        state = outSpeech;
                        ////fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        ////fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }

            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check" && TemporaryResult.Result.Grammar.Name == "getCommand")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = Things[(int)TemporaryResult.Result.Semantics["thing"].Value];
                    WhoisWhoTask.stuffName.Add(Things[(int)TemporaryResult.Result.Semantics["thing"].Value]);//保存物品名字
                    //ReturnD = TemporaryResult.Result.Semantics["direction"].Value.ToString();
                    string massge2 = "Ok I know.";
                    Console.WriteLine(massge2);
                    state = massge2;
                    ////fState.state(massge2);
                    robotSpeakOwner(massge2);
                    creatWhoIsWhoCommand();
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    ////fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatWhoIsWhoCommand();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                ////fState.state(e.Result.Confidence.ToString());
            }
            //this.speechStop();
        }

        public void whoIsWhoRecogine(string[] name, string[] things)
        {
            Things = things;
            Names = name;
            ReturnName = null;
            ReturnCommand = null;

            ////fState.Close();
            //this.////fState = new fStateOutput(null,Things,name);
            ////fState.Show();


            robotSpeakOwner("What's your name?");
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatWhoIsWhoName();

                speechEngine.SpeechRecognized += whoIsWhoRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatHomeRecognize()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Points = new Choices();


            Points.Add(new SemanticResultValue("water", "water"));

            Points.Add(new SemanticResultValue("first aid kit", "first aid kit"));

            Points.Add(new SemanticResultValue("medicine", "medicine"));

            CommandsBuilder.Append("Please give me the");
            CommandsBuilder.Append(new SemanticResultKey("thing", Points));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void homeRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you need the "
                            + TemporaryResult.Result.Semantics["thing"].Value.ToString() + " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = TemporaryResult.Result.Semantics["thing"].Value.ToString();
                    string massge2 = "Ok I know.I will bring the " + TemporaryResult.Result.Semantics["thing"].Value.ToString();
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatHomeRecognize();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void homeRecognize()
        {
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatHomeRecognize();

                speechEngine.SpeechRecognized += homeRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatReaderGrammar()
        {
            speechEngine.UnloadAllGrammars();
            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;
            // CommandsBuilder.Append("Please help me to convey this document ");
            CommandsBuilder.Append("go to bedroom and take care for the child");
            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void readerRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.ToString() != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        //string outSpeech = "Do you want me convey this document?";
                        string outSpeech = "Do you want me go to the bedroom and care this kid?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = "true";
                    string massge2 = "ok i know, i will try my best!";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeak(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);


                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatReaderGrammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void readerRecognize()
        {
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatReaderGrammar();

                speechEngine.SpeechRecognized += readerRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatIntlResGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Points = new Choices();
            for (int i = 0; i < point.Length; i++)
            {
                Points.Add(point[i]);
            }

            Choices Thing = new Choices();
            for (int j = 0; j < Things.Length; j++)
            {
                Thing.Add(Things[j]);
            }


            CommandsBuilder.Append(new SemanticResultKey("first", Thing));
            CommandsBuilder.Append(new SemanticResultKey("second", Thing));
            CommandsBuilder.Append("on");
            CommandsBuilder.Append(new SemanticResultKey("third", Points));
            CommandsBuilder.Append("and");
            CommandsBuilder.Append(new SemanticResultKey("forth", Thing));
            CommandsBuilder.Append("on");
            CommandsBuilder.Append(new SemanticResultKey("fifth", Points));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void intlResRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.ToString() != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you want me put the " + e.Result.Semantics["first"].Value + " "
                            + e.Result.Semantics["second"].Value + " on " + e.Result.Semantics["third"].Value + ".and put the "
                            + e.Result.Semantics["forth"].Value + " on "
                            + e.Result.Semantics["fifth"].Value + "?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = "true";
                    ReIntlRes = new string[5];
                    ReIntlRes[0] = TemporaryResult.Result.Semantics["first"].Value.ToString();
                    ReIntlRes[1] = TemporaryResult.Result.Semantics["second"].Value.ToString();
                    ReIntlRes[2] = TemporaryResult.Result.Semantics["third"].Value.ToString();
                    ReIntlRes[3] = TemporaryResult.Result.Semantics["forth"].Value.ToString();
                    ReIntlRes[4] = TemporaryResult.Result.Semantics["fifth"].Value.ToString();

                    string massge2 = "Ok I know.";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    outSpeech = null;
                    creatIntlResGrammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void intlResRecognize(string[] objects, string[] thing)
        {
            point = objects;
            Things = thing;

            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatIntlResGrammar();

                speechEngine.SpeechRecognized += intlResRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatHamburger()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices Choices = new Choices(new string[] { "chicken", "beef", "pork" });


            CommandsBuilder.Append("please give a");
            CommandsBuilder.Append(new SemanticResultKey("kind", Choices));
            CommandsBuilder.Append("hamburger");

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "Command";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void hamburgerRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "Command")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you need the " +
                                           TemporaryResult.Result.Semantics["kind"].Value.ToString() + " hamburger?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = TemporaryResult.Result.Semantics["kind"].Value.ToString();

                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatHamburger();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void hamburgerRecognize()
        {
            ReturnCommand = null;

            //fState = new fStateOutput();
            //fState.Show();
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatHamburger();

                this.speechEngine.SpeechRecognized += this.hamburgerRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void creatFollowGrammar()
        {
            speechEngine.UnloadAllGrammars();
            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;
            CommandsBuilder.Append("come in");

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void followRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.ToString() != null)
                {

                    TemporaryResult = e;
                    string outSpeech = "come in";
                    Console.WriteLine(outSpeech);
                    state = outSpeech;
                    //fState.state(outSpeech);
                    robotSpeakOwner(outSpeech);

                    //catch
                    //{
                    //    string massge1 = "sorry I miss heard, please repeat your command";
                    //    Console.WriteLine(massge1);
                    //    state = massge1;
                    //    //fState.state(massge1);
                    //    robotSpeakOwner(massge1);
                    //    TemporaryResult = null;
                    //}
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = "true";
                    string massge2 = "Ok I know.";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnCommand = null;
                    creatFollowGrammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        public void followRecognize()
        {
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatFollowGrammar();

                speechEngine.SpeechRecognized += followRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }











        public void askName()
        {
            Names = new string[] { "James", "Jerry", "Sam", "Jones", "Herry", "Mary", "Rose", "Lily", "Alice", "Wisdom" };

            robotSpeakOwner("Hello. What's your name?");
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatWhoIsWhoName();

                speechEngine.SpeechRecognized += askRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。音频级更新
                //this.speechEngine.AudioLevelUpdated += this.sre_AudioLevelUpdated;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void askRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "getName")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                ////fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Are you " + Names[(int)TemporaryResult.Result.Semantics["name"].Value] +
                                           " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        ////fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        ////fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check" && TemporaryResult.Result.Grammar.Name == "getName")
            {
                if (e.Result.Text == "yes")
                {
                    string massge2 = "Ok I know.your name is " + Names[(int)TemporaryResult.Result.Semantics["name"].Value];
                    ReturnName = Names[(int)TemporaryResult.Result.Semantics["name"].Value];
                    //ReturnD = TemporaryResult.Result.Semantics["direction"].Value.ToString();                    
                    Console.WriteLine(massge2);
                    state = massge2;
                    ////fState.state(massge2);
                    robotSpeakOwner(massge2);
                    //this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    ////fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    ReturnName = null;
                    creatWhoIsWhoName();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                ////fState.state(e.Result.Confidence.ToString());
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public bool speechInfo()
        {
            // 只支持一个传感器
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
                return false;
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                return true;
                //while (true)
                //{
                //    stage = "wait";
                // }
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
                return false;
            }
        }

        public void showSpeechRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                createShowGrammar();

                speechEngine.SpeechRecognized += showSpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }

        public void showSRADRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatSRADGrammar();

                speechEngine.SpeechRecognized += showSRADRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }

        public void showOCRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                createOCGrammar();

                speechEngine.SpeechRecognized += showOCRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ;
            }
        }

        private void createShowGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            request.Add(new SemanticResultValue("please give me water", 1));
            request.Add(new SemanticResultValue("thank you very much", 2));
            request.Add(new SemanticResultValue("please introduce yourself", 3));

            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }

        private void createOCGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            request.Add(new SemanticResultValue("I need some water", 1));
            request.Add(new SemanticResultValue("No,thanks", 2));
            request.Add(new SemanticResultValue("Yes,please", 3));
            request.Add(new SemanticResultValue("No, i drink by myself", 4));

            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }


        public void PersonSpeechRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatPersonGrammar();

                speechEngine.SpeechRecognized += PersonSpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }
        private void creatPersonGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无
            Choices Obj = new Choices();
            Obj.Add(new SemanticResultValue("yes", 1));
            gb.Append(new SemanticResultKey("obj", Obj));


            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }
        private void PersonSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int obj = (int)e.Result.Semantics["obj"].Value;
                    ReturnCommand = e.Result.Text;
                    robotSpeakOwner("I know");
                    Console.WriteLine(ReturnCommand);
                    speechStop();
                }
                else
                {
                    robotSpeak("sorry I miss heard.");
                    Console.WriteLine("sorry I miss heard.");
                }

            }
        }
        public void PersonNameSpeechRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatPersonNameGrammar();

                speechEngine.SpeechRecognized += PersonNameSpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }
        private void creatPersonNameGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无
            Choices Obj = new Choices();
            Obj.Add(new SemanticResultValue("Alex", 1));
            Obj.Add(new SemanticResultValue("Angel", 2));
            Obj.Add(new SemanticResultValue("Edward", 3));
            Obj.Add(new SemanticResultValue("Eve", 4));
            Obj.Add(new SemanticResultValue("Homer", 5));
            Obj.Add(new SemanticResultValue("Jamie", 6));
            Obj.Add(new SemanticResultValue("John", 7));
            Obj.Add(new SemanticResultValue("Jane", 8));
            Obj.Add(new SemanticResultValue("Liza", 9));
            Obj.Add(new SemanticResultValue("Kevin", 10));
            Obj.Add(new SemanticResultValue("Melissa", 11));
            Obj.Add(new SemanticResultValue("Kurt", 12));
            Obj.Add(new SemanticResultValue("Tracy", 13));
            Obj.Add(new SemanticResultValue("Robin", 14));
            Obj.Add(new SemanticResultValue("Sophia", 15));
            gb.Append(new SemanticResultKey("obj", Obj));


            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }
        private void PersonNameSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int obj = (int)e.Result.Semantics["obj"].Value;
                    robotSpeakOwner("ok");
                    ReturnCommand = e.Result.Text;
                    Console.WriteLine(ReturnCommand);
                    speechStop();
                }
                else
                {
                    robotSpeak("sorry I miss heard.");
                    Console.WriteLine("sorry I miss heard.");
                }

            }
        }
        public void WakeMeUpSpeechRecognize(string[] obj)
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatWakeMeUpGrammar(obj);

                speechEngine.SpeechRecognized += WakeMeUpSpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }
        private void creatWakeMeUpGrammar(string[] obj)
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无
            Choices Obj = new Choices();
            for (int i = 0; i < obj.Length; i++)
                Obj.Add(new SemanticResultValue(obj[i], i + 1));
            gb.Append(new SemanticResultKey("obj", Obj));


            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }
        private void WakeMeUpSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int obj = (int)e.Result.Semantics["obj"].Value;

                    robotSpeakOwner("Ok," + e.Result.Text);
                    ReturnCommand = e.Result.Text;
                    Console.WriteLine(ReturnCommand);
                    speechStop();

                }
                else
                {
                    robotSpeak("sorry I miss heard.");
                    Console.WriteLine("sorry I miss heard.");
                }

            }
        }

        private void creatSRADGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无
            Choices request = new Choices();
            request.Add(new SemanticResultValue("Which German Count invented the zeppelin?", 1));
            request.Add(new SemanticResultValue("Who was the first president of the USA?", 2));
            request.Add(new SemanticResultValue("In ancient China what meat was reserved for the Emperor?", 3));
            request.Add(new SemanticResultValue("In which city was the Titanic built", 4));
            request.Add(new SemanticResultValue("How many children did Queen Victoria have?", 5));
            request.Add(new SemanticResultValue("Which French king was called the Sun King?", 6));
            request.Add(new SemanticResultValue("What was in England the northern frontier of the Roman Empire?", 7));
            request.Add(new SemanticResultValue("In which nineteen seventy-nine film was the spaceship called Nostromo?", 8));
            request.Add(new SemanticResultValue("Who was the first king of Belgium ?", 9));
            request.Add(new SemanticResultValue("What was the former name of New York?", 10));
            request.Add(new SemanticResultValue("What was the Latin name of Paris in Roman times?", 11));
            request.Add(new SemanticResultValue("Which city was the capital of Australia from 1901 to 1927?", 12));
            request.Add(new SemanticResultValue("Give another name for the study of fossils?", 13));
            request.Add(new SemanticResultValue("What do dragonflies prefer to eat?", 14));
            request.Add(new SemanticResultValue("Which insects cannot fly, but can jump higher than 30 centimeters?", 15));
            request.Add(new SemanticResultValue("What is the name of the European Bison?", 16));
            request.Add(new SemanticResultValue("What is called a fish with a snake-like body?", 17));
            request.Add(new SemanticResultValue("Which plant does the Canadian flag contain?", 18));
            request.Add(new SemanticResultValue("Which is the largest species of the tiger?", 19));
            request.Add(new SemanticResultValue("Which malformation did Marilyn Monroe have when she was born?", 20));
            request.Add(new SemanticResultValue("What is the house number of the Simpsons?", 21));
            request.Add(new SemanticResultValue("What dog in ancient China was restricted to the aristocracy?", 22));
            request.Add(new SemanticResultValue("Who is the director of Reservoir Dogs?", 23));
            request.Add(new SemanticResultValue("What number is on Herbie the beatle?", 24));
            request.Add(new SemanticResultValue("Give the name of the best James Bond parody?", 25));
            request.Add(new SemanticResultValue("Which American state is nearest to the former Soviet Union?", 26));
            request.Add(new SemanticResultValue("How many tentacles does a squid have?", 27));
            request.Add(new SemanticResultValue("What is converted into alcohol during brewing?", 28));
            request.Add(new SemanticResultValue("Which river forms the eastern section of the border between England and Scotland?", 29));
            request.Add(new SemanticResultValue("In what year was Prince Andrew born?", 30));
            request.Add(new SemanticResultValue("Name the two families in Romeo and Juliet?", 31));
            request.Add(new SemanticResultValue("If cats are feline, what are sheep?", 32));
            request.Add(new SemanticResultValue("For which fruit is the US state of Georgia famous?", 33));
            request.Add(new SemanticResultValue("Which is the financial centre and main city of Switzerland?", 34));
            request.Add(new SemanticResultValue("Which TV programme's theme tune was called Hit and Miss?", 35));
            request.Add(new SemanticResultValue("Which guitarist is known as Slowhand?", 36));
            request.Add(new SemanticResultValue("What is an infant whale commonly called?", 37));
            request.Add(new SemanticResultValue("What do the British call the vegetables that Americans call zucchini?", 38));
            request.Add(new SemanticResultValue("What is an otter's home called?", 39));
            request.Add(new SemanticResultValue("How have vegetables been cut which are served Julienne?", 40));
            request.Add(new SemanticResultValue("In Roman mythology, Neptune is the equivalent to which Greek god?", 41));
            request.Add(new SemanticResultValue("Which TV character said, 'Live long and prosper'?", 42));
            request.Add(new SemanticResultValue("In which State would you find the city of Birmingham?", 43));
            request.Add(new SemanticResultValue("Which hills divide England from Scotland?", 44));
            request.Add(new SemanticResultValue("What continent has the fewest flowering plants?", 45));
            request.Add(new SemanticResultValue("What is Canada's national animal?", 46));
            request.Add(new SemanticResultValue("What is the alternative common name for a Black Leopard?", 47));
            request.Add(new SemanticResultValue("What in Cornwall is the most southerly point of mainland Britain?", 48));
            request.Add(new SemanticResultValue("What explorer introduced pigs to North America?", 49));
            request.Add(new SemanticResultValue("What is the world biggest island?", 50));




            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }

        private void creatGPSR_SRADGrammar()
        {

            speechEngine.UnloadAllGrammars();

            var ocgm = new GrammarBuilder();
            ocgm.Culture = ri.Culture;

            Choices userproblem = new Choices();
            userproblem.Add(new SemanticResultValue("what is the capital of China", 1));
            userproblem.Add(new SemanticResultValue("what is the biggest province of China", 2));
            userproblem.Add(new SemanticResultValue("what is the world biggest island", 3));
            userproblem.Add(new SemanticResultValue("what was the former name of New York?", 4));
            userproblem.Add(new SemanticResultValue("what is China's national animal", 5));

            userproblem.Add(new SemanticResultValue("how large is the area of China", 6));
            userproblem.Add(new SemanticResultValue("how many hours in a day", 7));
            userproblem.Add(new SemanticResultValue("how many season are there in one year", 8));
            userproblem.Add(new SemanticResultValue("how many children did Queen Victoria have", 9));
            userproblem.Add(new SemanticResultValue("how many seconds in one minute", 10));

            userproblem.Add(new SemanticResultValue("who was the first president of the USA", 11));

            ocgm.Append(new SemanticResultKey("problem", userproblem));

            var g = new Grammar(ocgm);
            speechEngine.LoadGrammar(g);

        }

        private void GPSR_SRADRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    int b = (int)e.Result.Semantics["problem"].Value;

                    switch (b)
                    {
                        case 1:
                            robotSpeakOwner("The answer is Beijing");
                            break;
                        case 2:
                            robotSpeakOwner("The answer is Xinjiang");
                            break;
                        case 3:
                            robotSpeakOwner("The answer is greenland");
                            break;
                        case 4:
                            robotSpeakOwner("The answer is New Amsterdam");
                            break;
                        case 5:
                            robotSpeakOwner("The answer is Panda");
                            break;
                        case 6:
                            robotSpeakOwner("The answer is Nine million and six hundred thousand saquare kilometers");
                            break;
                        case 7:
                            robotSpeakOwner("The answer is 24");
                            break;
                        case 8:
                            robotSpeakOwner("The answe is four");
                            break;
                        case 9:
                            robotSpeakOwner("She had nine children");
                            break;
                        case 10:
                            robotSpeakOwner("The answer is 60");
                            break;
                        case 11:
                            robotSpeakOwner("The first president in the USA is George Washington");
                            break;
                    }
                }
                else
                {
                    robotSpeakOwner("Sorry,I miss herad, please repeat your question");
                }
            }
            else
            {
                robotSpeak("");
                Console.WriteLine("X.X");
                //一次问题只能问一回且没有确认环节
            }
        }

        public void GPSR_SRADRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatGPSR_SRADGrammar();

                speechEngine.SpeechRecognized += GPSR_SRADRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }
        private void showOCRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int b = (int)e.Result.Semantics["request"].Value;

                    switch (b)
                    {
                        case 1:
                            robotSpeakOwner("OK,please wait a moment.");
                            ReturnNo = 1;
                            Console.WriteLine(ReturnNo);
                            speechStop();
                            break;
                        case 2:
                            robotSpeakOwner("You're welcome.");
                            ReturnCommand = "nowater";
                            speechStop();
                            break; break;
                        case 3:
                            robotSpeakOwner("Please enjoy it");
                            ReturnCommand = "pourwater";
                            speechStop();
                            break;
                        case 4:
                            robotSpeakOwner("All right.");
                            ReturnCommand = "putwater";
                            speechStop();
                            break;
                    }
                }
                else
                {
                    robotSpeak("Pardon?");
                    Console.WriteLine("Pardon?");
                }
            }
        }

        private void showSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int b = (int)e.Result.Semantics["request"].Value;
                    switch (b)
                    {
                        case 1:
                            robotSpeakOwner("Yes,i'm on my way");
                            ReturnCommand = b + "haha";
                            Console.WriteLine(ReturnCommand);
                            speechStop();
                            break;
                        case 2:
                            robotSpeakOwner("my pleasure");
                            ReturnCommand = "1haha";
                            speechStop();
                            break;
                        case 3:
                            robotSpeakOwner("ok,i know");
                            ReturnCommand = "1haha";
                            speechStop();
                            break;
                    }
                }
                else
                {
                    robotSpeak("Sorry,i missed heard");
                    Console.WriteLine("Sorry,i missed heard");
                }

            }
        }

        public void showSRADRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int b = (int)e.Result.Semantics["request"].Value;
                    switch (b)
                    {
                        case 1:
                            robotSpeakOwner("Count von Zeppelin");
                            ReturnCommand = "1haha";
                            break;
                        case 2:
                            robotSpeakOwner("George Washington");
                            ReturnCommand = "1haha";
                            break;
                        case 3:
                            robotSpeakOwner("Pork");
                            ReturnCommand = "1haha";
                            break;
                        case 4:
                            robotSpeakOwner("Belfast");
                            ReturnCommand = "1haha";
                            break;
                        case 5:
                            robotSpeakOwner("Nine children");
                            ReturnCommand = "1haha";
                            break;
                        case 6:
                            robotSpeakOwner("Louis fourteenth");
                            ReturnCommand = "1haha";
                            break;
                        case 7:
                            robotSpeakOwner("Hadrians wall");
                            ReturnCommand = "1haha";
                            break;
                        case 8:
                            robotSpeakOwner("Alien");
                            ReturnCommand = "1haha";
                            break;
                        case 9:
                            robotSpeakOwner("Leopold first");
                            ReturnCommand = "1haha";
                            break;
                        case 10:
                            robotSpeakOwner("New Amsterdam");
                            ReturnCommand = "1haha";
                            break;
                        case 11:
                            robotSpeakOwner("Lutetia");
                            ReturnCommand = "1haha";
                            break;
                        case 12:
                            robotSpeakOwner("Melbourne");
                            ReturnCommand = "1haha";
                            break;
                        case 13:
                            robotSpeakOwner("Paleontology");
                            ReturnCommand = "1haha";
                            break;
                        case 14:
                            robotSpeakOwner("Mosquitoes");
                            ReturnCommand = "1haha";
                            break;
                        case 15:
                            robotSpeakOwner("Fleas");
                            ReturnCommand = "1haha";
                            break;
                        case 16:
                            robotSpeakOwner("Wisent");
                            ReturnCommand = "1haha";
                            break;
                        case 17:
                            robotSpeakOwner("Eel fish");
                            ReturnCommand = "1haha";
                            break;
                        case 18:
                            robotSpeakOwner("Maple");
                            ReturnCommand = "1haha";
                            break;
                        case 19:
                            robotSpeakOwner("Siberian tiger");
                            ReturnCommand = "1haha";
                            break;
                        case 20:
                            robotSpeakOwner("Six toes");
                            ReturnCommand = "1haha";
                            break;
                        case 21:
                            robotSpeakOwner("Number seven four two");
                            ReturnCommand = "1haha";
                            break;
                        case 22:
                            robotSpeakOwner("Pekinese");
                            ReturnCommand = "1haha";
                            break;
                        case 23:
                            robotSpeakOwner("Quentin Tarantino");
                            ReturnCommand = "1haha";
                            break;
                        case 24:
                            robotSpeakOwner("Fifty-three");
                            ReturnCommand = "1haha";
                            break;
                        case 25:
                            robotSpeakOwner("Austin Powers");
                            ReturnCommand = "1haha";
                            break;
                        case 26:
                            robotSpeakOwner("ALASKA");
                            ReturnCommand = "1haha";
                            break;
                        case 27:
                            robotSpeakOwner("ten");
                            ReturnCommand = "1haha";
                            break;
                        case 28:
                            robotSpeakOwner("sugar");
                            ReturnCommand = "1haha";
                            break;
                        case 29:
                            robotSpeakOwner("tweed");
                            ReturnCommand = "1haha";
                            break;
                        case 30:
                            robotSpeakOwner("nineteen sixty");
                            ReturnCommand = "1haha";
                            break;
                        case 31:
                            robotSpeakOwner("MONTAGUE and" +
                                            " CAPULET");
                            ReturnCommand = "1haha";
                            break;
                        case 32:
                            robotSpeakOwner("ovine");
                            ReturnCommand = "1haha";
                            break;
                        case 33:
                            robotSpeakOwner("peach");
                            ReturnCommand = "1haha";
                            break;
                        case 34:
                            robotSpeakOwner("zurich");
                            ReturnCommand = "1haha";
                            break;
                        case 35:
                            robotSpeakOwner("JUKE BOX JURY");
                            ReturnCommand = "1haha";
                            break;
                        case 36:
                            robotSpeakOwner("ERIC CLAPTON");
                            ReturnCommand = "1haha";
                            break;
                        case 37:
                            robotSpeakOwner("calf");
                            ReturnCommand = "1haha";
                            break;
                        case 38:
                            robotSpeakOwner("COURGETTES");
                            ReturnCommand = "1haha";
                            break;
                        case 39:
                            robotSpeakOwner("holt");
                            ReturnCommand = "1haha";
                            break;
                        case 40:
                            robotSpeakOwner("THIN STRIPS");
                            ReturnCommand = "1haha";
                            break;
                        case 41:
                            robotSpeakOwner("POSEIDON");
                            ReturnCommand = "1haha";
                            break;
                        case 42:
                            robotSpeakOwner("MR SPOCK");
                            ReturnCommand = "1haha";
                            break;
                        case 43:
                            robotSpeakOwner("ALABAMA");
                            ReturnCommand = "1haha";
                            break;
                        case 44:
                            robotSpeakOwner("CHEVIOTS");
                            ReturnCommand = "1haha";
                            break;
                        case 45:
                            robotSpeakOwner("ANTARTICA");
                            ReturnCommand = "1haha";
                            break;
                        case 46:
                            robotSpeakOwner("BEAVER");
                            ReturnCommand = "1haha";
                            break;
                        case 47:
                            robotSpeakOwner("PANTHER");
                            ReturnCommand = "1haha";
                            break;
                        case 48:
                            robotSpeakOwner("LIZARD POINT");
                            ReturnCommand = "1haha";
                            break;
                        case 49:
                            robotSpeakOwner("CRISTOPHER COLUMBUS");
                            ReturnCommand = "1haha";
                            break;
                        case 50:
                            robotSpeakOwner("GREENLAND");
                            ReturnCommand = "1haha";
                            break;



                    }
                }
                else
                {
                    robotSpeak("");
                    Console.WriteLine("");
                }
            }
        }

        public void Ro_nurse_2015SpeechRecognize()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                createR_N_2015Grammar();

                speechEngine.SpeechRecognized += R_O_2015SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);

            }
        }

        private void createR_N_2015Grammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            request.Add(new SemanticResultValue("give me pill", 1));
            request.Add(new SemanticResultValue("need my pill", 1));
            request.Add(new SemanticResultValue("thank you", 2));
            request.Add(new SemanticResultValue("Yes", 3));
            request.Add(new SemanticResultValue("No", 4));


            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }

        private void R_O_2015SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int b = (int)e.Result.Semantics["request"].Value;
                    switch (b)
                    {
                        case 1:
                            robotSpeakOwner("OK,i'm on my way");
                            ReturnNo = 1;
                            Console.WriteLine(ReturnNo);
                            speechStop();
                            break;
                        case 2:
                            robotSpeakOwner("my pleasure");
                            ReturnCommand = "1haha";
                            speechStop();
                            break;
                        case 3:
                            robotSpeakOwner("ok,i know");
                            ReturnCommand = "1haha";
                            speechStop();
                            break;
                        case 4:
                            robotSpeakOwner("All right, i will find another one");
                            ReturnCommand = b + "haha";
                            speechStop();
                            break;
                    }
                }
                else
                {
                    robotSpeak("Sorry,i missed heard");
                    Console.WriteLine("Sorry,i missed heard");
                }

            }
        }


        public string[] milk { get; set; }



        //restaurant接受去哪张桌子的语音识别
        private string[] table;
        public void Res2015Stage2Recognize(string[] Table)
        {
            if (speechInfo())
            {

                table = Table;
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatRes2015Stage2GRammar();

                speechEngine.SpeechRecognized += Res2015Stage2CRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ;
            }
        }
        private void creatRes2015Stage2GRammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            for (int i = 0; i < table.Length; i++)
            {
                request.Add(new SemanticResultValue(table[i], i));
            }
            gb.Append("move to the");
            gb.Append(new SemanticResultKey("aimtable", request));

            var g = new Grammar(gb);
            g.Name = "Command";
            speechEngine.LoadGrammar(g);
        }
        private void Res2015Stage2CRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "Command")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you want me go to the " +
                                           TemporaryResult.Result.Semantics["aimtable"].Value.ToString() + " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    this.ReturnNo = (int)TemporaryResult.Result.Semantics["aimtable"].Value;

                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    this.ReturnNo = -1;
                    creatHamburger();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        //询问顾客需求
        public void RequestRecognize(string[] objects)
        {
            if (speechInfo())
            {
                Things = objects;
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatRequestGRammar();

                speechEngine.SpeechRecognized += RequestRecognized;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ;
            }
        }

        private void creatRequestGRammar()
        {
            speechEngine.UnloadAllGrammars();

            var reqA = new GrammarBuilder();
            reqA.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            for (int i = 0; i < Things.Length; i++)
            {
                request.Add(new SemanticResultValue(Things[i], i));
            }

            reqA.Append("please give me the");
            reqA.Append(new SemanticResultKey("object1", request));

            var reqB = reqA;
            reqB.Append("and the");
            reqB.Append(new SemanticResultKey("object2", request));

            var A = new Grammar(reqA);
            A.Name = "RequestA";
            speechEngine.LoadGrammar(A);

            var B = new Grammar(reqB);
            B.Name = "RequestB";
            speechEngine.LoadGrammar(B);
        }


        private void RequestRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Grammar.Name == "RequestA" && e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you need the " +
                                           this.Things[(int)TemporaryResult.Result.Semantics["object1"].Value] + " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                else if (e.Result.Grammar.Name == "RequestB" && e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "Do you need the " + this.Things[(int)TemporaryResult.Result.Semantics["object1"].Value]
                                           + " and the " + this.Things[(int)TemporaryResult.Result.Semantics["object2"].Value]
                                           + " ?";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes" && TemporaryResult.Result.Grammar.Name == "RequestA")
                {

                    this.rCommand = new int[1];
                    this.rCommand[0] = (int)TemporaryResult.Result.Semantics["object1"].Value;
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "yes" && TemporaryResult.Result.Grammar.Name == "RequestB")
                {

                    this.rCommand = new int[2];
                    this.rCommand[0] = (int)TemporaryResult.Result.Semantics["object1"].Value;
                    this.rCommand[1] = (int)TemporaryResult.Result.Semantics["object2"].Value;
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    state = massge2;
                    //fState.state(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    this.ReturnCommand = null;
                    creatRequestGRammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        /// <summary>
        /// 传入string数组顺序:物品,地点,房间,人名
        /// </summary>
        /// <param name="DBthing"></param>
        /// <param name="DBobject"></param>
        /// <param name="DBthing"></param>
        public void GPSR2015Test(string[] DBthing, string[] DBobject, string[] DBplace, string[] DBname)
        {
            if (speechInfo())
            {
                this.Things = DBthing;
                this.Objects = DBobject;
                this.Places = DBplace;
                this.Names = DBname;

                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatGPSR2015TestRammar();//change

                speechEngine.SpeechRecognized += GPSR2015TestRecognized;//change

                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                // speechEngine.SetInputToAudioStream(
                //  convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.SetInputToDefaultAudioDevice();
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        //可替换动词数组
        Choices get = new Choices(new string[] { "get the", "take the", "grasp the" });
        Choices bring = new Choices(new string[] { "carry it to", "deliver it to", "take it to", "bring it to" });
        Choices at = new Choices(new string[] { "in the", "at the", "which is in the" });
        Choices navigate = new Choices(new string[] { "go to the", "navigate to the", "reach the" });
        Choices find = new Choices(new string[] { "find the", "look for the", "search for the" });
        Choices tell = new Choices(new string[] { "tell", "speak", "say" });
        private void creatGPSR2015TestRammar()
        {
            //卸载所有语法
            speechEngine.UnloadAllGrammars();

            //语法1.1
            GrammarBuilder gbA = new GrammarBuilder();//change
            gbA.Culture = ri.Culture;

            gbA.Append(new SemanticResultKey("navigate", navigate));
            gbA.Append(new SemanticResultKey("Objects", new Choices(this.Objects)));
            gbA.Append("and");
            gbA.Append(new SemanticResultKey("find", find));
            gbA.Append(new SemanticResultKey("Things", new Choices(this.Things)));
            gbA.Append("and go back here");

            Grammar GA = new Grammar(gbA);//change
            GA.Name = "1.1";//change
            speechEngine.LoadGrammar(GA);

            ////语法1.2
            //GrammarBuilder gbB = new GrammarBuilder();
            //gbB.Culture = ri.Culture;

            //gbB.Append(new SemanticResultKey("get", get));
            //gbB.Append(new SemanticResultKey("Things", new Choices(this.Things)));
            //gbB.Append("from the");
            //gbB.Append(new SemanticResultKey("Objects1", new Choices(this.Objects)));
            //gbB.Append("and");
            //gbB.Append(new SemanticResultKey("bring", bring));
            //gbB.Append(new SemanticResultKey("Names", new Choices(this.Names)));
            //gbB.Append(new SemanticResultKey("at", at));
            //gbB.Append(new SemanticResultKey("Objects2", new Choices(this.Objects)));

            //Grammar GB = new Grammar(gbB);
            //GB.Name = "1.2";
            //speechEngine.LoadGrammar(GB);

            //语法1.3
            GrammarBuilder gbL = new GrammarBuilder();
            gbL.Culture = ri.Culture;

            gbL.Append(new SemanticResultKey("get", get));
            gbL.Append(new SemanticResultKey("Things", new Choices(this.Things)));
            gbL.Append("from the");
            gbL.Append(new SemanticResultKey("Objects1", new Choices(this.Objects)));
            gbL.Append("and");
            gbL.Append(new SemanticResultKey("bring", bring));
            gbL.Append("the");
            gbL.Append(new SemanticResultKey("Objects2", new Choices(this.Objects)));

            Grammar GL = new Grammar(gbL);
            GL.Name = "1.3";
            speechEngine.LoadGrammar(GL);

            ////语法2.1
            //GrammarBuilder gbC = new GrammarBuilder();//change
            //gbC.Culture = ri.Culture;

            //gbC.Append(new SemanticResultKey("navigate", navigate));
            //gbC.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbC.Append("and");
            //gbC.Append(new SemanticResultKey("find", find));
            //gbC.Append(new SemanticResultKey("Things", new Choices(this.Things)));

            //Grammar GC = new Grammar(gbC);//change
            //GC.Name = "2.1";//change
            //speechEngine.LoadGrammar(GC);

            //语法3.1
            GrammarBuilder gbD = new GrammarBuilder();//change
            gbD.Culture = ri.Culture;

            gbD.Append(new SemanticResultKey("find", find));
            gbD.Append(new SemanticResultKey("person", new Choices(this.Names)));
            gbD.Append("in the");
            gbD.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            gbD.Append("and answer a question");

            Grammar GD = new Grammar(gbD);//change
            GD.Name = "3.1";//change
            speechEngine.LoadGrammar(GD);

            ////语法3.2
            //GrammarBuilder gbE = new GrammarBuilder();//change
            //gbE.Culture = ri.Culture;

            //gbE.Append(new SemanticResultKey("find", find));
            //gbE.Append("person in the");
            //gbE.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbE.Append("and");
            //gbE.Append(new SemanticResultKey("tell", tell));
            //gbE.Append("your name");

            //Grammar GE = new Grammar(gbE);//change
            //GE.Name = "3.2";//change
            //speechEngine.LoadGrammar(GE);

            ////语法3.3
            //GrammarBuilder gbF = new GrammarBuilder();//change
            //gbF.Culture = ri.Culture;

            //gbF.Append(new SemanticResultKey("find", find));
            //gbF.Append("person in the");
            //gbF.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbF.Append("and");
            //gbF.Append(new SemanticResultKey("tell", tell));
            //gbF.Append("the name of your team");

            //Grammar GF = new Grammar(gbF);//change
            //GF.Name = "3.3";//change
            //speechEngine.LoadGrammar(GF);

            ////语法3.4
            //GrammarBuilder gbG = new GrammarBuilder();//change
            //gbG.Culture = ri.Culture;

            //gbG.Append(new SemanticResultKey("find", find));
            //gbG.Append("person in the");
            //gbG.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbG.Append("and");
            //gbG.Append(new SemanticResultKey("tell", tell));
            //gbG.Append("the time");

            //Grammar GG = new Grammar(gbG);//change
            //GG.Name = "3.4";//change
            //speechEngine.LoadGrammar(GG);

            ////语法3.5
            //GrammarBuilder gbH = new GrammarBuilder();//change
            //gbH.Culture = ri.Culture;

            //gbH.Append(new SemanticResultKey("find", find));
            //gbH.Append("person in the");
            //gbH.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbH.Append("and");
            //gbH.Append(new SemanticResultKey("tell", tell));
            //gbH.Append("what time is it");

            //Grammar GH = new Grammar(gbH);//change
            //GH.Name = "3.5";//change
            //speechEngine.LoadGrammar(GH);

            ////语法3.6
            //GrammarBuilder gbI = new GrammarBuilder();//change
            //gbI.Culture = ri.Culture;

            //gbI.Append(new SemanticResultKey("find", find));
            //gbI.Append("person in the");
            //gbI.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbI.Append("and");
            //gbI.Append(new SemanticResultKey("tell", tell));
            //gbI.Append("tell the date");

            //Grammar GI = new Grammar(gbI);//change
            //GI.Name = "3.6";//change
            //speechEngine.LoadGrammar(GI);

            ////语法3.7
            //GrammarBuilder gbJ = new GrammarBuilder();//change
            //gbJ.Culture = ri.Culture;

            //gbJ.Append(new SemanticResultKey("find", find));
            //gbJ.Append("person in the");
            //gbJ.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbJ.Append("and");
            //gbJ.Append(new SemanticResultKey("tell", tell));
            //gbJ.Append("what day is");
            //gbJ.Append(new SemanticResultKey("TodayOrTomorrow", new Choices(new string[]{"today","tomorrow"})));

            //Grammar GJ = new Grammar(gbJ);//change
            //GJ.Name = "3.7";//change
            //speechEngine.LoadGrammar(GJ);

            ////语法3.8
            //GrammarBuilder gbK = new GrammarBuilder();//change
            //gbK.Culture = ri.Culture;

            //gbK.Append(new SemanticResultKey("find", find));
            //gbK.Append("person in the");
            //gbK.Append(new SemanticResultKey("Places", new Choices(this.Places)));
            //gbK.Append("and");
            //gbK.Append(new SemanticResultKey("tell", tell));
            //gbK.Append("what day is");
            //gbK.Append(new SemanticResultKey("MonthOrWeek", new Choices(new string[] { "month", "week" })));

            //Grammar GK = new Grammar(gbK);//change
            //GK.Name = "3.8";//change
            //speechEngine.LoadGrammar(GK);

            ListGrammars(this.speechEngine);
        }

        private void GPSR2015TestRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("识别到:" + e.Result.Text);
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name != "check")
            {
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "";
                        switch (e.Result.Grammar.Name)
                        {
                            case "1.1":
                                outSpeech = "Do you want me to " +
                                           TemporaryResult.Result.Semantics["navigate"].Value.ToString() + " " +
                                           TemporaryResult.Result.Semantics["Objects"].Value.ToString() +
                                           " and " + TemporaryResult.Result.Semantics["find"].Value.ToString() + " " +
                                           TemporaryResult.Result.Semantics["Things"].Value.ToString()
                                           + " and go back here";
                                break;
                            case "1.2":
                            case "1.3":
                                outSpeech = "Do you want me to " +
                                    TemporaryResult.Result.Semantics["get"].Value.ToString() + " " +
                                    TemporaryResult.Result.Semantics["Things"].Value.ToString() +
                                    " from the " + TemporaryResult.Result.Semantics["Objects1"].Value.ToString() +
                                    " and " + TemporaryResult.Result.Semantics["bring"].Value.ToString() + " " +
                                    TemporaryResult.Result.Semantics["Objects2"].Value.ToString();
                                break;
                            case "2.1":
                            case "3.1":
                                outSpeech = "Do you want me to find" + TemporaryResult.Result.Semantics["person"].Value.ToString() +
                                    " in the " + TemporaryResult.Result.Semantics["Places"].Value.ToString() +
                                    " and answer a question";
                                break;
                            case "3.4":
                            case "3.5":
                            case "3.6":
                            case "3.7":
                            case "3.8":
                                outSpeech = "Do you want me " + TemporaryResult.Result.Text.ToString() + "?";
                                break;
                            case "3.2":
                                outSpeech = "Do you want me " + TemporaryResult.Result.Semantics["find"].Value.ToString()
                                    + " person in the " + TemporaryResult.Result.Semantics["Places"].Value.ToString()
                                    + " and " + TemporaryResult.Result.Semantics["tell"].Value.ToString()
                                    + "  my name?";
                                break;
                            case "3.3":
                                outSpeech = "Do you want me " + TemporaryResult.Result.Semantics["find"].Value.ToString()
                                    + " person in the " + TemporaryResult.Result.Semantics["Places"].Value.ToString()
                                    + " and " + TemporaryResult.Result.Semantics["tell"].Value.ToString()
                                    + "  the name of our team?";
                                break;

                        }
                        Console.WriteLine(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat.";
                        Console.WriteLine(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                else
                {
                    string massge1 = "sorry I miss heard";
                    Console.WriteLine(massge1);
                    robotSpeakOwner(massge1);
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    //if(TemporaryResult.Result.Grammar.Name=="1.3")
                    //{
                    //    ReturnCommand = TemporaryResult.Result.Semantics["firtAction"].Value + ","
                    //                        + Objects[(int)TemporaryResult.Result.Semantics["firstObject"].Value]
                    //                    + "@" + TemporaryResult.Result.Semantics["secondAction"].Value + ","
                    //                        + Things[(int)TemporaryResult.Result.Semantics["secondObject"].Value]
                    //                    + "@" + TemporaryResult.Result.Semantics["thirdAction"].Value + ","
                    //                        + Objects[(int)TemporaryResult.Result.Semantics["thirdObject"].Value];
                    //}
                    ReturnCommand = TemporaryResult.Result.Grammar.Name + "@" + TemporaryResult.Result.Text.ToString();
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    this.ReturnCommand = null;
                    creatGPSR2015TestRammar();
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }

        private static void ListGrammars(SpeechRecognitionEngine recognizer)
        {
            string qualifier;
            List<Grammar> grammars = new List<Grammar>(recognizer.Grammars);
            foreach (Grammar g in grammars)
            {
                qualifier = (g.Enabled) ? "enabled" : "disabled";

                Console.WriteLine("Grammar {0} is loaded and is {1}.",
                  g.Name, qualifier);
            }
        }


        public string SetTable2017Recognize(string[] commands)
        {
            ReturnCommand = null;
            ////fState = new fStateOutput();
            ////fState.Show();
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatSetTableGrammar(commands);

                this.speechEngine.SpeechRecognized += this.SetTableRecognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。ShowRecognized;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ListGrammars(this.speechEngine);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }

            return ReturnCommand;
        }

        private void creatSetTableGrammar(string[] commands)
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            CommandsBuilder.Append(new Choices(commands));


            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void SetTableRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;

                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        ReturnCommand = e.Result.Text;
                        string outSpeech = "OK";
                        Console.WriteLine(outSpeech);
                        state = outSpeech;
                        //fState.state(outSpeech);
                        //robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                //if (TemporaryResult != null)
                //{
                //    createShowJudgeGrammars();
                //}
            }

        }

        //-------------------------------------------------------------------
        public string Restaurant2017Recognize(string[] commands)
        {
            ReturnCommand = null;
            ////fState = new fStateOutput();
            ////fState.Show();
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatRestaurant2017Grammar(commands);

                this.speechEngine.SpeechRecognized += this.Restaurant2017Recognized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。ShowRecognized;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ListGrammars(this.speechEngine);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }

            return ReturnCommand;
        }

        private void creatRestaurant2017Grammar(string[] commands)
        {
            speechEngine.UnloadAllGrammars();
            Objects = commands;
            //语法一
            Choices order = new Choices("take the order", "stop the challenge", "wait");
            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            CommandsBuilder.Append("eva");
            //CommandsBuilder.Append(new SemanticResultKey("name", new Choices(commands)));
            CommandsBuilder.Append(new SemanticResultKey("order", order));


            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);



            //语法2
            Choices get = new Choices(commands);
            var CommandsBuilder2 = new GrammarBuilder();
            CommandsBuilder2.Culture = ri.Culture;

            CommandsBuilder2.Append("please give me the");
            CommandsBuilder2.Append(new SemanticResultKey("get", get));



            var Commands2 = new Grammar(CommandsBuilder2);
            Commands2.Name = "commands2";
            Commands2.Priority = 2;
            speechEngine.LoadGrammar(Commands2);

            //语法3
            var CommandsBuilder3 = new GrammarBuilder();
            CommandsBuilder3.Culture = ri.Culture;

            CommandsBuilder3.Append("please give me the");
            CommandsBuilder3.Append(new SemanticResultKey("get1", get));
            CommandsBuilder3.Append("and");
            CommandsBuilder3.Append(new SemanticResultKey("get2", get));

            var Commands3 = new Grammar(CommandsBuilder3);
            Commands3.Name = "commands3";
            Commands3.Priority = 3;
            speechEngine.LoadGrammar(Commands3);

        }

        private void Restaurant2017Recognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("识别到:" + e.Result.Text);
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name != "check")
            {
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "";
                        switch (e.Result.Grammar.Name)
                        {
                            case "commands":
                                outSpeech = "Do you want me to " + TemporaryResult.Result.Semantics["order"].Value.ToString();
                                break;
                            case "commands2":
                                outSpeech = "Do you want a " + TemporaryResult.Result.Semantics["get"].Value.ToString();
                                break;
                            case "commands3":
                                outSpeech = "Do you want " + TemporaryResult.Result.Semantics["get1"].Value.ToString()
                                    + " and " + TemporaryResult.Result.Semantics["get2"].Value.ToString()
                                    + '?';
                                break;

                        }
                        Console.WriteLine(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat.";
                        Console.WriteLine(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                else
                {
                    string massge1 = "sorry I miss heard";
                    Console.WriteLine(massge1);
                    robotSpeakOwner(massge1);
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    ReturnCommand = TemporaryResult.Result.Text.ToString();
                    string massge2 = "Ok I know";
                    Console.WriteLine(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    Console.WriteLine(massge3);
                    state = massge3;
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    this.ReturnCommand = null;
                    creatRestaurant2017Grammar(Objects);
                }
            }
            else
            {
                Console.WriteLine(e.Result.Confidence.ToString());
                state = e.Result.Confidence.ToString();
                //fState.state(e.Result.Confidence.ToString());
            }

        }


        public void createShowJudgeGrammars()
        {
            speechEngine.UnloadAllGrammars();

            Choices what = new Choices("what is it", "no");
            GrammarBuilder CheckBuilder = new GrammarBuilder(what);
            CheckBuilder.Culture = ri.Culture;
            Grammar CheckGrammar = new Grammar(CheckBuilder);
            CheckGrammar.Name = "check";
            CheckGrammar.Priority = 2;
            speechEngine.LoadGrammar(CheckGrammar);

        }

        public void createHelpMeCarryGrammar()
        {
            speechEngine.UnloadAllGrammars();
            GrammarBuilder gb1 = new GrammarBuilder();
            gb1.Culture = ri.Culture;
            gb1.Append(new SemanticResultKey("Carry it to", bring));
            gb1.Append("the");
            gb1.Append(new SemanticResultKey("Objects", new Choices(this.Objects)));

            Grammar Gb = new Grammar(gb1);
            Gb.Name = "request";
            Gb.Priority = 1;
            speechEngine.LoadGrammar(Gb);
        }

        public void HelpMeCarryRecognized(Object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text + "   " + e.Result.Confidence);
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "request")
            {
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        string outSpeech = "";
                        outSpeech = "Do you want me to carry this to the " +
                            TemporaryResult.Result.Semantics["Objects"].Value.ToString();
                        // Console.WriteLine(outSpeech);
                        robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat.";
                        //Console.WriteLine(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                else
                {
                    string massge1 = "sorry I miss heard";
                    // Console.WriteLine(massge1);
                    robotSpeakOwner(massge1);
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }
            else if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "check")
            {
                if (e.Result.Text == "yes")
                {
                    //ReturnCommand =TemporaryResult.Result.Text.ToString();
                    ReturnCommand = TemporaryResult.Result.Semantics["Objects"].Value.ToString();//直接返回了地点
                    string massge2 = "Ok I know";
                    // Console.WriteLine(massge2);
                    robotSpeakOwner(massge2);
                    this.speechStop();
                }
                else// if (e.Result.Text == "no")
                {
                    string massge3 = "Maybe I am worry";
                    //Console.WriteLine(massge3);
                    state = massge3;
                    //fState.state(massge3);
                    robotSpeakOwner(massge3);
                    TemporaryResult = null;
                    this.ReturnCommand = null;
                    createHelpMeCarryGrammar();
                }
            }
        }

        public void HelpMeCarryRecognition(String[] DBobject)
        {
            this.kinectSensor = KinectSensor.GetDefault();
            if (speechInfo())
            {
                this.Objects = DBobject;
                ReturnCommand = null;
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                createHelpMeCarryGrammar();//

                speechEngine.SpeechRecognized += HelpMeCarryRecognized;//

                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
        public string HelpMeCarryEasyRecgnization(string[] commands)
        {
            ReturnCommand = null;
            ////fState = new fStateOutput();
            ////fState.Show();
            // 只支持一个传感器
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // 打开传感器
                this.kinectSensor.Open();

                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // 创建转换流
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                //fState.state(state);
            }

            ri = TryGetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatHelpMeCarryEasyRecgnizedGrammar(commands);

                this.speechEngine.SpeechRecognized += this.HelpMeCarryEasyRecgnized;
                this.speechEngine.SpeechRecognitionRejected += this.speechRejected;
                this.speechEngine.RecognizerUpdateReached += this.recognizerUpdateReached;

                // 为AudioLevelUpdated事件添加一个事件处理程序。ShowRecognized;

                // 让convertStream知道speech活动状态
                this.convertStream.SpeechActive = true;

                // 长时间识别会话(几个小时或更多),它可能是有益的关掉声学模型的适应。
                // 随着时间的推移这将防止识别精度降低。
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToDefaultAudioDevice();
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                ListGrammars(this.speechEngine);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                //fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }

            return ReturnCommand;
        }

        private void creatHelpMeCarryEasyRecgnizedGrammar(string[] commands)
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            CommandsBuilder.Append(new Choices(commands));


            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "commands";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }

        private void HelpMeCarryEasyRecgnized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "commands")
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;

                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {
                    try
                    {
                        TemporaryResult = e;
                        ReturnCommand = e.Result.Text;
                        //string outSpeech = "OK";
                        //Console.WriteLine(outSpeech);
                        //state = outSpeech;
                        ////fState.state(outSpeech);
                        //robotSpeakOwner(outSpeech);
                    }
                    catch
                    {
                        string massge1 = "sorry I miss heard, please repeat your command";
                        Console.WriteLine(massge1);
                        state = massge1;
                        //fState.state(massge1);
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }// end if(2)              
            }// end if(1)
        }//end method





        //ysh======开语言识别=====================================================================
        public void FuckingSurpriseRecognition(string something_)
        {
            something = something_;
            ReturnName = null;
            ReturnCommand = null;


            robotSpeakOwner("what the hell you want?");
            // open kinect
            kinectSensor = KinectSensor.GetDefault();
            if (kinectSensor != null)
            {
                // 打开传感器
                kinectSensor.Open();
                // 获取音频流
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                Stream audioStream = audioBeamList[0].OpenInputStream();
                // 创建转换流
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // 失败,设置状态文本
                Console.WriteLine("no kinect ready!");
                state = "no kinect ready!";
                ////fState.state(state);
                return;
            }

            ri = TryGetKinectRecognizer();//?
            if (null != ri)
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);

                FuckingSurpriseGrammars();

                

                speechEngine.SpeechRecognized += FuckingSurpriseVoiceSender;  // 识别吧
                speechEngine.SpeechRecognitionRejected += speechRejectedAndSpeak;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;
                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;
                speechEngine.SetInputToDefaultAudioDevice();
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                Console.WriteLine("no speech recognizer!");
                state = "no speech recognizer!";
                ////fState.state(state);
                robotSpeakOwner("no speech recognizer!");
            }
        }

        private void FuckingSurpriseVoiceSender(object sender, SpeechRecognizedEventArgs e)
        {
            
            if (e.Result.Confidence >= ConfidenceThreshold && e.Result.Grammar.Name == "getCommand")
            {
                Console.WriteLine("result is :" + e.Result.Text);

                if (e.Result.Semantics != null)
                {
                    if(e.Result.Semantics != null)
                    {
                        TemporaryResult = e;
                        string outSpeech = "Oh I will bring something to you";
                        state = outSpeech;
                        robotSpeakOwner(outSpeech);
                    }
                    else
                    {
                        string massge1 = "Too young Too simple";
                        Console.WriteLine(massge1);
                        state = massge1;
                        robotSpeakOwner(massge1);
                        TemporaryResult = null;
                    }
                }
                if (TemporaryResult != null)
                {
                    createJudgeGrammars();
                }
            }//end if(1)
            this.speakStop();
        }

        private void FuckingSurpriseGrammars()
        {
            speechEngine.UnloadAllGrammars();

            var CommandsBuilder = new GrammarBuilder();
            CommandsBuilder.Culture = ri.Culture;

            Choices namesChoices = new Choices();


            namesChoices.Add(new SemanticResultValue(something, 0));


            CommandsBuilder.Append("i need");
            CommandsBuilder.Append(new SemanticResultKey("something", namesChoices));

            var Commands = new Grammar(CommandsBuilder);
            Commands.Name = "getCommand";
            Commands.Priority = 1;
            speechEngine.LoadGrammar(Commands);
        }
        private void speechRejectedAndSpeak(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            string massge1 = "未能识别:" + e.Result.Text;         //识别被驳回后所说的话
            this.robotSpeak("I Don't Know what your said.But! Let me tell you a story");
            Console.WriteLine(massge1);
        }

        //==========================end ysh==============================================================



    }//end class
}
