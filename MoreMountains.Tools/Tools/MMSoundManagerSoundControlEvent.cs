using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000CD RID: 205
	public struct MMSoundManagerSoundControlEvent
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x000140DE File Offset: 0x000122DE
		public MMSoundManagerSoundControlEvent(MMSoundManagerSoundControlEventTypes eventType, int soundID, AudioSource source = null)
		{
			this.SoundID = soundID;
			this.TargetSource = source;
			this.MMSoundManagerSoundControlEventType = eventType;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000140F5 File Offset: 0x000122F5
		public static void Trigger(MMSoundManagerSoundControlEventTypes eventType, int soundID, AudioSource source = null)
		{
			MMSoundManagerSoundControlEvent.e.SoundID = soundID;
			MMSoundManagerSoundControlEvent.e.TargetSource = source;
			MMSoundManagerSoundControlEvent.e.MMSoundManagerSoundControlEventType = eventType;
			MMEventManager.TriggerEvent<MMSoundManagerSoundControlEvent>(MMSoundManagerSoundControlEvent.e);
		}

		// Token: 0x040005AA RID: 1450
		public int SoundID;

		// Token: 0x040005AB RID: 1451
		public MMSoundManagerSoundControlEventTypes MMSoundManagerSoundControlEventType;

		// Token: 0x040005AC RID: 1452
		public AudioSource TargetSource;

		// Token: 0x040005AD RID: 1453
		private static MMSoundManagerSoundControlEvent e;
	}
}
