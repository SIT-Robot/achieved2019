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
        private int nMale = 0;
        private int nFemale = 0;
        private int nPersonSitting = 0;
        private int nPersonStanding = 0;

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
            Function.Move_Distance(0, 0, 180);

            CountPeople();
            AskQuestion();
        }

        /***************************************************
         * 代码注释 2019.4.20  01:24
         * 
         * 测试使用百度云人脸识别
         **************************************************/
        public void CountPeople()
        {
            List<FaceInfo> stFaces =Function.Vision_BaiduFaceDetect();
            double fYAxis = 0.0;
            int nMale = 0;
            int nFemale = 0;

            foreach (var Face in stFaces)
            {
                fYAxis += Face.FaceLocation.Y;
                if (Face.sex == 1) nMale++;
                else nFemale++;
            }

            double fAvgY = fYAxis / (nMale + nFemale);

            foreach (var Face in stFaces)
            {
                if (Face.FaceLocation.Y < fAvgY)
                {
                    nPersonStanding++;
                }
                else
                {
                    nPersonSitting++;
                }
            }
            String a = nPersonStanding.ToString() + " Person stand, " + nPersonSitting.ToString() + " Person sit.";
            Console.WriteLine(a);
            Function.Speech_TTS(a, false);

            a = nMale.ToString() +  "man, " + nFemale.ToString() + " women";
            Console.WriteLine(a);
            Function.Speech_TTS(a, false);

            Thread.Sleep(300);
        }


        public void AskQuestion()
        {
            //add questions;
            string[] questions = new string[] {
                "Who's the most handsome person in Canada"
                ,"How many time zones are there in Canada"
                ,"What's the longest street in the world"
                ,"Where was the Blackberry Smartphone developed"
                ,"What is the world's largest coin"
                ,"In what year was Canada invaded by the USA for the first time"
                ,"What year was Canada invaded by the USA for the second time"
                ,"What country holds the record for the most gold medals at the Winter Olympics"
                ,"When was The Mounted Police formed"
                ,"When was The Royal Canadian Mounted Police formed"
                ,"Where is Canada's only desert"
                ,"How big is Canada's only desert"
                ,"What is a nanobot"
                ,"How small can a nanobot be"
                ,"Which was the first computer with a hard disk drive"
                ,"When was the first computer with a hard disk drive launched"
                ,"How big was the first hard disk drive"
                ,"What was the first computer bug"
                ,"What is a Mechanical Knight"
                ,"What is the AI knowledge engineering bottleneck"
                ,"What is a chatbot"
                ,"Are self-driving cars safe"
                ,"Who invented the compiler"
                ,"Who created the C Programming Language"
                ,"Who created the Python Programming Language"
                ,"Is Mark Zuckerberg a robot"
                ,"Who is the inventor of the Apple I microcomputer"
                ,"Who is considered to be the first computer programmer"
                ,"Which program do Jedi use to open PDF files"
                ,"Where is the shelf "
                ,"Where is the plant "
                ,"How many chairs are in the dining room"
                ,"What's the smallest food"
                ,"What's the lightest drink"
                ,"What day is today"
                ,"In which year was RoboCup@Home founded"
                ,"where is the sofa"
                ,"where is the chair"
                ,"where is the table"
                ,"where is the tvtable"
                ,"where can i find the drink"
                ,"where can i find the food"
                ,"where can i find the bed"
                ,"what is the number of the people who waiving arms"
                ,"what is the number of the people who standing"
                ,"what is the number of the people who sitting"};

            string[] answers = new string[]
            {
                "I that Justin Trudeau is very handsome."
                ,"Canada spans almost 10 million square km and comprises 6 time zones"
                ,"Yonge Street in Ontario is the longest street in the world."
                ,"It was developed in Ontario, at Research In Motion's Waterloo offices."
                ,"The Big Nickel in Sudbury, Ontario. It is nine meters in diameter."
                ,"The first time that the USA invaded Canada was in 1775"
                ,"The USA invaded Canada a second time in 1812."
                ,"Canada does! With 14 Golds at the 2010 Vancouver Winter Olympics."
                ,"The Mounted Police was formed in 1873."
                ,"In 1920, when The Mounted Police merged with the Dominion Police."
                ,"Canada's only desert is British Columbia."
                ,"The British Columbia desert is only 15 miles long."
                ,"The smallest robot possible is called a nanobot. "
                ,"A nanobot can be less than one-thousandth of a millimeter. "
                ,"The IBM 305 RAMAC."
                ,"The IBM 305 RAMAC was launched in 1956."
                ,"The IBM 305 RAMAC hard disk weighed over a ton and stored 5 MB of data."
                ,"The first actual computer bug was a dead moth stuck in a Harvard Mark II."
                ,"A robot sketch made by Leonardo DaVinci."
                ,"It is when you need to load an AI with enough knowledge to start learning."
                ,"A chatbot is an A.I. you put in customer service to avoid paying salaries."
                ,"Yes. Car accidents are product of human misconduct."
                ,"Grace Hoper. She wrote it in her spare time."
                ,"C was invented by Dennis MacAlistair Ritchie."
                ,"Python was invented by Guido van Rossum."
                ,"Sure. I've never seen him drink water."
                ,"My lord and master Steve Wozniak."
                ,"Ada Lovelace."
                ,"Adobe Wan Kenobi."
                ,"The shelf is in the kitchen."
                ,"The plant is in the living room."
                ,"There is no chair in the dining room."
                ,"The bread is the smallest in the food category."
                ,"The Coke Zero, is lighter than water."
                ,"Today is Friday."
                ,"RoboCup@Home founded in 2006。"
                ,"Near the table."
                ,"Near the tvtable."
                ,"Near the sofa."
                ,"Between the chairs."
                ,"In the kitchen."
                ,"In the dining room."
                ,"In the bedroom."
                , "2"
                ,"There are "+ nPersonStanding.ToString()+" people standing"
                ,"There are " + nPersonSitting.ToString()+" people sitting"
            };

            Function.Speech_TTS("Who want to play riddles with me?", false);
            ArrayList userProblems = new ArrayList();
            userProblems.Add(questions);

            //here is the logic
            //resultInt[0] return the index of the sentence that recognized;


            //the riddle game;
            //ask 5 questions;
            AudioDetection audio = new AudioDetection();
            for (int i = 0; i < 5; i++)
            {
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(userProblems);
                Function.Move_Distance(0, 0, audio.getAudioAngle() * 1.2f);
                System.Console.WriteLine(audio.getAudioAngle() * 1.2f);
                Function.Speech_TTS(answers[resultInt[0]], false);
            }

            Console.WriteLine("SPR第二轮 开启声源定位");

            for (int i = 0; i < 5; i++)
            {
                int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(userProblems);
                Function.Move_Distance(0, 0, audio.getAudioAngle() * 1.2f);
                System.Console.WriteLine(audio.getAudioAngle() * 1.2f);
                Function.Speech_TTS(answers[resultInt[0]], false);
            }

        }
    }
}