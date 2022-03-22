using i_Shit_Core.Core;
using i_Shit_Core.Core.Drivers;
using i_Shit_Core.Core.Functions;
using i_Shit_Core.Library;
using i_Shit_Scirpt.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        public ArrayList UpdateNameList()
        {
            ArrayList nameList = new ArrayList();
            DataSet resultDataSet = Driver.SQLite_Query(Driver.SQLite_OpenDB("RosLocationDB"), "Location", new string[] { }, new string[] { });//查询整个Location表
            for (int i = 0; i < resultDataSet.Tables[0].Rows.Count; i++)
            {
                nameList.Add(resultDataSet.Tables[0].Rows[i]["name"].ToString());
            }
            return nameList;
        }


        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                LocationInfo li = Function.Location_GetLocationInfoByName(nameBox.Text);
                pxBox.Text = li._positionX.ToString();
                pyBox.Text = li._positionY.ToString();
                pzBox.Text = li._positionZ.ToString();
                oxBox.Text = li._orientationX.ToString();
                oyBox.Text = li._orientationY.ToString();
                ozBox.Text = li._orientationZ.ToString();
                owBox.Text = li._orientationW.ToString();
            }
            catch { MessageBox.Show("DB Error\nMaybe no this record."); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo li = new LocationInfo();
            li._locationName = nameBox.Text;
            li._positionX = double.Parse(pxBox.Text);
            li._positionY = double.Parse(pyBox.Text);
            li._positionZ = double.Parse(pzBox.Text);
            li._orientationX = double.Parse(oxBox.Text);
            li._orientationY = double.Parse(oyBox.Text);
            li._orientationZ = double.Parse(ozBox.Text);
            li._orientationW = double.Parse(owBox.Text);
            Function.Location_UpdateLocationInfo(li);
            //重加载namelist
            ArrayList nameList = UpdateNameList();
            nameBox.Items.Clear();
            for (int i = 0; i < nameList.Count; i++)
            {

                nameBox.Items.Add(((string)nameList[i]));

            }
            //--重加载nameList
            MessageBox.Show("Updated");
        }

        private void getLocationButton_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo li = Function.Location_GetCurrectLocationFromRos();
            pxBox.Text = li._positionX.ToString();
            pyBox.Text = li._positionY.ToString();
            pzBox.Text = li._positionZ.ToString();
            oxBox.Text = li._orientationX.ToString();
            oyBox.Text = li._orientationY.ToString();
            ozBox.Text = li._orientationZ.ToString();
            owBox.Text = li._orientationW.ToString();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Function.Location_DeleteLocationInfoByName(nameBox.Text);
            //删除后重新加载 namelist
            ArrayList nameList = UpdateNameList();
            nameBox.Items.Clear();
            for (int i = 0; i < nameList.Count; i++)
            {

                nameBox.Items.Add(((string)nameList[i]));

            }
            //--删除后重新加载nameList
            pxBox.Text = "";
            pyBox.Text = "";
            pzBox.Text = "";
            oxBox.Text = "";
            oyBox.Text = "";
            ozBox.Text = "";
            owBox.Text = "";
        }

        private void gogogoButton_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo li = new LocationInfo();
            li._locationName = nameBox.Text;
            li._positionX = double.Parse(pxBox.Text);
            li._positionY = double.Parse(pyBox.Text);
            li._positionZ = double.Parse(pzBox.Text);
            li._orientationX = double.Parse(oxBox.Text);
            li._orientationY = double.Parse(oyBox.Text);
            li._orientationZ = double.Parse(ozBox.Text);
            li._orientationW = double.Parse(owBox.Text);
            new Thread(new ThreadStart(delegate
            {
                Console.WriteLine("Sent Location to ROS in a new thread.");
                try
                {
                    Function.Move_Navigate(li);
                }
                catch { }
            })).Start();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList nameList = UpdateNameList();
            for (int i = 0; i < nameList.Count; i++)
            {
                nameBox.Items.Add(((string)nameList[i]));

            }
        }

        private void nameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                LocationInfo li = Function.Location_GetLocationInfoByName(nameBox.SelectedItem.ToString());
                pxBox.Text = li._positionX.ToString();
                pyBox.Text = li._positionY.ToString();
                pzBox.Text = li._positionZ.ToString();
                oxBox.Text = li._orientationX.ToString();
                oyBox.Text = li._orientationY.ToString();
                ozBox.Text = li._orientationZ.ToString();
                owBox.Text = li._orientationW.ToString();
            }
            catch { }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    break;
                case Key.F2:
                    break;
                case Key.F3:
                    break;
                case Key.F4:
                    break;
                case Key.F5:
                    break;
                case Key.F6:
                    break;
                default:
                    break;
            }
        }

        private void stopNavigateButton_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo li = new LocationInfo();
            li._orientationW = 0;
            li._orientationX = 0;
            li._orientationY = 0;
            li._orientationZ = 0;
            li._positionX = 0;
            li._positionY = 0;
            li._positionZ = 0;
            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
            new Thread(new ThreadStart(delegate { Function.Move_Navigate(li); })).Start();
            new Thread(new ThreadStart(delegate { Function.Move_SetSpeed(0, 0, 0); })).Start();
        }
    }
}
