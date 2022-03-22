/*这里是个主要的流程。
  例如：“Move to the living room, get the cup and put it on the kitchen 
table.”
   这里包含三个任务 
   1.move to the living room         verb+object
   2.get the cup 
   3.put it on the kitchen table    
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Competitions;

using SITRobotSystem_wpf.BLL.Competitions.GPSR_2017_RiZhao;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Stages;

namespace SITRobotSystem_wpf.BLL.Competitions.GPSR_2017_RiZhao
{
    class GPSR_2017Competition : Competition  // 
    {
        private GPSR_2017Stage gPSR_2017Stage;

        public GPSR_2017Competition()
        {
            gPSR_2017Stage = new GPSR_2017Stage();     // here need to open kinect & navigation
        }

        public override void init()
        {
            this.ThreadNameStr = "GPSR_2017";
            gPSR_2017Stage.init();
        }
        public override void process()              //这里开始你表演
        {
            // gPSR_2017Stage.Start();                 //主要是开始时需不需要移动至某点。这里显然不要。
            //接受指令
            List<Command> commandlist = gPSR_2017Stage.SpeechReconigizeCommand();   // return List<Command> commands , 
            foreach (var command in commandlist)                                  //var means: value type is uncertain; but in means command type is as same as commandlist
            {
                Console.WriteLine(command.action + command.thing.ToString());    //prove
            }

            //处理指令。
            foreach (var command in commandlist)
            {
                gPSR_2017Stage.ProcessCommand2017(command);   
            }
        }//end process
    }
}
