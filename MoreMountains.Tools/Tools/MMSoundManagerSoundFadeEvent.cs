using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000CE RID: 206
	public struct MMSoundManagerSoundFadeEvent
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x00014122 File Offset: 0x00012322
		public MMSoundManagerSoundFadeEvent(MMSoundManagerSoundFadeEvent.Modes mode, int soundID, float fadeDuration, float finalVolume, MMTweenType fadeTween)
		{
			this.Mode = mode;
			this.SoundID = soundID;
			this.FadeDuration = fadeDuration;
			this.FinalVolume = finalVolume;
			this.FadeTween = fadeTween;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001414C File Offset: 0x0001234C
		public static void Trigger(MMSoundManagerSoundFadeEvent.Modes mode, int soundID, float fadeDuration, float finalVolume, MMTweenType fadeTween)
		{
			MMSoundManagerSoundFadeEvent.e.Mode = mode;
			MMSoundManagerSoundFadeEvent.e.SoundID = soundID;
			MMSoundManagerSoundFadeEvent.e.FadeDuration = fadeDuration;
			MMSoundManagerSoundFadeEvent.e.FinalVolume = finalVolume;
			MMSoundManagerSoundFadeEvent.e.FadeTween = fadeTween;
			MMEventManager.TriggerEvent<MMSoundManagerSoundFadeEvent>(MMSoundManagerSoundFadeEvent.e);
		}

		// Token: 0x040005AE RID: 1454
		public MMSoundManagerSoundFadeEvent.Modes Mode;

		// Token: 0x040005AF RID: 1455
		public int SoundID;

		// Token: 0x040005B0 RID: 1456
		public float FadeDuration;

		// Token: 0x040005B1 RID: 1457
		public float FinalVolume;

		// Token: 0x040005B2 RID: 1458
		public MMTweenType FadeTween;

		// Token: 0x040005B3 RID: 1459
		private static MMSoundManagerSoundFadeEvent e;

		// Token: 0x02000410 RID: 1040
		public enum Modes
		{
			// Token: 0x04001EE1 RID: 7905
			PlayFade,
			// Token: 0x04001EE2 RID: 7906
			StopFade
		}
	}
}
