namespace Xbox_360_BadStick
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.ContinueBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.VerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.creditsLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startupProgressBar = new System.Windows.Forms.ProgressBar();
            this.startupLabel = new System.Windows.Forms.Label();
            this.warningTip = new System.Windows.Forms.ToolTip(this.components);
            this.errorTip = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.updateNotice = new System.Windows.Forms.PictureBox();
            this.discordLink = new System.Windows.Forms.PictureBox();
            this.githubpicLink = new System.Windows.Forms.PictureBox();
            this.fatalError = new System.Windows.Forms.PictureBox();
            this.noadminWarning = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.updateTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateNotice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discordLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.githubpicLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fatalError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noadminWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 19F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(123, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "BadStick";
            // 
            // ExitBtn
            // 
            this.ExitBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ExitBtn.Location = new System.Drawing.Point(347, 192);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(92, 25);
            this.ExitBtn.TabIndex = 3;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // ContinueBtn
            // 
            this.ContinueBtn.Enabled = false;
            this.ContinueBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ContinueBtn.Location = new System.Drawing.Point(249, 192);
            this.ContinueBtn.Name = "ContinueBtn";
            this.ContinueBtn.Size = new System.Drawing.Size(92, 25);
            this.ContinueBtn.TabIndex = 1;
            this.ContinueBtn.Text = "Continue";
            this.ContinueBtn.UseVisualStyleBackColor = true;
            this.ContinueBtn.Click += new System.EventHandler(this.ContinueBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VerLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 219);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(446, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // VerLabel
            // 
            this.VerLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.VerLabel.Name = "VerLabel";
            this.VerLabel.Size = new System.Drawing.Size(114, 17);
            this.VerLabel.Text = "BadStick V2.1-Stable ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 6.5F);
            this.label3.Location = new System.Drawing.Point(300, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Brought To You by The Xenon Forge";
            // 
            // creditsLabel
            // 
            this.creditsLabel.AutoSize = true;
            this.creditsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.creditsLabel.Location = new System.Drawing.Point(0, 169);
            this.creditsLabel.Name = "creditsLabel";
            this.creditsLabel.Size = new System.Drawing.Size(43, 13);
            this.creditsLabel.TabIndex = 10;
            this.creditsLabel.TabStop = true;
            this.creditsLabel.Text = "Credits";
            this.creditsLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.creditsLabel_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(222, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "v2.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(127, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(255, 30);
            this.label4.TabIndex = 12;
            this.label4.Text = "The Xbox 360 BadUpdate Automated Setup Tool.\r\nExploit Your Console In Seconds. No" +
    "t Minutes.";
            // 
            // startupProgressBar
            // 
            this.startupProgressBar.Location = new System.Drawing.Point(129, 128);
            this.startupProgressBar.Name = "startupProgressBar";
            this.startupProgressBar.Size = new System.Drawing.Size(310, 23);
            this.startupProgressBar.TabIndex = 13;
            // 
            // startupLabel
            // 
            this.startupLabel.AutoSize = true;
            this.startupLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.startupLabel.Location = new System.Drawing.Point(127, 112);
            this.startupLabel.Name = "startupLabel";
            this.startupLabel.Size = new System.Drawing.Size(94, 13);
            this.startupLabel.TabIndex = 14;
            this.startupLabel.Text = "Status: DEV_STAT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Light", 7F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(332, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "Developed By Lxcy_Dr0p";
            // 
            // updateNotice
            // 
            this.updateNotice.BackColor = System.Drawing.Color.Transparent;
            //this.updateNotice.Image = global::Xbox_360_BadStick.Properties.Resources.Check;
            this.updateNotice.Location = new System.Drawing.Point(405, 90);
            this.updateNotice.Name = "updateNotice";
            this.updateNotice.Size = new System.Drawing.Size(41, 32);
            this.updateNotice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.updateNotice.TabIndex = 21;
            this.updateNotice.TabStop = false;
            this.updateNotice.Visible = false;
            // 
            // discordLink
            // 
            this.discordLink.BackColor = System.Drawing.Color.Transparent;
            //this.discordLink.Image = global::Xbox_360_BadStick.Properties.Resources.discord;
            this.discordLink.Location = new System.Drawing.Point(43, 185);
            this.discordLink.Name = "discordLink";
            this.discordLink.Size = new System.Drawing.Size(41, 32);
            this.discordLink.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.discordLink.TabIndex = 20;
            this.discordLink.TabStop = false;
            this.discordLink.Click += new System.EventHandler(this.discordLink_Click);
            // 
            // githubpicLink
            // 
            this.githubpicLink.BackColor = System.Drawing.Color.Transparent;
            //this.githubpicLink.Image = global::Xbox_360_BadStick.Properties.Resources.github;
            this.githubpicLink.Location = new System.Drawing.Point(0, 185);
            this.githubpicLink.Name = "githubpicLink";
            this.githubpicLink.Size = new System.Drawing.Size(41, 32);
            this.githubpicLink.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.githubpicLink.TabIndex = 19;
            this.githubpicLink.TabStop = false;
            this.githubpicLink.Click += new System.EventHandler(this.githubpicLink_Click);
            // 
            // fatalError
            // 
            this.fatalError.BackColor = System.Drawing.Color.Transparent;
            //this.fatalError.Image = global::Xbox_360_BadStick.Properties.Resources.red_x_on_network_icon;
            this.fatalError.Location = new System.Drawing.Point(405, 90);
            this.fatalError.Name = "fatalError";
            this.fatalError.Size = new System.Drawing.Size(41, 32);
            this.fatalError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.fatalError.TabIndex = 16;
            this.fatalError.TabStop = false;
            this.fatalError.Visible = false;
            // 
            // noadminWarning
            // 
            this.noadminWarning.BackColor = System.Drawing.Color.Transparent;
            //this.noadminWarning.Image = global::Xbox_360_BadStick.Properties.Resources.Security_And_Maintenance_Alert_Windows_10_version_21H2;
            this.noadminWarning.Location = new System.Drawing.Point(405, 90);
            this.noadminWarning.Name = "noadminWarning";
            this.noadminWarning.Size = new System.Drawing.Size(41, 32);
            this.noadminWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.noadminWarning.TabIndex = 15;
            this.noadminWarning.TabStop = false;
            this.noadminWarning.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Xbox_360_BadStick.Properties.Resources.BadStick_Tilted;
            this.pictureBox1.Location = new System.Drawing.Point(21, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(94, 86);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 241);
            this.Controls.Add(this.updateNotice);
            this.Controls.Add(this.discordLink);
            this.Controls.Add(this.githubpicLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fatalError);
            this.Controls.Add(this.noadminWarning);
            this.Controls.Add(this.startupLabel);
            this.Controls.Add(this.startupProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.creditsLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ContinueBtn);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BadStick";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateNotice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discordLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.githubpicLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fatalError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noadminWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Button ContinueBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel VerLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel creditsLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar startupProgressBar;
        private System.Windows.Forms.Label startupLabel;
        private System.Windows.Forms.PictureBox noadminWarning;
        private System.Windows.Forms.PictureBox fatalError;
        private System.Windows.Forms.ToolTip warningTip;
        private System.Windows.Forms.ToolTip errorTip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox githubpicLink;
        private System.Windows.Forms.PictureBox discordLink;
        private System.Windows.Forms.PictureBox updateNotice;
        private System.Windows.Forms.ToolTip updateTip;
    }
}

