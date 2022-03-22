using System.Windows.Forms;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows
{
    public partial class fStateOutput : Form
    {
        public fStateOutput(string[] objects,string[] things,string[] names)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            if (objects==null)
            {
                GPSRlb3.Items.AddRange(names);
                GPSRlb2.Items.AddRange(things);
            }
            else
            {
                GPSRlb1.Items.AddRange(objects);
                GPSRlb2.Items.AddRange(things);
                GPSRlb3.Items.AddRange(names);
            }
        }
        public fStateOutput()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            GPSRlb1.Visible = false;
            GPSRlb2.Visible = false;
            GPSRlb3.Visible = false;
            GPSRlabel1.Visible = false;
            GPSRlabel2.Visible = false;
            GPSRlabel3.Visible = false;
        }
        public void state(string a)
        {
            richTextBox1.Text += a+"\n";
        }

    }
}
