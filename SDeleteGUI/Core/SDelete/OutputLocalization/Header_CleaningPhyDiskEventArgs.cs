#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	//Cleaning disk 6:
	internal class Header_CleaningPhyDiskEventArgs(string raw, uint diskNumber) : DataReceivedEventArgsEx(raw)
	{

		private const string C_PREFIX = "Cleaning disk ";

		private static readonly Regex _rx = new(@"Cleaning \s disk \s (?<DiskNumber>\d+)\:",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

		public readonly uint DiskNumber = diskNumber;


		public static bool TryParse(string raw, out Header_CleaningPhyDiskEventArgs? piea)
		{
			piea = null;

			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			piea = new(raw, uint.Parse(rGroups["DiskNumber"].Value));
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_PHY_DISK_NO;
			return localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(DiskNumber);
		}

	}
}
