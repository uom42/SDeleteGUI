
using System.Management;
using System.Text;

using SDeleteGUI.Core;

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		public frmMain()
		{
			InitializeComponent();

			this.Load += async (_, _) => await _Load();
		}

		SDeleteManager? sdmgr = null;

		private async Task _Load()
		{
			try
			{
				sdmgr = new SDeleteManager();


				this.FormClosing += (s, e) =>
				{
					if (e.CloseReason != CloseReason.UserClosing) return;
					sdmgr.Stop();
				};


				txtSDeleteBinPath.Text = sdmgr.SDeleteBinary.FullName;

				optSource_Disk.Checked = true;


				txtSource_Dir.Text = @"E:\_222 — копия";

				await FillDisksList();



				DataReceivedEventHandler outputDataReceived = new((_, e) =>
				{
					if (e.Data == null) return;
					string s = e.Data ?? string.Empty;
					if (s.e_IsNullOrEmpty()) return;

					Debug.WriteLine($"Output Data: '{s}'");
					txtOutput.e_runInUIThread_AppendLine(s, 1000);
				});

				DataReceivedEventHandler errorDataReceived = new((_, e) =>
				{
					if (e.Data == null) return;
					string s = e.Data ?? string.Empty;
					if (s.e_IsNullOrEmpty()) return;

					Debug.WriteLine($"Error Data: '{s}'");
					txtOutput.e_runInUIThread_AppendLine("ERROR!: " + (s), 1000);
				});


				sdmgr.Output += outputDataReceived;
				sdmgr.Error += errorDataReceived;
				sdmgr.Finished += (_, _) => this.e_runInUIThread(() => OnFinished());


			}
			catch (Exception ex)
			{
				txtSDeleteBinPath.Text = ex.Message;
				return;
			}
			finally
			{
				UpdateUI();
			}
		}


		/// <summary>Load Physical disk list from WMI</summary>
		private async Task FillDisksList()
		{
			cboSource_Disk.Items.Clear();
			optSource_Disk.Enabled = false;
			cboSource_Disk.Enabled = false;

			WmiDisk[] wd = await WmiDisk.GetDisksAsync();

			optSource_Disk.Enabled = wd.Any();
			cboSource_Disk.Enabled = wd.Any();
			if (wd.Any())
			{
				cboSource_Disk.Items.AddRange(wd);
				cboSource_Disk.SelectedItem = wd.Last();
			}
		}

		private void UpdateUI()
		{
		}

		private async void btnSourceSelect_Click(object sender, EventArgs e)
		{
			try
			{
				if (optSource_Disk.Checked)
				{
					await FillDisksList();
				}
				else if (optSource_Dir.Checked)
				{
					using var ofd = new FolderBrowserDialog()
					{
						AutoUpgradeEnabled = true,
						RootFolder = Environment.SpecialFolder.MyComputer,
						InitialDirectory = txtSource_Dir.Text.Trim()
					};

					var dr = ofd.ShowDialog(this);
					if (dr != DialogResult.OK) return;

					txtSource_Dir.Text = ofd.SelectedPath;
				}
				else if (optSource_Files.Checked)
				{
				}
				else
				{
					throw new ArgumentOutOfRangeException("Unknown source selected!");
				}
			}
			catch (Exception ex)
			{
				ex.FIX_ERROR(true);
			}
			finally
			{
				UpdateUI();
			}
		}

		private bool _isRunning = false;
		private void btnStartStop_Click(object sender, EventArgs e)
		{
			if (_isRunning)
			{
				sdmgr!.Stop();
			}
			else
			{
				_isRunning = true;
				tlpParams.Enabled = false;
				txtOutput.Clear();
				btnStartStop.Text = "Stop";
				pbProgress.e_SetValues();
				pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
				pbProgress.Style = ProgressBarStyle.Marquee;
				try
				{

					uint passes = (uint)numPasses.Value;

					if (optSource_Disk.Checked)
					{
						if (cboSource_Disk.Items.Count < 1) throw new Exception("Disk to clean not selected!");
						sdmgr!.Run(passes, (WmiDisk)cboSource_Disk.SelectedItem);
					}
					else if (optSource_Dir.Checked)
					{
						sdmgr!.Run(passes, new DirectoryInfo(txtSource_Dir.Text.Trim()));
					}
					else if (optSource_Files.Checked)
					{
					}
					else
					{
						throw new ArgumentOutOfRangeException("Unknown source selected!");
					}
				}
				catch (Exception ex)
				{
					pbProgress.Style = ProgressBarStyle.Blocks;
					pbProgress.e_SetValues(0, 100, 100);
					pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_ERROR);
					ex.FIX_ERROR(true);
					OnFinished();
				}
			}
		}

		private void OnFinished()
		{
			_isRunning = false;
			tlpParams.Enabled = true;
			btnStartStop.Text = "Start";
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
			pbProgress.e_SetValues(0, 100, 100);
			pbProgress.Style = ProgressBarStyle.Blocks;
			UpdateUI();
		}
	}
}
