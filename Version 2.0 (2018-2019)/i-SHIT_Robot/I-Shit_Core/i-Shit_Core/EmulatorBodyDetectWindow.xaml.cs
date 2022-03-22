using i_Shit_Core.Library;
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
    /// Interaction logic for EmulatorBodyDetectWindow.xaml
    /// </summary>
    public partial class EmulatorBodyDetectWindow : Window
    {
        Point bodymanPos = new Point();
        internal float bodyX = 0;
        internal float bodyZ = 1;
        internal User fakeUser = new User();
        public EmulatorBodyDetectWindow()
        {
            InitializeComponent();
            string path = System.Environment.CurrentDirectory + "/body.jpg";//获取图片绝对路径
            BitmapImage image = new BitmapImage(new Uri(path, UriKind.Absolute));//打开图片
            bodyman.Source = image;//将控件和图片绑定，logo为Image控件名称
            bodyman.Cursor = Cursors.Hand;
            bodyinfoLabel.Content = "user[0].BodyCenter X(左右)=" + bodyX + " Z(前后)=" + bodyZ;
            fakeUser.BodyCenter.Z = bodyZ;
            fakeUser.BodyCenter.X = bodyX;
            fakeUser.ID = 1;
        }

        private void bodyman_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Image tmp = (Image)sender;
                double dx = e.GetPosition(null).X - bodymanPos.X + tmp.Margin.Left;
                bodyX = (float)tmp.Margin.Left / 71;
                double dy = e.GetPosition(null).Y - bodymanPos.Y + tmp.Margin.Top;
                bodyZ = 1 - ((float)tmp.Margin.Top / 71);
                fakeUser.BodyCenter.Z = bodyZ;
                fakeUser.BodyCenter.X = bodyX;
                bodyinfoLabel.Content = "user[0].BodyCenter X(左右)=" + bodyX + " Z(前后)=" + bodyZ;
                tmp.Margin = new Thickness(dx, dy, 0, 0);
                bodymanPos = e.GetPosition(null);
            }
        }

        private void bodyman_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image tmp = (Image)sender;
            bodymanPos = e.GetPosition(null);
            tmp.CaptureMouse();
        }

        private void bodyman_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image tmp = (Image)sender;
            tmp.ReleaseMouseCapture();
        }

        private void RaiseHandCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RaiseHandCheckBox.IsChecked == true)
            {
                PushHandCheckBox.IsChecked = false;
                PushHandCheckBox_Click(sender, e);
                fakeUser.isRaisingHand = true;
            }
            else
            {
                fakeUser.isRaisingHand = false;
            }
        }

        private void PushHandCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (PushHandCheckBox.IsChecked == true)
            {
                RaiseHandCheckBox.IsChecked = false;
                RaiseHandCheckBox_Click(sender, e);
                fakeUser.isHandRightPush = true;
                fakeUser.isHandPush = true;
            }
            else
            {
                fakeUser.isHandRightPush = false;
                fakeUser.isHandPush = false;
            }
        }
    }
}
