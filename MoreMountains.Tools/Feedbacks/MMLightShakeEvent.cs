using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024C RID: 588
	public struct MMLightShakeEvent
	{
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x0600102D RID: 4141 RVA: 0x00047DE0 File Offset: 0x00045FE0
		// (remove) Token: 0x0600102E RID: 4142 RVA: 0x00047E14 File Offset: 0x00046014
		private static event MMLightShakeEvent.Delegate OnEvent;

		// Token: 0x0600102F RID: 4143 RVA: 0x00047E47 File Offset: 0x00046047
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMLightShakeEvent.OnEvent = null;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00047E4F File Offset: 0x0004604F
		public static void Register(MMLightShakeEvent.Delegate callback)
		{
			MMLightShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00047E57 File Offset: 0x00046057
		public static void Unregister(MMLightShakeEvent.Delegate callback)
		{
			MMLightShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00047E60 File Offset: 0x00046060
		public static void Trigger(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			MMLightShakeEvent.Delegate onEvent = MMLightShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(shakeDuration, relativeValues, modifyColor, colorOverTime, intensityCurve, remapIntensityZero, remapIntensityOne, rangeCurve, remapRangeZero, remapRangeOne, shadowStrengthCurve, remapShadowStrengthZero, remapShadowStrengthOne, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, useRange, eventRange, eventOriginPosition);
		}

		// Token: 0x0200051D RID: 1309
		// (Invoke) Token: 0x06001C7B RID: 7291
		public delegate void Delegate(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3));
	}
}
