/* 机器人自我介绍  
 * 机器人识别客人并走向另一个
 * 记作客人和客人的要求
 * 机器人必须说出它所理解的名字和物品
 * 导航到指定位置
 * 导航到房间找到相应客人
 * 离开场地
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Competitions.GPSR_2015;
//add
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Competitions.WhoisWho;
using System.Threading;

namespace SITRobotSystem_wpf.BLL.Competitions.WhoisWho
{
    class WhoisWhoCompetition : Competition
    {
        public WhoisWhoStage WhoisWhoStage;
        public WhoisWhoTask whoiswhoTask;
        System.DateTime currentTime = new System.DateTime();
        public WhoisWhoCompetition()
        {
            WhoisWhoStage = new WhoisWhoStage();
            whoiswhoTask = new WhoisWhoTask();
        }
        public override void init()
        {
            ThreadNameStr = "WhoIsWho";
            WhoisWhoStage.init();
        }

        public override void process()
        {
            // DateTime start = DateTime.Now;
            // DateTime end = DateTime.Now;
            //TimeSpan delta = end - start;
            //System.Console.WriteLine("Time:" + delta);
            //WhoisWhoStage.ComeIn();

            // List<Command> commands= WhoisWhoStage.rememberPeople();
            // WhoisWhoStage.moveCatch();
            Thread.Sleep(5000);

            try
            {
              // WhoisWhoStage.moveLiving();
               WhoisWhoStage.moveToPeople();      //走向客人                                            
            }
            catch (Exception e)
            {
                WhoisWhoStage.speakError(e);
            }
            try
            {
                WhoisWhoStage.RememberPeople();    //客人先机器人介绍自己  里面循环要根据需求该
            }
            catch (Exception e)
            {
                WhoisWhoStage.speakError(e);
            }
            try
            {
                WhoisWhoStage.moveCatch();
            }        //移动 物品放置的位置 需要指定修改 到这里拿东西 
            catch
            { whoiswhoTask.getout(); }



            //-------------来回拿东西----------------------
            try
            {
                for (int i = 0; i < 5; i++)  //来回拿5次东西
                {
                    try
                    {

                    }
                    catch (Exception e)
                    {
                        WhoisWhoStage.speakError(e);
                    }
                    try
                    {
                        WhoisWhoStage.getstuff(i); //拿东西模块 // tasks.cs facetogood  拿5次                                                
                    }
                    catch (Exception e)
                    {
                        WhoisWhoStage.speakError(e);
                    }
                    try
                    {
                        // whoiswhoTask.moveToPlace(whoiswhoTask.p[i]);  //定点到人的位置   到这里来找人                                        
                    }
                    catch (Exception e)
                    {
                        WhoisWhoStage.speakError(e);
                    }
                    try
                    {
                      //  WhoisWhoStage.findOnePeople(i);     //找人 保存照片
                    }
                    catch (Exception e)
                    {
                        WhoisWhoStage.getout();            //定点出门里面要取消注释
                    }
                }//end for
                WhoisWhoStage.moveLiving();
                WhoisWhoStage.speakFindningpeople();
            }
            catch//catch for loop
            {
                WhoisWhoStage.getout();
            }
        }//end process method
    }
}

