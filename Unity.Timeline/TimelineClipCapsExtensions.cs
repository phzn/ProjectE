using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000019 RID: 25
	internal static class TimelineClipCapsExtensions
	{
		// Token: 0x0600019F RID: 415 RVA: 0x00006AC3 File Offset: 0x00004CC3
		public static bool SupportsLooping(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Looping) > ClipCaps.None;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006AD5 File Offset: 0x00004CD5
		public static bool SupportsExtrapolation(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Extrapolation) > ClipCaps.None;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006AE7 File Offset: 0x00004CE7
		public static bool SupportsClipIn(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.ClipIn) > ClipCaps.None;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006AF9 File Offset: 0x00004CF9
		public static bool SupportsSpeedMultiplier(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.SpeedMultiplier) > ClipCaps.None;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006B0B File Offset: 0x00004D0B
		public static bool SupportsBlending(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Blending) > ClipCaps.None;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006B1E File Offset: 0x00004D1E
		public static bool HasAll(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) == flags;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00006B26 File Offset: 0x00004D26
		public static bool HasAny(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) > ClipCaps.None;
		}
	}
}
