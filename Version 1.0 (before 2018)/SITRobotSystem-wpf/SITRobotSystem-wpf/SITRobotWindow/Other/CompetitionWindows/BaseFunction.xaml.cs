using System.Threading;
using System.Windows;
using SITRobotSystem_wpf.BLL.Competitions.BaseFunction;
using SITRobotSystem_wpf.UI;

namespace SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows
{
    /// <summary>
    /// BaseFunction.xaml 的交互逻辑
    /// </summary>
    public partial class BaseFunction : Window
    {
        private BaseFunctionCompetition BFComp;
        private WindowCtrl windowCtrl;

        public BaseFunction()
        {
            BFComp=new BaseFunctionCompetition();
            BFComp.init();
            windowCtrl = new WindowCtrl();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BFComp.GoPlace1 = tbgo1.Text.ToString();
            BFComp.GoPlace2 = tbgo2.Text.ToString();
            BFComp.Obj = tbObj.Text.ToString();
            BFComp.Start();
        }

        private void btnGetObj_Click(object sender, RoutedEventArgs e)
        {
            BFComp.NavPlace1 = tbNav1.Text.ToString();
            BFComp.NavPlace2 = tbNav2.Text.ToString();
            BFComp.GoPlace1 = tbgo1.Text.ToString();
            BFComp.GoPlace2 = tbgo2.Text.ToString();
            BFComp.Obj = tbObj.Text.ToString();
            ThreadStart(BFComp.runGetObj);
        }

        private void BtnQuestion_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart(BFComp.runBaseQuestion);
        }

        private void BtnNav_Click(object sender, RoutedEventArgs e)
        {
            BFComp.NavPlace1 = tbNav1.Text.ToString();
            BFComp.NavPlace2 = tbNav2.Text.ToString();
            ThreadStart(BFComp.runNav);
        }

        public void ThreadStart(ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
