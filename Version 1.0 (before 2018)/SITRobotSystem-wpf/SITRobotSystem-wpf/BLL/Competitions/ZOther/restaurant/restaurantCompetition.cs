using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.restaurant;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.restaurant
{
    class restaurantCompetition : Competition
    {
        private restaurantStage restaurantStage;
        private restaurantStageFollow restaurantStageFollow;
        Place[] place = new Place[6];
        string[] objects = new string[] { "drink","snack","location one","location two","location three","checkout" };
        string[] drink = new string[]{"green tea","flower tea"};
        string[] snack = new string[] { "water","coffee","ice tea" };
        string[] direction = new string[6];
        public restaurantCompetition()
        {
            

            restaurantStageFollow = new restaurantStageFollow(objects);            
            restaurantStage = new restaurantStage(objects,direction, place,drink,snack);
        }

        public override void init()
        {
            ThreadNameStr = "restaurant";
            restaurantStage.init();
            restaurantStageFollow.init();
            
        }

        public override void process()
        {
            place[place.Length-1]=restaurantStageFollow.task.GetPlace();

            User user=restaurantStageFollow.ensureUser();
            restaurantStageFollow.followTrackingUser(user);

            for (int i=0;i<restaurantStageFollow.place.Length;i++)
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

            restaurantStage.restaurantCommand();

        }
    }
}
