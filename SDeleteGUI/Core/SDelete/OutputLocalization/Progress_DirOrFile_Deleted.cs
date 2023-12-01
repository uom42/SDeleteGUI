#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{

	/*
	T:\file.txt...deleted.
	T:\folder...deleted.
	 */
	internal class Progress_DirOrFile_Deleted : DataReceivedEventArgsEx
	{
		private const string C_SUFFIX = "...deleted.";

		public readonly string FileSystemObject;


		internal Progress_DirOrFile_Deleted(string raw, string fileSystemObject) : base(raw)
			=> FileSystemObject = fileSystemObject;


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
