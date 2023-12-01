#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Pass 0 (step 1/3) progress: 92% (99.23 MB/s)
	internal class Progress_PhyDisk_ProgressEventArgs : Progress_BaseEventArgs
	{
		private const string C_PREFIX = "Pass";

		private static readonly Regex _rx
			= new(@"^Pass\s(?<PassCount>\d+)\s (\(step \s (?<StepCurrent>\d+) \/ (?<StepsTotal>\d+) \)\s)? progress\:\s(?<PercentProgress>\d+)\%\s\((?<SpeedValue>\d+\.\d\d)\s(?<SpeedUnits>\w+\/s)\)",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

		private const double ESTIMATION_MIN_PERCENT = 1d;
		private const double ESTIMATION_MIN_SECONDS_SPENT = 5d;
		//private string _estimated = string.Empty;

		public readonly uint PassNumber;
		public readonly float SpeedValue;
		public readonly string SpeedUnits;

		public readonly int StepCurrent;
		public readonly int StepsTotal;


		internal Progress_PhyDisk_ProgressEventArgs(string raw, uint passCount, uint currentOperationProgressPercent, float speedValue, string speedUnits, int stepCurrent, int stepsTotal) : base(raw, currentOperationProgressPercent)
		{
			PassNumber = passCount;
			SpeedValue = speedValue;
			SpeedUnits = speedUnits;

			StepCurrent = stepCurrent;
			StepsTotal = stepsTotal;
		}


		public static bool TryParse(string raw, out Progress_PhyDisk_ProgressEventArgs? piea)
		{
			piea = null;


			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			//Parse Progress Value
			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			uint passNumber = uint.Parse(rGroups["PassCount"].Value) + 1;
			uint progressPercent = uint.Parse(rGroups["PercentProgress"].Value);

			string speedString = rGroups["SpeedValue"].Value;
			speedString = speedString.Replace(".", ",");
			float speedValue = float.Parse(speedString, System.Globalization.NumberStyles.Float);
			string speedUnits = rGroups["SpeedUnits"].Value ?? "";

			int stepCurrent = rGroups.e_ParseRegexValueAsNumeric<int>("StepCurrent", -1);
			int stepsTotal = rGroups.e_ParseRegexValueAsNumeric<int>("StepsTotal", -1);

			piea = new(raw, passNumber, progressPercent, speedValue, speedUnits, stepCurrent, stepsTotal);
			return true;
		}


		private bool IsVersion_2_05_Output => (StepCurrent > 0 && StepsTotal > 0);

		public float TotalPercent
		{
			get
			{
				float p = CurrentOperationProgressPercent;
				if (StepCurrent > 0 && StepsTotal > 1 && CurrentOperationProgressPercent > 0)
				{
					//for SDelete version 2.05: Recalculation total progress over all steps
					float maxPercent = 100f * (float)StepsTotal;
					float currentPercent = ((StepCurrent - 1) * 100f) + CurrentOperationProgressPercent;
					p = (currentPercent / maxPercent) * 100f;
				}
				return p;
			}
		}

		protected override void RecalculateEstimation()
		{
			/*
			_estimated = string.Empty;

			base.RecalculateEstimation();
			if (CurrentOperationProgressPercent < ESTIMATION_MIN_PERCENT || CurrentOperationProgressPercent >= 100d) return;
			double timeSpent = (DateTime.Now - Timestamp).TotalSeconds;
			if (timeSpent < ESTIMATION_MIN_SECONDS_SPENT) return;


			double progressPercent = TotalProgress;
			double progressPercentPerSecond = (progressPercent) / timeSpent;
			double progressPercentLeave = (100d - progressPercent);
			double dblSecondsLeave = (progressPercentLeave / progressPercentPerSecond).e_Round(0);
			if (dblSecondsLeave < 1d) return;

			dblSecondsLeave *= 1000d;

			if (dblSecondsLeave <= (double)UInt32.MaxValue)
			{
				int iSecondsLeave = (int)dblSecondsLeave;
				string sSecondsLeave = uom.WinAPI.Shell.StrFromTimeInterval(iSecondsLeave);
				_estimated = $". {Localization.Strings.M_ESTIMATED}: {sSecondsLeave}";
			}
			 */
		}


		/// <summary>Pass 0 progress: 20% (80.61 MB/s)</summary>
		public override string ToString()
		{
			string localizedFormat = !IsVersion_2_05_Output
				? Localization.Strings.M_OUTPUT_LOCALIZATION_PROGRESS_PERCENT
				: Localization.Strings.M_OUTPUT_LOCALIZATION_PROGRESS_PERCENT_2_05;

			//string progressPercent = CurrentOperationProgressPercent.ToString("N1").Trim();
			var s = localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(PassNumber, CurrentOperationProgressPercent, SpeedValue, SpeedUnits, StepCurrent, StepsTotal);

			return s;// + _estimated;
		}
	}
}
