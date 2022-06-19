
using SDeleteGUI.Core.SDelete;

#nullable enable

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
			=> LogAddRowMTSafe(e?.Data, false);

		private void OnCore_Error(object sender, DataReceivedEventArgs e)
			=> LogAddRowMTSafe(e?.Data, true);

		private void OnCore_Data_Progress(object sender, ProgressInfo e) => this.e_runInUIThread(() => OnCore_Data_Progress(e));

		private void OnCore_Data_Progress(ProgressInfo e) => ProgressBarSetState_Progress(e.ProgressPercent);


		private void LogAddRowMTSafe(string? s, bool isError = false)
		{
			if (s.e_IsNullOrEmpty()) return;//Skip empty lines

			const int C_MAX_LOG_ROWS = 1000;

			if (isError) s = "!ERROR STREAM!: " + s;
			Debug.WriteLine($"Output Data: '{s}'");

			Action addRowInUIThread = new(() =>
			{
				lstLog.BeginUpdate();
				try
				{
					//Limit count of lines to limit
					while (lstLog.Items.Count >= C_MAX_LOG_ROWS) lstLog.Items.RemoveAt(0);

					if (!isError && lstLog.Items.Count > 0)
					{
						//We will search for string equality with previous log line
						//and if we found - we just replace text in last report line
						const int MIN_EQUAL_CHARS = 3;
						const int MIN_EQUAL_WORDS = 2;

						string lastRow = (string)lstLog.Items[lstLog.Items.Count - 1];
						if (!string.IsNullOrWhiteSpace(lastRow))
						{
							var eq = s
							.e_GetEqualityMetrics(lastRow);

							if ((eq.CommonPrefix == s)
							|| (
							(eq.CommonPrefix.Length >= MIN_EQUAL_CHARS) && (eq.UniqueWordsInBothStrings.Length >= MIN_EQUAL_WORDS)
							))
							{
								//Looks like the some string as previous with some changes... Just update last row
								lstLog.Items[lstLog.Items.Count - 1] = s;
								return;
							}
						}
					}

					//No found equality or this is error.
					//Just add new row to log
					lstLog.Items.Add(s);
					if (lstLog.Items.Count > 1) lstLog.SelectedIndex = (lstLog.Items.Count - 1);
				}
				finally { lstLog.EndUpdate(); }
			});

			this.e_runInUIThread(addRowInUIThread);
		}
	}
}
