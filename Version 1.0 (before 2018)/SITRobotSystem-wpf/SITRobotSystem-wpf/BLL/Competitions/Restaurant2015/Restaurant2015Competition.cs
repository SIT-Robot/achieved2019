using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.restaurant;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.Restaurant2015
{
    class Restaurant2015Competition:Competition
    {
        private Restaurant2015Stage restaurantStage;
        private Restaurant2015StageFollow restaurantStageFollow;
        Place[] place = new Place[4];
        string[] objects = new string[] { "table A","table B","location C"};

        //顾客点的酒水及混合改这里,数据库的图片记得名字统一 
        string[] drink = { "bottle green", "bottle red", "bottle yellow", "coffee", "ice tea" };
        string[] snack = { "chips" };

        string[] direction = new string[3];
        public Restaurant2015Competition()
        {
            restaurantStageFollow = new Restaurant2015StageFollow(objects);            
            restaurantStage = new Restaurant2015Stage(objects,direction, place,drink,snack);
        }

        public override void init()
        {
            ThreadNameStr = "restaurant";
            restaurantStage.init();
            restaurantStageFollow.init();
            //VisionCtrl.startCoordinateMapping();
            
        }

        public override void process()
        {
            place[place.Length - 1] = restaurantStageFollow.task.GetPlace();
            User followUser = new User();
            Leg leg = new Leg();
            restaurantStageFollow.ensureUser(ref followUser, ref leg);
            restaurantStageFollow.task.speak("I will follow you");
            restaurantStageFollow.followTrackingUser(ref followUser, ref leg);
            
            for (int i = 0; i < restaurantStageFollow.place.Length; i++)
            {
                place[i] = restaurantStageFollow.place[i];
            }

            for (int k = 0; k < restaurantStageFollow.direction.Length; k++)
            {
                direction[k] = restaurantStageFollow.direction[k];
            }

            restaurantStage.place = place;
            restaurantStage.objects = objects;
            restaurantStage.direction = direction;

            /*
             * 未测试代码段
             * 如出现问题 需逐步 转到定义 来debug
             * 新增的stage、task代码会有注释
             */
            restaurantStage.stage2();
        }
    }
}
