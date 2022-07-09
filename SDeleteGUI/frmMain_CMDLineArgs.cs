
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;

using SDeleteGUI.Core;
using SDeleteGUI.Core.SDelete;

using static SDeleteGUI.Core.SDelete.SDeleteManager;

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{
		/// <summary>Process CMDLine args</summary>			
		private void ProcessCMDLine()
		{
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				_logger.Value.DebugArray(args, $"GetCommandLineArgs()");

				if (args.Length < 2) return;
				List<string> lArgs = args.ToList();
				lArgs.RemoveAt(0);//Remove first this executable path
				string cmdCommand = lArgs[0];
				lArgs.RemoveAt(0);//Remove first this executable path
				args = lArgs.ToArray();
				ProcessCMDLine(cmdCommand, args);
			}
			catch (Exception ex)
			{
				_logger.Value.Error($"_Load", ex);
				ex.FIX_ERROR(true);
			}
		}


		private void ProcessCMDLine(string cmd, string[] args)
		{
			cmd = cmd.Trim().ToLower();
			_logger.Value.Debug($"ProcessCMDLine(Cmd='{cmd}')");
			_logger.Value.DebugArray(args, $"Args");

			switch (cmd)
			{
				case Program.C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR:
					{
						if (args.Length != 1) throw new Exception("Command line arguments ERROR!\nOnly one directory must be specifed to clean!");
						DirectoryInfo di = new(args[0]);
						txtSource_Dir.Text = di.FullName;
						optSource_Dir.Checked = true;
						OnSourceChanged();
					}
					break;
				default: throw new Exception($"Unknown first CommandLine argument = '{cmd}'"!);
			}
		}
	}
}