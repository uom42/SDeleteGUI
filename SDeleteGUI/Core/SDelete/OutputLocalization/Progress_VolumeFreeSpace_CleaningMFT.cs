#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	/*
	Cleaning MFT.../
	Cleaning MFT...-
	Cleaning MFT...\
	Cleaning MFT...|
	Cleaning MFT.../
	Cleaning MFT...-
	Cleaning MFT...\
	Cleaning MFT...|	 
	 */
	internal class Progress_VolumeFreeSpace_CleaningMFT(string raw, char progressChar) : DataReceivedEventArgsEx(raw)
	{

		private const string C_PREFIX = "Cleaning MFT...";

		private static readonly Regex _rx
			= new(@"^Cleaning \s MFT \.\.\. (?<ProgressChar>.{1})",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public readonly char ProgressChar = progressChar;


		public static bool TryParse(string raw, out Progress_VolumeFreeSpace_CleaningMFT? piea)
		{
			piea = null;

			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			//Parse Progress Value
			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			char progressChar = ' ';
			string progressCharString = rGroups["ProgressChar"].Value;
			if (progressCharString.e_IsNOTNullOrWhiteSpace()) progressChar = progressCharString[0];

			piea = new(raw, progressChar);
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_VOLUME_CLEANING_MFT;
			return localizedFormat.e_IsNullOrWhiteSpace()
			? RAWData
			: localizedFormat.e_Format(ProgressChar);
		}
	}
}
