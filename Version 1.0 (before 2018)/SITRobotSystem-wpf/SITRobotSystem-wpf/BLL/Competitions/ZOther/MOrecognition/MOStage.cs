using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.MOrecognition
{
    public class MOStage:Stage
    {
        public MOTask task;

        public MOStage()
        {
            task = new MOTask();
        }

        public override void init()
        {
            task.initSurfFrmae();
        }

        public void start(string[] _things)
        {
            Goods good=new Goods();

            good.Name = "";
            string [] things = task.getAllGoodsName();
            List<SurfResult> surfResults=new List<SurfResult>();
            //task.moveToPlaceByName("shelf");
            for (int i = 0; i < things.Length; i++)
            {
                Goods g = new Goods();
                g.Name = things[i];
                g=task.GetGoodsByName(things[i]);
                SurfResult result = task.findObject(g);
                surfResults.Add(result);
            }
            List<SurfResult> surffinalResults=new List<SurfResult>();
            List<string> sPath=new List<string>();
            List<string> sName=new List<string>();
            for (int j = 0; j < surfResults.Count; j++)
            {
                if (surfResults[j].isSuccess)
                {
                    surffinalResults.Add(surfResults[j]);
                    sPath.Add(surfResults[j].ImgPath);
                    sName.Add(surfResults[j].name);
                    
                }
            }
            //PdfTest();
            task.speak("I found");
            Thread.Sleep(1000);
            foreach (var sn in sName)
            {
                task.speak(sn);
                Thread.Sleep(800);
            }

            foreach (var res in surffinalResults)
            {
                
                if (res.isReachable)
                {
                    task.speak("let me get " + res.name);
                    Thread.Sleep(800);
                    task.OGetObject(res);
                }
                else
                {
                    task.speak("sorry,I can't reach " + res.name);
                    Thread.Sleep(800);
                }
                
            }

        }

        public void PdfTest(string thePath)
        {
            task.thePath = thePath;
            Thread thread = new Thread(task.PDFMaker);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
