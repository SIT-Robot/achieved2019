using System;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Emgu.CV.Util;

#if !IOS

#endif

//http://yy-programer.blogspot.com/2013/06/emgucv-image-process-detecting-and.html

namespace SITRobotSystem_wpf.BLL.Service
{
    /// <summary>
    /// SURF算法
    /// </summary>
    class SurfFeature
    {

        /// <summary>
        /// 寻找匹配点
        /// </summary>
        /// <param name="modelImage">模型图像</param>
        /// <param name="observedImage">场景图像</param>
        /// <param name="matchTime">匹配时间</param>
        /// <param name="modelKeyPoints">模型的特征点</param>
        /// <param name="observedKeyPoints">场景特征点</param>
        /// <param name="indices">索引</param>
        /// <param name="mask">mask？</param>
        /// <param name="homography">单应性</param>
        public static void FindMatch(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, out Matrix<int> indices, out Matrix<byte> mask, out HomographyMatrix homography)
        {

            int k = 2;
            //特诊阈值
            double uniquenessThreshold = 0.8;
            //SURF特征检测
            SURFDetector surfCPU = new SURFDetector(300, false);
            //运行时间测量
            Stopwatch watch;
            //单应性初始化
            homography = null;
#if !IOS
            //如果GPU可用
            if (GpuInvoke.HasCuda&&false )
            {
                //使用GPU的SURF特征检测,参数频率作用未知
                GpuSURFDetector surfGPU = new GpuSURFDetector(surfCPU.SURFParams, 0.01f);

                using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(modelImage))

                //extract features from the object image
                //GPU运算的特征点
                using (GpuMat<float> gpuModelKeyPoints = surfGPU.DetectKeyPointsRaw(gpuModelImage, null))
                //物体描述符？
                using (GpuMat<float> gpuModelDescriptors = surfGPU.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
                //蛮力匹配？
                using (GpuBruteForceMatcher<float> matcher = new GpuBruteForceMatcher<float>(DistanceType.L2))
                {
                    modelKeyPoints = new VectorOfKeyPoint();
                    surfGPU.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
                    watch = Stopwatch.StartNew();

                    // extract features from the observed image
                    using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray, byte>(observedImage))
                    using (GpuMat<float> gpuObservedKeyPoints = surfGPU.DetectKeyPointsRaw(gpuObservedImage, null))
                    using (GpuMat<float> gpuObservedDescriptors = surfGPU.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
                    using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
                    using (Stream stream = new Stream())
                    {
                        matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                        indices = new Matrix<int>(gpuMatchIndices.Size);
                        mask = new Matrix<byte>(gpuMask.Size);

                        //gpu implementation of voteForUniquess
                        using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                        using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                        {
                            GpuInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                            GpuInvoke.Compare(col0, col1, gpuMask, CMP_TYPE.CV_CMP_LE, stream);
                        }

                        observedKeyPoints = new VectorOfKeyPoint();
                        surfGPU.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                        //wait for the stream to complete its tasks
                        //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                        stream.WaitForCompletion();

                        gpuMask.Download(mask);
                        gpuMatchIndices.Download(indices);

                        if (GpuInvoke.CountNonZero(gpuMask) >= 4)
                        {
                            int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                            if (nonZeroCount >= 4)
                            {
                                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                                //改动
                                matchPoints = nonZeroCount;
                            }

                        }

                        watch.Stop();
                    }
                }
            }
            else
#endif
            {
                //extract features from the object image
                modelKeyPoints = new VectorOfKeyPoint();
                Matrix<float> modelDescriptors = surfCPU.DetectAndCompute(modelImage, null, modelKeyPoints);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                observedKeyPoints = new VectorOfKeyPoint();
                Matrix<float> observedDescriptors = surfCPU.DetectAndCompute(observedImage, null, observedKeyPoints);
                BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
                matcher.Add(modelDescriptors);

                indices = new Matrix<int>(observedDescriptors.Rows, k);
                using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
                {
                    matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                    mask = new Matrix<byte>(dist.Rows, 1);
                    mask.SetValue(255);
                    Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
                }

                int nonZeroCount = CvInvoke.cvCountNonZero(mask);
                if (nonZeroCount >= 4)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                    if (nonZeroCount >= 4)
                    {
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                        matchPoints = nonZeroCount;
                    }
                }

                watch.Stop();
            }
            matchTime = watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        public static Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
        {
            //单应性：是几何中的一个概念。单应性是一个从实射影平面到射影平面的可逆变换，直线在该变换下仍映射为直线。
            HomographyMatrix homography;
            //模型的特征点
            VectorOfKeyPoint modelKeyPoints;
            //场景的特征点
            VectorOfKeyPoint observedKeyPoints;
            //索引
            Matrix<int> indices;
            //mask？蒙板？
            Matrix<byte> mask;

            //匹配过程
            FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography);

            //Draw the matched keypoints
            Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
               indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

            #region draw the projected region on the image
            if (homography != null)
            {  //draw a rectangle along the projected model
                Rectangle rect = modelImage.ROI;
                PointF[] pts = { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
                homography.ProjectPoints(pts);
                ObjectPts = pts;
                result.DrawPolyline(Array.ConvertAll(pts, Point.Round), true, new Bgr(Color.Blue), 5);
                int centerX =(int) (ObjectPts[0].X + ObjectPts[1].X +
                    ObjectPts[2].X + ObjectPts[3].X) / 4;
                int centerY =(int) (ObjectPts[0].Y + ObjectPts[1].Y +
                                    ObjectPts[2].Y + ObjectPts[3].Y) / 4; ;
                
                Ellipse ellipse=new Ellipse(new PointF(centerX,centerY),new SizeF(5,5), 0);
                result.Draw(ellipse,new Bgr(255,0,0),5);
            }
            #endregion
            return result;
        }

        public static int matchPoints;
        public static int goodMatchPoints;
        public static PointF[] ObjectPts;
    }
}
