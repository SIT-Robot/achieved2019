namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows
{
    partial class SpeechTestForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnBaseSpeech = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rtbStageOutput = new System.Windows.Forms.RichTextBox();
            this.btnGpsrSPeech = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnRestaurant = new System.Windows.Forms.Button();
            this.btnWhoIsWho = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btnHome = new System.Windows.Forms.Button();
            this.btnIntlRes = new System.Windows.Forms.Button();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.GPSR2015Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBaseSpeech
            // 
            this.btnBaseSpeech.Location = new System.Drawing.Point(12, -4);
            this.btnBaseSpeech.Name = "btnBaseSpeech";
            this.btnBaseSpeech.Size = new System.Drawing.Size(75, 52);
            this.btnBaseSpeech.TabIndex = 0;
            this.btnBaseSpeech.Text = "baseSpeech";
            this.btnBaseSpeech.UseVisualStyleBackColor = true;
            this.btnBaseSpeech.Click += new System.EventHandler(this.btnBaseSpeech_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rtbStageOutput
            // 
            this.rtbStageOutput.Location = new System.Drawing.Point(293, 10);
            this.rtbStageOutput.Name = "rtbStageOutput";
            this.rtbStageOutput.Size = new System.Drawing.Size(189, 247);
            this.rtbStageOutput.TabIndex = 1;
            this.rtbStageOutput.Text = "";
            // 
            // btnGpsrSPeech
            // 
            this.btnGpsrSPeech.Location = new System.Drawing.Point(12, 54);
            this.btnGpsrSPeech.Name = "btnGpsrSPeech";
            this.btnGpsrSPeech.Size = new System.Drawing.Size(75, 52);
            this.btnGpsrSPeech.TabIndex = 2;
            this.btnGpsrSPeech.Text = "gpsrSPeech";
            this.btnGpsrSPeech.UseVisualStyleBackColor = true;
            this.btnGpsrSPeech.Click += new System.EventHandler(this.btnGpsrSPeech_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(67, 236);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(61, 23);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(93, 83);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(61, 23);
            this.btnRestart.TabIndex = 4;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnRestaurant
            // 
            this.btnRestaurant.Location = new System.Drawing.Point(12, 112);
            this.btnRestaurant.Name = "btnRestaurant";
            this.btnRestaurant.Size = new System.Drawing.Size(75, 52);
            this.btnRestaurant.TabIndex = 5;
            this.btnRestaurant.Text = "restaurantSPeech";
            this.btnRestaurant.UseVisualStyleBackColor = true;
            this.btnRestaurant.Click += new System.EventHandler(this.btnRestaurant_Click);
            // 
            // btnWhoIsWho
            // 
            this.btnWhoIsWho.Location = new System.Drawing.Point(12, 170);
            this.btnWhoIsWho.Name = "btnWhoIsWho";
            this.btnWhoIsWho.Size = new System.Drawing.Size(75, 52);
            this.btnWhoIsWho.TabIndex = 6;
            this.btnWhoIsWho.Text = "WhoIsWho";
            this.btnWhoIsWho.UseVisualStyleBackColor = true;
            this.btnWhoIsWho.Click += new System.EventHandler(this.btnWhoIsWho_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(169, -4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(75, 52);
            this.btnHome.TabIndex = 7;
            this.btnHome.Text = "Home  Accident";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnIntlRes
            // 
            this.btnIntlRes.Location = new System.Drawing.Point(169, 54);
            this.btnIntlRes.Name = "btnIntlRes";
            this.btnIntlRes.Size = new System.Drawing.Size(75, 52);
            this.btnIntlRes.TabIndex = 8;
            this.btnIntlRes.Text = "IntlRes";
            this.btnIntlRes.UseVisualStyleBackColor = true;
            this.btnIntlRes.Click += new System.EventHandler(this.btnIntlRes_Click);
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 45);
            this.button1.TabIndex = 9;
            this.button1.Text = "show";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GPSR2015Btn
            // 
            this.GPSR2015Btn.Location = new System.Drawing.Point(169, 170);
            this.GPSR2015Btn.Name = "GPSR2015Btn";
            this.GPSR2015Btn.Size = new System.Drawing.Size(75, 52);
            this.GPSR2015Btn.TabIndex = 10;
            this.GPSR2015Btn.Text = "GPSR2015";
            this.GPSR2015Btn.UseVisualStyleBackColor = true;
            this.GPSR2015Btn.Click += new System.EventHandler(this.GPSR2015Btn_Click);
            // 
            // SpeechTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 273);
            this.Controls.Add(this.GPSR2015Btn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnIntlRes);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.btnWhoIsWho);
            this.Controls.Add(this.btnRestaurant);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnGpsrSPeech);
            this.Controls.Add(this.rtbStageOutput);
            this.Controls.Add(this.btnBaseSpeech);
            this.Name = "SpeechTestForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBaseSpeech;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RichTextBox rtbStageOutput;
        private System.Windows.Forms.Button btnGpsrSPeech;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnRestaurant;
        private System.Windows.Forms.Button btnWhoIsWho;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnIntlRes;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button GPSR2015Btn;
    }
}

