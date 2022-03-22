using System.Windows;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows
{
    /// <summary>
    /// HandTest.xaml 的交互逻辑
    /// </summary>
    public partial class HandTest : Window
    {
        public HandTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ArmAction armAction=new ArmAction();
            armAction.ID = int.Parse(TBHandID.Text);
            ArmCtrl armCtrl=new ArmCtrl();
            armCtrl.getThing(armAction);
        }
    }
}
