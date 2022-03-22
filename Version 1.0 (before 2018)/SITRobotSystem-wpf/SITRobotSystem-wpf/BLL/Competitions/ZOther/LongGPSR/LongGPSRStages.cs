using System.Collections.Generic;
using System.Threading;
using SITRobotSystem_wpf.BLL.Competitions.GPSR;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

/*
 本项目尤为注重以下方面: 

• 执行非预定义顺序的动作(摆脱机械的状态规划)。 

• 语音识别方面提升的复杂度。 

• 环境识别

• 机器人长时间运行 


6.2.2 任务
 
1. 入场并获取命令: 机器人在舞台上一个设定好的位置启动，并在此区域等待更多的命令。

2. 确定命令: 对机器人的命令将随机产生, 内容由该队伍从命令类别（如下）中选择。 

3. 命令类别: 该队伍可以从如下三个命令类别中选择一个。
3.1. 类别I : 该命令由三个动作组成，机器人必须表示它已经识别了该命令。机器人可以重读它理解出的命令，并请求确认。如果它不能正确的识别命令，可以要求讲述着完整的重复命令。
3.2. 类别II : 机器人将得到一个并不完全包含所有必须信息的命令。 命令像以下这样：
    • 只给出物品的类型（给我一瓶饮料），或者地点（去桌子旁边），而不是确实的物品或地点。
    • 或者不给出任务地点（或物品类型）。
        机器人可以询问关于任务的遗落的信息，但并不是必须如此。
    机器人提出的问题必须明确指出它已经获得了什么信息，例如，告诉操作者它知道要拿来某个特定的饮料罐，但是不知道罐子的位置在哪里。机器人也可以简单的开始搜索。
3.3. 类别III: 命令包含错误的信息。机器人在执行这项任务时，应该能够意识到这样的错误。
 * 回到操作者身边，并清楚的陈述它为什么不能完成此任务。如果机器人能够用替代品，合理的方案来解决这个问题，将获得额外的加分（详见计分表）。

4. 指派任务: 操作者给机器人发出命令，机器人可以直接开始执行被指派的任务。

5. 执行任务: 机器人必须在5分钟内停止执行任务并回到指定地点。超时的话，机器人必须立即移动到指定位置。如果该队伍有重新开始的机会，机器人可以在指定位置重新开始。

6. 返回: 在完成任务之后，机器人必须返回指定位置等待，并检索下一个命令，（既，回到1.，而不需要重新进入赛场。机器人可以执行最多三个命令。

7. 定时: 给机器人分配的指令检索和执行任务的总时间为10分钟。如果机器人在超时后不在指定的位置，它必须马上移动到该位置。

Action 

Command Category I 
正确执行第一个任务 200
正确执行第一、二个任务 200
成功完成全部任务并返回指定位置 200

Command Category II 
询问有道理的问题来获得丢失的信息 300
解决一半以上的问题 (表现出机器人已经理解了命令，并且执行了任务) 200 
完整地解决问题并返回指定位置 400

Command Category III 
执行任务直到错误发生的步骤 300
在执行任务的时候宣布发生了错误 200
返回指定位置，并解释出了什么问题 300
给出一个对该问题的替代方案 200
 
 */

namespace SITRobotSystem_wpf.BLL.Competitions.LongGPSR
{
    class LongGPSRStages:GpsrStages
    {
        private GpsrTask longGPSRTask;
        public LongGPSRStages()
        {
            longGPSRTask = new LongGPSRTask();
        }

        /// <summary>
        /// 初始化开启窗口服务等
        /// </summary>
        public override void init()
        {
            longGPSRTask.initSurfFrmae();
            longGPSRTask.initSpeech();
            longGPSRTask.initBodyDetect();
        }

        /// <summary>
        /// 自主进场
        /// </summary>
        public override void ComeIn()
        {
            if (true)
            {
                longGPSRTask.moveToPlaceByName("sofa");
            }
        }

        
        ///  类别I : 该命令由三个动作组成，机器人必须表示它已经识别了该命令。
        /// 机器人可以重读它理解出的命令，并请求确认。如果它不能正确的识别命令，可以要求讲述着完整的重复命令。
        /// SpeechReconigizeCommand();
        /// 正确执行第一个任务--------------------------------------------------200
        /// 正确执行第一、二个任务 ---------------------------------------------------------200
        ///  ProcessCommand(Command command);

        /// <summary>
        /// Command Category I :成功完成任务并返回指定位置------------------------------------------------200
        /// Command Category II :完整地解决问题并返回指定位置---------------------------------------------400
        /// </summary>
        public void BackToGoal()
        {
            longGPSRTask.moveToPlaceByName("sofa");
        }


//        /// <summary>
//        /// 正确理解命令(机器人完整复述相应的任务) …………………………200 分
//        /// </summary>
//        public List<Command>  SpeechReconigizeCommand()
//        {
//            longGPSRTask.speakReady();
//            string commandStrs="";
//            while (string.IsNullOrEmpty(commandStrs))
//            {
//                commandStrs = longGPSRTask.speechGetCommand();
//            }
//            List<Command> commands = longGPSRTask.commandTranslate(commandStrs);
//            return commands;
//        }
//
//        /// <summary>
//        /// 完成执行任务
//        /// </summary>
//        /// <param name="command"></param>
//        public void ProcessCommand(Command command)
//        {
//            switch (command.action)
//            {
//                case ActionType.put:
//                    Thread.Sleep(1000);
//                    longGPSRTask.ArmPut();
//                    break;
//
//                case ActionType.move:
//                    longGPSRTask.moveToPlaceByName(command.thing.Name);
//                    break;
//
//                case ActionType.get:
//                    Goods g = new Goods();
//                    g = longGPSRTask.GetGoodsByName(g.Name);
//                    longGPSRTask.FaceToGoods(g);
//                    Thread.Sleep(1000);
//                    longGPSRTask.ArmGet();
//                    break;
//
//                default:
//                    break;
//
//            }
//        }
    }
}
