
using SDeleteGUI.Core.SDelete;


#nullable enable

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{
		private const int C_MAX_LOG_ROWS = 1000;

		private EventArgs _lastLogRowSync = new();
		private object? _lastLogRow = null;

		private DateTime _dtStarted = DateTime.Now;
		private TimeSpan? _tsElapsed;
		private string _estimatedTimeString = string.Empty;


		private void SDeleteEngine_AttachEvents()
		{
			_sdmgr!.OutputRAW += OnCore_Data_RAW;
			_sdmgr!.Error += OnCore_Error;
			_sdmgr!.Finished += OnFinished;

			_lastLogRow = null;
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
			string? s = e?.RAWData;
			if (string.IsNullOrWhiteSpace(s)) return;//Skip empty lines

			Debug.WriteLine($"Output Data: '{s}'");

			void addRowInUIThread()
			{
				try
				{
					lock (_lastLogRowSync)
					{
						if (_lastLogRow != null)
						{
							switch (_lastLogRow)
							{
								case DataReceivedEventArgsEx dreaxlastLogRow:
									{
										//Previous Row is DataReceivedEventArgsEx
										bool rowLooksLikePrevious = false;
										if (chkCompactOutput.Checked)
										{
											if (e!.GetType().FullName != dreaxlastLogRow.GetType().FullName)
											{
												rowLooksLikePrevious = false;
											}
											else if (e!.GetType().Namespace == typeof(SDeleteGUI.Core.SDelete.OutputLocalization.Progress_BaseEventArgs).Namespace)
											{
												rowLooksLikePrevious = true;
											}

											/*
											const string C_OUTPUT_DELETED = ".deleted.";
							//We will search for string equality with previous log line						//and if we found - we just replace text in last report line
							const int MIN_EQUAL_CHARS = 3;
							const int MIN_EQUAL_WORDS = 2;

											else if ((_SourceMode is SourceModes.Dir or SourceModes.Files)
												&& s!.EndsWith(C_OUTPUT_DELETED, StringComparison.InvariantCultureIgnoreCase) && dreaxlastLogRow.RAWData.EndsWith(C_OUTPUT_DELETED, StringComparison.InvariantCultureIgnoreCase))
											{
												//Both current and previous rows EndsWith("deleted.")
												rowLooksLikePrevious = true;
											}
											else
											{
												//var eq = s!.e_GetEqualityMetrics(dreaxlastLogRow.RAWData);
												//rowLooksLikePrevious = ((eq.CommonPrefix == s) || (eq.CommonPrefix.Length >= MIN_EQUAL_CHARS && eq.UniqueWordsInBothStrings.Length >= MIN_EQUAL_WORDS));
											}
											 */
										}

										if (rowLooksLikePrevious)
										{
											//Looks like the some string as previous with some changes... Just update last row
											//e!.UpdateTimestamp(dreaxlastLogRow.Timestamp);
											lstLog.e_SetLastItem(e);
											_lastLogRow = e;
											return;
										}
										break;
									}
							}
						}
					}

					//Add new row to log
					lstLog.Items.Add(e!);
					_lastLogRow = e!;
					lstLog.e_SelectLastRow();
				}
				finally
				{
					lstLog.e_LimitRowsCountTo(C_MAX_LOG_ROWS);

					_estimatedTimeString = string.Empty;
					float totalProgress = -1f;

					switch (e)
					{
						case SDeleteGUI.Core.SDelete.OutputLocalization.Progress_PhyDisk_ProgressEventArgs pdp:
							{
								totalProgress = pdp.TotalPercent;

								if (totalProgress > 0 && _sdmgr!.Passes > 1 && pdp.PassNumber > 0)
								{
									//Recalculation total progress over all stages
									float maxPercent = 100f * (float)_sdmgr!.Passes;
									float currentPercent = ((pdp.PassNumber - 1) * 100f) + totalProgress;
									totalProgress = (currentPercent / maxPercent) * 100f;
								}

								_estimatedTimeString = $". Всего выполнено: {totalProgress:N2}%";

								//Recalculating Estimation
								const double ESTIMATION_MIN_SECONDS_SPENT = 10d;
								const double ESTIMATION_MIN_PERCENT = .1d;

								double timeSpent = (DateTime.Now - _dtStarted).TotalSeconds;
								if (timeSpent >= ESTIMATION_MIN_SECONDS_SPENT && totalProgress >= ESTIMATION_MIN_PERCENT)
								{
									//double progressPercent = totalProgress;
									double progressPercentPerSecond = (totalProgress) / timeSpent;
									double progressPercentLeave = (100d - totalProgress);
									double dblSecondsLeave = (progressPercentLeave / progressPercentPerSecond).e_Round(0);
									if (dblSecondsLeave >= 1d)
									{
										dblSecondsLeave *= 1000d;

										if (dblSecondsLeave <= (double)UInt32.MaxValue)
										{
											uint iSecondsLeave = (uint)dblSecondsLeave;
											string sSecondsLeave = uom.WinAPI.Shell.StrFromTimeInterval(iSecondsLeave);
											_estimatedTimeString += $". {Localization.Strings.M_ESTIMATED}: {sSecondsLeave}";
										}
									}
								}

								break;
							}
						case SDeleteGUI.Core.SDelete.OutputLocalization.Progress_BaseEventArgs pb:
							{
								totalProgress = pb.CurrentOperationProgressPercent;
								break;
							}
					}

					if (totalProgress >= 0) ProgressBarSetState_Progress(totalProgress);
				}

			}

			lstLog.e_runOnLockedUpdateMTSafe(addRowInUIThread);
		}


		private void OnElapsedTimerTick()
		{
			string s;
			if (_isRunning)
			{
				_tsElapsed = DateTime.Now - _dtStarted;
				s = string.Format(Localization.Strings.M_ELAPSED, _tsElapsed.Value.e_ToShellTimeString(8))
					+ _estimatedTimeString;
			}
			else
			{
				s = !_tsElapsed.HasValue
					? ""
					: string.Format(Localization.Strings.M_COMPLETED_AT, _tsElapsed.Value.e_ToShellTimeString(8));
			}
			lblStatus.Text = s;
		}
	}
}
