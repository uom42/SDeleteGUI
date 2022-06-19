global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Windows.Forms;

global using uom.Extensions;

using NLog;

#nullable enable

namespace SDeleteGUI
{
	internal static class Program
	{
		private static Lazy<Logger> _logger = new(() => LogManager.GetCurrentClassLogger());

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmMain());
			}
			catch (Exception ex)
			{
				ex.FIX_ERROR(true);
			}
		}
	}
}
