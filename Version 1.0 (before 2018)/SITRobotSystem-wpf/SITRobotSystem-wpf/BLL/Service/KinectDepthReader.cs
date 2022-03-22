using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace SITRobotSystem_wpf.BLL.Service
{
    class KinectDepthReader
    {
        /// <summary>
        /// Map depth range to byte range
        /// </summary>
        private const int MapDepthToByte = 8000 / 256;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Reader for depth frames
        /// </summary>
        private DepthFrameReader depthFrameReader = null;

        /// <summary>
        /// Description of the data contained in the depth frame
        /// </summary>
        private FrameDescription depthFrameDescription = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap depthBitmap = null;

        /// <summary>
        /// Intermediate storage for frame data converted to color
        /// </summary>
        private byte[] depthPixels = null;

        public void Init()
        {
            // get the kinectSensor object
            this.kinectSensor = KinectSensor.GetDefault();

            // open the reader for the depth frames
            this.depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();

            // wire handler for frame arrival
            this.depthFrameReader.FrameArrived += this.Reader_FrameArrived;

            // get FrameDescription from DepthFrameSource
            this.depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            // allocate space to put the pixels being received and converted
            this.depthPixels = new byte[this.depthFrameDescription.Width * this.depthFrameDescription.Height];

            // create the bitmap to display

            // set IsAvailableChanged event notifier
            //this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

           
        }

        public void kill()
        {
            if (this.depthFrameReader != null)
            {
                // DepthFrameReader is IDisposable
                this.depthFrameReader.Dispose();
                this.depthFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }


        public const int WIDTH = 512;
        public const int HEIGHT = 424;
        public  float KINECT_HIGHT = 0.23f;
        public  float TOTAL_HIGHT = 0.37f;
        public  float SAFE_DISTANCE = 1.1f;
        public float Xline = WIDTH/2;
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
            this.Xline = range;
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
            for (int i = 0; i < (int)(depthFrameDataSize / this.depthFrameDescription.BytesPerPixel); ++i)
            {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                this.depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
            }
        }

        /// <summary>
        /// Renders color pixels into the writeableBitmap.
        /// </summary>
        private void RenderDepthPixels()
        {
            this.depthBitmap.WritePixels(
                new Int32Rect(0, 0, this.depthBitmap.PixelWidth, this.depthBitmap.PixelHeight),
                this.depthPixels,
                this.depthBitmap.PixelWidth,
                0);
        }

        public event PropertyChangedEventHandler PropertyChanged;



    }


}
