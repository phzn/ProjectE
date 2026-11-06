using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000220 RID: 544
	[Serializable]
	public class MMSequenceNote
	{
		// Token: 0x06000EF9 RID: 3833 RVA: 0x00042EA5 File Offset: 0x000410A5
		public virtual MMSequenceNote Copy()
		{
			return new MMSequenceNote
			{
				ID = this.ID,
				Timestamp = this.Timestamp
			};
		}

		// Token: 0x0400118D RID: 4493
		public float Timestamp;

		// Token: 0x0400118E RID: 4494
		public int ID;
	}
}
