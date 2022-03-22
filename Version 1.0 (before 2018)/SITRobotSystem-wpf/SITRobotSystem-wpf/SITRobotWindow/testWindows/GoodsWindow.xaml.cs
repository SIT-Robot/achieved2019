using System.Windows;

namespace WpfApplication1.DAL
{
    /// <summary>
    /// GoodsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GoodsWindow : Window
    {
        public GoodsWindow()
        {
            InitializeComponent();
        }

        private void Bt_Find_Click(object sender, RoutedEventArgs e)
        {
            string Sqlcmd = "select Gno from Goods where GName='" + TB_TypeName.Text + "'";
            TB_FindNo.Text = Sqlcmd;

            string Sqlcmd1 = "select GName from Goods where Gno'" + TB_FindNo.Text + "'";
            TB_TypeName.Text = Sqlcmd1;
        }

      

      
    }
}
