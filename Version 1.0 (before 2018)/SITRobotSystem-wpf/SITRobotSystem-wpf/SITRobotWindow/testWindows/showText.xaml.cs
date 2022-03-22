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


using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;
using System.CodeDom;
using System.Threading;
using Emgu.CV.Util;
using Point = SITRobotSystem_wpf.entity.Point;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// showText.xaml 的交互逻辑
    /// </summary>
    public partial class showText : Window
    {
        public showText()
        {
            InitializeComponent();
        }
    }
}
