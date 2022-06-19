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
			return $"{_di.Name.Substring(0, 2)} {_di.DriveFormat}"
				+ (string.IsNullOrWhiteSpace(_di.VolumeLabel)
					? string.Empty
					: ($" ({_di.VolumeLabel.Trim()})"))
					+ $" {_di.TotalSize.e_FormatByteSize_Win32()} ({_di.AvailableFreeSpace.e_FormatByteSize_Win32()} free)";
		}


		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static LogDisk[] GetDisks()
		{
			var eee = DriveInfo
				.GetDrives()
				.Select(di =>
					{
						LogDisk? d = null;
						try
						{
							d = new LogDisk(di);
							string ddd = d.ToString();
						}
						catch { d = null; }
						return d;
					}
					)
				.Where(d => d != null)
				.Select(d => d!)
				.ToArray();

			return eee;
		}

		public static async Task<LogDisk[]> GetDisksAsync()
		{
			using Task<LogDisk[]> tskGetDisks = new(() => GetDisks(), TaskCreationOptions.LongRunning);
			tskGetDisks.Start();
			return await tskGetDisks;
		}
	}
}
