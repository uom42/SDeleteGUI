using SDeleteGUI.Core;

namespace SDeleteGUI
{
	partial class frmMain
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.tlpParams = new System.Windows.Forms.TableLayoutPanel();
			this.lblSDeleteBinPath = new System.Windows.Forms.LinkLabel();
			this.optSource_Dir = new System.Windows.Forms.RadioButton();
			this.optSource_Files = new System.Windows.Forms.RadioButton();
			this.txtSource_Dir = new System.Windows.Forms.TextBox();
			this.txtSource_Files = new System.Windows.Forms.TextBox();
			this.lblWhatToClean = new System.Windows.Forms.Label();
			this.btnSource_DisplaySelectionUI = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pnlSeparetor1 = new System.Windows.Forms.Panel();
			this.optSource_PhyDisk = new System.Windows.Forms.RadioButton();
			this.cboSource_PhyDisk = new System.Windows.Forms.ComboBox();
			this.optSource_LogDisk = new System.Windows.Forms.RadioButton();
			this.cboSource_LogDisk = new System.Windows.Forms.ComboBox();
			this.btnSource_Refresh = new System.Windows.Forms.Button();
			this.tlpSection2 = new System.Windows.Forms.TableLayoutPanel();
			this.pnlSeparator2 = new System.Windows.Forms.Label();
			this.numPasses = new System.Windows.Forms.NumericUpDown();
			this.tlpCleanFreeSpaceMethods = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.optCleanMode_Clean = new System.Windows.Forms.RadioButton();
			this.optCleanMode_Zero = new System.Windows.Forms.RadioButton();
			this.pnlSeparator3 = new System.Windows.Forms.Panel();
			this.lstLog = new SDeleteGUI.Core.ListBoxEx();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.tlpDown = new System.Windows.Forms.TableLayoutPanel();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tmrElapsed = new System.Windows.Forms.Timer(this.components);
			this.tlpParams.SuspendLayout();
			this.tlpSection2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPasses)).BeginInit();
			this.tlpCleanFreeSpaceMethods.SuspendLayout();
			this.tlpDown.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpParams
			// 
			this.tlpParams.AutoSize = true;
			this.tlpParams.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpParams.ColumnCount = 4;
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.tlpParams.Controls.Add(this.lblSDeleteBinPath, 0, 0);
			this.tlpParams.Controls.Add(this.optSource_Dir, 1, 5);
			this.tlpParams.Controls.Add(this.optSource_Files, 1, 6);
			this.tlpParams.Controls.Add(this.txtSource_Dir, 2, 5);
			this.tlpParams.Controls.Add(this.txtSource_Files, 2, 6);
			this.tlpParams.Controls.Add(this.lblWhatToClean, 0, 2);
			this.tlpParams.Controls.Add(this.btnSource_DisplaySelectionUI, 3, 5);
			this.tlpParams.Controls.Add(this.panel1, 0, 7);
			this.tlpParams.Controls.Add(this.pnlSeparetor1, 0, 1);
			this.tlpParams.Controls.Add(this.optSource_PhyDisk, 1, 3);
			this.tlpParams.Controls.Add(this.cboSource_PhyDisk, 2, 3);
			this.tlpParams.Controls.Add(this.optSource_LogDisk, 1, 4);
			this.tlpParams.Controls.Add(this.cboSource_LogDisk, 2, 4);
			this.tlpParams.Controls.Add(this.btnSource_Refresh, 3, 3);
			this.tlpParams.Controls.Add(this.tlpSection2, 0, 8);
			this.tlpParams.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpParams.Location = new System.Drawing.Point(14, 14);
			this.tlpParams.Name = "tlpParams";
			this.tlpParams.RowCount = 9;
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tlpParams.Size = new System.Drawing.Size(682, 188);
			this.tlpParams.TabIndex = 1;
			// 
			// lblSDeleteBinPath
			// 
			this.lblSDeleteBinPath.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.lblSDeleteBinPath, 3);
			this.lblSDeleteBinPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSDeleteBinPath.LinkArea = new System.Windows.Forms.LinkArea(0, 7);
			this.lblSDeleteBinPath.Location = new System.Drawing.Point(3, 0);
			this.lblSDeleteBinPath.Name = "lblSDeleteBinPath";
			this.lblSDeleteBinPath.Size = new System.Drawing.Size(650, 17);
			this.lblSDeleteBinPath.TabIndex = 0;
			this.lblSDeleteBinPath.TabStop = true;
			this.lblSDeleteBinPath.Text = "SDelete binary location:";
			this.lblSDeleteBinPath.UseCompatibleTextRendering = true;
			// 
			// optSource_Dir
			// 
			this.optSource_Dir.AutoSize = true;
			this.optSource_Dir.Checked = true;
			this.optSource_Dir.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Dir.Location = new System.Drawing.Point(20, 96);
			this.optSource_Dir.Name = "optSource_Dir";
			this.optSource_Dir.Size = new System.Drawing.Size(148, 20);
			this.optSource_Dir.TabIndex = 7;
			this.optSource_Dir.TabStop = true;
			this.optSource_Dir.Text = "Folder and all its contents:";
			this.optSource_Dir.UseVisualStyleBackColor = true;
			// 
			// optSource_Files
			// 
			this.optSource_Files.AutoSize = true;
			this.optSource_Files.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Files.Location = new System.Drawing.Point(20, 122);
			this.optSource_Files.Name = "optSource_Files";
			this.optSource_Files.Size = new System.Drawing.Size(52, 20);
			this.optSource_Files.TabIndex = 9;
			this.optSource_Files.Text = "File(s)";
			this.optSource_Files.UseVisualStyleBackColor = true;
			// 
			// txtSource_Dir
			// 
			this.txtSource_Dir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Dir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.txtSource_Dir.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Dir.Location = new System.Drawing.Point(222, 96);
			this.txtSource_Dir.Name = "txtSource_Dir";
			this.txtSource_Dir.Size = new System.Drawing.Size(431, 20);
			this.txtSource_Dir.TabIndex = 8;
			// 
			// txtSource_Files
			// 
			this.txtSource_Files.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Files.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtSource_Files.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Files.Location = new System.Drawing.Point(222, 122);
			this.txtSource_Files.Name = "txtSource_Files";
			this.txtSource_Files.ReadOnly = true;
			this.txtSource_Files.Size = new System.Drawing.Size(431, 20);
			this.txtSource_Files.TabIndex = 10;
			// 
			// lblWhatToClean
			// 
			this.lblWhatToClean.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.lblWhatToClean, 4);
			this.lblWhatToClean.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblWhatToClean.Location = new System.Drawing.Point(3, 26);
			this.lblWhatToClean.Name = "lblWhatToClean";
			this.lblWhatToClean.Size = new System.Drawing.Size(676, 13);
			this.lblWhatToClean.TabIndex = 1;
			this.lblWhatToClean.Text = "What to clean:";
			// 
			// btnSource_DisplaySelectionUI
			// 
			this.btnSource_DisplaySelectionUI.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSource_DisplaySelectionUI.Location = new System.Drawing.Point(659, 96);
			this.btnSource_DisplaySelectionUI.Name = "btnSource_DisplaySelectionUI";
			this.tlpParams.SetRowSpan(this.btnSource_DisplaySelectionUI, 2);
			this.btnSource_DisplaySelectionUI.Size = new System.Drawing.Size(20, 46);
			this.btnSource_DisplaySelectionUI.TabIndex = 11;
			this.btnSource_DisplaySelectionUI.Text = "...";
			this.btnSource_DisplaySelectionUI.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel1, 4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 148);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(676, 2);
			this.panel1.TabIndex = 4;
			// 
			// pnlSeparetor1
			// 
			this.pnlSeparetor1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.pnlSeparetor1, 4);
			this.pnlSeparetor1.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSeparetor1.Location = new System.Drawing.Point(3, 20);
			this.pnlSeparetor1.Name = "pnlSeparetor1";
			this.pnlSeparetor1.Size = new System.Drawing.Size(676, 3);
			this.pnlSeparetor1.TabIndex = 9;
			// 
			// optSource_PhyDisk
			// 
			this.optSource_PhyDisk.AutoSize = true;
			this.optSource_PhyDisk.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_PhyDisk.Location = new System.Drawing.Point(20, 42);
			this.optSource_PhyDisk.Name = "optSource_PhyDisk";
			this.optSource_PhyDisk.Size = new System.Drawing.Size(196, 21);
			this.optSource_PhyDisk.TabIndex = 2;
			this.optSource_PhyDisk.TabStop = true;
			this.optSource_PhyDisk.Text = "Physical Disk (without any partitions)";
			this.optSource_PhyDisk.UseVisualStyleBackColor = true;
			// 
			// cboSource_PhyDisk
			// 
			this.cboSource_PhyDisk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboSource_PhyDisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSource_PhyDisk.FormattingEnabled = true;
			this.cboSource_PhyDisk.Location = new System.Drawing.Point(222, 42);
			this.cboSource_PhyDisk.Name = "cboSource_PhyDisk";
			this.cboSource_PhyDisk.Size = new System.Drawing.Size(431, 21);
			this.cboSource_PhyDisk.TabIndex = 3;
			// 
			// optSource_LogDisk
			// 
			this.optSource_LogDisk.AutoSize = true;
			this.optSource_LogDisk.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_LogDisk.Location = new System.Drawing.Point(20, 69);
			this.optSource_LogDisk.Name = "optSource_LogDisk";
			this.optSource_LogDisk.Size = new System.Drawing.Size(176, 21);
			this.optSource_LogDisk.TabIndex = 4;
			this.optSource_LogDisk.TabStop = true;
			this.optSource_LogDisk.Text = "Logical drive (clean free space):";
			this.optSource_LogDisk.UseVisualStyleBackColor = true;
			// 
			// cboSource_LogDisk
			// 
			this.cboSource_LogDisk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboSource_LogDisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSource_LogDisk.FormattingEnabled = true;
			this.cboSource_LogDisk.Location = new System.Drawing.Point(222, 69);
			this.cboSource_LogDisk.Name = "cboSource_LogDisk";
			this.cboSource_LogDisk.Size = new System.Drawing.Size(431, 21);
			this.cboSource_LogDisk.TabIndex = 5;
			// 
			// btnSource_Refresh
			// 
			this.btnSource_Refresh.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSource_Refresh.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.btnSource_Refresh.Location = new System.Drawing.Point(659, 42);
			this.btnSource_Refresh.Name = "btnSource_Refresh";
			this.tlpParams.SetRowSpan(this.btnSource_Refresh, 2);
			this.btnSource_Refresh.Size = new System.Drawing.Size(20, 48);
			this.btnSource_Refresh.TabIndex = 6;
			this.btnSource_Refresh.Text = "🗘";
			this.btnSource_Refresh.UseVisualStyleBackColor = true;
			// 
			// tlpSection2
			// 
			this.tlpSection2.AutoSize = true;
			this.tlpSection2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpSection2.ColumnCount = 3;
			this.tlpParams.SetColumnSpan(this.tlpSection2, 4);
			this.tlpSection2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpSection2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
			this.tlpSection2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSection2.Controls.Add(this.pnlSeparator2, 0, 0);
			this.tlpSection2.Controls.Add(this.numPasses, 1, 0);
			this.tlpSection2.Controls.Add(this.tlpCleanFreeSpaceMethods, 2, 0);
			this.tlpSection2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpSection2.Location = new System.Drawing.Point(3, 156);
			this.tlpSection2.Name = "tlpSection2";
			this.tlpSection2.RowCount = 1;
			this.tlpSection2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpSection2.Size = new System.Drawing.Size(676, 29);
			this.tlpSection2.TabIndex = 12;
			// 
			// pnlSeparator2
			// 
			this.pnlSeparator2.AutoSize = true;
			this.pnlSeparator2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlSeparator2.Location = new System.Drawing.Point(3, 0);
			this.pnlSeparator2.Name = "pnlSeparator2";
			this.pnlSeparator2.Size = new System.Drawing.Size(92, 29);
			this.pnlSeparator2.TabIndex = 0;
			this.pnlSeparator2.Text = "Overwrite Passes:";
			this.pnlSeparator2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numPasses
			// 
			this.numPasses.Dock = System.Windows.Forms.DockStyle.Top;
			this.numPasses.Location = new System.Drawing.Point(101, 3);
			this.numPasses.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPasses.Name = "numPasses";
			this.numPasses.Size = new System.Drawing.Size(63, 20);
			this.numPasses.TabIndex = 1;
			this.numPasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numPasses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// tlpCleanFreeSpaceMethods
			// 
			this.tlpCleanFreeSpaceMethods.AutoSize = true;
			this.tlpCleanFreeSpaceMethods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpCleanFreeSpaceMethods.ColumnCount = 3;
			this.tlpCleanFreeSpaceMethods.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpCleanFreeSpaceMethods.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpCleanFreeSpaceMethods.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpCleanFreeSpaceMethods.Controls.Add(this.label4, 0, 0);
			this.tlpCleanFreeSpaceMethods.Controls.Add(this.optCleanMode_Clean, 1, 0);
			this.tlpCleanFreeSpaceMethods.Controls.Add(this.optCleanMode_Zero, 2, 0);
			this.tlpCleanFreeSpaceMethods.Dock = System.Windows.Forms.DockStyle.Right;
			this.tlpCleanFreeSpaceMethods.Location = new System.Drawing.Point(250, 3);
			this.tlpCleanFreeSpaceMethods.Name = "tlpCleanFreeSpaceMethods";
			this.tlpCleanFreeSpaceMethods.RowCount = 1;
			this.tlpCleanFreeSpaceMethods.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCleanFreeSpaceMethods.Size = new System.Drawing.Size(423, 23);
			this.tlpCleanFreeSpaceMethods.TabIndex = 12;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Right;
			this.label4.Location = new System.Drawing.Point(3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Free space cleaning method:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// optCleanMode_Clean
			// 
			this.optCleanMode_Clean.AutoSize = true;
			this.optCleanMode_Clean.Checked = true;
			this.optCleanMode_Clean.Location = new System.Drawing.Point(153, 3);
			this.optCleanMode_Clean.Name = "optCleanMode_Clean";
			this.optCleanMode_Clean.Size = new System.Drawing.Size(55, 17);
			this.optCleanMode_Clean.TabIndex = 1;
			this.optCleanMode_Clean.TabStop = true;
			this.optCleanMode_Clean.Text = "Clean.";
			this.optCleanMode_Clean.UseVisualStyleBackColor = true;
			// 
			// optCleanMode_Zero
			// 
			this.optCleanMode_Zero.AutoSize = true;
			this.optCleanMode_Zero.Location = new System.Drawing.Point(214, 3);
			this.optCleanMode_Zero.Name = "optCleanMode_Zero";
			this.optCleanMode_Zero.Size = new System.Drawing.Size(206, 17);
			this.optCleanMode_Zero.TabIndex = 2;
			this.optCleanMode_Zero.Text = "Zero (good for virtual disk optimization)";
			this.optCleanMode_Zero.UseVisualStyleBackColor = true;
			// 
			// pnlSeparator3
			// 
			this.pnlSeparator3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpDown.SetColumnSpan(this.pnlSeparator3, 2);
			this.pnlSeparator3.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSeparator3.Location = new System.Drawing.Point(3, 3);
			this.pnlSeparator3.Name = "pnlSeparator3";
			this.pnlSeparator3.Size = new System.Drawing.Size(676, 3);
			this.pnlSeparator3.TabIndex = 22;
			// 
			// lstLog
			// 
			this.tlpDown.SetColumnSpan(this.lstLog, 2);
			this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLog.IntegralHeight = false;
			this.lstLog.Location = new System.Drawing.Point(3, 25);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(676, 219);
			this.lstLog.TabIndex = 1;
			// 
			// pbProgress
			// 
			this.tlpDown.SetColumnSpan(this.pbProgress, 2);
			this.pbProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbProgress.Location = new System.Drawing.Point(3, 257);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(676, 7);
			this.pbProgress.TabIndex = 25;
			// 
			// btnStartStop
			// 
			this.btnStartStop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnStartStop.Location = new System.Drawing.Point(575, 271);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(104, 29);
			this.btnStartStop.TabIndex = 2;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			// 
			// tlpDown
			// 
			this.tlpDown.ColumnCount = 2;
			this.tlpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.tlpDown.Controls.Add(this.pbProgress, 0, 4);
			this.tlpDown.Controls.Add(this.btnStartStop, 1, 5);
			this.tlpDown.Controls.Add(this.lstLog, 0, 2);
			this.tlpDown.Controls.Add(this.lblStatus, 0, 5);
			this.tlpDown.Controls.Add(this.label1, 0, 1);
			this.tlpDown.Controls.Add(this.pnlSeparator3, 0, 0);
			this.tlpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpDown.Location = new System.Drawing.Point(14, 202);
			this.tlpDown.Name = "tlpDown";
			this.tlpDown.RowCount = 6;
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpDown.Size = new System.Drawing.Size(682, 303);
			this.tlpDown.TabIndex = 1;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblStatus.Location = new System.Drawing.Point(3, 268);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(566, 35);
			this.lblStatus.TabIndex = 26;
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Output:";
			// 
			// tmrElapsed
			// 
			this.tmrElapsed.Interval = 1000;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(710, 519);
			this.Controls.Add(this.tlpDown);
			this.Controls.Add(this.tlpParams);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(517, 439);
			this.Name = "frmMain";
			this.Padding = new System.Windows.Forms.Padding(14, 14, 14, 14);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmMain";
			this.tlpParams.ResumeLayout(false);
			this.tlpParams.PerformLayout();
			this.tlpSection2.ResumeLayout(false);
			this.tlpSection2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPasses)).EndInit();
			this.tlpCleanFreeSpaceMethods.ResumeLayout(false);
			this.tlpCleanFreeSpaceMethods.PerformLayout();
			this.tlpDown.ResumeLayout(false);
			this.tlpDown.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpParams;
		private LinkLabel lblSDeleteBinPath;
		private System.Windows.Forms.RadioButton optSource_Dir;
		private System.Windows.Forms.RadioButton optSource_Files;
		private System.Windows.Forms.TextBox txtSource_Dir;
		private System.Windows.Forms.TextBox txtSource_Files;
		private Label lblWhatToClean;
		private Button btnSource_DisplaySelectionUI;
		private Panel panel1;
		private Panel pnlSeparetor1;
		private Label pnlSeparator2;
		private NumericUpDown numPasses;
		private RadioButton optCleanMode_Zero;
		private RadioButton optCleanMode_Clean;
		private TableLayoutPanel tlpCleanFreeSpaceMethods;
		private Label label4;
		private RadioButton optSource_PhyDisk;
		private ComboBox cboSource_PhyDisk;
		private Button btnStartStop;
		private ListBoxEx lstLog;
		private ProgressBar pbProgress;
		private TableLayoutPanel tlpDown;
		private RadioButton optSource_LogDisk;
		private ComboBox cboSource_LogDisk;
		private Button btnSource_Refresh;
		private Panel pnlSeparator3;
		private Label lblStatus;
		private Timer tmrElapsed;
		private TableLayoutPanel tlpSection2;
		private Label label1;
	}
}