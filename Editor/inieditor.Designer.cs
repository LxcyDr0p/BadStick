namespace Xbox_360_BadStick
{
    partial class inieditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inieditor));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.launchReturn = new System.Windows.Forms.Button();
            this.newlaunchiniBtn = new System.Windows.Forms.Button();
            this.launchiniOpen = new System.Windows.Forms.Button();
            this.launchiniClear = new System.Windows.Forms.Button();
            this.launchiniSave = new System.Windows.Forms.Button();
            this.launchinieditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.jrpcReturn = new System.Windows.Forms.Button();
            this.newjrpciniBtn = new System.Windows.Forms.Button();
            this.jrpcOpen = new System.Windows.Forms.Button();
            this.jrpcClear = new System.Windows.Forms.Button();
            this.jrpcSave = new System.Windows.Forms.Button();
            this.jrpcinieditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.xbdmReturn = new System.Windows.Forms.Button();
            this.newxbdminiBtn = new System.Windows.Forms.Button();
            this.xbdmOpen = new System.Windows.Forms.Button();
            this.xbdmClear = new System.Windows.Forms.Button();
            this.xbdmSave = new System.Windows.Forms.Button();
            this.xbdminieditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.VerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.launchinieditor)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jrpcinieditor)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xbdminieditor)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(738, 416);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.launchReturn);
            this.tabPage1.Controls.Add(this.newlaunchiniBtn);
            this.tabPage1.Controls.Add(this.launchiniOpen);
            this.tabPage1.Controls.Add(this.launchiniClear);
            this.tabPage1.Controls.Add(this.launchiniSave);
            this.tabPage1.Controls.Add(this.launchinieditor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(730, 390);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Launch.ini";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // launchReturn
            // 
            this.launchReturn.Location = new System.Drawing.Point(636, 339);
            this.launchReturn.Name = "launchReturn";
            this.launchReturn.Size = new System.Drawing.Size(86, 45);
            this.launchReturn.TabIndex = 14;
            this.launchReturn.Text = "Return";
            this.launchReturn.UseVisualStyleBackColor = true;
            this.launchReturn.Click += new System.EventHandler(this.launchReturn_Click);
            // 
            // newlaunchiniBtn
            // 
            this.newlaunchiniBtn.Location = new System.Drawing.Point(636, 120);
            this.newlaunchiniBtn.Name = "newlaunchiniBtn";
            this.newlaunchiniBtn.Size = new System.Drawing.Size(86, 45);
            this.newlaunchiniBtn.TabIndex = 5;
            this.newlaunchiniBtn.Text = "New Launch.ini";
            this.newlaunchiniBtn.UseVisualStyleBackColor = true;
            this.newlaunchiniBtn.Click += new System.EventHandler(this.newlaunchiniBtn_Click);
            // 
            // launchiniOpen
            // 
            this.launchiniOpen.Location = new System.Drawing.Point(636, 82);
            this.launchiniOpen.Name = "launchiniOpen";
            this.launchiniOpen.Size = new System.Drawing.Size(86, 32);
            this.launchiniOpen.TabIndex = 4;
            this.launchiniOpen.Text = "Open";
            this.launchiniOpen.UseVisualStyleBackColor = true;
            this.launchiniOpen.Click += new System.EventHandler(this.launchiniOpen_Click);
            // 
            // launchiniClear
            // 
            this.launchiniClear.Location = new System.Drawing.Point(636, 44);
            this.launchiniClear.Name = "launchiniClear";
            this.launchiniClear.Size = new System.Drawing.Size(86, 32);
            this.launchiniClear.TabIndex = 2;
            this.launchiniClear.Text = "Clear";
            this.launchiniClear.UseVisualStyleBackColor = true;
            this.launchiniClear.Click += new System.EventHandler(this.launchiniClear_Click);
            // 
            // launchiniSave
            // 
            this.launchiniSave.Location = new System.Drawing.Point(636, 6);
            this.launchiniSave.Name = "launchiniSave";
            this.launchiniSave.Size = new System.Drawing.Size(86, 32);
            this.launchiniSave.TabIndex = 1;
            this.launchiniSave.Text = "Save";
            this.launchiniSave.UseVisualStyleBackColor = true;
            this.launchiniSave.Click += new System.EventHandler(this.launchiniSave_Click);
            // 
            // launchinieditor
            // 
            this.launchinieditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.launchinieditor.AutoIndentCharsPatterns = "\n^\\s*\\$[\\w\\.\\[\\]\\\'\\\"]+\\s*(?<range>=)\\s*(?<range>[^;]+);\n";
            this.launchinieditor.AutoScrollMinSize = new System.Drawing.Size(25, 12);
            this.launchinieditor.BackBrush = null;
            this.launchinieditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.launchinieditor.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.launchinieditor.CharHeight = 12;
            this.launchinieditor.CharWidth = 7;
            this.launchinieditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.launchinieditor.DefaultMarkerSize = 8;
            this.launchinieditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.launchinieditor.Font = new System.Drawing.Font("Courier New", 8F);
            this.launchinieditor.IsReplaceMode = false;
            this.launchinieditor.Language = FastColoredTextBoxNS.Language.PHP;
            this.launchinieditor.LeftBracket = '(';
            this.launchinieditor.LeftBracket2 = '{';
            this.launchinieditor.Location = new System.Drawing.Point(4, 4);
            this.launchinieditor.Name = "launchinieditor";
            this.launchinieditor.Paddings = new System.Windows.Forms.Padding(0);
            this.launchinieditor.RightBracket = ')';
            this.launchinieditor.RightBracket2 = '}';
            this.launchinieditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.launchinieditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("launchinieditor.ServiceColors")));
            this.launchinieditor.Size = new System.Drawing.Size(625, 380);
            this.launchinieditor.TabIndex = 0;
            this.launchinieditor.Zoom = 100;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.jrpcReturn);
            this.tabPage2.Controls.Add(this.newjrpciniBtn);
            this.tabPage2.Controls.Add(this.jrpcOpen);
            this.tabPage2.Controls.Add(this.jrpcClear);
            this.tabPage2.Controls.Add(this.jrpcSave);
            this.tabPage2.Controls.Add(this.jrpcinieditor);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(730, 390);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "JRPC.ini";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // jrpcReturn
            // 
            this.jrpcReturn.Location = new System.Drawing.Point(638, 339);
            this.jrpcReturn.Name = "jrpcReturn";
            this.jrpcReturn.Size = new System.Drawing.Size(86, 45);
            this.jrpcReturn.TabIndex = 14;
            this.jrpcReturn.Text = "Return";
            this.jrpcReturn.UseVisualStyleBackColor = true;
            this.jrpcReturn.Click += new System.EventHandler(this.jrpcReturn_Click);
            // 
            // newjrpciniBtn
            // 
            this.newjrpciniBtn.Location = new System.Drawing.Point(638, 121);
            this.newjrpciniBtn.Name = "newjrpciniBtn";
            this.newjrpciniBtn.Size = new System.Drawing.Size(86, 45);
            this.newjrpciniBtn.TabIndex = 11;
            this.newjrpciniBtn.Text = "New JRPC.ini";
            this.newjrpciniBtn.UseVisualStyleBackColor = true;
            this.newjrpciniBtn.Click += new System.EventHandler(this.newjrpciniBtn_Click);
            // 
            // jrpcOpen
            // 
            this.jrpcOpen.Location = new System.Drawing.Point(638, 83);
            this.jrpcOpen.Name = "jrpcOpen";
            this.jrpcOpen.Size = new System.Drawing.Size(86, 32);
            this.jrpcOpen.TabIndex = 10;
            this.jrpcOpen.Text = "Open";
            this.jrpcOpen.UseVisualStyleBackColor = true;
            this.jrpcOpen.Click += new System.EventHandler(this.jrpcOpen_Click);
            // 
            // jrpcClear
            // 
            this.jrpcClear.Location = new System.Drawing.Point(638, 45);
            this.jrpcClear.Name = "jrpcClear";
            this.jrpcClear.Size = new System.Drawing.Size(86, 32);
            this.jrpcClear.TabIndex = 8;
            this.jrpcClear.Text = "Clear";
            this.jrpcClear.UseVisualStyleBackColor = true;
            this.jrpcClear.Click += new System.EventHandler(this.jrpcClear_Click);
            // 
            // jrpcSave
            // 
            this.jrpcSave.Location = new System.Drawing.Point(638, 7);
            this.jrpcSave.Name = "jrpcSave";
            this.jrpcSave.Size = new System.Drawing.Size(86, 32);
            this.jrpcSave.TabIndex = 7;
            this.jrpcSave.Text = "Save";
            this.jrpcSave.UseVisualStyleBackColor = true;
            this.jrpcSave.Click += new System.EventHandler(this.jrpcSave_Click);
            // 
            // jrpcinieditor
            // 
            this.jrpcinieditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.jrpcinieditor.AutoIndentCharsPatterns = "\n^\\s*\\$[\\w\\.\\[\\]\\\'\\\"]+\\s*(?<range>=)\\s*(?<range>[^;]+);\n";
            this.jrpcinieditor.AutoScrollMinSize = new System.Drawing.Size(2, 12);
            this.jrpcinieditor.BackBrush = null;
            this.jrpcinieditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jrpcinieditor.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.jrpcinieditor.CharHeight = 12;
            this.jrpcinieditor.CharWidth = 7;
            this.jrpcinieditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jrpcinieditor.DefaultMarkerSize = 8;
            this.jrpcinieditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.jrpcinieditor.Font = new System.Drawing.Font("Courier New", 8F);
            this.jrpcinieditor.IsReplaceMode = false;
            this.jrpcinieditor.Language = FastColoredTextBoxNS.Language.PHP;
            this.jrpcinieditor.LeftBracket = '(';
            this.jrpcinieditor.LeftBracket2 = '{';
            this.jrpcinieditor.Location = new System.Drawing.Point(4, 4);
            this.jrpcinieditor.Name = "jrpcinieditor";
            this.jrpcinieditor.Paddings = new System.Windows.Forms.Padding(0);
            this.jrpcinieditor.RightBracket = ')';
            this.jrpcinieditor.RightBracket2 = '}';
            this.jrpcinieditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.jrpcinieditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("jrpcinieditor.ServiceColors")));
            this.jrpcinieditor.Size = new System.Drawing.Size(625, 380);
            this.jrpcinieditor.TabIndex = 6;
            this.jrpcinieditor.Zoom = 100;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.xbdmReturn);
            this.tabPage3.Controls.Add(this.newxbdminiBtn);
            this.tabPage3.Controls.Add(this.xbdmOpen);
            this.tabPage3.Controls.Add(this.xbdmClear);
            this.tabPage3.Controls.Add(this.xbdmSave);
            this.tabPage3.Controls.Add(this.xbdminieditor);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(730, 390);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "XBDM.ini";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // xbdmReturn
            // 
            this.xbdmReturn.Location = new System.Drawing.Point(638, 339);
            this.xbdmReturn.Name = "xbdmReturn";
            this.xbdmReturn.Size = new System.Drawing.Size(86, 45);
            this.xbdmReturn.TabIndex = 13;
            this.xbdmReturn.Text = "Return";
            this.xbdmReturn.UseVisualStyleBackColor = true;
            this.xbdmReturn.Click += new System.EventHandler(this.xbdmReturn_Click);
            // 
            // newxbdminiBtn
            // 
            this.newxbdminiBtn.Location = new System.Drawing.Point(638, 121);
            this.newxbdminiBtn.Name = "newxbdminiBtn";
            this.newxbdminiBtn.Size = new System.Drawing.Size(86, 45);
            this.newxbdminiBtn.TabIndex = 12;
            this.newxbdminiBtn.Text = "New XBDM.ini";
            this.newxbdminiBtn.UseVisualStyleBackColor = true;
            this.newxbdminiBtn.Click += new System.EventHandler(this.newxbdminiBtn_Click);
            // 
            // xbdmOpen
            // 
            this.xbdmOpen.Location = new System.Drawing.Point(638, 83);
            this.xbdmOpen.Name = "xbdmOpen";
            this.xbdmOpen.Size = new System.Drawing.Size(86, 32);
            this.xbdmOpen.TabIndex = 10;
            this.xbdmOpen.Text = "Open";
            this.xbdmOpen.UseVisualStyleBackColor = true;
            this.xbdmOpen.Click += new System.EventHandler(this.xbdmOpen_Click);
            // 
            // xbdmClear
            // 
            this.xbdmClear.Location = new System.Drawing.Point(638, 45);
            this.xbdmClear.Name = "xbdmClear";
            this.xbdmClear.Size = new System.Drawing.Size(86, 32);
            this.xbdmClear.TabIndex = 8;
            this.xbdmClear.Text = "Clear";
            this.xbdmClear.UseVisualStyleBackColor = true;
            this.xbdmClear.Click += new System.EventHandler(this.xbdmClear_Click);
            // 
            // xbdmSave
            // 
            this.xbdmSave.Location = new System.Drawing.Point(638, 7);
            this.xbdmSave.Name = "xbdmSave";
            this.xbdmSave.Size = new System.Drawing.Size(86, 32);
            this.xbdmSave.TabIndex = 7;
            this.xbdmSave.Text = "Save";
            this.xbdmSave.UseVisualStyleBackColor = true;
            this.xbdmSave.Click += new System.EventHandler(this.xbdmSave_Click);
            // 
            // xbdminieditor
            // 
            this.xbdminieditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.xbdminieditor.AutoIndentCharsPatterns = "\n^\\s*\\$[\\w\\.\\[\\]\\\'\\\"]+\\s*(?<range>=)\\s*(?<range>[^;]+);\n";
            this.xbdminieditor.AutoScrollMinSize = new System.Drawing.Size(2, 12);
            this.xbdminieditor.BackBrush = null;
            this.xbdminieditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xbdminieditor.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.xbdminieditor.CharHeight = 12;
            this.xbdminieditor.CharWidth = 7;
            this.xbdminieditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.xbdminieditor.DefaultMarkerSize = 8;
            this.xbdminieditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.xbdminieditor.Font = new System.Drawing.Font("Courier New", 8F);
            this.xbdminieditor.IsReplaceMode = false;
            this.xbdminieditor.Language = FastColoredTextBoxNS.Language.PHP;
            this.xbdminieditor.LeftBracket = '(';
            this.xbdminieditor.LeftBracket2 = '{';
            this.xbdminieditor.Location = new System.Drawing.Point(4, 4);
            this.xbdminieditor.Name = "xbdminieditor";
            this.xbdminieditor.Paddings = new System.Windows.Forms.Padding(0);
            this.xbdminieditor.RightBracket = ')';
            this.xbdminieditor.RightBracket2 = '}';
            this.xbdminieditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.xbdminieditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("xbdminieditor.ServiceColors")));
            this.xbdminieditor.Size = new System.Drawing.Size(625, 380);
            this.xbdminieditor.TabIndex = 6;
            this.xbdminieditor.Zoom = 100;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VerLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(738, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // VerLabel
            // 
            this.VerLabel.Name = "VerLabel";
            this.VerLabel.Size = new System.Drawing.Size(105, 17);
            this.VerLabel.Text = "BadStick .ini Editor";
            // 
            // inieditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(738, 442);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "inieditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BadStick INI Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.inieditor_FormClosing_1);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.launchinieditor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.jrpcinieditor)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xbdminieditor)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel VerLabel;
        private FastColoredTextBoxNS.FastColoredTextBox launchinieditor;
        private System.Windows.Forms.Button launchiniOpen;
        private System.Windows.Forms.Button launchiniClear;
        private System.Windows.Forms.Button launchiniSave;
        private System.Windows.Forms.Button jrpcOpen;
        private System.Windows.Forms.Button jrpcClear;
        private System.Windows.Forms.Button jrpcSave;
        private FastColoredTextBoxNS.FastColoredTextBox jrpcinieditor;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button xbdmOpen;
        private System.Windows.Forms.Button xbdmClear;
        private System.Windows.Forms.Button xbdmSave;
        private FastColoredTextBoxNS.FastColoredTextBox xbdminieditor;
        private System.Windows.Forms.Button newlaunchiniBtn;
        private System.Windows.Forms.Button newjrpciniBtn;
        private System.Windows.Forms.Button newxbdminiBtn;
        private System.Windows.Forms.Button xbdmReturn;
        private System.Windows.Forms.Button launchReturn;
        private System.Windows.Forms.Button jrpcReturn;
    }
}