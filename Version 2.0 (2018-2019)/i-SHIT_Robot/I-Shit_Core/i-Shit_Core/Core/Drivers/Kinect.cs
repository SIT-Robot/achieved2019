using System;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Threading;
using System.Collections.Generic;

namespace i_Shit_Core.Core.Drivers
{

    public static partial class Driver
    {
        //用来给外面的（其它线程的）操作Kinect用的
        internal static bool KinectShotFlag = false;
        internal static string KinectShotImgPath = null;
        internal static CameraSpacePoint[] cameraPointsFinalResult;
        internal static bool CoordinateMappingReadyFlag = false;
        internal static bool EnableCoordinateMappingFlag = false;
        //--用来给外面的（其它线程的）操作Kinect用的

        //Kinect RGB&Depth Camera分辨率信息
        internal static int depthWidth;
        internal static int depthHeight;
        internal static int colorWidth;
        internal static int colorHeight;
        //--Kinect RGB&Depth Camera分辨率信息

        internal static KinectSensor kinectSensor;
        internal static WriteableBitmap colorBitmap;
        internal static ColorFrameReader colorFrameReader;

        //For Depth and CoordinateMap(Depth&RGB = MultiFrame)
        private static MultiSourceFrameReader multiFrameSourceReader;
        private static CoordinateMapper coordinateMapper;
        private static DepthSpacePoint[] colorMappedToDepthPoints;
        private static CameraSpacePoint[] cameraPoints;
        private static uint bitmapBackBufferSize;
        //--For Depth and CoordinateMap(Depth&RGB = MultiFrame)


        //BodyDetect部分用的
        internal static bool Kinect_BodyDetect_DataReceived = false;//kinect数据是否收到；
        internal static bool Kinect_BodyDetect_isBodyReady = false;
        internal static List<Body> Kinect_BodyDetect_CorrentBodies = new List<Body>();
        private static Body[] bodies;//Body_FrameArrived用来存很多很多的身体
        internal static BodyDetectWindow bodyDetectWindow;

        //--BodyDetect部分用的


        /// <summary>
        /// 初始化Kinect，同时Init RGB & Depth Camera
        /// </summary>
        public static void Kinect_InitKinect()
        {
            if (kinectSensor == null)
            {
                kinectSensor = KinectSensor.GetDefault();

                // Init Kinect RGB Camera
                // open the reader for the color frames
                colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();

                // wire handler for frame arrival
                colorFrameReader.FrameArrived += Kinect_ColorFrameReader_FrameArrived;

                // create the colorFrameDescription from the ColorFrameSource using Bgra format
                FrameDescription colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

                // create the bitmap to display
                colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

                // open the sensor

                //--Init Kinect RGB Camera



                //Init Kinect Depth Camera (Depth&RGB=multiFrame)
                multiFrameSourceReader =
                kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);

                //添加事件
                multiFrameSourceReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                //this.multiFrameSourceReader.MultiSourceFrameArrived += Reader_MultiSourceFrameColorArrived;

                //mapper
                coordinateMapper = kinectSensor.CoordinateMapper;


                FrameDescription depthFrameDescription = kinectSensor.DepthFrameSource.FrameDescription;

                depthWidth = depthFrameDescription.Width;
                depthHeight = depthFrameDescription.Height;

                //FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;
                colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

                colorWidth = colorFrameDescription.Width;
                colorHeight = colorFrameDescription.Height;

                colorMappedToDepthPoints = new DepthSpacePoint[colorWidth * colorHeight];
                cameraPointsFinalResult = new CameraSpacePoint[colorWidth * colorHeight];
                cameraPoints = new CameraSpacePoint[colorWidth * colorHeight];
                //cameraPointsCpy = new CameraSpacePoint[colorWidth * colorHeight];
                //Bitmap = new WriteableBitmap(colorWidth, colorHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

                // Calculate the WriteableBitmap back buffer size
                bitmapBackBufferSize =
                    (uint)
                        ((colorBitmap.BackBufferStride * (colorBitmap.PixelHeight - 1)) +
                         (colorBitmap.PixelWidth * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8));
                //--Init Kinect Depth Camera (Depth&RGB=multiFrame)
                bodyDetectWindow = new BodyDetectWindow();
                // bodyDetectWindow.Show();

                bodyDetectWindow.bodyFrameReader.FrameArrived += Kinect_Body_FrameArrived;
                //Init BodyDetect


                //--Init BodyDetect

                kinectSensor.Open();
                kinectSensor.IsAvailableChanged += delegate
                {
                    Console.WriteLine(kinectSensor.IsAvailable ? "Kinect Connected." : "Kinect Not Connected.");
                };
                Console.WriteLine("Kinect Inited.");


            }
        }

        /// <summary>
        /// DEPTH&RGB相机FrameArrived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            CoordinateMappingReadyFlag = false;
            DepthFrame depthFrame = null;
            ColorFrame colorFrame = null;
            bool isBitmapLocked = false;

            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();

            // If the Frame has expired by the time we process this event, return.
            if (multiSourceFrame == null)
            {
                return;
            }

            // We use a try/finally to ensure that we clean up before we exit the function.  
            // This includes calling Dispose on any Frame objects that we may have and unlocking the bitmap back buffer.
            try
            {

                depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame();
                colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame();


                // If any frame has expired by the time we process this event, return.
                // The "finally" statement will Dispose any that are not null.
                if ((depthFrame == null) || (colorFrame == null))
                {
                    return;
                }

                if (EnableCoordinateMappingFlag)
                {

                    // Access the depth frame data directly via LockImageBuffer to avoid making a copy
                    using (KinectBuffer depthFrameData = depthFrame.LockImageBuffer())
                    {
                        coordinateMapper.MapColorFrameToCameraSpaceUsingIntPtr(depthFrameData.UnderlyingBuffer,
                            depthFrameData.Size,
                            cameraPoints);

                        coordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(
                            depthFrameData.UnderlyingBuffer,
                            depthFrameData.Size,
                            colorMappedToDepthPoints);

                    }
                    cameraPoints.CopyTo(cameraPointsFinalResult, 0);
                    CoordinateMappingReadyFlag = true;
                }

                //retrieve the color to space mapping of the current pixel
                //resCameraSpacePoint =
                //    this.cameraPoints[(int)findColorSpacePoint.Y * colorWidth + (int)findColorSpacePoint.X];
                //Console.WriteLine(resCameraSpacePoint.X.ToString() + " " + resCameraSpacePoint.Y.ToString() + " " +
                //                    resCameraSpacePoint.Z.ToString());







                // Process Color

                // Lock the bitmap for writing

                colorBitmap.Lock();
                isBitmapLocked = true;

                depthFrame.Dispose();
                depthFrame = null;
                // We're done with the ColorFrame 
                colorFrame.Dispose();
                colorFrame = null;
            }
            finally
            {

                if (isBitmapLocked)
                {
                    colorBitmap.Unlock();
                }



            }

        }


        /// <summary>
        /// RGB相机FrameArrived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Kinect_ColorFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        colorBitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == colorBitmap.PixelWidth) && (colorFrameDescription.Height == colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            colorBitmap.AddDirtyRect(new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight));
                        }

                        colorBitmap.Unlock();
                    }
                }
            }


            if (KinectShotFlag == true)
            {

                if (kinectSensor != null)
                {
                    if (colorBitmap != null)
                    {
                        // create a png bitmap encoder which knows how to save a .png file
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        // create frame from the writable bitmap and add to encoder
                        encoder.Frames.Add(BitmapFrame.Create(colorBitmap));
                        string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
                        KinectShotImgPath = @"..\Data\KinectShotImages\" + "KinectScreenshot-Color-" + time + ".png";
                        KinectShotImgPath = Path.GetFullPath(KinectShotImgPath);
                        // write the new file to disk
                        try
                        {
                            // FileStream is IDisposable
                            using (FileStream fs = new FileStream(KinectShotImgPath, FileMode.Create))
                            {
                                encoder.Save(fs);
                            }
                            Console.WriteLine("Kinect拍照成功，路径:" + KinectShotImgPath);
                        }
                        catch (IOException)
                        { Console.WriteLine("Error:Kinect拍照失败！路径" + KinectShotImgPath); }

                    }
                    else
                    {
                        Console.WriteLine("Error:Kinect拍照失败！colorBitmap为空(==null)");

                    }
                }
                else
                {
                    Console.WriteLine("Error:Kinect拍照失败！Kinect未初始化！请在Init Script里把isInitKinect设为true");

                }
                KinectShotFlag = false;
            }
        }

        /// <summary>
        /// Kinect BodyFrame Arrived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Kinect_Body_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            Kinect_BodyDetect_DataReceived = false;
            Kinect_BodyDetect_isBodyReady = false;
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
                    Kinect_BodyDetect_DataReceived = true;
                }
            }
            List<Body> bodiesList = new List<Body>();
            if (Kinect_BodyDetect_DataReceived)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        bodiesList.Add(body);
                    }
                }
            }

            Kinect_BodyDetect_CorrentBodies = bodiesList;
            Kinect_BodyDetect_isBodyReady = true;
        }


    }
}
