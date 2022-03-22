using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System.Collections;
using System.Threading;
using System.IO;
using System.Data;

using System;
using System.Collections.Generic;


namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {

        static bool isRecognized = false;
        static bool bLastThreadCancel = true;

        /// <summary>
        /// Write Init Code Here. 在这里写初始化Script的代码
        /// Run in UI Thread. 在UI线程中运行
        /// Such as Init Kinect. 比如Init Kinect. 
        /// </summary>
        public override void InitScript()
        {
            Driver.Kinect_InitKinect();

        }

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        /// 
        public override void Script_Process()
        {

            Function.Speech_TTS("I want to play riddles", false);
            Thread.Sleep(1000);
            Function.Speech_TTS("you can start",false);
            Thread.Sleep(1000);
            //Function.Move_Distance(0, 0, 180);
            AskQuestion();
            
        }

        // nType
        // 1: 第一轮
        // 2: 第二轮
        public static void RecognizeOneQuestion(object   nType)
        {
            AudioDetection audio = new AudioDetection();

            ArrayList userProblems = new ArrayList();

            string[] questions = new string[] {
                "Who's the most handsome person in Canada"   ,
                "How many time zones are there in Canada"    ,
                "What's the longest street in the world" ,
                "Where was the Blackberry Smartphone developed"  ,
                "What is the world's largest coin"   ,
                "In what year was Canada invaded by the USA for the first time"  ,
                "What year was Canada invaded by the USA for the second time"    ,
                "What country holds the record for the most gold medals at the Winter Olympics"  ,
                "When was The Mounted Police formed" ,
                "When was The Royal Canadian Mounted Police formed"  ,
                "Where is Canada's only desert"  ,
                "How big is Canada's only desert"    ,
                "What is a nanobot"  ,
                "How small can a nanobot be" ,
                "Which was the first computer with a hard disk drive"    ,
                "When was the first computer with a hard disk drive launched"    ,
                "How big was the first hard disk drive"  ,
                "What was the first computer bug"    ,
                "What is a Mechanical Knight"    ,
                "What is the AI knowledge engineering bottleneck"    ,
                "What is a chatbot"  ,
                "Are self-driving cars safe" ,
                "Who invented the compiler"  ,
                "Who created the C Programming Language" ,
                "Who created the Python Programming Language"    ,
                "Is Mark Zuckerberg a robot" ,
                "Who is the inventor of the Apple I microcomputer"   ,
                "Who is considered to be the first computer programmer"  ,
                "Which program do Jedi use to open PDF files"    ,
                "Where is the shelf"    ,
                "Where is the plant"    ,
                "How many chairs are there in the dining room"   ,
                "What's the smallest food"   ,
                "What's the lightest drink"  ,
                "What day is today"  ,
                "In which year was RoboCup@Home founded" ,
                "where is the sofa"  ,
                "where is the chair" ,
                "where is the table" ,
                "where is the tvtable"   ,
                "where can I find the drink" ,
                "where can I find the food"  ,
                "where can I find the bed"
            };

            userProblems.Add(questions);

            string[] answers = new string[]
            {
                    "I that Justin Trudeau is very handsome" ,
                    "Canada spans almost 10 million square km and comprises 6 time zones" ,
                    "Yonge Street in Ontario is the longest street in the world" ,
                    "It was developed in Ontario, at Research In Motion's Waterloo offices"  ,
                    "The Big Nickel in Sudbury, Ontario. It is nine meters in diameter"  ,
                    "The first time that the USA invaded Canada was in 1775"  ,
                    "The USA invaded Canada a second time in 1812"   ,
                    "Canada does! With 14 Golds at the 2010 Vancouver Winter Olympics"   ,
                    "The Mounted Police was formed in 1873"  ,
                    "In 1920, when The Mounted Police merged with the Dominion Police"   ,
                    "Canada's only desert is British Columbia"   ,
                    "The British Columbia desert is only 15 miles long"  ,
                    "The smallest robot possible is called a nanobot"   ,
                    "A nanobot can be less than one-thousandth of a millimeter" ,
                    "The IBM 305 RAMAC"  ,
                    "The IBM 305 RAMAC was launched in 1956" ,
                    "The IBM 305 RAMAC hard disk weighed over a ton and stored 5 MB of data" ,
                    "The first actual computer bug was a dead moth stuck in a Harvard Mark II"   ,
                    "A robot sketch made by Leonardo DaVinci"    ,
                    "It is when you need to load an AI with enough knowledge to start learning"  ,
                    "A chatbot is an A.I. you put in customer service to avoid paying salaries"  ,
                    "Yes. Car accidents are product of human misconduct" ,
                    "Grace Hoper. She wrote it in her spare time"    ,
                    "C was invented by Dennis MacAlistair Ritchie"   ,
                    "Python was invented by Guido van Rossum"    ,
                    "Sure. I've never seen him drink water"  ,
                    "My lord and master Steve Wozniak"   ,
                    "Ada Lovelace"   ,
                    "Adobe Wan Kenobi"   ,
                    "The shelf is in the kitchen"    ,
                    "The plant is in the living room"    ,
                    "There is no chair in the dining room"   ,
                    "The bread is the smallest in the food category" ,
                    "The Coke Zero, is lighter than water"   ,
                    "Today is Friday"    ,
                    "RoboCup@Home was founded in 2006"   ,
                    "Near the table" ,
                    "Near the tvtable"   ,
                    "Near the sofa"  ,
                    "Between the chairs" ,
                    "In the kitchen" ,
                    "In the dining room" ,
                    "In the bedroom"
            };
            int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(userProblems);

            isRecognized = true;

            Function.Speech_TTS(questions[resultInt[0]], true);
            Thread.Sleep(4000);

            if ((int)nType == 2)
            {
                Function.Move_Distance(0, 0, audio.getAudioAngle() * 1.2f);
                Console.WriteLine(audio.getAudioAngle() * 1.2f);
            }
            Function.Speech_TTS(answers[resultInt[0]], true);

            bLastThreadCancel = true;

        }
        public void AskQuestion()
        {

            //here is the logic
            //resultInt[0] return the index of the sentence that recognized;


            //the riddle game;
            //ask 5 questions;
            
            //第一个任务，直接语音识别
            Console.WriteLine("SPR第一轮 直接语音识别");

            for (int i = 0; i < 5; i++)   
            {
                ParameterizedThreadStart s = new ParameterizedThreadStart(RecognizeOneQuestion);
                Thread  newThread = new Thread(s);

                isRecognized = false;
                bLastThreadCancel = true;

                newThread.Start(1);
                int g = 0;
                while (bLastThreadCancel == false && g <= 20)
                {
                    Thread.Sleep(1000);
                    ++g;

                    if (isRecognized == true)
                    {
                        g = 0;
                    }
                }

                if (bLastThreadCancel == false)
                {
                    newThread.Abort();
                }
            }
           
            
            Console.WriteLine("SPR第二轮 开启声源定位");
            //第二个任务，声源定位语音识别
           for (int i = 0; i < 5; i++)
            {
 
            }
            
            
        }
        
    }
}
/*不舍得删
 * 
 * userProblems.Add(new string[]{"what is the capital of China","what is the biggest province of China","what is the world biggest island",
                "what was the former name of New York?","what is China's national animal","how large is the area of China","how many hours in a day",
                "how many season are there in one year","how many children did Queen Victoria have","how many seconds in one minute","who was the first president of the USA"});
            string[] answer = new string[] { "The answer is Beijing" , "The answer is Xinjiang" , "The answer is greenland" , "The answer is New Amsterdam" , "The answer is Panda" ,
                "The answer is Nine million and six hundred thousand saquare kilometers","The answer is twentyfour","The answer is four","She had nine children","The answer is sixty",
            "The first president in the USA is George Washington"};
 */
