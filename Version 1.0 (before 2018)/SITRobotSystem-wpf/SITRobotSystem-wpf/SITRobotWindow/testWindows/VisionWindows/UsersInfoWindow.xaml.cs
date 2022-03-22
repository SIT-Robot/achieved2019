using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;
using SITRobotSystem_wpf.BLL.Service;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.VisionWindows
{

    public delegate void ProcessDelegate(object sender, EventArgs e);


    /// <summary>
    /// UsersInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UsersInfoWindow : Window
    {

        private delegate void FlushClient();//代理

        public UserTracker userTracker;

        public TextBlock[] UserInfoTB = new TextBlock[6];
        public UsersInfoWindow()
        {

            userTracker = new UserTracker();

            InitializeComponent();
            UserInfoTB[0] = TBuser1Info;
            UserInfoTB[1] = TBuser2Info;
            UserInfoTB[2] = TBuser3Info;
            UserInfoTB[3] = TBuser4Info;
            UserInfoTB[4] = TBuser5Info;
            UserInfoTB[5] = TBuser6Info;
        }

        private void CrossThreadFlush()
        {

        }

        private void clsALLInfo()
        {
            for (int i = 0; i < 6; i++)
            {
                UserInfoTB[i].Text = "USER" + i + ": NONE";
            }
        }

        public void UsersArrived()
        {
            userTracker.ensureUser();

            ShowALLUser();
            
        }
        public void ShowUser(User user)
        {
            UserInfoTB[0].Text += "ID:";
            TBuser1Info.Text += user.ID + Environment.NewLine;
            TBuser1Info.Text += "isTracking:";
            TBuser1Info.Text += user.State + Environment.NewLine;
            TBuser1Info.Text += "headX:";
            TBuser1Info.Text += user.body.Joints[JointType.Head].Position.X + Environment.NewLine;
            TBuser1Info.Text += "headY:";
            TBuser1Info.Text += user.body.Joints[JointType.Head].Position.Y + Environment.NewLine;
            TBuser1Info.Text += "headZ:";
            TBuser1Info.Text += user.body.Joints[JointType.Head].Position.Z + Environment.NewLine;
            TBuser1Info.Text += "isSeated:";
            TBuser1Info.Text += user.isSeated.ToString() + Environment.NewLine;
            TBuser1Info.Text += "SeatedConfidence:";
            TBuser1Info.Text += user.seatedConfidence.ToString() + Environment.NewLine;
        }

        public void ShowALLUser()
        {
            if (UserTracker.isBodyReady)
            {
                int i = -1;
                int usersCount = UserTracker.users.Count();
                if (usersCount <= 6)
                {
                    foreach (User user in UserTracker.users)
                    {
                        i++;
                        UserInfoTB[i].Text = "USER" + i + Environment.NewLine;
                        UserInfoTB[i].Text += user.ID + Environment.NewLine;
                        UserInfoTB[i].Text += "isTracking:";
                        UserInfoTB[i].Text += user.State + Environment.NewLine;
                        UserInfoTB[i].Text += "HeightChara:";
                        UserInfoTB[i].Text += user.HeightCharacteristic + Environment.NewLine;
                        //UserInfoTB[i].Text += "headX:";
                        //UserInfoTB[i].Text += user.body.Joints[JointType.Head].Position.X + Environment.NewLine;
                        //UserInfoTB[i].Text += "headY:";
                        //UserInfoTB[i].Text += user.body.Joints[JointType.Head].Position.Y + Environment.NewLine;
                        //UserInfoTB[i].Text += "headZ:";
                        UserInfoTB[i].Text += "sex:" + user.sex.ToString() + Environment.NewLine;
                        UserInfoTB[i].Text += "breastSize:" + user.breastsize.ToString() + Environment.NewLine;
                        UserInfoTB[i].Text += "isSeated:";
                        UserInfoTB[i].Text += user.isSeated.ToString() + Environment.NewLine;
                        UserInfoTB[i].Text += "SeatedConfidence:";
                        UserInfoTB[i].Text += user.seatedConfidence.ToString() + Environment.NewLine;
                        UserInfoTB[i].Background = Brushes.Green;
                        if (user.isLeanLeft)
                        {
                             UserInfoTB[i].Background =Brushes.Aqua;
                        }
                        if (user.isLeanRight) 
                        {
                            UserInfoTB[i].Background = Brushes.Brown;
                        }
                        if (user.isJumping)
                        {
                            UserInfoTB[i].Background = Brushes.Blue;
                        }
                        if (user.isSquat)
                        {
                            UserInfoTB[i].Background = Brushes.Chartreuse;
                        }
                        if (user.sex == Sex.Female )
                        {
                            UserInfoTB[i].Background = Brushes.Pink;
                        }
                        if (user.sex == Sex.Male)
                        {
                            UserInfoTB[i].Background = Brushes.SteelBlue;
                        }
                        bodyCharStr.Text = user.bodyCharacteristicStr;

                    }
                    for (int j = i + 1; j < 6; j++)
                    {
                        UserInfoTB[j].Text = "USER" + j + ": NONE";
                        UserInfoTB[j].Background = Brushes.LightGray;
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //KinectConnection.GetInstance().endBodyDetect();
        }

    }
}
