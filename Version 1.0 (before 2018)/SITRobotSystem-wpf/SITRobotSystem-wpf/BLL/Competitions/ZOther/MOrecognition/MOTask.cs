using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.MOrecognition
{
    public class MOTask : Tasks.Tasks
    {

        public void handGetHigh()
        {
            ArmAction armAction0 = new ArmAction(1, "catch"); //1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void handGetMiddle()
        {
            ArmAction armAction0 = new ArmAction(1, "catch"); //1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void handGetBase()
        {
            ArmAction armAction0 = new ArmAction(1, "catch"); //1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public void handPut()
        {
            ArmAction armAction0 = new ArmAction(1, "catch"); //1代表发送的数据
            armCtrl.getThing(armAction0);
        }

        public string[] infos;
        public Image[] imgs;
        public string thePath;
        public void PDFMaker()
        {
            PDFMaker pdfMaker = new PDFMaker(thePath);
            pdfMaker.Maker();
        }

        public SurfResult findObject(Goods obj)
        {
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result=new SurfResult();
            VisionCtrl.startCoordinateMapping();
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("searching for " + obj.Name + i + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
            }
            return result;
        }

        public string[] getAllGoodsName()
        {
                  DBCtrl dbCtrl = new DBCtrl();
                  string[] strThing = dbCtrl.GetAllGoodsName();
            return strThing;
        }

        public float BestFindPlace=0.6f;
        public void OGetObject(SurfResult sn)
        {
            float detaX = sn.centerCameraPoint.Z - BestFindPlace;
            float detaY = sn.centerCameraPoint.X-0;
            baseCtrl.moveToDirectionSpeed(-detaX,-detaY);
            
            baseCtrl.moveToDirectionSpeed(-0.2f, 0);
            armCtrl.send(new ArmAction(61,""));

            baseCtrl.moveToDirectionSpeed(0.2f, 0);
            armCtrl.send(new ArmAction(62, ""));

            baseCtrl.moveToDirectionSpeed(-0.2f, 0);
            armCtrl.send(new ArmAction(63, ""));

            //baseCtrl.moveToDirectionSpeed(0.2f, 0);
            //armCtrl.send(new ArmAction(64, ""));

        }
    }
}
