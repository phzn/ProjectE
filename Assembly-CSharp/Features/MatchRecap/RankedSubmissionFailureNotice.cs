using System;
using System.Collections.Generic;
using Features.RunModes;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.MatchRecap
{
	// Token: 0x02000FD3 RID: 4051
	internal static class RankedSubmissionFailureNotice
	{
		// Token: 0x06007A64 RID: 31332 RVA: 0x0031FF79 File Offset: 0x0031E179
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Register()
		{
			RankedSubmissionFailureNotice.NotifiedRuns.Clear();
			RankedLeaderboardService.SubmissionFailed -= RankedSubmissionFailureNotice.Show;
			RankedLeaderboardService.SubmissionFailed += RankedSubmissionFailureNotice.Show;
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x0031FFA8 File Offset: 0x0031E1A8
		private static void Show(RankedSubmissionFailure failure)
		{
			string text;
			if (!Application.isPlaying || !RankedSubmissionFailureNotice.TryGetNotice(failure, out text))
			{
				return;
			}
			ToastService.Show(text, "Score not saved", null, 8f, "rank_ui");
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x0031FFE0 File Offset: 0x0031E1E0
		private static bool TryGetNotice(RankedSubmissionFailure failure, out string message)
		{
			message = string.Empty;
			if (failure == null || string.IsNullOrEmpty(failure.RunId) || !RankedSubmissionFailureNotice.NotifiedRuns.Add(failure.RunId))
			{
				return false;
			}
			message = (failure.RetainedForRetry ? "Your run result couldn't be saved to Steam yet.\nIt's saved on this device. We'll retry automatically." : "Your run result couldn't be saved to Steam.");
			return true;
		}

		// Token: 0x04007DB5 RID: 32181
		private static readonly HashSet<string> NotifiedRuns = new HashSet<string>(StringComparer.Ordinal);
	}
}
