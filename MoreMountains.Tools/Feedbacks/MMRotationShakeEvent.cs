using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000251 RID: 593
	public struct MMRotationShakeEvent
	{
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001066 RID: 4198 RVA: 0x000493FC File Offset: 0x000475FC
		// (remove) Token: 0x06001067 RID: 4199 RVA: 0x00049430 File Offset: 0x00047630
		private static event MMRotationShakeEvent.Delegate OnEvent;

		// Token: 0x06001068 RID: 4200 RVA: 0x00049463 File Offset: 0x00047663
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMRotationShakeEvent.OnEvent = null;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0004946B File Offset: 0x0004766B
		public static void Register(MMRotationShakeEvent.Delegate callback)
		{
			MMRotationShakeEvent.OnEvent += callback;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00049473 File Offset: 0x00047673
		public static void Unregister(MMRotationShakeEvent.Delegate callback)
		{
			MMRotationShakeEvent.OnEvent -= callback;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0004947C File Offset: 0x0004767C
		public static void Trigger(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMRotationShakeEvent.Delegate onEvent = MMRotationShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, useRange, rangeDistance, useRangeFalloff, rangeFalloff, remapRangeFalloff, rangePosition, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000522 RID: 1314
		// (Invoke) Token: 0x06001C89 RID: 7305
		public delegate void Delegate(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
