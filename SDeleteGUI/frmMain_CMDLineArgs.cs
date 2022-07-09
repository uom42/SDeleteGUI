
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;

using SDeleteGUI.Core;
using SDeleteGUI.Core.SDelete;

using static SDeleteGUI.Core.SDelete.SDeleteManager;

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{



		//private static void ShellRegisterAsync(bool regisster) => new Task(() => ShellRegister(regisster), TaskCreationOptions.LongRunning).Start();
		const string C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR = "dir";

		private void ShellRegister(bool regisster)
		{
			const string C_SHELL_CONTEXTMENU_MENU_TITLE_PREFIX = "¤ ";
			const string C_SHELL_CONTEXTMENU_MENU_TITLE = C_SHELL_CONTEXTMENU_MENU_TITLE_PREFIX + "Clean";
			const string C_SHELL_CONTEXTMENU_MENU_REGVALUE = "UOM_Clean";

			_logger.Value.Debug($"ShellRegister({regisster})");
			if (regisster)
			{
				bool AlreadyRegistered = uom.OS.Shell.ContextMenu_IsRegisteredForDirectory(C_SHELL_CONTEXTMENU_MENU_REGVALUE,
					C_SHELL_CONTEXTMENU_MENU_TITLE,
					null,
					C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR);

				if (AlreadyRegistered)
				{
					_logger.Value.Debug($"ShellRegister - Already registered OK.");
					return; //Already registered
				}

				DialogResult dr = "Register in shell context menu for folders clean up?".e_MsgboxAskWithCheckbox("DoNotAskContextMenuShellRegistration");
				_logger.Value.Debug($"MsgboxAskWithCheckbox() = {dr}");

				if (dr != DialogResult.Yes) return;

				_logger.Value.Debug($"ShellRegister - Still Unregistered! Calling ContextMenu_RegisterForDirectory()...");
				uom.OS.Shell.ContextMenu_RegisterForDirectory(
					C_SHELL_CONTEXTMENU_MENU_REGVALUE,
					C_SHELL_CONTEXTMENU_MENU_TITLE,
					 null,
					 C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR);
			}
		}





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
				case C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR:
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