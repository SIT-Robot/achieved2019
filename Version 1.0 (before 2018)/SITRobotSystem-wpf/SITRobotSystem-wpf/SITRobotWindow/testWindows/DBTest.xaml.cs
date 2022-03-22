using System.Windows;
using System.Windows.Controls;
using SITRobotSystem_wpf.BLL.Connection;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using Point = SITRobotSystem_wpf.entity.Point;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DBTestWindow : Window
    {

        private TextBox _TB_PorientationX=new TextBox();
        private TextBox _TB_PorientationY = new TextBox();
        private TextBox _TB_PorientationZ = new TextBox();
        private TextBox _TB_PorientationW = new TextBox();
        private TextBox _TB_PpostionX = new TextBox();
        private TextBox _TB_PpostionY = new TextBox();
        private TextBox _TB_PpostionZ = new TextBox();



        public DBTestWindow()
        {
            TB_PorientationX = _TB_PorientationX;
            TB_PorientationY = _TB_PorientationY;
            TB_PorientationZ = _TB_PorientationZ;
            TB_PorientationW = _TB_PorientationW;
            TB_PpostionX = _TB_PpostionX;
            TB_PpostionY = _TB_PpostionY;
            TB_PpostionZ = _TB_PpostionZ;


            InitializeComponent();
        }

        private Place UIgetPlace()
        {
            string Pno = TB_Pno.Text;
            string ProomName = TB_ProomName.Text;
            string PName = TB_PName.Text;
            string PCategroy = TB_PCategroy.Text;
            Point point = new Point(float.Parse(TB_PpostionX.Text), float.Parse(TB_PpostionY.Text),
                float.Parse(TB_PpostionZ.Text));
            Quaternion quaternion = new Quaternion(float.Parse(TB_PorientationX.Text),
                float.Parse(TB_PorientationY.Text), float.Parse(TB_PorientationZ.Text),
                float.Parse(TB_PorientationW.Text));
            Header header = new Header(PName,"map");
            Place p = new Place(Pno, PName, ProomName, PCategroy, quaternion, point, header);
            return p;
        }
        private void BTN_INSERT_Click(object sender, RoutedEventArgs e)
        {
            Place p=UIgetPlace();
            DBCtrl dbCtrl = new DBCtrl();
            dbCtrl.InsertPlace(p);
        }


        private void BTNload_Click(object sender, RoutedEventArgs e)
        {



        }

        private void TB_Pno_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBCtrl dbCtrl=new DBCtrl();
            Place place= dbCtrl.GetPlaceByID(TB_Pno.Text);
            if (place.No!=null)
            {
                showPlaceNameInfo(place);
                showPlacePositionInfo(place);
            }
        }


        private void BtnGetPosition_Click(object sender, RoutedEventArgs e)
        {
            BaseConnection baseConnection=new BaseConnection();
            Place place =baseConnection.getPosition();
            showPlacePositionInfo(place);
            
        }

        /// <summary>
        /// 显示place的点的信息
        /// </summary>
        /// <param name="place"></param>
        private void showPlacePositionInfo(Place place)
        {
            TB_PpostionX.Text = place.position.X.ToString();
            TB_PpostionY.Text = place.position.Y.ToString();
            TB_PpostionZ.Text = place.position.Z.ToString();

            TB_PorientationX.Text = place.oritation.X.ToString();
            TB_PorientationY.Text = place.oritation.Y.ToString();
            TB_PorientationZ.Text = place.oritation.Z.ToString();
            TB_PorientationW.Text = place.oritation.W.ToString();
        }

        /// <summary>
        /// 显示place的名字等信息
        /// </summary>
        private void showPlaceNameInfo(Place place)
        {
            TB_PName. Text = place.Name;
            TB_PCategroy.Text = place.PCategory;
            TB_ProomName.Text = place.ProomName;
            TB_Pno.Text = place.No;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Place p = UIgetPlace();
            DBCtrl dbCtrl = new DBCtrl();
            dbCtrl.UpdatePlace(p);
        }

        private void GoToPlace_Click(object sender, RoutedEventArgs e)
        {
            BaseCtrl baseCtrl=new BaseCtrl();
            DBCtrl dbCtrl = new DBCtrl();
            string pname = TB_PNameGola.Text;
            Place goal = dbCtrl.GetPlaceByID(pname);
            if (!string.IsNullOrEmpty(goal.No))
            {
                baseCtrl.moveToGoal(goal);
            }
        }



        private void TB_ProomName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            Place_window placeWindow=new Place_window();
            placeWindow.Show();
        }

        private void TB_PName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_PName.Text.Contains("'"))
                TB_PName.Text = "";
            DBCtrl dbCtrl = new DBCtrl();
            Place place = dbCtrl.GetPlaceByName(TB_ProomName.Text);
            if (place.ProomName != null)
            {
                showPlaceNameInfo(place);
                showPlacePositionInfo(place);
            }
        }

    }
}
