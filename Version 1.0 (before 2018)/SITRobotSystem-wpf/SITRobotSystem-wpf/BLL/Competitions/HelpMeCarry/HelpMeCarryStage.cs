using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using Microsoft.Kinect;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.GPSR_2015;
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.BLL.Competitions.HelpMeCarry;
/*
Reach the car 3 × 10
Natural handover to accept the item from the owner 3 × 20
Understand the commanded destination 3 × 5
Reach inside the arena again 3 × 10
Reach the destination 3 × 10
Put the item at the floor or a nearby table 3 × 5
*/
namespace SITRobotSystem_wpf.BLL.Competitions.HelpMeCarry
{
    class HelpMeCarryStage:Stage
    {

        private HelpMeCarryTask helpMeCarryTask;
        
        //public SitRobotSpeech userSpeech;

        public HelpMeCarryStage()
        {
            helpMeCarryTask = new HelpMeCarryTask();
        }
        public override void init()
        {
            helpMeCarryTask.initBodyDetect();
            helpMeCarryTask.initSpeech(); 
        }
        /// <summary>
        /// 跟踪人到达车旁
        /// </summary>
        public Place ReachTheCar()
        {
            helpMeCarryTask.Start();
            helpMeCarryTask.followEasy();
            return helpMeCarryTask.RemeberCar();//得到车的位置
        }
        /// <summary>
        /// 理解要把商品放在什么地方
        /// </summary>
        
        public string UnderstandTheCommandedDestination(string[] goals)
        {
            //helpMeCarryTask.HandoverGroceries();
           
            string commandStrs = "";
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = helpMeCarryTask.CommandDestination(goals);
            }
            
            return commandStrs;

            
        }
        /// <summary>
        /// 送到目的地
        /// </summary>
        public void ReachTheDestination(string goal)
        {
            helpMeCarryTask.moveToPlaceByName(goal);
            Console.WriteLine("到达目的地");
        }
        /// <summary>
        /// 再次进入房子中
        /// </summary>
        public void ReachInsideTheArena()
        {
            helpMeCarryTask.moveToPlaceByName("startpoint");
            Console.WriteLine("回到赛场内");
        }
        

        /// <summary>
        /// 放商品的动作
        /// </summary>
        public void PutTheItemAtTheFloor()
        {
            helpMeCarryTask.ArmPutOnFloor();
            Console.WriteLine("把商品放在地上");
        }
        public void PutTheItemAtTheTble()
        {
            helpMeCarryTask.ArmPutOnTable();
            Console.WriteLine("把商品放在桌上");
        }
        public void AcceptTheItem()
        {
            helpMeCarryTask.HandoverGroceries();
            helpMeCarryTask.ArmGetGrocerie();
            Console.WriteLine("把商品接过来");
        }

        /// <summary>
        /// 再次回到车旁
        /// </summary>
        /// <param name="car"></param>
        public void ReachTheCarAgain(Place car)
        {
            helpMeCarryTask.moveToPlace(car);
            Console.WriteLine("再次达到车旁");
        }

    }
}
