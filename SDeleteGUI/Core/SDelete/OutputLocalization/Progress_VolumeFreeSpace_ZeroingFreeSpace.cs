#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Zeroing free space on T:\: 0%
	internal class Progress_VolumeFreeSpace_ZeroingFreeSpace(string raw, string volumeName, uint progressPercent) : Progress_BaseEventArgs(raw, progressPercent)
	{

		private const string C_PREFIX = "Zeroing free space on ";

		private static readonly Regex _rx
			= new(@"^Zeroing \s free \s space \s on \s (?<VolumeName>.+) \:\s (?<PercentProgress>\d+)%",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public readonly string VolumeName = volumeName;


		public static bool TryParse(string raw, out Progress_VolumeFreeSpace_ZeroingFreeSpace? piea)
		{
			piea = null;



			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			//Parse Progress Value
			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			string volumeName = rGroups["VolumeName"].Value;
			uint progressPercent = uint.Parse(rGroups["PercentProgress"].Value);

			piea = new(raw, volumeName, progressPercent);
			return true;
		}


		public override string ToString()
		{

			var ddd = this.VolumeName;


			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_VOLUME_ZEROING_PROGRESS;
			return localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(VolumeName, CurrentOperationProgressPercent);
		}
	}
}
