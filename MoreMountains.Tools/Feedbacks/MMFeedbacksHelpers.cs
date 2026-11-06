using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B0 RID: 432
	[AddComponentMenu("")]
	public class MMFeedbacksHelpers : MonoBehaviour
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x0002F7D4 File Offset: 0x0002D9D4
		public static float Remap(float x, float A, float B, float C, float D)
		{
			return C + (x - A) / (B - A) * (D - C);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002F7E4 File Offset: 0x0002D9E4
		public static void MigrateCurve(AnimationCurve oldCurve, MMTweenType newTweenType, MMF_Player owner)
		{
			if (oldCurve.keys.Length != 0 && !newTweenType.Initialized)
			{
				newTweenType.Curve = oldCurve;
				newTweenType.MMTweenDefinitionType = MMTweenDefinitionTypes.AnimationCurve;
				oldCurve = null;
				newTweenType.Initialized = true;
			}
		}
	}
}
