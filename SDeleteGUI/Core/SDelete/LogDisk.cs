#nullable enable

namespace SDeleteGUI.Core.SDelete
{
	internal class LogDisk
	{
		private readonly DriveInfo _di;

		private LogDisk(DriveInfo di) { _di = di; }

		public char DiskLetter => ToString()[0];

		public override string ToString()
		{
			string label = string.IsNullOrWhiteSpace(_di.VolumeLabel)
				? string.Empty
				: ($" ({_di.VolumeLabel.Trim()})");

			string size = $"{_di.TotalSize.e_FormatByteSize_Win32()} ({_di.AvailableFreeSpace.e_FormatByteSize_Win32()} free)";

			return $"{_di.Name.Substring(0, 2)} {_di.DriveFormat}{label} {size}";
			//var _displayName = $@"{Index}: {Model}, {InterfaceType} ({Size!.e_FormatByteSize_Win32()}), {Partitions} partition(s)";
		}



		private static LogDisk[] GetDisks()
			=> DriveInfo.GetDrives().Select(di => new LogDisk(di)).ToArray();


		public static async Task<LogDisk[]> GetDisksAsync()
		{
			using Task<LogDisk[]> tskGetDisks = new(() => GetDisks(), TaskCreationOptions.LongRunning);
			tskGetDisks.Start();
			return await tskGetDisks;
		}
	}
}
