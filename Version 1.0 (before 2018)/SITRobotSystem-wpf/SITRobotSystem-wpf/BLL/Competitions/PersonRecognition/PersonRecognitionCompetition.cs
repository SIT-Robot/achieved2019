using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.PersonRecognition
{
    class PersonRecognitionCompetition:Competition
    {
        public PersonRecognitionStage personrecognitionStage;
        public PersonRecognitionCompetition()
        {
            personrecognitionStage = new PersonRecognitionStage();
        }
        public override void init()
        {
            ThreadNameStr = "PersonRecognition";
            personrecognitionStage.init();
        }
        public override void process()
        {
            this.personrecognitionStage.Start();
            
        }
    }
}
