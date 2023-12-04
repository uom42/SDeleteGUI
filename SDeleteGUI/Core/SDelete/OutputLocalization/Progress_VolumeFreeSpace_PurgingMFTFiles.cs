#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Purging MFT files 4% complete
	internal class Progress_VolumeFreeSpace_PurgingMFTFiles(string raw, uint currentOperationProgressPercent) : Progress_BaseEventArgs(raw, currentOperationProgressPercent)
	{

		private const string C_PREFIX = "Purging MFT files ";

		private static readonly Regex _rx
			= new(@"^Purging \s MFT \s files \s (?<PercentProgress>\d+)%",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public static bool TryParse(string raw, out Progress_VolumeFreeSpace_PurgingMFTFiles? piea)
		{
			piea = null;


			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			//Parse Progress Value
			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			uint currentOperationProgressPercent = uint.Parse(rGroups["PercentProgress"].Value);

			piea = new(raw, currentOperationProgressPercent);
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_VOLUME_PURGING_MFT_FILES;

			return localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(CurrentOperationProgressPercent);
		}
	}
}
