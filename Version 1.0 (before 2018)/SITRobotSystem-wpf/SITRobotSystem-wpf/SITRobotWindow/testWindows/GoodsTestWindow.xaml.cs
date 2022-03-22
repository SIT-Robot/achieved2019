using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using SITRobotSystem_wpf.DAL;
using SITRobotSystem_wpf.entity;
using WpfApplication1.DAL;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// GoodsTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GoodsTestWindow : Window
    {
        public GoodsTestWindow()
        {
            InitializeComponent();
        }

        private void BtnPicChoice_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "jpg文件|*.jpg|png文件|*.png|所有文件|*.*";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "jpg";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            String filepath = openFileDialog.FileName;
            string[] filePathCut = filepath.Split('\\');
            string MainPath = filePathCut[filePathCut.Count()-1];
            TBPicPath.Text = MainPath;

        }

        private void TBGno_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DBCtrl dbCtrl=new DBCtrl();
            Goods goods = dbCtrl.GetGoodsByNo(TBGno.Text);
            if (!string.IsNullOrWhiteSpace(goods.Name))
            {
                showGoods(goods);
            }
        }

        private void showGoods(Goods goods)
        {
            TBGno.Text = goods.No;
            TBGname.Text = goods.Name;
            TBGCategory.Text = goods.Category;
            TBPicPath.Clear();
            
            foreach (var VARIABLE in goods.imgPath)
            {
                TBPicPath.Text += VARIABLE.ToString();
            }
        }

        private void TBGname_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DBCtrl dbCtrl = new DBCtrl();
            Goods goods = dbCtrl.GetGoodsByName(TBGname.Text);
            if (!string.IsNullOrWhiteSpace(goods.Name))
            {
                showGoods(goods);
            }
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            ShowGoodsWindow showGoodsWindow=new ShowGoodsWindow();
            showGoodsWindow.Show();
        }

    }



}
