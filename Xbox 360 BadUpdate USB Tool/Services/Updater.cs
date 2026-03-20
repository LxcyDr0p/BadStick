using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using Xbox_360_BadStick.DTO;
using Xbox_360_BadStick.Shared.EventArgs;
using Xbox_360_BadStick.Shared.Enums;

namespace Xbox_360_BadStick.Services
{
    public class Updater
    {
        public EventHandler<ProgressReportEventArgs> ProgressChanged;
        public EventHandler<UpdateFoundEventArgs> UpdateFound;
        public EventHandler NoInetDetected;

        public Updater()
        {
            var pastebin = HttpClientFactory.GetClient("pastebin", client =>
            {
                client.BaseAddress = new Uri("https://pastebin.com");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            var ghapi = HttpClientFactory.GetClient("ghapi", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com");
                c.Timeout = TimeSpan.FromSeconds(10);
            });
        }
        
        
        public async Task DoUpdate()
        {
            try
            {
                var exePath = Assembly.GetExecutingAssembly().Location;
                var exeDir = Path.GetDirectoryName(exePath);
                var windowsName = GetWindowsVersionName();
                var isLegacyOS = windowsName.StartsWith("Windows 7") || windowsName.StartsWith("Windows 8");

                string apiUrl = "https://api.github.com/repos/LxcyDr0p/BadStick/releases/latest";

                var apiClient = HttpClientFactory.GetClient("ghapi");
                var responese = await apiClient.GetAsync("/repos/LxcyDr0p/BadStick/releases/latest");

                if (!responese.IsSuccessStatusCode)
                    throw new Exception(responese.ReasonPhrase);

                LatestReleasesDto releases =
                    JsonConvert.DeserializeObject<LatestReleasesDto>(await responese.Content.ReadAsStringAsync());

                var downloadLegacyLink =
                    releases.assets.FirstOrDefault(a => a.name.Contains("Legacy"))?.browser_download_url;
                var downloadCurrentLink = releases.assets.FirstOrDefault(a => !a.name.Contains("Legacy"))
                    ?.browser_download_url;

                if (String.IsNullOrWhiteSpace(downloadCurrentLink) && String.IsNullOrWhiteSpace(downloadLegacyLink))
                    throw new Exception("No suitable asset found for this OS in latest release.");

                if(isLegacyOS)
                    downloadCurrentLink = downloadLegacyLink;

                using (var response =
                       await apiClient.GetAsync(downloadCurrentLink, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    using (var strf = await response.Content.ReadAsStreamAsync())
                    {
                        using (var stwt = File.Open("update.zip", FileMode.Create))
                        {
                            await stwt.CopyToAsync(strf);
                        }
                    }
                    
                }
                        
                
                
                using (var wcJson = new System.Net.WebClient())
                {
                    wcJson.Headers.Add("User-Agent", "BadStick-Updater");
                    string json = wcJson.DownloadString(apiUrl);
                    dynamic release = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    string downloadUrl = null;
                    foreach (var asset in release.assets)
                    {
                        string name = asset.name.Value;
                        if (isLegacyOS && name.Contains("Legacy"))
                        {
                            downloadUrl = asset.browser_download_url.Value;
                            break;
                        }
                        else if (!isLegacyOS && !name.Contains("Legacy"))
                        {
                            downloadUrl = asset.browser_download_url.Value;
                            break;
                        }
                    }

                    if (downloadUrl == null)
                        throw new Exception("No suitable asset found for this OS in latest release.");
                    //startupLabel.Text = windowsName + " detected, installing compatible version...";
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
                    Process.Start(new ProcessStartInfo()
                        { FileName = batPath, UseShellExecute = true, CreateNoWindow = false });
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private string GetWindowsVersionName()
        {
            Version osVer = Environment.OSVersion.Version;
            
            if (osVer.Major == 6 && osVer.Minor == 1) return "Windows 7";
            if (osVer.Major == 6 && osVer.Minor == 2) return "Windows 8";
            if (osVer.Major == 6 && osVer.Minor == 3) return "Windows 8.1";
            if (osVer.Major == 10 && osVer.Build < 22000) return "Windows 10";
            if (osVer.Major == 10 && osVer.Build >= 22000) return "Windows 11";
            return "Unknown Windows Version";
        }
        
        public void DoCleanup()
        {
            if (File.Exists("./updater.bat") == true) { File.Delete("./updater.bat"); }
            if (File.Exists("./updater.txt") == true) { File.Delete("./updater.txt"); }
        }
        
        private async Task<bool> IsInternetAvailableAsync()
        {
            try
            {

                ProgressChanged?.Invoke(this,
                    new ProgressReportEventArgs()
                    {
                        Message = "Status - Checking for internet availability...",
                        Progress = 5
                    });

                var http = HttpClientFactory.GetClient("ping");
                var response = await http.GetAsync("https://www.google.com");
                
                ProgressChanged?.Invoke(this, new ProgressReportEventArgs(100));
                
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                NoInetDetected?.Invoke(this, EventArgs.Empty);
                return false;
            }
        }
        
        public async Task CheckForUpdatesAsync()
        {
            if (!await IsInternetAvailableAsync())
                return;
            
            var http = HttpClientFactory.GetClient("pastebin");
            string latestVersion = await http.GetStringAsync("/raw/SHpqTNY0");
            latestVersion = latestVersion.Trim();

            if (latestVersion == Shared.Settings.CurrentVersion)
            {
                return;
            }
            
            if(Shared.Settings.Legacy)
            {
                UpdateFound?.Invoke(this, 
                    new UpdateFoundEventArgs(){ UpdateType = UpdateType.Legacy});
                return;
            }
            
            UpdateFound?.Invoke(this, 
                new UpdateFoundEventArgs(){ UpdateType = UpdateType.Current});
            
        }
        
        public async Task<string> GetCommunityBoardMessages()
        {
            try
            {
                ProgressChanged?.Invoke(this, new ProgressReportEventArgs()
                {
                    Message = "Status - Checking for community messages...",
                    Progress = 30
                });
                var http = HttpClientFactory.GetClient("pastebin",null);
                string state = await http.GetStringAsync("/raw/sEsrQJve");
                state = state.Trim().ToLowerInvariant();

                if (state != "true")
                {
                    return string.Empty;
                }
                ProgressChanged?.Invoke(this, new ProgressReportEventArgs(50));
                
                string messageText = await http.GetStringAsync("/raw/aJzwnQN4");
                
                ProgressChanged?.Invoke(this, new ProgressReportEventArgs(75));

                return messageText.Trim();

            }
            catch (Exception ex)
            {
                return $"Error checking for community messages:\n{ex.Message}";
            }
        }
    }
}