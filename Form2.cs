using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox_360_BadStick;

namespace Xbox_360_BadUpdate_USB_Tool
{
    public partial class Form2 : Form
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            DefaultRequestHeaders =
            {
                UserAgent = { ParseAdd("Mozilla/5.0 (compatible; BadStickTool/1.0)") }
            } // This way, you don't have to keep re-instancing the HTTPClient, which can lead to some socket issues :0 --- You also never used this before and instead made a new client each download.
        };
        public bool DriveSet = true;
        public string DevicePath = "";
        private int _totalSteps;
        private int _currentStep;
        private Dictionary<string, CheckBox> _checkBoxDict;

        public static string one = "Welcome to the Help and FaQ section. " +
            "Here, you will find all the possible information you could need " +
            "regarding the Bad Update exploit, homebrew, plugins and more.\r\n\r\n" +
            "If you need answers to questions that are not listed here, please join my " +
            "Discord and provide your questions there and I will try to get back to you " +
            "as soon as possible! :)";
        public static string two = "Test";
        public static string tree = "Test2";

        private bool IsRunAsAdmin()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public Form2()
        {
            InitializeComponent();
            InitializeCheckBoxDict();
            LoadUsbDrives();
            VerLabel.Text = "BadStick " + Form1.currentver + "";

            if (!IsRunAsAdmin())
            {
                skipformatToggle.Enabled = false;
                skipformatToggle.Checked = true;
            }
        }

        private class UsbDriveItem
        {
            public string RootPath { get; }
            public string DisplayName { get; }

            public UsbDriveItem(string rootPath, string volumeLabel)
            {
                RootPath = rootPath;
                DisplayName = string.IsNullOrEmpty(volumeLabel)
                    ? rootPath
                    : $"{rootPath} ({volumeLabel})";
            }

            public override string ToString() => DisplayName;
        }

        private void UpdateStatus(string text)
        {
            StatusLabel.Text = text;
        }

        private void SetProgressBar(int percent)
        {
            ProgressBar.Value = percent;
        }

        private void LoadUsbDrives()
        {
            DeviceList.DroppedDown = false;
            DeviceList.BeginUpdate();
            DeviceList.Items.Clear();

            var drives = DriveInfo.GetDrives()
                .Where(d => (d.DriveType == DriveType.Removable || d.DriveType == DriveType.Fixed) && d.IsReady &&
                            string.Equals(d.DriveFormat, "FAT32", StringComparison.OrdinalIgnoreCase))
                .Select(d => new UsbDriveItem(
                    d.RootDirectory.FullName,
                    string.IsNullOrEmpty(d.VolumeLabel) ? "No Label" : d.VolumeLabel))
                .ToList();

            foreach (var drive in drives)
                DeviceList.Items.Add(drive);

            if (DeviceList.Items.Count > 0)
            {
                DeviceList.SelectedIndex = 0;
                var firstDrive = DeviceList.Items[0] as UsbDriveItem;
                if (firstDrive != null)
                {
                    DevicePath = firstDrive.RootPath;
                    DriveSet = true;
                }
                warningLabel.Visible = false;
            }
            else
            {
                DevicePath = null;
                DriveSet = false;
                warningLabel.Text = "Warning: No Fat32 USB Detected";
                warningLabel.Visible = true;
            }

            DeviceList.EndUpdate();

            DeviceList.Enabled = false;
            DeviceList.Enabled = true;
            DeviceList.Focus();
        }


        private async Task CountdownExitStatusAsync()
        {
            if (!ExitToggle.Checked)
                return;

            for (int i = 3; i >= 1; i--)
            {
                UpdateStatus($"Status: Exiting in {i}...");
                await Task.Delay(1000);
            }
            Application.Exit();
        }


        public async Task DownloadFileAsync(string url, string destinationFilePath, IProgress<int> progress = null)
        {
            // Use the _httpClient directly without disposing it. -- Small change but needed when using the static _httpClient.
            using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var total = response.Content.Headers.ContentLength ?? -1L;
                var canReportProgress = total != -1 && progress != null;

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var buffer = new byte[8192];
                    int read;
                    while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, read);
                        totalRead += read;

                        if (canReportProgress)
                        {
                            int percent = (int)((totalRead * 100L) / total);
                            progress.Report(percent);
                        }
                    }
                }
            }
        }
        
        private Task ExtractPackageAsync(string pkgFilePath, string destinationPath, IProgress<int> progress = null)
        {
            // Open ZIP file stream asynchronously instead of the fake-async that it was before.
            // You could include Cancellation Tokens in this but I don't think its needed due to the small file sizes.
            await using var fileStream = File.OpenRead(pkgFilePath);
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read);
        
            int totalEntries = archive.Entries.Count;
            int processedEntries = 0;
        
            foreach (var entry in archive.Entries)
            {
        
                var fullPath = Path.Combine(destinationPath, entry.FullName);
                var directory = Path.GetDirectoryName(fullPath);
        
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
        
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    await using var entryStream = entry.Open();
                    await using var outputStream = File.Create(fullPath);
                    await entryStream.CopyToAsync(outputStream);
                }
        
                processedEntries++;
                if (progress != null) // Kept progress. If you didn't want to use this you could do all this extract stuff with a new method ExtractToDirectoryAsync, just FYI.
                {
                    int percent = (int)((processedEntries * 100L) / totalEntries);
                    progress.Report(percent);
                }
            }
        }


        public class PackageInfo
        {
            public string FileName { get; set; }
            public string CheckBoxName { get; set; }
            public string DownloadUrl { get; set; }
            public bool AlwaysDownload => string.IsNullOrEmpty(CheckBoxName);
            public bool SkipDownload { get; set; } = false;
        }

        private readonly List<PackageInfo> _allPackages = new List<PackageInfo>
        {
            new PackageInfo { FileName = "Aurora.zip", CheckBoxName = "AuroraToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Aurora.zip" },
            new PackageInfo { FileName = "Freestyle.zip", CheckBoxName = "FSDToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Freestyle.zip" },
            new PackageInfo { FileName = "Emerald.zip", CheckBoxName = "EmeraldToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Emerald.zip" },
            new PackageInfo { FileName = "FFPlay.zip", CheckBoxName = "FFPlayToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/FFPlay.zip" },
            new PackageInfo { FileName = "GOD.Unlocker.zip", CheckBoxName = "GODUnlockerToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/GOD.Unlocker.zip" },
            new PackageInfo { FileName = "HDDx.Fixer.zip", CheckBoxName = "HDDxToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/HDDx.Fixer.zip" },
            new PackageInfo { FileName = "IngeniouX.zip", CheckBoxName = "IngeniousXToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/IngeniouX.zip" },
            new PackageInfo { FileName = "NXE2GOD.zip", CheckBoxName = "NXE2GODToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/NXE2GOD.zip" },
            new PackageInfo { FileName = "Payload-XeUnshackle.zip", CheckBoxName = "badavatarToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Payload-XeUnshackle.zip" },
            new PackageInfo { FileName = "BUPayload-XeUnshackle.zip", CheckBoxName = "xeunshackleToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/BUPayload-XeUnshackle.zip" },
            new PackageInfo { FileName = "ABadAvatarHDD.zip", CheckBoxName = "badavatarhddToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/ABadAvatarHDD.zip" },
            new PackageInfo { FileName = "ABadMemUnit0.zip", CheckBoxName = "abadmemunitToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/ABadMemUnit0.zip" },
            new PackageInfo { FileName = "BUPayload-FreeMyXe.zip", CheckBoxName = "freemyxeToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/BUPayload-FreeMyXe.zip" },
            new PackageInfo { FileName = "RBB.zip", CheckBoxName = null, DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/RBB/RBB.zip" },
            new PackageInfo { FileName = "Viper360.zip", CheckBoxName = "Viper360Toggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Viper360.zip" },
            new PackageInfo { FileName = "Xenu.zip", CheckBoxName = "XenuToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Xenu.zip" },
            new PackageInfo { FileName = "XeXLoader.zip", CheckBoxName = "XeXLoaderToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XeXLoader.zip" },
            new PackageInfo { FileName = "XeXMenu.zip", CheckBoxName = null, DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XeXMenu.zip" },
            new PackageInfo { FileName = "XM360.zip", CheckBoxName = "XM360Toggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XM360.zip" },
            new PackageInfo { FileName = "XNA.Offline.zip", CheckBoxName = "XNAToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XNA.Offline.zip" },
            new PackageInfo { FileName = "XPG.Chameleon.zip", CheckBoxName = "XPGToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XPG.Chameleon.zip" },
            new PackageInfo { FileName = "Plugins.zip", CheckBoxName = "PluginsToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Plugins.zip" },
            new PackageInfo { FileName = "CipherLive.zip", CheckBoxName = "CipherToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/CipherLive.zip" },
            new PackageInfo { FileName = "Flasher.zip", CheckBoxName = "flasherToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Flasher.zip" },
            new PackageInfo { FileName = "Hacked.Compatibility.Files.zip", CheckBoxName = "haxfilesToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Hacked.Compatibility.Files.zip" },
            new PackageInfo { FileName = "Nfinite.zip", CheckBoxName = "NfiniteToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Nfinite.zip" },
            new PackageInfo { FileName = "Original.Compatibility.Files.zip", CheckBoxName = "origfilesToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Original.Compatibility.Files.zip" },
            new PackageInfo { FileName = "Proto.zip", CheckBoxName = "ProtoToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Proto.zip" },
            new PackageInfo { FileName = "TetheredLive.zip", CheckBoxName = "tetheredToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/TetheredLive.zip" },
            new PackageInfo { FileName = "X-Notify.Pack.zip", CheckBoxName = "xnotifyToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/X-Notify.Pack.zip" },
            new PackageInfo { FileName = "xbGuard.zip", CheckBoxName = "XbGuardToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XbGuard.zip" },
            new PackageInfo { FileName = "XBL.Kyuubii.zip", CheckBoxName = "KyuubiiToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XBL.Kyuubii.zip" },
            new PackageInfo { FileName = "XBLS.zip", CheckBoxName = "XBLSToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XBLS.zip" },
            new PackageInfo { FileName = "Xbox.One.Files.zip", CheckBoxName = "XB1Toggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/Xbox.One.Files.zip" },
            new PackageInfo { FileName = "XEFU.Spoofer.zip", CheckBoxName = "xefuToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/XEFU.Spoofer.zip" },
            new PackageInfo { FileName = "xbNetwork.zip", CheckBoxName = "xbNetworkToggle", DownloadUrl = "https://github.com/32BitKlepto/BadStick/releases/download/packages/xbNetwork.zip" }
        };
        private void InitializeCheckBoxDict()
        {
            _checkBoxDict = new Dictionary<string, CheckBox>
            {
                { "AuroraToggle", AuroraToggle },
                { "FSDToggle", FSDToggle },
                { "EmeraldToggle", EmeraldToggle },
                { "FFPlayToggle", FFPlayToggle },
                { "GODUnlockerToggle", GODUnlockerToggle },
                { "HDDxToggle", HDDxToggle },
                { "IngeniousXToggle", IngeniouXToggle },
                { "NXE2GODToggle", NXE2GODToggle },
                { "Viper360Toggle", Viper360Toggle },
                { "XenuToggle", XenuToggle },
                { "XeXLoaderToggle", XeXLoaderToggle },
                { "XM360Toggle", XM360Toggle },
                { "XNAToggle", XNAToggle },
                { "XPGToggle", XPGToggle },
                { "PluginsToggle", PluginsToggle },
                { "CipherToggle", CipherToggle },
                { "flasherToggle", flasherToggle },
                { "haxfilesToggle", haxfilesToggle },
                { "NfiniteToggle", NfiniteToggle },
                { "origfilesToggle", origfilesToggle },
                { "ProtoToggle", ProtoToggle },
                { "tetheredToggle", tetheredToggle },
                { "xnotifyToggle", xnotifyToggle },
                { "XbGuardToggle", XbGuardToggle },
                { "KyuubiiToggle", KyuubiiToggle },
                { "XBLSToggle", XBLSToggle },
                { "XB1Toggle", XB1Toggle },
                { "xefuToggle", xefuToggle },
                { "XeXDashToggle", XeXDashToggle },
                { "xbNetworkToggle", xbNetworkToggle },
                { "badavatarToggle", badavatarToggle },
                { "badavatarhddToggle", badavatarhddToggle },
                { "abadmemunitToggle", abadmemunitToggle },
                { "xeunshackleToggle", xeunshackleToggle }
            };
        }

        private Dictionary<string, CheckBox> _excludedFromSelectAll;

        private void InitializeExcludedCheckBoxes()
        {
            _excludedFromSelectAll = new Dictionary<string, CheckBox>
            {
                { "xeunshackleToggle", xeunshackleToggle },
                { "skipformatToggle", skipformatToggle },
                { "skipxexmenuToggle", skipxexmenuToggle },
                { "skipmainfilesToggle", skipmainfilesToggle }
            };
        }

        private List<PackageInfo> GetSelectedPackages()
        {
            return _allPackages.Where(pkg =>
                pkg.AlwaysDownload ||
                (_checkBoxDict.TryGetValue(pkg.CheckBoxName, out var checkbox) && checkbox.Checked)
            ).ToList();
        }

        public async Task DownloadAndExtractPackagesAsync(
            List<PackageInfo> packages,
            Dictionary<string, CheckBox> checkBoxes,
            string usbRootPath,
            IProgress<int> progress = null)
        {
            if (string.IsNullOrWhiteSpace(usbRootPath) || !Directory.Exists(usbRootPath))
            {
                UpdateStatus("Status: Please Select A Valid USB Device");
                return;
            }

            string appTempFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            if (!Directory.Exists(appTempFolder))
            {
                Directory.CreateDirectory(appTempFolder);
            }

            bool skipMainFilesChecked = checkBoxes.TryGetValue("skipmainfilesToggle", out var skipMainFilesCb) && skipMainFilesCb.Checked;
            bool skipRbbChecked = checkBoxes.TryGetValue("skiprbbToggle", out var skipRbbCb) && skipRbbCb.Checked;
            bool skipXexChecked = checkBoxes.TryGetValue("skipxexToggle", out var skipXexCb) && skipXexCb.Checked;

            int totalPackages = packages.Count;
            int currentPackageIndex = 0;

            foreach (var pkg in packages)
            {
                if (skipMainFilesChecked)
                {
                    string[] mainFilesToSkip = {
                        "Payload-XeUnshackle.zip",
                        "Payload-FreeMyXe.zip",
                        "XeXMenu.zip",
                        "RBB.zip"
                    };
                    if (mainFilesToSkip.Contains(pkg.FileName, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }
                else
                {
                    if (pkg.FileName.Equals("RBB.zip", StringComparison.OrdinalIgnoreCase) && skipRbbChecked)
                    {
                        continue;
                    }
                    if (pkg.FileName.Equals("XeXMenu.zip", StringComparison.OrdinalIgnoreCase) && skipXexChecked)
                    {
                        continue;
                    }
                    if (pkg.FileName.Equals("Payload-XeUnshackle.zip", StringComparison.OrdinalIgnoreCase) &&
                        checkBoxes.TryGetValue("freemyxeToggle", out var freeMyXeCb) && freeMyXeCb.Checked)
                    {
                        continue;
                    }
                    if (pkg.FileName.Equals("Payload-FreeMyXe.zip", StringComparison.OrdinalIgnoreCase) &&
                        checkBoxes.TryGetValue("xeunshackleToggle", out var xeUnshackleCb) && xeUnshackleCb.Checked)
                    {
                        continue;
                    }
                }

                if (!pkg.AlwaysDownload)
                {
                    if (!checkBoxes.TryGetValue(pkg.CheckBoxName, out var cb) || !cb.Checked)
                    {
                        continue;
                    }
                }

                currentPackageIndex++;
                var tempFilePath = Path.Combine(appTempFolder, pkg.FileName);

                bool needsDownload = true;
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        using (var archive = ZipFile.OpenRead(tempFilePath))
                        {
                            needsDownload = false;
                        }
                    }
                    catch
                    {
                        needsDownload = true;
                    }
                }

                if (needsDownload)
                {
                    UpdateStatus($"Status: Downloading {pkg.FileName} ({currentPackageIndex}/{totalPackages})");
                    var downloadProgress = new Progress<int>(percent =>
                    {
                        int overallPercent = (int)(((currentPackageIndex - 1 + (percent / 100.0)) / totalPackages) * 100 * 0.5);
                        SetProgressBar(overallPercent);
                    });
                    await DownloadFileAsync(pkg.DownloadUrl, tempFilePath, downloadProgress);
                }
                else
                {
                    UpdateStatus($"Status: {pkg.FileName} already exists, skipping download");
                }

                if (File.Exists(tempFilePath))
                {
                    UpdateStatus($"Status: Extracting {pkg.FileName} ({currentPackageIndex}/{totalPackages})");
                    var extractProgress = new Progress<int>(percent =>
                    {
                        int overallPercent = (int)(((currentPackageIndex - 1 + (percent / 100.0)) / totalPackages) * 100 * 0.5 + 50);
                        SetProgressBar(overallPercent);
                    });
                    await ExtractPackageAsync(tempFilePath, usbRootPath, extractProgress);
                }
                else
                {
                    UpdateStatus($"Status: Skipping extraction of {pkg.FileName} because file does not exist");
                }
            }

            UpdateStatus("Status: All Downloads Completed");
            SetProgressBar(100);
        }



        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DeviceList.SelectedItem is UsbDriveItem selectedDrive)
            {
                DevicePath = selectedDrive.RootPath;
                DriveSet = true;
                Debug.WriteLine($"Selected drive: {DevicePath}");
            }
            else
            {
                DevicePath = null;
                DriveSet = false;
            }
        }


        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool FormatDriveToFat32(string drivePath)
        {
            try
            {
                string driveLetter = Path.GetPathRoot(drivePath).TrimEnd('\\');

                string query = $"SELECT * FROM Win32_Volume WHERE DriveLetter = '{driveLetter}'";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    var volumes = searcher.Get();

                    foreach (ManagementObject volume in volumes)
                    {
                        var inParams = volume.GetMethodParameters("Format");
                        inParams["FileSystem"] = "FAT32";
                        inParams["QuickFormat"] = true;

                        ManagementBaseObject outParams = volume.InvokeMethod("Format", inParams, null);

                        uint returnValue = (uint)(outParams.Properties["ReturnValue"].Value);

                        if (returnValue == 0)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to format drive. WMI Format returned error code: {returnValue}", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Debug.WriteLine($"Format failed with error code: {returnValue}");
                            return false;
                        }
                    }
                }

                MessageBox.Show("Drive not found or inaccessible for formatting.", "BadStick Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error formatting drive: {ex.Message}", "BadStick Format Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            if (badavatarToggle.Checked == true && badupdateToggle.Checked == true && abadmemunitToggle.Checked == true && badavatarhddToggle.Checked == true)
            {
                MessageBox.Show("Please select only one exploit method.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DeviceList.SelectedItem == null)
            {
                MessageBox.Show("Please select a USB device.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string usbPath;

            if (DeviceList.SelectedItem is UsbDriveItem selectedDrive)
            {
                usbPath = selectedDrive.RootPath;
            }
            else
            {
                MessageBox.Show("Please select a valid USB device.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(usbPath) || !Directory.Exists(usbPath))
            {
                MessageBox.Show("Please select a valid USB device.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!skipformatToggle.Checked)
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to select {usbPath} as your USB drive to format and configure? This will erase all data on the device. Please" +
                    $" ensure that this is the device that you want to use before you go ahead. I am not responsible for any accidental " +
                    $"data loss on your behalf.",
                    "Confirm Format",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes)
                {
                    UpdateStatus("Status: Format cancelled");
                    return;
                }

                UpdateStatus("Status: Formatting device...");
                ProgressBar.Value = 0;
                bool formatSuccess = await Task.Run(() => FormatDriveToFat32(usbPath));
                if (!formatSuccess)
                {
                    MessageBox.Show("Failed to format the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Status: Format failed");
                    return;
                }
                UpdateStatus("Status: Format completed. Starting downloads...");
            }
            else
            {
                UpdateStatus("Status: Skipping format (per user request)...");
            }

            var packagesToDownload = GetSelectedPackages();

            if (skipmainfilesToggle.Checked)
            {
                string[] mainFiles = { "RBB.zip", "Payload-XeUnshackle.zip", "Payload-FreeMyXe.zip", "XeXMenu.zip" };
                packagesToDownload = packagesToDownload
                    .Where(pkg => !mainFiles.Contains(pkg.FileName, StringComparer.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                if (skiprbbToggle.Checked)
                {
                    packagesToDownload = packagesToDownload
                        .Where(pkg => !string.Equals(pkg.FileName, "RBB.zip", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (_checkBoxDict.TryGetValue("freemyxeToggle", out var freeMyXeCb) && freeMyXeCb.Checked)
                {
                    packagesToDownload = packagesToDownload
                        .Where(pkg => !string.Equals(pkg.FileName, "Payload-XeUnshackle.zip", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (_checkBoxDict.TryGetValue("xeunshackleToggle", out var xeUnshackleCb) && xeUnshackleCb.Checked)
                {
                    packagesToDownload = packagesToDownload
                        .Where(pkg => !string.Equals(pkg.FileName, "Payload-FreeMyXe.zip", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (skipxexmenuToggle.Checked)
                {
                    packagesToDownload = packagesToDownload
                        .Where(pkg => !string.Equals(pkg.FileName, "XeXMenu.zip", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            _totalSteps = packagesToDownload.Count;
            foreach (var pkg in packagesToDownload)
            {
                string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp", pkg.FileName);
                if (File.Exists(tempFilePath))
                {
                    using (var archive = ZipFile.OpenRead(tempFilePath))
                    {
                        _totalSteps += archive.Entries.Count;
                    }
                }
                else
                {
                    _totalSteps += 10;
                }
            }

            _currentStep = 0;

            var progress = new Progress<int>(percent =>
            {
                ProgressBar.Value = percent;
            });

            await DownloadAndExtractPackagesAsync(packagesToDownload, _checkBoxDict, usbPath, progress);

            UpdateStatus("Status: Done! USB Ready.");
            ProgressBar.Value = 100;
            MessageBox.Show(this, "Done. Your USB is ready to go, thank you for using BadStick. Now go hax that xbox!11!!111!!1!11!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Thread.Sleep(500);
            await CountdownExitStatusAsync();
        }





        private void SelectAllToggle_CheckedChanged(object sender, EventArgs e)
        {
            bool checkAll = SelectAllToggle.Checked;

            foreach (var kvp in _checkBoxDict)
            {
                kvp.Value.Checked = checkAll;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void RefDrivesBtn_Click(object sender, EventArgs e)
        {
            LoadUsbDrives();
        }

        private void widBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Dashlaunch is not listed here, because it cannot be ran on BadUpdate " +
                "consoles. If you were to install Dashlaunch on a BadUpdate exploited console, it would" +
                " temporarily brick your nand, and you would then have to perform a RGH to revive it.", "Where is Dashlaunch?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void skipmainQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Enabling this will skip the main files that BadStick installs by default (Rock Band" +
                " Blitz, the payload, and XeXMenu V1.2). This is useful if you already have your USB setup for the " +
                "Bad Update exploit and only want to install other packages.", "What Is This?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void skipformatQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Enabling this will skip the format process entirely, in case" +
                " you don't want to format the drive at all (recommended regardless).", "What Is This?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void badstickredditBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.reddit.com/r/360hacks/comments/1mmaaz2/release_badstick_a_badupdate_usb_auto_installer/");
        }

        private void reddit360Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.reddit.com/r/360hacks/");
        }

        private void githubpageBtn_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://github.com/32BitKlepto/BadStick");
        }

        private void skipmainfilesToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!skipmainfilesToggle.Checked)
            {
                freemyxeToggle.Enabled = true;
                xeunshackleToggle.Enabled = true;
                skiprbbToggle.Enabled = true;
                badupdateToggle.Enabled = true;
                badavatarToggle.Enabled = true;
                badavatarhddToggle.Enabled = true;
                return;
            }
            else
            {
                freemyxeToggle.Checked = false;
                freemyxeToggle.Enabled = false;
                xeunshackleToggle.Checked = false;
                xeunshackleToggle.Enabled = false;
                skiprbbToggle.Checked = false;
                skiprbbToggle.Enabled = false;
                badupdateToggle.Enabled = false;
                badavatarToggle.Enabled = false;
                badupdateToggle.Checked = false;
                badavatarToggle.Checked = false;
                badavatarhddToggle.Enabled = false;
                badavatarhddToggle.Checked = false;
                return;
            }
        }

        private void xeunshackleToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!xeunshackleToggle.Checked)
            {
                freemyxeToggle.Enabled = true;             
            }
            else
            {
                freemyxeToggle.Checked = false;
                freemyxeToggle.Enabled= false;
            }

            if (!xeunshackleToggle.Checked && !freemyxeToggle.Checked)
            {
                StartBtn.Enabled = false;
            }
            else
            {
                StartBtn.Enabled = true;
            }
        }

        private void freemyxeToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!freemyxeToggle.Checked)
            {
                xeunshackleToggle.Enabled = true;
            }
            else
            {
                xeunshackleToggle.Checked = false;
                xeunshackleToggle.Enabled = false;
            }

            if (!xeunshackleToggle.Checked && !freemyxeToggle.Checked)
            {
                StartBtn.Enabled = false;
            }
            else
            {
                StartBtn.Enabled = true;
            }
        }

        private void editorBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            inieditor editor = new inieditor();
            editor.Show();
        }

        private void installallQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This will install all of the packages included within BadStick " +
                "and install XeUnshackle by default (which is recommended to begin with).", "What Is This?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void noticeLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("It is highly recommended that you install XeUnshackle version of the " +
                "BadUpdate payload, as this is far more reliable and stable than FreeMyXe. You will " +
                "also not have to manually patch executable files (.xex/.xbes) manually. (Thank you to @jimdude7404 " +
                "on Discord for pointing that out to me! :)", "Recommended", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void badupdateToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!badupdateToggle.Checked)
            {

            }
            else
            {
                skiprbbToggle.Enabled = true;
                badavatarToggle.Enabled = true;
            }
        }

        private void badavatarToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!badavatarToggle.Checked)
            {
                skiprbbToggle.Enabled = true;
                skiprbbToggle.Checked = false;
                xeunshackleToggle.Enabled = true;
                freemyxeToggle.Enabled = true;
            }
            else
            {
                badupdateToggle.Checked = false;
                skiprbbToggle.Checked = true;
                skiprbbToggle.Enabled = false;
                freemyxeToggle.Enabled = false;
                xeunshackleToggle.Enabled = false;
                freemyxeToggle.Checked = false;
                xeunshackleToggle.Checked = false;
            }
        }

        private void badavatarhddToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!badavatarhddToggle.Checked)
            {
                skiprbbToggle.Enabled = true;
                skiprbbToggle.Checked = false;
                xeunshackleToggle.Enabled = true;
                freemyxeToggle.Enabled = true;
            }
            else
            {
                badupdateToggle.Checked = false;
                skiprbbToggle.Checked = true;
                skiprbbToggle.Enabled = false;
                freemyxeToggle.Enabled = false;
                xeunshackleToggle.Enabled = false;
                freemyxeToggle.Checked = false;
                xeunshackleToggle.Checked = false;
            }
        }

        private void abadmemunitToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!abadmemunitToggle.Checked)
            {

                skiprbbToggle.Enabled = true;
                skiprbbToggle.Checked = false;
                xeunshackleToggle.Enabled = true;
                freemyxeToggle.Enabled = true;
            }
            else
            {
                skiprbbToggle.Checked = true;
                skiprbbToggle.Enabled = false;
                freemyxeToggle.Enabled = false;
                xeunshackleToggle.Enabled = false;
                freemyxeToggle.Checked = false;
                xeunshackleToggle.Checked = false;
            }
        }
    }
}

