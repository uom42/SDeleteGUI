using System;
using System.Windows.Forms;

using SDeleteGUI.Core;

namespace SDeleteGUI
{
	internal  partial class frmMain : Form
	{

		public frmMain()
		{
			InitializeComponent();

			this.Load += (_, _) => _Load();
		}

		SDeleteManager? sdmgr = null;

		private void _Load()
		{
			try
			{
				sdmgr = new SDeleteManager();
				txtSDeleteBinPath.Text = sdmgr.Binary.FullName;
			}
			catch (Exception ex)
			{
				txtSDeleteBinPath.Text = ex.Message ;
				return;
			}
		}


		
	 
	}
}
