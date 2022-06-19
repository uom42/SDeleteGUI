
using Microsoft.WindowsAPICodePack.Taskbar;

#nullable enable

namespace SDeleteGUI
{
	internal partial class frmMain : Form
	{

		private Lazy<TaskbarManager> _tbm = new(() => TaskbarManager.Instance);


		private void ProgressBarSetState_Marquee()
		{
			pbProgress.e_SetValues();
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
			pbProgress.Style = ProgressBarStyle.Marquee;

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Indeterminate);
		}
		private void ProgressBarSetState_Progress(float progress)
		{
			int iProgress = (int)progress;

			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
			pbProgress.e_SetValues(0, 100, iProgress);
			pbProgress.Style = ProgressBarStyle.Continuous;

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Normal);
			_tbm.Value.SetProgressValue(iProgress, 100);
		}
		private void ProgressBarSetState_Error()
		{
			pbProgress.Style = ProgressBarStyle.Blocks;
			pbProgress.e_SetValues(0, 100, 100);
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_ERROR);
			_tbm.Value.SetProgressState(TaskbarProgressBarState.Error);
		}
		private void ProgressBarSetState_Finished()
		{
			pbProgress.Style = ProgressBarStyle.Blocks;
			pbProgress.e_SetValues(0, 100, 100);
			pbProgress.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);

			_tbm.Value.SetProgressState(TaskbarProgressBarState.Normal);
			_tbm.Value.SetProgressValue(100, 100);
		}

	}
}