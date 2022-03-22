using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.makehamburger
{
    class makeHamburgerStage:Stage
    {
        makeHamburgerTask task = new makeHamburgerTask();
        Goods g = new Goods();

        public override void init()
        {
            task.initSurfFrmae();
            
        }

        public void MakingHamburger()
        {
            task.SelectHumburger();
            float pointDistance = 0.27f;
            string meat;
            while (true)
            {
                if (task.baseSpeech.ReturnCommand!=null)
                {
                    meat = task.baseSpeech.ReturnCommand;
                    break;
                }
            }

            //从左至右摆放：底面包，生菜，上面包，操作台，鸡肉，牛肉，猪肉
            //初始位置在：操作台最佳抓取距离

            //task.moveToPlaceByName("material");
            task.SendXY(0.0f, 3*pointDistance);
            g=task.GetGoodsByName("bread downside");
            //task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandCatch();

            //task.moveToPlaceByName("hearth");
            task.SendXY(0.0f, -3 * pointDistance);
            g = task.GetGoodsByName("finishment");
            task.FaceToGoods(g);
            
            Thread.Sleep(1000);            
            task.HandTurn();
            Thread.Sleep(1000);
            task.SendXY(0.0f, -1 * pointDistance);
            Thread.Sleep(1000);
            task.HandDown();

            //task.moveToPlaceByName("meat");
            if (meat=="chicken")
            {
                task.SendXY(0.0f, -1 * pointDistance);
                g = task.GetGoodsByName(meat);
                //task.FaceToGoods(g);
                Thread.Sleep(1000);
                task.HandCatch();
                task.SendXY(0.0f, 2 * pointDistance);
            }
            else if (meat == "beef")
            {
                task.SendXY(0.0f, -2 * pointDistance);
                g = task.GetGoodsByName(meat);
                //task.FaceToGoods(g);
                Thread.Sleep(1000);
                task.HandCatch();
                task.SendXY(0.0f, 3 * pointDistance);
            }
            else
            {
                task.SendXY(0.0f, -3* pointDistance);
                //g = task.GetGoodsByName(meat);
                task.FaceToGoods(g);
                Thread.Sleep(1000);
                task.HandCatch();
                task.SendXY(0.0f, 4 * pointDistance);
            }

            //task.moveToPlaceByName("hearth");
            g = task.GetGoodsByName("finishment");
            //task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandTurn();
            Thread.Sleep(1000);
            task.SendXY(0.0f, -1 * pointDistance);
            Thread.Sleep(1000);
            task.HandDown();

            //task.moveToPlaceByName("material");
            task.SendXY(0.0f, 3 * pointDistance);
            g = task.GetGoodsByName("vegetable");
            task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandCatch();

            //task.moveToPlaceByName("hearth");
            task.SendXY(0.0f, -3 * pointDistance);
            g = task.GetGoodsByName("finishment");
            task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandTurn();
            Thread.Sleep(1000);
            task.SendXY(0.0f, -1 * pointDistance);
            Thread.Sleep(1000);
            task.HandDown();

            //task.moveToPlaceByName("material");
            task.SendXY(0.0f, 4 * pointDistance);
            g = task.GetGoodsByName("bread upside");
            task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandCatch();

            //task.moveToPlaceByName("hearth");
            task.SendXY(0.0f, -4 * pointDistance);
            g = task.GetGoodsByName("finishment");
            task.FaceToGoods(g);
            Thread.Sleep(1000);
            task.HandTurn();
            Thread.Sleep(1000);
            task.SendXY(0.0f, -1 * pointDistance);
            Thread.Sleep(1000);
            task.HandDown();

            task.speak("Ok,I have finished.please take your hamburger.");
        }

        
    }
}
