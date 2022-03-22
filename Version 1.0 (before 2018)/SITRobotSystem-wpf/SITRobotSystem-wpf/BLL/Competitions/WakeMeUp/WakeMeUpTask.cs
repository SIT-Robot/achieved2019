using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Competitions.WakeMeUp
{
    class WakeMeUpTask :Tasks.Tasks
    {
        public string milk;
        public string fruit;
        public string cereal;
        SurfResult trayres;
        public SurfResult bowlres;
        public Place tray;
        //AwakeningOwner/*
        public bool IsAwake()
        {
            List<User> users = new List<User>();
            users = this.getAllUser();
            for (int i = 0; i < users.Count; i++)
            {
                if(users[i].isSeated==true)
                    return true;
            }
            return false;
        }
        public bool Playsound()
        {
            this.speak("it is time to get up");
            Thread.Sleep(1000);
            return true;
        }
        public bool Greet()
        {
            this.speak("Good morning");
            Thread.Sleep(1000);
            return true;
        }
        /*
         * smart-house option
         */
        public bool TurnOnLight()
        {
            return true;
        } //AwakeningOwner*/

        //Deliver newspaper option
        public bool GotoNews ()
        {
            return true;
        }
        public bool FindNews()
        {
            return true;
        }
        public bool GetNews()
        {
            return true;
        }
        public bool ReturnOwner()
        {
            return true;
        }
        public bool DeliverNews()
        {
            return true;
        }//Deliver newspaper option

        //Take breakfast order
        public bool ShowOrder(string[] fruitlist,string[] snacklist,string[] milklist,string[] cereallist)
        {
            string cmdfruit="";
            for (int i = 0; i < fruitlist.Length; i++)
                cmdfruit += fruitlist[i] + ",";
            string cmdsnack="";
            for (int i = 0; i < snacklist.Length; i++)
                cmdsnack += snacklist[i] + ",";
            string cmdmilk = "";
            for (int i = 0; i < milklist.Length; i++)
                cmdmilk += milklist[i] + ",";
            string cmdcereal = "";
            for (int i = 0; i < cereallist.Length; i++)
                cmdcereal += cereallist[i]+",";

            this.speak("there are "+cmdfruit+"as fruit");
            Thread.Sleep(1000);
            this.speak(cmdsnack + "as snack");
            Thread.Sleep(1000);
            this.speak(cmdmilk+"as milk");
            Thread.Sleep(1000);
            this.speak(cmdcereal + "as cereal today");
            Thread.Sleep(1000);
            return true;
        }
        public bool AskedMilk(string[] obj)
        {
            this.baseSpeech = new SitRobotSpeech();
            this.speak("which kind of milk would you like?");
            Thread.Sleep(1000);
            milk = "";
            this.baseSpeech.WakeMeUpSpeechRecognize(obj);
            while (string.IsNullOrWhiteSpace(milk))
            {
                milk = baseSpeech.ReturnCommand;
            }
            return true;
        }
        public bool AskedFruit(string[] obj)
        {
            this.baseSpeech = new SitRobotSpeech();
            this.speak("which kind of fruit or snack would you like?");
            Thread.Sleep(1000);
            fruit = "";
            this.baseSpeech.WakeMeUpSpeechRecognize(obj);
            while (string.IsNullOrWhiteSpace(fruit))
            {
                fruit = baseSpeech.ReturnCommand;
                
            }

            baseSpeech.speechStop();
            return true;
        }
        public bool AskedCereal(string[] obj)
        {
            this.baseSpeech = new SitRobotSpeech();
            this.speak("which kind of cereal would you like?");
            Thread.Sleep(1000);
            cereal = "";
            this.baseSpeech.WakeMeUpSpeechRecognize(obj);
            while (string.IsNullOrWhiteSpace(cereal))
            {
                cereal = baseSpeech.ReturnCommand;
            }
            return true;
        }
        public bool StatePlaceDelivered(string strpla)
        {

            this.speak("I will place "+fruit+" "+milk +" "+cereal+" to "+strpla);
            Thread.Sleep(5000);
            return true;
        }//Take breakfast order

        //Open Kitchen door option
        //Open Kitchen door option

        //Turn on Kitchen light option
        //Turn on Kitchen light option

        //Serve the breakfast
        public bool FindTray(Goods obj)
        {
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            trayres = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("searching for " + obj.Name + i + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
            }
            if (!result.isSuccess)
                return false;
            trayres = result;
            BaseConnection baseConnection = new BaseConnection();
            tray = baseConnection.getPosition();
            return result.isSuccess;
        }
        public bool FindBowl(Goods obj)
        {
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            bowlres = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            for (int i = 0; i < 5; i++)
            {
                speak("I am finding" + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                res = result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
                if (result.isReachable)
                {
                    speak("I found " + obj.Name);
                    VisionCtrl.endCoordinateMapping();
                    break;
                }
            }

            if (res)
            {
                speak("let me get " + obj.Name);
                adjustMoveDirection(result.centerCameraPoint);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
                speak("sorry I can't reach " + obj.Name);
            }
            if (!result.isSuccess)
                return false;
            bowlres = result;
            return res;
        }
        public bool FindObj(Goods obj)
        {
            return this.FaceToGoods(obj);
        }

        public bool GraspObj()
        {
            this.ArmGet();
            return true;
        }
        /// <summary>
        /// 移动到 place
        /// 放下手中物品
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public bool MoveAndPlaceObj(Place place)
        {
            this.moveToPlace(place);
            this.speak("I will place the thing here.");
            Thread.Sleep(1000);
            this.ArmGive();
            return true;
        }
        public bool PourCereal()
        {
            this.ArmPour();
            return true;
        } //Serve the breakfast
        public void ArmPour()
        {
            ArmAction armAction1 = new ArmAction(52, "pour");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);

        }
        public virtual void ArmPlace()
        {
            ArmAction armAction1 = new ArmAction(21, "put");
            armCtrl.putThing(armAction1);
            Thread.Sleep(800);
            baseCtrl.moveToDirectionSpeed(0.3f, 0f);
            ArmAction armAction2 = new ArmAction(22, "put");
            armCtrl.putThing(armAction2);
            Thread.Sleep(800);
            ArmAction armAction3 = new ArmAction(1000, "put");
            armCtrl.putThing(armAction3);
        }
        //Place the spoon optional
        //Place the spoon optional

        //Turn off kitchen light smart-house option
        //Turn off kitchen light smart-house option

        //Do the bed optional
        //Do the bed optional
    }
}
