using System.ComponentModel;
using System.Drawing;

#nullable enable

namespace SDeleteGUI.Core
{

	/// <summary>Flicker-free ListBox</summary>
	[ToolboxItem(true)]
	public class ListBoxEx : System.Windows.Forms.ListBox
	{
		protected string emptyText = string.Empty;
		protected ContentAlignment emptyTextAlign = ContentAlignment.MiddleCenter;

		public ListBoxEx() : base()
		{
			//SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

			//Activate double buffering
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

			//Enable the OnNotifyMessage event so we get a chance to filter out 
			// Windows messages before they get to the form's WndProc
			this.SetStyle(ControlStyles.EnableNotifyMessage, true);
		}


		protected override CreateParams CreateParams
		{
			get
			{
				const int WS_EX_COMPOSITED = 0x02000000;
				CreateParams cp = base.CreateParams;
				//cp.ExStyle |= WS_EX_COMPOSITED;
				return cp;
			}
		}


		protected override void OnNotifyMessage(Message m)
		{
			//Filter out the WM_ERASEBKGND message
			if (m.Msg != 0x14)
			{
				base.OnNotifyMessage(m);
			}
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}



		private void LockWindow(Control ctl)
		{
			//  DIALOG SEND ghDlg, %WM_SETREDRAW,%FALSE,0 'do not allow messages
			uom.WinAPI.Windows.SendMessage(ctl.Handle, uom.WinAPI.Windows.WindowMessages.WM_SETREDRAW, (int)0, 0);
			ClearBuffers();
		}

		private void UnlockWindow(Control ctl)
		{
			ClearBuffers();//do not use DIALOG ENABLE or DIALOG DISABLE
			uom.WinAPI.Windows.SendMessage(ctl.Handle, uom.WinAPI.Windows.WindowMessages.WM_SETREDRAW, (int)-1, 0);
			/*
  DIALOG SEND ghDlg, % WM_SETREDRAW,% TRUE,0  'allow messages
  DIALOG REDRAW ghDlg   'required for scrollbars to refresh  
			 */

			//RedrawWindow(hWnd, NULL, NULL, RDW_ERASE | RDW_FRAME | RDW_INVALIDATE | RDW_ALLCHILDREN) для перерисовки списка.
		}

		private void ClearBuffers()
		{
			//http://stackoverflow.com/questions/2400332/how-to-clear-mouse-click-buffer


			//call just before exit sub/function to prevent re-entry by stray input
			/*  
  LOCAL SystemMsg AS TagMsg

  WHILE PeekMessage(SystemMsg,% NULL,% WM_KEYFIRST,% WM_KEYLAST, % PM_REMOVE OR % PM_NOYIELD):WEND
  WHILE PeekMessage(SystemMsg,% NULL,% WM_MOUSEFIRST,% WM_MOUSELAST, % PM_REMOVE OR % PM_NOYIELD) :WEND
			 */
		}


	}
}