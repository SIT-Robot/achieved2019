using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using Newtonsoft.Json.Linq;

using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.State;
using Emgu.CV.CvEnum;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.BLL.ServiceCtrl
{
    /// <summary>
    /// 图像控制类
    /// </summary>
    public class VisionCtrl
    {
        Image<Gray, byte> observedImage;
        public static string PROGRAMPATH = System.Windows.Forms.Application.StartupPath;

        public void StartSurf()
        {
            KinectConnection.GetInstance().StartSurf();
        }

        public void StartSurf(bool surFlag, bool bgFlag)
        {
            KinectConnection.GetInstance().StartSurf(surFlag, bgFlag);
        }

        /// <summary>
        /// 结束SURF
        /// </summary>
        public void endSURF()
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();

            kinectConnection.endSURF();
        }

        /// <summary>
        /// 找到某一个物体
        /// </summary>
        /// <param name="obj">物体</param>
        /// <returns>返回物体的坐标信息</returns>
        public SurfResult findObject(Goods goods)
        {
            StartSurf();
            Stopwatch watch;

            List<SurfResult> surfResultList = new List<SurfResult>();
            watch = Stopwatch.StartNew();
            int ind = 0;
            foreach (var path in goods.imgPath)
            {
                ind++;
                SurfResult surfResult = new SurfResult();
                surfResult.name = goods.Name;
                setSaveImgNameandIndex(goods.Name, ind);
                surfResult = FindObj(path, 0);
                surfResultList.Add(surfResult);
            }
            watch.Stop();
            long matchTime = watch.ElapsedMilliseconds;
            int goodMax = 0;
            SurfResult finalSurfResult = new SurfResult();
            finalSurfResult.name = goods.Name;
            for (int i = 0; i < surfResultList.Count; i++)
            {
                surfResultList[i].isSuccess = false;
                ColorSpacePoint[] corners = new ColorSpacePoint[4];
                corners[0] = surfResultList[i].cornerColorPoints[0];
                corners[1] = surfResultList[i].cornerColorPoints[1];
                corners[2] = surfResultList[i].cornerColorPoints[2];
                corners[3] = surfResultList[i].cornerColorPoints[3];
                float[] angelsRes = new float[4];
                angelsRes[0] = MathPloblems.pointAngel(corners[0], corners[1], corners[2]);
                angelsRes[1] = MathPloblems.pointAngel(corners[1], corners[2], corners[3]);
                angelsRes[2] = MathPloblems.pointAngel(corners[2], corners[3], corners[0]);
                angelsRes[3] = MathPloblems.pointAngel(corners[3], corners[0], corners[1]);


                //如果目标图近似矩形则成功        
                int NumOfLegal = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (angelsRes[j] > 60 && angelsRes[j] < 120)
                    {
                        NumOfLegal++;
                    }
                }
                if (NumOfLegal >= 3)
                {
                    surfResultList[i].isSuccess = true;
                }



                if ((goodMax < surfResultList[i].numOfGoodPoint) && surfResultList[i].isSuccess)
                {
                    goodMax = surfResultList[i].numOfGoodPoint;
                    finalSurfResult = surfResultList[i];
                    finalSurfResult.name = goods.Name;
                }
            }
            endSURF();
            return finalSurfResult;
        }

        /// <summary>
        /// 找到某一个物体
        /// 预处理用图像分割代替背景过滤
        /// </summary>
        /// <param name="obj">物体</param>
        /// <returns>返回物体的坐标信息</returns>
        public SurfResult findObject2(Goods goods, Rectangle rect, int count = 1)
        {
            StartSurf(true, false);
            Stopwatch watch;

            List<SurfResult> surfResultList = new List<SurfResult>();
            watch = Stopwatch.StartNew();
            int ind = 0;
            foreach (var path in goods.imgPath)
            {
                ind++;
                SurfResult surfResult = new SurfResult();
                surfResult.name = goods.Name;
                setSaveImgNameandIndex(goods.Name, ind);
                surfResult = FindObj2(path, rect, count);
                surfResultList.Add(surfResult);
            }
            watch.Stop();
            long matchTime = watch.ElapsedMilliseconds;
            int goodMax = 0;
            SurfResult finalSurfResult = new SurfResult();
            finalSurfResult.name = goods.Name;
            for (int i = 0; i < surfResultList.Count; i++)
            {
                surfResultList[i].isSuccess = false;
                ColorSpacePoint[] corners = new ColorSpacePoint[4];
                corners[0] = surfResultList[i].cornerColorPoints[0];
                corners[1] = surfResultList[i].cornerColorPoints[1];
                corners[2] = surfResultList[i].cornerColorPoints[2];
                corners[3] = surfResultList[i].cornerColorPoints[3];
                float[] angelsRes = new float[4];
                angelsRes[0] = MathPloblems.pointAngel(corners[0], corners[1], corners[2]);
                angelsRes[1] = MathPloblems.pointAngel(corners[1], corners[2], corners[3]);
                angelsRes[2] = MathPloblems.pointAngel(corners[2], corners[3], corners[0]);
                angelsRes[3] = MathPloblems.pointAngel(corners[3], corners[0], corners[1]);


                //如果目标图近似矩形则成功        
                int NumOfLegal = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (angelsRes[j] > 60 && angelsRes[j] < 120)
                    {
                        NumOfLegal++;
                    }
                }
                if (NumOfLegal >= 3)
                {
                    surfResultList[i].isSuccess = true;
                }



                if ((goodMax < surfResultList[i].numOfGoodPoint) && surfResultList[i].isSuccess)
                {
                    goodMax = surfResultList[i].numOfGoodPoint;
                    finalSurfResult = surfResultList[i];
                    finalSurfResult.name = goods.Name;
                }
            }
            endSURF();
            return finalSurfResult;
        }
        public static void initSurf()
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            kinectConnection.StartMutipleFrame();
            //kinectConnection.startSURF();
        }

        public static void initMachineLearning()
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            kinectConnection.StartMutipleFrameMachineLearning();
        }



        public string name = "";
        public int index = 0;

        public void setSaveImgNameandIndex(string name, int index)
        {
            this.name = name;
            this.index = index;
        }

        /// <summary>
        /// 保存返回的图片
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string SaveResultImg(Image<Bgr, byte> Img, string name, int index)
        {

            string time = DateTime.Now.ToString("hh'-'mm'-'s-" + index, CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, "successfulSurf");

            string imgPath = Path.Combine(myPhotos, "SurfResult-" + name + index + "-" + time + ".png");
            //CvInvoke.Imwrite(imgPath, Img);
            CvInvoke.cvSaveImage(imgPath, Img, Img);
            return imgPath;
        }

        /// <summary>
        /// 抠出指定区域的物体
        /// </summary>
        /// <param name="Img">场景图片</param>
        /// <param name="rect">扣的范围</param>
        /// <param name="count">迭代次数</param>
        /// <returns>处理之后的图片</returns>
        public Image<Bgr, byte> GrabcutImg(Image<Bgr, byte> Img, Rectangle rect, int count = 1)
        {
            //Image<Bgr, byte> Img1 = new Image<Bgr, byte>("66.png");
            //Rectangle rect1 = new Rectangle(361, 205, 675, 486);
            Matrix<Single> foregroundModel = null;              //前景模型
            Matrix<Single> backgroundModel = null;              //背景模型
            Image<Bgr, byte> result;
            Stopwatch sw = new Stopwatch();
            Image<Gray, Byte> mask = null;
            sw.Start();
            //用掩码初始化
            //CvInvoke.CvGrabCut(Img.Ptr, mask.Ptr, ref rect, backgroundModel.Ptr, foregroundModel.Ptr, count, GRABCUT_INIT_TYPE.EVAL);

            //用矩形初始化
            mask = Img.GrabCut(rect, count);
            sw.Stop();

            //将掩码图像和1进行按位“与”操作，这样背景及可能的背景将变为0；而前景及可能的前景将变成1
            CvInvoke.cvAndS(mask.Ptr, new MCvScalar(1d), mask.Ptr, IntPtr.Zero);

            result = Img.Copy(mask);
            CvInvoke.cvShowImage("haha", result);
            return result;
        }

        /// <summary>
        /// 抠出指定区域的物体
        /// </summary>
        /// <param name="Img">场景图片的路径</param>
        /// <param name="rect">扣的范围</param>
        /// <param name="count">迭代次数</param>
        /// <returns>处理之后的图片</returns>
        public Image<Bgr, byte> GrabcutImg(string ImgStr, Rectangle rect, int count = 1)
        {
            Image<Bgr, byte> Img = new Image<Bgr, byte>(ImgStr);
            //Rectangle rect1 = new Rectangle(361, 205, 675, 486);
            Matrix<Single> foregroundModel = null;              //前景模型
            Matrix<Single> backgroundModel = null;              //背景模型
            Image<Bgr, byte> result;
            Stopwatch sw = new Stopwatch();
            Image<Gray, Byte> mask = null;
            sw.Start();
            //用掩码初始化
            //CvInvoke.CvGrabCut(Img.Ptr, mask.Ptr, ref rect, backgroundModel.Ptr, foregroundModel.Ptr, count, GRABCUT_INIT_TYPE.EVAL);

            //用矩形初始化
            mask = Img.GrabCut(rect, count);
            sw.Stop();
            System.Console.WriteLine("物体分割耗时:{0}", sw.Elapsed);
            //将掩码图像和1进行按位“与”操作，这样背景及可能的背景将变为0；而前景及可能的前景将变成1
            CvInvoke.cvAndS(mask.Ptr, new MCvScalar(1d), mask.Ptr, IntPtr.Zero);

            result = Img.Copy(mask);
            return result;
        }

        /// <summary>
        /// 抠出指定区域的物体
        /// </summary>
        /// <param name="Img">场景图片的路径</param>
        /// <param name="rect">扣的范围</param>
        /// <param name="count">迭代次数</param>
        /// <returns>处理之后的图片</returns>
        public Image<Gray, Byte> GrabcutGrayImg(string ImgStr, Rectangle rect, int count = 1)
        {
            Image<Bgr, byte> Img = new Image<Bgr, byte>(ImgStr);
            //Rectangle rect1 = new Rectangle(361, 205, 675, 486);
            Matrix<Single> foregroundModel = null;              //前景模型
            Matrix<Single> backgroundModel = null;              //背景模型
            Image<Bgr, byte> result;
            Stopwatch sw = new Stopwatch();
            Image<Gray, Byte> mask = null;
            sw.Start();
            //用掩码初始化
            //CvInvoke.CvGrabCut(Img.Ptr, mask.Ptr, ref rect, backgroundModel.Ptr, foregroundModel.Ptr, count, GRABCUT_INIT_TYPE.EVAL);

            //用矩形初始化
            mask = Img.GrabCut(rect, count);
            sw.Stop();
            System.Console.WriteLine("物体分割耗时:{0}", sw.Elapsed);
            //将掩码图像和1进行按位“与”操作，这样背景及可能的背景将变为0；而前景及可能的前景将变成1
            CvInvoke.cvAndS(mask.Ptr, new MCvScalar(1d), mask.Ptr, IntPtr.Zero);

            result = Img.Copy(mask);
            return result.Convert<Gray, Byte>();
        }

        /// <summary>
        /// 保存返回的人脸
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string SaveFace(Image<Gray, byte> Img, string name, int index)
        {

            string time = DateTime.Now.ToString("hh'-'mm'-'s-" + index, CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, "face");

            string imgPath = Path.Combine(myPhotos, name + "_" + index + "-" + time + ".png");
            //CvInvoke.Imwrite(imgPath, Img);
            CvInvoke.cvSaveImage(imgPath, Img, Img);
            return imgPath;
        }

        /// <summary>
        /// 保存模板的人脸
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string SaveTrainedFace(string KinethImgPath, string name)
        {
            int numOfFace;
            string resultPath = null;
            Image<Gray, Byte> faceROI;
            Rectangle[] faces;
            String face_cascade_name = "haarcascade_frontalface_alt.xml";
            CascadeClassifier face_cascade;
            Image<Bgr, byte> BgrobservedImage = new Image<Bgr, byte>(KinethImgPath);
            observedImage = new Image<Gray, byte>(KinethImgPath);
            {
                face_cascade = new CascadeClassifier(face_cascade_name);
                CvInvoke.cvEqualizeHist(observedImage, observedImage);
                faces = face_cascade.DetectMultiScale(observedImage, 1.3, 3, new Size(20, 20), Size.Empty);
                numOfFace = faces.Count();
                //if (numOfFace != 1)
                //{
                //   System.Console.WriteLine("人头数不为1");
               //    return null;
               // }
                //裁剪图片
                bool faceOK = true;
                int faceIndex = 0;

                Console.WriteLine("Finding faces " + (faceIndex + 1) + "of " + numOfFace);
                System.Drawing.Size roisize = new Size(faces[faceIndex].Width, faces[faceIndex].Height);

                System.Drawing.PointF[] pts = {
                    new System.Drawing.PointF(faces[faceIndex].Left, faces[faceIndex].Bottom),
                    new System.Drawing.PointF(faces[faceIndex].Right, faces[faceIndex].Bottom),
                    new System.Drawing.PointF(faces[faceIndex].Right, faces[faceIndex].Top),
                    new System.Drawing.PointF(faces[faceIndex].Left, faces[faceIndex].Top)};
                BgrobservedImage.DrawPolyline(Array.ConvertAll(pts, System.Drawing.Point.Round), true, new Bgr(Color.Blue), 5);
                IntPtr dst = CvInvoke.cvCreateImage(roisize, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                CvInvoke.cvSetImageROI(observedImage, faces[faceIndex]);
                faceROI = observedImage.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC).Clone();
                resultPath = SaveTrainedFace(faceROI, name);
                SaveResultImg(BgrobservedImage, "modle", 0);

            }
            try
            {
                resultPath = resultPath.Replace("trainedFacesWhoiswho", "trainedFaces");
            }
            catch { resultPath = null; }
            return resultPath;
        }
        public SurfResult[] FindFace(string KinethImgPath)
        {
            int numOfFace;
            SurfResult surfResult = new SurfResult();
            SurfResult[] finalResult;
            string resultPath;
            Image<Gray, Byte> faceROI;
            Rectangle[] faces;

            surfResult.isSuccess = false;

            String face_cascade_name = "haarcascade_frontalface_alt.xml";
            CascadeClassifier face_cascade;
            using (Image<Gray, Byte> observedImage = new Image<Gray, Byte>(KinethImgPath))
            {
                face_cascade = new CascadeClassifier(face_cascade_name);
                CvInvoke.cvEqualizeHist(observedImage, observedImage);
                faces = face_cascade.DetectMultiScale(observedImage, 1.3, 3, new Size(20, 20), Size.Empty);
                numOfFace = faces.Count();
                if (numOfFace == 0)
                {
                    finalResult = new SurfResult[1];
                    System.Console.WriteLine("人头数为:0");
                    surfResult.ImgPath = KinethImgPath;
                    finalResult[0] = surfResult;
                    return finalResult;
                }
                finalResult = new SurfResult[numOfFace];
                //裁剪图片
                for (int i = 0; i < numOfFace; i++)
                {
                    surfResult = new SurfResult();
                    System.Drawing.Size roisize = new Size(faces[i].Width, faces[i].Height);
                    IntPtr dst = CvInvoke.cvCreateImage(roisize, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                    CvInvoke.cvSetImageROI(observedImage, faces[i]);
                    faceROI = observedImage.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC).Clone();
                    resultPath = SaveFace(faceROI, "user", i);
                    surfResult.centerColorPoint.X = (faces[i].Left + faces[i].Right) / 2;
                    surfResult.centerColorPoint.Y = (faces[i].Top + faces[i].Bottom) / 2;
                    surfResult.ImgPath = resultPath;
                    surfResult.isSuccess = true;
                    finalResult[i] = surfResult;
                }
                System.Console.WriteLine("人头数为:{0}", numOfFace);
            }
            return finalResult;
        }
        public string SaveTrainedFace(Image<Gray, byte> Img, string name)
        {
            Image<Gray, byte> GrayFace = Img.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace._EqualizeHist();//得到均衡化人脸的灰度图像

            string time = DateTime.Now.ToString("hh'-'mm'-'s-" + index, CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, "trainedFaces");

            //string imgPath = Path.Combine(myPhotos, name + "_" + time + ".png");
            string imgPath = Path.Combine(myPhotos, name + "_" + System.Guid.NewGuid().ToString() + ".png");
            //CvInvoke.Imwrite(imgPath, Img);
            CvInvoke.cvSaveImage(imgPath, GrayFace, GrayFace);
            System.Console.WriteLine("成功保存");
            return imgPath;
        }
        public string SaveTrainedFaceforWhoisWho(Image<Gray, byte> Img, string name)
        {
            Image<Gray, byte> GrayFace = Img.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace._EqualizeHist();//得到均衡化人脸的灰度图像

            string time = DateTime.Now.ToString("hh'-'mm'-'s-" + index, CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, "trainedFacesWhoiswho");

            //string imgPath = Path.Combine(myPhotos, name + "_" + time + ".png");
            string imgPath = Path.Combine(myPhotos, name + "_" + System.Guid.NewGuid().ToString() + ".png");
            //CvInvoke.Imwrite(imgPath, Img);
            CvInvoke.cvSaveImage(imgPath, GrayFace, GrayFace);
            System.Console.WriteLine("成功保存");
            return imgPath;
        }
        /// <summary>
        /// 辨别人脸
        /// </summary>
        /// <returns></returns>
        public String FindFaceName(string KinethImgPath)
        {
            try
            {
                Image<Gray, byte> GrayFace = new Image<Gray, Byte>(KinethImgPath).Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                GrayFace._EqualizeHist();//得到均衡化人脸的灰度图像

                Emgu.CV.FaceRecognizer faceRecognizer;
                faceRecognizer = new Emgu.CV.EigenFaceRecognizer(80, double.PositiveInfinity);

                TrainedFileList tf = new TrainedFileList();
                DirectoryInfo di = new DirectoryInfo(PROGRAMPATH + "\\trainedFaces");
                int i = 0;
                foreach (FileInfo fi in di.GetFiles())
                {
                    tf.trainedImages.Add(new Image<Gray, byte>(fi.FullName));
                    tf.trainedLabelOrder.Add(i);
                    tf.trainedFileName.Add(fi.Name.Split('_')[0]);
                    i++;
                }
                faceRecognizer.Train(tf.trainedImages.ToArray(), tf.trainedLabelOrder.ToArray());

                Emgu.CV.FaceRecognizer.PredictionResult pr = faceRecognizer.Predict(GrayFace);
                string recogniseName = tf.trainedFileName[pr.Label].ToString();
                string name = tf.trainedFileName[pr.Label].ToString();
                System.Console.WriteLine("人物为：{0}", name);
                return name;
            }
            catch { name = null; return name; }
        }

        /// <summary>
        /// 辨别人脸
        /// </summary>
        /// <returns></returns>
        public String FindFaceName(Image<Gray, byte> tempImg)
        {
            Image<Gray, byte> GrayFace = tempImg.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace._EqualizeHist();//得到均衡化人脸的灰度图像

            Emgu.CV.FaceRecognizer faceRecognizer;
            faceRecognizer = new Emgu.CV.EigenFaceRecognizer(80, double.PositiveInfinity);

            TrainedFileList tf = new TrainedFileList();
            DirectoryInfo di = new DirectoryInfo(PROGRAMPATH + "\\trainedFaces");
            int i = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                tf.trainedImages.Add(new Image<Gray, byte>(fi.FullName));
                tf.trainedLabelOrder.Add(i);
                tf.trainedFileName.Add(fi.Name.Split('_')[0]);
                i++;
            }
            faceRecognizer.Train(tf.trainedImages.ToArray(), tf.trainedLabelOrder.ToArray());

            Emgu.CV.FaceRecognizer.PredictionResult pr = faceRecognizer.Predict(GrayFace);
            string recogniseName = tf.trainedFileName[pr.Label].ToString();
            string name = tf.trainedFileName[pr.Label].ToString();
            System.Console.WriteLine("人物为：{0}", name);
            return name;
        }

        private SurfResult FindObj(string objStrPath, int type)
        {
            SurfResult surfResult = new SurfResult();
            surfResult.isSuccess = false;
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            BitmapSource surfresBitmapSource;

            while (!kinectConnection.isKinectReady())
            {
                Console.WriteLine("waiting for Kinect");
                Thread.Sleep(100);
            }

            ColorSpacePoint resColorCenterPoint = new ColorSpacePoint();
            CameraSpacePoint resCameraCenterPoint = new CameraSpacePoint();
            CameraSpacePoint[] resCameraPoints = new CameraSpacePoint[4];
            //获取图像
            string KinethImgPath = kinectConnection.ScreenShot();

            while (string.IsNullOrWhiteSpace(KinethImgPath))
            {
                Thread.Sleep(500);
                KinethImgPath = kinectConnection.ScreenShot();
            }
            //Console.WriteLine("1 "+KinethImgPath+"  "+FileManager.getFileSize(KinethImgPath));
            while (FileManager.getFileSize(KinethImgPath) < 10)
            {
                //Console.WriteLine("2"+KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
                Thread.Sleep(500);
                KinethImgPath = kinectConnection.ScreenShot();

            }
            //Console.WriteLine("3" + KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
            string objPathStr = objStrPath;
            if (string.IsNullOrWhiteSpace(objPathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            string scenePathStr = KinethImgPath;
            if (string.IsNullOrWhiteSpace(scenePathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            long matchTime = 0;
            using (Image<Gray, Byte> modelImage = new Image<Gray, Byte>(objPathStr))
            using (Image<Gray, Byte> observedImage = new Image<Gray, Byte>(KinethImgPath))
            {
                Image<Bgr, byte> result = SurfFeature.Draw(modelImage, observedImage, out matchTime);
                surfresBitmapSource = BitmapSourceConvert.ToBitmapSource(result);


                string resString = String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime);
                resString += ("  find" + SurfFeature.matchPoints);
                surfResult.numOfGoodPoint = SurfFeature.matchPoints;

                //计算
                if (SurfFeature.matchPoints > 8)
                {
                    resColorCenterPoint.X = (SurfFeature.ObjectPts[0].X + SurfFeature.ObjectPts[1].X +
                                        SurfFeature.ObjectPts[2].X + SurfFeature.ObjectPts[3].X) / 4;
                    resColorCenterPoint.Y = (SurfFeature.ObjectPts[0].Y + SurfFeature.ObjectPts[1].Y +
                                        SurfFeature.ObjectPts[2].Y + SurfFeature.ObjectPts[3].Y) / 4;
                    surfResult.isSuccess = true;
                }
                resCameraCenterPoint = kinectConnection.GetCameraSpacePoint(resColorCenterPoint);
                surfResult.centerCameraPoint = resCameraCenterPoint;
                surfResult.centerColorPoint = resColorCenterPoint;

                //计算四个角的位置
                for (int i = 0; i < SurfFeature.ObjectPts.Count(); i++)
                {
                    surfResult.cornerColorPoints[i].X = SurfFeature.ObjectPts[i].X;
                    surfResult.cornerColorPoints[i].Y = SurfFeature.ObjectPts[i].Y;
                    surfResult.cornerCameraPoints[i] = kinectConnection.GetCameraSpacePoint(surfResult.cornerColorPoints[i]);
                }

                resString += "Color:" + resColorCenterPoint.X + "  " + resColorCenterPoint.Y;
                resString += "CAM:" + resCameraCenterPoint.X + "  " + resCameraCenterPoint.Y + "  " + resCameraCenterPoint.Z + "  using:" + matchTime + "ms";
                Console.WriteLine(resString);
                surfResult.resultStr = resString;
                surfResult.usingTime = matchTime;
                string resultPath = SaveResultImg(result, this.name, this.index);
                surfResult.ImgPath = resultPath;
                kinectConnection.ShowSurfImg(result, resString);
                modelImage.Dispose();
                observedImage.Dispose();
            }
            FileManager.deleteFile(scenePathStr);
            return surfResult;
        }

        //用物体分割代替上面的背景过滤
        private SurfResult FindObj2(string objStrPath, Rectangle rect, int count = 1)
        {
            SurfResult surfResult = new SurfResult();
            surfResult.isSuccess = false;
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            BitmapSource surfresBitmapSource;

            while (!kinectConnection.isKinectReady())
            {
                Console.WriteLine("waiting for Kinect");
                Thread.Sleep(100);
            }

            ColorSpacePoint resColorCenterPoint = new ColorSpacePoint();
            CameraSpacePoint resCameraCenterPoint = new CameraSpacePoint();
            CameraSpacePoint[] resCameraPoints = new CameraSpacePoint[4];
            //获取图像
            string KinethImgPath = kinectConnection.ScreenShot();

            while (string.IsNullOrWhiteSpace(KinethImgPath))
            {
                Thread.Sleep(500);
                KinethImgPath = kinectConnection.ScreenShot();
            }
            //Console.WriteLine("1 "+KinethImgPath+"  "+FileManager.getFileSize(KinethImgPath));
            while (FileManager.getFileSize(KinethImgPath) < 10)
            {
                //Console.WriteLine("2"+KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
                Thread.Sleep(500);
                KinethImgPath = kinectConnection.ScreenShot();

            }
            //Console.WriteLine("3" + KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
            string objPathStr = objStrPath;
            if (string.IsNullOrWhiteSpace(objPathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            string scenePathStr = KinethImgPath;
            if (string.IsNullOrWhiteSpace(scenePathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            long matchTime = 0;
            using (Image<Gray, Byte> observedImage = GrabcutGrayImg(KinethImgPath, rect, count))
            using (Image<Gray, Byte> modelImage = new Image<Gray, Byte>(objPathStr))
            {
                Image<Bgr, byte> result = SurfFeature.Draw(modelImage, observedImage, out matchTime);
                surfresBitmapSource = BitmapSourceConvert.ToBitmapSource(result);


                string resString = String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime);
                resString += ("  find" + SurfFeature.matchPoints);
                surfResult.numOfGoodPoint = SurfFeature.matchPoints;

                //计算
                if (SurfFeature.matchPoints > 8)
                {
                    resColorCenterPoint.X = (SurfFeature.ObjectPts[0].X + SurfFeature.ObjectPts[1].X +
                                        SurfFeature.ObjectPts[2].X + SurfFeature.ObjectPts[3].X) / 4;
                    resColorCenterPoint.Y = (SurfFeature.ObjectPts[0].Y + SurfFeature.ObjectPts[1].Y +
                                        SurfFeature.ObjectPts[2].Y + SurfFeature.ObjectPts[3].Y) / 4;
                    surfResult.isSuccess = true;
                }
                resCameraCenterPoint = kinectConnection.GetCameraSpacePoint(resColorCenterPoint);
                surfResult.centerCameraPoint = resCameraCenterPoint;
                surfResult.centerColorPoint = resColorCenterPoint;

                //计算四个角的位置
                for (int i = 0; i < SurfFeature.ObjectPts.Count(); i++)
                {
                    surfResult.cornerColorPoints[i].X = SurfFeature.ObjectPts[i].X;
                    surfResult.cornerColorPoints[i].Y = SurfFeature.ObjectPts[i].Y;
                    surfResult.cornerCameraPoints[i] = kinectConnection.GetCameraSpacePoint(surfResult.cornerColorPoints[i]);
                }

                resString += "Color:" + resColorCenterPoint.X + "  " + resColorCenterPoint.Y;
                resString += "CAM:" + resCameraCenterPoint.X + "  " + resCameraCenterPoint.Y + "  " + resCameraCenterPoint.Z + "  using:" + matchTime + "ms";
                Console.WriteLine(resString);
                surfResult.resultStr = resString;
                surfResult.usingTime = matchTime;
                string resultPath = SaveResultImg(result, this.name, this.index);
                surfResult.ImgPath = resultPath;
                kinectConnection.ShowSurfImg(result, resString);
                modelImage.Dispose();
                observedImage.Dispose();
            }
            FileManager.deleteFile(scenePathStr);
            return surfResult;
        }
        private SurfResult FindObjwithBGremove(string objStrPath, int type)
        {
            SurfResult surfResult = new SurfResult();
            surfResult.isSuccess = false;
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            BitmapSource surfresBitmapSource;

            while (!kinectConnection.isKinectReady())
            {
                Console.WriteLine("waiting for Kinect");
                Thread.Sleep(100);
            }

            ColorSpacePoint resColorCenterPoint = new ColorSpacePoint();
            CameraSpacePoint resCameraCenterPoint = new CameraSpacePoint();
            CameraSpacePoint[] resCameraPoints = new CameraSpacePoint[4];
            //获取图像
            string KinethImgPath = kinectConnection.ScreenShot();

            while (string.IsNullOrWhiteSpace(KinethImgPath))
            {
                Thread.Sleep(200);
                KinethImgPath = kinectConnection.ScreenShot();
            }
            //Console.WriteLine("1 "+KinethImgPath+"  "+FileManager.getFileSize(KinethImgPath));
            while (FileManager.getFileSize(KinethImgPath) < 500)
            {
                //Console.WriteLine("2"+KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
                Thread.Sleep(500);
                KinethImgPath = kinectConnection.ScreenShot();

            }
            //Console.WriteLine("3" + KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
            string objPathStr = objStrPath;
            if (string.IsNullOrWhiteSpace(objPathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            string scenePathStr = KinethImgPath;
            if (string.IsNullOrWhiteSpace(scenePathStr))
            {
                surfResult.isSuccess = false;
                return surfResult;
            }

            long matchTime = 0;
            //Console.WriteLine("4" + KinethImgPath + "  " + FileManager.getFileSize(KinethImgPath));
            using (Image<Gray, Byte> modelImage = new Image<Gray, Byte>(objPathStr))
            using (Image<Gray, Byte> observedImage = new Image<Gray, Byte>(KinethImgPath))

            {
                Image<Bgr, byte> result = SurfFeature.Draw(modelImage, observedImage, out matchTime);
                surfresBitmapSource = BitmapSourceConvert.ToBitmapSource(result);


                string resString = String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime);
                resString += ("  find" + SurfFeature.matchPoints);
                surfResult.numOfGoodPoint = SurfFeature.matchPoints;

                //计算
                if (SurfFeature.matchPoints > 8)
                {
                    resColorCenterPoint.X = (SurfFeature.ObjectPts[0].X + SurfFeature.ObjectPts[1].X +
                                        SurfFeature.ObjectPts[2].X + SurfFeature.ObjectPts[3].X) / 4;
                    resColorCenterPoint.Y = (SurfFeature.ObjectPts[0].Y + SurfFeature.ObjectPts[1].Y +
                                        SurfFeature.ObjectPts[2].Y + SurfFeature.ObjectPts[3].Y) / 4;
                    surfResult.isSuccess = true;
                }
                resCameraCenterPoint = kinectConnection.GetCameraSpacePoint(resColorCenterPoint);
                surfResult.centerCameraPoint = resCameraCenterPoint;
                surfResult.centerColorPoint = resColorCenterPoint;

                //计算四个角的位置
                for (int i = 0; i < SurfFeature.ObjectPts.Count(); i++)
                {
                    surfResult.cornerColorPoints[i].X = SurfFeature.ObjectPts[i].X;
                    surfResult.cornerColorPoints[i].Y = SurfFeature.ObjectPts[i].Y;
                    surfResult.cornerCameraPoints[i] = kinectConnection.GetCameraSpacePoint(surfResult.cornerColorPoints[i]);
                }

                resString += "Color:" + resColorCenterPoint.X + "  " + resColorCenterPoint.Y;
                resString += "CAM:" + resCameraCenterPoint.X + "  " + resCameraCenterPoint.Y + "  " + resCameraCenterPoint.Z + "  using:" + matchTime + "ms";
                Console.WriteLine(resString);
                surfResult.resultStr = resString;
                surfResult.usingTime = matchTime;
                string resultPath = SaveResultImg(result, this.name, this.index);
                surfResult.ImgPath = resultPath;
                kinectConnection.ShowSurfImg(result, resString);
                modelImage.Dispose();
                observedImage.Dispose();
            }
            FileManager.deleteFile(scenePathStr);
            return surfResult;
        }



        public void setSurfDistance(float dis)
        {
            KinectConnection.GetInstance().setSurfDistance(dis);
        }

        public List<Person> getCorrentPersons()
        {
            List<Person> persons = new List<Person>(3);
            return persons;
        }

        public CameraSpacePoint getCenterCameraSpacePoint()
        {
            startCoordinateMapping();
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            ColorSpacePoint colorSpacePoint = new ColorSpacePoint();
            colorSpacePoint.Y = 540;
            colorSpacePoint.X = 960;
            CameraSpacePoint resPoint = kinectConnection.GetCameraSpacePoint(colorSpacePoint);
            endCoordinateMapping();
            return resPoint;
        }

        public ColorSpacePoint MapCamera2Color(CameraSpacePoint cameraSpacePoint)
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            ColorSpacePoint colorSpacePoint = kinectConnection.GetColorSpacePoint(cameraSpacePoint);
            return colorSpacePoint;
        }

        private bool bodyStartFlag = false;

        public void startUserTracker()
        {
            //if (!bodyStartFlag)
            {
                KinectConnection kinectConnection = KinectConnection.GetInstance();
                kinectConnection.startBodyDetect();
                kinectConnection.getBodyPublisher().FrameArrived += Body_FrameArrived;
                bodyStartFlag = true;
            }

        }
        /// <summary>
        /// 从场景中获得所有人的信息
        /// </summary>
        /// <returns></returns>
        public List<User> getAllusers()
        {

            List<User> correntUsers = new List<User>();
            while (true)
            {
                Thread.Sleep(500);
                if (dataReceived)
                {
                    while (isBodyReady)
                    {
                        bool usedFlag = false;
                        foreach (var body in correntBodies)
                        {
                            User user = new User();
                            user.body = body;
                            user.sync();
                            user.trackingHand();
                            user.trackingHeight();
                            correntUsers.Add(user);
                            usedFlag = true;
                        }
                        if (usedFlag)
                        {
                            break;
                        }
                    }
                    break;
                }

            }

            return UserTracker.users;
        }

        public User findUserSex(User u)
        {
            startCoordinateMapping();
            CameraSpacePoint neck = u.body.Joints[JointType.Neck].Position;
            CameraSpacePoint NeckReal = KinectConnection.GetInstance().GetCameraSpacePoint(KinectConnection.GetInstance().GetColorSpacePoint(neck));
            CameraSpacePoint SpineMid = u.body.Joints[JointType.SpineMid].Position;
            SpineMid.Y += 0.1f;
            CameraSpacePoint breastMidReal = KinectConnection.GetInstance().GetCameraSpacePoint(KinectConnection.GetInstance().GetColorSpacePoint(SpineMid));
            endCoordinateMapping();
            u.breastsize = NeckReal.Z - breastMidReal.Z;
            if (u.breastsize > 0.10)
            {
                u.sex = Sex.Female;
            }
            else if (u.breastsize > 0)
            {
                u.sex = Sex.Male;
            }
            else
            {
                u.sex = Sex.UnKnown;
            }
            return u;
        }



        /// <summary>
        /// 信号 body是否被准备好
        /// </summary>
        private bool isBodyReady = false;
        /// <summary>
        /// 信号 body是否被使用
        /// </summary>
        private bool isBodyUsed = true;
        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies;

        private List<Body> correntBodies = new List<Body>();


        static bool dataReceived = false;

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Body_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            dataReceived = false;
            isBodyReady = false;
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }
            }
            List<Body> bodiesList = new List<Body>();
            if (dataReceived)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        bodiesList.Add(body);
                    }
                }
            }

            correntBodies = bodiesList;
            isBodyReady = true;




        }

        public static void startMachineLearningMapping()
        {
            KinectConnection.GetInstance().startMachineLearningCoordinateMapping();
        }

        public static void endMachineLearningMapping()
        {
            WindowCtrl.EndMachineLearningMapping();
        }

        public static string screenShotForMachineLearning()
        {
            return KinectConnection.GetInstance().ScreenShotForMachineLearning();
        }



        public static void startCoordinateMapping()
        {
            KinectConnection.GetInstance().startCoordinateMapping();
        }

        public static void endCoordinateMapping()
        {
            KinectConnection.GetInstance().endCoordinateMapping();
        }

        public string getKinectImage()
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            string KinethImgPath = kinectConnection.ScreenShot();
            while (string.IsNullOrWhiteSpace(KinethImgPath))
            {
                Thread.Sleep(200);
                KinethImgPath = kinectConnection.ScreenShot();

            }
            while (FileManager.getFileSize(KinethImgPath) < 500)
            {
                Thread.Sleep(200);
                KinethImgPath = kinectConnection.ScreenShot();
            }
            return KinethImgPath;
        }

        public void SetCamshiftBodyPoint(User user)
        {
            ColorSpacePoint[] bodyPoint = new ColorSpacePoint[2];
            bodyPoint[0] = KinectProcesser.GetInstance().CameraToColor(user.body.Joints[JointType.ShoulderLeft].Position);
            bodyPoint[1] = KinectProcesser.GetInstance().CameraToColor(user.body.Joints[JointType.FootRight].Position);
            KinectConnection.GetInstance().SendBodyPoints(bodyPoint);
        }

        public ColorSpacePoint GetCamshiftRes()
        {
            ColorSpacePoint[] bodyPoints = KinectConnection.GetInstance().GetResultPoints();
            ColorSpacePoint centerPoint = new ColorSpacePoint();
            centerPoint.X = (bodyPoints[0].X + bodyPoints[1].X) / 2;
            centerPoint.Y = (bodyPoints[0].Y + bodyPoints[1].Y) / 2;
            return centerPoint;
        }

        public void FlashCamshift()
        {
            KinectConnection.GetInstance().flashCamShift();
        }

        public void readyVisionHub()
        {
            KinectConnection.GetInstance().initConnection();

        }
        public void stopVisionHub()
        {
            KinectConnection.GetInstance().Tcp_exit();

        }
        public string GetOcrWords()
        {
            KinectConnection.GetInstance().StartOCR();
            string s = KinectConnection.GetInstance().readOCR();
            return s;
        }
        public string GetFaceNum()
        {
            string s = KinectConnection.GetInstance().startface();
            return s;
        }

        public void SetDepthReaderRobotInfo(float KINECT_HIGHT, float TOTAL_HIGHT, float SAFE_DISTANCE)
        {
            KinectConnection.GetInstance().SetDepthReaderRobotInfo(KINECT_HIGHT, TOTAL_HIGHT, SAFE_DISTANCE);

        }

        public void startDepthReader()
        {
            KinectConnection kinectConnection = KinectConnection.GetInstance();
            kinectConnection.startDepthReader();
        }

        public int getUnSafeCount(int xrange)
        {
            return KinectConnection.GetInstance().GetDepthReaderUnSafeCount();
        }

        public void FaceRecognitionTrain(string name)
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            sitRobotHub.FaceRecognitionTrain(name);
        }

        public string FaceRecognition()
        {
            SitRobotHub sitRobotHub = new SitRobotHub(SitRobotConfig.getHubAddress(), SitRobotConfig.getHubPort());
            return sitRobotHub.FaceRecognition();
        }
        /// <summary>
        /// 解析json文件，通过物体的名字找到物体
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public MachineLearingResult FindObjectMachineLearing(string thing)
        {
            float confideence = 0;

            CameraSpacePoint thingPoit = new CameraSpacePoint();

            VisionCtrl.startCoordinateMapping();
            string photoPath = VisionCtrl.screenShotForMachineLearning();
            Console.WriteLine("photoPath:" + photoPath);
            ChangePath(photoPath, "Y:\\0.jpg");
            string receiveStr = MachineLearningConnection.StartAndGetResult();
            Console.WriteLine("OBJ JSON:" + receiveStr);
            JObject obj = JObject.Parse(receiveStr);
            MachineLearingResult machineLearingresult = new MachineLearingResult(obj.Count);

            for (int i = 1; i <= obj.Count; i++)
            {

                if (obj[i.ToString()][thing] != null)
                {
                    string result = (string)obj[i.ToString()][thing];
                    string[] splitResult = result.Split(new char[] { 'a' });
                    ColorSpacePoint resColorCenterPoint = new ColorSpacePoint();
                    //  resColorCenterPoint.X = float.Parse(splitResult[0]);
                    // resColorCenterPoint.Y = float.Parse(splitResult[1]);
                    // thingPoit = KinectConnection.GetInstance().GetCameraSpacePoint(resColorCenterPoint);
                    thingPoit = KinectConnection.GetInstance().GetCameraSpacePointForMachineLearning(float.Parse(splitResult[0]), float.Parse(splitResult[1]));
                    confideence = float.Parse(splitResult[2]);
                    machineLearingresult.CameraSpacePoint[i - 1] = thingPoit;
                    machineLearingresult.Confidence[i - 1] = confideence;


                }

            }
            if (machineLearingresult.ThingsNumber != 0)
            {
                return machineLearingresult;

            }
            else
            {
                throw new Exception("未找到该物体");
            }





        }

        /// <summary>
        /// 转换图片格式和路径
        /// </summary>
        /// <param name="Img">源图片</param>
        /// <param name="name">目标图片</param>
        /// <returns>图片保存后的路径</returns>
        public void ChangePath(string Imgpath, string name)
        {

            Image<Bgr, byte> Img = new Image<Bgr, byte>(Imgpath);
            string myPhotos = Environment.CurrentDirectory;
            myPhotos = Path.Combine(myPhotos, name);

            CvInvoke.cvSaveImage(name, Img, Img);

        }


        /// <summary>
        /// 截屏并保存到指定的位置，用于给深度学习识别。
        /// </summary>

    }
}
