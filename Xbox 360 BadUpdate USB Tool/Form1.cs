using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using Xbox_360_BadStick.Shared.EventArgs;
using Xbox_360_BadStick.Services;
using Xbox_360_BadStick.Shared.Enums;
using Xbox_360_BadUpdate_USB_Tool;

namespace Xbox_360_BadStick
{
    public partial class Form1 : Form
    {
        private readonly Updater _updater;

        public Form1()
        {
            InitializeComponent();
            _updater = new Updater();
            _updater.ProgressChanged += On_UpdaterProgressChanged;
            _updater.UpdateFound += On_UpdaterUpdateFound;
            _updater.NoInetDetected += On_UpdaterNoInetDetected;

            VerLabel.Text = $"BadStick {Shared.Settings.CurrentVersion}";
        }

        private void On_UpdaterUpdateFound(object sender, UpdateFoundEventArgs e)
        {
            if (e.UpdateType == UpdateType.Legacy)
            {
                updateTip.SetToolTip(updateNotice, "An update for BadStick Legacy is available.");
                updateNotice.Visible = true;
                startupLabel.Text = "BadStick Lgeacy Update Available!";
                return;
            }

            ContinueBtn.Enabled = true;
            updateNotice.Visible = true;
            ContinueBtn.Text = "Update";
            startupLabel.Text = "BadStick Update Available!";
            warningTip.SetToolTip(updateNotice,
                "There is an update available for BadStick, please update to the latest version.");
        }

        private void On_UpdaterProgressChanged(object sender, ProgressReportEventArgs e)
        {
            startupLabel.Text = !string.IsNullOrWhiteSpace(e.Message) ? e.Message : startupLabel.Text;
            startupProgressBar.Value = e.Progress <= 100 ? e.Progress : startupProgressBar.Maximum;
        }

        private void On_UpdaterNoInetDetected(object sender, EventArgs e)
        {
            fatalError.Visible = true;
            warningTip.SetToolTip(fatalError,
                "No internet access detected, please restart BadStick with a working internet connection.");
        }

        public bool IsRanAsAdmin()
        {
            startupProgressBar.Value = 90;
            startupLabel.Text = "Status - Checking for administrator privileges...";
            using (var identity = WindowsIdentity.GetCurrent()) { var principal = new WindowsPrincipal(identity); return principal.IsInRole(WindowsBuiltInRole.Administrator); }
        }


        private void ExitBtn_Click(object sender, EventArgs e)
        { 
            Application.Exit(); 
        }

        private async void ContinueBtn_Click(object sender, EventArgs e)
        {
            if (ContinueBtn.Text == "Update")
            {
                ContinueBtn.Enabled = false;
                await _updater.DoUpdate();
                return;
            }

            this.Hide();
            Form2 Next = new Form2();
            Next.Show();
        }

        private void creditsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This is for everyone who worked tirelessly to develop and bring BadUpdate to the community. " +
                "Thank you to everyone for your undying dedication and devotion to this community. " +
                "\n\n\nBadStick Developers & Creators:\n" +
                "- Shelby\n- Lxcy_Dr0p\n\n" +
                "BadStick Contributors\n- u/wonderingfloatilla - WMI Bug Work\n\n" +
                "BadUpdate Exploit Credits:\n" +
                "- Grimdoomer (Ryan Miceli)\n- InvoxiPlayGames (Emma)\n- kmx360 (Mate Kukri)\n\n\n" +
                "Of course, thank you to all of the homebrew developers for bringing such " +
                "programs and tools to the community. Your work has done so much over " +
                "the last 20 years for everyone in this community. You are all legends." +
                "\n\n Deserved Honorable Mentions:\n" +
                "- MrMario2011\n" +
                "- ModdedWarfare\n" +
                "- Sharkys Customs / DavisOrNaw\n" +
                "- Modern Vintage Gamer\n" +
                "- Element18592", "Credits Where They Are Due <3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                ContinueBtn.Enabled = false;
                startupProgressBar.Value = 0;
                bool isAdmin = IsRanAsAdmin();
                try
                {
                    if (!Shared.Settings.DevMode)
                    {
                        await _updater.CheckForUpdatesAsync();
                        if (ContinueBtn.Text == "Update")
                        {
                            return;
                        }
                    }

                    var communityMessages = await _updater.GetCommunityBoardMessages();
                    if (!String.IsNullOrWhiteSpace(communityMessages))
                    {
                        MessageBox.Show(communityMessages,
                            "Community Notice",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (!isAdmin)
                    {
                        noadminWarning.Visible = true;
                        warningTip.SetToolTip(noadminWarning,
                            "BadStick not run as administrator, formatting will be disabled.");
                    }

                    if (noadminWarning.Visible && Shared.Settings.Legacy)
                    {
                        noadminWarning.Location = new Point(358, 90);
                    }
                }
                catch (Exception ex)
                {
                    fatalError.Visible = true;
                    startupLabel.Text = "An error occurred during startup, please restart BadStick.";
                }

                startupProgressBar.Value = 100;
                startupLabel.Text = "Welcome to BadStick!";
                ContinueBtn.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _updater.DoCleanup();
        }
        private void githubpicLink_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/LxcyDr0p/BadStick");
        }

        private void discordLink_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/HzUP3shMgQ");
        }
    }
}
