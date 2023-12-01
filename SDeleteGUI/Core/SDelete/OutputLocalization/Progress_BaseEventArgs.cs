#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{
	internal abstract class Progress_BaseEventArgs : DataReceivedEventArgsEx
	{
		public readonly uint CurrentOperationProgressPercent;

		internal Progress_BaseEventArgs(string raw, uint currentOperationProgressPercent) : base(raw)
			=> CurrentOperationProgressPercent = currentOperationProgressPercent;
	}
}
