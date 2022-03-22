using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using System;
using System.Collections;
using System.Threading;


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
            Function.Hand_Init();
  
            
        }
        LocationInfo car_place;
        LocationInfo kicten_table;

        /// <summary>
        /// 在这里写Script的过程，可随意阻塞。
        /// </summary>
        public override void Script_Process()
        {
            Thread.Sleep(500);
            FuckFollow();
            //Bring the grocieries in
            FuckOrders();
            //Bag delivery
            //Asking for help
            Function.BodyDetect_GetAllusers();
            Function.Speech_TTS("Hello,could you help me carry the", false);
            public void FuckFollow()
            {
                Function.Speech_TTS("I am ready", false);
                while (true) 
                { ArrayList speech = new ArrayList();
                    speech.Add(new string[] { "Follow  me","stop follow me"});
                    int[] resultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                    if (resultInt[0] == 0);
                    {
                        Function.BodyDetect_ShowBodyDetectWindow();
                        Function.Speech_TTS(" Start Follow ", true);
                        Function.BodyDetect_ShowBodyDetectWindow();
                        Function.Speech_TTS("End Follow", true);
                        Function.Move_SetSpeed(0, 0, 0);
                    }

                  
                    else if (resultInt[0] == 1)
                    { car_place=Function.Location_GetCurrectLocationFormRos();
                        break;

                    }
                    public void FuckOrders()
                    { 
                    while (true)
                    { Function.Hand_GoUp(1500);
                        Function.Speech_TTS("please hand me the bag", false);
                        ArrayList speech = new ArrayList();
                        speech.Add(new string[] { "take this" });
                        speech.Add(new string[] { "bag" });
                        speech.Add(new string[] { "to the" });

                        string[] places = new string[]
                        { "Bedroom"};
                        speech.Add(places);
                        int[] ResultInt = Function.Speech_Recognize_StartSimpleRecognize(speech);
                        if (ResultInt[0] == 0)
                        {
                            string stuff_bag = "bag";
                            string place_name = places[resultInt[3]];
                            Function.Speech_TTS("do you want to take" + stuff_bag + "to the" + place_name, false);
                            speech = new ArrayList();
                            speech.Add(new string[] { "yes", "NO" });
                            if (Function.Speech_Recognize_StartSimpleRecognize(speech)[0] == 0)

                            {
                                Function.Speech_TTS("OK, I know" + stuff_bag, true);
                                Function.Move_Navigate(Function.Location_DeleteLocationInfoByName("bbb"));
                                Function.Hand_SetRotationPosition(0x05);
                                Function.Speech_TTS("could i get ", false);
                                Function.Move_Navigate(car_place);
                                Console.WriteLine("succeed");
                                break;
                            }
                            else { Function.Speech_TTS("I am wrong",false ) };

                        } 




                    }

                            

                    }





                }    
            }


        }   
    }
}
