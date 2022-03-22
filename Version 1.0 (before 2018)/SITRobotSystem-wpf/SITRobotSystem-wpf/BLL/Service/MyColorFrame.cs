using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.FunctionClass;

namespace SITRobotSystem_wpf.BLL.Service
{
    public class MyColorFrame : INotifyPropertyChanged, IDisposable
    {
           /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor;

        /// <summary>
        /// Reader for color frames
        /// </summary>
        public ColorFrameReader colorFrameReader;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// The size in bytes of the bitmap back buffer
        /// </summary>
        private uint bitmapBackBufferSize;

        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        private FrameDescription colorFrameDescription;


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
        /// Current status text to display
        /// </summary>
        private string statusText;


        public MyColorFrame()
        {
            // get the kinectSensor object
            kinectSensor = KinectSensor.GetDefault();

            // open the reader for the color frames
            colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();

            // wire handler for frame arrival
            colorFrameReader.FrameArrived += Reader_ColorFrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using Bgra format
            //FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

            // create the bitmap to display
            colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            // Calculate the WriteableBitmap back buffer size
            bitmapBackBufferSize =
                (uint)
                    ((colorBitmap.BackBufferStride * (colorBitmap.PixelHeight - 1)) +
                     (colorBitmap.PixelWidth * bytesPerPixel));

            // set IsAvailableChanged event notifier
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            // open the sensor
            kinectSensor.Open();

            // set the status text
            StatusText = kinectSensor.IsAvailable ? "running"
                                                            : "None";

            // use the window object as the view model in this simple example
            //this.DataContext = this;

            // initialize the components (controls) of the window
            //this.InitializeComponent();
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
                return colorBitmap;
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
            if (colorFrameReader != null)
            {
                // ColorFrameReder is IDisposable
                colorFrameReader.Dispose();
                colorFrameReader = null;
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
            if (colorBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(colorBitmap));

                string time = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = Path.Combine(myPhotos, "KinectScreenshot-Color-" + time + ".png");

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
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
        }


        private byte[] pixels;

        /// <summary>
        /// Handles the color frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

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


                        colorFrame.CopyConvertedFrameDataToIntPtr(colorBitmap.BackBuffer, bitmapBackBufferSize,
    ColorImageFormat.Bgra);


                        //Image<Bgra, byte> CVimg=ToImage(colorFrame);
                        pixels = new byte[colorFrameDescription.Width * colorFrameDescription.Height * colorFrameDescription.BytesPerPixel];

                        using (KinectBuffer ColorFrameData = colorFrame.LockRawImageBuffer())
                        {

                            if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                                colorFrame.CopyRawFrameDataToArray(pixels);
                            else
                                colorFrame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);

                            //Initialize Emgu CV image then assign byte array of pixels to it
                            cvImg = new Image<Bgra, byte>(colorFrameDescription.Width, colorFrameDescription.Height);
                            cvImg.Bytes = pixels;
                            cvBitmap = BitmapSourceConvert.ToBitmapSource(cvImg);
                        }




                        colorBitmap.Unlock();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the evvent which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            StatusText = kinectSensor.IsAvailable ? "running"
                                                            : "None";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //KinectConnection.GetInstance().endcolorScan();
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
