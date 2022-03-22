using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Shit_Core
{




    public partial class CoreWindow : Form
    {

        class TextBoxWriter : TextWriter
        {
            TextBox textBox;
            delegate void WriteFunc(string value);
            WriteFunc write;
            WriteFunc writeLine;

            public TextBoxWriter(TextBox textBox)
            {
                this.textBox = textBox;
                write = Write;
                writeLine = WriteLine;
            }


            // 使用UTF-16避免不必要的编码转换
            public override Encoding Encoding
            {
                get { return Encoding.Unicode; }
            }


            // 最低限度需要重写的方法
            public override void Write(string value)
            {
                if (textBox.InvokeRequired)
                {
                    textBox.BeginInvoke(write, value);
                }
                else
                {
                    textBox.AppendText(value);
                    textBox.ScrollToCaret();
                }
            }


            // 为提高效率直接处理一行的输出
            public override void WriteLine(string value)
            {
                if (textBox.InvokeRequired)
                {
                    textBox.BeginInvoke(writeLine, value);
                }
                else
                {
                    textBox.AppendText(value);
                    textBox.AppendText(this.NewLine);
                    textBox.ScrollToCaret();
                }
            }

        }
        public CoreWindow()
        {
            InitializeComponent();
        }

        private void CoreWindow_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            Console.SetOut(new TextBoxWriter(logBox));
        }

        private void CoreWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }


    }
}
