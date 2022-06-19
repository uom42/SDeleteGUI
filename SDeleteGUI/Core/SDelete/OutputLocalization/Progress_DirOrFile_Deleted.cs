#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	/*
	T:\folder...deleted.
	T:\file.txt...deleted.
	 */
	internal class Progress_DirOrFile_Deleted(string raw, string fileSystemObject) : DataReceivedEventArgsEx(raw)
	{
		private const string C_SUFFIX = "...deleted.";

		public readonly string FileSystemObject = fileSystemObject;


		public static bool TryParse(string raw, out Progress_DirOrFile_Deleted? piea)
		{
			piea = null;

			if (!(raw.EndsWith(C_SUFFIX, StringComparison.InvariantCultureIgnoreCase) && (raw.Length >= C_SUFFIX.Length))) return false;
			string fileSystemObject = raw.Substring(0, raw.Length - C_SUFFIX.Length);

			piea = new(raw, fileSystemObject);
			return true;
		}


		public override string ToString()
		{
			string localizedFormat = Localization.Strings.M_OUTPUT_LOCALIZATION_FSO_DELETED;
			return localizedFormat.e_IsNullOrWhiteSpace()
					? RAWData
					: localizedFormat.e_Format(FileSystemObject);
		}
	}
}
