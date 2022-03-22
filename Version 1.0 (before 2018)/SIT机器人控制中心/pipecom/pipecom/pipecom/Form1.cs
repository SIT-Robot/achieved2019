using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Pipes;
using System.IO;
using System.Security.Principal;
using System.Threading;

namespace pipecom
{
    public partial class Form1 : Form
    {
        int motor1, motor2, motor3;
        NamedPipeServerStream pipeServer =
                   new NamedPipeServerStream("odom", PipeDirection.InOut, 1,
                                             PipeTransmissionMode.Message, PipeOptions.Asynchronous);
        StreamWriter sw = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                pipeServer.BeginWaitForConnection((o) =>
                {
                    NamedPipeServerStream pServer = (NamedPipeServerStream)o.AsyncState;
                    pServer.EndWaitForConnection(o);
                    StreamReader sr = new StreamReader(pServer);
                    //while (true)
                    //{
                    //    this.Invoke((MethodInvoker)delegate {listView1.Items.Add(sr.ReadLine()); });
                    //}
                }, pipeServer);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);
            pipeServer.Write(byteArray, 0, byteArray.Length);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval =Int32.Parse( textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            motor1 = Int32.Parse(textBox3.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            motor2 = Int32.Parse(textBox4.Text);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            motor3 = Int32.Parse(textBox5.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string odom;
            odom = "LL," + motor1 + "," +motor2 + "," + motor3 + ",JJ";
            listView1.Items.Add(odom);
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(odom);
            try
            {
                pipeServer.Write(byteArray, 0, byteArray.Length);
            }
            catch { }
        }


    }
}
