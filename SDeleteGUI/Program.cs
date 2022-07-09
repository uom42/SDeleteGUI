global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Windows.Forms;

global using uom.Extensions;

using System.Text;
using System.Threading;

using NLog;




#nullable enable

namespace SDeleteGUI
{
	internal static class Program
	{

		private static Lazy<Logger> _logger = new(LogManager.GetCurrentClassLogger());

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			//TEST(); return;

			try
			{
				Application.SetHighDpiMode(HighDpiMode.SystemAware);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmMain());
			}
			catch (Exception ex)
			{
				ex.FIX_ERROR(true);
			}
		}



		private static void TEST()
		{

			try
			{
				string MUTEX_NAME = $"{Application.ProductName}-fsd fsdf:asda sd";

				using (Mutex mtx = new(true, MUTEX_NAME, out bool isNewMutex))
				{

					//mtx.WaitOne();
					try
					{

						using (Mutex mtx2 = new(true, MUTEX_NAME, out bool newMutex2))
						{
							mtx2.WaitOne();
							Thread.Sleep(2000);
							mtx2.ReleaseMutex();
						}

					}
					finally { mtx.ReleaseMutex(); }
				}
			}
			catch (Exception ex)
			{
				ex.FIX_ERROR(true);
			}
		}
	}
}
