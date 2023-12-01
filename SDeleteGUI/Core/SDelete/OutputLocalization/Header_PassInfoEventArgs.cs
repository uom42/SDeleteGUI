#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{
	internal class Header_PassInfoEventArgs : DataReceivedEventArgsEx
	{

		private const string C_PREFIX = "SDelete is set for ";

		//SDelete is set for 1 pass.
		private static readonly Regex _rx = new(@".+ set \s for \s (?<PassCount>\d+) \s pass",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);


		public readonly uint PassCount;


		internal Header_PassInfoEventArgs(string raw, uint passCount) : base(raw) => PassCount = passCount;


		public static bool TryParse(string raw, out Header_PassInfoEventArgs? piea)
		{
			piea = null;

			if (!raw.StartsWith(C_PREFIX, StringComparison.InvariantCultureIgnoreCase)) return false;

			Match? mx = _rx.Match(raw.Trim());
			if (!mx?.Success ?? false) return false;

			GroupCollection rGroups = mx!.Groups;
			piea = new(raw, uint.Parse(rGroups["PassCount"].Value));
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_PASS_COUNT;

			return localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(PassCount);
		}

	}
}
