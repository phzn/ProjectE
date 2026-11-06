using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C9 RID: 713
	public struct MMLight2DShakeEvent
	{
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06001436 RID: 5174 RVA: 0x00057498 File Offset: 0x00055698
		// (remove) Token: 0x06001437 RID: 5175 RVA: 0x000574CC File Offset: 0x000556CC
		private static event MMLight2DShakeEvent.Delegate OnEvent;

		// Token: 0x06001438 RID: 5176 RVA: 0x000574FF File Offset: 0x000556FF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMLight2DShakeEvent.OnEvent = null;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00057507 File Offset: 0x00055707
		public static void Register(MMLight2DShakeEvent.Delegate callback)
		{
			MMLight2DShakeEvent.OnEvent += callback;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0005750F File Offset: 0x0005570F
		public static void Unregister(MMLight2DShakeEvent.Delegate callback)
		{
			MMLight2DShakeEvent.OnEvent -= callback;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00057518 File Offset: 0x00055718
		public static void Trigger(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			MMLight2DShakeEvent.Delegate onEvent = MMLight2DShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakeDuration, relativeValues, modifyColor, colorOverTime, intensityCurve, remapIntensityZero, remapIntensityOne, rangeCurve, remapRangeZero, remapRangeOne, shadowStrengthCurve, remapShadowStrengthZero, remapShadowStrengthOne, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, useRange, eventRange, eventOriginPosition);
		}

		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x06001D04 RID: 7428
		public delegate void Delegate(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3));
	}
}
