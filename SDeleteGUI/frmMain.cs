
using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;

using SDeleteGUI.Core.SDelete;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static SDeleteGUI.Core.SDelete.SDeleteManager;

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{
		private enum SourceModes : int
		{
			Unknown = 0,
			PhyDisk,
			LogDisk,
			Dir,
			Files
		}

		private SourceModes _SourceMode = SourceModes.Unknown;
		private static readonly Exception _exSourceError = new("Unknown Source selected!");

		private SDeleteManager? _sdmgr = null;
		private bool _isRunning = false;
		private Lazy<TaskbarManager> _tbm = new(TaskbarManager.Instance);


		public frmMain()
		{
			InitializeComponent();

			Text = $"{Application.ProductName} {Application.ProductVersion}";

			this.Load += async (_, _) => await _Load();
			this.FormClosing += (s, e) => { if (e.CloseReason == CloseReason.UserClosing) _sdmgr!.Stop(); };

			optSource_PhyDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_LogDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Dir.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Files.CheckedChanged += (_, _) => OnSourceChanged();

			btnSource_Refresh.Click += async (_, _) => await OnSource_RefreshLists();
			btnSource_DisplaySelectionUI.Click += (_, _) => OnSource_DisplaySelectionUI();

			txtSource_Dir.TextChanged += (_, _) => UpdateUI();
			txtSource_Files.TextChanged += (_, _) => UpdateUI();

			btnStartStop.Click += (_, _) => OnStartStop();
		}


		private async Task _Load()
		{

			Func<FileInfo> cbAskUserForBinary = new(() =>
			{
				//if (ofd.ShowDialog() != DialogResult.OK) throw new NotImplementedException();

				string sMsg = @$"Sysinternals '{SDeleteManager.C_SDBIN_FILE64}' or '{SDeleteManager.C_SDBIN_FILE}' was not found in well-known locations!

Do you want to specify it manualy ?";
				if (MessageBox.Show(
					sMsg,
					Application.ProductName,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
					) != DialogResult.Yes)
					throw new NotImplementedException();

				using OpenFileDialog ofd = new()
				{
					AddExtension = true,
					AutoUpgradeEnabled = true,
					CheckFileExists = true,
					CheckPathExists = true,
					DereferenceLinks = true,
					Filter = $"{SDeleteManager.C_SDBIN_FILE64}|{SDeleteManager.C_SDBIN_FILE64}"
				};
				if (ofd.ShowDialog() != DialogResult.OK) throw new NotImplementedException();
				return new(ofd.FileName);
			});

			try
			{
				_sdmgr = new SDeleteManager(cbAskUserForBinary);
			}

			catch (Exception ex)
			{
				if (ex is not NotImplementedException)//Not user canceled error
					ex.FIX_ERROR(true);

				Application.Exit();
				return;
			}

			try
			{
				lblSDeleteBinPath.Text = _sdmgr!.SDeleteBinary.FullName;
				//txtSource_Dir.Text = @"E:\_222 — копия";


				optSource_PhyDisk.Checked = true;
				//OnSourceChanged();


				await FillDisksList();

				_tbm.Value.SetProgressState(TaskbarProgressBarState.NoProgress);

			}
			catch (Exception ex)
			{
				lblSDeleteBinPath.Text = ex.Message;
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
			cboSource_PhyDisk.DisabeAndShowBanner("Loading disk list...");
			optSource_PhyDisk.Enabled = false;

			cboSource_LogDisk.DisabeAndShowBanner("Loading disk list...");
			optSource_LogDisk.Enabled = false;

			try
			{
				string[] logDisks = Directory.GetLogicalDrives();
				optSource_LogDisk.Enabled = logDisks.Any();
				cboSource_LogDisk.FillAndSelectLast(logDisks, _SourceMode == SourceModes.LogDisk);
			}
			catch (Exception ex)
			{
				cboSource_LogDisk.DisabeAndShowError(ex);
			}


			try
			{
				WmiDisk[] wd = await WmiDisk.GetDisksAsync();
				optSource_PhyDisk.Enabled = wd.Any();
				cboSource_PhyDisk.FillAndSelectLast(wd, _SourceMode == SourceModes.PhyDisk);
			}
			catch (Exception ex)
			{
				cboSource_PhyDisk.DisabeAndShowError(ex);
			}
		}


		private void OnSourceChanged()
		{
			try
			{
				_SourceMode = SourceModes.Unknown;
				if (optSource_PhyDisk.Checked) _SourceMode = SourceModes.PhyDisk;
				else if (optSource_LogDisk.Checked) _SourceMode = SourceModes.LogDisk;
				else if (optSource_Dir.Checked) _SourceMode = SourceModes.Dir;
				else if (optSource_Files.Checked) _SourceMode = SourceModes.Files;
				else throw _exSourceError;

				cboSource_PhyDisk.Enabled = _SourceMode == SourceModes.PhyDisk;
				cboSource_LogDisk.Enabled = _SourceMode == SourceModes.LogDisk;

				txtSource_Dir.Enabled = _SourceMode == SourceModes.Dir;
				txtSource_Files.Enabled = _SourceMode == SourceModes.Files;


				bool isSourceDisk = _SourceMode == SourceModes.PhyDisk || _SourceMode == SourceModes.LogDisk;
				tlpCleanModes.Enabled = isSourceDisk;
				btnSource_DisplaySelectionUI.Enabled = !isSourceDisk;
			}
			finally
			{
				UpdateUI();
			}
		}


		private async Task OnSource_RefreshLists()
		{
			try
			{
				await FillDisksList();
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

		private void OnSource_DisplaySelectionUI()
		{
			try
			{
				switch (_SourceMode)
				{
					case SourceModes.Dir:
						{
							using FolderBrowserDialog ofd = new()
							{
								AutoUpgradeEnabled = true,
								RootFolder = Environment.SpecialFolder.MyComputer,
								InitialDirectory = txtSource_Dir.Text.Trim()
							};

							var dr = ofd.ShowDialog(this);
							if (dr != DialogResult.OK) return;

							txtSource_Dir.Text = ofd.SelectedPath;
						}
						break;

					case SourceModes.Files:
						{
							using OpenFileDialog ofd = new()
							{
								AutoUpgradeEnabled = true,
								AddExtension = true,
								CheckFileExists = true,
								CheckPathExists = true,
								DereferenceLinks = true,
								Filter = "Any Files|*.*",
								Multiselect = true,
								ShowHelp = false,
								ShowReadOnly = false,
								SupportMultiDottedExtensions = true,
								ValidateNames = true
							};

							var dr = ofd.ShowDialog(this);
							if (dr != DialogResult.OK) return;
							FileInfo[] files = ofd
							   .FileNames
							   .Select(s => new FileInfo(s))
							   .ToArray();

							txtSource_Files.Tag = files;
							txtSource_Files.Text = string.Join(",", files.Select(fi => fi.FullName).ToArray());
						}
						break;

					default:
						throw _exSourceError;
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

		/// <summary>Update button state by UI selections</summary>
		private void UpdateUI()
		{
			bool bCanRun = false;
			try
			{
				switch (_SourceMode)
				{
					case SourceModes.PhyDisk:
						bCanRun = cboSource_PhyDisk.Items.Count > 0;
						break;

					case SourceModes.LogDisk:
						bCanRun = cboSource_LogDisk.Items.Count > 0;
						break;

					case SourceModes.Dir:
						{
							DirectoryInfo di = new(txtSource_Dir.Text.Trim());
							bCanRun = di.Exists;
						}
						break;
					case SourceModes.Files:
						{
							if (null != txtSource_Files.Tag)
							{
								FileInfo[] files = (FileInfo[])txtSource_Files.Tag;
								//bCanRun = files.Any();
								if (files.Any())
								{
									var existFiles = files.Where(fi => { fi.Refresh(); return fi.Exists; }).ToArray();
									bCanRun = files.Length == existFiles.Length;
								}
							}
						}
						break;

					default:
						break;
				}
			}
			catch (Exception ex)
			{
				//ex.FIX_ERROR(true);
			}
			finally
			{
				btnStartStop.Enabled = bCanRun;
			}
		}


		private void OnStartStop()
		{
			if (_isRunning)
			{
				_sdmgr!.Stop();
			}
			else
			{
				_isRunning = true;
				tlpParams.Enabled = false;
				lstLog.Items.Clear();
				btnStartStop.Text = "Stop";

				ProgressBarSetState_Marquee();

				SDeleteEngine_AttachEvents();
				try
				{

					uint passes = (uint)numPasses.Value;
					CleanModes cm = optCleanMode_Zero.Checked
									? CleanModes.Zero
									: CleanModes.Clean;

					switch (_SourceMode)
					{
						case SourceModes.PhyDisk:
							{
								if (cboSource_PhyDisk.Items.Count < 1) throw _exSourceError;
								_sdmgr!.Run(passes, (WmiDisk)cboSource_PhyDisk.SelectedItem, cm);
							}
							break;

						case SourceModes.LogDisk:
							{
								if (cboSource_LogDisk.Items.Count < 1) throw _exSourceError;
								string sLogDisk = (string)cboSource_LogDisk.SelectedItem;
								_sdmgr!.Run(passes, sLogDisk[0], cm);
							}
							break;

						case SourceModes.Dir:
							_sdmgr!.Run(passes, new DirectoryInfo(txtSource_Dir.Text.Trim()));
							break;

						case SourceModes.Files:
							{
								if (null == txtSource_Files.Tag) throw _exSourceError;
								FileInfo[] files = (FileInfo[])txtSource_Files.Tag;
								if (!files.Any()) throw _exSourceError;
								_sdmgr!.Run(passes, files);
							}
							break;

						default:
							throw _exSourceError;
					}
				}
				catch (Exception ex)
				{
					ProgressBarSetState_Error();
					ex.FIX_ERROR(true);
					OnFinished();
				}
			}
		}

		private void OnFinished()
		{
			SDeleteEngine_DetachEvents();

			_isRunning = false;
			tlpParams.Enabled = true;
			btnStartStop.Text = "Start";
			ProgressBarSetState_Finished();
			UpdateUI();
		}




		private void SDeleteEngine_AttachEvents()
		{
			_sdmgr!.OutputRAW += OnCore_Data_RAW;
			_sdmgr!.OutputProgress += OnCore_Data_Progress;
			_sdmgr!.Error += OnCore_Error;
			_sdmgr!.Finished += (_, _) => this.e_runInUIThread(() => OnFinished());
		}
		private void SDeleteEngine_DetachEvents()
		{
			_sdmgr!.OutputRAW -= OnCore_Data_RAW;
			_sdmgr!.OutputProgress -= OnCore_Data_Progress!;
			_sdmgr!.Error -= OnCore_Error;
			_sdmgr!.Finished -= (_, _) => this.e_runInUIThread(() => OnFinished());
		}


		private void OnCore_Data_RAW(object sender, DataReceivedEventArgs e)
		{

			if (e.Data == null) return;
			string s = (e.Data ?? string.Empty).Trim();
			if (s.e_IsNullOrEmpty()) return;

			Debug.WriteLine($"Output Data: '{s}'");
			LogAddRow(s);
		}
		private void LogAddRow(string s)
		{
			//lstLog.e_runInUIThread_AppendLine(s, 1000);
			const int C_MAX_LOG_ROWS = 1000;

			this.e_runInUIThread(() =>
			{
				lstLog.BeginUpdate();
				try
				{
					while (lstLog.Items.Count >= C_MAX_LOG_ROWS)
					{
						lstLog.Items.RemoveAt(0);
					}
					lstLog.Items.Add(s);
					lstLog.SelectedIndex = (lstLog.Items.Count - 1);
				}
				finally { lstLog.EndUpdate(); }
			});
		}

		private void OnCore_Data_Progress(object sender, ProgressInfo e)
			=> this.e_runInUIThread(() => OnCore_Data_Progress(e));

		private void OnCore_Data_Progress(ProgressInfo e)
		{
			//Debug.WriteLine("Progress data detected! " + e.ToString());
			ProgressBarSetState_Progress(e.ProgressPercent);
		}


		private void OnCore_Error(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null) return;
			string s = e.Data ?? string.Empty;
			if (s.e_IsNullOrEmpty()) return;

			Debug.WriteLine($"Error Data: '{s}'");
			//lstLog.e_runInUIThread_AppendLine("ERROR!: " + (s), 1000);
			LogAddRow("ERROR!: " + (s));
		}





		private void ProgressBarSetState_Marquee()
		{
			pbProgress.e_SetValues();
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
			pbProgress.Style = ProgressBarStyle.Marquee;

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Indeterminate);
		}
		private void ProgressBarSetState_Progress(float progress)
		{
			int iProgress = (int)progress;

			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
			pbProgress.e_SetValues(0, 100, iProgress);
			pbProgress.Style = ProgressBarStyle.Continuous;

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Normal);
			_tbm.Value.SetProgressValue(iProgress, 100);
		}
		private void ProgressBarSetState_Error()
		{
			pbProgress.Style = ProgressBarStyle.Blocks;
			pbProgress.e_SetValues(0, 100, 100);
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_ERROR);
			_tbm.Value.SetProgressState(TaskbarProgressBarState.Error);
		}
		private void ProgressBarSetState_Finished()
		{
			pbProgress.Style = ProgressBarStyle.Blocks;
			pbProgress.e_SetValues(0, 100, 100);
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Normal);
			_tbm.Value.SetProgressValue(100, 100);
		}

		private void btnSource_Refresh_Click(object sender, EventArgs e)
		{

		}
	}
}
