using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000D2 RID: 210
	public struct MMSoundManagerTrackFadeEvent
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x00014392 File Offset: 0x00012592
		public MMSoundManagerTrackFadeEvent(MMSoundManagerTrackFadeEvent.Modes mode, MMSoundManager.MMSoundManagerTracks track, float fadeDuration, float finalVolume, MMTweenType fadeTween)
		{
			this.Mode = mode;
			this.Track = track;
			this.FadeDuration = fadeDuration;
			this.FinalVolume = finalVolume;
			this.FadeTween = fadeTween;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000143BC File Offset: 0x000125BC
		public static void Trigger(MMSoundManagerTrackFadeEvent.Modes mode, MMSoundManager.MMSoundManagerTracks track, float fadeDuration, float finalVolume, MMTweenType fadeTween)
		{
			MMSoundManagerTrackFadeEvent.e.Mode = mode;
			MMSoundManagerTrackFadeEvent.e.Track = track;
			MMSoundManagerTrackFadeEvent.e.FadeDuration = fadeDuration;
			MMSoundManagerTrackFadeEvent.e.FinalVolume = finalVolume;
			MMSoundManagerTrackFadeEvent.e.FadeTween = fadeTween;
			MMEventManager.TriggerEvent<MMSoundManagerTrackFadeEvent>(MMSoundManagerTrackFadeEvent.e);
		}

		// Token: 0x040005C1 RID: 1473
		public MMSoundManagerTrackFadeEvent.Modes Mode;

		// Token: 0x040005C2 RID: 1474
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x040005C3 RID: 1475
		public float FadeDuration;

		// Token: 0x040005C4 RID: 1476
		public float FinalVolume;

		// Token: 0x040005C5 RID: 1477
		public MMTweenType FadeTween;

		// Token: 0x040005C6 RID: 1478
		private static MMSoundManagerTrackFadeEvent e;

		// Token: 0x02000412 RID: 1042
		public enum Modes
		{
			// Token: 0x04001EE4 RID: 7908
			PlayFade,
			// Token: 0x04001EE5 RID: 7909
			StopFade
		}
	}
}
