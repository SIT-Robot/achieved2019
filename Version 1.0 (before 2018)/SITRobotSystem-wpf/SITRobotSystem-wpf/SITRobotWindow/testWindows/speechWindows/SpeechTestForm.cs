using System;
using System.Linq;
using System.Windows.Forms;
using SITRobotSystem_wpf.BLL.ServiceCtrl;

namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows
{
    public partial class SpeechTestForm : Form
    {
        private SitRobotSpeech spBase = new SitRobotSpeech();
        string strStage = "-1"; 
        string[] place = new string[] { "bed room","living room","dinning room","kitchen"};
        string[] objectsGPSR = new string[] { "sofa", "bed", "conference table", "trash" };
        string[] thing = new string[] { "toothpaste","napkin","soap","shampoo","mushroom sauce",
                "caramel treat","sardines","spice sauce","tomato sauce","porridge",
                "beverage","mineral water","nut milk","juce","milk",
                "sprite","green tea","peanut","chewing gum","pretzel",
                "biscuit","chips"};
        string[] name = new string[] { "Michael", "John", "Pete5r", "Tom", "Sandy", 
                "Kitty", "Angela", "Ben", "Jobs", "Jack" };

        string[] point = new string[] { "first location", "second location", "third location", "forth location", "five location" };
        int restaurantTimes = 0;//shopping 记点次数

        string[] objects = new string[] { "drink", "snack", "location one", "location two", "location three", "checkout" };
        string[] drink = new string[] { "water", "juice", "green tea", "milk" };
        string[] snack = new string[] { "chips", "peanut", "biscuit", "chewing gum" };
        public SpeechTestForm()
        {
            InitializeComponent();
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
        }
        private void btnBaseSpeech_Click(object sender, EventArgs e)
        {
            rtbStageOutput.Text += "已选择basic function项目语音识别\n";
            spBase.baseRecognize();           
            rtbStageOutput.Text += "启动完毕\n";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (spBase.ReturnCommand!=null)
            {
                //if (point.Contains(spBase.ReturnCommand))
                //{
                //    restaurantTimes++;
                //    this.rtbStageOutput.Text += "第" + restaurantTimes + "次：\n";
                //    this.rtbStageOutput.Text += spBase.ReturnCommand + "\n";                    
                //    //spBase.ReturnCommand = null;
                //    if (restaurantTimes < 3)
                //    {
                //        spBase = new SitRobotSpeech();
                //        spBase.robotSpeak("I'm ready");
                //        spBase.restaurantRecognize(point);
                //        this.rtbStageOutput.Text += "测试结束\n";

                //    }
                //    spBase.ReturnCommand = null;
                 
                //}
                this.rtbStageOutput.Text += spBase.ReturnCommand;
                spBase = null;
                timer1.Enabled = false;                
            }
        }

        private void btnGpsrSPeech_Click(object sender, EventArgs e)
        {
            timer1.Start();
            spBase = new SitRobotSpeech();
            spBase.robotSpeak("I'm ready");
            rtbStageOutput.Text += "已选择GPSR项目语音识别\n";
            spBase.GPSR2015Test(thing,objectsGPSR,place,name);
            rtbStageOutput.Text += "启动完毕\n";
            btnRestart.Visible = true;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if(btnPause.Text=="Pause")
            {
                spBase.speechPause();
                btnPause.Text = "Continue";
            }
            else if (btnPause.Text == "Continue")
            {
                spBase.speechContinue();
                btnPause.Text = "Pause";
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            spBase.speechStop();
            
            GC.Collect();
            spBase = new SitRobotSpeech();
            spBase.gpsrRecogize(place, thing);
            rtbStageOutput.Text += "启动完毕\n";
            btnPause.Text = "Pause";
        }

        private void btnRestaurant_Click(object sender, EventArgs e)
        {
            timer1.Start();
            spBase.robotSpeak("I'm ready");
            rtbStageOutput.Text += "已选择Restaurant项目语音识别\n";
            spBase.restaurantRecognize(point);
            rtbStageOutput.Text += "启动完毕\n";
            //while((spBase.ReturnCommand == null || spBase.ReturnCommand.Length == 0)&&restaurant<5)
            //{
                
            //}
            //spBase.speechStop();
        }

        private void btnWhoIsWho_Click(object sender, EventArgs e)
        {
            timer2.Start();
            spBase.robotSpeak("I'm ready");
            rtbStageOutput.Text += "已选择WhoIsWho项目语音识别\n";
            spBase.askName();
            rtbStageOutput.Text += "启动完毕\n";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (spBase.ReturnCommand!=null)
            {
                restaurantTimes++;
                this.rtbStageOutput.Text += "第" + restaurantTimes + "次：\n";
                this.rtbStageOutput.Text += spBase.ReturnName + "\n";
                this.rtbStageOutput.Text += spBase.ReturnCommand + "\n";
                spBase.ReturnCommand = null;
                if (restaurantTimes < 3)
                {
                    spBase = new SitRobotSpeech();
                    spBase.robotSpeak("I'm ready");
                    spBase.whoIsWhoRecogine(name, drink);
                    this.rtbStageOutput.Text += "测试结束\n";
                    spBase.ReturnCommand = null;
                }
                spBase.ReturnCommand = null;
            }
            else
            {
                //this.rtbStageOutput.Text += spBase.ReturnCommand + "\n";
                //spBase.speechStop();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            spBase = new SitRobotSpeech();
            spBase.robotSpeak("I'm ready");
            spBase.homeRecognize();
     
            this.rtbStageOutput.Text += "测试结束\n";
            this.rtbStageOutput.Text += spBase.ReturnCommand + "\n";
        }

        private void btnIntlRes_Click(object sender, EventArgs e)
        {
            spBase = new SitRobotSpeech();
            spBase.robotSpeak("I'm ready");
            thing = new string[drink.Length + snack.Length];
            int i = 0;
            for (; i < drink.Length; i++)
            {
                thing[i] = drink[i];
            }
            for (; i < drink.Length + snack.Length; i++)
            {
                thing[i] = snack[i - snack.Length];
            }

            timer3.Start();
            spBase.intlResRecognize(objects,thing);

            this.rtbStageOutput.Text += "测试结束\n";
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (spBase.ReturnCommand!=null)
            {
                int i = 0;
                this.rtbStageOutput.Text += spBase.ReIntlRes[i] + "\n";
                i++;
                this.rtbStageOutput.Text += spBase.ReIntlRes[i] + "\n";
                i++;
                this.rtbStageOutput.Text += spBase.ReIntlRes[i] + "\n";
                i++;
                this.rtbStageOutput.Text += spBase.ReIntlRes[i] + "\n";
                i++;
                this.rtbStageOutput.Text += spBase.ReIntlRes[i] + "\n";
                spBase.ReturnCommand = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            spBase = new SitRobotSpeech();
            spBase.showSpeechRecognize();
            spBase.robotSpeak("I'm ready");
        }

        private void GPSR2015Btn_Click(object sender, EventArgs e)
        {

        }
    }
}
