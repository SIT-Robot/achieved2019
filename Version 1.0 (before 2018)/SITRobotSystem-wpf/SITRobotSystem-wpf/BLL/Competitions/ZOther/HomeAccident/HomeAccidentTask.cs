using System.Collections.Generic;
using System.IO;
using System.Threading;
using iTextSharp.text;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.HomeAccident
{
    internal class HomeAccidentTask : Tasks.Tasks
    {

        public void findPeople()
        {
            List<User> users = this.getAllUser();
            while (users.Count == 0)
            {
                users = this.getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();

            foreach (var user in users)
            {
                if (user.isRaisingHand)
                {
                    usersposition.Add(user.BodyCenter);
                }
            }
            foreach (var position in usersposition)
            {
                this.moveToDirection(position.Z - 0.5f, position.X, 0);
            }
        }

        public void AskPeople()
        {
            this.baseSpeech.robotSpeak("what can i do for you");
            this.baseSpeech.robotSpeak("Please give me the aid thing,water, the aid box or phone?");
            this.baseSpeech.homeRecognize();
        }

        //public string info;
        //public void PDFThread()
        //{
        //    PDFMaker(info);
        //}
        //public void PDFMaker(string info)
        //{
        //    SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
        //    //拍照
        //    Image PP = Image.GetInstance(new FileStream(this.getCorrentImg(), FileMode.Open, FileAccess.ReadWrite));

        //    //获取环境地图
        //    Image EM = Image.GetInstance(sitRobotHub.getEM().ToArray());

        //    //获取病人位置信息
        //    Image PM = Image.GetInstance(sitRobotHub.getPM().ToArray());
            
        //    PDFMaker pdfMaker = new PDFMaker(EM, PM, PP, info);
        //    pdfMaker.Maker();
        //}

        public void ProcessCommand(Command command)
        {
            switch (command.action)
            {
                case ActionType.put:
                    Thread.Sleep(1000);
                    this.ArmPut();
                    break;

                case ActionType.move:
                    this.moveToPlaceByName(command.thing.Name);
                    break;

                case ActionType.get:
                    Goods g = new Goods();
                    g = this.GetGoodsByName(g.Name);
                    this.FaceToGoods(g);
                    Thread.Sleep(1000);
                    this.ArmGet();
                    break;

                default:
                    break;

            }
        }
    }
}
