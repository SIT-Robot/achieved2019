using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Emgu.CV.Util;
using iTextSharp.text;
using Microsoft.Kinect;
using Org.BouncyCastle.Bcpg;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.BLL.State;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.Service;
using System.IO;
using SITRobotSystem_wpf.SITRobotWindow.testWindows;
using SITRobotSystem_wpf.BLL.Competitions.FuckingSurprise;

namespace SITRobotSystem_wpf.BLL.Tasks
{
    /// <summary>
    /// task中为常用任务。
    /// </summary>
    public class Tasks
    {
        public BaseCtrl baseCtrl = new BaseCtrl();
        public VisionCtrl visionCtrl = new VisionCtrl();
        public ArmCtrl armCtrl = new ArmCtrl();
        public SitRobotSpeech baseSpeech = new SitRobotSpeech();

        public void setSurfDistance(float dis)
        {
            visionCtrl.setSurfDistance(dis);
        }

        public virtual String FindName(String scrPath)
        {
            String name = visionCtrl.FindFaceName(scrPath);
            return name;
        }



        /// <summary>
        /// 进行多次人脸识别(因为识别不太准,所以识别多次取最大值)
        /// </summary>
        public virtual SurfResult[] CountFace()
        {
            int i = 0;
            SurfResult[] result;
            do
            {
                i++;
                string src = getCorrentImg();
                result = visionCtrl.FindFace(src);
            } while (!result[0].isSuccess && i < 5);
            return result;
        }

        /// <summary>
        /// 获得某样物体
        /// </summary>
        /// <param name="obj">物体</param>
        /// <returns>是否获得</returns>
        public virtual bool FaceToGoods(Goods obj)
        {
            setSurfDistance(0.99f);
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            PDFMaker myMaker;
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 3; i++)
            {
                //过滤掉有毒的第一张
                //if (i == 0)
                //    visionCtrl.findObject(obj);
                speak("I am finding" + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
                res = result.isReachable;
                if (result.isReachable && result.isSuccess)
                {
                    speak("I found " + obj.Name);
                    myMaker = new PDFMaker(result.ImgPath, obj.Name);
                    myMaker.Maker();
                    VisionCtrl.endCoordinateMapping();
                    break;
                }
            }
            Place moveDirection;

            if (res)
            {
                speak("let me get " + obj.Name);
                adjustMoveDirection(result.centerCameraPoint);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
                speak("sorry I can't reach " + obj.Name);
            }
            return res;
        }


        /// <summary>
        /// 获得某样物体
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="rect">物体分割处理范围</param>
        /// <param name="IsDefalutRect">是否采用默认分割范围</param>
        /// <param name="count">物体分割处理次数，次数越多效果越好，用时越长</param>
        /// <returns>是否获得</returns>
        /// 


        public virtual List<string> FindNames()
        {
            SurfResult[] faces = null;
            int maxNum = 0;
            string name;
            faces = CountFace();
            SurfResult[] facesResult;
            List<string> names = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                faces = this.CountFace();
                if (faces.Count() > maxNum)
                {
                    maxNum = faces.Count();
                    facesResult = faces;
                }
            }
            foreach (SurfResult face in faces)
            {
                names.Add(visionCtrl.FindFaceName(face.ImgPath));
                VisionCtrl vc = new VisionCtrl();
                vc.ChangePath(face.ImgPath, "C:\\" + visionCtrl.FindFaceName(face.ImgPath) + ".jpg");
            }
            return names;
        }



        public virtual void openKinect(string name)
        {
            bool result = false;
            string path = null;
            string src = getCorrentImg(); //得到kinect的截图
            path = visionCtrl.SaveTrainedFace(src, name);//找到人脸并把src里名为name的图片保存到路径:~/trainedFaces 
            int i = 0;
            while (string.IsNullOrEmpty(path) || i < 3 && i < 7)//确定path里面有图片
            {
                src = getCorrentImg();
                path = visionCtrl.SaveTrainedFace(src, name);
                i++;
            }
        }
        public virtual bool FaceToGoods2(Goods obj, System.Drawing.Rectangle rect, int count = 2)
        {
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            PDFMaker myMaker;
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 2; i++)
            {
                speak("I am finding" + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                //每次查找失败，迭代次数加一次
                result = visionCtrl.findObject2(obj, rect, count + i);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
                res = result.isReachable;
                if (result.isReachable && result.isSuccess)
                {
                    speak("I found " + obj.Name);
                    myMaker = new PDFMaker(result.ImgPath, obj.Name);
                    myMaker.Maker();
                    break;
                }
            }
            VisionCtrl.endCoordinateMapping();
            if (res)
            {
                speak("let me get " + obj.Name);
                adjustMoveDirection(result.centerCameraPoint);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
                speak("sorry I can't reach " + obj.Name);
            }
            return res;
        }
        public virtual bool FaceToGoods2(Goods obj, int count = 2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(300, 500, 1300, 400);
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            PDFMaker myMaker;
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 2; i++)
            {
                speak("I am finding" + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                //每次查找失败，迭代次数加一次
                result = visionCtrl.findObject2(obj, rect, count + i);
                Console.WriteLine("final find:  " + result.resultStr);
                result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
                res = result.isReachable;
                if (result.isReachable && result.isSuccess)
                {
                    speak("I found " + obj.Name);
                    myMaker = new PDFMaker(result.ImgPath, obj.Name);
                    myMaker.Maker();
                    break;
                }
            }
            VisionCtrl.endCoordinateMapping();
            if (res)
            {
                speak("let me get " + obj.Name);
                adjustMoveDirection(result.centerCameraPoint);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
                speak("sorry I can't reach " + obj.Name);
            }
            return res;
        }

        public bool lookToGoodsMachineLearing(string obj, MachineLearningTestWindow mlt)
        {
            MutipleFrame mutipleFrame = mlt.mutipleFrame;

            MachineLearingResult machineLearingResult = null;
            int maxConfidenceIndex = 0;//可信度最大的下标
            bool res = false;
            //ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            CameraSpacePoint cameraSpacePoint = new CameraSpacePoint();
            //SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            mutipleFrame.coordinateMappingFlag = true;
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 1; i++)
            {

                VisionCtrl.screenShotForMachineLearning();
                //speak("I am finding" + obj);

                try
                {
                    machineLearingResult = visionCtrl.FindObjectMachineLearing(obj);
                    if (machineLearingResult.ThingsNumber != 0)
                    {
                        //VisionCtrl.endCoordinateMapping();
                        mutipleFrame.coordinateMappingFlag = false;
                        //找到可信度最大的物体
                        float maxConfidence = 0;
                        for (int j = 0; j < machineLearingResult.ThingsNumber; j++)
                        {
                            if (machineLearingResult.Confidence[j] >= maxConfidence)
                            {
                                maxConfidence = machineLearingResult.Confidence[j];
                                maxConfidenceIndex = j;
                            }

                        }
                        res = true;
                        break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    res = false;
                }

                Console.WriteLine("searching for " + 0 + "times");


                //res = result.isReachable;
                //if (result.isReachable && result.isSuccess)
                //{
                //    speak("I found " + obj);
                //    VisionCtrl.endCoordinateMapping();
                //    break;
                //}
            }


            Place moveDirection;

            if (res)
            {
                speak("let me get " + obj);
                adjustMoveDirection(machineLearingResult.CameraSpacePoint[maxConfidenceIndex]);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
                //speak("sorry I can't reach " + obj);
            }
            return res;
        }
        public virtual bool TrainFace(string name)
        {
            bool result = false;
            string path = null;
            string src = getCorrentImg(); //得到kinect的截图
            path =@"trainedFaces\\"+ "tom_33aeed94-58b0-432d-93e3-9f3da94234af.png";// visionCtrl.SaveTrainedFace(src, name);//找到人脸并把src里名为name的图片保存到路径:~/trainedFaces 
            int i = 0;
            //while (string.IsNullOrEmpty(path))//确定path里面有图片
            //{
               // src = getCorrentImg();
                //path = visionCtrl.SaveTrainedFace(src, name);
               // i++;
            //}
            if (!string.IsNullOrEmpty(path))//如果有图片
            {
                //this.speak("I have remembered you");  //ysh has changed it below
                this.speak("I have remembered");
                result = true;
            }
            else
                this.speak("I can't remember you");
            return result;
        }
        public bool lookToGoodsMachineLearing(string obj)
        {

            MachineLearingResult machineLearingResult = null;
            int maxConfidenceIndex = 0;//可信度最大的下标
            bool res = false;
            //ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
 
            //SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();

            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 1; i++)
            {

                VisionCtrl.screenShotForMachineLearning();
                //speak("I am finding" + obj);

                try
                {
                    machineLearingResult = visionCtrl.FindObjectMachineLearing(obj);
                    if (machineLearingResult.ThingsNumber != 0)
                    {
                        //VisionCtrl.endCoordinateMapping();

                        //找到可信度最大的物体
                        float maxConfidence = 0;
                        for (int j = 0; j < machineLearingResult.ThingsNumber; j++)
                        {
                            if (machineLearingResult.Confidence[j] >= maxConfidence)
                            {
                                maxConfidence = machineLearingResult.Confidence[j];
                                maxConfidenceIndex = j;
                            }

                        }
                        res = true;
                        break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    res = false;
                }

                finally
                {
                    VisionCtrl.endCoordinateMapping();
                }

                Console.WriteLine("searching for " + 0 + "times");


                //res = result.isReachable;
                //if (result.isReachable && result.isSuccess)
                //{
                //    speak("I found " + obj);
                //    VisionCtrl.endCoordinateMapping();
                //    break;
                //}
            }


            Place moveDirection;

            if (res)
            {
                speak("let me get " + obj);
                FuckingSurpriseCompetition.targetCSP = machineLearingResult.CameraSpacePoint[maxConfidenceIndex];
                //adjustMoveDirection(machineLearingResult.CameraSpacePoint[maxConfidenceIndex]);
                //res = baseCtrl.moveToDirectionSpeed(moveDirection);
            }
            else
            {
              //  speak("sorry I can't reach " + obj);
            }
            return res;
        }


        /// <summary>
        /// 找一个东西返回是否找到
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool LookForGoods(Goods obj)
        {
            setSurfDistance(0.80f);
            bool res = false;
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 3; i++)
            {
                speak("I am finding" + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                res = result.isReachable = ifObjCameraPositionLegal(result.centerCameraPoint);
                if (result.isReachable)
                {
                    speak("I found " + obj.Name);
                    VisionCtrl.endCoordinateMapping();
                    break;
                }
            }
            return result.isReachable;
        }
        /// <summary>
        /// 找一个东西返回匹配结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SurfResult LookForGood(Goods obj)
        {
            PDFMaker myMaker;
            setSurfDistance(0.80f);
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 3; i++)
            {
                speak("I am finding " + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                result = visionCtrl.findObject(obj);
                Console.WriteLine("final find:  " + result.resultStr);
                if (result.isSuccess)
                {
                    speak("I found " + obj.Name);
                    myMaker = new PDFMaker(result.ImgPath, result.name);
                    myMaker.Maker();
                    VisionCtrl.endCoordinateMapping();
                    break;
                }
            }
            if (!result.isSuccess)
                speak("I don't find " + obj.Name);
            return result;
        }

        /// <summary>
        /// 找一个东西返回匹配结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SurfResult LookForGood2(Goods obj, System.Drawing.Rectangle rect, int count = 1)
        {
            PDFMaker myMaker;
            setSurfDistance(0.80f);
            ObjCameraPosition objCameraPosition = new ObjCameraPosition(0, 0, 0);
            SurfResult result = new SurfResult();
            VisionCtrl.startCoordinateMapping();
            //由于匹配算法不精确，匹配i次直到成功
            for (int i = 0; i < 3; i++)
            {
                speak("I am finding " + obj.Name);
                Console.WriteLine("searching for " + 0 + "times");
                result = visionCtrl.findObject2(obj, rect, 1);
                Console.WriteLine("final find:  " + result.resultStr);
                if (result.isSuccess)
                {
                    speak("I found " + obj.Name);
                    myMaker = new PDFMaker(result.ImgPath, result.name);
                    myMaker.Maker();
                    VisionCtrl.endCoordinateMapping();
                    break;
                }
            }
            if (!result.isSuccess)
                speak("I don't find " + obj.Name);
            return result;
        }

        /// <summary>
        /// 自定义手臂动作
        /// </summary>
        public virtual void Arm(int id)
        {
            ArmAction armAction0 = new ArmAction(id, "get");
            armCtrl.getThing(armAction0);
            baseCtrl.moveToDirectionSpeed(-0.05f, 0);
        }


        /// <summary>
        /// GPSR物体抓取
        /// </summary>
        public virtual void ArmGet()
        {
            //ArmAction armAction0 = new ArmAction(14, "get");
            //armCtrl.getThing(armAction0);
            //baseCtrl.moveToDirectionSpeed(-0.3f,0f);
            //ArmAction armAction1= new ArmAction(12, "get");
            //armCtrl.getThing(armAction1);
            ArmAction armAction0 = new ArmAction(600, "get");
            armCtrl.getThing(armAction0);
            baseCtrl.moveToDirectionSpeed(-0.05f, 0);
        }
        /// <summary>
        /// GPSR物体抓取大物体
        /// </summary>
        public virtual void ArmGetBig()
        {
            ArmAction armAction0 = new ArmAction(31, "get");
            armCtrl.getThing(armAction0);
            baseCtrl.moveToDirectionSpeed(-0.3f, 0f);
            ArmAction armAction1 = new ArmAction(32, "get");
            armCtrl.getThing(armAction1);
        }

        /// <summary>
        /// GPSR判断物体的位置是否合法
        /// </summary>
        /// <param name="objCameraPosition">物体的坐标</param>
        /// <returns>返回是否合法</returns>
        protected virtual bool ifObjCameraPositionLegal(CameraSpacePoint objCameraPosition)
        {
            bool res = (objCameraPosition.Z > 0 && objCameraPosition.Z < 1.500);
            return res;
        }
        /// <summary>
        /// GPSR判断物体的位置是否合法
        /// </summary>
        /// <param name="objCameraPosition">物体的坐标</param>
        /// <returns>返回是否合法</returns>
        protected virtual bool ifObjCameraPositionLegal(ObjCameraPosition objCameraPosition)
        {
            bool res = (objCameraPosition.Z > 0 && objCameraPosition.Z < 1.500);
            return res;
        }
        /// <summary>
        /// 移动到某处
        /// </summary>
        /// <param name="p">地点名</param>
        /// <returns>返回是否到达</returns>
        public bool moveToPlaceByName(string name)
        {
            bool res = false;
            BaseCtrl baseCtrl = new BaseCtrl();
            DBCtrl dbCtrl = new DBCtrl();
            string pname = name;
            Place goal = dbCtrl.GetPlaceByName(pname);
            if (!string.IsNullOrEmpty(goal.No))
            {
                res = baseCtrl.moveToGoal(goal);
            }
            return res;
        }

        public bool moveToPlaceByUser(User user)
        {
            baseCtrl.moveToDirectionSpeed(user.BodyCenter.Z - 0.8f, user.BodyCenter.X);
            return true;
        }

        /// <summary>
        /// 机器人最佳抓取距离
        /// </summary>
        public readonly float bestDistence = 0.71f;//越大往后越多

        /// <summary>
        /// 根据物体的位置调整机器人位置\
        /// </summary>
        /// <param name="objCameraPosition">物体的位置</param>
        /// <returns>计算后调整的位置</returns>
        public virtual void adjustMoveDirection(CameraSpacePoint objCameraPosition)
        {
            Console.WriteLine("move:X:" + (objCameraPosition.Z - bestDistence) + "  Y:" + objCameraPosition.X);
            float X = objCameraPosition.Z - bestDistence;
            //由于不太准确,需要向左纠正3厘米 +是左 -是右
            float Y = objCameraPosition.X -0.07f;
            //float Z = objCameraPosition.Z;
            if (objCameraPosition.Z == 0)
            { 
                baseCtrl.moveToDirectionSpeed(0, 0);
            }
            else
            {
                Y = Y + (float)0.11;
                //Z = Z + (float)0.02;
               
                baseCtrl.moveToDirectionSpeed(0, Y);
                baseCtrl.moveToDirectionSpeed(X, 0);
            }
            //return res;
        }


        /// <summary>
        /// robot说话
        /// </summary>
        /// <param name="words">所说的话</param>
        public void speak(string words)
        {
            baseSpeech.robotSpeak(words);
        }

        /// <summary>
        /// 放置物体，需要更具情况改动
        /// </summary>
        public virtual void ArmPut()
        {
            ArmAction armAction1 = new ArmAction(700, "put");
            armCtrl.putThing(armAction1);
            Thread.Sleep(1500);
            baseCtrl.moveToDirectionSpeed(-0.1f, 0);
            ArmAction armAction2 = new ArmAction(1000, "fuck");
            armCtrl.putThing(armAction2);
            //baseCtrl.moveToDirectionSpeed(0.3f, 0f);
            //ArmAction armAction2 = new ArmAction(22, "put");
            //armCtrl.putThing(armAction2);
            //Thread.Sleep(800);
            //ArmAction armAction3 = new ArmAction(1000, "put");
            //armCtrl.putThing(armAction3);
        }
        /// <summary>
        /// 放置物体，需要更具情况改动
        /// </summary>
        public virtual void ArmGive()
        {
            //改过
            ArmAction armAction1 = new ArmAction(700, "put");
            armCtrl.putThing(armAction1);
        }

        /// <summary>
        /// 从数据库中读取物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Goods GetGoodsByName(string name)
        {
            DBCtrl dbCtrl = new DBCtrl();
            return dbCtrl.GetGoodsByName(name);
        }

        /// <summary>
        /// 地盘找到自己所处的位置
        /// </summary>
        /// <returns></returns>
        public Place GetPlace()
        {
            return baseCtrl.getNowPlace();
        }

        /// <summary>
        /// 开始骨架检测
        /// </summary>
        public void initBodyDetect()
        {
            visionCtrl.startUserTracker();
        }

        /// <summary>
        /// 初始化camshift
        /// </summary>
        public void initCamshift()
        {
            visionCtrl.readyVisionHub();
        }

        /// <summary>
        /// 获取所有场景中所有人
        /// </summary>
        /// <returns></returns>
        public List<User> getAllUser()
        {
            return visionCtrl.getAllusers();
        }



        public int getfaceNum()
        {
            SurfResult[] peoples;
            int maxNum = 0;
            for (int i = 0; i < 5; i++)
            {
                peoples = this.CountFace();
                if (peoples.Count() > maxNum)
                    maxNum = peoples.Count();
            }
            return maxNum;
        }

        /// <summary>
        /// 通过人的位置信息的坐标点计算机器人速度
        /// </summary>
        /// <param name="bodyPoint"></param>
        /// <returns></returns>
        public Point ComputSpeed(CameraSpacePoint bodyPoint)
        {
            return MathPloblems.twistCompute(bodyPoint.X, bodyPoint.Z);
        }

        /// <summary>
        /// 根据人腿位置计算速度
        /// </summary>
        /// <param name="leg"></param>
        /// <returns></returns>
        public Point ComputSpeed(Leg leg)
        {
            return MathPloblems.twistCompute(leg.Y, leg.X);
        }
        /// <summary>
        /// 直接向地盘发送速度
        /// </summary>
        /// <param name="twist"></param>
        public void SendSpeed(Point twist)
        {
            baseCtrl.SendSpeed(twist);
        }

        /// <summary>
        /// 发送距离，向前为x正，向下为x负，向左为y正，向右为y负
        /// </summary>
        /// <returns>返回值为1</returns>
        public int SendXY(float x, float y)
        {
            return baseCtrl.SendDisplacemet(x, y);
        }
        /// <summary>
        /// 发送速度运动，并作平滑处理
        /// </summary>
        /// <param name="twist"></param>
        public void SendSpeedSmooth(Point twist)
        {
            baseCtrl.SendSpeed(MathPloblems.smoothSpeed(twist));
        }

        /// <summary>
        /// 移动至某个地点
        /// </summary>
        /// <param name="place">place 类型地点</param>
        public void moveToPlace(Place place)
        {
            baseCtrl.moveToGoal(place);
        }

        /// <summary>
        /// 直线跑动（通过延时跑动）
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void moveDirectionBySpeed(float X, float Y)
        {
            baseCtrl.moveToDirectionSpeed(X, Y);
        }

        /// <summary>
        /// 转圈跑动（通过延时转圈，参数为角度制）
        /// </summary>
        public void moveDirectionBySpeedW(float w)
        {
            float rad = (float)(Math.PI * w / 180);

            baseCtrl.moveToDirectionSpeedW(rad);
        }

        /// <summary>
        /// 发送直接停止动作
        /// </summary>
        public void SpeedStop()
        {
            baseCtrl.SendSpeed(new entity.Point(0, 0, 0));
        }

        /// <summary>
        /// 发送平缓速度停止
        /// </summary>
        public void SpeedStopSmooth()
        {
            while (true)
            {
                entity.Point speed = MathPloblems.smoothSpeed(new entity.Point(0, 0, 0));
                SendSpeed(speed);
                if (speed.X == 0 || speed.Y == 0 || speed.Z == 0)
                {
                    break;
                }
            }

        }

        /// <summary>
        /// 开始SURF检测
        /// </summary>
        public void StartSurf()
        {
            VisionCtrl.startCoordinateMapping();
        }
        /// <summary>
        /// 初始化图像
        /// </summary>
        public void initSurfFrmae()
        {
            VisionCtrl.initSurf();
        }

        /// <summary>
        /// 初始化深度学习
        /// </summary>
        public void initMachineLearning()
        {
            VisionCtrl.initMachineLearning();
        }

        /// <summary>
        /// 初始化语音
        /// </summary>
        public void initSpeech()
        {


        }

        public User LastUser = new User();
        /// <summary>
        /// 寻找正确的人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User findCorrectUser(int userId)
        {
            List<User> allUsersList = getAllUser();
            User[] allUser = allUsersList.ToArray();

            User u = null;
            for (int i = 0; i < allUser.Length; i++)
            {
                if (allUser[i].ID == userId)
                {
                    u = allUser[i];
                }
            }
            if (allUser.Length == 0)
            {

                return u;
            }
            if (u != null)
            {
                u.confidence = 1;
                LastUser = u;
            }
            else
            {
                for (int i = 0; i < allUser.Length; i++)
                {
                    User user = allUser[i];

                    if (user.BodyCenter.Z > LastUser.BodyCenter.Z)
                    {
                        user.trackingHeight();
                        if (MathPloblems.Distance3D(user.BodyCenter, LastUser.BodyCenter) < 0.5)
                        {
                            user.confidence += 0.3f;
                        }
                        else
                        {
                            user.confidence += 0.1f;
                        }
                        if (user.BodyCenter.Z > 3)
                        {
                            user.confidence -= 0.1f;
                        }
                        if (Math.Abs(LastUser.HeightCharacteristic - user.HeightCharacteristic) > 0.1)
                        {
                            user.confidence += 0.1f;
                        }
                        else
                        {
                            user.confidence -= 0.1f;
                        }
                        Console.WriteLine("Now ID:" + user.ID.ToString() + "  Height:" + user.UserHeight.ToString() + "  X:" +
      user.BodyCenter.X.ToString() + "  Z:" + user.BodyCenter.Z.ToString() + "  confidence:" +
      user.confidence.ToString());

                    }
                }
                float maxConfidence = 0;
                for (int i = 0; i < allUser.Length; i++)
                {
                    if (maxConfidence < allUser[i].confidence)
                    {
                        maxConfidence = allUser[i].confidence;
                    }
                }
                for (int i = 0; i < allUser.Length; i++)
                {
                    if (Math.Abs(allUser[i].confidence - maxConfidence) < 0.05)
                    {
                        u = allUser[i];
                    }
                }
            }
            Console.WriteLine("find user");
            return u;
        }
        /// <summary>
        /// 等待门开启
        /// </summary>
        /// <returns></returns>
        public virtual bool WaitForDoor()
        {
            bool res;
            speak("May I coming?");
            while (true)
            {
                CameraSpacePoint centerPoint = visionCtrl.getCenterCameraSpacePoint();
                res = (centerPoint.Z > 1.5 || centerPoint.Z < 0);
                if (res)
                {
                    speak("I am coming!");
                    break;
                }
                Thread.Sleep(100);
            }
            return res;
        }

        /// <summary>
        /// 向某个位置移动(带有导航)
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="A"></param>
        public void moveToDirection(float X, float Y, float A)
        {
            baseCtrl.moveToDirection(X, Y, A);
        }

        /// <summary>
        /// 说出已经准备好
        /// </summary>
        public void speakReady()
        {
            baseSpeech.robotSpeak("I am Ready!");
        }

        /// <summary>
        /// 获取机器人现在的图像信息
        /// </summary>
        /// <returns>图像的路径</returns>
        public string getCorrentImg()
        {
            return visionCtrl.getKinectImage();
        }
        /// <summary>
        /// 获取camshift获取的人体的位置
        /// </summary>
        /// <param name="u"></param>
        public void setCanshiftBody(User u)
        {
            visionCtrl.SetCamshiftBodyPoint(u);
        }
        /// <summary>
        /// 获取目前的camshift点的位置
        /// </summary>
        /// <returns></returns>
        public ColorSpacePoint findCamShiftRes()
        {
            ColorSpacePoint BodyCenter = visionCtrl.GetCamshiftRes();
            BodyCenter.X -= 960;
            BodyCenter.Y -= 540;
            return BodyCenter;
        }
        /// <summary>
        /// 更新camshift的信息
        /// </summary>
        public void FlashCamShift()
        {
            visionCtrl.FlashCamshift();
        }
        /// <summary>
        /// 寻找场景中最中间
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public User findUserMiddleRasingHand(List<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    user.trackingHand();
                }
            }
            catch { }
            List<User> handOnUsers = users.FindAll(user => user.isRaisingHand);
            User resUser = new User();
            if (handOnUsers.Count != 0)
            {
                int MinX = 0;
                resUser = handOnUsers[0];
                foreach (var user in handOnUsers)
                {
                    if (Math.Abs(resUser.BodyCenter.X) - Math.Abs(user.BodyCenter.X) > 0)
                    {
                        resUser = user;
                        speak("I Find you !");
                    }
                }
            }
            return resUser;
        }
        public User EnsureUser(List<User> users)
        {
            User resUser = new User();
            return null;
        }
        /// <summary>
        /// 正对人
        /// </summary>
        /// <param name="user"></param>
        public void GetCloseToUser(User user, bool isMoveDirection = false)
        {
            if (isMoveDirection)
                baseCtrl.moveToDirection(user.BodyCenter.Z - 0.5f, user.BodyCenter.X, (float)(Math.Atan((user.BodyCenter.X / user.BodyCenter.Z) * 180 / Math.PI)));
            else
                baseCtrl.moveToDirection(0, 0, (float)(Math.Atan((user.BodyCenter.X / user.BodyCenter.Z) * 180 / Math.PI)));
        }

        /// <summary>
        /// 简单的跟踪一个人
        /// </summary>
        public void followEasy()
        {
            bool FollowFlag = true;
            bool isUserReady = false;
            User user = new User();
            int userId = 0;
            while (FollowFlag)
            {
                List<User> usersList = getAllUser();
                try
                {
                    foreach (User userPush in usersList)
                    {
                        userPush.trackingHandLeftPush();
                        userPush.trackingHandRightPush();
                        if (userPush.isHandLeftPush || userPush.isHandRightPush)
                        {
                            SendSpeed(new Point(0, 0, 0));
                            speak("end follow ");
                            FollowFlag = false;
                            break;
                        }

                    }
                }
                catch { }
                while (true)
                {
                    try
                    {
                        foreach (User userPush in usersList)
                        {
                            userPush.trackingHandLeftPush();
                            userPush.trackingHandRightPush();
                            if (userPush.isHandLeftPush || userPush.isHandRightPush)
                            {
                                SendSpeed(new Point(0, 0, 0));
                                speak("end follow ");
                                FollowFlag = false;

                            }

                        }
                    }
                    catch { }

                    if (isUserReady || !FollowFlag)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    if (usersList.Count != 0)
                        user = findUserMiddleRasingHand(usersList);

                    if (user.ID != 0)
                    {
                        speak("I find you");
                        userId = user.ID;
                        isUserReady = true;
                        break;
                    }
                }
                User u = usersList.Find(us => us.ID == userId);
                if (u == null)
                {
                    isUserReady = false;
                    SendSpeed(new Point(0, 0, 0));
                    speak("I lost you");
                    userId = 0;

                }
                else
                {
                    Point twist = ComputSpeed(u.BodyCenter);
                    u.trackingHandLeftPush();
                    u.trackingHandRightPush();
                    if (!u.isHandPush)
                    {
                        SendSpeedSmooth(twist);
                    }
                    else
                    {
                        SendSpeedSmooth(new Point(0, 0, 0));
                    }
                    if (u.isHandLeftPush && u.isHandRightPush)
                    {
                        SendSpeed(new Point(0, 0, 0));
                        speak("end follow ");
                        FollowFlag = false;
                        break;
                    }
                }
            }


        }

        public bool FindPeople()
        {
            bool res = false;

            speak("start finding people!");
            List<User> users = new List<User>();
            List<User> finalUsers = new List<User>();
            while (users.Count == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    users = getAllUser();
                    foreach (var user in users)
                    {
                        if (finalUsers.Find(us => us.ID == user.ID) == null)
                        {
                            finalUsers.Add(user);
                        }

                    }
                }
                users = getAllUser();
            }
            List<CameraSpacePoint> usersposition = new List<CameraSpacePoint>();
            //CameraSpacePoint usersposition = new CameraSpacePoint();
            int numofbody = 0;
            foreach (var user in finalUsers)
            {
                if (user.ID != 0)
                {
                    numofbody++;
                    usersposition.Add(user.BodyCenter);
                }
            }
            //roboNures2015Task.speak("I find " + numofbody + "people");
            foreach (var position in usersposition)
            {
                moveToDirection(position.Z - 1.0f, -position.X, 0);
                speak("i have found you.");
                res = true;
                //roboNures2015Task.ask();
                //CommandStrList.Add(roboNures2015Task.whoisWhoCommand);
            }

            return res;
        }
    }
}
