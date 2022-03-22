using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.BaseFunction
{
    class BaseFunctionStage:Stage
    {
        private BaseFunctionTask task;

        public BaseFunctionStage()
        {
            task=new BaseFunctionTask();
        }
        public override void init()
        {
            task.initSurfFrmae();
            task.initSpeech();
        }
        /// <summary>
        /// 3. 你说什么?: 
        /// 机器人进入一个有人（TC成员）的房间，
        /// 它应该找到并靠近这个人，宣告它已找到此人，并开始聊天。
        /// 如果无法识别这个人，机器人可以要求此人走到它前方。
        /// 此时，机器人将失去探测人物的分数。机器人要回答预先宣布的general trivia表格里的10个问题中的3个
        /// （如“德国首都是哪儿？”，“世界上最重的动物是什么？”等）。
        /// ，无需且人，机器人应该重复他听到的问题，以及提供正确答案。
        /// 自动移动到位  ---------------------------------------------- 50
        /// 找到此人 --------------------------------------------------- 150
        /// 识别问题与回答 --------------------------------------------- 3X50 
        /// </summary>
        public void BaseQuestion()
        {
            task.speak("start BaseQuestion");
            task.BaseQuestion();
        }

        /// <summary>
        /// 避障 
        /// 避障: 机器人应该移动到赛场中的一个地点。TC成员将在机器人的路径上放置两个障碍。
        /// 第一个障碍是可以避开的(即椅子):
        /// 机器人不应碰到它，而是保持向目标地点移动。第二个障碍是无法避免的(即关闭的门)，
        /// 这种情况下，机器人应该重新规划路线。 
        /// 自动移动到位 ------------------------------------------------ 50
        /// 成功壁障 ---------------------------------------------------- 100
        /// 到达目标点 -------------------------------------------------- 150
        /// </summary>
        public void Nav(string place1,string place2)
        {
            task.speak("start navigation");
            task.moveToPlaceByName(place1);
            task.moveToPlaceByName(place2);
        }

        /// <summary>
        /// 拾取与放置
        /// 1.	拾取与放置: 机器人移动到一个放置着两个物品（一个已知，一个未知）的地点，
        /// 机器人应当捡起一个，并将其放在另一个地点。如果机器人捡起了已知物品，
        /// 它应该将其放在对着放置这个类型的物品的地方；如果是位置物品，它应该把它放进垃圾桶。
        /// 自动移动到位 -------------------------------------------- 50
        /// 拾起物品 ------------------------------------------------ 150
        /// 物品放到正确位置 ---------------------------------------- 150 
        /// </summary>
        public void BaseGetObj(string place1,string obj,string place2)
        {
            task.speak("start get something");
           // task.moveToPlaceByName(place1);
            Goods goods = task.GetGoodsByName(obj);
            task.FaceToGoods(goods);
            task.ArmGet();
            //task.moveToPlaceByName(place2);
            task.ArmPut();
        }
    }
}
