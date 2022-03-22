using i_Shit_Core.Core.Drivers;
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
    /// Interaction logic for EmulatorWindow.xaml
    /// </summary>
    public partial class EmulatorMainWindow : Window
    {
        public EmulatorMainWindow()
        {
            InitializeComponent();
        }


        private void move_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (move_checkbox.IsChecked == true)

            {
                Driver.Emulator_Move_Enable = true;
                Driver.Emulator_FootprintWindow.Visibility = Visibility.Visible;
            }
            else
            {
                Driver.Emulator_Move_Enable = false;
                Driver.Emulator_FootprintWindow.Visibility = Visibility.Hidden;
            }

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Driver.Emulator_Ready = true;
            startButton.IsEnabled = false;

        }

        private void move_checkbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void hand_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (hand_checkbox.IsChecked == true)

            {
                Driver.Emulator_Hand_Enable = true;
                Driver.Emulator_HandWindow.Visibility = Visibility.Visible;
            }
            else
            {
                Driver.Emulator_Hand_Enable = false;
                Driver.Emulator_HandWindow.Visibility = Visibility.Hidden;
            }
        }

        private void bodydetect_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (bodydetect_checkbox.IsChecked == true)

            {
                Driver.Emulator_BodyDetect_Enable = true;
                Driver.Emulator_BodyDetectWindow.Visibility = Visibility.Visible;
            }
            else
            {
                Driver.Emulator_BodyDetect_Enable = false;
                Driver.Emulator_BodyDetectWindow.Visibility = Visibility.Hidden;
            }
        }

        private void cameraspacepoint_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (cameraspacepoint_checkbox.IsChecked == true)

            {
                Driver.Emulator_MLCSP_Enable = true;
                Driver.Emulator_MLCSPWindow.Visibility = Visibility.Visible;
            }
            else
            {
                Driver.Emulator_MLCSP_Enable = false;
                Driver.Emulator_MLCSPWindow.Visibility = Visibility.Hidden;
            }
        }
    }
}
