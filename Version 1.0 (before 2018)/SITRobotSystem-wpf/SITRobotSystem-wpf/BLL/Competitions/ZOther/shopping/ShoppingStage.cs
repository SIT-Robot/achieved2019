using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SITRobotSystem_wpf.BLL.Competitions.Follow;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.shopping
{
    class ShoppingStage:Stage
    {
        public ShoppingTask task;
        public BaseCtrl baseCtrl = new BaseCtrl();

        public ShoppingStage(string[] objects, string[] direction, Place[] place)
        {
            this.objects = objects;
            this.direction = direction;
            this.place = place;
            task = new ShoppingTask();
        }

        public Place[] place;
        public string[] objects;
        public string[] direction;
        int[] mark;
        string[] Thing = new[] { "bottle yellow", "bottle blue", "bottle red", "drinks", "bag", "gum", "paper", "crackers", "sauce", "water" };
        public override void init()
        {
            task.initSurfFrmae();
        }


        /// <summary>
        /// shopping 执行任务阶段
        /// </summary>
        public void ShoppingCommand(string[] thing)
        {
            int thisStage = 0;
            try
            {
                task.speechCommand(thing);
                thisStage = 1;
                while (true)
                {
                    if (task.baseSpeech.rCommand != null)
                    {
                        mark = new int[3];
                        mark[0] = task.baseSpeech.rCommand[0];
                        mark[1] = task.baseSpeech.rCommand[1];
                        mark[2] = task.baseSpeech.rCommand[2];
                        

                        task.baseSpeech.rCommand = null;
                        thisStage = 2;
                        break;
                    }
                }

                for (int i = 0; i < mark.Length; i++)
                {
                    try
                    {
                        Goods g = new Goods();
                        Goods[] goods = new Goods[3];
                        List<Goods> goodslist = new List<Goods>();
                        goodslist.Add(task.GetGoodsByName(Thing[mark[0]]));
                        goodslist.Add(task.GetGoodsByName(Thing[mark[1]]));
                        goodslist.Add(task.GetGoodsByName(Thing[mark[2]]));
                        //task.moveToPlace(place[mark[i]]);
                        //thisStage = 3;
                        //if (i % 2 == 0)
                        //{
                        //    task.moveToDirection(direction[mark[i]]);
                        //    thisStage = 4;

                        //    Goods g = new Goods();
                        //    g = task.GetGoodsByName(g.Name);
                        //    thisStage = 5;

                        //    task.FaceToGoods(g);
                        //    thisStage = 6;

                        //    task.ArmGet();
                        //    thisStage = 7;
                        //}
                        //else
                        //{
                        //    task.ArmPut();
                        //    thisStage = 8;
                        //}
                        task.moveToPlace(place[3]);
                        task.moveToDirection(direction[3]);
                        task.baseSpeech = new SitRobotSpeech();
                        task.baseSpeech.robotSpeak("I having arrived the table");
                        Thread.Sleep(2000);
                        //g = task.GetGoodsByName(Thing[mark[0]]);
                        foreach (var good in goodslist)
                        {
                            task.FaceToGoods(good);
                            if (task.FaceToGoods(good))
                            {
                                task.armCtrl.send(new ArmAction(500, "get"));
                                task.baseSpeech.robotSpeak("I got it");
                                baseCtrl.moveToDirectionSpeed(-0.10f, 0);
                                task.moveToPlace(place[4]);
                                task.baseSpeech.robotSpeak( "here you are");
                                task.armCtrl.send(new ArmAction(502, "give"));
                                break;
                            }
                             
                        }
                        

                        task.moveToPlace(place[2]);
                        task.moveToDirection(direction[2]);
                        task.baseSpeech = new SitRobotSpeech();
                        task.baseSpeech.robotSpeak("I having arrived the table");
                        Thread.Sleep(2000);
                        foreach (var good in goodslist)
                        {
                            task.FaceToGoods(good);
                            if (task.FaceToGoods(good))
                            {
                                task.armCtrl.send(new ArmAction(500, "get"));
                                task.baseSpeech.robotSpeak("I got it");
                                baseCtrl.moveToDirectionSpeed(-0.10f, 0);
                                task.moveToPlace(place[4]);
                                task.baseSpeech.robotSpeak("here you are");
                                task.armCtrl.send(new ArmAction(502, "gieve"));
                                break;
                            }

                        }

                        task.moveToPlace(place[1]);
                        task.moveToDirection(direction[1]);
                        task.baseSpeech.robotSpeak("I having arrived the shelf");
                        Thread.Sleep(1000);
                        foreach (var good in goodslist)
                        {
                            task.FaceToGoods(good);
                            if (task.FaceToGoods(good))
                            {
                                task.armCtrl.send(new ArmAction(500, "get"));
                                task.baseSpeech.robotSpeak("I got it");
                                baseCtrl.moveToDirectionSpeed(-0.05f, 0);
                                task.moveToPlace(place[4]);
                                task.baseSpeech.robotSpeak("here you are");
                                task.armCtrl.send(new ArmAction(502, "ss"));
                                break;
                            }

                        }

                        task.moveToPlace(place[0]);
                        task.moveToDirection(direction[0]);
                        task.baseSpeech.robotSpeak("I having arrived the shelf");
                        Thread.Sleep(1000);
                        foreach (var good in goodslist)
                        {
                            task.FaceToGoods(good);
                            if (task.FaceToGoods(good))
                            {
                                task.armCtrl.send(new ArmAction(500, "get"));
                                task.baseSpeech.robotSpeak("I got it");
                                baseCtrl.moveToDirectionSpeed(-0.05f, 0);
                                task.moveToPlace(place[4]);
                                task.baseSpeech.robotSpeak("here you are");
                                task.armCtrl.send(new ArmAction(502, "ss"));
                                break;
                            }

                        }
                        //Goods g = new Goods();
                        //g = task.GetGoodsByName(Thing[mark[0]]);
                        //task.FaceToGoods(g);
                        //task.armCtrl.send(new ArmAction(500,"ss"));
                        //task.baseSpeech.robotSpeak("I got it");

                        //task.moveToPlace(place[2]);
                        //task.baseSpeech.robotSpeak("this is the "+Thing[mark[0]]+"here you are");

                    }
                    catch
                    {
                        MessageBox.Show("阶段二最终执行状态：" + thisStage);
                        continue;
                    }
                }

                task.baseSpeech = new SitRobotSpeech();
                task.speechCommand(objects);
                thisStage = 9;

                while (true)
                {
                    if (task.baseSpeech.ReturnCommand != null)
                    {
                        task.moveToPlace(place[place.Length - 1]);
                        thisStage = 10;
                    }
                }

            }
            catch
            {
                MessageBox.Show("阶段二最终执行状态：" + thisStage);
            }

        }
    }
}
