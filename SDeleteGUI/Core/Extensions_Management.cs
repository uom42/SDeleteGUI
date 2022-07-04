using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using uom.Extensions;

#nullable enable

namespace uom.Extensions
{
	internal static class Extensions_Management
	{

		public static (string Name, object Value, PropertyData PropData)[]
			GetAllProperties(this ManagementObject mo)
		{
			var props = mo.Properties
				.Cast<PropertyData>()
				.Select(pd => (pd.Name, pd.Value, pd))
				.OrderBy(t => t.Name)
				.ToArray();
			return props;
		}

		private static object? ReadProperty(this ManagementObject mo, string name)
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
		public static string ReadProperty_Str(this ManagementObject mo, string name)
			=> (ReadProperty(mo, name)?.ToString() ?? "").Trim();

		public static T? ReadProperty_T<T>(this ManagementObject mo, string name)
			=> (T?)ReadProperty(mo, name);

		public static uint? ReadProperty_uint(this ManagementObject mo, string name)
			=> (ReadProperty(mo, name) is uint ui) ? ui : null;

		public static ulong? ReadProperty_ulong(this ManagementObject mo, string name)
			=> (ReadProperty(mo, name) is ulong ul) ? ul : null;




	}
}