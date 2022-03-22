using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Service;
using System.Threading;
using iTextSharp.text;
namespace SITRobotSystem_wpf.BLL.Competitions.MORecognition
{
    class MORecognitionTask : Tasks.Tasks
    {

        int ThingId;
        
        public bool Ready()
        {
            ThingId = 0;
            return true;
        }
        public bool ReachBookcase(string name)
        {
            speak("I will reach " + name + " for ready");
            //this.moveToPlaceByName(name);
            baseCtrl.moveToDirectionSpeed(0.2f, 0f);
          //  baseCtrl.moveToDirection(0, 0, -90);
            //speak("let me sleep for 14 seconds");
            Thread.Sleep(16000);
            return true;
        }
        public bool SearchObject(Goods obj)
        {
            return this.FaceToGoods(obj);
        }


        public float BestFindPlace = 0.72f;
        public bool GraspObject(SurfResult surfres)
        {
            this.ArmGet();
            return true;
        }
        public bool PlaceObject()
        {
            this.speak("I will place the thing here.");
            Thread.Sleep(1000);
            this.ArmPlace();
            return true;
        }

        public bool PlaceObject2()
        {
            this.speak("I will place the thing here.");
            Thread.Sleep(1000);
            this.ArmPlace(102);
            return true;
        }

        public bool HandObject(int time)
        {
            Thread.Sleep(time);
            return true;
        }
        public void PdfTest(string myPath)
        {
            this.thePath = myPath;
            Thread thread = new Thread(this.PDFMaker);
            thread.IsBackground = true;
            thread.Start();
        }
        public string[] infos;
        public Image[] imgs;
        public string thePath;
        public void PDFMaker()
        {
            //getCorrentImg();
            PDFMaker pdfMaker = new PDFMaker(thePath);
            pdfMaker.Maker();
        }
        public virtual void ArmPlace(int id=102)
        {
           // baseCtrl.moveToDirectionSpeed(-0.1f, 0f);
            ArmAction armAction1 = new ArmAction(id, "put");
            armCtrl.putThing(armAction1);
            ArmAction armAction2 = new ArmAction(1000, "put");
            armCtrl.putThing(armAction2);
        }


        public virtual void ArmGet()
        {
            ArmAction armAction0 = new ArmAction(300, "get");
            armCtrl.getThing(armAction0);
         //   baseCtrl.moveToDirectionSpeed(-0.3f, 0f);
        }
    }
}
