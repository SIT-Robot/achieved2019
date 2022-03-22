using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.BLL.ServiceCtrl;

namespace SITRobotSystem_wpf.BLL.Competitions.OpenChallenge2015
{
    class OpenChallenge2015Task:Tasks.Tasks
    {
        public SitRobotSpeech ocspeech;

        public OpenChallenge2015Task()
        {
            ocspeech = new SitRobotSpeech();
        }
        
        //检测客户后移至沙发
        public void findPeople()
        {
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            List<User> users = new List<User>();
            
            do
            {
                users = this.getAllUser();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].isRaisingHand)
                {
                    usersposition.Add(users[i].BodyCenter);
                    }
            }
                Thread.Sleep(50);
            } while (!(usersposition.Count >0));
            
            foreach (var position in usersposition)
            {
                speak("I am coming. ");
                   // moveToPlaceByName("sofa");
          //      this.ArmPut();
                speak("Here's your registration");
            }
        }

     

        public void ArmPour()
        {
            ArmAction armAction1 = new ArmAction(52, "pour");
            armCtrl.putThing(armAction1);
            Thread.Sleep(400);

        }

        public override void ArmPut()
        {
            ArmAction armAction1 = new ArmAction(51, "put");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);

        }

        public void askCommand()
        {
            this.speak("What can i do for you?");
            showOCRecognize();
        }

        //语音识别部分：
        public void showOCRecognized(object sender, SpeechRecognizedEventArgs e)
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
                            //1.I need some water   2.No thanks
                            case 1:
                                robotSpeakOwner("OK,please wait a moment.");
                                ReturnCommand = "getwater";
                                Console.WriteLine(ReturnCommand);
                                //moveToPlaceByName("bookcase");
                                //ArmGet();
                                //moveToPlaceByName("sofa");
                                //ArmPut();
                               
                                askpour();
                                speechStop();
                                break;
                            case 2:
                                robotSpeakOwner("You're welcome.");
                                ReturnCommand = "nowater";
                                moveToPlaceByName("ticketpoint");
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

        public void showAskPoured(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine("result is :" + e.Result.Text);
                state = "result is :" + e.Result.Text;
                //fState.state("result is :" + e.Result.Text);
                if (e.Result.Semantics != null)
                {

                    int b = (int) e.Result.Semantics["request"].Value;
                    switch (b)
                    {
                        case 33:
                            robotSpeakOwner("Please enjoy it");
                            //手臂动作：倒水
                            ReturnCommand = "pourwater";
                            speechStop();
                            break;
                        case 44:
                            robotSpeakOwner("All right.Please hold the bottle in my hand.");
                            //手臂动作：延迟松爪
                            ReturnCommand = "putwater";
                            speechStop();
                            break;
                    }
                }
            }
        }

        public void askpour()
        {
            speak("Here is your water,would you want to pour?");
            showAskPour();
        }



        //以下为语音基本组件调用
        public string ReturnCommand;        //返回指令号
        protected double ConfidenceThreshold = 0.5;//识别结果可信度标准
        public string state;//状态信息
        RecognizerInfo ri;
        private SpeechRecognitionEngine speechEngine;//语音识别引擎
        private KinectSensor kinectSensor;//kinect传感器
        SpeechSynthesizer synth;//语音合成引擎
        private KinectAudioStream convertStream;//kinect传来的音频流
        private void createOCGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            request.Add(new SemanticResultValue("I need some water", 1));
            request.Add(new SemanticResultValue("No,thanks", 2));

            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }

        private void creatAskGrammar()
        {
            speechEngine.UnloadAllGrammars();

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;//文档无

            Choices request = new Choices();
            request.Add(new SemanticResultValue("Yeah,please go ahead.", 33));
            request.Add(new SemanticResultValue("No, i drink by myself", 44));

            gb.Append(new SemanticResultKey("request", request));

            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
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
            }
        }

        public void showAskPour()
        {
            if (speechInfo())
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);
                creatAskGrammar();
                 
                speechEngine.SpeechRecognized += showAskPoured;
                speechEngine.SpeechRecognitionRejected += speechRejected;
                speechEngine.RecognizerUpdateReached += recognizerUpdateReached;

                // 让convertStream知道speech活动状态
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            
        }
        public void speechStop()
        {
            speechEngine.RecognizeAsyncStop();
            speechEngine.UnloadAllGrammars();
            //this.speechEngine.Dispose();
            //kinectSensor.Close();
            convertStream.Close();
            //ReturnCommand = null;
            state = null;
            //speechEngine = null;
            //fState.Close();
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
        void synth_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            Console.WriteLine("Speak started");
            state = "Speak started";
            //fState.state(state);
        }
        void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            Console.WriteLine("Speak progress: " + e.Text);
            state = "Speak progress: " + e.Text;
            //fState.state(state);
        }
        private void speechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            string massge1 = "";         //识别被驳回后所说的话
            Console.WriteLine(massge1);
            state = massge1;
            robotSpeakOwner(massge1);
            //TemporaryResult = null;
            //fState.state(state);
        }
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
        void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Console.WriteLine("Speak completed");
            state = "Speak completed";
            //fState.state(state);
        }
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
        void synth_BookmarkReached(object sender, BookmarkReachedEventArgs e)
        {
            Console.WriteLine("Bookmark reached: " + e.Bookmark);
            state = "Bookmark reached: " + e.Bookmark;
            //fState.state(state);
        }
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
        public void speechPause()
        {
            speechEngine.RecognizeAsyncCancel();
        }
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
    }
}
