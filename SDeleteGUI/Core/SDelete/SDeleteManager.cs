﻿using static uom.AppTools;
using System.Text;
using uom;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace SDeleteGUI.Core.SDelete
{
	internal class SDeleteManager
	{
		internal const string C_SDBIN_FILE64 = @"sdelete64.exe";
		internal const string C_SDBIN_FILE = @"sdelete.exe";
		private const string C_DEFAULT_CHOCOLATEY_SDBIN_DIR = @"C:\ProgramData\chocolatey\lib\sysinternals\tools";

		public readonly FileInfo SDeleteBinary;

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

		private Process? runningProcess = null;
		private Thread? _thCore = null;


		public event EventHandler Finished = delegate { };
		public event DataReceivedEventHandler OutputRAW = delegate { };
		public event EventHandler<ProgressInfo> OutputProgress = delegate { };
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

			var knownBinPath = Application.UserAppDataRegistry.e_GetValue_StringOrEmpty(C_LAST_KNOWN_SDELETE_NAME);
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
					}
				}
			}

			if (knownBinPath == null || !File.Exists(knownBinPath))
			{
				FileInfo fiUserBinPath = binaryPathSpecifer.Invoke();
				if (fiUserBinPath.Exists)
					knownBinPath = fiUserBinPath.FullName;
			}

			if (knownBinPath == null || !File.Exists(knownBinPath))
				throw new Exception($"Not found SDelete binary file '{C_SDBIN_FILE}'!");

			SDeleteBinary = new(knownBinPath);
			Application.UserAppDataRegistry.e_SetValue(C_LAST_KNOWN_SDELETE_NAME, SDeleteBinary.FullName);
		}




		/// <summary>Clean entrie Physical disk
		/// Make sure that the disk has no file system volumes!
		/// </summary>
		public void Run(uint passes, WmiDisk disk, CleanModes cm)
		{
			if (disk.Partitions > 0) throw new Exception($"Make sure that the disk '{disk}' has no file system volumes!");
			string args = $"{cm.ToArgs()} {disk.Index}";
			StartSDeleteCore(passes, args);
		}


		/// <summary>Zeroing free space on Disk</summary>
		public void Run(uint passes, char disk, CleanModes cm)
		{
			string args = @$"{C_ARG_FORCE_PATH} {C_ARG_REMOVE_RO} {C_ARG_RECURSE} {cm.ToArgs()} {disk}:";
			StartSDeleteCore(passes, args);
		}


		/// <summary>Clean Directory on disk</summary>
		public void Run(uint passes, DirectoryInfo dirToClean)
		{
			if (!dirToClean.Exists) throw new Exception($"Directory '{dirToClean}' not found!");

			string args = $"{C_ARG_FORCE_PATH} {C_ARG_REMOVE_RO} {C_ARG_RECURSE} ";
			bool isRoot = dirToClean.ToString().ToLower() == dirToClean.Root.ToString().ToLower();
			if (isRoot)
				args += @$"""{dirToClean.FullName[0]}:\*""";
			else
				args += @$"""{dirToClean}""";

			StartSDeleteCore(passes, args);
		}

		/// <summary>Clean Directory on disk</summary>
		public void Run(uint passes, FileInfo[] filesToClean)
		{
			if (!filesToClean.Any()) throw new ArgumentNullException(nameof(filesToClean));
			foreach (FileInfo fi in filesToClean)
			{
				fi.Refresh();
				if (!fi.Exists) throw new Exception($"File '{fi}' not found!");
			}

			string args = C_ARG_REMOVE_RO + " "
				+ string.Join(" ", filesToClean
				.Select(fi => (constants.CC_QUOTE + fi.FullName + constants.CC_QUOTE))
				.ToArray());

			StartSDeleteCore(passes, args);
		}


		private void StartSDeleteCore(uint passes, string SDeleteArgs)
		{
			SDeleteArgs = @$"{C_ARG_ACCEPT_LICENSE} {C_ARG_PASSES} {passes} " + SDeleteArgs;


			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			//Encoding enc = Encoding.GetEncoding(866);
			Encoding? enc = null;// Encoding.GetEncoding("866")!;			
			enc = Encoding.GetEncoding(20866)!;
			runningProcess = null;

			//ILogger logger = LogManager.GetCurrentClassLogger();
			//logger.Debug($"*** RunConsole '{fiExe.FullName}', Args: '{arguments}'");
			Debug.WriteLine($"*** RunConsole '{SDeleteBinary}', Args: '{SDeleteArgs}'");

			ProcessStartInfo psi = new()
			{
				FileName = SDeleteBinary.FullName,
				WorkingDirectory = SDeleteBinary.DirectoryName,
				Arguments = SDeleteArgs,
				UseShellExecute = false,

				RedirectStandardOutput = true,
				RedirectStandardError = true,

				LoadUserProfile = true,
				CreateNoWindow = true
			};

			if (null != enc)
			{
				psi.StandardOutputEncoding = enc;
				psi.StandardErrorEncoding = enc;
			}

			runningProcess = Process.Start(psi) ?? throw new ArgumentException($"Failed to start '{SDeleteBinary.FullName}'!");

			_thCore = new Thread(CoreThreadProc)
			{
				Name = "safd",
				IsBackground = true
			};
			_evtCleanFinished = new AutoResetEvent(false);
			_thCore.Start();
		}

		private void CoreConnect()
		{
			if (runningProcess == null) throw new ArgumentNullException(nameof(runningProcess));

			runningProcess.OutputDataReceived += OnCore_Data;
			runningProcess.ErrorDataReceived += OnCore_Error;

			runningProcess.BeginOutputReadLine();
			runningProcess.BeginErrorReadLine();
		}


		private void OnCore_Data(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null) return;
			string s = (e.Data ?? string.Empty).Trim();
			if (s.e_IsNullOrEmpty()) return;

			if (null == OutputRAW) Debug.WriteLine($"WARNING! Unprocessed Output Data: '{s}'");
			OutputRAW!.Invoke(sender, e);

			if (ProgressInfo.IsMatch(s))
			{
				ProgressInfo pi = new(s);
				OutputProgress!.Invoke(this, pi);
			}
		}
		private void OnCore_Error(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null) return;
			string s = (e.Data ?? string.Empty).Trim();
			if (s.e_IsNullOrEmpty()) return;

			if (null == Error) Debug.WriteLine($"WARNING! Unprocessed Error Data: '{s}'");
			Error!.Invoke(sender, e);
		}



		private void CoreDisConnect()
		{
			if (runningProcess == null) throw new ArgumentNullException(nameof(runningProcess));
			try { runningProcess!.CancelOutputRead(); } catch { }
			try { runningProcess!.CancelErrorRead(); } catch { }
		}

		private AutoResetEvent _evtCleanFinished = new AutoResetEvent(false);

		private void CoreThreadProc()
		{
			CoreConnect();
			try
			{
				while (!runningProcess!.HasExited)
				{
					///Debug.WriteLine($"*** ConsoleCore Waitning...");
					runningProcess.WaitForExit(1000);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"*** Console ERROR! '{SDeleteBinary}' {ex.Message}.");
			}
			finally
			{
				Debug.WriteLine($"*** FinishedConsole '{SDeleteBinary}'.");

				_evtCleanFinished.Set();
				CoreDisConnect();
				OnFinished();
			}
		}

		private void OnFinished()
		{
			runningProcess = null;
			Finished!.Invoke(this, EventArgs.Empty);
			_thCore = null;
		}

		public void Stop()
		{
			if (runningProcess == null) return; //; throw new Exception("Still not started!");

			if (!runningProcess!.HasExited)
			{
				runningProcess.Kill();
				_evtCleanFinished.WaitOne();
			}
		}
	}
}
