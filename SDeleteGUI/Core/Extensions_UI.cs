using System.Management;

#nullable enable

namespace uom.Extensions
{
	internal static class Extensions_UI
	{

		public static void DisabeAndShowBanner(this ComboBox cbo, string bannerText)
		{
			cbo.Items.Clear();
			cbo.Items.Add(bannerText);
			cbo.SelectedIndex = 0;
			cbo.Enabled = false;


		}

		public static void DisabeAndShowError(this ComboBox cbo, Exception ex)
		{
			cbo.Items.Clear();
			cbo.Items.Add(ex.Message);
			cbo.SelectedIndex = 0;
			cbo.Enabled = false;
		}

		public static void FillAndSelectLast(this ComboBox cbo, object[] data, bool enable)
		{
			cbo.Enabled = enable;
			cbo.Items.Clear();
			if (data.Any())
			{
				cbo.Items.AddRange(data);
				cbo.SelectedItem = data.Last();

			}
		}
	}
}