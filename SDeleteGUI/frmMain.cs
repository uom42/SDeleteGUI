#nullable enable

using Microsoft.WindowsAPICodePack.Taskbar;

using NLog;



using SDeleteGUI.Core;
using SDeleteGUI.Core.SDelete;

using static System.Windows.Forms.LinkLabel;
using static SDeleteGUI.Core.SDelete.SDeleteManager;


namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private static string C_LOADING_DISKS_LIST = Localization.Strings.M_LOADING_DISK_LIST;
		private static string C_ASK_USER = string.Format(Localization.Strings.Q_MANUAL_SEARCH_SDELETE_BINARY, SDeleteManager.C_SDBIN_FILE64, SDeleteManager.C_SDBIN_FILE);

		private enum SourceModes : int
		{
			Unknown = 0,
			PhyDisk,
			LogDisk,
			Dir,
			Files
		}

		private SourceModes _SourceMode = SourceModes.Unknown;
		private static readonly Exception _exSourceError = new(Localization.Strings.E_UNKNOWN_SOURCE);

		private SDeleteManager? _sdmgr = null;
		private bool _isRunning = false;

		private RadioButton[] _optSources = { };

		private Lazy<Logger> _logger = new(() => LogManager.GetCurrentClassLogger());

		public frmMain()
		{
			InitializeComponent();

			LocalizeUI();

			this.Load += async (_, _) => await _Load();
			this.Shown += (_, _) => _Shown();
			this.FormClosing += (s, e) =>
			{
				_logger.Value.Debug($"FormClosing, CloseReason = {e.CloseReason}");
				if (e.CloseReason == CloseReason.UserClosing && _isRunning)
				{
					string msg = Localization.Strings.Q_STOP_ACTIVE_CLEANUP;
					if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						e.Cancel = true;
						return;
					}
					_sdmgr!.Stop();
				}
			};


			_optSources = new RadioButton[] { optSource_PhyDisk, optSource_LogDisk, optSource_Dir, optSource_Files };

			llSDeleteBinPath.LinkClicked += OnSDBinary_LinkClicked;
			llShellRegister.LinkClicked += (_, _) => OnShellRegister_Click();

			optSource_PhyDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_LogDisk.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Dir.CheckedChanged += (_, _) => OnSourceChanged();
			optSource_Files.CheckedChanged += (_, _) => OnSourceChanged();

			btnSource_Refresh.Click += async (_, _) => await OnSource_RefreshLists();
			btnSource_DisplaySelectionUI.Click += (_, _) => OnSource_DisplaySelectionUI();
			txtSource_Files.Click += (_, _) => OnSource_DisplaySelectionUI();


			cboSource_PhyDisk.SelectedIndexChanged += (_, _) => UpdateUI();
			txtSource_Dir.TextChanged += (_, _) => UpdateUI();
			txtSource_Files.TextChanged += (_, _) => UpdateUI();

			btnStartStop.Click += (_, _) => OnStartStop();

			tmrElapsed.Tick += (_, _) => OnElapsedTimerTick();
		}

		private void LocalizeUI()
		{
			Text = $"{Application.ProductName} {Assembly.GetExecutingAssembly().GetName().Version}";

			btnSource_DisplaySelectionUI.Text = "...";
			btnSource_Refresh.Text = "🗘";
			llShellRegister.Text = "*";

			lblWhatToClean.Text = Localization.Strings.L_CLEAN_TARGET;
			optSource_PhyDisk.Text = Localization.Strings.L_CLEAN_TARGET_PHY_DISK;
			optSource_LogDisk.Text = Localization.Strings.L_CLEAN_TARGET_LOG_DISK_FREEE_SPACE;
			optSource_Dir.Text = Localization.Strings.L_CLEAN_TARGET_FOLDER;
			optSource_Files.Text = Localization.Strings.L_CLEAN_TARGET_FILES;

			label4.Text = Localization.Strings.L_FREE_SPACE_CLEAN_METHOD;
			optCleanMode_Clean.Text = Localization.Strings.L_FREE_SPACE_CLEAN_METHOD_CLEAN;
			optCleanMode_Zero.Text = Localization.Strings.L_FREE_SPACE_CLEAN_METHOD_ZERO.e_Wrap();
			lblOverwritePassCount.Text = Localization.Strings.L_OVERWRITE_PASS_COUNT;
			btnStartStop.Text = Localization.Strings.L_START;
			label1.Text = Localization.Strings.L_OUTPUT;

		}






		private async Task _Load()
		{
			_logger.Value.Debug("_Load");

			FileInfo cbAskUserForBinary()
			{
				_logger.Value.Debug(C_ASK_USER);

				if (!C_ASK_USER.e_MsgboxAskIsYes(false, Application.ProductName)) throw new NotImplementedException();

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
			}


			try
			{
				_sdmgr = new SDeleteManager(cbAskUserForBinary);
			}
			catch (Exception ex)
			{
				_logger.Value.Error($"_Load", ex);

				if (ex is not NotImplementedException)//Not user canceled error
					ex.e_LogError(true);

				Application.Exit();
				return;
			}

			try
			{
				string sDleteExe = _sdmgr!.SDeleteBinary.Name;
				string prefix = sDleteExe + Localization.Strings.L_SDELETE_LOCATION;
				string txt = prefix + _sdmgr!.SDeleteBinary.Directory.FullName;
				llSDeleteBinPath.Links.Clear();
				llSDeleteBinPath.Text = txt;
				llSDeleteBinPath.Links.Add(0, sDleteExe.Length);
				llSDeleteBinPath.Links.Add(prefix.Length, txt.Length - prefix.Length);

				optCleanMode_Clean.Checked = true;

				await FillDisksList();

				await CheckShellRegistration(true);

				ProcessCMDLine();
			}
			catch (Exception ex)
			{
				_logger.Value.Error($"_Load2", ex);
				llSDeleteBinPath.Text = ex.Message;
				return;
			}
			finally { UpdateUI(); }
		}



		private void _Shown()
			=> _tbm.Value.SetProgressState(TaskbarProgressBarState.NoProgress);



		/// <summary>Load disk list</summary>
		private async Task FillDisksList()
		{
			_logger.Value.Debug($"FillDisksList");

			RadioButton? oldSource = _optSources.Where(radio => radio.Checked).FirstOrDefault();

			await Task.WhenAll(
				FillDisks_Log(optSource_LogDisk, cboSource_LogDisk, _SourceMode, _logger.Value)
				,
				FillDisks_Phy(optSource_PhyDisk, cboSource_PhyDisk, _SourceMode, _logger.Value)
				);

			if (oldSource == null || (oldSource != null && !oldSource.Enabled))
			{
				RadioButton furstAvailRadio = _optSources.Where(radio => radio.Enabled).First()!;
				furstAvailRadio.Checked = true;
			}
		}

		/// <summary>Load Physical disk list from WMI</summary>
		private static async Task FillDisks_Log(RadioButton opt, ComboBox cbo, SourceModes cm, Logger log)
		{
			log.Debug("FillDisks_Log");

			LogDisk? oldSelectedDisk = null;
			cbo.e_RunInUIThread(() =>
			{
				opt.Enabled = false;

				oldSelectedDisk = ((cbo.SelectedItem != null) && (cbo.SelectedItem is LogDisk selectedDisk)) ? selectedDisk : null;
				cbo.e_DisabeAndShow(C_LOADING_DISKS_LIST);
				cbo.Enabled = false;
			});

			try
			{
				LogDisk[] disks = await LogDisk.GetDisksAsync(); log.e_DumpArray(disks, "LogDisk.GetDisksAsync");
				LogDisk? newSelection = (oldSelectedDisk == null)
					? disks.LastOrDefault()
					: (disks.Where(d => d.DiskLetter == oldSelectedDisk.DiskLetter).FirstOrDefault()) ?? disks.LastOrDefault();

				cbo.e_RunInUIThread(() =>
				{
					opt.Enabled = disks.Any();
					cbo.e_FillWithObjects(disks, cm == SourceModes.LogDisk, false);
					if (newSelection != null) cbo.SelectedItem = newSelection!;
				});
			}
			catch (Exception ex)
			{
				log.Error(ex);
				cbo.e_RunInUIThread(() => cbo.e_DisabeAndShow(ex));
			}
		}

		/// <summary>Load Physical disk list from WMI</summary>
		private static async Task FillDisks_Phy(RadioButton opt, ComboBox cbo, SourceModes cm, Logger log)
		{
			log.Debug("FillDisks_Phy");

			Win32_DiskDrive? oldSelectedDisk = null;
			cbo.e_RunInUIThread(() =>
			{
				opt.Enabled = false;

				oldSelectedDisk = ((cbo.SelectedItem != null) && (cbo.SelectedItem is Win32_DiskDrive selectedDisk))
					? selectedDisk
					: null;

				cbo.e_DisabeAndShow(C_LOADING_DISKS_LIST);
				cbo.Enabled = false;
			});

			try
			{
				Win32_DiskDrive[] disks = await Win32_DiskDrive.GetDisksAsync(); log.e_DumpArray(disks, "WmiDisk.GetDisksAsync");
				disks = disks
					.Where(d => d.Partitions < 1)
					.ToArray();
				log.e_DumpArray(disks, "WmiDisks with 0 partitions");


				Win32_DiskDrive? newSelection = (oldSelectedDisk == null)
					? disks.FirstOrDefault()
					: (disks.Where(d => d == oldSelectedDisk).FirstOrDefault())
						?? disks.FirstOrDefault();

				cbo.e_RunInUIThread(() =>
				{
					opt.Enabled = disks.Any();
					cbo.e_FillWithObjects(
						   disks,
						   ((cm == SourceModes.PhyDisk) && disks.Any()),
						   false);

					if (newSelection != null)
						cbo.SelectedItem = newSelection!;
				});

			}
			catch (Exception ex)
			{
				log.Error(ex);
				cbo.e_RunInUIThread(() => cbo.e_DisabeAndShow(ex));
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
				ex.e_LogError(true);
			}
			finally { UpdateUI(); }
		}



		private FolderBrowserDialog _bffUI = new()
		{
			RootFolder = Environment.SpecialFolder.MyComputer,
			ShowNewFolderButton = false
		};

		private OpenFileDialog _ofdUI = new()
		{
			AutoUpgradeEnabled = true,
			AddExtension = true,
			CheckFileExists = true,
			CheckPathExists = true,
			DereferenceLinks = true,
			Filter = Localization.Strings.FILTER_ANY_FILE,
			Multiselect = true,
			ShowHelp = false,
			ShowReadOnly = false,
			SupportMultiDottedExtensions = true,
			ValidateNames = true,
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)

		};



		private void OnSource_DisplaySelectionUI()
		{
			_logger.Value.Debug($"OnSource_DisplaySelectionUI, _SourceMode = {_SourceMode}");

			try
			{
				switch (_SourceMode)
				{
					case SourceModes.Dir:
						{
							_bffUI.SelectedPath = txtSource_Dir.Text.Trim();
							var dr = _bffUI.ShowDialog(this);
							if (dr != DialogResult.OK) return;

							txtSource_Dir.Text = _bffUI.SelectedPath;
						}
						break;

					case SourceModes.Files:
						{
							var dr = _ofdUI.ShowDialog(this);
							if (dr != DialogResult.OK) return;

							FileInfo[] files = _ofdUI
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
				ex.e_LogError(true);
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
						Win32_DiskDrive? selDisk = cboSource_PhyDisk.SelectedItem as Win32_DiskDrive;
						var tt = (selDisk != null);
						bCanRun = (selDisk != null) && (selDisk!.Partitions < 1);
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
								_sdmgr!.Run(passes, (Win32_DiskDrive)cboSource_PhyDisk.SelectedItem, cm);
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
					ex.e_LogError(true);
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
			btnStartStop.Text = Localization.Strings.L_START;
			ProgressBarSetState_Finished();
			UpdateUI();
		}

		private void OnElapsedTimerTick()
		{
			TimeSpan tsElapsed = DateTime.Now - _dtStarted;
			lblStatus.Text = string.Format(Localization.Strings.L_ELAPSED, tsElapsed.e_ToShellTimeString(8));
		}

		private void OnSDBinary_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Link ll = e.Link;

			if (ll.Start == 0)
				@"https://docs.microsoft.com/en-us/sysinternals/downloads/sdelete".e_OpenURLInBrowser();
			else
				new Action(() => _sdmgr!.SDeleteBinary.e_OpenExplorer()).e_tryCatch();
		}
	}
}
