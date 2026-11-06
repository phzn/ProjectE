using System;
using UnityEngine;

namespace Features.Score
{
	// Token: 0x02000C80 RID: 3200
	public static class ScoreGameplaySignals
	{
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06005CF5 RID: 23797 RVA: 0x002480E8 File Offset: 0x002462E8
		// (remove) Token: 0x06005CF6 RID: 23798 RVA: 0x0024811C File Offset: 0x0024631C
		public static event Action<ScoreHidingSpotKind, Object> HidingStarted;

		// Token: 0x06005CF7 RID: 23799 RVA: 0x0024814F File Offset: 0x0024634F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			ScoreGameplaySignals.HidingStarted = null;
		}

		// Token: 0x06005CF8 RID: 23800 RVA: 0x00248157 File Offset: 0x00246357
		public static void NotifyHidingStarted(ScoreHidingSpotKind kind, Object source)
		{
			Action<ScoreHidingSpotKind, Object> hidingStarted = ScoreGameplaySignals.HidingStarted;
			if (hidingStarted == null)
			{
				return;
			}
			hidingStarted(kind, source);
		}
	}
}
