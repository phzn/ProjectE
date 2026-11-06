using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000242 RID: 578
	public struct MMCameraShakeEvent
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000FF1 RID: 4081 RVA: 0x00046F28 File Offset: 0x00045128
		// (remove) Token: 0x06000FF2 RID: 4082 RVA: 0x00046F5C File Offset: 0x0004515C
		private static event MMCameraShakeEvent.Delegate OnEvent;

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00046F8F File Offset: 0x0004518F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraShakeEvent.OnEvent = null;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00046F97 File Offset: 0x00045197
		public static void Register(MMCameraShakeEvent.Delegate callback)
		{
			MMCameraShakeEvent.OnEvent += callback;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00046F9F File Offset: 0x0004519F
		public static void Unregister(MMCameraShakeEvent.Delegate callback)
		{
			MMCameraShakeEvent.OnEvent -= callback;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00046FA8 File Offset: 0x000451A8
		public static void Trigger(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite = false, MMChannelData channelData = null, bool useUnscaledTime = false)
		{
			MMCameraShakeEvent.Delegate onEvent = MMCameraShakeEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(duration, amplitude, frequency, amplitudeX, amplitudeY, amplitudeZ, infinite, channelData, useUnscaledTime);
		}

		// Token: 0x02000519 RID: 1305
		// (Invoke) Token: 0x06001C6B RID: 7275
		public delegate void Delegate(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite = false, MMChannelData channelData = null, bool useUnscaledTime = false);
	}
}
