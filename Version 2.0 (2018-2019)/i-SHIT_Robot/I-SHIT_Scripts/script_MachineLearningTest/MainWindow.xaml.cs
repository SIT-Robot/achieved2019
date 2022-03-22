using i_Shit_Core.Core;
using i_Shit_Core.Core.Functions;
using i_Shit_Scirpt.Script;
using Microsoft.Kinect;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace i_Shit_Scirpt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Core(new MyScript());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SynchronizationContext sc = SynchronizationContext.Current;
            string objname = objTextBox.Text;
            CameraSpacePoint csp  = new CameraSpacePoint();
            new Thread(new ThreadStart(delegate
                {
                    try
                    {
                       csp = Function.Vision_GetCameraSpacePoint(Function.Vision_FindObjectByMachineLearning(objname));
                    }
                    catch { }
                    sc.Post(delegate
                    {
                        xBox.Text = csp.Z.ToString();
                        yBox.Text = csp.X.ToString();
                    }, null);
                })).Start();


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Function.Move_Distance(0, float.Parse(yBox.Text), 0);
            Function.Move_Distance(float.Parse(xBox.Text), 0, 0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(delegate
            {
                Function.Vision_SaveJPGPhoto(Function.Vision_Kinect_Shot(), "Z:\\0.jpg");
            })).Start();
        }
    }
}
