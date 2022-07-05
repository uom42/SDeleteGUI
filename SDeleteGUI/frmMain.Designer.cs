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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.tlpParams = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSDeleteBinPath = new System.Windows.Forms.Label();
			this.optSource_Dir = new System.Windows.Forms.RadioButton();
			this.optSource_Files = new System.Windows.Forms.RadioButton();
			this.txtSource_Dir = new System.Windows.Forms.TextBox();
			this.txtSource_Files = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSource_DisplaySelectionUI = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.numPasses = new System.Windows.Forms.NumericUpDown();
			this.tlpCleanModes = new System.Windows.Forms.TableLayoutPanel();
			this.optCleanMode_Zero = new System.Windows.Forms.RadioButton();
			this.optCleanMode_Clean = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.optSource_PhyDisk = new System.Windows.Forms.RadioButton();
			this.cboSource_PhyDisk = new System.Windows.Forms.ComboBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.optSource_LogDisk = new System.Windows.Forms.RadioButton();
			this.cboSource_LogDisk = new System.Windows.Forms.ComboBox();
			this.btnSource_Refresh = new System.Windows.Forms.Button();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.tlpDown = new System.Windows.Forms.TableLayoutPanel();
			this.tlpParams.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPasses)).BeginInit();
			this.tlpCleanModes.SuspendLayout();
			this.tlpDown.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpParams
			// 
			this.tlpParams.AutoSize = true;
			this.tlpParams.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpParams.ColumnCount = 4;
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpParams.Controls.Add(this.label1, 0, 0);
			this.tlpParams.Controls.Add(this.lblSDeleteBinPath, 1, 1);
			this.tlpParams.Controls.Add(this.optSource_Dir, 1, 6);
			this.tlpParams.Controls.Add(this.optSource_Files, 1, 7);
			this.tlpParams.Controls.Add(this.txtSource_Dir, 2, 6);
			this.tlpParams.Controls.Add(this.txtSource_Files, 2, 7);
			this.tlpParams.Controls.Add(this.label2, 0, 3);
			this.tlpParams.Controls.Add(this.btnSource_DisplaySelectionUI, 3, 6);
			this.tlpParams.Controls.Add(this.panel1, 0, 8);
			this.tlpParams.Controls.Add(this.panel2, 0, 2);
			this.tlpParams.Controls.Add(this.label3, 2, 9);
			this.tlpParams.Controls.Add(this.numPasses, 1, 9);
			this.tlpParams.Controls.Add(this.tlpCleanModes, 1, 11);
			this.tlpParams.Controls.Add(this.label4, 0, 10);
			this.tlpParams.Controls.Add(this.optSource_PhyDisk, 1, 4);
			this.tlpParams.Controls.Add(this.cboSource_PhyDisk, 2, 4);
			this.tlpParams.Controls.Add(this.panel3, 0, 12);
			this.tlpParams.Controls.Add(this.optSource_LogDisk, 1, 5);
			this.tlpParams.Controls.Add(this.cboSource_LogDisk, 2, 5);
			this.tlpParams.Controls.Add(this.btnSource_Refresh, 3, 4);
			this.tlpParams.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpParams.Location = new System.Drawing.Point(16, 16);
			this.tlpParams.Name = "tlpParams";
			this.tlpParams.RowCount = 13;
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpParams.Size = new System.Drawing.Size(796, 299);
			this.tlpParams.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label1, 3);
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(760, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "SDelete[64] binary location:";
			// 
			// lblSDeleteBinPath
			// 
			this.tlpParams.SetColumnSpan(this.lblSDeleteBinPath, 2);
			this.lblSDeleteBinPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSDeleteBinPath.Location = new System.Drawing.Point(23, 15);
			this.lblSDeleteBinPath.Name = "lblSDeleteBinPath";
			this.lblSDeleteBinPath.Size = new System.Drawing.Size(740, 23);
			this.lblSDeleteBinPath.TabIndex = 1;
			// 
			// optSource_Dir
			// 
			this.optSource_Dir.AutoSize = true;
			this.optSource_Dir.Checked = true;
			this.optSource_Dir.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Dir.Location = new System.Drawing.Point(23, 124);
			this.optSource_Dir.Name = "optSource_Dir";
			this.optSource_Dir.Size = new System.Drawing.Size(163, 23);
			this.optSource_Dir.TabIndex = 6;
			this.optSource_Dir.TabStop = true;
			this.optSource_Dir.Text = "Folder and all its contents:";
			this.optSource_Dir.UseVisualStyleBackColor = true;
			// 
			// optSource_Files
			// 
			this.optSource_Files.AutoSize = true;
			this.optSource_Files.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Files.Location = new System.Drawing.Point(23, 153);
			this.optSource_Files.Name = "optSource_Files";
			this.optSource_Files.Size = new System.Drawing.Size(56, 23);
			this.optSource_Files.TabIndex = 8;
			this.optSource_Files.Text = "File(s)";
			this.optSource_Files.UseVisualStyleBackColor = true;
			// 
			// txtSource_Dir
			// 
			this.txtSource_Dir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Dir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.txtSource_Dir.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Dir.Location = new System.Drawing.Point(249, 124);
			this.txtSource_Dir.Name = "txtSource_Dir";
			this.txtSource_Dir.Size = new System.Drawing.Size(514, 23);
			this.txtSource_Dir.TabIndex = 7;
			// 
			// txtSource_Files
			// 
			this.txtSource_Files.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Files.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtSource_Files.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Files.Location = new System.Drawing.Point(249, 153);
			this.txtSource_Files.Name = "txtSource_Files";
			this.txtSource_Files.ReadOnly = true;
			this.txtSource_Files.Size = new System.Drawing.Size(514, 23);
			this.txtSource_Files.TabIndex = 9;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label2, 4);
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(3, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(790, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "What to clean:";
			// 
			// btnSource_DisplaySelectionUI
			// 
			this.btnSource_DisplaySelectionUI.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSource_DisplaySelectionUI.Location = new System.Drawing.Point(769, 124);
			this.btnSource_DisplaySelectionUI.Name = "btnSource_DisplaySelectionUI";
			this.tlpParams.SetRowSpan(this.btnSource_DisplaySelectionUI, 2);
			this.btnSource_DisplaySelectionUI.Size = new System.Drawing.Size(24, 52);
			this.btnSource_DisplaySelectionUI.TabIndex = 10;
			this.btnSource_DisplaySelectionUI.Text = "...";
			this.btnSource_DisplaySelectionUI.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel1, 4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 182);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(790, 4);
			this.panel1.TabIndex = 8;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel2, 4);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 41);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(790, 4);
			this.panel2.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Left;
			this.label3.Location = new System.Drawing.Point(249, 189);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 29);
			this.label3.TabIndex = 10;
			this.label3.Text = "Overwrite Passes:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numPasses
			// 
			this.numPasses.Dock = System.Windows.Forms.DockStyle.Right;
			this.numPasses.Location = new System.Drawing.Point(171, 192);
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
			this.numPasses.Size = new System.Drawing.Size(72, 23);
			this.numPasses.TabIndex = 11;
			this.numPasses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// tlpCleanModes
			// 
			this.tlpCleanModes.AutoSize = true;
			this.tlpCleanModes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpCleanModes.ColumnCount = 1;
			this.tlpParams.SetColumnSpan(this.tlpCleanModes, 3);
			this.tlpCleanModes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpCleanModes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpCleanModes.Controls.Add(this.optCleanMode_Zero, 0, 0);
			this.tlpCleanModes.Controls.Add(this.optCleanMode_Clean, 0, 1);
			this.tlpCleanModes.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpCleanModes.Location = new System.Drawing.Point(23, 236);
			this.tlpCleanModes.Name = "tlpCleanModes";
			this.tlpCleanModes.RowCount = 2;
			this.tlpCleanModes.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCleanModes.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCleanModes.Size = new System.Drawing.Size(770, 50);
			this.tlpCleanModes.TabIndex = 12;
			// 
			// optCleanMode_Zero
			// 
			this.optCleanMode_Zero.AutoSize = true;
			this.optCleanMode_Zero.Checked = true;
			this.optCleanMode_Zero.Location = new System.Drawing.Point(3, 3);
			this.optCleanMode_Zero.Name = "optCleanMode_Zero";
			this.optCleanMode_Zero.Size = new System.Drawing.Size(292, 19);
			this.optCleanMode_Zero.TabIndex = 0;
			this.optCleanMode_Zero.TabStop = true;
			this.optCleanMode_Zero.Text = "Zero free space (good for virtual disk optimization)";
			this.optCleanMode_Zero.UseVisualStyleBackColor = true;
			// 
			// optCleanMode_Clean
			// 
			this.optCleanMode_Clean.AutoSize = true;
			this.optCleanMode_Clean.Location = new System.Drawing.Point(3, 28);
			this.optCleanMode_Clean.Name = "optCleanMode_Clean";
			this.optCleanMode_Clean.Size = new System.Drawing.Size(114, 19);
			this.optCleanMode_Clean.TabIndex = 1;
			this.optCleanMode_Clean.Text = "Clean free space.";
			this.optCleanMode_Clean.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label4, 2);
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(3, 218);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(240, 15);
			this.label4.TabIndex = 19;
			this.label4.Text = "How to clean free space:";
			// 
			// optSource_PhyDisk
			// 
			this.optSource_PhyDisk.AutoSize = true;
			this.optSource_PhyDisk.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_PhyDisk.Location = new System.Drawing.Point(23, 66);
			this.optSource_PhyDisk.Name = "optSource_PhyDisk";
			this.optSource_PhyDisk.Size = new System.Drawing.Size(220, 23);
			this.optSource_PhyDisk.TabIndex = 1;
			this.optSource_PhyDisk.TabStop = true;
			this.optSource_PhyDisk.Text = "Physical Disk (without any partitions)";
			this.optSource_PhyDisk.UseVisualStyleBackColor = true;
			// 
			// cboSource_PhyDisk
			// 
			this.cboSource_PhyDisk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboSource_PhyDisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSource_PhyDisk.FormattingEnabled = true;
			this.cboSource_PhyDisk.Location = new System.Drawing.Point(249, 66);
			this.cboSource_PhyDisk.Name = "cboSource_PhyDisk";
			this.cboSource_PhyDisk.Size = new System.Drawing.Size(514, 23);
			this.cboSource_PhyDisk.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel3, 4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 292);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(790, 4);
			this.panel3.TabIndex = 22;
			// 
			// optSource_LogDisk
			// 
			this.optSource_LogDisk.AutoSize = true;
			this.optSource_LogDisk.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_LogDisk.Location = new System.Drawing.Point(23, 95);
			this.optSource_LogDisk.Name = "optSource_LogDisk";
			this.optSource_LogDisk.Size = new System.Drawing.Size(190, 23);
			this.optSource_LogDisk.TabIndex = 3;
			this.optSource_LogDisk.TabStop = true;
			this.optSource_LogDisk.Text = "Logical drive (clean free space):";
			this.optSource_LogDisk.UseVisualStyleBackColor = true;
			// 
			// cboSource_LogDisk
			// 
			this.cboSource_LogDisk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboSource_LogDisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSource_LogDisk.FormattingEnabled = true;
			this.cboSource_LogDisk.Location = new System.Drawing.Point(249, 95);
			this.cboSource_LogDisk.Name = "cboSource_LogDisk";
			this.cboSource_LogDisk.Size = new System.Drawing.Size(514, 23);
			this.cboSource_LogDisk.TabIndex = 4;
			// 
			// btnSource_Refresh
			// 
			this.btnSource_Refresh.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSource_Refresh.Location = new System.Drawing.Point(769, 66);
			this.btnSource_Refresh.Name = "btnSource_Refresh";
			this.tlpParams.SetRowSpan(this.btnSource_Refresh, 2);
			this.btnSource_Refresh.Size = new System.Drawing.Size(24, 52);
			this.btnSource_Refresh.TabIndex = 5;
			this.btnSource_Refresh.Text = "🗘";
			this.btnSource_Refresh.UseVisualStyleBackColor = true;
			this.btnSource_Refresh.Click += new System.EventHandler(this.btnSource_Refresh_Click);
			// 
			// lstLog
			// 
			this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLog.ItemHeight = 15;
			this.lstLog.Location = new System.Drawing.Point(3, 3);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(790, 190);
			this.lstLog.TabIndex = 0;
			// 
			// pbProgress
			// 
			this.pbProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbProgress.Location = new System.Drawing.Point(3, 207);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(790, 8);
			this.pbProgress.TabIndex = 25;
			// 
			// btnStartStop
			// 
			this.btnStartStop.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnStartStop.Location = new System.Drawing.Point(666, 223);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(127, 42);
			this.btnStartStop.TabIndex = 1;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			// 
			// tlpDown
			// 
			this.tlpDown.ColumnCount = 1;
			this.tlpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.Controls.Add(this.pbProgress, 0, 2);
			this.tlpDown.Controls.Add(this.btnStartStop, 0, 3);
			this.tlpDown.Controls.Add(this.lstLog, 0, 0);
			this.tlpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpDown.Location = new System.Drawing.Point(16, 315);
			this.tlpDown.Name = "tlpDown";
			this.tlpDown.RowCount = 4;
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tlpDown.Size = new System.Drawing.Size(796, 268);
			this.tlpDown.TabIndex = 1;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(828, 599);
			this.Controls.Add(this.tlpDown);
			this.Controls.Add(this.tlpParams);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 500);
			this.Name = "frmMain";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmMain";
			this.tlpParams.ResumeLayout(false);
			this.tlpParams.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPasses)).EndInit();
			this.tlpCleanModes.ResumeLayout(false);
			this.tlpCleanModes.PerformLayout();
			this.tlpDown.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpParams;
		private System.Windows.Forms.Label label1;
		private Label lblSDeleteBinPath;
		private System.Windows.Forms.RadioButton optSource_Dir;
		private System.Windows.Forms.RadioButton optSource_Files;
		private System.Windows.Forms.TextBox txtSource_Dir;
		private System.Windows.Forms.TextBox txtSource_Files;
		private Label label2;
		private Button btnSource_DisplaySelectionUI;
		private Panel panel1;
		private Panel panel2;
		private Label label3;
		private NumericUpDown numPasses;
		private RadioButton optCleanMode_Zero;
		private RadioButton optCleanMode_Clean;
		private TableLayoutPanel tlpCleanModes;
		private Label label4;
		private RadioButton optSource_PhyDisk;
		private ComboBox cboSource_PhyDisk;
		private Button btnStartStop;
		private ListBox lstLog;
		private ProgressBar pbProgress;
		private TableLayoutPanel tlpDown;
		private RadioButton optSource_LogDisk;
		private ComboBox cboSource_LogDisk;
		private Button btnSource_Refresh;
		private Panel panel3;
	}
}