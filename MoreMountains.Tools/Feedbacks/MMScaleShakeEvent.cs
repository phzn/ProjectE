using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000253 RID: 595
	public struct MMScaleShakeEvent
	{
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600107D RID: 4221 RVA: 0x00049C00 File Offset: 0x00047E00
		// (remove) Token: 0x0600107E RID: 4222 RVA: 0x00049C34 File Offset: 0x00047E34
		private static event MMScaleShakeEvent.Delegate OnEvent;

		// Token: 0x0600107F RID: 4223 RVA: 0x00049C67 File Offset: 0x00047E67
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMScaleShakeEvent.OnEvent = null;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00049C6F File Offset: 0x00047E6F
		public static void Register(MMScaleShakeEvent.Delegate callback)
		{
			MMScaleShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00049C77 File Offset: 0x00047E77
		public static void Unregister(MMScaleShakeEvent.Delegate callback)
		{
			MMScaleShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00049C80 File Offset: 0x00047E80
		public static void Trigger(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMScaleShakeEvent.Delegate onEvent = MMScaleShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, useRange, rangeDistance, useRangeFalloff, rangeFalloff, remapRangeFalloff, rangePosition, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000524 RID: 1316
		// (Invoke) Token: 0x06001C8D RID: 7309
		public delegate void Delegate(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
