using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

using uom.Extensions;


#nullable enable


namespace uom.Extensions
{
	internal static class ExtensionsConsole
	{



		/*						 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string NormalizeConsoleOutput(this string? source)
		{
			if (source.e_IsNullOrWhiteSpace()) return String.Empty;

			StringBuilder sb = new();
			foreach (string s in source.e_SplitToLines(true, false))
				sb.AppendLine(s);

			string sOutput = sb.ToString().Trim();
			return sOutput;
		}
		
		public const string CR = "\n";
		public const string LF = "\r";
		public const string CRLF = CR + LF;


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string NormalizedOutput(this ConsoleOutput co)
			=> co.Output.NormalizeConsoleOutput();
		 */
	}

	/*						 
	

	internal class ConsoleOutput
	{
		public readonly string Output;
		public readonly string Errors;


		internal ConsoleOutput(string? o, string? e)
		{
			Output = o ?? string.Empty;
			Errors = e ?? string.Empty;
		}


		public override string ToString() => $"Output Stream:\n'{Output}',\nErrors Stream: '{Errors}'";


		public void ThrowOutputOrDefault(string defaultError = "Unknown error!")
		{
			string sErr = defaultError;
			if (!Errors.e_IsNOTNullOrWhiteSpace())
				sErr = Errors;
			else
				if (!Output.e_IsNOTNullOrWhiteSpace()) sErr = Output;

			throw new Exception(sErr);
		}
	}
	 */
}
