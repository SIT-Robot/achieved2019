using System;
using System.Collections.Generic;
using System.Drawing;
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
using SITRobotSystem_wpf.BLL.Connection;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// ImageTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTestWindow : Window
    {
        public ImageTestWindow()
        {
            InitializeComponent();
        }

        private void BtnMake_Click(object sender, RoutedEventArgs e)
        {
            SitRobotHub sitRobotHub = new SitRobotHub("192.168.1.100", 8081);
            Bitmap bit = new Bitmap(sitRobotHub.getPM());
        }
    }
}
