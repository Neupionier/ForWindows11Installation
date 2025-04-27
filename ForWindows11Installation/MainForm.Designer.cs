namespace ForWindows11Installation
{
    partial class MainForm
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
            this.lblFooter = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbUsbDrives = new System.Windows.Forms.ComboBox();
            this.labelUsb = new System.Windows.Forms.Label();
            this.groupBoxISO = new System.Windows.Forms.GroupBox();
            this.btnSelectISO = new System.Windows.Forms.Button();
            this.btnOpenMsIsoPage = new System.Windows.Forms.Button();
            this.groupBoxISO.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFooter.ForeColor = System.Drawing.Color.Blue;
            this.lblFooter.Location = new System.Drawing.Point(0, 286);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(317, 16);
            this.lblFooter.TabIndex = 13;
            this.lblFooter.Text = "by Neupionier | github.com/Neupionier | MIT License";
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Items.AddRange(new object[] {
            "English",
            "한국어",
            "Deutsch"});
            this.cbLanguage.Location = new System.Drawing.Point(3, 215);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(121, 24);
            this.cbLanguage.TabIndex = 12;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 183);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 11;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 151);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbUsbDrives
            // 
            this.cbUsbDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsbDrives.FormattingEnabled = true;
            this.cbUsbDrives.Location = new System.Drawing.Point(3, 124);
            this.cbUsbDrives.Name = "cbUsbDrives";
            this.cbUsbDrives.Size = new System.Drawing.Size(121, 24);
            this.cbUsbDrives.TabIndex = 9;
            // 
            // labelUsb
            // 
            this.labelUsb.AutoSize = true;
            this.labelUsb.Location = new System.Drawing.Point(5, 105);
            this.labelUsb.Name = "labelUsb";
            this.labelUsb.Size = new System.Drawing.Size(107, 16);
            this.labelUsb.TabIndex = 8;
            this.labelUsb.Text = "USB 드라이브 선택";
            // 
            // groupBoxISO
            // 
            this.groupBoxISO.Controls.Add(this.btnOpenMsIsoPage);
            this.groupBoxISO.Controls.Add(this.btnSelectISO);
            this.groupBoxISO.Location = new System.Drawing.Point(3, 2);
            this.groupBoxISO.Name = "groupBoxISO";
            this.groupBoxISO.Size = new System.Drawing.Size(302, 100);
            this.groupBoxISO.TabIndex = 7;
            this.groupBoxISO.TabStop = false;
            this.groupBoxISO.Text = "ISO준비";
            // 
            // btnSelectISO
            // 
            this.btnSelectISO.Location = new System.Drawing.Point(0, 62);
            this.btnSelectISO.Name = "btnSelectISO";
            this.btnSelectISO.Size = new System.Drawing.Size(148, 23);
            this.btnSelectISO.TabIndex = 3;
            this.btnSelectISO.Text = "ISO 파일 선택";
            this.btnSelectISO.UseVisualStyleBackColor = true;
            // 
            // btnOpenMsIsoPage
            // 
            this.btnOpenMsIsoPage.Location = new System.Drawing.Point(0, 21);
            this.btnOpenMsIsoPage.Name = "btnOpenMsIsoPage";
            this.btnOpenMsIsoPage.Size = new System.Drawing.Size(296, 24);
            this.btnOpenMsIsoPage.TabIndex = 4;
            this.btnOpenMsIsoPage.Text = "MS 공식 ISO 다운로드";
            this.btnOpenMsIsoPage.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 303);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cbUsbDrives);
            this.Controls.Add(this.labelUsb);
            this.Controls.Add(this.groupBoxISO);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBoxISO.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbUsbDrives;
        private System.Windows.Forms.Label labelUsb;
        private System.Windows.Forms.GroupBox groupBoxISO;
        private System.Windows.Forms.Button btnSelectISO;
        private System.Windows.Forms.Button btnOpenMsIsoPage;
    }
}

