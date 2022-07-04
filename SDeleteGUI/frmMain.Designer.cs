﻿namespace SDeleteGUI
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
			this.tlpParams = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSDeleteBinPath = new System.Windows.Forms.TextBox();
			this.optSource_Dir = new System.Windows.Forms.RadioButton();
			this.optSource_Files = new System.Windows.Forms.RadioButton();
			this.txtSource_Dir = new System.Windows.Forms.TextBox();
			this.txtSource_Files = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSourceSelect = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.numPasses = new System.Windows.Forms.NumericUpDown();
			this.tlpCleanModes = new System.Windows.Forms.TableLayoutPanel();
			this.optCleanMode_Zero = new System.Windows.Forms.RadioButton();
			this.optCleanMode_Clean = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.optSource_Disk = new System.Windows.Forms.RadioButton();
			this.cboSource_Disk = new System.Windows.Forms.ComboBox();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.tlpDown = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
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
			this.tlpParams.Controls.Add(this.txtSDeleteBinPath, 2, 0);
			this.tlpParams.Controls.Add(this.optSource_Dir, 1, 4);
			this.tlpParams.Controls.Add(this.optSource_Files, 1, 5);
			this.tlpParams.Controls.Add(this.txtSource_Dir, 2, 4);
			this.tlpParams.Controls.Add(this.txtSource_Files, 2, 5);
			this.tlpParams.Controls.Add(this.label2, 0, 2);
			this.tlpParams.Controls.Add(this.btnSourceSelect, 3, 3);
			this.tlpParams.Controls.Add(this.panel1, 0, 6);
			this.tlpParams.Controls.Add(this.panel2, 0, 1);
			this.tlpParams.Controls.Add(this.label3, 1, 9);
			this.tlpParams.Controls.Add(this.numPasses, 2, 9);
			this.tlpParams.Controls.Add(this.tlpCleanModes, 1, 8);
			this.tlpParams.Controls.Add(this.label4, 0, 6);
			this.tlpParams.Controls.Add(this.optSource_Disk, 1, 3);
			this.tlpParams.Controls.Add(this.cboSource_Disk, 2, 3);
			this.tlpParams.Controls.Add(this.panel3, 0, 10);
			this.tlpParams.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpParams.Location = new System.Drawing.Point(16, 16);
			this.tlpParams.Name = "tlpParams";
			this.tlpParams.RowCount = 12;
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
			this.tlpParams.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpParams.Size = new System.Drawing.Size(727, 281);
			this.tlpParams.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label1, 2);
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "SDelete.exe location:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtSDeleteBinPath
			// 
			this.tlpParams.SetColumnSpan(this.txtSDeleteBinPath, 2);
			this.txtSDeleteBinPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSDeleteBinPath.Location = new System.Drawing.Point(127, 3);
			this.txtSDeleteBinPath.Name = "txtSDeleteBinPath";
			this.txtSDeleteBinPath.Size = new System.Drawing.Size(597, 23);
			this.txtSDeleteBinPath.TabIndex = 1;
			// 
			// optSource_Dir
			// 
			this.optSource_Dir.AutoSize = true;
			this.optSource_Dir.Checked = true;
			this.optSource_Dir.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Dir.Location = new System.Drawing.Point(23, 86);
			this.optSource_Dir.Name = "optSource_Dir";
			this.optSource_Dir.Size = new System.Drawing.Size(73, 23);
			this.optSource_Dir.TabIndex = 2;
			this.optSource_Dir.TabStop = true;
			this.optSource_Dir.Text = "Directory";
			this.optSource_Dir.UseVisualStyleBackColor = true;
			// 
			// optSource_Files
			// 
			this.optSource_Files.AutoSize = true;
			this.optSource_Files.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Files.Location = new System.Drawing.Point(23, 115);
			this.optSource_Files.Name = "optSource_Files";
			this.optSource_Files.Size = new System.Drawing.Size(56, 23);
			this.optSource_Files.TabIndex = 3;
			this.optSource_Files.Text = "File(s)";
			this.optSource_Files.UseVisualStyleBackColor = true;
			// 
			// txtSource_Dir
			// 
			this.txtSource_Dir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Dir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.txtSource_Dir.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Dir.Location = new System.Drawing.Point(127, 86);
			this.txtSource_Dir.Name = "txtSource_Dir";
			this.txtSource_Dir.Size = new System.Drawing.Size(567, 23);
			this.txtSource_Dir.TabIndex = 4;
			// 
			// txtSource_Files
			// 
			this.txtSource_Files.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txtSource_Files.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtSource_Files.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSource_Files.Location = new System.Drawing.Point(127, 115);
			this.txtSource_Files.Name = "txtSource_Files";
			this.txtSource_Files.Size = new System.Drawing.Size(567, 23);
			this.txtSource_Files.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label2, 4);
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(3, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(721, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "What to clean:";
			// 
			// btnSourceSelect
			// 
			this.btnSourceSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSourceSelect.Location = new System.Drawing.Point(700, 57);
			this.btnSourceSelect.Name = "btnSourceSelect";
			this.tlpParams.SetRowSpan(this.btnSourceSelect, 3);
			this.btnSourceSelect.Size = new System.Drawing.Size(24, 81);
			this.btnSourceSelect.TabIndex = 7;
			this.btnSourceSelect.Text = "...";
			this.btnSourceSelect.UseVisualStyleBackColor = true;
			this.btnSourceSelect.Click += new System.EventHandler(this.btnSourceSelect_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel1, 4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 159);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(721, 4);
			this.panel1.TabIndex = 8;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel2, 4);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 32);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(721, 4);
			this.panel2.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Right;
			this.label3.Location = new System.Drawing.Point(23, 222);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 29);
			this.label3.TabIndex = 10;
			this.label3.Text = "Overwrite Passes:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numPasses
			// 
			this.numPasses.Dock = System.Windows.Forms.DockStyle.Left;
			this.numPasses.Location = new System.Drawing.Point(127, 225);
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
			this.tlpCleanModes.Location = new System.Drawing.Point(23, 169);
			this.tlpCleanModes.Name = "tlpCleanModes";
			this.tlpCleanModes.RowCount = 2;
			this.tlpCleanModes.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCleanModes.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpCleanModes.Size = new System.Drawing.Size(701, 50);
			this.tlpCleanModes.TabIndex = 18;
			// 
			// optCleanMode_Zero
			// 
			this.optCleanMode_Zero.AutoSize = true;
			this.optCleanMode_Zero.Checked = true;
			this.optCleanMode_Zero.Location = new System.Drawing.Point(3, 3);
			this.optCleanMode_Zero.Name = "optCleanMode_Zero";
			this.optCleanMode_Zero.Size = new System.Drawing.Size(292, 19);
			this.optCleanMode_Zero.TabIndex = 14;
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
			this.optCleanMode_Clean.TabIndex = 15;
			this.optCleanMode_Clean.Text = "Clean free space.";
			this.optCleanMode_Clean.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.tlpParams.SetColumnSpan(this.label4, 4);
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(3, 141);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(721, 15);
			this.label4.TabIndex = 19;
			this.label4.Text = "How to clean:";
			// 
			// optSource_Disk
			// 
			this.optSource_Disk.AutoSize = true;
			this.optSource_Disk.Dock = System.Windows.Forms.DockStyle.Left;
			this.optSource_Disk.Location = new System.Drawing.Point(23, 57);
			this.optSource_Disk.Name = "optSource_Disk";
			this.optSource_Disk.Size = new System.Drawing.Size(47, 23);
			this.optSource_Disk.TabIndex = 20;
			this.optSource_Disk.TabStop = true;
			this.optSource_Disk.Text = "Disk";
			this.optSource_Disk.UseVisualStyleBackColor = true;
			// 
			// cboSource_Disk
			// 
			this.cboSource_Disk.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboSource_Disk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSource_Disk.FormattingEnabled = true;
			this.cboSource_Disk.Location = new System.Drawing.Point(127, 57);
			this.cboSource_Disk.Name = "cboSource_Disk";
			this.cboSource_Disk.Size = new System.Drawing.Size(567, 23);
			this.cboSource_Disk.TabIndex = 21;
			// 
			// txtOutput
			// 
			this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOutput.Location = new System.Drawing.Point(3, 3);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOutput.Size = new System.Drawing.Size(721, 177);
			this.txtOutput.TabIndex = 24;
			// 
			// pbProgress
			// 
			this.pbProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbProgress.Location = new System.Drawing.Point(3, 194);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(721, 8);
			this.pbProgress.TabIndex = 25;
			// 
			// btnStartStop
			// 
			this.btnStartStop.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnStartStop.Location = new System.Drawing.Point(597, 210);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(127, 42);
			this.btnStartStop.TabIndex = 22;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// tlpDown
			// 
			this.tlpDown.ColumnCount = 1;
			this.tlpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.Controls.Add(this.pbProgress, 0, 2);
			this.tlpDown.Controls.Add(this.btnStartStop, 0, 3);
			this.tlpDown.Controls.Add(this.txtOutput, 0, 0);
			this.tlpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpDown.Location = new System.Drawing.Point(16, 297);
			this.tlpDown.Name = "tlpDown";
			this.tlpDown.RowCount = 4;
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.tlpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tlpDown.Size = new System.Drawing.Size(727, 255);
			this.tlpDown.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.tlpParams.SetColumnSpan(this.panel3, 4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 254);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(721, 4);
			this.panel3.TabIndex = 22;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(759, 568);
			this.Controls.Add(this.tlpDown);
			this.Controls.Add(this.tlpParams);
			this.Name = "frmMain";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.Text = "frmMain";
			this.tlpParams.ResumeLayout(false);
			this.tlpParams.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPasses)).EndInit();
			this.tlpCleanModes.ResumeLayout(false);
			this.tlpCleanModes.PerformLayout();
			this.tlpDown.ResumeLayout(false);
			this.tlpDown.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpParams;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSDeleteBinPath;
		private System.Windows.Forms.RadioButton optSource_Dir;
		private System.Windows.Forms.RadioButton optSource_Files;
		private System.Windows.Forms.TextBox txtSource_Dir;
		private System.Windows.Forms.TextBox txtSource_Files;
		private Label label2;
		private Button btnSourceSelect;
		private Panel panel1;
		private Panel panel2;
		private Label label3;
		private NumericUpDown numPasses;
		private RadioButton optCleanMode_Zero;
		private RadioButton optCleanMode_Clean;
		private TableLayoutPanel tlpCleanModes;
		private Label label4;
		private RadioButton optSource_Disk;
		private ComboBox cboSource_Disk;
		private Button btnStartStop;
		private TextBox txtOutput;
		private ProgressBar pbProgress;
		private TableLayoutPanel tlpDown;
		private Panel panel3;
	}
}