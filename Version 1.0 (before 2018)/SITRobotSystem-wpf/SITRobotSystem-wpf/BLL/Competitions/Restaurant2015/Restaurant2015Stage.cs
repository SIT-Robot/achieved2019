using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.shopping;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using System.Windows.Forms;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2015
{
    class Restaurant2015Stage:Stage
    {
          private Restaurant2015Task task;

          public Place[] place;
          public string[] objects;
          public string[] direction;
          public string[] thing;
          private string[] drink;
          private string[] snack;
          int[] mark;

          int needTable;//收银台给出的桌子索引
          int[] needThing;//顾客给出的需求物品索引

          public Restaurant2015Stage(string[] objectsString, string[] directions, Place[] places, string[] drinks, string[] snacks)
        {
            thing = new string[drinks.Length + snacks.Length];
            int i = 0;
            for (; i < drinks.Length; i++)
            {
                thing[i] = drinks[i];
            }
            for (; i < drinks.Length + snacks.Length; i++)
            {
                thing[i] = snacks[i - drinks.Length];
            }
            this.objects = objectsString;
            this.direction = directions;
            this.place = places;
            this.drink = drinks;
            this.snack = snacks;
            task = new Restaurant2015Task();
        }
        
        public override void init()
        {
            
            task.initSpeech();
            task.initSurfFrmae();
        }


        /// <summary>
        /// shopping 执行任务阶段
        /// </summary>
        public void restaurantCommand()
        {
            int thisStage = -4;
            try
            {
                task.RestaurantRecognize(objects,thing);
                thisStage = -3;
                while (true)
                {
                    if (task.baseSpeech.rCommand != null)
                    {
                        mark = new int[5];
                        for (int i = 0; i < 5; i++)
                        {
                            if (i == 2 || i == 4)
                            {
                                mark[i] = findString(objects, task.baseSpeech.ReIntlRes[i]);
                            }
                            else if (drink.Contains(task.baseSpeech.ReIntlRes[i]))
                            {
                                mark[i] = 0;
                            }
                            else if (snack.Contains(task.baseSpeech.ReIntlRes[i]))
                            {
                                mark[i] = 1;
                            }
                            else
                            {
                                mark[i] = -1;
                            }

                        }
                        thisStage = -2;

                        task.baseSpeech.rCommand = null;
                        thisStage = -1;
                        break;
                    }
                }

               
                try
                {
                    bool ready = true;
                    for (int i = 0; i < mark.Length; i++)
                    {
                        if (mark[i]==-1)
                        {
                            ready = false;
                        }
                    }

                    if (ready==false)
                    {
                        thisStage = 0;
                        for (int i = 0; i < mark.Length; i++)
                        {
                            if (mark[i] != -1)
                            {
                                task.moveToPlace(place[mark[i]]);
                                thisStage += 10;
                            }
                        }
                        return;
                    }

                    task.moveToPlace(place[mark[0]]);
                    thisStage = 1;
                    task.moveToDirection(direction[mark[0]]);
                    Goods g1 = new Goods();
                    g1 = task.GetGoodsByName(task.baseSpeech.ReIntlRes[0]);
                    task.FaceToGoods(g1);
                    task.ArmGet();
                    task.moveToPlace(place[mark[2]]);
                    thisStage ++;
                    task.moveToDirection(direction[mark[2]]);
                    task.ArmPut();

                    task.moveToPlace(place[mark[3]]);
                    thisStage++;
                    task.moveToDirection(direction[mark[1]]);
                    Goods g2 = new Goods();
                    g2 = task.GetGoodsByName(task.baseSpeech.ReIntlRes[1]);
                    task.FaceToGoods(g2);
                    task.ArmGet();
                    task.moveToPlace(place[mark[2]]);
                    thisStage++;
                    task.moveToDirection(direction[mark[2]]);
                    task.ArmPut();

                    task.moveToPlace(place[mark[3]]);
                    thisStage ++;
                    task.moveToDirection(direction[mark[3]]);
                    Goods g3 = new Goods();
                    g3 = task.GetGoodsByName(task.baseSpeech.ReIntlRes[3]);
                    task.FaceToGoods(g3);
                    task.ArmGet();
                    task.moveToPlace(place[mark[4]]);
                    thisStage++;
                    task.moveToDirection(direction[mark[4]]);
                    task.ArmPut();

                    thisStage = 7;
                }
                catch
                {
                    MessageBox.Show("阶段二最终执行状态：" + thisStage);
                    
                }
            }
            catch
            {
                MessageBox.Show("阶段二最终执行状态：" + thisStage);
            }

        }

        private int findString(string[] str1, string str2)
        {
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i]==str2)
                {
                    return i;
                }
            }
            return -1;
        }
        public void moveToPoint1()
        {
            task.moveToPlaceByName("waypoint1");
            task.speak("I have reached waypoint one");
        }

        public void moveToPoint2()
        {
            task.moveToPlaceByName("waypoint2");
            task.speak("I have reached waypoint two");
        }

        public void moveToPoint3()
        {
            task.moveToPlaceByName("waypoint3");
            task.speak("I have reached waypoint three");
        }

        public void Exit()
        {
            task.moveToPlaceByName("exit");
        }


        /// <summary>
        /// 第二阶段
        /// </summary>
        public void stage2()
        {
            //返回收银台
            task.moveToPlace(place[place.Length-1]);
            //面向收银台(转180度 根据赛规须作更改)
            task.moveToDirection(0,0,180);
            //接受语音,获取去哪张桌子
            needTable = task.WhichTable(this.objects);
            //跑到该位置
            task.moveToPlace(place[needTable]);
            //面向桌子
            task.moveToDirection(direction[needTable]);
            //询问所需物品
            needThing = task.WitchNeed(this.thing);
            //依次取物品(可能只是酒水,也可能是酒水和零食)
            for(int i=0;i<needThing.Length;i++)
            {
                //返回收银台
                task.moveToPlace(place[place.Length - 1]);
                //面向收银台(转180度 根据赛规须作更改)
                task.moveToDirection(0, 0, 180);
                //图像检测第一个需要抓取的物品
                Goods g1 = new Goods();
                g1 = task.GetGoodsByName(this.thing[needThing[i]]);
                //调整到适合抓取位置
                task.FaceToGoods(g1);
                //手臂抓取动作
                task.speak("I will catch");
                task.ArmGet();
                //回到顾客所在桌子
                task.speak("On my way");
                task.moveToPlace(place[needTable]);
                //面向桌子
                task.moveToDirection(direction[needTable]);
                //手臂放下动作
                task.speak("Here you are");
                task.ArmPut();

                //全人物流程结束,未实现执行任务过程中的招手叫喊识别
            }

            //任务结束返回收银台
            task.moveToPlace(place[place.Length - 1]);
            //面向收银台(转180度 根据赛规须作更改)
            task.moveToDirection(0, 0, 180);
            task.speak("I have finish my task");
        }
    }
}
