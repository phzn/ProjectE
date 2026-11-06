using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024F RID: 591
	public struct MMPositionShakeEvent
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600104F RID: 4175 RVA: 0x00048BD4 File Offset: 0x00046DD4
		// (remove) Token: 0x06001050 RID: 4176 RVA: 0x00048C08 File Offset: 0x00046E08
		private static event MMPositionShakeEvent.Delegate OnEvent;

		// Token: 0x06001051 RID: 4177 RVA: 0x00048C3B File Offset: 0x00046E3B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPositionShakeEvent.OnEvent = null;
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00048C43 File Offset: 0x00046E43
		public static void Register(MMPositionShakeEvent.Delegate callback)
		{
			MMPositionShakeEvent.OnEvent += callback;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00048C4B File Offset: 0x00046E4B
		public static void Unregister(MMPositionShakeEvent.Delegate callback)
		{
			MMPositionShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00048C54 File Offset: 0x00046E54
		public static void Trigger(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool randomizeDirectionX, bool randomizeDirectionY, bool randomizeDirectionZ, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			MMPositionShakeEvent.Delegate onEvent = MMPositionShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, randomizeDirectionX, randomizeDirectionY, randomizeDirectionZ, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, useRange, rangeDistance, useRangeFalloff, rangeFalloff, remapRangeFalloff, rangePosition, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop, restore);
		}

		// Token: 0x02000520 RID: 1312
		// (Invoke) Token: 0x06001C85 RID: 7301
		public delegate void Delegate(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool randomizeDirectionX, bool randomizeDirectionY, bool randomizeDirectionZ, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false);
	}
}
