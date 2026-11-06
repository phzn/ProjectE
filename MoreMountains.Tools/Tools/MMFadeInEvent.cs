using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000161 RID: 353
	public struct MMFadeInEvent
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x000266F3 File Offset: 0x000248F3
		public MMFadeInEvent(float duration, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			this.ID = id;
			this.Duration = duration;
			this.Curve = tween;
			this.IgnoreTimeScale = ignoreTimeScale;
			this.WorldPosition = worldPosition;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002671C File Offset: 0x0002491C
		public static void Trigger(float duration, MMTweenType tween, int id = 0, bool ignoreTimeScale = true, Vector3 worldPosition = default(Vector3))
		{
			MMFadeInEvent.e.ID = id;
			MMFadeInEvent.e.Duration = duration;
			MMFadeInEvent.e.Curve = tween;
			MMFadeInEvent.e.IgnoreTimeScale = ignoreTimeScale;
			MMFadeInEvent.e.WorldPosition = worldPosition;
			MMEventManager.TriggerEvent<MMFadeInEvent>(MMFadeInEvent.e);
		}

		// Token: 0x0400098B RID: 2443
		public int ID;

		// Token: 0x0400098C RID: 2444
		public float Duration;

		// Token: 0x0400098D RID: 2445
		public MMTweenType Curve;

		// Token: 0x0400098E RID: 2446
		public bool IgnoreTimeScale;

		// Token: 0x0400098F RID: 2447
		public Vector3 WorldPosition;

		// Token: 0x04000990 RID: 2448
		private static MMFadeInEvent e;
	}
}
