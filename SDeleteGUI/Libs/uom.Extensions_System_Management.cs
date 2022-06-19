#nullable enable

using System.Management;


namespace uom.Extensions
{
	internal static class Extensions_System_Management
	{

		public static (string Name, object Value, PropertyData PropData)[] e_GetAllProperties(this ManagementObject mo)
		{
			var props = mo.Properties
				.Cast<PropertyData>()
				.Select(pd => (pd.Name, pd.Value, pd))
				.OrderBy(t => t.Name)
				.ToArray();
			return props;
		}


		private static object? e_ReadMOProperty(this ManagementObject mo, string name)
		{
			try
			{
#if DEBUG
				var props = mo.Properties
					.Cast<PropertyData>()
					.Select(pd
						=> (Name: pd.Name, PropData: pd, Value: pd.Value)
					)
					.OrderBy(t => t.Name)
					.ToArray();

#endif

				var prop = mo.Properties[name];
				return prop?.Value;
			}
			catch { }
			return null;
		}


		public static string e_ReadMOProperty_Str(this ManagementObject mo, string name)
			=> (e_ReadMOProperty(mo, name)?.ToString() ?? "").Trim();


		public static T? e_ReadMOProperty_T<T>(this ManagementObject mo, string name)
			=> (T?)e_ReadMOProperty(mo, name);


		public static uint? e_ReadMOProperty_uint(this ManagementObject mo, string name)
			=> (e_ReadMOProperty(mo, name) is uint ui) ? ui : null;


		public static ulong? e_ReadMOProperty_ulong(this ManagementObject mo, string name)
			=> (e_ReadMOProperty(mo, name) is ulong ul) ? ul : null;




	}
}