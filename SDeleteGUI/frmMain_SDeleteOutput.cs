
using SDeleteGUI.Core.SDelete;

using static SDeleteGUI.Core.SDelete.SDeleteManager;


#nullable enable

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private void SDeleteEngine_AttachEvents()
		{
			_sdmgr!.OutputRAW += OnCore_Data_RAW;
			_sdmgr!.Error += OnCore_Error;
			_sdmgr!.Finished += OnFinished;
		}

		private void SDeleteEngine_DetachEvents()
		{
			_sdmgr!.OutputRAW -= OnCore_Data_RAW;
			_sdmgr!.Error -= OnCore_Error;
			_sdmgr!.Finished -= OnFinished;
		}

		private void OnFinished(object _, EventArgs a) => this.e_RunInUIThread(() => OnFinished());

		private void OnCore_Data_RAW(object _, DataReceivedEventArgsEx e) => LogAddRowMTSafe(e);

		private void OnCore_Error(object _, DataReceivedEventArgs e) => LogAddErrorMTSafe(e);

		//private void OnCore_Data_Progress(object _, ProgressInfoEventArgs e) => this.e_runInUIThread(() => ProgressBarSetState_Progress(e.ProgressPercent));


		private const int C_MAX_LOG_ROWS = 1000;

		private void LogAddErrorMTSafe(DataReceivedEventArgs? e)
		{
			string? s = e?.Data;
			if (s.e_IsNullOrEmpty()) return;//Skip empty lines


			s = "!ERROR STREAM!: " + s;
			Debug.WriteLine($"Output Data: '{s}'");

			void addRowInUIThread()
			{
				//Limit count of lines to limit
				while (lstLog.Items.Count >= C_MAX_LOG_ROWS) lstLog.Items.RemoveAt(0);

				lstLog.Items.Add(s);
				if (lstLog.Items.Count > 1) lstLog.SelectedIndex = (lstLog.Items.Count - 1);
			}
			lstLog.e_runOnLockedUpdateMTSafe(addRowInUIThread);
		}

		private void LogAddRowMTSafe(DataReceivedEventArgsEx? e)
		{
			string? s = e?.Data;
			if (string.IsNullOrWhiteSpace(s)) return;//Skip empty lines

			Debug.WriteLine($"Output Data: '{s}'");

			void addRowInUIThread()
			{
				try
				{
					object? lastRow = lstLog.Items.Cast<Object>().LastOrDefault();
					if (lastRow != null)
					{
						//We will search for string equality with previous log line						//and if we found - we just replace text in last report line
						const int MIN_EQUAL_CHARS = 3;
						const int MIN_EQUAL_WORDS = 2;

						switch (lastRow)
						{
							case DataReceivedEventArgsEx dreax:
								{
									//Previous Row is DataReceivedEventArgsEx!
									var eq = s!.e_GetEqualityMetrics(dreax.Data);

									if ((eq.CommonPrefix == s)
									|| (eq.CommonPrefix.Length >= MIN_EQUAL_CHARS && eq.UniqueWordsInBothStrings.Length >= MIN_EQUAL_WORDS))
									{
										//Looks like the some string as previous with some changes... Just update last row

										e!.UpdateTimestamp(dreax.Timestamp);
										lstLog.Items[lstLog.Items.Count - 1] = e;
										return;
									}
									break;
								}
						}

					}

					//No found equality. Just add new row to log
					lstLog.Items.Add(e!);
					if (lstLog.Items.Count > 1) lstLog.SelectedIndex = (lstLog.Items.Count - 1);
				}
				finally
				{
					lstLog.e_LimitRowsCountTo(C_MAX_LOG_ROWS);
					if (e!.ProgressInfo != null) ProgressBarSetState_Progress(e.ProgressInfo.ProgressPercent);
				}

			}

			//this.e_runInUIThread(addRowInUIThread);
			lstLog.e_runOnLockedUpdateMTSafe(addRowInUIThread);
		}
	}
}
