// For Windows 11 Installation
// Copyright (c) 2024 Neupionier (Neupionier)
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace ForWindows11Installation
{
    public partial class MainForm : Form
    {
        private string downloadFolder = "";
        private string isoPath = "";

        public MainForm()
        {
            InitializeComponent();

            // 다국어 적용
            ApplyLocalization();

            // 언어 콤보박스 초기화
            cbLanguage.Items.AddRange(new string[] { "English", "한국어", "Deutsch" });
            cbLanguage.SelectedIndex = 0;
            cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;

            // USB 드라이브 목록 갱신
            RefreshUsbDrives();

            // ProgressBar 초기화
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            // 이벤트 연결
            btnSelectISO.Click += btnSelectISO_Click;
            btnStart.Click += btnStart_Click;
            btnOpenMsIsoPage.Click += btnOpenMsIsoPage_Click; // MS 공식 ISO 다운로드 버튼
            lblFooter.Text = Properties.Resources.Footer;
            lblFooter.ForeColor = System.Drawing.Color.Blue;
            lblFooter.Cursor = Cursors.Hand;
            lblFooter.Click += (s, e) => Process.Start("https://github.com/Neupionier");
        }


        private void RefreshUsbDrives()
        {
            cbUsbDrives.Items.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable && drive.IsReady && drive.TotalSize >= 8L * 1024 * 1024 * 1024)
                {
                    cbUsbDrives.Items.Add($"{drive.Name} ({drive.VolumeLabel}) - {drive.TotalSize / (1024 * 1024 * 1024)}GB");
                }
            }
            if (cbUsbDrives.Items.Count > 0) cbUsbDrives.SelectedIndex = 0;
        }

        private void ApplyLocalization()
        {
            this.Text = Properties.Resources.App_Title;
            groupBoxISO.Text = Properties.Resources.Label_SelectISO;
            labelUsb.Text = Properties.Resources.Label_USBSelect;
            btnStart.Text = Properties.Resources.Btn_Start;
            btnOpenMsIsoPage.Text = Properties.Resources.Btn_OpenMsIsoPage;
            btnSelectISO.Text = Properties.Resources.Btn_SelectISO; // ← 이 줄 추가!
            lblFooter.Text = Properties.Resources.Footer;
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lang = "en";
            if (cbLanguage.SelectedIndex == 1) lang = "ko";
            else if (cbLanguage.SelectedIndex == 2) lang = "de";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            ApplyLocalization();
        }

        private void btnSelectDownloadFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    downloadFolder = fbd.SelectedPath;
                }
            }
        }

        private void btnSelectISO_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "ISO files (*.iso)|*.iso";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    isoPath = ofd.FileName;
                }
            }
        }

        // MS 공식 ISO 다운로드 버튼 이벤트
        private void btnOpenMsIsoPage_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.microsoft.com/software-download/windows11");
            MessageBox.Show(Properties.Resources.Msg_OpenMsIsoPage, Properties.Resources.App_Title);
        }

        // ISO 전체 복사 (C# Directory/File 재귀 복사)
        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                try
                {
                    File.Copy(file, destFile, true);
                }
                catch (Exception ex)
                {
                    // 권한 문제 등은 무시하고 계속 진행
                    Console.WriteLine($"파일 복사 실패: {file} -> {destFile} : {ex.Message}");
                }
            }

            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destSubDir);
            }
        }

        // ISO 마운트 및 드라이브 문자 찾기 (Process.Start 방식)
        private string MountISOAndGetDriveLetter(string isoPath)
        {
            try
            {
                // 1. ISO 마운트 (관리자 권한 필요)
                var mountPsi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"Mount-DiskImage -ImagePath '{isoPath}'\"",
                    Verb = "runas",
                    CreateNoWindow = true,
                    UseShellExecute = true
                };
                var mountProc = Process.Start(mountPsi);
                mountProc.WaitForExit();

                // 2. 드라이브 문자 찾기 (출력값 읽기)
                var getDrivePsi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"$iso = Get-DiskImage -ImagePath '{isoPath}'; $vol = Get-Volume -DiskImage $iso; $vol.DriveLetter\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var getDriveProc = Process.Start(getDrivePsi);
                string output = getDriveProc.StandardOutput.ReadToEnd();
                getDriveProc.WaitForExit();

                string driveLetter = output.Trim();
                if (!string.IsNullOrEmpty(driveLetter))
                    return driveLetter + ":\\";
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.Msg_Error + ex.Message, Properties.Resources.App_Title);
                throw;
            }
        }

        // 드라이브 문자(E:)로부터 디스크 번호 찾기 (WMI)
        private int GetDiskNumberFromDriveLetter(string driveLetter)
        {
            driveLetter = driveLetter.TrimEnd('\\', ':');
            using (var searcher = new ManagementObjectSearcher(
                @"SELECT * FROM Win32_LogicalDiskToPartition"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    string dependent = mo["Dependent"].ToString();
                    if (dependent.Contains(driveLetter + ":"))
                    {
                        string antecedent = mo["Antecedent"].ToString();
                        int diskIndexStart = antecedent.IndexOf("Disk #") + 6;
                        int diskIndexEnd = antecedent.IndexOf(",", diskIndexStart);
                        string diskIndexStr = antecedent.Substring(diskIndexStart, diskIndexEnd - diskIndexStart).Trim();
                        if (int.TryParse(diskIndexStr, out int diskNumber))
                            return diskNumber;
                    }
                }
            }
            throw new Exception(Properties.Resources.Msg_Error + " (Disk Number)");
        }

        // select disk로 USB 전체 포맷 (NTFS, MBR, Active)
        private void FormatUsbByDiskNumber(int usbDiskNumber)
        {
            string script = $@"
select disk {usbDiskNumber}
clean
convert mbr
create partition primary
select partition 1
active
format fs=ntfs quick
assign
exit";
            string scriptPath = Path.GetTempFileName();
            File.WriteAllText(scriptPath, script);

            var psi = new ProcessStartInfo("diskpart.exe", $"/s \"{scriptPath}\"")
            {
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true
            };
            var proc = Process.Start(psi);
            proc.WaitForExit();
            File.Delete(scriptPath);
        }

        // 포맷 후 드라이브 선택 다이얼로그
        private string ShowDriveSelectDialog()
        {
            // 드라이브 리스트 새로 갱신
            RefreshUsbDrives();

            Form dialog = new Form();
            dialog.Text = Properties.Resources.Label_USBSelect;
            dialog.Width = 350;
            dialog.Height = 120;
            Label label = new Label() { Text = Properties.Resources.Msg_SelectUSB, Left = 10, Top = 10, Width = 300 };
            ComboBox combo = new ComboBox() { Left = 10, Top = 35, Width = 300 };
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable && drive.IsReady && drive.TotalSize >= 8L * 1024 * 1024 * 1024)
                    combo.Items.Add($"{drive.Name} ({drive.VolumeLabel}) - {drive.TotalSize / (1024 * 1024 * 1024)}GB");
            }
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
            Button ok = new Button() { Text = "OK", Left = 220, Width = 90, Top = 65, DialogResult = DialogResult.OK };
            dialog.Controls.Add(label);
            dialog.Controls.Add(combo);
            dialog.Controls.Add(ok);
            dialog.AcceptButton = ok;

            if (dialog.ShowDialog() == DialogResult.OK && combo.SelectedItem != null)
            {
                // 드라이브 문자만 추출 (예: "E:\")
                return combo.SelectedItem.ToString().Substring(0, 3);
            }
            else
            {
                return null;
            }
        }

        // 시작 버튼 클릭
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(isoPath))
            {
                MessageBox.Show(Properties.Resources.Msg_SelectISO, Properties.Resources.App_Title);
                return;
            }

            if (MessageBox.Show(Properties.Resources.Msg_Warning, Properties.Resources.App_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                progressBar.Value = 0;

                // 1. USB 포맷 (select disk)
                string driveLetterBefore = cbUsbDrives.SelectedItem.ToString().Substring(0, 2); // 예: "E:"
                int usbDiskNumber = GetDiskNumberFromDriveLetter(driveLetterBefore);
                FormatUsbByDiskNumber(usbDiskNumber);
                progressBar.Value = 40;

                // 2. 포맷 후 드라이브 선택 다이얼로그
                string driveLetterAfter = ShowDriveSelectDialog();
                if (string.IsNullOrEmpty(driveLetterAfter))
                {
                    MessageBox.Show(Properties.Resources.Msg_SelectUSB, Properties.Resources.App_Title);
                    return;
                }

                // 3. ISO 마운트
                string isoDrive = MountISOAndGetDriveLetter(isoPath);
                if (string.IsNullOrEmpty(isoDrive))
                {
                    MessageBox.Show(Properties.Resources.Msg_Error + " (ISO Mount)", Properties.Resources.App_Title);
                    return;
                }
                progressBar.Value = 60;

                // 4. 복사 직전 경로 확인
                MessageBox.Show($"isoDrive: {isoDrive}\ndriveLetterAfter: {driveLetterAfter}", Properties.Resources.Msg_Copying);

                // 5. ISO 전체 복사 (C# Directory/File 재귀 복사)
                CopyDirectory(isoDrive, driveLetterAfter);
                progressBar.Value = 100;

                MessageBox.Show(Properties.Resources.Msg_Done, Properties.Resources.App_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.Msg_Error + ex.Message, Properties.Resources.App_Title);
            }
        }
    }
}