
using SDeleteGUI.Core;

#nullable enable

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private const string C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR = "dir";
		private const string C_SHELL_CONTEXTMENU_MENU_TITLE_PREFIX = "¤ ";
		private const string C_SHELL_CONTEXTMENU_MENU_REGVALUE = "UOM_Clean";
		private const string C_DIALOGID_DO_NOT_ASK_SHELL_CONTEXT_MENU_REGISTRATION = "DoNotAskContextMenuShellRegistration";
		static readonly string C_SHELL_CONTEXTMENU_MENU_TITLE = C_SHELL_CONTEXTMENU_MENU_TITLE_PREFIX + Localization.Strings.L_SHELL_MENU_DISPLAY_NAME;

		private bool _isShellRegistered = false;
		private async Task CheckShellRegistration(bool isFirstRun)
		{
			_logger.Value.Debug($"CheckShellRegistration()");
			llShellRegister.Text = string.Empty;

			using Task<bool> tskCheckRegistration = new(()
				=> uom.OS.Shell.ContextMenu_IsRegisteredForDirectory(C_SHELL_CONTEXTMENU_MENU_REGVALUE,
					   C_SHELL_CONTEXTMENU_MENU_TITLE,
					   null,
					   C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR), TaskCreationOptions.LongRunning);

			_isShellRegistered = await tskCheckRegistration.e_StartAndWaitAsync();


			llShellRegister.Text = _isShellRegistered
				? Localization.Strings.L_SHELL_MENU_UNREGISTER
						: Localization.Strings.L_SHELL_MENU_REGISTER;

			if (!isFirstRun || _isShellRegistered) return;

#if DEBUG
			return;
#endif

			DialogResult dr = Localization.Strings.Q_REGISTER_SHELL_MENU.e_MsgboxWithCheckboxAsk(C_DIALOGID_DO_NOT_ASK_SHELL_CONTEXT_MENU_REGISTRATION, checkBoxText: Localization.Strings.L_DONT_ASK_AGAIN);
			_logger.Value.Debug($"MsgboxAskWithCheckbox() = {dr}");
			if (dr != DialogResult.Yes) return;

			OnShellRegister_Click();
		}

		private void OnShellRegister_Click() => ShellRegister(!_isShellRegistered);

		private void ShellRegister(bool regisster)
		{
			_logger.Value.Debug($"ShellRegister(regisster = {regisster}, isShellRegistered = {_isShellRegistered})");
			try
			{
				if (regisster)
				{
					uom.OS.Shell.ContextMenu_RegisterForDirectory(
						C_SHELL_CONTEXTMENU_MENU_REGVALUE,
						C_SHELL_CONTEXTMENU_MENU_TITLE,
						 null,
						 C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR);
				}
				else
				{
					uom.OS.Shell.ContextMenu_UnRegisterForDirectory(C_SHELL_CONTEXTMENU_MENU_REGVALUE);
					C_DIALOGID_DO_NOT_ASK_SHELL_CONTEXT_MENU_REGISTRATION.e_MsgboxWithCheckboxClearLastUserAnswer();
				}
			}
			catch (Exception ex)
			{
				ex.e_LogError(true);
			}
			finally
			{
				_ = CheckShellRegistration(false);
			}
		}


		/// <summary>Process CMDLine args</summary>			
		private void ProcessCMDLine()
		{
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				_logger.Value.e_DumpArray(args, $"GetCommandLineArgs()");

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
				ex.e_LogError(true);
			}
		}


		private void ProcessCMDLine(string cmd, string[] args)
		{
			cmd = cmd.Trim().ToLower();
			_logger.Value.Debug($"ProcessCMDLine(Cmd='{cmd}')");
			_logger.Value.e_DumpArray(args, $"Args");

			switch (cmd)
			{
				case C_SHELL_CONTEXTMENU_MENU_ARG_CLEAN_DIR:
					{
						if (args.Length != 1) throw new Exception(Localization.Strings.E_CMDLINE_ONLY_1_DIR_MUST_BE);
						DirectoryInfo di = new(args[0]);
						txtSource_Dir.Text = di.FullName;
						optSource_Dir.Checked = true;
						OnSourceChanged();
					}
					break;
				default: throw new Exception(string.Format(Localization.Strings.E_CMDLINE_UNKNOWN_1_ARG, cmd).e_Wrap());
			}
		}
	}
}