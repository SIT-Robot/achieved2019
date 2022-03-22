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

namespace SITRobotSystem_wpf.BLL.Competitions.restaurant
{
    class restaurantStage : Stage
    {
         private ShoppingTask task;

         public restaurantStage(string[] objectsString, string[] directions, Place[] places, string[] drinks, string[] snacks)
         {
             this.objects = objectsString;
             this.direction = directions;
             this.place = places;
             this.drink = drinks;
             this.snack = snacks;
             string[] things = new string[drink.Length + snack.Length];
             int i = 0;
             for (; i < drink.Length; i++)
             {
                 things[i] = drink[i];
             }
             for (; i < drink.Length + snack.Length; i++)
             {
                 things[i] = snack[i - drink.Length];
             }

             this.thing = things;

             task = new ShoppingTask();
         }

        public Place[] place;
        public string[] objects;
        public string[] direction;
        public string[] thing;
        private string[] drink;
        private string[] snack;
        int[] mark;
        
        public override void init()
        {
            task.StartSurf();
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
    }
}
