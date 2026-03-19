using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox_360_BadStick;
using Xbox_360_BadStick.Properties;

namespace Xbox_360_BadUpdate_USB_Tool
{
    public partial class Form2 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private UsbDriveItem _manualUsbDrive;
        private static readonly string[] CrossOverMountRoots =
        {
            @"Z:\Volumes",
            @"Z:\media",
            @"Z:\mnt"
        };
        private static readonly string[] ExcludedMountedVolumeNames =
        {
            "Macintosh HD",
            "Macintosh HD - Data",
            "Preboot",
            "Recovery",
            "Update",
            "VM"
        };
        public bool DriveSet = true;
        public bool IsAdmin = false;
        public string DevicePath = "";
        private int _totalSteps;
        private int _currentStep;
        private Dictionary<string, CheckBox> _checkBoxDict;
        private readonly List<PackageInfo> _allPackages = new List<PackageInfo>
        {
            new PackageInfo { FileName = "Aurora.zip", CheckBoxName = "AuroraToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Aurora.zip" },
            new PackageInfo { FileName = "Freestyle.zip", CheckBoxName = "FSDToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Freestyle.zip" },
            new PackageInfo { FileName = "Emerald.zip", CheckBoxName = "EmeraldToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Emerald.zip" },
            new PackageInfo { FileName = "FFPlay.zip", CheckBoxName = "FFPlayToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/FFPlay.zip" },
            new PackageInfo { FileName = "GOD.Unlocker.zip", CheckBoxName = "GODUnlockerToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/GOD.Unlocker.zip" },
            new PackageInfo { FileName = "HDDx.Fixer.zip", CheckBoxName = "HDDxToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/HDDx.Fixer.zip" },
            new PackageInfo { FileName = "IngeniouX.zip", CheckBoxName = "IngeniousXToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/IngeniouX.zip" },
            new PackageInfo { FileName = "NXE2GOD.zip", CheckBoxName = "NXE2GODToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/NXE2GOD.zip" },
            new PackageInfo { FileName = "Payload-XeUnshackle.zip", CheckBoxName = "badavatarToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Payload-XeUnshackle.zip" },
            new PackageInfo { FileName = "BUPayload-XeUnshackle.zip", CheckBoxName = "xeunshackleToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/BUPayload-XeUnshackle.zip" },
            new PackageInfo { FileName = "ABadAvatarHDD.zip", CheckBoxName = "badavatarhddToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/ABadAvatarHDD.zip" },
            new PackageInfo { FileName = "ABadMemUnit0.zip", CheckBoxName = "abadmemunitToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/ABadMemUnit0.zip" },
            new PackageInfo { FileName = "BUPayload-FreeMyXe.zip", CheckBoxName = "freemyxeToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/BUPayload-FreeMyXe.zip" },
            new PackageInfo { FileName = "RBB.zip", CheckBoxName = null, DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/RBB/RBB.zip" },
            new PackageInfo { FileName = "Viper360.zip", CheckBoxName = "Viper360Toggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Viper360.zip" },
            new PackageInfo { FileName = "Xenu.zip", CheckBoxName = "XenuToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Xenu.zip" },
            new PackageInfo { FileName = "XeXLoader.zip", CheckBoxName = "XeXLoaderToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XeXLoader.zip" },
            new PackageInfo { FileName = "XeXMenu.zip", CheckBoxName = null, DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XeXMenu.zip" },
            new PackageInfo { FileName = "XM360.zip", CheckBoxName = "XM360Toggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XM360.zip" },
            new PackageInfo { FileName = "XNA.Offline.zip", CheckBoxName = "XNAToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XNA.Offline.zip" },
            new PackageInfo { FileName = "XPG.Chameleon.zip", CheckBoxName = "XPGToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XPG.Chameleon.zip" },
            new PackageInfo { FileName = "Plugins.zip", CheckBoxName = "PluginsToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Plugins.zip" },
            new PackageInfo { FileName = "CipherLive.zip", CheckBoxName = "CipherToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/CipherLive.zip" },
            new PackageInfo { FileName = "Flasher.zip", CheckBoxName = "flasherToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Flasher.zip" },
            new PackageInfo { FileName = "Hacked.Compatibility.Files.zip", CheckBoxName = "haxfilesToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Hacked.Compatibility.Files.zip" },
            new PackageInfo { FileName = "Nfinite.zip", CheckBoxName = "NfiniteToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Nfinite.zip" },
            new PackageInfo { FileName = "Original.Compatibility.Files.zip", CheckBoxName = "origfilesToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Original.Compatibility.Files.zip" },
            new PackageInfo { FileName = "Proto.zip", CheckBoxName = "ProtoToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Proto.zip" },
            new PackageInfo { FileName = "TetheredLive.zip", CheckBoxName = "tetheredToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/TetheredLive.zip" },
            new PackageInfo { FileName = "X-Notify.Pack.zip", CheckBoxName = "xnotifyToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/X-Notify.Pack.zip" },
            new PackageInfo { FileName = "xbGuard.zip", CheckBoxName = "XbGuardToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XbGuard.zip" },
            new PackageInfo { FileName = "XBL.Kyuubii.zip", CheckBoxName = "KyuubiiToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XBL.Kyuubii.zip" },
            new PackageInfo { FileName = "XBLS.zip", CheckBoxName = "XBLSToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XBLS.zip" },
            new PackageInfo { FileName = "Xbox.One.Files.zip", CheckBoxName = "XB1Toggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Xbox.One.Files.zip" },
            new PackageInfo { FileName = "XEFU.Spoofer.zip", CheckBoxName = "xefuToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/XEFU.Spoofer.zip" },
            new PackageInfo { FileName = "Boot.Animations.zip", CheckBoxName = "bootanimpackToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/Boot.Animations.zip" },
            new PackageInfo { FileName = "HvP2.zip", CheckBoxName = "hvp2Toggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/HvP2.zip" },
            new PackageInfo { FileName = "hiddriver360.zip", CheckBoxName = "hiddriverToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/hiddriver360.zip" },
            new PackageInfo { FileName = "xbPirate.zip", CheckBoxName = "xbpirateToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/xbpirate.zip" },
            new PackageInfo { FileName = "fakeAnim.zip", CheckBoxName = "fakeanimToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/FakeAnim.zip" },
            new PackageInfo { FileName = "xbNetwork.zip", CheckBoxName = "xbNetworkToggle", DownloadUrl = "https://github.com/LxcyDr0p/BadStick/releases/download/packages/xbNetwork.zip" }
        };
        private Dictionary<string, CheckBox> _excludedFromSelectAll;
        private void UpdateStatus(string text) { StatusLabel.Text = text; }
        private void SetProgressBar(int percent) { ProgressBar.Value = percent; }
        private bool IsRunAsAdmin() { using (var identity = WindowsIdentity.GetCurrent()) { var principal = new WindowsPrincipal(identity); return principal.IsInRole(WindowsBuiltInRole.Administrator); }}
        private void ExitBtn_Click(object sender, EventArgs e) { Application.Exit(); }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e) { Application.Exit(); }
        public Form2()
        {
            InitializeComponent();
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; BadStickTool/1.0)");
            InitializeCheckBoxDict();
            LoadUsbDrives();
            if (!IsRunAsAdmin() && File.Exists("./updater.bat"))
            {
                File.Delete("./updater.bat");
            }
        }
        private class UsbDriveItem
        {
            public string RootPath { get; }
            public string DisplayName { get; }
            public bool CanFormat { get; }
            public bool IsManualSelection { get; }
            public UsbDriveItem(string rootPath, string volumeLabel, bool canFormat = true, bool isManualSelection = false)
            {
                RootPath = rootPath;
                CanFormat = canFormat;
                IsManualSelection = isManualSelection;
                string baseDisplayName = string.IsNullOrEmpty(volumeLabel) ? rootPath : $"{rootPath} ({volumeLabel})";
                DisplayName = isManualSelection ? $"Manual: {baseDisplayName}" : baseDisplayName;
            }
            public override string ToString() { return DisplayName; }
        }
        private bool PathsEqual(string left, string right)
        {
            string normalizedLeft = NormalizeRootPath(left);
            string normalizedRight = NormalizeRootPath(right);
            return string.Equals(normalizedLeft, normalizedRight, StringComparison.OrdinalIgnoreCase);
        }
        private string NormalizeRootPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        private IEnumerable<UsbDriveItem> GetWindowsUsbDrives()
        {
            return DriveInfo.GetDrives()
                .Where(d => (d.DriveType == DriveType.Removable || d.DriveType == DriveType.Fixed) && d.IsReady &&
                            string.Equals(d.DriveFormat, "FAT32", StringComparison.OrdinalIgnoreCase))
                .Select(d => new UsbDriveItem(
                    d.RootDirectory.FullName,
                    string.IsNullOrEmpty(d.VolumeLabel) ? "No Label" : d.VolumeLabel));
        }
        private IEnumerable<UsbDriveItem> GetCrossOverMountedDrives()
        {
            foreach (string mountRoot in CrossOverMountRoots)
            {
                if (!Directory.Exists(mountRoot))
                {
                    continue;
                }
                string[] mountedDirectories;
                try
                {
                    mountedDirectories = Directory.GetDirectories(mountRoot);
                }
                catch
                {
                    continue;
                }
                foreach (string mountedDirectory in mountedDirectories)
                {
                    string volumeName = Path.GetFileName(mountedDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    if (string.IsNullOrWhiteSpace(volumeName) || volumeName.StartsWith(".", StringComparison.Ordinal) ||
                        ExcludedMountedVolumeNames.Contains(volumeName, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    yield return new UsbDriveItem(mountedDirectory, $"Mounted Volume: {volumeName}", canFormat: false);
                }
            }
        }
        private List<UsbDriveItem> GetAvailableUsbTargets()
        {
            var targets = new List<UsbDriveItem>();
            targets.AddRange(GetWindowsUsbDrives());
            if (targets.Count == 0)
            {
                foreach (var mountedDrive in GetCrossOverMountedDrives())
                {
                    if (!targets.Any(existing => PathsEqual(existing.RootPath, mountedDrive.RootPath)))
                    {
                        targets.Add(mountedDrive);
                    }
                }
            }
            return targets;
        }
        private void UpdateFormatSelectionState(UsbDriveItem selectedDrive)
        {
            if (!IsAdmin)
            {
                skipformatToggle.Checked = true;
                skipformatToggle.Enabled = false;
                return;
            }
            if (selectedDrive == null)
            {
                skipformatToggle.Enabled = true;
                return;
            }
            if (!selectedDrive.CanFormat)
            {
                skipformatToggle.Checked = true;
                skipformatToggle.Enabled = false;
                string selectionSource = selectedDrive.IsManualSelection ? "Manual folder" : "Mounted volume";
                UpdateStatus($"Status: {selectionSource} selected. Format is unavailable; files will be copied only.");
                return;
            }
            skipformatToggle.Enabled = true;
        }
        private void LoadUsbDrives()
        {
            string currentSelectionPath = (DeviceList.SelectedItem as UsbDriveItem)?.RootPath;
            bool hasManualSelection = _manualUsbDrive != null && Directory.Exists(_manualUsbDrive.RootPath);
            if (!hasManualSelection)
            {
                _manualUsbDrive = null;
            }
            DeviceList.DroppedDown = false;
            DeviceList.BeginUpdate();
            DeviceList.Items.Clear();

            var drives = GetAvailableUsbTargets();

            foreach (var drive in drives)
                DeviceList.Items.Add(drive);

            if (hasManualSelection && !drives.Any(drive => PathsEqual(drive.RootPath, _manualUsbDrive.RootPath)))
            {
                DeviceList.Items.Add(_manualUsbDrive);
                drives.Add(_manualUsbDrive);
            }

            if (DeviceList.Items.Count > 0)
            {
                UsbDriveItem driveToSelect = drives.FirstOrDefault(drive => PathsEqual(drive.RootPath, currentSelectionPath))
                    ?? drives.FirstOrDefault();
                if (driveToSelect != null)
                {
                    DeviceList.SelectedItem = driveToSelect;
                    DevicePath = driveToSelect.RootPath;
                    DriveSet = true;
                    UpdateFormatSelectionState(driveToSelect);
                }
                warningLabel.Visible = false;
            }
            else
            {
                DevicePath = null;
                DriveSet = false;
                UpdateFormatSelectionState(null);
                warningLabel.Text = "Warning: No FAT32 USB or mounted media detected. Use Browse Folder to select a target.";
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
                UpdateStatus($"Status: Exiting In {i}...");
                await Task.Delay(1000);
            }
            Application.Exit();
        }
        public async Task DownloadFileAsync(string url, string destinationFilePath, IProgress<int> progress = null)
        {
            const int maxAttempts = 3;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            if (attempt < maxAttempts && IsTransientStatusCode(response.StatusCode))
                            {
                                await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
                                continue;
                            }
                            throw new HttpRequestException($"Server returned {(int)response.StatusCode} ({response.ReasonPhrase}) for {url}");
                        }
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
                        return;
                    }
                }
                catch (HttpRequestException ex)
                {
                    CleanupPartialDownload(destinationFilePath);
                    if (attempt >= maxAttempts)
                    {
                        throw new HttpRequestException($"Failed to download after {maxAttempts} attempts. {ex.Message}", ex);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
                }
                catch
                {
                    CleanupPartialDownload(destinationFilePath);
                    throw;
                }
            }
        }
        private bool IsTransientStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.BadGateway ||
                   statusCode == HttpStatusCode.ServiceUnavailable ||
                   statusCode == HttpStatusCode.GatewayTimeout ||
                   (int)statusCode == 429;
        }
        private void CleanupPartialDownload(string destinationFilePath)
        {
            if (!File.Exists(destinationFilePath))
            {
                return;
            }
            try
            {
                File.Delete(destinationFilePath);
            }
            catch
            {
            }
        }
        private async Task ExtractPackageAsync(string pkgFilePath, string destinationPath, IProgress<int> progress = null)
        {
            using (var fileStream = File.OpenRead(pkgFilePath))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                int totalEntries = archive.Entries.Count;
                int processedEntries = 0;
                string destinationFullPath = Path.GetFullPath(destinationPath);
                foreach (var entry in archive.Entries)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(destinationFullPath, entry.FullName));
                    if (!fullPath.StartsWith(destinationFullPath, StringComparison.OrdinalIgnoreCase)) { throw new IOException("Entry Is Outside Target Directory");}
                    string directory = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrEmpty(directory)) { Directory.CreateDirectory(directory); }
                    if (!string.IsNullOrEmpty(entry.Name)) { using (var entryStream = entry.Open()) { using (var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 131072, true)) { await entryStream.CopyToAsync(outputStream); } } }
                    processedEntries++;
                    int percent = (int)((processedEntries * 100L) / totalEntries);
                    progress?.Report(percent);
                }
            }
        }
        public class PackageInfo { public string FileName { get; set; } public string CheckBoxName { get; set; } public string DownloadUrl { get; set; } public bool AlwaysDownload => string.IsNullOrEmpty(CheckBoxName); public bool SkipDownload { get; set; } = false; }
        private void InitializeCheckBoxDict()
        {
            _checkBoxDict = new Dictionary<string, CheckBox>
            {
                { "AuroraToggle", AuroraToggle },
                { "FSDToggle", FSDToggle },
                { "bootanimpackToggle", bootanimpackToggle },
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
                { "fakeanimToggle", fakeanimToggle },
                { "hvp2Toggle", hvp2Toggle },
                { "hiddriverToggle", hiddriverToggle },
                { "xbpirateToggle", xbpirateToggle },
                { "xeunshackleToggle", xeunshackleToggle }
            };
        }
        private List<PackageInfo> GetSelectedPackages()
        {
            return _allPackages.Where(pkg =>
                pkg.AlwaysDownload ||
                (_checkBoxDict.TryGetValue(pkg.CheckBoxName, out var checkbox) && checkbox.Checked)
            ).ToList();
        }
        public async Task DownloadAndExtractPackagesAsync(List<PackageInfo> packages, Dictionary<string, CheckBox> checkBoxes, string usbRootPath, IProgress<int> progress = null)
        {
            if (string.IsNullOrWhiteSpace(usbRootPath) || !Directory.Exists(usbRootPath)) { UpdateStatus("Status: Please Select A Valid USB Device"); return; }
            string appTempFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
            if (!Directory.Exists(appTempFolder)) { Directory.CreateDirectory(appTempFolder); }
            bool skipMainFilesChecked = checkBoxes.TryGetValue("skipmainfilesToggle", out var skipMainFilesCb) && skipMainFilesCb.Checked;
            bool skipRbbChecked = checkBoxes.TryGetValue("skiprbbToggle", out var skipRbbCb) && skipRbbCb.Checked;
            bool skipXexChecked = checkBoxes.TryGetValue("skipxexToggle", out var skipXexCb) && skipXexCb.Checked;
            int totalPackages = packages.Count;
            int currentPackageIndex = 0;
            foreach (var pkg in packages)
            {
                if (skipMainFilesChecked) { string[] mainFilesToSkip = { "Payload-XeUnshackle.zip", "Payload-FreeMyXe.zip", "XeXMenu.zip", "RBB.zip" }; if (mainFilesToSkip.Contains(pkg.FileName, StringComparer.OrdinalIgnoreCase)) { continue; } }
                else
                {
                    if (pkg.FileName.Equals("RBB.zip", StringComparison.OrdinalIgnoreCase) && skipRbbChecked) { continue; }
                    if (pkg.FileName.Equals("XeXMenu.zip", StringComparison.OrdinalIgnoreCase) && skipXexChecked) { continue; }
                    if (pkg.FileName.Equals("Payload-XeUnshackle.zip", StringComparison.OrdinalIgnoreCase) && checkBoxes.TryGetValue("freemyxeToggle", out var freeMyXeCb) && freeMyXeCb.Checked) {  continue; }
                    if (pkg.FileName.Equals("Payload-FreeMyXe.zip", StringComparison.OrdinalIgnoreCase) && checkBoxes.TryGetValue("xeunshackleToggle", out var xeUnshackleCb) && xeUnshackleCb.Checked) { continue; }
                }
                if (!pkg.AlwaysDownload) { if (!checkBoxes.TryGetValue(pkg.CheckBoxName, out var cb) || !cb.Checked) { continue; } }
                currentPackageIndex++;
                var tempFilePath = Path.Combine(appTempFolder, pkg.FileName);
                bool needsDownload = true;
                if (File.Exists(tempFilePath)) { try { using (var archive = ZipFile.OpenRead(tempFilePath)) { needsDownload = false; } } catch { needsDownload = true; } }
                if (needsDownload)
                {
                    UpdateStatus($"Status: Downloading {pkg.FileName} ({currentPackageIndex}/{totalPackages})");
                    var downloadProgress = new Progress<int>(percent =>
                    {
                        int overallPercent = (int)(((currentPackageIndex - 1 + (percent / 100.0)) / totalPackages) * 100 * 0.5);
                        SetProgressBar(overallPercent);
                    });
                    try
                    {
                        await DownloadFileAsync(pkg.DownloadUrl, tempFilePath, downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Unable to download {pkg.FileName}. {ex.Message}", ex);
                    }
                }
                else { UpdateStatus($"Status: {pkg.FileName} already exists, skipping download");}
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
                else { UpdateStatus($"Status: Skipping extraction of {pkg.FileName} because file does not exist"); }
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
                UpdateFormatSelectionState(selectedDrive);
                warningLabel.Visible = false;
                Debug.WriteLine($"Selected drive: {DevicePath}");
            }
            else
            {
                DevicePath = null;
                DriveSet = false;
                UpdateFormatSelectionState(null);
            }
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
                        ulong capacity = (ulong)(volume["Capacity"] ?? 0UL);
                        const ulong F32L = 34359738368;
                        if (capacity >= F32L)
                        {
                            var result = MessageBox.Show(
                                "The selected drive is larger than 32GB and cannot be formatted to FAT32. Do you want to continue without formatting?",
                                "Drive Too Large",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning
                            );
                            if (result == DialogResult.Yes)
                            {
                                UpdateStatus("Status: Continuing Install...");
                                return true;
                            }
                            else
                            {
                                UpdateStatus("Status: Install Cancelled By User");
                                return false;
                            }
                        }
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
                            MessageBox.Show(
                                $"Format Failed w/ an Error Code: {returnValue}",
                                "BadStick Format Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            return false;
                        }
                    }
                }
                MessageBox.Show(
                    "An Unexpected Error Has Occured.",
                    "BadStick Format Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error Formatting Drive: {ex.Message}",
                    "BadStick Format Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }
        private async void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string usbPath;
                if (badavatarToggle.Checked == true && badupdateToggle.Checked == true && abadmemunitToggle.Checked == true && badavatarhddToggle.Checked == true) { MessageBox.Show("Please select only one exploit method.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (DeviceList.SelectedItem == null) { MessageBox.Show("Please select a FAT32-compatible device or browse to a mounted folder.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (DeviceList.SelectedItem is UsbDriveItem selectedDrive)
                {
                    usbPath = selectedDrive.RootPath;
                    if (!selectedDrive.CanFormat && !skipformatToggle.Checked)
                    {
                        MessageBox.Show(
                            "Formatting is only available for automatically detected Windows volumes. For a manually selected folder or mounted volume, enable Skip Format and make sure the target media is already FAT32-formatted.",
                            "BadStick Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        UpdateStatus("Status: Manual folder selected; skipping format is required.");
                        return;
                    }
                }
                else { MessageBox.Show("Please select a FAT32-compatible device or browse to a mounted folder.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (string.IsNullOrEmpty(usbPath) || !Directory.Exists(usbPath)) { MessageBox.Show("Please select a valid target device or folder.", "BadStick Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!skipformatToggle.Checked)
                {
                    var confirm = MessageBox.Show(
                        $"Are you sure you want to select {usbPath} as your FAT32 device to format and configure? This will erase all data on the device. Please" +
                        $" ensure that this is the device that you want to use before you go ahead. I am not responsible for any accidental " +
                        $"data loss on your behalf.",
                        "Confirm Format",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes) { UpdateStatus("Status: Format Cancelled"); return; }
                    UpdateStatus("Status: Formatting Device...");
                    ProgressBar.Value = 0;
                    bool formatSuccess = await Task.Run(() => FormatDriveToFat32(usbPath));
                    if (!formatSuccess)
                    {
                        MessageBox.Show("Failed to format the device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateStatus("Status: Format Failed");
                        return;
                    }
                    UpdateStatus("Status: Format Completed. Starting Downloads...");
                }
                else { UpdateStatus("Status: Skipping Format (per user request)..."); }
                var packagesToDownload = GetSelectedPackages();
                if (skipmainfilesToggle.Checked)
                {
                    string[] mainFiles = { "RBB.zip", "Payload-XeUnshackle.zip", "Payload-FreeMyXe.zip" };
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
                var progress = new Progress<int>(percent => { ProgressBar.Value = percent; });
                await DownloadAndExtractPackagesAsync(packagesToDownload, _checkBoxDict, usbPath, progress);
                UpdateStatus("Status: Done! Device Ready.");
                ProgressBar.Value = 100;
                MessageBox.Show(this, "BadStick has finished setting up your device. Any packages you have included with your install have been neatly arranged on your device.", "Plug and Play!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(500);
                await CountdownExitStatusAsync();
            }
            catch (Exception ex)
            {
                UpdateStatus("Status: Install failed");
                MessageBox.Show(this, ex.Message, "BadStick Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SelectAllToggle_CheckedChanged(object sender, EventArgs e)
        {
            bool checkAll = SelectAllToggle.Checked;

            if (!checkAll)
            {
                skipmainfilesToggle.Checked = false;
                skipmainfilesToggle.Enabled = true;
                skiprbbToggle.Checked = false;
                skiprbbToggle.Enabled = true;
                skipxexmenuToggle.Checked = false;
                skipxexmenuToggle.Enabled = true;
            }
            else
            {
                skipmainfilesToggle.Checked = true;
                skipmainfilesToggle.Enabled = false;
                skiprbbToggle.Checked = false;
                skiprbbToggle.Enabled = false;
                skipxexmenuToggle.Checked = false;
                skipxexmenuToggle.Enabled = false;
            }

            foreach (var kvp in _checkBoxDict) { kvp.Value.Checked = checkAll; kvp.Value.Enabled = true; }
            badupdateToggle.Checked = false;
            badavatarToggle.Checked = false;
            badavatarhddToggle.Checked = false;
            abadmemunitToggle.Checked = false;
            xeunshackleToggle.Checked = false;
            freemyxeToggle.Checked = false;

            badupdateToggle.Enabled = !checkAll;
            badavatarToggle.Enabled = !checkAll;
            badavatarhddToggle.Enabled = !checkAll;
            abadmemunitToggle.Enabled = !checkAll;
            xeunshackleToggle.Enabled = !checkAll;
            freemyxeToggle.Enabled = !checkAll;
            skipxexmenuToggle.Enabled = !checkAll;
            skiprbbToggle.Enabled = !checkAll;
        }
        private void RefDrivesBtn_Click(object sender, EventArgs e) { LoadUsbDrives(); }
        private void BrowseFolderBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder or mounted drive to use as the USB target.";
                dialog.ShowNewFolderButton = false;
                if (!string.IsNullOrWhiteSpace(DevicePath) && Directory.Exists(DevicePath))
                {
                    dialog.SelectedPath = DevicePath;
                }
                if (dialog.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    return;
                }
                _manualUsbDrive = new UsbDriveItem(dialog.SelectedPath, "Selected Folder", canFormat: false, isManualSelection: true);
                LoadUsbDrives();
                UpdateStatus($"Status: Using manually selected folder: {dialog.SelectedPath}");
            }
        }
        private void widBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Dashlaunch is not listed here, because it cannot be ran on BadUpdate " +
                "consoles. If you were to install Dashlaunch on a BadUpdate exploited console, it would" +
                " temporarily brick your nand, and you would then have to perform a RGH to revive it.", "Where is Dashlaunch?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void badstickredditBtn_Click(object sender, EventArgs e) { Process.Start("https://www.reddit.com/r/360hacks/comments/1mmaaz2/release_badstick_a_badupdate_usb_auto_installer/"); }
        private void reddit360Btn_Click(object sender, EventArgs e) { Process.Start("https://www.reddit.com/r/360hacks/"); }
        private void githubpageBtn_Click_1(object sender, EventArgs e) { Process.Start("https://github.com/LxcyDr0p/BadStick"); }
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
                abadmemunitToggle.Enabled = true;
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
                abadmemunitToggle.Enabled = false;
                abadmemunitToggle.Checked = false;
                return;
            }

            bool enable = skipmainfilesToggle.Checked;

            skipxexmenuToggle.Enabled = enable;
            skiprbbToggle.Enabled = enable;
        }
        private void xeunshackleToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!xeunshackleToggle.Checked){ freemyxeToggle.Enabled = true;  }
            else { freemyxeToggle.Checked = false; freemyxeToggle.Enabled = false; }
            if (xeunshackleToggle.Checked && freemyxeToggle.Checked) { StartBtn.Enabled = false; }
            else { StartBtn.Enabled = true; }
        }
        private void freemyxeToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!freemyxeToggle.Checked) { xeunshackleToggle.Enabled = true; }
            else { xeunshackleToggle.Checked = false; xeunshackleToggle.Enabled = false; }
            if (xeunshackleToggle.Checked && freemyxeToggle.Checked) { StartBtn.Enabled = false; }
            else { StartBtn.Enabled = true; }

        }
        private void editorBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            inieditor editor = new inieditor();
            editor.Show();
        }
        private void badupdateToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!badupdateToggle.Checked)
            {
                badavatarToggle.Enabled = true;
                badavatarhddToggle.Enabled = true;
                abadmemunitToggle.Enabled = true;
            }
            else
            {
                skiprbbToggle.Enabled = true;
                badavatarToggle.Enabled = true;
                badavatarhddToggle.Enabled = false;
                badavatarhddToggle.Checked = false;
                badavatarToggle.Enabled = false;
                badavatarToggle.Checked = false;
                abadmemunitToggle.Enabled = false;
                abadmemunitToggle.Checked = false;
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
                badupdateToggle.Enabled = true;
                badavatarhddToggle.Enabled = true;
                abadmemunitToggle.Enabled = true;
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
                badavatarhddToggle.Enabled = false;
                badavatarhddToggle.Checked = false;
                badupdateToggle.Enabled = false;
                badupdateToggle.Checked = false;
                abadmemunitToggle.Enabled = false;
                abadmemunitToggle.Checked = false;
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
                badavatarToggle.Enabled = true;
                badupdateToggle.Enabled = true;
                abadmemunitToggle.Enabled = true;
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
                badupdateToggle.Enabled = false;
                badupdateToggle.Checked = false;
                badavatarToggle.Enabled = false;
                badavatarToggle.Checked = false;
                abadmemunitToggle.Enabled = false;
                abadmemunitToggle.Checked = false;
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
                badavatarToggle.Enabled = true;
                badavatarhddToggle.Enabled = true;
                badupdateToggle.Enabled = true;
            }
            else
            {
                skiprbbToggle.Checked = true;
                skiprbbToggle.Enabled = false;
                freemyxeToggle.Enabled = false;
                xeunshackleToggle.Enabled = false;
                freemyxeToggle.Checked = false;
                xeunshackleToggle.Checked = false;
                badavatarhddToggle.Enabled = false;
                badavatarhddToggle.Checked = false;
                badavatarToggle.Enabled = false;
                badavatarToggle.Checked = false;
                badupdateToggle.Enabled = false;
                badupdateToggle.Checked = false;
            }
        }
        private void wipetempBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete ./temp And Everything Inside It? You Will Have To Re-Download All Packages You Have Already Downloaded.", "BadStick Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists("./temp"))
                    Directory.Delete("./temp", true);
            }
        }
        private void deleteselfBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Annihilate BadStick?", "\"BadStick kys bro\"", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string exePath = Application.ExecutablePath;
                Process.Start(new ProcessStartInfo() { FileName = "cmd.exe", Arguments = $"/C timeout 2 > nul & del \"{exePath}\"", CreateNoWindow = true, UseShellExecute = false });
                Application.Exit();
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            IsAdmin = IsRunAsAdmin();
            UpdateFormatSelectionState(DeviceList.SelectedItem as UsbDriveItem);
        }
        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.com/invite/HzUP3shMgQ");
        }
    }
}
