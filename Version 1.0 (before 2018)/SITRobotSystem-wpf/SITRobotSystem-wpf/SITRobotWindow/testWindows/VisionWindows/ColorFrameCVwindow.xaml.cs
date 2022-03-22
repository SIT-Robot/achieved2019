using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Service;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{
    /// <summary>
    /// ColorFrameCVwindow.xaml 的交互逻辑
    /// </summary>
    public partial class ColorFrameCVwindow : Window
    {
        public ColorFrameCVwindow()
        {
            myColorFrame = new MyColorFrame();
            myColorFrame.colorFrameReader.FrameArrived += showImg;
            InitializeComponent();
        }


        private MyColorFrame myColorFrame;

        private Image<Bgr, byte> colorFrameImg;
        private byte[] pixels = null;

        public void showImg(object sender, ColorFrameArrivedEventArgs e)
        {
            SurfImg.Source = myColorFrame.GetBitmapSource();
            //this.SurfImg.Source = BitmapSourceConvert.ToBitmapSource(mutipleFrame.GetcvImg());
        }


    }
}
