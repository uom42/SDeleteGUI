using System.Diagnostics;
using System.Runtime.CompilerServices;

using NLog;

using uom.Extensions;

#nullable enable

namespace common
{
	internal static class ExtensionsNLog
	{

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Handle(
			this Exception ex,
			bool showError = true,
			ILogger? logger = null,
			MessageBoxIcon icon = MessageBoxIcon.Error)
		{
			logger ??= LogManager.GetCurrentClassLogger();
			logger.Error(ex);

			if (showError)
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, icon);
		}

		public const int DEFAULT_FORM_SHOWN_DELAY = 500;


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_RunDelay(
			this Form f,
			Task[] tasks,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool showError = true,
			bool CloseFormOnError = true,
			bool useWaitCursor = true)
		{
			f.Shown += async (s, e) =>
			{
				await Task.Delay(delay);

				if (useWaitCursor) f.UseWaitCursor = true;
				try
				{
					try { await Task.WhenAll(tasks); }
					finally { if (useWaitCursor) f.UseWaitCursor = false; }
				}
				catch (Exception ex)
				{
					ex.Handle(showError);
					if (CloseFormOnError) f.Close();
				}
			};
		}

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_RunDelay(
			this Form f,
			Task task,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool showError = true,
			bool CloseFormOnError = true,
			bool useWaitCursor = true)
				=> e_RunDelay(f, task.e_ToArrayOf(), delay, showError, CloseFormOnError, useWaitCursor);


	}

}
