using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xbox_360_BadUpdate_USB_Tool
{

    public partial class Form1 : Form
    {
        public bool IsRunAsAdmin()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static string currentver = "v1.4-Stable";

        public static async Task<bool> IsInternetAvailableAsync()
        {
            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(3);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Checker/1.0");

                    var response = await http.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task ComMSG()
        {
            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Updater/1.0");
                    string state = await http.GetStringAsync("https://pastebin.com/raw/aJzwnQN4");
                    state = state.Trim().ToLowerInvariant();

                    if (state == "true")
                    {
                        string messageText = await http.GetStringAsync("https://pastebin.com/raw/aJzwnQN4");
                        messageText = messageText.Trim();

                        MessageBox.Show(messageText, "Community Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving message from server:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task CheckForUpdatesAsync()
        {
            bool internetAvailable = await IsInternetAvailableAsync();
            if (!internetAvailable)
            {
                MessageBox.Show("No internet connection detected. The application will continue without update check.",
                    "No Internet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Updater/1.0");

                    string latestVersion = await http.GetStringAsync("https://pastebin.com/raw/SHpqTNY0");
                    latestVersion = latestVersion.Trim();

                    if (latestVersion != currentver)
                    {
                        var result = MessageBox.Show(
                            $"An update for BadStick is available!\n\nYour Version: {currentver}\nLatest Version: {latestVersion}\n\nWould you like to update now?",
                            "Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("https://github.com/LxcyDr0p/BadStick");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates:\n{ex.Message}", "Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();
            VerLabel.Text = "BadStick " + Form1.currentver + "";
            _ = ComMSG();
        }

        private void CreditsBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is for everyone who worked tirelessly to develop and bring BadUpdate to the community. " +
                "Thank you to everyone for your undying dedication and devotion to this community. " +
                "\n\n\nBadStick Developers & Creators:\n" +
                "- Shelby (@1xShelby / Klepto) \n- Lxcy_Dr0p\n\n\nBadUpdate Exploit Credits:\n" +
                "- Grimdoomer (Ryan Miceli)\n- InvoxiPlayGames (Emma)\n- kmx360 (Mate Kukri)\n\n\n" +
                "Bill Gates (no jk. fuck you bill microdick)\n\n" +
                "And thank you to all of the homebrew developers for bringing such " +
                "programs and tools to the community. Your work has done so much over" +
                "the last 20 years for everyone in this community. You are all legends." +
                "\n\n Honorable Mentions:\n" +
                "- MrMario2011\n" +
                "- ModdedWarfare\n" +
                "- Sharkys Customs / DavisorNaw\n" +
                "- Modern Vintage Gamer" + 
                "- Element18592", "Credits Where They Are Due <3", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void ContinueBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRunAsAdmin())
                {
                    MessageBox.Show("This program was not ran as an Administrator. You will not be able to format any USB device.", "No Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);                  
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured:\n\n" + ex.Message, "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            bool hasInternet = await IsInternetAvailableAsync();
            if (!hasInternet)
            {
                MessageBox.Show("No internet connection detected. The application will now exit.",
                                "No Internet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else
            {
                try
                {
                    await CheckForUpdatesAsync();
                }
                catch
                {
                    MessageBox.Show("Error: Unable to check for program updates.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Hide();
                Form2 Next = new Form2();
                Next.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
