
using Microsoft.WindowsAPICodePack.Taskbar;

using NLog;

using SDeleteGUI.Core;
using SDeleteGUI.Core.SDelete;

using static System.Windows.Forms.LinkLabel;
using static SDeleteGUI.Core.SDelete.SDeleteManager;

#nullable enable

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private const string C_LOADING_DISKS_LIST = "Loading disk list...";
		private const string C_ASK_USER = @$"Sysinternals '{SDeleteManager.C_SDBIN_FILE64}' or '{SDeleteManager.C_SDBIN_FILE}' was not found in well-known locations!

Do you want to specify it manualy ?";

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

		private Lazy<Logger> _logger = new(() => LogManager.GetCurrentClassLogger());

		public frmMain()
		{
			InitializeComponent();

			Text = $"{Application.ProductName} {Application.ProductVersion}";

			this.Load += async (_, _) => await _Load();
			this.Shown += (_, _) => _Shown();
			this.FormClosing += (s, e) =>
			{
				_logger.Value.Debug($"FormClosing, CloseReason = {e.CloseReason}");
				if (e.CloseReason == CloseReason.UserClosing && _isRunning)
				{
					string msg = "The cleanup operation is still in progress!\n\nCancel cleaning?";
					if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						e.Cancel = true;
						return;
					}
					_sdmgr!.Stop();
				}
			};


			lblSDeleteBinPath.LinkClicked += OnSDBinary_LinkClicked;

			optSource_PhyDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_LogDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Dir.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Files.CheckedChanged += (_, _) => OnSourceChanged();

			btnSource_Refresh.Click += async (_, _) => await OnSource_RefreshLists();
			btnSource_DisplaySelectionUI.Click += (_, _) => OnSource_DisplaySelectionUI();


			cboSource_PhyDisk.SelectedIndexChanged += (_, _) => UpdateUI();
			txtSource_Dir.TextChanged += (_, _) => UpdateUI();
			txtSource_Files.TextChanged += (_, _) => UpdateUI();

			btnStartStop.Click += (_, _) => OnStartStop();

			tmrElapsed.Tick += (_, _) => OnElapsedTimerTick();
		}


		private async Task _Load()
		{
			_logger.Value.Debug("_Load");

			Func<FileInfo> cbAskUserForBinary = new(() =>
			{
				_logger.Value.Debug(C_ASK_USER);

				if (MessageBox.Show(
					C_ASK_USER,
					Application.ProductName,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
					) != DialogResult.Yes)
					throw new NotImplementedException();

				_logger.Value.Debug("Yes!");

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
				_logger.Value.Debug($"OpenFileDialog.FileName = '{ofd.FileName}'");
				return new(ofd.FileName);
			});
			try
			{
				_sdmgr = new SDeleteManager(cbAskUserForBinary);
			}

			catch (Exception ex)
			{
				_logger.Value.Error($"_Load", ex);

				if (ex is not NotImplementedException)//Not user canceled error
					ex.FIX_ERROR(true);

				Application.Exit();
				return;
			}

			try
			{
				const string C_SDELETE = "SDelete";
				const string C_PREFIX = C_SDELETE + " binary location: ";
				string txt = C_PREFIX + _sdmgr!.SDeleteBinary.FullName;
				lblSDeleteBinPath.Links.Clear();
				lblSDeleteBinPath.Text = txt;
				lblSDeleteBinPath.Links.Add(0, C_SDELETE.Length);
				lblSDeleteBinPath.Links.Add(C_PREFIX.Length, txt.Length - C_PREFIX.Length);

				optSource_PhyDisk.Checked = true;
				optCleanMode_Clean.Checked = true;

				await FillDisksList();

				ShellRegister(true);

				ProcessCMDLine();
			}
			catch (Exception ex)
			{
				_logger.Value.Error($"_Load2", ex);
				lblSDeleteBinPath.Text = ex.Message;
				return;
			}
			finally { UpdateUI(); }
		}

		private void _Shown()
		{
			_tbm.Value.SetProgressState(TaskbarProgressBarState.NoProgress);
		}


		/// <summary>Load disk list</summary>
		private async Task FillDisksList()
		{
			_logger.Value.Debug($"FillDisksList");

			await Task.WhenAll(
				FillDisks_Log(optSource_LogDisk, cboSource_LogDisk, _SourceMode, _logger.Value)
				,
				FillDisks_Phy(optSource_PhyDisk, cboSource_PhyDisk, _SourceMode, _logger.Value)
				);
		}
		/// <summary>Load Physical disk list from WMI</summary>
		private static async Task FillDisks_Log(
			RadioButton opt,
			ComboBox cbo,
			SourceModes cm,
			Logger log)
		{
			log.Debug("FillDisks_Log");

			LogDisk? old = null;
			cbo.e_runInUIThread(() =>
			{
				opt.Enabled = false;

				old = ((cbo.SelectedItem != null) && (cbo.SelectedItem is LogDisk selectedDisk)) ? selectedDisk : null;
				cbo.DisabeAndShowBanner(C_LOADING_DISKS_LIST);
				cbo.Enabled = false;
			});

			try
			{
				LogDisk[] disks = await LogDisk.GetDisksAsync(); log.DebugArray(disks, "LogDisk.GetDisksAsync");
				LogDisk? newSelection = (old == null)
					? disks.LastOrDefault()
					: (disks.Where(d => d.DiskLetter == old.DiskLetter).FirstOrDefault()) ?? disks.LastOrDefault();

				cbo.e_runInUIThread(() =>
				{
					opt.Enabled = disks.Any();
					cbo.FillAndSelectLast(disks, cm == SourceModes.LogDisk, false);
					if (newSelection != null) cbo.SelectedItem = newSelection!;
				});
			}
			catch (Exception ex)
			{
				log.Error(ex);
				cbo.e_runInUIThread(() => cbo.DisabeAndShowError(ex));
			}
		}
		/// <summary>Load Physical disk list from WMI</summary>
		private static async Task FillDisks_Phy(
			RadioButton opt,
			ComboBox cbo,
			SourceModes cm,
			Logger log)
		{
			log.Debug("FillDisks_Phy");

			WmiDisk? old = null;
			cbo.e_runInUIThread(() =>
			{
				opt.Enabled = false;

				old = ((cbo.SelectedItem != null) && (cbo.SelectedItem is WmiDisk selectedDisk)) ? selectedDisk : null;
				cbo.DisabeAndShowBanner(C_LOADING_DISKS_LIST);
				cbo.Enabled = false;
			});

			try
			{
				WmiDisk[] disks = await WmiDisk.GetDisksAsync(); log.DebugArray(disks, "WmiDisk.GetDisksAsync");
				WmiDisk? newSelection = (old == null)
					? disks.LastOrDefault()
					: (disks.Where(d => d.Index == old.Index).FirstOrDefault()) ?? disks.LastOrDefault();

				cbo.e_runInUIThread(() =>
				{
					opt.Enabled = disks.Any();
					cbo.FillAndSelectLast(disks, cm == SourceModes.PhyDisk, false);
					if (newSelection != null) cbo.SelectedItem = newSelection!;
				});

			}
			catch (Exception ex)
			{
				log.Error(ex);
				cbo.e_runInUIThread(() => cbo.DisabeAndShowError(ex));
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

				_logger.Value.Debug($"OnSourceChanged, _SourceMode = {_SourceMode}");

				cboSource_PhyDisk.Enabled = _SourceMode == SourceModes.PhyDisk;
				cboSource_LogDisk.Enabled = _SourceMode == SourceModes.LogDisk;

				txtSource_Dir.Enabled = _SourceMode == SourceModes.Dir;
				txtSource_Files.Enabled = _SourceMode == SourceModes.Files;


				bool isSourceDisk = _SourceMode == SourceModes.PhyDisk || _SourceMode == SourceModes.LogDisk;
				tlpCleanFreeSpaceMethods.Enabled = isSourceDisk;
				btnSource_DisplaySelectionUI.Enabled = !isSourceDisk;
			}
			finally
			{
				UpdateUI();
			}
		}


		private async Task OnSource_RefreshLists()
		{
			_logger.Value.Debug("OnSource_RefreshLists");

			try { await FillDisksList(); }
			catch (Exception ex)
			{
				_logger.Value.Error("OnSource_RefreshLists", ex);
				ex.FIX_ERROR(true);
			}
			finally { UpdateUI(); }
		}

		private void OnSource_DisplaySelectionUI()
		{
			_logger.Value.Debug($"OnSource_DisplaySelectionUI, _SourceMode = {_SourceMode}");

			try
			{
				switch (_SourceMode)
				{
					case SourceModes.Dir:
						{
							using FolderBrowserDialog ofd = new()
							{
								RootFolder = Environment.SpecialFolder.MyComputer,
								ShowNewFolderButton = false,
								SelectedPath = txtSource_Dir.Text.Trim()
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
			_logger.Value.Debug($"UpdateUI, _SourceMode = {_SourceMode}");

			bool bCanRun = false;
			try
			{
				switch (_SourceMode)
				{
					case SourceModes.PhyDisk:
						bCanRun = (cboSource_PhyDisk.Items.Count > 0) && (((WmiDisk)cboSource_PhyDisk.SelectedItem).Partitions < 1);
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
				_logger.Value.Error($"UpdateUI", ex);
			}
			finally { btnStartStop.Enabled = bCanRun; }
		}

		private DateTime _dtStarted = DateTime.Now;

		private void OnStartStop()
		{
			_logger.Value.Debug($"OnStartStop, _SourceMode = {_SourceMode}, _isRunning = {_isRunning}");

			if (_isRunning)
			{
				tmrElapsed.Stop();
				//tmrElapsed.Enabled = false;

				_sdmgr!.Stop();
			}
			else
			{
				_dtStarted = DateTime.Now;
				OnElapsedTimerTick();
				tmrElapsed.Start();

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
								_sdmgr!.Run(passes, (LogDisk)cboSource_LogDisk.SelectedItem, cm);
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
					_logger.Value.Error($"OnStartStop", ex);

					ProgressBarSetState_Error();
					ex.FIX_ERROR(true);
					OnFinished();
				}
			}
		}

		private void OnFinished()
		{
			tmrElapsed.Stop();

			_logger.Value.Debug("OnFinished");

			SDeleteEngine_DetachEvents();

			_isRunning = false;
			tlpParams.Enabled = true;
			btnStartStop.Text = "Start";
			ProgressBarSetState_Finished();
			UpdateUI();
		}

		private void OnElapsedTimerTick()
		{
			TimeSpan tsElapsed = DateTime.Now - _dtStarted;
			lblStatus.Text = $"{tsElapsed.e_ToShellTimeString(8)} since the beginning";
		}

		private void OnSDBinary_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Link ll = e.Link;

			if (ll.Start == 0)
				@"https://docs.microsoft.com/en-us/sysinternals/downloads/sdelete".e_OpenURLInBrowser();
			else
				new Action(() => _sdmgr!.SDeleteBinary.e_OpenExplorer()).e_RunTryCatch();
		}
	}
}
