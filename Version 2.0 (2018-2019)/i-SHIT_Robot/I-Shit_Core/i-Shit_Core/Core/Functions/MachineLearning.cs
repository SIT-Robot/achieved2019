/*this file intent to implement Machine Learning function
 * 这个文件用于实现深度学习的功能
 * the naming rule      values are all lowercase;
 *                      arguement using underline: arg_;
 *                      the class fields using underline frond: _class_property;
 *                      functions has the first upcase: Func();
 *                   
 * 命名规则：  变量全部小写
 *            形参使用后下滑线： arg_
 *            字段使用前下划线： _class_property
 *            函数开头大写：   LookingForStuff();
 * 
 */
using i_Shit_Core.Core.Drivers;
using Microsoft.Kinect;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace i_Shit_Core.Core.Functions
{
    public static partial class Function
    {
        public class MachineLearningResult
        {
            string name;
            ColorSpacePoint[] colorSpacePoints;
            int thingsNumber;
            float[] confidence;

            public ColorSpacePoint[] ColorSpacePoints
            {
                get
                {
                    return colorSpacePoints;
                }

                set
                {
                    colorSpacePoints = value;
                }
            }

            public int ThingsNumber
            {
                get
                {
                    return thingsNumber;
                }

                set
                {
                    thingsNumber = value;
                }
            }

            public float[] Confidence
            {
                get
                {
                    return confidence;
                }

                set
                {
                    confidence = value;
                }
            }

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public MachineLearningResult(int incomingthingsnumber)
            {
                Name = "";
                this.colorSpacePoints = new Microsoft.Kinect.ColorSpacePoint[incomingthingsnumber];
                this.thingsNumber = incomingthingsnumber;
                this.confidence = new float[incomingthingsnumber];
            }

            public MachineLearningResult(string name, ColorSpacePoint[] colorSpacePoint, int thingsNumber, float[] confidence)
            {
                this.Name = name;
                this.colorSpacePoints = colorSpacePoint;
                this.thingsNumber = thingsNumber;
                this.confidence = confidence;
            }
        }

        /// <summary>
        /// 拍照去让ML识别解析JSON，找到至少一个的话返回MachineLearningResult。一个都找不到返回NULL。
        /// </summary>
        /// <param name="thing">要找的Object Name</param>
        /// <returns></returns>
        private static MachineLearningResult GetMachineLearningResult(string thing)
        {
            float confideence = 0;

            ColorSpacePoint thingPoit = new ColorSpacePoint();
            string receiveStr = Driver.MachineLearningSocket_StartAndGetResult();
            Console.WriteLine("ML JSON: " + receiveStr);
            JObject obj = JObject.Parse(receiveStr);
            MachineLearningResult machineLearingresult = new MachineLearningResult(obj.Count);

            for (int i = 1; i <= obj.Count; i++)
            {

                if (obj[i.ToString()][thing] != null)
                {
                    string result = (string)obj[i.ToString()][thing];
                    string[] splitResult = result.Split(new char[] { 'a' });
                    thingPoit.X = float.Parse(splitResult[0]);
                    thingPoit.Y = float.Parse(splitResult[1]);
                    confideence = float.Parse(splitResult[2]);
                    machineLearingresult.ColorSpacePoints[i - 1] = thingPoit;
                    machineLearingresult.Confidence[i - 1] = confideence;
                }

            }
            if (machineLearingresult.ThingsNumber != 0)
            {
                return machineLearingresult;

            }
            else
            {
                throw new Exception("未找到该物体！");
            }

        }

        /// <summary>
        /// 深度学习找物体，需要自己拍照放到ML Server。找到后返回可信度最大的物体的ColorSpacePoint（物体中心点在照片上的坐标）
        /// ！！注意！！此方法会抛异常，找不到就抛异常，注意try catch。
        /// </summary>
        /// <param name="objName">要找到的物体名称</param>
        /// <returns></returns>
        public static ColorSpacePoint Vision_FindObjectByMachineLearning(string objName)//**
        {
            if ((Driver.Emulator_Mode == true && Driver.Emulator_MLCSP_Enable == true))
            {
                Console.WriteLine("Emulator: FindObjectByMachineLearning:" + objName);
                return new ColorSpacePoint();
            }
            //MachineLearingResult machineLearingResult = null;
            int maxConfidenceIndex = 0;//可信度最大的下标
            try
            {
                MachineLearningResult machineLearingResult = GetMachineLearningResult(objName);//resolve json 
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
                    Console.WriteLine("ML: Searching for " + 0 + "times");
                    return machineLearingResult.ColorSpacePoints[maxConfidenceIndex];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MachineLearning ERROR: 结果中没有物体 " + objName + " ！");
                throw new Exception("MachineLearning Obj NotFound. " + e.Message);
            }
            Console.WriteLine("MachineLearning ERROR: 结果中没有物体 " + objName + " ！");
            throw new Exception("MachineLearning Obj NotFound. ");
            //return new ColorSpacePoint();
        }
    }
}

