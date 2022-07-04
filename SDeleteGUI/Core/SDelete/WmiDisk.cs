using System.Management;

namespace SDeleteGUI.Core.SDelete
{
	internal class WmiDisk
	{
		public readonly string DeviceID;
		public readonly string PNPDeviceID;

		public readonly uint Index;

		public readonly string Model;

		public readonly ulong Size;
		public readonly uint? Partitions;



		public readonly string FirmwareRevision;
		public readonly string SerialNumber;

		public readonly string InterfaceType;
		public readonly ushort? SCSIPort;
		public readonly string MediaType;

		private WmiDisk(ManagementObject mo)
		{
			/*
		  Get-PhysicalDisk | FT

			Number FriendlyName              SerialNumber         MediaType CanPool OperationalStatus HealthStatus Usage            Size
			------ ------------              ------------         --------- ------- ----------------- ------------ -----            ----
			3      Samsung SSD 980 PRO 500GB 0025_38BC_0151_74DB. SSD       True    OK                Healthy      Auto-Select 465.76 GB
			4      ASMT USB3.0               100000000000         HDD       False   OK                Healthy      Auto-Select 465.76 GB
			0      WDC WD20EFAX-68B2RN1      WD-WXJ2A31DDK24      HDD       False   OK                Healthy      Auto-Select   1.82 TB
			2      ST1000NM0033-9ZM173       Z1W34FLQ             HDD       False   OK                Healthy      Auto-Select 931.51 GB
			1      Samsung SSD 870 EVO 1TB   S5Y2NF0RA07234W      SSD       True    OK                Healthy      Auto-Select 931.51 GB
											  		 
		 */

			DeviceID = mo.ReadProperty_Str("DeviceID");
			PNPDeviceID = mo.ReadProperty_Str("PNPDeviceID");


			Index = mo.ReadProperty_uint("Index") ?? 0;
			Size = mo.ReadProperty_ulong("Size") ?? 0;

			Model = mo.ReadProperty_Str("Model");

			FirmwareRevision = mo.ReadProperty_Str("FirmwareRevision");
			SerialNumber = mo.ReadProperty_Str("SerialNumber");

			InterfaceType = mo.ReadProperty_Str("InterfaceType");
			SCSIPort = mo.ReadProperty_T<ushort>("SCSIPort");
			MediaType = mo.ReadProperty_Str("MediaType");

			Partitions = mo.ReadProperty_uint("Partitions");

#if DEBUG
			var aaa = mo.GetAllProperties();
#endif
			var T = 9;
		}


		public override string ToString()
		{
			var _displayName = $"{Index} {Model}, {InterfaceType} ({Size!.e_FormatByteSize_Win32()}), Partitions: {Partitions}";
			return _displayName.Replace("  ", " ").Trim();
		}


		private static WmiDisk[] GetDisks()
		{
			WqlObjectQuery query = new("SELECT * FROM Win32_DiskDrive");
			using (ManagementObjectSearcher searcher = new(query))
			{
				var result = searcher.Get()
									 .OfType<ManagementObject>()
									 .Select(o => new WmiDisk(o))
									 .OrderBy(d => d.Index)
									 .ToArray();
				return result;
			}
		}

		public static async Task<WmiDisk[]> GetDisksAsync()
		{
			//using Task<WmiDisk[]> tskGetDisks = Task.Run(() => GetDisks());
			using Task<WmiDisk[]> tskGetDisks = new(() => GetDisks(), TaskCreationOptions.LongRunning);
			tskGetDisks.Start();
			return await tskGetDisks;
		}
	}
}
