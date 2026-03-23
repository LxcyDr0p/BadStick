using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using Xbox_360_BadStick.DTO;
using Xbox_360_BadStick.Shared.EventArgs;
using Xbox_360_BadStick.Shared.Enums;
using System.IO.Compression;
using Serilog;

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

            Log.Information("Initialized updates.");
        }


        public async Task DoUpdate()
        {
            try
            {
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var exeDir = Path.GetDirectoryName(exePath);
                var batPath = Path.Combine(Path.GetTempPath(), "update.bat");
                var tempDir = Path.GetDirectoryName(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "dummy.file"));
                var updatePackage = Path.Combine(tempDir, "update.zip");

                var pid = Process.GetCurrentProcess().Id;
                var windowsName = GetWindowsVersionName();
                var isLegacyOS = windowsName.StartsWith("Windows 7") || windowsName.StartsWith("Windows 8");

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

                if (isLegacyOS)
                    downloadCurrentLink = downloadLegacyLink;

                Directory.CreateDirectory(tempDir);

                using (var response = await apiClient.GetAsync(downloadCurrentLink, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    using (var strf = await response.Content.ReadAsStreamAsync())
                    using (var stwt = File.Open(updatePackage, FileMode.Create))
                    {
                        byte[] buffer = new byte[8_192];
                        int bytesRead;

                        while ((bytesRead = strf.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            await stwt.WriteAsync(buffer, 0, bytesRead);
                        }
                    }
                }

                ExtractZipFile(updatePackage, tempDir);
                File.Delete(updatePackage);

                var batContent =
                    $@"@echo off
:wait
tasklist /FI ""PID eq {pid}"" | find /I /N ""{pid}"" >NUL
if ""%ERRORLEVEL%""==""0"" (
    timeout /t 1 /nobreak >nul
    goto wait
)

ren ""{exePath}"" ""{Path.GetFileName(exePath)}.old""
xcopy /y /s ""{tempDir}\*.*"" ""{exeDir}""
start """" ""{exePath}""
del ""%~f0""
";
                File.WriteAllText(batPath, batContent);
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    FileName = batPath
                });
                Log.Information("Shutting down for update...");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Log.Error("Error while updating: {message}", ex.Message);
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Debug))
                    Log.Debug(ex.ToString());
            }
        }


        private string ExtractZipFile(string zipPath, string destination)
        {
            Log.Information("Extracting update file. {zip}", zipPath);
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            try
            {
                ZipFile.ExtractToDirectory(zipPath, destination);
            }
            catch (IOException ex)
            {
                Log.Error("IOException: ", ex);
                return $"There was an issue while extracting update.";
            }
            catch (Exception ex)
            {
                Log.Error("Exception: ", ex);
                return $"There was an issue while extracting update.";
            }
            return "Extracted";
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
            Log.Information("Doing cleanup.");
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
            catch (Exception ex)
            {
                Log.Error("Error while checking internet connection {message}, defaulting to false.", ex.Message);
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
                Log.Information("Latest version already installed, no need to update.");
                return;
            }

            if (Shared.Settings.Legacy)
            {
                Log.Information("Found legacy installation update.");
                UpdateFound?.Invoke(this,
                    new UpdateFoundEventArgs() { UpdateType = UpdateType.Legacy });
                return;
            }

            Log.Information("Found current update.");
            UpdateFound?.Invoke(this,
                new UpdateFoundEventArgs() { UpdateType = UpdateType.Current });

        }

        public async Task<string> GetCommunityBoardMessages()
        {
            try
            {
                Log.Information("Checking community messages...");

                ProgressChanged?.Invoke(this, new ProgressReportEventArgs()
                {
                    Message = "Status - Checking for community messages...",
                    Progress = 30
                });
                var http = HttpClientFactory.GetClient("pastebin", null);
                string state = await http.GetStringAsync("/raw/sEsrQJve");
                state = state.Trim().ToLowerInvariant();

                if (state != "true")
                {
                    return string.Empty;
                }
                ProgressChanged?.Invoke(this, new ProgressReportEventArgs(50));

                string messageText = await http.GetStringAsync("/raw/aJzwnQN4");

                ProgressChanged?.Invoke(this, new ProgressReportEventArgs(75));

                Log.Information("Found message: {message}", messageText.Trim());
                return messageText.Trim();

            }
            catch (Exception ex)
            {
                Log.Error("Exception while checking messages.", ex);
                return $"Error checking for community messages.";
            }
        }
    }
}