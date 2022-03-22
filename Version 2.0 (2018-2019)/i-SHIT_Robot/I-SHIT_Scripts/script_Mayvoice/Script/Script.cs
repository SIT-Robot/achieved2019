using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System.Collections;

namespace i_Shit_Scirpt.Script
{
    public class MyScript : Scripts
    {
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
        public override void Script_Process()
        {
            AudioDetection audio = new AudioDetection();
            //ysh does not take responsability for the wrong answer
            //questions and answers all copying and pasting from internet
            //add questions;
            string[] questions = new string[] {"What day is today",
                " What is your name",
                " What is your school name",
                " How old do you think I am" ,
                " How many members are there in your team",
                " How many chairs are there in the diningroom",
                " What is the smallest food",
                " What is the lightest drink",
                " Where is the desk",
                "It is in the bedroom",//10
                " Where is the tray", 
                 "It is in the shelf",
                " Am I a man or woman",
                "Sorry, I could not tell",
                " What is the color of shampoo",
                " How many objects are there in the shelf",
                " What is the capital of England",
                "What are the colors of the British flag" ,
                " Who invented the first robot",
                " Where does the term computer bug come from",//10
                "What is the highest point in the world",
                " What city is the capital of the united states" ,
                " In which room ls the dining table" ,
                " Where is located the fridge" ,
                "Where can I find chopsticks",
                " How many minutes in an hour" ,
                " What is the largest lake in China" ,
                " Where can I wash my face",
                " How many fireplace are there in the bedroom",
                " Who was the first president of the USA",
                " What is the biggest city in japan" ,
                " How many sofas are there in the living room",
                " How many children did Queen Victoria have" ,
                "What was the former name of New York",
                " Where is bed",
                " How many books are there in the shelf",
                " How many continents are there in the world",
                " Who invented the first compiler",
                " How often is the Olympic Games held",
                " In which country will the next world expo be held" ,
                "What is the most toothed animal on earth" ,
                " How many fingers are there in one hand" ,
                " When was the statue of liberty built",
                " How many people live in Shanghai" ,
                " What do Chinese people usually eat on Mid-Autumn Festival",
                " What is Canada's national animal " ,
                " What is the color of pineapple" ,
                " Where is my hat",
                " How long is the Great Wall",
                " How many stars are there in the china's national flag",
                " Where can I buy watermelon " ,
                " What is the landmark building in Beijing",
                " What is the number of the police"
            };

            string[] answers = new string[]
            {
                "Saturday",
                "Gay",
                "sit",
                "About twenty years old",
                "twelve","two",
                "The egg is the smallest food",
                "the coke zero ",
                "it is in the bedroom ",
                "it is in the shelf ",
                "sorry, i could not tell ",//10
                "blue ",
                "Eight ",
                "London ",
                "red, white and blue ",
                "Mr.Engelberg",
                "From a mouth trapped in a relay. ",
                "The highest point in the world is Qomolangma. ",
                "Washington Dc",
                "Diningroom",
                "Kitchen",//20
                "Cupboard",
                "the answer is sixty",
                "qinghai", 
                "Lake",
                "In the bathroom",
                "zero",
                "George Washington",
                "Tokyo",
                "THREE",
                "Nine children",//30
                "New Amsterdam",
                "In the Bedroom",
                "Ten",
                "seven",
                "Grace Brewster Murray Hopper invented it",
                "Once every four years",
                "Dubai",
                "Snail",
                "Five",
                "1884",
                "A little over twenty four million",
                "Mooncake",
                "Beaver",
                "Yellow",
                " In the living room",
                " A little Over twenty one million meters",
                "Five",
                "In the supermarket",
                "The palace museum.",
                "110"//50
            };

            ArrayList speech = new ArrayList();
            speech.Add(questions);

            while (true)

            {
                int[] NewInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                Function.Move_Distance(0, 0, audio.getAudioAngle() * 1.2f);
               Function.Speech_TTS(questions[NewInt[0]], false);
                
                Function.Speech_TTS(answers[NewInt[0]], false);

                
            };

        }   



    }
}
