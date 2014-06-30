namespace Burt_ControlApp
{
    partial class Burt
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
            this.pictureOfBurt = new System.Windows.Forms.PictureBox();
            this.IPV4_0_1 = new System.Windows.Forms.TextBox();
            this.IPV4_2 = new System.Windows.Forms.TextBox();
            this.IPV4_3 = new System.Windows.Forms.TextBox();
            this.IPV4_ListeningPort = new System.Windows.Forms.TextBox();
            this.IPV4_link = new System.Windows.Forms.LinkLabel();
            this.IPV4_ListeningPort_link = new System.Windows.Forms.LinkLabel();
            this.distanceSensorProgressBar = new System.Windows.Forms.ProgressBar();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.leftMotorProgressBar = new System.Windows.Forms.ProgressBar();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.rightMotorProgressBar = new System.Windows.Forms.ProgressBar();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.connectedRadioButton1 = new System.Windows.Forms.RadioButton();
            this.connectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.progressBar4 = new System.Windows.Forms.ProgressBar();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.progressBar5 = new System.Windows.Forms.ProgressBar();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.progressBar6 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.progressBar7 = new System.Windows.Forms.ProgressBar();
            this.linkLabel10 = new System.Windows.Forms.LinkLabel();
            this.progressBar8 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabel11 = new System.Windows.Forms.LinkLabel();
            this.progressBar9 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOfBurt)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureOfBurt
            // 
            this.pictureOfBurt.Image = global::Burt_ControlApp.Properties.Resources.Photo0060;
            this.pictureOfBurt.Location = new System.Drawing.Point(12, 12);
            this.pictureOfBurt.Name = "pictureOfBurt";
            this.pictureOfBurt.Size = new System.Drawing.Size(322, 428);
            this.pictureOfBurt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureOfBurt.TabIndex = 0;
            this.pictureOfBurt.TabStop = false;
            // 
            // IPV4_0_1
            // 
            this.IPV4_0_1.Location = new System.Drawing.Point(466, 12);
            this.IPV4_0_1.Name = "IPV4_0_1";
            this.IPV4_0_1.Size = new System.Drawing.Size(50, 20);
            this.IPV4_0_1.TabIndex = 1;
            this.IPV4_0_1.Text = "127.0";
            this.IPV4_0_1.TextChanged += new System.EventHandler(this.IPV4_0_1_TextChanged);
            // 
            // IPV4_2
            // 
            this.IPV4_2.Location = new System.Drawing.Point(522, 12);
            this.IPV4_2.Name = "IPV4_2";
            this.IPV4_2.Size = new System.Drawing.Size(25, 20);
            this.IPV4_2.TabIndex = 2;
            this.IPV4_2.Text = "0";
            this.IPV4_2.TextChanged += new System.EventHandler(this.IPV4_2_TextChanged);
            // 
            // IPV4_3
            // 
            this.IPV4_3.Location = new System.Drawing.Point(553, 12);
            this.IPV4_3.Name = "IPV4_3";
            this.IPV4_3.Size = new System.Drawing.Size(25, 20);
            this.IPV4_3.TabIndex = 3;
            this.IPV4_3.Text = "1";
            this.IPV4_3.TextChanged += new System.EventHandler(this.IPV4_3_TextChanged);
            // 
            // IPV4_ListeningPort
            // 
            this.IPV4_ListeningPort.Location = new System.Drawing.Point(616, 12);
            this.IPV4_ListeningPort.Name = "IPV4_ListeningPort";
            this.IPV4_ListeningPort.Size = new System.Drawing.Size(50, 20);
            this.IPV4_ListeningPort.TabIndex = 4;
            this.IPV4_ListeningPort.Text = "80";
            this.IPV4_ListeningPort.TextChanged += new System.EventHandler(this.IPV4_ListeningPort_TextChanged);
            // 
            // IPV4_link
            // 
            this.IPV4_link.AutoSize = true;
            this.IPV4_link.Location = new System.Drawing.Point(390, 15);
            this.IPV4_link.Name = "IPV4_link";
            this.IPV4_link.Size = new System.Drawing.Size(71, 13);
            this.IPV4_link.TabIndex = 5;
            this.IPV4_link.TabStop = true;
            this.IPV4_link.Text = "IPV4 Address";
            this.IPV4_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.IPV4_0_1_link_LinkClicked);
            // 
            // IPV4_ListeningPort_link
            // 
            this.IPV4_ListeningPort_link.AutoSize = true;
            this.IPV4_ListeningPort_link.Location = new System.Drawing.Point(584, 15);
            this.IPV4_ListeningPort_link.Name = "IPV4_ListeningPort_link";
            this.IPV4_ListeningPort_link.Size = new System.Drawing.Size(26, 13);
            this.IPV4_ListeningPort_link.TabIndex = 8;
            this.IPV4_ListeningPort_link.TabStop = true;
            this.IPV4_ListeningPort_link.Text = "Port";
            this.IPV4_ListeningPort_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.IPV4_ListeningPort_LinkClicked);
            // 
            // distanceSensorProgressBar
            // 
            this.distanceSensorProgressBar.Location = new System.Drawing.Point(466, 311);
            this.distanceSensorProgressBar.Name = "distanceSensorProgressBar";
            this.distanceSensorProgressBar.Size = new System.Drawing.Size(200, 23);
            this.distanceSensorProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.distanceSensorProgressBar.TabIndex = 9;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(340, 321);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(120, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Distance Sensor: L to H";
            // 
            // leftMotorProgressBar
            // 
            this.leftMotorProgressBar.Location = new System.Drawing.Point(466, 234);
            this.leftMotorProgressBar.Name = "leftMotorProgressBar";
            this.leftMotorProgressBar.Size = new System.Drawing.Size(100, 23);
            this.leftMotorProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.leftMotorProgressBar.TabIndex = 11;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(340, 244);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 12;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Left Motor";
            // 
            // rightMotorProgressBar
            // 
            this.rightMotorProgressBar.Location = new System.Drawing.Point(466, 263);
            this.rightMotorProgressBar.Name = "rightMotorProgressBar";
            this.rightMotorProgressBar.Size = new System.Drawing.Size(100, 23);
            this.rightMotorProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.rightMotorProgressBar.TabIndex = 13;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(340, 273);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(62, 13);
            this.linkLabel3.TabIndex = 14;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Right Motor";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(566, 234);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(100, 23);
            this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar2.TabIndex = 15;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(566, 263);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 16;
            // 
            // connectedRadioButton1
            // 
            this.connectedRadioButton1.AutoSize = true;
            this.connectedRadioButton1.Enabled = false;
            this.connectedRadioButton1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.connectedRadioButton1.Location = new System.Drawing.Point(594, 422);
            this.connectedRadioButton1.Name = "connectedRadioButton1";
            this.connectedRadioButton1.Size = new System.Drawing.Size(14, 13);
            this.connectedRadioButton1.TabIndex = 17;
            this.connectedRadioButton1.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            this.connectButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.connectButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.connectButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectButton.Location = new System.Drawing.Point(466, 38);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(200, 23);
            this.connectButton.TabIndex = 18;
            this.connectButton.Text = "Connect to Burt";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.connectButton_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(612, 422);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Heartbeat";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(340, 347);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(96, 13);
            this.linkLabel4.TabIndex = 20;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Temperature \'C (\'F)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(466, 344);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(50, 20);
            this.textBox1.TabIndex = 21;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(566, 186);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(100, 23);
            this.progressBar3.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar3.TabIndex = 27;
            // 
            // progressBar4
            // 
            this.progressBar4.Location = new System.Drawing.Point(566, 157);
            this.progressBar4.Name = "progressBar4";
            this.progressBar4.Size = new System.Drawing.Size(100, 23);
            this.progressBar4.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar4.TabIndex = 26;
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(340, 196);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(98, 13);
            this.linkLabel5.TabIndex = 25;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Pan Servo (bottom)";
            // 
            // progressBar5
            // 
            this.progressBar5.Location = new System.Drawing.Point(466, 186);
            this.progressBar5.Name = "progressBar5";
            this.progressBar5.Size = new System.Drawing.Size(100, 23);
            this.progressBar5.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar5.TabIndex = 24;
            // 
            // linkLabel6
            // 
            this.linkLabel6.AutoSize = true;
            this.linkLabel6.Location = new System.Drawing.Point(340, 167);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(76, 13);
            this.linkLabel6.TabIndex = 23;
            this.linkLabel6.TabStop = true;
            this.linkLabel6.Text = "Tilt Servo (top)";
            // 
            // progressBar6
            // 
            this.progressBar6.Location = new System.Drawing.Point(466, 157);
            this.progressBar6.Name = "progressBar6";
            this.progressBar6.Size = new System.Drawing.Size(100, 23);
            this.progressBar6.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar6.TabIndex = 22;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(343, 422);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // linkLabel7
            // 
            this.linkLabel7.AutoSize = true;
            this.linkLabel7.Location = new System.Drawing.Point(364, 422);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(89, 13);
            this.linkLabel7.TabIndex = 29;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "Switch to Manual";
            // 
            // linkLabel8
            // 
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Location = new System.Drawing.Point(340, 114);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(78, 13);
            this.linkLabel8.TabIndex = 30;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "Head Direction";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(466, 111);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(50, 20);
            this.textBox2.TabIndex = 31;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(516, 111);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(50, 20);
            this.textBox3.TabIndex = 32;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(616, 111);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(50, 20);
            this.textBox4.TabIndex = 34;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(566, 111);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(50, 20);
            this.textBox5.TabIndex = 33;
            // 
            // linkLabel9
            // 
            this.linkLabel9.AutoSize = true;
            this.linkLabel9.Location = new System.Drawing.Point(395, 137);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(65, 13);
            this.linkLabel9.TabIndex = 36;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "Battery level";
            this.linkLabel9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar7
            // 
            this.progressBar7.ForeColor = System.Drawing.Color.Red;
            this.progressBar7.Location = new System.Drawing.Point(466, 138);
            this.progressBar7.Name = "progressBar7";
            this.progressBar7.Size = new System.Drawing.Size(200, 12);
            this.progressBar7.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar7.TabIndex = 35;
            // 
            // linkLabel10
            // 
            this.linkLabel10.AutoSize = true;
            this.linkLabel10.Location = new System.Drawing.Point(395, 291);
            this.linkLabel10.Name = "linkLabel10";
            this.linkLabel10.Size = new System.Drawing.Size(65, 13);
            this.linkLabel10.TabIndex = 38;
            this.linkLabel10.TabStop = true;
            this.linkLabel10.Text = "Battery level";
            this.linkLabel10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar8
            // 
            this.progressBar8.ForeColor = System.Drawing.Color.Red;
            this.progressBar8.Location = new System.Drawing.Point(466, 292);
            this.progressBar8.Name = "progressBar8";
            this.progressBar8.Size = new System.Drawing.Size(200, 12);
            this.progressBar8.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar8.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(483, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "w";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(534, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "x";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(583, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(634, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "z";
            // 
            // linkLabel11
            // 
            this.linkLabel11.AutoSize = true;
            this.linkLabel11.Location = new System.Drawing.Point(395, 66);
            this.linkLabel11.Name = "linkLabel11";
            this.linkLabel11.Size = new System.Drawing.Size(65, 13);
            this.linkLabel11.TabIndex = 44;
            this.linkLabel11.TabStop = true;
            this.linkLabel11.Text = "Battery level";
            this.linkLabel11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar9
            // 
            this.progressBar9.ForeColor = System.Drawing.Color.Red;
            this.progressBar9.Location = new System.Drawing.Point(466, 67);
            this.progressBar9.Name = "progressBar9";
            this.progressBar9.Size = new System.Drawing.Size(200, 12);
            this.progressBar9.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar9.TabIndex = 43;
            // 
            // Burt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(680, 452);
            this.Controls.Add(this.linkLabel11);
            this.Controls.Add(this.progressBar9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel10);
            this.Controls.Add(this.progressBar8);
            this.Controls.Add(this.linkLabel9);
            this.Controls.Add(this.progressBar7);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.linkLabel8);
            this.Controls.Add(this.linkLabel7);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar4);
            this.Controls.Add(this.linkLabel5);
            this.Controls.Add(this.progressBar5);
            this.Controls.Add(this.linkLabel6);
            this.Controls.Add(this.progressBar6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.connectedRadioButton1);
            this.Controls.Add(this.pictureOfBurt);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.rightMotorProgressBar);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.leftMotorProgressBar);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.distanceSensorProgressBar);
            this.Controls.Add(this.IPV4_ListeningPort_link);
            this.Controls.Add(this.IPV4_link);
            this.Controls.Add(this.IPV4_ListeningPort);
            this.Controls.Add(this.IPV4_3);
            this.Controls.Add(this.IPV4_2);
            this.Controls.Add(this.IPV4_0_1);
            this.Name = "Burt";
            this.Text = "Burt";
            this.Load += new System.EventHandler(this.Burt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOfBurt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureOfBurt;
        private System.Windows.Forms.TextBox IPV4_0_1;
        private System.Windows.Forms.TextBox IPV4_2;
        private System.Windows.Forms.TextBox IPV4_3;
        private System.Windows.Forms.TextBox IPV4_ListeningPort;
        private System.Windows.Forms.LinkLabel IPV4_link;
        private System.Windows.Forms.LinkLabel IPV4_ListeningPort_link;
        private System.Windows.Forms.ProgressBar distanceSensorProgressBar;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ProgressBar leftMotorProgressBar;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.ProgressBar rightMotorProgressBar;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton connectedRadioButton1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.ProgressBar progressBar4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.ProgressBar progressBar5;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.ProgressBar progressBar6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.LinkLabel linkLabel9;
        private System.Windows.Forms.ProgressBar progressBar7;
        private System.Windows.Forms.LinkLabel linkLabel10;
        private System.Windows.Forms.ProgressBar progressBar8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel11;
        private System.Windows.Forms.ProgressBar progressBar9;
    }
}

