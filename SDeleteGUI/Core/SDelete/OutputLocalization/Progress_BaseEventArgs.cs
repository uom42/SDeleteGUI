#nullable enable

namespace SDeleteGUI.Core.SDelete.OutputLocalization
{
	internal abstract class Progress_BaseEventArgs(string raw, uint currentOperationProgressPercent) : DataReceivedEventArgsEx(raw)
	{
		public readonly uint CurrentOperationProgressPercent = currentOperationProgressPercent;
	}
}
