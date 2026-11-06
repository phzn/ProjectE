using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000D8 RID: 216
	[Serializable]
	public struct MMSoundManagerSound
	{
		// Token: 0x0400061B RID: 1563
		public int ID;

		// Token: 0x0400061C RID: 1564
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x0400061D RID: 1565
		public AudioSource Source;

		// Token: 0x0400061E RID: 1566
		public bool Persistent;

		// Token: 0x0400061F RID: 1567
		public float PlaybackTime;

		// Token: 0x04000620 RID: 1568
		public float PlaybackDuration;
	}
}
