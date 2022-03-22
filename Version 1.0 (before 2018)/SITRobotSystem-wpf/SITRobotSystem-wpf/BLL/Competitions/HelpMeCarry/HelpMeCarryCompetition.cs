using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SITRobotSystem_wpf.BLL.Competitions;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.HelpMeCarry
{
    class HelpMeCarryCompetition:Competition
    {
        private bool KeepOn=true ;//判断是否停止此项目
        private string[] strRoomName = { "sofa", "chair", "table", "startpoint" };
        private string Destination;//目标房间名称
        private Place car;//车的地点
        private HelpMeCarryStage helpMeCarryStage;
        public HelpMeCarryCompetition()
        {
            helpMeCarryStage = new HelpMeCarryStage();
        }
        public override void init()
        {
            this.ThreadNameStr = "HelpMeCarry";
            helpMeCarryStage.init();
        }
        public override void process()
        {
            car = helpMeCarryStage.ReachTheCar();
            Thread.Sleep(5000);
            while (KeepOn)
            {
                helpMeCarryStage.AcceptTheItem();
                //Thread.Sleep(5000);
                Destination = helpMeCarryStage.UnderstandTheCommandedDestination(strRoomName);
                //Thread.Sleep(5000);
                //helpMeCarryStage.ReachInsideTheArena();
                helpMeCarryStage.ReachTheDestination(Destination);
                helpMeCarryStage.PutTheItemAtTheFloor();
                //helpMeCarryStage.PutTheItemAtTheTble();
                helpMeCarryStage.ReachTheCarAgain(car);
            }
        }
    }
}
