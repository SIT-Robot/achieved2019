using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace SIT_ROBOT_CTRL_CENTER
{
    public partial class Form1 : Form
    {
        public delegate void UpdateText(string text);
        public UpdateText UpdateTextDelegate;
        public Socket clientSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private static byte[] result = new byte[1024];
        public const int Port = 24;   //端口  

        static Socket serverSocket;
        public void receivedupdater(string text)
        {
            //Invoke
            this.Invoke(new UpdateText(
                delegate(string s)
                {
                    //Do
                    textBox2.Text += System.Environment.NewLine + "Received:" + s;

                    if (text == "linuxliving")
                    {
                        linuxstateLabel.Text = "Linux子系统正常";
                        linuxstateLabel.ForeColor = Color.Green;
                        linuxwatchdog.Stop();
                        linuxwatchdog.Start();

                    }
                    //--Do
                }), text);
            //--Invoke


        }
        public static ArrayList ReadToArrayList(string filename)
        {
            string strReadFilePath = filename + "";
            StreamReader srReadFile = new StreamReader(strReadFilePath);
            ArrayList list = new ArrayList();
 
            while (!srReadFile.EndOfStream)
            {

                string strReadLine = srReadFile.ReadLine();
                list.Add(strReadLine + Environment.NewLine);
            }
            srReadFile.Close();

            return list;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ArrayList linuxbuttonList = ReadToArrayList("linux_name.config");
            ArrayList linuxfeatureList = ReadToArrayList("linux_feature.config");
            for (int i = 0; i < linuxbuttonList.Count; i++)
            {
                System.Windows.Forms.Button button = new Button();
                button.Text = (string)linuxbuttonList[i];
                button.Size = new Size(150, 40);
                button.Location = new Point(5, i*40);
                int nowindex = i;
                button.Click += delegate
                {

                    clientSocket.Send(System.Text.Encoding.Default.GetBytes(linuxfeatureList[nowindex].ToString().TrimEnd().Trim()));
                };
                panel1.Controls.Add(button);


            }

            ArrayList windowsbuttonList = ReadToArrayList("windows_name.config");
            ArrayList windowsfeatureList = ReadToArrayList("windows_feature.config");
            for (int i = 0; i < windowsbuttonList.Count; i++)
            {
                System.Windows.Forms.Button button = new Button();
                button.Text = (string)windowsbuttonList[i];
                button.Size = new Size(150, 40);
                button.Location = new Point(5, i * 40);
                int nowindex = i;
                button.Click += delegate
                {
                    string urlaa=windowsfeatureList[nowindex].ToString();
                    Process.Start(urlaa.TrimEnd().TrimStart());
 
                };
                panel3.Controls.Add(button);


            }


            UpdateTextDelegate += receivedupdater;
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse("0.0.0.0");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, Port));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        public void ListenClientConnect()
        {
            while (true)
            {
                clientSocket = serverSocket.Accept();
                Console.Write("Client:" + clientSocket.RemoteEndPoint.ToString());
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
                Console.Write("New Client thread");
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        public void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (myClientSocket.Connected)
            {
                try
                {
                    Console.Write("aaa");
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber != 0)
                    {
                        UpdateTextDelegate(Encoding.ASCII.GetString(result, 0, receiveNumber));
                    }
                    else { myClientSocket.Close();
                       }
                }
                catch
                {
                    Console.Write("Client Fail");
                    myClientSocket.Close();
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientSocket.Send(System.Text.Encoding.Default.GetBytes(textBox1.Text));
        }

        private void linuxwatchdog_Tick(object sender, EventArgs e)
        {
            linuxstateLabel.Text = "Linux子系统已失联！";
            linuxstateLabel.ForeColor = Color.Red;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }
            else { panel2.Visible = false; }
        }
    }
}

