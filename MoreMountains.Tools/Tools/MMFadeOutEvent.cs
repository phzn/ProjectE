using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000162 RID: 354
	public struct MMFadeOutEvent
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x0002676B File Offset: 0x0002496B
		public MMFadeOutEvent(float duration, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			this.ID = id;
			this.Duration = duration;
			this.Curve = tween;
			this.IgnoreTimeScale = ignoreTimeScale;
			this.WorldPosition = worldPosition;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00026794 File Offset: 0x00024994
		public static void Trigger(float duration, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			MMFadeOutEvent.e.ID = id;
			MMFadeOutEvent.e.Duration = duration;
			MMFadeOutEvent.e.Curve = tween;
			MMFadeOutEvent.e.IgnoreTimeScale = ignoreTimeScale;
			MMFadeOutEvent.e.WorldPosition = worldPosition;
			MMEventManager.TriggerEvent<MMFadeOutEvent>(MMFadeOutEvent.e);
		}

		// Token: 0x04000991 RID: 2449
		public int ID;

		// Token: 0x04000992 RID: 2450
		public float Duration;

		// Token: 0x04000993 RID: 2451
		public MMTweenType Curve;

		// Token: 0x04000994 RID: 2452
		public bool IgnoreTimeScale;

		// Token: 0x04000995 RID: 2453
		public Vector3 WorldPosition;

		// Token: 0x04000996 RID: 2454
		private static MMFadeOutEvent e;
	}
}
