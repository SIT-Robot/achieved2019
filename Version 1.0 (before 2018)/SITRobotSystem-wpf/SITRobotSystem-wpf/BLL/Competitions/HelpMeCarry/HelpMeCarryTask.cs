using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Tasks;
/*
1. Start: The robot is waiting somewhere in the apartment. The operator approaches the
robot and tells it to follow. E.g. “Robot, follow me”
2. Following: The robot starts following the operator, who guides the robot to the car
containing the groceries.
3. Arrive at car: When at the car, the operator tells the robot they have reached the car
and to remember this location. E.g. “Remember: this location is the car”.
4. Handover groceries: The operator gives the robot a command to carry something, e.g.
a box or a bag. E.g. “Carry this box” The robot puts up its arms and the operator gives
the item to the robot.
5. Command destination: The operator tells where the given item should go, one of the
rooms in the arena. E.g. “Bring this to the kitchen”
6. Delivery: The robot then goes inside the house to deliver the item to the destination.
7. Repeat until time is up: Then, the robot goes back to the car (by itself, as it must
remember where the car is) to bring another item inside.*/
namespace SITRobotSystem_wpf.BLL.Competitions.HelpMeCarry
{
    class HelpMeCarryTask:Tasks.Tasks
    {

        
        /// <summary>
        /// 构造函数
        /// </summary>
        public HelpMeCarryTask()
        { }

        public void Start()
        {
            speakReady();
            //此处加语音识别
            //人对机器说的话，可以是多个选择
            string commandStrs;
            string[] commands = { "robot follow me" ,"follow me","eva follow me"};

            baseSpeech = new SitRobotSpeech();
            //baseSpeech.robotSpeak("I am waitting for cleaning table");
            commandStrs = "";
            baseSpeech.HelpMeCarryEasyRecgnization(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
                //识别不到commands内的语句，重新识别
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] != commandStrs && i == commands.Length)
                        commandStrs = "";

                }
            }
            baseSpeech.robotSpeakOwner("OK");
            baseSpeech.speechStop();
            

        }
        public void Following()
        {
            followEasy();
        }
        public Place RemeberCar()
        {
            //语音识别记点
            string commandStrs;
            string[] commands = { "this is the car", "this is car" };

            baseSpeech = new SitRobotSpeech();
            commandStrs = "";
            baseSpeech.HelpMeCarryEasyRecgnization(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
                //识别不到commands内的语句，重新识别
                for(int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] != commandStrs && i == commands.Length)
                        commandStrs = "";

                }
            }
            baseSpeech.robotSpeakOwner("I have remembered");
            baseSpeech.speechStop();
            Place car;
            car = GetPlace();
            System.Console.WriteLine("获得车的点成功");
            return car ;
            
        }
        public void ArmGetGrocerie()
        {
            ArmAction armAction1 = new ArmAction(300,"getgrocerie");
            armCtrl.putThing(armAction1);
        }
        public void ArmPutOnFloor()
        {
            ArmAction armAction1 = new ArmAction(1000, "getgrocerie");
            armCtrl.putThing(armAction1);
        }
        public void ArmPutOnTable()
        {
            ArmAction armAction1 = new ArmAction(301, "getgrocerie");
            armCtrl.putThing(armAction1);
        }
        public void HandoverGroceries()
        {

            //语音语法识别出是否开始伸手
            string commandStrs;
            string[] commands = { "bring this box", "bring this" ,"bring the"};

            baseSpeech = new SitRobotSpeech();
            //baseSpeech.robotSpeak("I am waitting for cleaning table");
            commandStrs = "";
            baseSpeech.HelpMeCarryEasyRecgnization(commands);
            while (string.IsNullOrEmpty(commandStrs))
            {
                commandStrs = baseSpeech.ReturnCommand;
                //识别不到commands内的语句，重新识别
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] != commandStrs && i == commands.Length)
                        commandStrs = "";

                }
            }
            baseSpeech.robotSpeakOwner("OK");
            baseSpeech.speechStop();
            
        }
        public string CommandDestination(string[]  goals)
        {
            //语音识别达到哪个地方
            string res = "";
            baseSpeech.HelpMeCarryRecognition(goals);
            
            while (string.IsNullOrWhiteSpace(res))
            {
                
                res = baseSpeech.ReturnCommand;
                
            }
            
            Console.WriteLine("要到达的房间是" + res);

            //baseSpeech.ReturnCommand = "";
            return res ;
        }
    }
}
