using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000241 RID: 577
	public struct MMCameraZoomEvent
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000FEB RID: 4075 RVA: 0x00046E7C File Offset: 0x0004507C
		// (remove) Token: 0x06000FEC RID: 4076 RVA: 0x00046EB0 File Offset: 0x000450B0
		private static event MMCameraZoomEvent.Delegate OnEvent;

		// Token: 0x06000FED RID: 4077 RVA: 0x00046EE3 File Offset: 0x000450E3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMCameraZoomEvent.OnEvent = null;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00046EEB File Offset: 0x000450EB
		public static void Register(MMCameraZoomEvent.Delegate callback)
		{
			MMCameraZoomEvent.OnEvent += callback;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00046EF3 File Offset: 0x000450F3
		public static void Unregister(MMCameraZoomEvent.Delegate callback)
		{
			MMCameraZoomEvent.OnEvent -= callback;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00046EFC File Offset: 0x000450FC
		public static void Trigger(MMCameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, MMChannelData channelData, bool useUnscaledTime = false, bool stop = false, bool relative = false, bool restore = false, MMTweenType tweenType = null)
		{
			MMCameraZoomEvent.Delegate onEvent = MMCameraZoomEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(mode, newFieldOfView, transitionDuration, duration, channelData, useUnscaledTime, stop, relative, restore, tweenType);
		}

		// Token: 0x02000518 RID: 1304
		// (Invoke) Token: 0x06001C67 RID: 7271
		public delegate void Delegate(MMCameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, MMChannelData channelData, bool useUnscaledTime = false, bool stop = false, bool relative = false, bool restore = false, MMTweenType tweenType = null);
	}
}
