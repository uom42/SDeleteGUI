#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Cleaning free space on T:\: 1%
	internal class Progress_VolumeFreeSpace_CleaningFreeSpace(string raw, string volumeName, uint currentOperationProgressPercent) : Progress_BaseEventArgs(raw, currentOperationProgressPercent)
	{

		private const string C_PREFIX = "Cleaning free space on ";

		private static readonly Regex _rx
			= new(@"^Cleaning \s free \s space \s on \s (?<VolumeName>.+) \:\s (?<PercentProgress>\d+)%",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public readonly string VolumeName = volumeName;


		public static bool TryParse(string raw, out Progress_VolumeFreeSpace_CleaningFreeSpace? piea)
		{
			piea = null;


			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			string volumeName = rGroups["VolumeName"].Value;
			uint currentOperationProgressPercent = uint.Parse(rGroups["PercentProgress"].Value);

			piea = new(raw, volumeName, currentOperationProgressPercent);
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_VOLUME_CLEANING_PROGRESS;
			return localizedFormat.e_IsNullOrWhiteSpace()
			? RAWData
			: localizedFormat.e_Format(VolumeName, CurrentOperationProgressPercent);
		}
	}
}
