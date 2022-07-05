global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Windows.Forms;

global using uom.Extensions;

using System.Text;


#nullable enable

namespace SDeleteGUI
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//TEST(); return;

			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}



		private static void TEST()
		{
			string s = @"Cleaning Cleaning MFT...-
Cleaning MFT...\
Cleaning MFT...|";
			var a = s.e_SplitToLines().ToArray();

			string old = "";
			foreach (var s2 in a)
			{
				var eq = s2.e_GetStringsEquality(old);
				old = s2;
			}
		}
	}
}
