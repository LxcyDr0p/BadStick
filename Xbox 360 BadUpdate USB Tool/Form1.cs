using FastColoredTextBoxNS;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace Xbox_360_BadUpdate_USB_Tool
{
    public partial class Form1 : Form
    {
        public static string currentver = "v2.1-Stable";
        public static bool devmode = false;
        public static bool legacy = true;
        public static string ChkIntStatus;
        public static string ChkAdminStatus;
        public static string ChkComStatus;
        public Form1() { InitializeComponent(); VerLabel.Text = "BadStick " + Form1.currentver + ""; }
        public bool IsRanAsAdmin()
        {
            startupProgressBar.Value = 90;
            startupLabel.Text = "Status - Checking for administrator privileges...";
            using (var identity = WindowsIdentity.GetCurrent()) {  var principal = new WindowsPrincipal(identity); return principal.IsInRole(WindowsBuiltInRole.Administrator); }
        }
        private async Task ComMSG()
        {
            try
            {
                startupProgressBar.Value = 30;
                startupLabel.Text = "Status - Checking for community messages...";
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Updater/1.0");
                    string state = await http.GetStringAsync("https://pastebin.com/raw/sEsrQJve");
                    startupProgressBar.Value = 50;
                    state = state.Trim().ToLowerInvariant();
                    if (state == "true")
                    {
                        string messageText = await http.GetStringAsync("https://pastebin.com/raw/aJzwnQN4");
                        startupProgressBar.Value = 75;
                        messageText = messageText.Trim();
                        MessageBox.Show(
                            messageText,
                            "Community Notice",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error checking for community messages:\n{ex.Message}", "Message Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private async Task<bool> IsInternetAvailableAsync()
        {
            try
            {
                startupLabel.Text = "Status - Checking for internet availability...";
                startupProgressBar.Value = 5;
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(3);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Checker/1.0");
                    var response = await http.GetAsync("https://www.google.com");
                    startupProgressBar.Value = 25;
                    return response.IsSuccessStatusCode;
                }
            }
            catch { return false; }
        }
        private string GetWindowsVersionName()
        {
            Version osVer = Environment.OSVersion.Version;
            startupLabel.Text = "Status - Checking Windows version...";
            if (osVer.Major == 6 && osVer.Minor == 1) return "Windows 7";
            if (osVer.Major == 6 && osVer.Minor == 2) return "Windows 8";
            if (osVer.Major == 6 && osVer.Minor == 3) return "Windows 8.1";
            if (osVer.Major == 10 && osVer.Build < 22000) return "Windows 10";
            if (osVer.Major == 10 && osVer.Build >= 22000) return "Windows 11";
            return "Unknown Windows Version";
        }
        public async Task CheckForUpdatesAsync()
        {
            bool internetAvailable = await IsInternetAvailableAsync();
            startupLabel.Text = "Status - Checking internet availability...";
            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("BadStick-Updater/1.0");
                    string latestVersion = await http.GetStringAsync("https://pastebin.com/raw/SHpqTNY0");
                    latestVersion = latestVersion.Trim();
                    if (latestVersion != currentver && !legacy == true)
                    {
                        startupProgressBar.Value = 100;
                        ContinueBtn.Enabled = true;
                        updateNotice.Visible = true;
                        ContinueBtn.Text = "Update";
                        startupLabel.Text = "BadStick Update Available!";
                        warningTip.SetToolTip(updateNotice, "There is an update available for BadStick, please update to the latest version.");
                    }
                    else
                    {
                        updateTip.SetToolTip(updateNotice, "An update for BadStick Legacy is available.");
                        updateNotice.Visible = true;
                        startupLabel.Text = "BadStick Lgeacy Update Available!";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error checking for updates:\n{ex.Message}", "Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void ExitBtn_Click(object sender, EventArgs e) { Application.Exit(); }
        private void Update()
        {
            try
            {
                string exePath = Application.ExecutablePath;
                string exeDir = Application.StartupPath;
                string windowsName = GetWindowsVersionName();
                bool isLegacyOS = windowsName.StartsWith("Windows 7") || windowsName.StartsWith("Windows 8");
                string apiUrl = "https://api.github.com/repos/LxcyDr0p/BadStick/releases/latest";
                using (var wcJson = new System.Net.WebClient())
                {
                    wcJson.Headers.Add("User-Agent", "BadStick-Updater");
                    string json = wcJson.DownloadString(apiUrl);
                    dynamic release = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    string tagName = release.tag_name;
                    string downloadUrl = null;
                    foreach (var asset in release.assets)
                    {
                        string name = asset.name.Value;
                        if (isLegacyOS && name.Contains("Legacy")) {  downloadUrl = asset.browser_download_url.Value; break; }
                        else if (!isLegacyOS && !name.Contains("Legacy")) { downloadUrl = asset.browser_download_url.Value; break; }
                    }
                    if (downloadUrl == null)
                        throw new Exception("No suitable asset found for this OS in latest release.");
                    startupLabel.Text = windowsName + " detected, installing compatible version...";
                    string psScript = string.Format(@"
try {{
    Write-Host '[BadStick] Downloading update...'
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile('{0}','update.zip')

    Write-Host '[BadStick] Extracting update...'
    if (Test-Path 'UpdateTemp') {{ Remove-Item 'UpdateTemp' -Recurse -Force }}
    Expand-Archive -Path 'update.zip' -DestinationPath 'UpdateTemp' -Force

    Remove-Item 'update.zip' -Force
    Copy-Item 'UpdateTemp\\*' -Destination '.' -Recurse -Force
    Remove-Item 'UpdateTemp' -Recurse -Force

    Write-Host '[BadStick] Launching updated application...'
    Start-Process -FilePath '{1}'
}} catch {{
    $_ | Out-File 'updater.log' -Encoding UTF8
    Write-Host 'Update failed — check updater.log'
    exit 1
}}
", downloadUrl, exePath);

                    string psPath = Path.Combine(exeDir, "updater.ps1");
                    File.WriteAllText(psPath, psScript);
                    string batContent = string.Format(@"
@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File ""{0}""
del %0
", psPath);
                    string batPath = Path.Combine(exeDir, "updater.bat");
                    File.WriteAllText(batPath, batContent);
                    Process.Start(new ProcessStartInfo() { FileName = batPath, UseShellExecute = true, CreateNoWindow = false });
                    Application.Exit();
                }
            }
            catch (Exception ex) { MessageBox.Show("Updater failed:\n\n" + ex.Message);  }
        }
        private async void ContinueBtn_Click(object sender, EventArgs e)
        {
            if (ContinueBtn.Text == "Update") { ContinueBtn.Enabled = false; Update(); }
            else { this.Hide(); Form2 Next = new Form2(); Next.Show(); }
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
            ContinueBtn.Enabled = false;
            startupProgressBar.Value = 0;
            bool isAdmin = IsRanAsAdmin();
            bool hasInternet = await IsInternetAvailableAsync();
            try
            {
                if (!devmode) { await CheckForUpdatesAsync(); if (ContinueBtn.Text == "Update") { return; } }
                if (!hasInternet) { fatalError.Visible = true; warningTip.SetToolTip(fatalError, "No internet access detected, please restart BadStick with a working internet connection."); return; }
                await ComMSG();
                if (!isAdmin) { noadminWarning.Visible = true; warningTip.SetToolTip(noadminWarning, "BadStick not run as administrator, formatting will be disabled."); }
                if (noadminWarning.Visible == true && legacy == true) { noadminWarning.Location = new Point(358, 90); }
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
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("./updater.bat") == true) { File.Delete("./updater.bat"); }
            if (File.Exists("./updater.txt") == true) { File.Delete("./updater.txt"); }
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
