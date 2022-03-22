using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2017
{

    //推断bar的哪一边5

    //调用相
    //注意到呼叫                                  2*10

    //有序相
    //到达呼叫者的桌子                            2*5
    //看着呼叫者                                  2*10
    //接受命令                                    2*10
    //在机器人经过的路径上躲开一个人              2*10

    //采集阶段
    //背诵餐桌的订单                              2*5
    //抓住正确的饮料                              2*15
    //端起盘子                                    2*20

    //交货期
    //按要求接近正确的桌子                        2*10
    //把饮料放在正确的桌子上                      2*15
    //把饮料方便的递给顾客                        2* 5
    //把盘子放在正确的桌子上                      2*20
    //把盘子方便的递给顾客                        2*5

    //处罚和奖励的标准
    //根据得分的数据得分和/总分数(see sec. 3.4)     10
    //没有到场(see sec. 3.9.1)                      -50
    //杰出表现(see sec. 3.9.3)                       28 

    class Restaurant2017Stage : Stage
    {
        private string[] strObjPlace = new[] { "table A", "table B", "table C" };
        Place startPoint;
        Place tableA, tableB;
        List<User> usersList;
        User userCoustomer;

        private string order;
        private List<string> listorder;
        private List<string> robot;

        Restaurant2017Task restaurant2017Task;

        public Restaurant2017Stage()
        {
            userCoustomer = new User();
            restaurant2017Task = new Restaurant2017Task();
        }

        public override void init()
        {
            //restaurant2017Task.initSpeech();
            //restaurant2017Task.initSurfFrmae();
            //restaurant2017Task.initBodyDetect();
        }

        public void Restaurant2017StageStart()
        {
            startPoint = restaurant2017Task.GetPlace();
            do
            {
                restaurant2017Task.waitForAsk();
                userCoustomer = restaurant2017Task.GetAskingCoustomer();
                restaurant2017Task.waitForOrder();
                restaurant2017Task.moveToPlaceByUser(userCoustomer);
                tableA = restaurant2017Task.GetPlace();
                tableA.PCategory = strObjPlace[0];
                //restaurant2017Task.GetCloseToUser(userCoustomer);
                order = restaurant2017Task.AskForOrder();
                restaurant2017Task.moveToPlace(startPoint);
                restaurant2017Task.RepeatOrder(order, tableA.PCategory);
                restaurant2017Task.GetObject();
                restaurant2017Task.moveToPlace(tableA);
                restaurant2017Task.PlaceObject();
                restaurant2017Task.moveToPlace(startPoint);
                order = restaurant2017Task.waitForOrder(false);
            } while (!restaurant2017Task.JudgeIsEnd(order));
        }

    }
}
