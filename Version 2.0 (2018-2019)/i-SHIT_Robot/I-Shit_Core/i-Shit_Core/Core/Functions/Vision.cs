using Emgu.CV;
using Emgu.CV.Structure;
using i_Shit_Core.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i_Shit_Core.Core.Drivers;

using Microsoft.Kinect;//CameraSpacePoint
using System.Threading;
using System.IO;
using System.Globalization;

namespace i_Shit_Core.Core.Functions
{
    public static partial class Function
    {
        /// <summary>
        /// 通过Kinect判断前方障碍位置，大于1.5就继续。
        /// </summary>
        public static void Vision_WaitForDoor()
        {

            Console.WriteLine("Waiting for door openning via Kinect....");
            bool res;
            while (true)
            {
                ColorSpacePoint csp = new ColorSpacePoint();
                csp.X = 960;
                csp.Y = 540;
                CameraSpacePoint centerPoint = Vision_GetCameraSpacePoint(csp);//960x540正好是1920x1080的一半，得到Kinect中心点的坐标。
                Console.WriteLine("WaitForDoor Distance :" + centerPoint.Z);
                res = (centerPoint.Z > 3 | centerPoint.Z < 0);
                if (res)
                {
                    Console.WriteLine("Door Opened. Program Continue.");
                    break;
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 使用Kinect拍照
        /// </summary>
        /// <returns>返回图像路径</returns>
        public static string Vision_Kinect_Shot()
        {
            if ((Driver.Emulator_Mode == true && Driver.Emulator_MLCSP_Enable == true))
            {
                Console.WriteLine("Emulator: Kinect开始拍照...");
                return "";
            }
            Console.WriteLine("Kinect开始拍照...");
            Driver.KinectShotImgPath = null;
            Driver.KinectShotFlag = true;
            while (Driver.KinectShotFlag) ;//等拍完照
            return Driver.KinectShotImgPath;
        }
        public static List<FaceInfo> Vision_BaiduFaceDetect()
        {

            List < FaceInfo > returnList = new List<FaceInfo>();
    
            // 设置APPID/AK/SK
            var APP_ID = "15347967";
            var API_KEY = "GnncmQGahxRYW6mvKzN5pUlS";
            var SECRET_KEY = "ahsaSDneQDGbPu7YvQxPyAvmNeOl4Db5";

            int nMale = 0;
            int nFemale = 0;
            int nPersonSitting = 0;
            int nPersonStanding = 0;

            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            string imgPath = Vision_Kinect_Shot();
            Vision_SaveJPGPhoto(imgPath, @"C:\Users\jia\Downloads\1.jpg");
            FileStream filestream = new FileStream(@"C:\Users\jia\Downloads\1.jpg", FileMode.Open);
            Console.WriteLine("拍照");
    
            byte[] bt = new byte[filestream.Length];

            // 调用read读取方法
            filestream.Read(bt, 0, bt.Length);
            var image = Convert.ToBase64String(bt);
            filestream.Close();

            var imageType = "BASE64";

            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"face_field", "gender"},
                {"max_face_num", 10},
                {"face_type", "LIVE"}
            };
            // 带参数调用人脸检测
            
                var result = client.Detect(image, imageType, options);

                // Console.WriteLine(result);
                if (Convert.ToDecimal(result["error_code"]) == 0) // Success.
                {
                    var FaceList = result["result"]["face_list"].ToArray();
                    double fTotalY = 0;

                    foreach (var Face in FaceList)
                    {
                        fTotalY += Convert.ToDouble(Face["location"]["top"]);
                        if (Face["gender"]["type"].ToString() == "male")
                        {
                            nMale++;
                        }
                        else
                        {
                            nFemale++;
                        }

                        // 导出到FaceInfo.

                        FaceInfo t = new FaceInfo();
                        t.FaceLocation.X = Convert.ToInt32(Face["location"]["left"]);
                        t.FaceLocation.Y = Convert.ToInt32(Face["location"]["top"]);
                        t.FaceLocation.Width = (int)Face["location"]["width"];
                        t.FaceLocation.Height = (int)Face["location"]["height"];

                        t.sex = Face["gender"]["type"].ToString() == "male" ? 1 : 0;
                        returnList.Add(t);

                    }
                    Console.WriteLine("Male: " + nMale.ToString() + "  Female: " + nFemale.ToString());
                    double fAvgY = fTotalY / (nMale + nFemale);

                    foreach (var Face in FaceList)
                    {
                        if (Convert.ToDouble(Face["location"]["top"]) < fAvgY)
                        {
                            nPersonStanding++;
                        }
                        else
                        {
                            nPersonSitting++;
                        }
                    }
                    Console.WriteLine("Standing: " + nPersonStanding.ToString() + "  Sitting: " + nPersonSitting.ToString());
                }
            //}
            //catch
            //{
            //    Console.WriteLine("Baidu Error");
            //}

            try
            {
                if (returnList.Count() != 0)
                {
                    Image<Bgr, byte> temp2 = new Image<Bgr, byte>(imgPath);
                    for (int i = 0; i < returnList.Count(); i++)
                    {

                        Rectangle lol = new Rectangle(returnList[i].FaceLocation.X, returnList[i].FaceLocation.Y, returnList[i].FaceLocation.Width, returnList[i].FaceLocation.Height);

                        if (returnList[i].sex == 1) // male
                        {
                            temp2.Draw(lol, new Bgr(Color.Blue), 2);
                        }
                        else
                        {
                            temp2.Draw(lol, new Bgr(Color.Red), 2);
                        }
                        
                    }
                    TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    long a = Convert.ToInt64(tss.TotalMilliseconds);
                    string myPhotos = @"..\Data\Faces\";
                    myPhotos = Path.Combine(myPhotos, "face-" + a + ".png");
                    CvInvoke.cvSaveImage(myPhotos, temp2, temp2);
                }
            }
            catch { Console.WriteLine("FaceDetect: Save Face Total Image Error!"); }
            return returnList;
        }
        /// <summary>
        /// 检测图片中的人脸
        /// </summary>
        /// <returns>返回图片中人脸的信息</returns>
        public static FaceInfo[] Vision_FaceDetect()
        {
            string imgPath = Vision_Kinect_Shot();
            FaceInfo[] allFace = null;         //所有的人脸
            CascadeClassifier face_cascade = new CascadeClassifier("haarcascade_frontalface_alt.xml");
            Image<Gray, byte> img = new Image<Gray, byte>(imgPath);
            img._EqualizeHist();            //对图片进行灰度处理
            Rectangle[] faces = face_cascade.DetectMultiScale(img, 1.3, 3, new System.Drawing.Size(20, 20), System.Drawing.Size.Empty);             //返回所有人脸的位置
            allFace = new FaceInfo[faces.Length];
            for (int i = 0; i < faces.Length; i++)
            {
                allFace[i] = new FaceInfo();
                allFace[i].ID = i;
                allFace[i].FaceLocation = faces[i];

                using (Image<Gray, Byte> observedImage = new Image<Gray, Byte>(imgPath))
                {
                    face_cascade = new CascadeClassifier("haarcascade_frontalface_alt.xml");
                    CvInvoke.cvEqualizeHist(observedImage, observedImage);
                    int numOfFace = faces.Count();
                    if (numOfFace == 0)
                    {
                        System.Console.WriteLine("FaceDetect:人头数为0");
                    }
                    //裁剪图片

                    try
                    {
                        System.IO.Directory.CreateDirectory(@"..\Data\Faces\" + numOfFace);
                    }
                    catch { System.IO.Directory.Delete(@"..\Data\Faces\" + numOfFace); System.IO.Directory.CreateDirectory(@"..\Data\Faces\" + numOfFace); }
                    for (int ii = 0; ii < numOfFace; ii++)
                    {

                        System.Drawing.Size roisize = new Size(faces[ii].Width, faces[ii].Height);
                        IntPtr dst = CvInvoke.cvCreateImage(roisize, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                        CvInvoke.cvSetImageROI(observedImage, faces[ii]);
                        Image<Gray, Byte> faceROI = observedImage.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC).Clone();

                        //保存faceROI人脸图案

                        string myPhotos = @"..\Data\Faces\" + numOfFace;
                        myPhotos = Path.Combine(myPhotos, "face-" + ii + ".png");
                        //CvInvoke.Imwrite(imgPath, Img);
                        CvInvoke.cvSaveImage(myPhotos, faceROI, faceROI);
                        //--保存faceROI人脸图案
                        Console.WriteLine("FaceDetect:总共有" + numOfFace + "人");
                    }

                }

            }
            //Save Face Img
            try
            {
                if (allFace.Length != 0)
                {
                    Image<Bgr, byte> temp2 = new Image<Bgr, byte>(imgPath);
                    for (int i = 0; i < allFace.Length; i++)
                    {

                        Rectangle lol = new Rectangle(allFace[i].FaceLocation.X, allFace[i].FaceLocation.Y, allFace[i].FaceLocation.Width, allFace[i].FaceLocation.Height);
                    
                        temp2.Draw(lol, new Bgr(Color.Red), 2);
                    }
                    TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    long a = Convert.ToInt64(tss.TotalMilliseconds);
                    string myPhotos = @"..\Data\Faces\";
                    myPhotos = Path.Combine(myPhotos, "face-" + a + ".png");
                    CvInvoke.cvSaveImage(myPhotos, temp2, temp2);
                }
            }
            catch { Console.WriteLine("FaceDetect: Save Face Total Image Error!"); }
            //--Save Face Img

            return allFace;
        }


        /// <summary>
        /// 获取colorSpacePoint对应的距离
        /// </summary>
        /// <param name="colorSpacePoint"></param>
        /// <returns></returns>
        public static CameraSpacePoint Vision_GetCameraSpacePoint(ColorSpacePoint colorSpacePoint)//***
        {
            if ((Driver.Emulator_Mode == true && Driver.Emulator_MLCSP_Enable == true))
            {
                CameraSpacePoint returnCSP = new CameraSpacePoint();
                Driver.UIThreadOperator.Send(delegate
                {
                    returnCSP.X = int.Parse(Driver.Emulator_MLCSPWindow.XText.Text);
                    returnCSP.Z = int.Parse(Driver.Emulator_MLCSPWindow.ZText.Text);
                }, null);

                return returnCSP;
            }
            Driver.EnableCoordinateMappingFlag = true;
            Thread.Sleep(1000);
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            while (true)
            {
                //judge the point is whether in the picture;
                if (colorSpacePoint.X > Driver.colorWidth || colorSpacePoint.X < 0 || colorSpacePoint.Y > Driver.colorHeight || colorSpacePoint.Y < 0)
                {
                    return cameraPoint;
                }
                if (Driver.CoordinateMappingReadyFlag)
                {
                    cameraPoint = Driver.cameraPointsFinalResult[(int)colorSpacePoint.Y * Driver.colorWidth + (int)colorSpacePoint.X];
                    cameraPoint.X = cameraPoint.X - 0.05f;
                    Driver.EnableCoordinateMappingFlag = false;
                    return cameraPoint;
                }
            }
        }

        /// <summary>
        /// 从PNG转JPG，可用于将Kinect拍得的照片保存成JPG并存到其它地方（比如Machine Learning Server）
        /// </summary>
        /// <param name="_SourcePNGImagePath"></param>
        /// <param name="_TargetJPGImagePath"></param>
        public static void Vision_SaveJPGPhoto(string _SourcePNGImagePath, string _TargetJPGImagePath)
        {
            if ((Driver.Emulator_Mode == true && Driver.Emulator_MLCSP_Enable == true))
            { return; }
            Image<Bgr, byte> Img = new Image<Bgr, byte>(_SourcePNGImagePath);
            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, _TargetJPGImagePath);
            CvInvoke.cvSaveImage(_TargetJPGImagePath, Img, Img);

        }

    }
}
