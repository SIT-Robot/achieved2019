using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using SITRobotSystem_wpf.BLL.Connection;

namespace SITRobotSystem_wpf.BLL.Competitions.WakeMeUp
{
    class WakeMeUpStage:Stage
    {
        public WakeMeUpTask wakemeupTask;
        public string[] fruit, drink, cereal,snack,food;

        string strpla="breakfast table";

        int soundstime = 10000;
        public WakeMeUpStage()
        {
            wakemeupTask = new WakeMeUpTask();
            fruit = new[] { "pear", "apple" };
            snack = new[] { "cookie", "oreo", "gum", "potato chips" };
            drink = new[] {"asam", "coconut juice","fruit milk","juice milk" ,"milk tea","palm milk","want milk"};
            cereal = new[] { "oatmeal", "porridge","tomato sauce" };
            food = new[] { "pear", "apple","cookie", "oreo", "gum", "potato chips" };
        }
        public override void init()
        {
            this.wakemeupTask.initSpeech();
            this.wakemeupTask.initSurfFrmae();
            this.wakemeupTask.initBodyDetect();
        }
        
        public bool AwakeHuman()    /*detect human awake 20*/
        {
            this.wakemeupTask.speak("move to Bedroom door");
            Thread.Sleep(1000);
            this.wakemeupTask.moveToPlaceByName("Bedroom door");
            
            this.wakemeupTask.speak("open the bedroom light");
            Thread.Sleep(1000);

            this.wakemeupTask.baseCtrl.OpenLight(bdrlight);

            this.wakemeupTask.speak("move to Bed");
            Thread.Sleep(1000);
            //this.wakemeupTask.moveToPlaceByName("Bed");
            while (!this.wakemeupTask.IsAwake())
            {
                this.wakemeupTask.Playsound();
                Thread.Sleep(1000);
            }
            this.wakemeupTask.Greet();
            return true;
        }
        public bool TakeOrder()     /*understand whole order   20*/
        {
            this.wakemeupTask.ShowOrder(fruit,snack,drink,cereal);
            this.wakemeupTask.AskedFruit(food);
            this.wakemeupTask.AskedMilk(drink);
            this.wakemeupTask.AskedCereal(cereal);
            this.wakemeupTask.StatePlaceDelivered(strpla);
            return true;
        }

        private int kitlight = 1,bdrlight=2;
        public bool ServeBreakfast()
        {
            /*int kpsign = 0;
            //tray
            Goods tray = new Goods();
            tray = this.wakemeupTask.GetGoodsByName("tray");
            //milk
            Goods milk = new Goods();
            milk = this.wakemeupTask.GetGoodsByName(this.wakemeupTask.milk);
            //fruit
            Goods fruit = new Goods();
            fruit = this.wakemeupTask.GetGoodsByName(this.wakemeupTask.fruit);
            //cereal
            Goods cereal = new Goods();
            cereal = this.wakemeupTask.GetGoodsByName(this.wakemeupTask.cereal);

            Goods bowl = new Goods();
            bowl = this.wakemeupTask.GetGoodsByName("bowl");*/



            this.wakemeupTask.speak("move to kitchen door1");
            Thread.Sleep(1000);
            this.wakemeupTask.moveToPlaceByName("kitchen door1");
            this.wakemeupTask.baseCtrl.OpenLight(kitlight);
            this.wakemeupTask.speak("open the kitchen light");
            Thread.Sleep(1000);

            this.wakemeupTask.speak("move to kitchen door2");
            Thread.Sleep(1000);
            this.wakemeupTask.moveToPlaceByName("kitchen door2");
            this.wakemeupTask.baseCtrl.CloseLight(kitlight);


            /*this.wakemeupTask.speak("point 1");
            Thread.Sleep(1000);
            this.wakemeupTask.moveToPlaceByName("kpoint1");

            if (!this.wakemeupTask.FindObj(tray))
            {
                this.wakemeupTask.speak("point 2");
                Thread.Sleep(1000);
                this.wakemeupTask.moveToPlaceByName("kpoint2");
                if (!this.wakemeupTask.FindObj(tray))
                {
                    this.wakemeupTask.speak("point 3");
                    Thread.Sleep(1000);
                    this.wakemeupTask.moveToPlaceByName("kpoint3");
                    if (!this.wakemeupTask.FindObj(tray))
                        return false;
                    else kpsign = 3;
                }
                else kpsign = 2;
            }
            else kpsign = 1;

            switch (kpsign)
            {
                case 1:
                    if (!this.wakemeupTask.FindObj(milk))
                    {
                        this.wakemeupTask.speak("point 2");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint2");
                        if (!this.wakemeupTask.FindObj(milk))
                        {
                            this.wakemeupTask.speak("point 3");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint3");
                            if (!this.wakemeupTask.FindObj(milk))
                                this.wakemeupTask.speak("can not find milk");
                            else
                            {
                                kpsign = 3;
                                BaseConnection baseConnection = new BaseConnection();
                                this.wakemeupTask.tray = baseConnection.getPosition();
                            }
                        }
                        else
                        {
                            kpsign = 2;
                            BaseConnection baseConnection = new BaseConnection();
                            this.wakemeupTask.tray = baseConnection.getPosition();
                        }
                    }
                    else
                    {
                        kpsign = 1;
                        BaseConnection baseConnection = new BaseConnection();
                        this.wakemeupTask.tray = baseConnection.getPosition(); 
                    }break;
                case 2:
                    if (!this.wakemeupTask.FindObj(milk))
                    {
                        this.wakemeupTask.speak("point 3");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint3");
                        if (!this.wakemeupTask.FindObj(milk))
                        {
                            this.wakemeupTask.speak("point 1");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint1");
                            if (!this.wakemeupTask.FindObj(milk))
                                this.wakemeupTask.speak("can not find milk");
                            else kpsign = 1;
                        }
                        else kpsign = 3;
                    }
                    else kpsign = 2; break;
                case 3:
                    if (!this.wakemeupTask.FindObj(milk))
                    {
                        this.wakemeupTask.speak("point 1");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint1");
                        if (!this.wakemeupTask.FindObj(milk))
                        {
                            this.wakemeupTask.speak("point 2");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint2");
                            if (!this.wakemeupTask.FindObj(milk))
                                this.wakemeupTask.speak("can not find milk");
                            else kpsign = 2;
                        }
                        else kpsign = 1;
                    }
                    else kpsign = 3; this.wakemeupTask.GraspObj();
            this.wakemeupTask.MoveAndPlaceObj(this.wakemeupTask.tray);break;
                default:break;
            }
            switch (kpsign)
            {
                case 1:
                    if (!this.wakemeupTask.FindObj(fruit))
                    {
                        this.wakemeupTask.speak("point 2");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint2");
                        if (!this.wakemeupTask.FindObj(fruit))
                        {
                            this.wakemeupTask.speak("point 3");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint3");
                            if (!this.wakemeupTask.FindObj(fruit))
                                this.wakemeupTask.speak("can not find fruit");
                            else kpsign = 3;
                        }
                        else kpsign = 2;
                    }
                    else kpsign = 1; break;
                case 2:
                    if (!this.wakemeupTask.FindObj(fruit))
                    {
                        this.wakemeupTask.speak("point 3");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint3");
                        if (!this.wakemeupTask.FindObj(fruit))
                        {
                            this.wakemeupTask.speak("point 1");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint1");
                            if (!this.wakemeupTask.FindObj(fruit))
                                this.wakemeupTask.speak("can not find fruit");
                            else kpsign = 1;
                        }
                        else kpsign = 3;
                    }
                    else kpsign = 2; break;
                case 3:
                    if (!this.wakemeupTask.FindObj(fruit))
                    {
                        this.wakemeupTask.speak("point 1");
                        Thread.Sleep(1000);
                        this.wakemeupTask.moveToPlaceByName("kpoint1");
                        if (!this.wakemeupTask.FindObj(fruit))
                        {
                            this.wakemeupTask.speak("point 2");
                            Thread.Sleep(1000);
                            this.wakemeupTask.moveToPlaceByName("kpoint2");
                            if (!this.wakemeupTask.FindObj(fruit))
                                this.wakemeupTask.speak("can not find fruit");
                            else kpsign = 2;
                        }
                        else kpsign = 1;
                    }
                    else kpsign = 3; this.wakemeupTask.GraspObj();
                    this.wakemeupTask.MoveAndPlaceObj(this.wakemeupTask.tray); break;
                default: break;
            }
            
           /* if (this.wakemeupTask.FindObj(cereal))
            {
                this.wakemeupTask.GraspObj();
                
                if (this.wakemeupTask.FindBowl(bowl))
                {
                    //this.wakemeupTask.PourCereal();
                    //this.wakemeupTask.moveToPlace();
                    this.wakemeupTask.ArmPlace();
                    //this.wakemeupTask.adjustMoveDirection(this.wakemeupTask.bowlres.centerCameraPoint);
                    ///this.wakemeupTask.GraspObj();
                    this.wakemeupTask.MoveAndPlaceObj(this.wakemeupTask.tray);
                    return true;
                }
            }*/
            return false;
        }
        public bool DeliverBreakfast()
        {
            
            return true;
        }
    }
}
