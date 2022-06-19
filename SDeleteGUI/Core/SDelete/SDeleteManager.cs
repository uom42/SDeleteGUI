#nullable enable


using uom;


namespace SDeleteGUI.Core.SDelete
{
	internal class SDeleteManager
	{
		internal const string C_SDBIN_FILE64 = @"sdelete64.exe";
		internal const string C_SDBIN_FILE = @"sdelete.exe";
		private const string C_DEFAULT_CHOCOLATEY_SDBIN_DIR = @"C:\ProgramData\chocolatey\lib\sysinternals\tools";

		private const string C_RESOURCE_ALREADY_BUSY = "Resource '{0}' is already in use!";

		#region SDelete binary CLI usage
		/*
		usage: sdelete [-p passes] [-r] [-s] [-q] <file or directory> [...]
		sdelete [-p passes] [-z|-c [percent free]] <drive letter [...]>
		sdelete [-p passes] [-z|-c] <physical disk number>

		-c         Clean free space. Specify an option amount of space
			to leave free for use by a running system.
		-f         Force arguments containing only letters to be treated as a file/directory rather than a disk.
			Not required if the argument contains other characters (path separators or file extensions for example)
		-p         Specifies number of overwrite passes (default is 1)
		-r         Remove Read-Only attribute
		-s         Recurse subdirectories
		-z         Zero free space (good for virtual disk optimization)
		-nobanner  Do not display the startup banner and copyright message.
		 */

		internal const string C_ARG_CLEAN_FREE_SPACE = @"-c";
		internal const string C_ARG_ZERO_FREE_SPACE = @"-z";
		internal const string C_ARG_FORCE_PATH = @"-f";
		internal const string C_ARG_PASSES = @"-p";
		internal const string C_ARG_REMOVE_RO = @"-r";
		internal const string C_ARG_RECURSE = @"-s";
		internal const string C_ARG_NO_BANNER = @"-nobanner";
		internal const string C_ARG_ACCEPT_LICENSE = @"/accepteula";

		#endregion

		private Process? _runningProcess = null;

		/// <summary>A thread waiting for an external process to terminate</summary>
		private Thread? _thCore;
		private AutoResetEvent _evtCleanFinished = new(false);
		/// <summary>Blocking mutex to diallow multiintance Disk/folder resource procesing</summary>
		private Mutex? _mtxLock;
		private Lazy<Logger> _logger = new(() => LogManager.GetCurrentClassLogger());

		public readonly FileInfo SDeleteBinary;

		public event EventHandler Finished = delegate { };

		public event EventHandler<DataReceivedEventArgsEx> OutputRAW = delegate { };
		public event DataReceivedEventHandler Error = delegate { };

		public enum CleanModes : int
		{
			/// <summary>Zero free space (good for virtual disk optimization)</summary>
			Zero = 0,
			/// <summary>Clean free space. Specify an option amount of space</summary>
			Clean
		}

		public SDeleteManager(Func<FileInfo> binaryPathSpecifer)
		{
			const string C_LAST_KNOWN_SDELETE_NAME = "LastKnownSDeletePath";

			_logger.Value.Debug($"NEW SDeleteManager");

			var knownBinPath = Application.UserAppDataRegistry.e_GetValue_StringOrEmpty(C_LAST_KNOWN_SDELETE_NAME);
			_logger.Value.Debug($"knownBinPath: {knownBinPath}");

			if (knownBinPath == null || !File.Exists(knownBinPath))
			{
				DirectoryInfo diChocolateySDeleteDir = new(C_DEFAULT_CHOCOLATEY_SDBIN_DIR);
				if (diChocolateySDeleteDir.Exists)
				{
					var found64 = diChocolateySDeleteDir.EnumerateFiles(C_SDBIN_FILE64).Any();
					var found32 = diChocolateySDeleteDir.EnumerateFiles(C_SDBIN_FILE).Any();

					if (found64 || found32)
					{

						knownBinPath = Path.Combine(diChocolateySDeleteDir.FullName,
							found64
							? C_SDBIN_FILE64
							: C_SDBIN_FILE);

						_logger.Value.Debug($"Found in Chocolatey well-known dir: {knownBinPath}");
					}
				}
			}

			if (knownBinPath == null || !File.Exists(knownBinPath))
			{
				_logger.Value.Debug($"Still not found, ask user...");
				FileInfo fiUserBinPath = binaryPathSpecifer.Invoke();
				if (fiUserBinPath.Exists)
					knownBinPath = fiUserBinPath.FullName;
			}

			_logger.Value.Debug(knownBinPath);
			if (knownBinPath == null || !File.Exists(knownBinPath))
				throw new Exception($"Not found SDelete binary file '{C_SDBIN_FILE}'!");

			SDeleteBinary = new(knownBinPath);
			Application.UserAppDataRegistry.e_SetValueString(C_LAST_KNOWN_SDELETE_NAME, SDeleteBinary.FullName);
			_logger.Value.Debug($"Saving registry setting value '{C_LAST_KNOWN_SDELETE_NAME}' = '{SDeleteBinary}'");
		}


		public uint Passes { get; private set; }


		/// <summary>Clean entrie Physical disk
		/// Make sure that the disk has no file system volumes!
		/// </summary>
		public void Run(uint passes, Win32_DiskDrive disk, CleanModes cm)
		{
			_logger.Value.Debug($"Run_Clean: Passes '{passes}', WmiDisk = '{disk}', CleanModes = '{cm}'");
			if (disk.Partitions > 0) throw new Exception($"Make sure that the disk '{disk}' has no file system volumes!");
			string args = $"{cm.ToArgs()} {disk.Index}";

			string mutexName = $"{Application.ProductName}-{disk.Index}";
			StartSDeleteCore(passes, args, mutexName, disk.ToString());
		}


		/// <summary>Zeroing free space on Disk</summary>
		public void Run(uint passes, LogDisk disk, CleanModes cm)
		{
			_logger.Value.Debug($"Run_Clean: Passes '{passes}', LogDisk = '{disk}', CleanModes = '{cm}'");
			string args = @$"{C_ARG_FORCE_PATH} {C_ARG_REMOVE_RO} {C_ARG_RECURSE} {cm.ToArgs()} {disk.DiskLetter}:";

			string mutexName = $"{Application.ProductName}-{disk.DiskLetter}";
			StartSDeleteCore(passes, args, mutexName, disk.ToString());
		}


		/// <summary>Clean Directory on disk</summary>
		public void Run(uint passes, DirectoryInfo dirToClean)
		{
			_logger.Value.Debug($"Run_Clean: Passes '{passes}', dirToClean = '{dirToClean}'");
			if (!dirToClean.Exists) throw new Exception($"Directory '{dirToClean}' not found!");

			string args = $"{C_ARG_FORCE_PATH} {C_ARG_REMOVE_RO} {C_ARG_RECURSE} ";
			bool isRoot = dirToClean.ToString().ToLower() == dirToClean.Root.ToString().ToLower();
			if (isRoot)
				args += @$"""{dirToClean.FullName[0]}:\*""";
			else
				args += @$"""{dirToClean}""";

			string mutexName = $"{Application.ProductName}-{dirToClean.ToString().GetHashCode()}";
			StartSDeleteCore(passes, args, mutexName, dirToClean.ToString());
		}

		/// <summary>Clean Directory on disk</summary>
		public void Run(uint passes, FileInfo[] filesToClean)
		{
			string allFiles = string.Join(" ", filesToClean
				.Select(fi => (constants.CC_QUOTE + fi.FullName + constants.CC_QUOTE))
				.ToArray());

			_logger.Value.Debug($"Run_Clean: Passes '{passes}', filesToClean = '{allFiles}'");

			if (!filesToClean.Any()) throw new ArgumentNullException(nameof(filesToClean));
			foreach (FileInfo fi in filesToClean)
			{
				fi.Refresh();
				if (!fi.Exists) throw new Exception($"File '{fi}' not found!");
			}

			string args = C_ARG_REMOVE_RO + " " + allFiles;
			StartSDeleteCore(passes, args, null, null);
		}


		private void StartSDeleteCore(uint passes, string SDeleteArgs, string? mutexName, string? resourceName)
		{
			Passes = passes;

			bool isNewMutex = false;
			Mutex? mtx = string.IsNullOrWhiteSpace(mutexName)
				? null
				: new(true, mutexName, out isNewMutex);

			try
			{
				if (null != mtx && !isNewMutex) throw new(C_RESOURCE_ALREADY_BUSY.e_Format(resourceName!));

				SDeleteArgs = @$"{C_ARG_ACCEPT_LICENSE} {C_ARG_PASSES} {passes} " + SDeleteArgs;
				//SDeleteArgs = @$"{C_ARG_PASSES} {passes} " + SDeleteArgs;

				_logger.Value.Debug($"Run_Clean: StartSDeleteCore. Passes '{passes}', SDeleteArgs = '{SDeleteArgs}'");
				_runningProcess = null;

				string sLog = $"*** RunConsole '{SDeleteBinary}', Args: '{SDeleteArgs}'";
				Debug.WriteLine(sLog);
				_logger.Value.Debug(sLog);

				ProcessStartInfo psi = new()
				{
					FileName = SDeleteBinary.FullName,
					WorkingDirectory = SDeleteBinary.DirectoryName,
					Arguments = SDeleteArgs,
					UseShellExecute = false,

					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput = true,

					LoadUserProfile = true,
					CreateNoWindow = true
				};

				Encoding enc = Encoding.GetEncoding(866);
				try
				{
					FileVersionInfo? sDeleteFileVersion = FileVersionInfo.GetVersionInfo(SDeleteBinary.FullName);
					if (sDeleteFileVersion != null)
					{
						System.Version unicodeSDeleteVersion = new(2, 5, 0, 0);

						System.Version sDeleteVersion = new(sDeleteFileVersion.FileMajorPart, sDeleteFileVersion.FileMinorPart, 0, 0);
						if (sDeleteVersion >= unicodeSDeleteVersion) enc = Encoding.Unicode;
					}
				}
				catch { }
				{
					psi.StandardOutputEncoding = enc;
					psi.StandardErrorEncoding = enc;
				}

				_runningProcess = Process.Start(psi) ?? throw new ArgumentException($"Failed to start '{SDeleteBinary}'!");
				_thCore = new Thread(CoreThreadProc)
				{
					Name = "A waiting thread for SDelete finish",
					IsBackground = true
				};
				_evtCleanFinished = new AutoResetEvent(false);
				_thCore.Start();
				_mtxLock = mtx;

				//Thread.Sleep(2000);//Wait

				string cli = $"{SDeleteBinary.FullName} {SDeleteArgs}";
				cli.e_SetClipboardSafe();
				DataReceivedEventArgsEx ea = new($"{Localization.Strings.M_STARING_SDELETE} {cli}");
				OnCore_Data(this, ea);
			}
			catch
			{
				mtx?.Dispose();
				throw;
			}
		}

		private void CoreConnect()
		{
			_logger.Value.Debug("CoreConnect");

			if (_runningProcess == null) throw new ArgumentNullException(nameof(_runningProcess));

			_runningProcess.OutputDataReceived += OnCore_Data;
			_runningProcess.ErrorDataReceived += OnCore_Error;

			_runningProcess.BeginOutputReadLine();
			_runningProcess.BeginErrorReadLine();
		}


		private void OnCore_Data(object sender, DataReceivedEventArgs e)
		{
			string s = (e.Data ?? string.Empty).Trim();
			if (string.IsNullOrWhiteSpace(s)) return;

			OnCore_Data(sender, DataReceivedEventArgsEx.Parse(e));
		}
		private void OnCore_Data(object sender, DataReceivedEventArgsEx e)
		{
			_logger.Value.Debug($"OnCore_Data: '{e.RAWData}'");
			OutputRAW.Invoke(sender, e);
		}

		private void OnCore_Error(object sender, DataReceivedEventArgs e)
		{
			_logger.Value.Debug($"OnCore_Error: '{e.Data}'");
			string s = (e.Data ?? string.Empty).Trim();
			if (s.e_IsNullOrEmpty()) return;

			if (null == Error) Debug.WriteLine($"WARNING! Unprocessed Error Data: '{s}'");
			Error!.Invoke(sender, e);
		}


		private void CoreDisConnect()
		{
			_logger.Value.Debug("CoreDisConnect");
			if (_runningProcess == null) throw new ArgumentNullException(nameof(_runningProcess));
			try { _runningProcess!.CancelOutputRead(); } catch { }
			try { _runningProcess!.CancelErrorRead(); } catch { }
		}


		private void CoreThreadProc()
		{
			CoreConnect();
			try
			{
				while (!_runningProcess!.HasExited)
				{
					_logger.Value.Debug($"*** CoreThreadProc Waiting for finish {SDeleteBinary}...");
					_runningProcess.WaitForExit(1000);
					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"*** CoreThreadProc ERROR! '{SDeleteBinary}' {ex.Message}.");
				_logger.Value.Error($"CoreThreadProc", ex);
			}
			finally
			{
				_mtxLock?.Dispose();
				_mtxLock = null;

				string s = $"*** CoreThreadProc Finished '{SDeleteBinary}'.";
				Debug.WriteLine(s);
				_logger.Value.Debug(s);

				_evtCleanFinished.Set();
				CoreDisConnect();

				_runningProcess = null;
				_thCore = null;
				Finished!.Invoke(this, EventArgs.Empty);
			}
		}


		public void Stop()
		{
			_logger.Value.Debug($"Core Need Stop!");
			if (_runningProcess == null) return;

			if (!_runningProcess!.HasExited)
			{
				if (!_runningProcess.e_Console_SendCtrlEvent(Extensions_Process.CtrlEvents.CTRL_C_EVENT, false))//Trying to send Exit signal to Ext App
				{
					_logger.Value.Debug($"SendCtrl+C FAILED! Killing SDelete via Process.Kill...");
					_runningProcess.Kill();//Emergency closing process
				}


				_evtCleanFinished.WaitOne();
			}
			_logger.Value.Debug($"Core Stopped");
		}




	}
}
