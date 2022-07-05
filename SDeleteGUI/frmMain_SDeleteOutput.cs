
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;

using SDeleteGUI.Core.SDelete;

using static SDeleteGUI.Core.SDelete.SDeleteManager;

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private void SDeleteEngine_AttachEvents()
		{
			_sdmgr!.OutputRAW += OnCore_Data_RAW;
			_sdmgr!.OutputProgress += OnCore_Data_Progress!;
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
			LogAddRowMTSafe(s);
		}


		private void LogAddRowMTSafe(string s)
		{
			//lstLog.e_runInUIThread_AppendLine(s, 1000);
			const int C_MAX_LOG_ROWS = 1000;


			Action addRowInUIThread = new(() =>
			{
				lstLog.BeginUpdate();
				try
				{
					while (lstLog.Items.Count >= C_MAX_LOG_ROWS)
					{
						lstLog.Items.RemoveAt(0);
					}

					if (lstLog.Items.Count > 0)
					{
						//Будем искать частичное совпадение с предыдущей строкой, если оно будет - то сделаем в предыдущей строке замену текста
						const int MIN_EQUAL_CHARS = 3;
						const int MIN_EQUAL_WORDS = 2;

						string lastRow = (string)lstLog.Items[^1];
						if (!string.IsNullOrWhiteSpace(lastRow))
						{
							var eq = s.e_GetStringsEquality(lastRow);

							if ((eq.CommonPrefixString == s)
							|| (
							(eq.CommonPrefixLen >= MIN_EQUAL_CHARS) && (eq.UniqueWordsInBothStrings.Length >= MIN_EQUAL_WORDS)
							))
							{
								//Looks like the some string as previous with some changes... Just update last row
								lstLog.Items[^1] = s;
							}
							else
							{
								//Looks like diferend string
								lstLog.Items.Add(s);
								lstLog.SelectedIndex = (lstLog.Items.Count - 1);
							}
						}
						else
						{
							lstLog.Items.Add(s);
							lstLog.SelectedIndex = (lstLog.Items.Count - 1);
						}
					}
					else
					{
						lstLog.Items.Add(s);
						//lstLog.SelectedIndex = (lstLog.Items.Count - 1);
					}
				}
				finally { lstLog.EndUpdate(); }
			});

			this.e_runInUIThread(addRowInUIThread);
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
			LogAddRowMTSafe("ERROR!: " + (s));
		}

	}
}
