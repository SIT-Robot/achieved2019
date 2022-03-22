using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.PersonRecognition
{
    /* 流程
     * 1.开始此项目任务
     * 2.记住Operator和他的名字
     * 3.等待下一步的开始命令
     * 4.找到Crowds，记录Crowds规模（男、女人数）和位置，听到语音命令后，转体180°接近Crowds，然后开始寻找Operator
     * 5.找到Operator，向其致敬，说出其名字、性别、姿势等信息，说出其位置或直接接近其位置
     * 6.描述Crowds的规模，男性人数和女性人数
     */

    /* 分数
     * 接近或指出Operator位置    30
     * 正确说出Operator的性别    30
     * 正确说出Operator的姿势    30
     * 正确说出Crowds的总人数    20
     * 正确说出Crowds的男性数    20
     * 正确说出Crowds的女性数    20
     * 
     * Not attending            -50
     * 杰出表现                  15
     */
    class PersonRecognitionStage:Stage
    {
        public PersonRecognitionTask personrecognitionTask;
        int CrowsCount;
        int CrowsMaleCount;
        int CrowsFemaleCount;
        int OperatorGender;
        int OperatorPose;
        Place OperatorPlace;
        
        
        public PersonRecognitionStage()
        {
            personrecognitionTask = new PersonRecognitionTask();
        }
        public override void init()
        {
           this.personrecognitionTask.initSpeech();
           this.personrecognitionTask.initSurfFrmae();
           this.personrecognitionTask.initBodyDetect();
           //this.personrecognitionTask.visionCtrl.readyVisionHub();
        }
        public bool Start()
        {
            this.personrecognitionTask.Memorize();
            //this.personrecognitionTask.speak("Memorize OK.");
            //Thread.Sleep(1000);
            //用户也说OK

            this.personrecognitionTask.Wait();
            Thread.Sleep(5000);
            /* if (this.personrecognitionTask.turnsign)
             {
                 this.personrecognitionTask.InitCrows();
                 for (int i = 0; i < 20; i++)
             {
                 this.personrecognitionTask.RecSize();
                 Thread.Sleep(1000);

             }
                 this.personrecognitionTask.StateSize();
             }*/
            //int num=this.personrecognitionTask.LegNum();
            int num = this.personrecognitionTask.getfaceNum();
            this.personrecognitionTask.speak("here are "+num+"people");
            //this.personrecognitionTask.StateSize();
            return true;
        }
        public bool MemorizeOperator()
        {
            this.personrecognitionTask.Memorize();
            return true;
        }
        public bool WaitForCmd()
        {

            return true;
        }
        public bool FindCrowds()
        {
            return true;
        }
        public bool FindOperator()
        {
            return true;
        }
        public bool DescribeCrowds()
        {
            this.StateCroSize();
            this.StateCroSize();
            this.StateCroSize();
            return true;
        }
        public bool ApproachOperator(Place Operator)
        {
            this.personrecognitionTask.Approach();
            return true;
        }

        public bool PointOperator(Place Operator)
        {
            
            return true;
        }
        public bool StateOpeGender()
        {
            this.personrecognitionTask.StateGender(OperatorGender);
            return true;
        }
        public bool StateOpePose()
        {
            this.personrecognitionTask.StatePose(OperatorPose);
            return true;
        }
        public bool StateCroSize()
        {
            this.personrecognitionTask.StateSize();
            return true;
        }

    }
}
