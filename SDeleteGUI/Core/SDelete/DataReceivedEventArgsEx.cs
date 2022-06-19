#nullable enable


namespace SDeleteGUI.Core.SDelete
{

	internal class DataReceivedEventArgsEx : System.EventArgs
	{

		#region OutputLocalization


		private const string C_METHOD_TryParse = "TryParse";
		//Get all classes from 'SDeleteGUI.Core.SDelete.OutputLocalization' namespace which have static|public TryParse() method
		private static Lazy<MethodInfo[]> _localizedOutputParsers = new(() =>
			Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(
				t =>
					t.Namespace == typeof(OutputLocalization.Progress_BaseEventArgs).Namespace
						&& !t.IsAbstract
						&& t.GetMember(C_METHOD_TryParse, BindingFlags.Static | BindingFlags.Public).Any()
				)
			.Select(t => (System.Reflection.MethodInfo)t.GetMember(C_METHOD_TryParse, BindingFlags.Static | BindingFlags.Public).First())
			.ToArray());

		#endregion


		public DateTime Timestamp { get; private set; } = DateTime.Now;
		public readonly string RAWData = string.Empty;


		public DataReceivedEventArgsEx(string data) : base()
		{
			this.RAWData = data;
		}


		public void UpdateTimestamp(DateTime newTimestamp)
		{
			Timestamp = newTimestamp;
			RecalculateEstimation();
		}


		protected virtual void RecalculateEstimation() { }


		public override string ToString() => RAWData;


		public static DataReceivedEventArgsEx Parse(DataReceivedEventArgs e)
		{
			string rawDada = e.Data;

#if DEBUG
			Debug.Write($"Parsing RAW '{rawDada}'... ");
#endif
			DataReceivedEventArgsEx? dreax = null;
			try
			{

				var lop = _localizedOutputParsers.Value;
				foreach (var miTryParse in lop)
				{
					var input = new object[] { rawDada, null };
					if ((bool)miTryParse.Invoke(null, input))
					{
						dreax = (DataReceivedEventArgsEx)input[1];
						break;
					}
				}

				/* OLD
				if (OutputLocalization.Header_CleaningPhyDiskEventArgs.TryParse(rawDada, out var hdr_pd))
					dreax = hdr_pd;
				else if (OutputLocalization.Header_PassInfoEventArgs.TryParse(rawDada, out var hdr_pi))
					dreax = hdr_pi;
				else if (OutputLocalization.Progress_VolumeFreeSpace_ZeroingFreeSpace.TryParse(rawDada, out var vol_pi))
					dreax = vol_pi;
				else if (OutputLocalization.Progress_PhyDisk_ProgressEventArgs.TryParse(rawDada, out var pri))
					dreax = pri;
				 */

			}
			catch (Exception ex)
			{
#if DEBUG
				Debug.WriteLine($"*** ERROR DataReceivedEventArgsEx.Parse: {ex}");
#endif
			}

			dreax ??= new DataReceivedEventArgsEx(rawDada);
#if DEBUG
			Debug.WriteLine(dreax.ToString());
#endif
			return dreax!;
		}
	}





}
