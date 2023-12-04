#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Pass 0 (step 1/3) progress: 92% (99.23 MB/s)
	internal class Progress_PhyDisk_ProgressEventArgs(string raw, uint passCount, uint currentOperationProgressPercent, float speedValue, string speedUnits, int stepCurrent, int stepsTotal)
		: Progress_BaseEventArgs(raw, currentOperationProgressPercent)
	{
		private const string C_PREFIX = "Pass";

		private static readonly Regex _rx
			= new(@"^Pass\s(?<PassCount>\d+)\s (\(step \s (?<StepCurrent>\d+) \/ (?<StepsTotal>\d+) \)\s)? progress\:\s(?<PercentProgress>\d+)\%\s\((?<SpeedValue>\d+\.\d\d)\s(?<SpeedUnits>\w+\/s)\)",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public readonly uint PassNumber = passCount;
		public readonly float SpeedValue = speedValue;
		public readonly string SpeedUnits = speedUnits;

		public readonly int StepCurrent = stepCurrent;
		public readonly int StepsTotal = stepsTotal;


		private bool IsVersion_2_05_Output => (StepCurrent > 0 && StepsTotal > 0);


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


		public override string ToString()
		{
			string localizedFormat = !IsVersion_2_05_Output
				? Localization.Strings.M_OUTPUT_LOCALIZATION_PROGRESS_PERCENT
				: Localization.Strings.M_OUTPUT_LOCALIZATION_PROGRESS_PERCENT_2_05;

			var s = localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(PassNumber, CurrentOperationProgressPercent, SpeedValue, SpeedUnits, StepCurrent, StepsTotal);
			return s;
		}
	}
}
