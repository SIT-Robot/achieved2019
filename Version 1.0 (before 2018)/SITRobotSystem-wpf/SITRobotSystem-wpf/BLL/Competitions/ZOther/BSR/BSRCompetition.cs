using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.BSR
{
    class BSRCompetition:Competition
    {
        BSRStage bsrStage;
        public override void init()
        {
            bsrStage=new BSRStage();
            bsrStage.init();
        }

        public override void process()
        {
            bsrStage.isThroughDoor = false ;
            bsrStage.Navigate();
            
        }

        public void runStage1(Quaternion pos)
        {
            
        }
        public void runStage2()
        {
            
        }
        public void runStage3()
        {

        }

        public void setIsThroughDoor(bool isThroughDoor)
        {
            bsrStage.isThroughDoor = isThroughDoor;
        }
    }
}
