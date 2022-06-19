#nullable enable


namespace SDeleteGUI.Core.SDelete
{

	internal class DataReceivedEventArgsEx(string data) : System.EventArgs()
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


		public readonly string RAWData = data;


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
				foreach (MethodInfo miTryParse in _localizedOutputParsers.Value)
				{
					object[] input = [rawDada, null];
					if ((bool)miTryParse.Invoke(null, input))
					{
						dreax = (DataReceivedEventArgsEx)input[1];
						break;
					}
				}
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
