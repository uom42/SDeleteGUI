using System.Management;

#nullable enable

namespace SDeleteGUI.Core.SDelete
{
	internal class Win32_DiskDrive : IEquatable<Win32_DiskDrive?>
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

		private Win32_DiskDrive(ManagementObject mo)
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

			DeviceID = mo.e_ReadMOProperty_Str("DeviceID");
			PNPDeviceID = mo.e_ReadMOProperty_Str("PNPDeviceID");


			Index = mo.e_ReadMOProperty_uint("Index") ?? 0;
			Size = mo.e_ReadMOProperty_ulong("Size") ?? 0;

			Model = mo.e_ReadMOProperty_Str("Model");

			FirmwareRevision = mo.e_ReadMOProperty_Str("FirmwareRevision");
			SerialNumber = mo.e_ReadMOProperty_Str("SerialNumber");

			InterfaceType = mo.e_ReadMOProperty_Str("InterfaceType");
			SCSIPort = mo.e_ReadMOProperty_T<ushort>("SCSIPort");
			MediaType = mo.e_ReadMOProperty_Str("MediaType");

			Partitions = mo.e_ReadMOProperty_uint("Partitions");

#if DEBUG
			var aaa = mo.e_GetAllProperties();
#endif
		}


		public override string ToString()
		{
			var _displayName = $@"#{Index}: {Model} ({InterfaceType}), {Size!.e_FormatByteSize_Win32()}, {Partitions} {Localization.Strings.L_DISK_PARTITIONS}";
			return _displayName.Replace("  ", " ").Trim();
		}

		public override bool Equals(object? obj) => Equals(obj as Win32_DiskDrive);

		public bool Equals(Win32_DiskDrive? other) => (other is not null) && (other!.DeviceID == DeviceID);

		public override int GetHashCode() => DeviceID.GetHashCode();

		/*
public static bool operator ==(WmiDisk d1, WmiDisk d2) => (d1 is null || d2 is null)
? false
: (d1!.DeviceID == d2!.DeviceID);

public static bool operator !=(WmiDisk d1, WmiDisk d2) => !(d1 == d2);
		 */

		/*

		 */

		private static Win32_DiskDrive[] GetDisks()
		{
			WqlObjectQuery query = new("SELECT * FROM Win32_DiskDrive");
			using (ManagementObjectSearcher searcher = new(query))
			{
				var result = searcher.Get()
									 .OfType<ManagementObject>()
									 .Select(o => new Win32_DiskDrive(o))
									 .OrderBy(d => d.Index)
									 .ToArray();
				return result;
			}
		}

		public static async Task<Win32_DiskDrive[]> GetDisksAsync()
		{
			//using Task<Win32_DiskDrive[]> tskGetDisks = new(() => GetDisks(), TaskCreationOptions.LongRunning);
			var f = GetDisks;
			return await f.e_StartAndWaitLongAsync();
		}

	}
}
