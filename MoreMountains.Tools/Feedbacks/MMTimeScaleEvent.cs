using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002DB RID: 731
	public struct MMTimeScaleEvent
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001475 RID: 5237 RVA: 0x00057B98 File Offset: 0x00055D98
		// (remove) Token: 0x06001476 RID: 5238 RVA: 0x00057BCC File Offset: 0x00055DCC
		private static event MMTimeScaleEvent.Delegate OnEvent;

		// Token: 0x06001477 RID: 5239 RVA: 0x00057BFF File Offset: 0x00055DFF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMTimeScaleEvent.OnEvent = null;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00057C07 File Offset: 0x00055E07
		public static void Register(MMTimeScaleEvent.Delegate callback)
		{
			MMTimeScaleEvent.OnEvent += callback;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00057C0F File Offset: 0x00055E0F
		public static void Unregister(MMTimeScaleEvent.Delegate callback)
		{
			MMTimeScaleEvent.OnEvent -= callback;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00057C18 File Offset: 0x00055E18
		public static void Trigger(MMTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite, MMTimeScaleLerpModes timeScaleLerpMode = MMTimeScaleLerpModes.Speed, MMTweenType timeScaleLerpCurve = null, float timeScaleLerpDuration = 0.2f, bool timeScaleLerpOnReset = false, MMTweenType timeScaleLerpCurveOnReset = null, float timeScaleLerpDurationOnReset = 0.2f)
		{
			MMTimeScaleEvent.Delegate onEvent = MMTimeScaleEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(timeScaleMethod, timeScale, duration, lerp, lerpSpeed, infinite, timeScaleLerpMode, timeScaleLerpCurve, timeScaleLerpDuration, timeScaleLerpOnReset, timeScaleLerpCurveOnReset, timeScaleLerpDurationOnReset);
		}

		// Token: 0x02000559 RID: 1369
		// (Invoke) Token: 0x06001D08 RID: 7432
		public delegate void Delegate(MMTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite, MMTimeScaleLerpModes timeScaleLerpMode = MMTimeScaleLerpModes.Speed, MMTweenType timeScaleLerpCurve = null, float timeScaleLerpDuration = 0.2f, bool timeScaleLerpOnReset = false, MMTweenType timeScaleLerpCurveOnReset = null, float timeScaleLerpDurationOnReset = 0.2f);
	}
}
