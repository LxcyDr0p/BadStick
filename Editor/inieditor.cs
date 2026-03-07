using FastColoredTextBoxNS;
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
using Xbox_360_BadUpdate_USB_Tool;

namespace Xbox_360_BadStick
{
    public partial class inieditor : Form
    {
        private string currentlaunchiniFilePath;
        private bool xbdminiIsDirty = false;
        private bool jrpciniIsDirty = false;
        private bool launchiniIsDirty = false;

        public inieditor()
        {
            InitializeComponent();
            VerLabel.Text = "BadStick " + Form1.currentver + "";
            TopMost = true;
        }

        private void inieditor_Load(object sender, EventArgs e)
        {

        }

        private void inieditor_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void launchinieditor_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            launchiniIsDirty = true;
        }

        private void jrpcinieditor_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            jrpciniIsDirty = true;
        }

        private void xbdminieditor_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            xbdminiIsDirty = true;
        }

        private void launchiniSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, launchinieditor.Text);
                    launchiniIsDirty = false;
                }
            }
        }

        private void launchiniClear_Click(object sender, EventArgs e)
        {
            if (launchiniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to clear?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }
            launchinieditor.Clear();
            launchiniIsDirty = false;
        }

        private void launchiniOpen_Click(object sender, EventArgs e)
        {
            if (launchiniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Open a new file anyway?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    launchinieditor.Text = System.IO.File.ReadAllText(ofd.FileName);
                    launchiniIsDirty = false;
                }
            }
        }

        private void jrpcSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, jrpcinieditor.Text);
                    jrpciniIsDirty = false;
                }
            }
        }

        private void jrpcClear_Click(object sender, EventArgs e)
        {
            if (jrpciniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to clear?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }
            jrpcinieditor.Clear();
            jrpciniIsDirty = false;
        }

        private void jrpcOpen_Click(object sender, EventArgs e)
        {
            if (jrpciniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Open a new file anyway?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    jrpcinieditor.Text = System.IO.File.ReadAllText(ofd.FileName);
                    jrpciniIsDirty = false;
                }
            }
        }

        private void xbdmSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, xbdminieditor.Text);
                    xbdminiIsDirty = false;
                }
            }
        }

        private void xbdmClear_Click(object sender, EventArgs e)
        {
            if (xbdminiIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to clear?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }
            xbdminieditor.Clear();
            xbdminiIsDirty = false;
        }

        private void xbdmOpen_Click(object sender, EventArgs e)
        {
            if (xbdminiIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Open a new file anyway?", "Confirm Open", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "INI Files (*.ini)|*.ini|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    xbdminieditor.Text = System.IO.File.ReadAllText(ofd.FileName);
                    xbdminiIsDirty = false;
                }
            }
        }

        private void newlaunchiniBtn_Click(object sender, EventArgs e)
        {
            if (launchiniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to create a new launch.ini?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            string blankLaunchIni = @"[Paths]
BUT_A = 
BUT_B = 
BUT_X = Usb:\Content\0000000000000000\C0DE9999\00080000\C0DE99990F586558
BUT_Y = Sfc:\dash.xex
Start = 
Back = 
LBump = 
RThumb = 
LThumb = 

Default = 

Guide = 

Power = 

Configapp = 

Fakeanim = 

Dumpfile = 

[Plugins]
plugin1 = 
plugin2 = 
plugin3 = 
plugin4 = 
plugin5 = 

[Externals]
ftpserv = false
ftpport = 21
updserv = false
calaunch = false
fahrenheit = false

[Settings]
nxemini = true
pingpatch = true
contpatch = false
xblapatch = false
licpatch = false
fatalfreeze = false
fatalreboot = false
safereboot = true
regionspoof = false
region = 0x7fff
dvdexitdash = false
xblaexitdash = false
nosysexit = false
nohud = false
noupdater = true
debugout = true
exchandler = true
liveblock = true
livestrong = true
remotenxe = false
hddalive = false
hddtimer = 210
signnotice = false
autoshut = false
autooff = false
xhttp = false
tempbcast = false
temptime = 10
tempport = 7030
sockpatch = true
passlaunch = false
fakelive = false
nonetstore = true
shuttemps = false
devprof = false
devlink = false
autoswap = false
nohealth = true
nooobe = true
autofake = false
autofake0 = 0x00000000
autofake1 = 0x00000000
autofake2 = 0x00000000
autofake3 = 0x00000000
autofake4 = 0x00000000
autofake5 = 0x00000000
autofake6 = 0x00000000
autofake7 = 0x00000000
autofake8 = 0x00000000
autofake9 = 0x00000000
autocont = false
";
            launchinieditor.Text = blankLaunchIni;
            launchiniIsDirty = false;
        }

        private void newxbdminiBtn_Click(object sender, EventArgs e)
        {
            if (xbdminiIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to create a new xbdm.ini?", "BadStick Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            string blankXbdmIni = @"dbgname name=""Bad Update Console""
setcolor name=""nosidecar""";

            xbdminieditor.Text = blankXbdmIni;
            xbdminiIsDirty = false;
        }

        private void newjrpciniBtn_Click(object sender, EventArgs e)
        {
            if (jrpciniIsDirty)
            {
                var res = MessageBox.Show("You have unsaved changes. Are you sure you want to create a new jrpc.ini?", "Confirm New", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                    return;
            }

            string blankJrpcIni = @"""Settings""
{
	//KV Stealer Protection just removes the ability to copy 'KV.bin' from the console via XBDM
	
	""KV Stealer Protection""	""False""
}

""Plugins""
{
	//NOTE: 'Hdd:\' is the only device with JRPC
	//NOTE: Do not use XBDM as a plugin here! use dashlaunch with it!

	//PLEASE DO NOT USE MORE THAN 2-3 PLUGINS! EXAMPLES:

	""plugin1""	""Hdd:\Insert plugin 1 here""
	""plugin2""	""Hdd:\Insert plugin 2 here""
	""plugin3""	""Hdd:\Insert plugin 3 here""

	//PLEASE DO NOT USE MORE THAN 2-3 PLUGINS!
}";
            jrpcinieditor.Text = blankJrpcIni;
            jrpciniIsDirty = false;
        }

        private void xbdmReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 main = new Form2();
            main.Show();
        }

        private void launchReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 main = new Form2();
            main.Show();
        }

        private void jrpcReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 main = new Form2();
            main.Show();
        }

        private void inieditor_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
