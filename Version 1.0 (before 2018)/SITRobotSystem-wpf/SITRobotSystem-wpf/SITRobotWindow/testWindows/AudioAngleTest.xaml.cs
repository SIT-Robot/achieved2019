using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// AudioAngleTest.xaml 的交互逻辑
    /// </summary>
    public partial class AudioAngleTest : Window
    {

        public AudioAngleTest()
        {
            InitializeComponent();
        }

        private void starttestbutton_Click(object sender, RoutedEventArgs e)
        {
            KinectSensor kinectSensor = null;
            kinectSensor = KinectSensor.GetDefault();
            kinectSensor.Open();
            AudioSource audioSource = kinectSensor.AudioSource;
            AudioDetection audioDetection = new AudioDetection(audioSource);

            Thread updateThread = new Thread(new ThreadStart(() => getangle(audioDetection)));
            updateThread.Start();


        }
        public static void getangle(AudioDetection audioDetection)
        {
            Console.WriteLine("angle" + audioDetection.nowangle);
            Thread.Sleep(1);
        }
    }
}
