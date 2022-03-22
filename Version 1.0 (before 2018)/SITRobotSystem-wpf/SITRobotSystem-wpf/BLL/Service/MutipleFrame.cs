using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;


namespace SITRobotSystem_wpf.BLL.Service
{



    /// <summary>
    /// 包含多种数据流的
    /// </summary>
    public class MutipleFrame
    {

        public bool coordinateMappingFlag = false;
        public bool coordinateMappingReadyFlag = false;

        /// <summary>
        /// 是否需要背景过滤
        /// </summary>
        public bool NeedBackGroundClear = false;
        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        public MultiSourceFrameReader multiFrameSourceReader;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap;

        /// <summary>
        /// The size in bytes of the bitmap back buffer
        /// </summary>
        private uint bitmapBackBufferSize;

        /// <summary>
        /// Intermediate storage for the color to depth mapping
        /// </summary>
        private DepthSpacePoint[] colorMappedToDepthPoints;

        /// Intermediate storage for color to camera mapping
        /// 
        private CameraSpacePoint[] cameraPoints;

        private CameraSpacePoint[] cameraPointsCpy;
        private int depthWidth;

        private int depthHeight;

        private int colorWidth;

        private int colorHeight;

        private ColorSpacePoint findColorSpacePoint = new ColorSpacePoint();

        private bool isFindRequest = false;

        private bool isCompleteRequest = false;

        private CameraSpacePoint resCameraSpacePoint = new CameraSpacePoint();
        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText;


        private FrameDescription colorFrameDescription;
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MutipleFrame()
        {
            //初始化
            kinectSensor = KinectSensor.GetDefault();

            //多重source读取器
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

            cameraPoints = new CameraSpacePoint[colorWidth * colorHeight];
            cameraPointsCpy = new CameraSpacePoint[colorWidth * colorHeight];
            Bitmap = new WriteableBitmap(colorWidth, colorHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

            // Calculate the WriteableBitmap back buffer size
            bitmapBackBufferSize =
                (uint)
                    ((Bitmap.BackBufferStride * (Bitmap.PixelHeight - 1)) +
                     (Bitmap.PixelWidth * bytesPerPixel));

            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            BRtool = new BackgroundRemovalTool(kinectSensor.CoordinateMapper);

            kinectSensor.Open();

            StatusText = kinectSensor.IsAvailable
                ? "running"
                : "stop";
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public bool isKinectReady;

        public bool KinectReady()
        {
            return isKinectReady;
        }
        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get { return Bitmap; }
        }

        private BitmapSource cvBitmap;
        public BitmapSource GetBitmapSource()
        {
            return cvBitmap;
        }

        private Image<Bgra, byte> cvImg;
        public Image<Bgra, byte> GetcvImg()
        {
            return cvImg;
        }


        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get { return statusText; }

            set
            {
                if (statusText != value)
                {
                    statusText = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        public WriteableBitmap Bitmap
        {
            get
            {
                return bitmap;
            }

            set
            {
                bitmap = value;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (multiFrameSourceReader != null)
            {
                // MultiSourceFrameReder is IDisposable
                multiFrameSourceReader.Dispose();
                multiFrameSourceReader = null;
            }

            if (kinectSensor != null)
            {
                kinectSensor.Close();
                kinectSensor = null;
            }
        }

        public void stop()
        {
            if (multiFrameSourceReader != null)
            {
                // MultiSourceFrameReder is IDisposable
                multiFrameSourceReader.Dispose();
                multiFrameSourceReader = null;
            }

            if (kinectSensor != null)
            {
                kinectSensor.Close();
                kinectSensor = null;
            }
        }

        private BackgroundRemovalTool BRtool;

        public void setDistance(float dis)
        {
            this.BRtool.Distance = dis;
        }
        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            isKinectReady = true;
            coordinateMappingReadyFlag = false;
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

                if (coordinateMappingFlag)
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
                    cameraPoints.CopyTo(cameraPointsCpy, 0);
                    coordinateMappingReadyFlag = true;
                }

                //retrieve the color to space mapping of the current pixel
                //resCameraSpacePoint =
                //    this.cameraPoints[(int)findColorSpacePoint.Y * colorWidth + (int)findColorSpacePoint.X];
                //Console.WriteLine(resCameraSpacePoint.X.ToString() + " " + resCameraSpacePoint.Y.ToString() + " " +
                //                    resCameraSpacePoint.Z.ToString());







                // Process Color

                // Lock the bitmap for writing

                Bitmap.Lock();
                isBitmapLocked = true;

                colorFrame.CopyConvertedFrameDataToIntPtr(Bitmap.BackBuffer, bitmapBackBufferSize,
                    ColorImageFormat.Bgra);

                if (NeedBackGroundClear)
                {
                    Byte[] displayPixels = BRtool.GreenScreen(colorFrame, depthFrame);
                    Marshal.Copy(displayPixels, 0, Bitmap.BackBuffer, displayPixels.Length);
                }
                //bitmap.Lock();

                /*
                //Image<Bgra, byte> CVimg=ToImage(colorFrame);
                this.pixels = new byte[colorFrameDescription.Width * colorFrameDescription.Height * colorFrameDescription.BytesPerPixel];

                using (KinectBuffer ColorFrameData = colorFrame.LockRawImageBuffer())
                {
                    
                    if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        colorFrame.CopyRawFrameDataToArray(pixels);
                    else
                        colorFrame.CopyConvertedFrameDataToArray(this.pixels, ColorImageFormat.Bgra);

                    //Initialize Emgu CV image then assign byte array of pixels to it
                    cvImg = new Image<Bgra, byte>(colorFrameDescription.Width, colorFrameDescription.Height);
                    cvImg.Bytes = pixels;
                    cvBitmap = BitmapSourceConvert.ToBitmapSource(cvImg);
                }
               */

                // We're done with the DepthFrame 
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
                    Bitmap.Unlock();
                }

                if (depthFrame != null)
                {
                    depthFrame.Dispose();
                }

                if (colorFrame != null)
                {
                    colorFrame.Dispose();
                }
                       
            }

            if (screenShotFlag)
            {
                screenShotPath = screenShort();
                screenShotFlag = false;
                isScreenShotSuccess = true;
            }


        }





        /**
 * Converts the ColorFrame of the Kinect v2 to an image applicable for Emgu CV
 */
        public static Image<Bgra, byte> ToImage(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            Image<Bgra, byte> img = new Image<Bgra, byte>(width, height);
            img.Bytes = pixels;

            return img;
        }






        public bool screenShotFlag;
        public bool isScreenShotSuccess;
        private int screenFps = 50;
        private int fpsCount = 0;
        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            StatusText = kinectSensor.IsAvailable
                ? "running"
                : "stop";
        }
        public string screenShotPath = "";
        public string getscreenShortPath()
        {
            return screenShotPath;
        }

        private int index = 0;

        public string screenShort()
        {
            index++;
            if (Bitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(Bitmap));

                string time = DateTime.Now.ToString("hh'-'mm'-'s-" + index, CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.CurrentDirectory;

                screenShotPath = Path.Combine(myPhotos, "KinectScreenshot-Color-" + time + ".png");
                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(screenShotPath, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    // this.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);
                }
                catch (IOException)
                {
                    //this.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }
            return screenShotPath;
        }
        /// <summary>
        /// 获取colorSpacePoint对应的距离
        /// </summary>
        /// <param name="colorSpacePoint"></param>
        /// <returns></returns>
        public CameraSpacePoint GetCameraSpacePoint(ColorSpacePoint colorSpacePoint)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();
            while (true)
            {
                if (colorSpacePoint.X > colorWidth || colorSpacePoint.X < 0 || colorSpacePoint.Y > colorHeight || colorSpacePoint.Y < 0)
                {
                    return cameraPoint;
                }
                if (coordinateMappingReadyFlag)
                {
                    cameraPoint = cameraPointsCpy[(int)colorSpacePoint.Y * colorWidth + (int)colorSpacePoint.X];
                    cameraPoint.X = cameraPoint.X - 0.05f;
                    return cameraPoint;
                }

            }

        }
        /// <summary>
        /// 使用像素在平面的X,Y 坐标，转化为三维坐标
        /// </summary>
        /// <param name="pixels_X"></param>
        /// <param name="pixels_Y"></param>
        /// <returns></returns>
        public CameraSpacePoint GetCameraSpacePoint(int pixels_X, int pixels_Y)
        {
            CameraSpacePoint cameraPoint = new CameraSpacePoint();

            if ((pixels_X > colorWidth || pixels_X < 0 || pixels_Y > colorHeight || pixels_Y < 0))
            {
                return cameraPoint;
            }
            else
            {
                while (coordinateMappingReadyFlag)
                {
                   
                        cameraPoint = cameraPointsCpy[(int)pixels_Y * colorWidth + (int)pixels_X];
                        cameraPoint.X = cameraPoint.X - 0.05f;
                        
                }
                return cameraPoint;
            }

        }

    }

    public class BackgroundRemovalTool
    {
        #region Constants

        /// <summary>
        /// The DPI.
        /// </summary>
        readonly double DPI = 96.0;

        /// <summary>
        /// Default format.
        /// </summary>
        readonly PixelFormat FORMAT = PixelFormats.Bgra32;

        /// <summary>
        /// Bytes per pixel.
        /// </summary>
        readonly int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        #endregion

        #region Members

        /// <summary>
        /// The bitmap source.
        /// </summary>
        WriteableBitmap _bitmap = null;

        /// <summary>
        /// The depth values.
        /// </summary>
        ushort[] _depthData = null;

        /// <summary>
        /// The body index values.
        /// </summary>
        byte[] _bodyData = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        byte[] _colorData = null;

        /// <summary>
        /// The RGB pixel values used for the background removal (green-screen) effect.
        /// </summary>
        byte[] _displayPixels = null;

        /// <summary>
        /// The color points used for the background removal (green-screen) effect.
        /// </summary>
        ColorSpacePoint[] _colorPoints = null;

        /// <summary>
        /// The coordinate mapper for the background removal (green-screen) effect.
        /// </summary>
        CoordinateMapper _coordinateMapper = null;


        /// <summary>
        /// Intermediate storage for the color to depth mapping
        /// </summary>
        private DepthSpacePoint[] _colorMappedToDepthPoints = null;

        /// Intermediate storage for color to camera mapping
        /// 
        private CameraSpacePoint[] _cameraPoints = null;


        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of BackgroundRemovalTool.
        /// </summary>
        /// <param name="mapper">The coordinate mapper used for the background removal.</param>
        public BackgroundRemovalTool(CoordinateMapper mapper)
        {
            _coordinateMapper = mapper;
        }

        #endregion

        #region Methods


        public float Distance = 0.80f;

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource and removes the background (green-screen effect).
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="colorFrame">The specified color frame.</param>
        /// <returns></returns>
        public Byte[] GreenScreen(ColorFrame colorFrame, DepthFrame depthFrame)
        {
            int colorWidth = colorFrame.FrameDescription.Width;
            int colorHeight = colorFrame.FrameDescription.Height;

            int depthWidth = depthFrame.FrameDescription.Width;
            int depthHeight = depthFrame.FrameDescription.Height;

            if (_displayPixels == null)
            {
                _depthData = new ushort[depthWidth * depthHeight];
                _colorData = new byte[colorWidth * colorHeight * BYTES_PER_PIXEL];
                _displayPixels = new byte[colorWidth * colorHeight * BYTES_PER_PIXEL];
                _colorPoints = new ColorSpacePoint[colorWidth * colorHeight];
                _bitmap = new WriteableBitmap(colorWidth, colorHeight, DPI, DPI, FORMAT, null);
                this._colorMappedToDepthPoints = new DepthSpacePoint[colorWidth * colorHeight];
                this._cameraPoints = new CameraSpacePoint[colorWidth * colorHeight];
            }

            if (((depthWidth * depthHeight) == _depthData.Length) &&
                ((colorWidth * colorHeight * BYTES_PER_PIXEL) == _colorData.Length))
            {
                depthFrame.CopyFrameDataToArray(_depthData);

                if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                {
                    colorFrame.CopyRawFrameDataToArray(_colorData);
                }
                else
                {
                    colorFrame.CopyConvertedFrameDataToArray(_colorData, ColorImageFormat.Bgra);
                }


                //_coordinateMapper.MapDepthFrameToColorSpace(_depthData, _colorPoints);


                // Access the depth frame data directly via LockImageBuffer to avoid making a copy
                using (KinectBuffer depthFrameData = depthFrame.LockImageBuffer())
                {
                    this._coordinateMapper.MapColorFrameToCameraSpaceUsingIntPtr(depthFrameData.UnderlyingBuffer,
                        depthFrameData.Size,
                        this._cameraPoints);

                    this._coordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(
                        depthFrameData.UnderlyingBuffer,
                        depthFrameData.Size,
                        this._colorMappedToDepthPoints);

                }


                Array.Clear(_displayPixels, 0, _displayPixels.Length);

                for (int y = 0; y < colorHeight; ++y)
                {
                    for (int x = 0; x < colorWidth; ++x)
                    {
                        int colorFrameIndex = (y * colorWidth) + x;

                        CameraSpacePoint cameraPoints = _cameraPoints[colorFrameIndex];

                        if (!float.IsNegativeInfinity(cameraPoints.X) &&
                            !float.IsNegativeInfinity(cameraPoints.Y) && !float.IsNegativeInfinity(cameraPoints.Z))
                        {
                            //DepthSpacePoint depthPoints = _coordinateMapper.MapCameraPointToDepthSpace(cameraPoints);
                            //int depthIndex = (int)(depthPoints.Y * depthWidth + depthPoints.X);

                            //int depth = _depthData[depthIndex];

                            if (cameraPoints.Z < Distance)
                            {
                                ColorSpacePoint colorPoint = _coordinateMapper.MapCameraPointToColorSpace(cameraPoints);

                                //int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                                //int colorY = (int)Math.Floor(colorPoint.Y + 0.5);
                                int colorX = x;
                                int colorY = y;
                                if ((colorX >= 0) && (colorX < colorWidth) && (colorY >= 0) && (colorY < colorHeight))
                                {
                                    int colorIndex = ((colorY * colorWidth) + colorX) * BYTES_PER_PIXEL;
                                    int displayIndex = colorFrameIndex * BYTES_PER_PIXEL;

                                    _displayPixels[displayIndex + 0] = _colorData[colorIndex];
                                    _displayPixels[displayIndex + 1] = _colorData[colorIndex + 1];
                                    _displayPixels[displayIndex + 2] = _colorData[colorIndex + 2];
                                    _displayPixels[displayIndex + 3] = 0xff;
                                }
                            }
                        }

                    }
                }
            }
            _bitmap.Lock();

            Marshal.Copy(_displayPixels, 0, _bitmap.BackBuffer, _displayPixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, colorWidth, colorHeight));

            _bitmap.Unlock();


            return _displayPixels;
        }

        #endregion
    }
}
