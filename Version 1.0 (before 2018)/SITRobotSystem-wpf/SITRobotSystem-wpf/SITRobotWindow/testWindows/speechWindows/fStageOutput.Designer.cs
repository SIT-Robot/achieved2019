namespace SITRobotSystem_wpf.SITRobotWindow.testWindows.speechWindows
{
    partial class fStateOutput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.GPSRlb1 = new System.Windows.Forms.ListBox();
            this.GPSRlb2 = new System.Windows.Forms.ListBox();
            this.GPSRlb3 = new System.Windows.Forms.ListBox();
            this.GPSRlabel1 = new System.Windows.Forms.Label();
            this.GPSRlabel2 = new System.Windows.Forms.Label();
            this.GPSRlabel3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(282, 261);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // GPSRlb1
            // 
            this.GPSRlb1.FormattingEnabled = true;
            this.GPSRlb1.ItemHeight = 12;
            this.GPSRlb1.Location = new System.Drawing.Point(303, 27);
            this.GPSRlb1.Name = "GPSRlb1";
            this.GPSRlb1.Size = new System.Drawing.Size(107, 232);
            this.GPSRlb1.TabIndex = 1;
            // 
            // GPSRlb2
            // 
            this.GPSRlb2.FormattingEnabled = true;
            this.GPSRlb2.ItemHeight = 12;
            this.GPSRlb2.Location = new System.Drawing.Point(433, 27);
            this.GPSRlb2.Name = "GPSRlb2";
            this.GPSRlb2.Size = new System.Drawing.Size(107, 232);
            this.GPSRlb2.TabIndex = 2;
            // 
            // GPSRlb3
            // 
            this.GPSRlb3.FormattingEnabled = true;
            this.GPSRlb3.ItemHeight = 12;
            this.GPSRlb3.Location = new System.Drawing.Point(564, 27);
            this.GPSRlb3.Name = "GPSRlb3";
            this.GPSRlb3.Size = new System.Drawing.Size(107, 232);
            this.GPSRlb3.TabIndex = 3;
            // 
            // GPSRlabel1
            // 
            this.GPSRlabel1.AutoSize = true;
            this.GPSRlabel1.Location = new System.Drawing.Point(301, 5);
            this.GPSRlabel1.Name = "GPSRlabel1";
            this.GPSRlabel1.Size = new System.Drawing.Size(41, 12);
            this.GPSRlabel1.TabIndex = 4;
            this.GPSRlabel1.Text = "地点：";
            // 
            // GPSRlabel2
            // 
            this.GPSRlabel2.AutoSize = true;
            this.GPSRlabel2.Location = new System.Drawing.Point(431, 5);
            this.GPSRlabel2.Name = "GPSRlabel2";
            this.GPSRlabel2.Size = new System.Drawing.Size(41, 12);
            this.GPSRlabel2.TabIndex = 5;
            this.GPSRlabel2.Text = "物品：";
            // 
            // GPSRlabel3
            // 
            this.GPSRlabel3.AutoSize = true;
            this.GPSRlabel3.Location = new System.Drawing.Point(562, 5);
            this.GPSRlabel3.Name = "GPSRlabel3";
            this.GPSRlabel3.Size = new System.Drawing.Size(41, 12);
            this.GPSRlabel3.TabIndex = 6;
            this.GPSRlabel3.Text = "人名：";
            // 
            // fStateOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 262);
            this.Controls.Add(this.GPSRlabel3);
            this.Controls.Add(this.GPSRlabel2);
            this.Controls.Add(this.GPSRlabel1);
            this.Controls.Add(this.GPSRlb3);
            this.Controls.Add(this.GPSRlb2);
            this.Controls.Add(this.GPSRlb1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "fStateOutput";
            this.Text = "fStateOutput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListBox GPSRlb1;
        private System.Windows.Forms.ListBox GPSRlb2;
        private System.Windows.Forms.ListBox GPSRlb3;
        private System.Windows.Forms.Label GPSRlabel1;
        private System.Windows.Forms.Label GPSRlabel2;
        private System.Windows.Forms.Label GPSRlabel3;
    }
}