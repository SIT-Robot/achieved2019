using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.BSR
{


    class BSRStage:Stage
    {

        public bool isThroughDoor = false;

        private BSRTask task;
        public override void init()
        {
            task = new BSRTask();
            task.startDepthReader();
            //task.initSpeech();
        }

        public void StationaryMoving()
        {
            
        }

        public void Navigate()
        {
            //是否过门
            if (isThroughDoor)
            {
                task.BSRmovetoPlaceByName("1");
                task.BSRmovetoPlaceByName("2");

                ThrowDoorByName("A");

                task.BSRmovetoPlaceByName("3");
                task.BSRmovetoPlaceByName("4");

                ThrowDoorByName("C");

                task.BSRmovetoPlaceByName("5");
                task.BSRmovetoPlaceByName("6");
                ThrowDoorByName("C");
                task.BSRmovetoPlaceByName("7");
            }
            else
            {
                task.BSRmovetoPlaceByName("1");
                task.BSRmovetoPlaceByName("2");
                task.BSRmovetoPlaceByName("3");
                task.BSRmovetoPlaceByName("4");
                task.BSRmovetoPlaceByName("5");
                task.BSRmovetoPlaceByName("6");
                task.BSRmovetoPlaceByName("7");
            }
            

            
            //while (true)
           /* {

                bool res=task.IsdoorOpen();
                if (res)
                {
                    task.moveDirectionBySpeedW(90);
                    task.moveDirectionBySpeed(0,5);
                    task.moveDirectionBySpeedW(-90);
                }
                Console.WriteLine(res.ToString());
                
            }
            */
            /*
            string[] pointName={"1","2","3","4","5","6","7","A","B","C","D"};

            foreach (var name in pointName)
            {
                task.moveToPlaceByName(name);
                task.speak("I have arrived at "+name);
                System.Threading.Thread.Sleep(1200);
            }*/
        }
           private void ThrowDoorByName(String throwDoor)
        {
            bool res = task.moveToPlaceByName(throwDoor);
               if (res)
               {
                   bool[] ifSoftNow = new bool[3];
                   while (true)
                   {
                       for (int i = 0; i < ifSoftNow.Length; i++)
                       {
                           ifSoftNow[i] = task.IsdoorOpen();
                       }
                       if ((ifSoftNow[0] == ifSoftNow[1]) && (ifSoftNow[0] == ifSoftNow[2]))
                       {
                           break;
                       }
                   }


                   if (ifSoftNow[2])
                   {
                       task.speak("the door is open");
                       Thread.Sleep(200);
                       task.moveDirectionBySpeedW(90);
                       Thread.Sleep(200);
                       task.moveDirectionBySpeed(0, -2.5f);
                       Thread.Sleep(200);
                       task.moveDirectionBySpeedW(-90);
                       Thread.Sleep(200);
                   }
                   else
                   {
                       task.speak("the door is closed");
                       task.moveToPlaceByName(throwDoor+"2");
                       Thread.Sleep(200);
                       task.moveDirectionBySpeedW(90);
                       Thread.Sleep(200);
                       task.moveDirectionBySpeed(0, -2.5f);
                       Thread.Sleep(200);
                       task.moveDirectionBySpeedW(-90);
                       Thread.Sleep(200);
                   }
               }
           
        }


    }
}
