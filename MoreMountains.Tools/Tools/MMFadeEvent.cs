using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000160 RID: 352
	public struct MMFadeEvent
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x00026641 File Offset: 0x00024841
		public MMFadeEvent(float duration, float targetAlpha, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			this.ID = id;
			this.Duration = duration;
			this.TargetAlpha = targetAlpha;
			this.Curve = tween;
			this.IgnoreTimeScale = ignoreTimeScale;
			this.WorldPosition = worldPosition;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00026670 File Offset: 0x00024870
		public static void Trigger(float duration, float targetAlpha)
		{
			MMFadeEvent.Trigger(duration, targetAlpha, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic), 0, true, default(Vector3));
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00026698 File Offset: 0x00024898
		public static void Trigger(float duration, float targetAlpha, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			MMFadeEvent.e.ID = id;
			MMFadeEvent.e.Duration = duration;
			MMFadeEvent.e.TargetAlpha = targetAlpha;
			MMFadeEvent.e.Curve = tween;
			MMFadeEvent.e.IgnoreTimeScale = ignoreTimeScale;
			MMFadeEvent.e.WorldPosition = worldPosition;
			MMEventManager.TriggerEvent<MMFadeEvent>(MMFadeEvent.e);
		}

		// Token: 0x04000984 RID: 2436
		public int ID;

		// Token: 0x04000985 RID: 2437
		public float Duration;

		// Token: 0x04000986 RID: 2438
		public float TargetAlpha;

		// Token: 0x04000987 RID: 2439
		public MMTweenType Curve;

		// Token: 0x04000988 RID: 2440
		public bool IgnoreTimeScale;

		// Token: 0x04000989 RID: 2441
		public Vector3 WorldPosition;

		// Token: 0x0400098A RID: 2442
		private static MMFadeEvent e;
	}
}
