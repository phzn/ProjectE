using System;
using UnityEngine;

namespace Features.Achievements
{
	// Token: 0x02001539 RID: 5433
	internal static class AchievementProgressionReconciler
	{
		// Token: 0x0600A834 RID: 43060 RVA: 0x00463B92 File Offset: 0x00461D92
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void ReconcileExistingSave()
		{
			AchievementTrackingService.ReconcileExistingSave();
		}
	}
}
