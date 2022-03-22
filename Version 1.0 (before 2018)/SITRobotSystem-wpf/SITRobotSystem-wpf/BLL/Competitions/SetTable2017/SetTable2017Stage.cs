using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.Stages;
using SITRobotSystem_wpf.entity;

/*
 *用餐的变化
 *寻问膳食变化与确定选择       10

 *抓物体
 *对于每个成功地抓取一个易于抓握的物体(将其提升到至少5厘米, 10秒以上) 6 × 10
 *对于每个成功的抓取物体的抓取(将其提升到至少5厘米, 10秒以上)         6 × 15

 *放置物体
 *成功放置任何易于抓握的物体在桌上任何位置(安全站立超过10秒)          3 × 10
 *成功放置任何难抓握的物体在桌上任何位置(安全站立超过10秒)            3 × 20
 *正确执行操作员的选择                                                30
 *对于每个对象与桌上另一个物体的碰撞                                  6 × -5

*/

namespace SITRobotSystem_wpf.BLL.Competitions.SetTable2017
{
    class SetTable2017Stage : Stage
    {
        private SetTable2017Task setTable2017Task;
        private Goods obj;
        private Goods choiceObj;
        //物体的名字和放置位置要对应，例如：strThing[2]可以在strObjPlace[2]找到
        private string[] strThing = new[] { "long bottle", "purple bottle", "milk tea"};
        private string[] strObjPlace = new[] { "sofa", "chair", "startpoint"};
        private SurfResult[] surfres;
        List<SurfResult> SearchsurfResultList;
        public bool IsStageReady;

        private int ObjectCount;
        private int NowObjectID;
        private int SearchObjectCount;
        private int UnSearchObjectCount;
        List<int> UnSearchObjectID;
        List<int> SearchObjectID;




        public SetTable2017Stage()
        {
            setTable2017Task = new SetTable2017Task();
            ObjectCount = strThing.Count();
            surfres = new SurfResult[ObjectCount];
            SearchsurfResultList = new List<SurfResult>();

            SearchObjectCount =0;
            UnSearchObjectCount=0;
            UnSearchObjectID = new List<int>();
            SearchObjectID = new List<int>();
        }


        public override void init()
        {
            setTable2017Task.initSpeech();
            setTable2017Task.initSurfFrmae();
        }


        public void SetTable2017StageStart()
        {
            //询问操作员的喜好
            string strChoice = setTable2017Task.AskforMile();
            choiceObj = this.setTable2017Task.GetGoodsByName(strChoice);
            setTable2017Task.moveToPlaceByName("table");
            //寻找桌子上已存在的物品
            for (int i = 0; i < ObjectCount; i++)
            {
                obj = this.setTable2017Task.GetGoodsByName(strThing[i]);
                //surfres[i] = setTable2017Task.LookForGood2(obj,setTable2017Task.rect);
                surfres[i] = setTable2017Task.LookForGood(obj);
                if (surfres[i].isSuccess)
                {
                    SearchObjectCount++;
                    SearchObjectID.Add(i);
                    SearchsurfResultList.Add(surfres[i]);
                }
                else
                {
                    UnSearchObjectCount++;
                    UnSearchObjectID.Add(i);
                }
            }
            //到指定地点取缺失的物品
            for (int i = 0; i < UnSearchObjectCount; i++)
            {
                NowObjectID = UnSearchObjectID[i];
                SearchObjectCount++;
                obj = this.setTable2017Task.GetGoodsByName(strThing[NowObjectID]);
                setTable2017Task.SpeakFindObject(strThing[NowObjectID]);
                setTable2017Task.moveToPlaceByName(strObjPlace[NowObjectID]);
                //如果找到则返回餐桌放好
                if (setTable2017Task.FaceToGoods(obj))
                {
                    SearchObjectID.Add(NowObjectID);
                    setTable2017Task.ArmGet();
                    setTable2017Task.moveToPlaceByName("table");
                    //选择合适的放置位置，还没写
                    setTable2017Task.moveToAdjust(SearchsurfResultList, SearchObjectID, NowObjectID);
                    setTable2017Task.ArmPlace(101);
                }

            }


            //收拾桌子阶段
            setTable2017Task.moveToPlaceByName("table");
            setTable2017Task.waitForClean();
            for (int i = 0; i < SearchObjectCount; i++)
            {
                NowObjectID = SearchObjectID[i];
                obj = this.setTable2017Task.GetGoodsByName(strThing[NowObjectID]);
                if (setTable2017Task.FaceToGoods(obj))
                {
                    setTable2017Task.ArmGet();
                    setTable2017Task.moveToPlaceByName(strObjPlace[NowObjectID]);
                    setTable2017Task.ArmPlace();
                    setTable2017Task.moveToPlaceByName("table");
                }
            }

        }
    }
}
