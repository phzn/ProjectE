using System;
using Features.Localization;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D69 RID: 3433
	internal static class RankedRunIntegrityNotice
	{
		// Token: 0x0600651D RID: 25885 RVA: 0x0028591A File Offset: 0x00283B1A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Register()
		{
			RunModeService.LeaderboardRunDisqualified -= RankedRunIntegrityNotice.Show;
			RunModeService.LeaderboardRunDisqualified += RankedRunIntegrityNotice.Show;
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x0028593E File Offset: 0x00283B3E
		private static void Show(string reason)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			ToastService.ShowComputed(RunStatusLocalization.CreateExclusionMessage(reason), "Ranked run excluded", 7f, "runtime_debug_ui", null);
		}
	}
}
