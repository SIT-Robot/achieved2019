//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class DepthWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Map depth range to byte range
        /// </summary>
        private const int MapDepthToByte = 8000 / 256;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        public KinectSensor kinectSensor;


        /// <summary>
        /// Reader for depth frames
        /// </summary>
        private DepthFrameReader depthFrameReader;

        /// <summary>
        /// Description of the data contained in the depth frame
        /// </summary>
        private FrameDescription depthFrameDescription;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap depthBitmap;



        /// <summary>
        /// Intermediate storage for frame data converted to color
        /// </summary>
        private byte[] depthPixels;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public DepthWindow()
        {
            // get the kinectSensor object
            kinectSensor = KinectSensor.GetDefault();

            // open the reader for the depth frames
            depthFrameReader = kinectSensor.DepthFrameSource.OpenReader();

            // wire handler for frame arrival
            depthFrameReader.FrameArrived += Reader_FrameArrived;


            // get FrameDescription from DepthFrameSource
            depthFrameDescription = kinectSensor.DepthFrameSource.FrameDescription;

            // allocate space to put the pixels being received and converted
            depthPixels = new byte[depthFrameDescription.Width * depthFrameDescription.Height];

            // create the bitmap to display
            depthBitmap = new WriteableBitmap(depthFrameDescription.Width, depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);

            // set IsAvailableChanged event notifier
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            // open the sensor
            kinectSensor.Open();

            // set the status text
            StatusText = kinectSensor.IsAvailable ? "running"
                                                            : "None";

            // use the window object as the view model in this simple example
            DataContext = this;

            // initialize the components (controls) of the window
            InitializeComponent();
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return depthBitmap;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return statusText;
            }

            set
            {
                if (statusText != value)
                {
                    statusText = value;

                    // notify any bound elements that the text has changed
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (depthFrameReader != null)
            {
                // DepthFrameReader is IDisposable
                depthFrameReader.Dispose();
                depthFrameReader = null;
            }

            if (kinectSensor != null)
            {
                kinectSensor.Close();
                kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            if (depthBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(depthBitmap));

                string time = DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = Path.Combine(myPhotos, "KinectScreenshot-Depth-" + time + ".png");

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.SavedScreenshotStatusTextFormat, path);
                }
                catch (IOException)
                {
                    //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }
        }



        private static  int WIDTH = 512;
        private static int HEIGHT = 424;
        private static float KINECT_HIGHT = 0.23f;
        private static float TOTAL_HIGHT = 0.37f;
        private static float SAFE_DISTANCE = 1.1f;
        private static int Xline=WIDTH/2;
        public int unsafeCount = 0;
        private bool isReady = false;
        /// <summary>
        /// Handles the depth frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            bool depthFrameProcessed = false;

            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    // the fastest way to process the body index data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                    {


                        // verify data and write the color data to the display bitmap
                        if (((this.depthFrameDescription.Width * this.depthFrameDescription.Height) == (depthBuffer.Size / this.depthFrameDescription.BytesPerPixel)) &&
                            (this.depthFrameDescription.Width == this.depthBitmap.PixelWidth) && (this.depthFrameDescription.Height == this.depthBitmap.PixelHeight))
                        {
                            // Note: In order to see the full range of depth (including the less reliable far field depth)
                            // we are setting maxDepth to the extreme potential depth threshold
                            ushort maxDepth = ushort.MaxValue;

                            // If you wish to filter by reliable depth distance, uncomment the following line:
                            //// maxDepth = depthFrame.DepthMaxReliableDistance

                            this.ProcessDepthFrameData(depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, maxDepth);
                            depthFrameProcessed = true;

                            //数据定义
                            ushort[] depthFrameArray = new ushort[depthFrameDescription.LengthInPixels];
                            CameraSpacePoint[] spacePointsBuffer = new CameraSpacePoint[depthFrameDescription.LengthInPixels];

                            //转换
                            depthFrame.CopyFrameDataToArray(depthFrameArray);
                            kinectSensor.CoordinateMapper.MapDepthFrameToCameraSpace(depthFrameArray, spacePointsBuffer);

                            //读取
                            int x = WIDTH / 2;
                            int y = HEIGHT / 2;
                            CameraSpacePoint centerPoint = spacePointsBuffer[y * depthFrameDescription.Width + x];
                            unsafeCount = 0;
                            Console.WriteLine(centerPoint.Z);
                            isReady = false;
                            unsafeCount = dis(spacePointsBuffer, WIDTH / 2, TOTAL_HIGHT, -KINECT_HIGHT, SAFE_DISTANCE);
                            isReady = true;
                            Console.WriteLine("UNsafe Count" + unsafeCount);
                        }
                    }
                }
            }

            if (depthFrameProcessed)
            {
                this.RenderDepthPixels();
            }
        }

        private int dis(CameraSpacePoint[] array, int xline, float ymax, float ymin, float Zrange)
        {
            int unsafeCount = 0;
            int ii = 0;
            CameraSpacePoint[] downCSP = new CameraSpacePoint[HEIGHT];
            for (int i = 0; i < HEIGHT - 1; i++)
            {
                int px = xline;
                int py = i;
                downCSP[ii] = array[py * WIDTH + px];

                if (downCSP[ii].Y > ymin && downCSP[ii].Y < ymax
                    && downCSP[ii].Z < Zrange && !float.IsNegativeInfinity(downCSP[ii].Z) && downCSP[ii].Z != 0)
                {
                    unsafeCount++;
                }
                ii++;
            }
            return unsafeCount;
        }

        public void setXline(int range)
        {
            Xline = range;
        }

        public int getUnsafeCount()
        {
            while (true)
            {
                if (isReady)
                {
                    return this.unsafeCount;
                }
            }

        }
        //设置机器人的
        public void SetRobotInfo(float _KINECT_HIGHT, float _TOTAL_HIGHT, float _SAFE_DISTANCE)
        {
            KINECT_HIGHT = _KINECT_HIGHT;
            TOTAL_HIGHT = _TOTAL_HIGHT;
            SAFE_DISTANCE=_SAFE_DISTANCE;

        }




        


        /// <summary>
        /// Directly accesses the underlying image buffer of the DepthFrame to 
        /// create a displayable bitmap.
        /// This function requires the /unsafe compiler option as we make use of direct
        /// access to the native memory pointed to by the depthFrameData pointer.
        /// </summary>
        /// <param name="depthFrameData">Pointer to the DepthFrame image data</param>
        /// <param name="depthFrameDataSize">Size of the DepthFrame image data</param>
        /// <param name="minDepth">The minimum reliable depth value for the frame</param>
        /// <param name="maxDepth">The maximum reliable depth value for the frame</param>
        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth)
        {
            // depth frame data is a 16 bit value
            ushort* frameData = (ushort*)depthFrameData;

            // convert depth to a visual representation
            for (int i = 0; i < (int)(depthFrameDataSize / depthFrameDescription.BytesPerPixel); ++i)
            {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
            }
        }

        /// <summary>
        /// Renders color pixels into the writeableBitmap.
        /// </summary>
        private void RenderDepthPixels()
        {
            depthBitmap.WritePixels(
                new Int32Rect(0, 0, depthBitmap.PixelWidth, depthBitmap.PixelHeight),
                depthPixels,
                depthBitmap.PixelWidth,
                0);
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            StatusText = kinectSensor.IsAvailable ? "running"
                                                            : "None";
        }
    }
}
