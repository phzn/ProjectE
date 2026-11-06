using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000D1 RID: 209
	public struct MMSoundManagerTrackEvent
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x0001434E File Offset: 0x0001254E
		public MMSoundManagerTrackEvent(MMSoundManagerTrackEventTypes trackEventType, MMSoundManager.MMSoundManagerTracks track = MMSoundManager.MMSoundManagerTracks.Master, float volume = 1f)
		{
			this.TrackEventType = trackEventType;
			this.Track = track;
			this.Volume = volume;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00014365 File Offset: 0x00012565
		public static void Trigger(MMSoundManagerTrackEventTypes trackEventType, MMSoundManager.MMSoundManagerTracks track = MMSoundManager.MMSoundManagerTracks.Master, float volume = 1f)
		{
			MMSoundManagerTrackEvent.e.TrackEventType = trackEventType;
			MMSoundManagerTrackEvent.e.Track = track;
			MMSoundManagerTrackEvent.e.Volume = volume;
			MMEventManager.TriggerEvent<MMSoundManagerTrackEvent>(MMSoundManagerTrackEvent.e);
		}

		// Token: 0x040005BD RID: 1469
		public MMSoundManagerTrackEventTypes TrackEventType;

		// Token: 0x040005BE RID: 1470
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x040005BF RID: 1471
		public float Volume;

		// Token: 0x040005C0 RID: 1472
		private static MMSoundManagerTrackEvent e;
	}
}
