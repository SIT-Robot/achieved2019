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

namespace i_Shit_Core
{
    /// <summary>
    /// Interaction logic for EmulatorHandWindow.xaml
    /// </summary>
    public partial class EmulatorHandWindow : Window
    {
        internal float HandAngle = 0;
        public EmulatorHandWindow()
        {

            InitializeComponent();

            string path = System.Environment.CurrentDirectory + "/handup.jpg";//获取图片绝对路径
            BitmapImage image = new BitmapImage(new Uri(path, UriKind.Absolute));//打开图片
            handup.Source = image;//将控件和图片绑定，logo为Image控件名称


            path = System.Environment.CurrentDirectory + "/handdown.jpg";//获取图片绝对路径
            image = new BitmapImage(new Uri(path, UriKind.Absolute));//打开图片
            handdown.Source = image;//将控件和图片绑定，logo为Image控件名称
        }
    }
}
