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
        NamedPipeServerStream pipeServer =
                   new NamedPipeServerStream("basemove", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
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
                    while (true)
                    {
                        this.Invoke((MethodInvoker)delegate { listView1.Items.Add(sr.ReadLine()); });
                    }
                }, pipeServer);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);
            pipeServer.Write(byteArray, 0, byteArray.Length);
        }


    }
}
