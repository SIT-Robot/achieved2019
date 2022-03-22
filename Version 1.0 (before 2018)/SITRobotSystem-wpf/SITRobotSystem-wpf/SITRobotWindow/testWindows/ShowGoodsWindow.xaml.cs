using System;
using System.Collections.Generic;
using System.Data;

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
using System.Configuration;
using SITRobotSystem_wpf.DAL;
using System.Data.Sql;
using System.Data.SqlClient;

namespace WpfApplication1.DAL
{
    /// <summary>
    /// ShowGoodsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ShowGoodsWindow : Window
    {
        public ShowGoodsWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            string ConString =
                DBConfig.connectString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Gno,GName,GCategory FROM goods";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Goods");
                sda.Fill(dt);
                DG_Goods.ItemsSource = dt.DefaultView;
            }
        }
    }
}
