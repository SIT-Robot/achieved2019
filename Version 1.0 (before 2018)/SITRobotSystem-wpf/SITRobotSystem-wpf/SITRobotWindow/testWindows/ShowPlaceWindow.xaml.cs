using System.Data;

using System.Windows;
using System.Windows.Controls;
using SITRobotSystem_wpf.DAL;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// Place_window.xaml 的交互逻辑
    /// </summary>
    public partial class Place_window : Window
    {
        public Place_window()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void DG_Place_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void FillDataGrid()
        {
            //string ConString = ConfigurationManager.ConnectionStrings["SITRobotSystemDataBase"].ConnectionString;
            string ConString =
                 DBConfig.connectString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Pno,ProomName,PName,PCategory,PpositionX,PpositionY,PpositionZ,PorientationX,PorientationY,PorientationZ,PorientationW FROM Place";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Place");
                sda.Fill(dt);
                DG_Place.ItemsSource = dt.DefaultView;
            }
        }
       


    }
}
