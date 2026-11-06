using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Tools
{
	// Token: 0x020000B6 RID: 182
	public struct MMSfxEvent
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000395 RID: 917 RVA: 0x00012138 File Offset: 0x00010338
		// (remove) Token: 0x06000396 RID: 918 RVA: 0x0001216C File Offset: 0x0001036C
		private static event MMSfxEvent.Delegate OnEvent;

		// Token: 0x06000397 RID: 919 RVA: 0x0001219F File Offset: 0x0001039F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMSfxEvent.OnEvent = null;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000121A7 File Offset: 0x000103A7
		public static void Register(MMSfxEvent.Delegate callback)
		{
			MMSfxEvent.OnEvent += callback;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000121AF File Offset: 0x000103AF
		public static void Unregister(MMSfxEvent.Delegate callback)
		{
			MMSfxEvent.OnEvent -= callback;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000121B7 File Offset: 0x000103B7
		public static void Trigger(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f, int priority = 128)
		{
			MMSfxEvent.Delegate onEvent = MMSfxEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(clipToPlay, audioGroup, volume, pitch, priority);
		}

		// Token: 0x020003FE RID: 1022
		// (Invoke) Token: 0x0600195B RID: 6491
		public delegate void Delegate(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f, int priority = 128);
	}
}
