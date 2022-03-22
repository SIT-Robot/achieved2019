using SITRobotSystem_wpf.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{
    /// <summary>
    /// MachineLearingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MachineLearingWindow : Window
    {
        private MutipleFrame mutipleFrame;
        private string objPathStr;

        public MachineLearingWindow()
        {
            mutipleFrame = new MutipleFrame();
            objPathStr = null;
            InitializeComponent();
            //mutipleColotImage.Source = ImageSource;

        }

       
    }
}
