#nullable enable

using AutoUpdaterDotNET;


namespace uom.Extensions
{

	/// <summary>
	/// https://github.com/ravibpatel/AutoUpdater.NET
	/// </summary>
	internal static class Extensions_AutoUpdaterDotNET
	{

		/*
		Thread BackgroundThread = new(CheckForUpdates) { IsBackground = true };
		BackgroundThread.SetApartmentState(System.Threading.ApartmentState.STA);
		BackgroundThread.Start();
		 */

		public static void e_StartAutoupdateOnShown(this Form f, string updaterXML, bool sync = true, bool runUpdateAsAdmin = false)
		{
			AutoUpdater.Synchronous = sync;
			AutoUpdater.RunUpdateAsAdmin = false;
			AutoUpdater.ShowSkipButton = false;
			//AutoUpdater.SetOwner(this);
			f.Shown += async (_, _) => await CheckForAppUpdates(updaterXML);
		}

		private async static Task CheckForAppUpdates(string updaterXML)
		{
			void cbCkeckUpdates() { AutoUpdater.Start(updaterXML); }
			await Task.Run(cbCkeckUpdates);
		}
	}
}