
using static SDeleteGUI.Core.SDelete.SDeleteManager;

#nullable enable

namespace SDeleteGUI.Core.SDelete
{
	internal static class Extensions_SDelete
	{
		public static string ToArgs(this CleanModes cm)
			=> ((cm == CleanModes.Clean)
			? C_ARG_CLEAN_FREE_SPACE
			: C_ARG_ZERO_FREE_SPACE)
			.ToLower();

	}
}