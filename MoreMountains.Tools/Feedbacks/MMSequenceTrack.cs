using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000221 RID: 545
	[Serializable]
	public class MMSequenceTrack
	{
		// Token: 0x06000EFB RID: 3835 RVA: 0x00042ECC File Offset: 0x000410CC
		public virtual void SetDefaults(int index)
		{
			if (!this.Initialized)
			{
				this.ID = index;
				this.TrackColor = MMSequence.RandomSequenceColor();
				this.Key = KeyCode.Space;
				this.Active = true;
				this.State = MMSequenceTrackStates.Idle;
				this.Initialized = true;
			}
		}

		// Token: 0x0400118F RID: 4495
		public int ID;

		// Token: 0x04001190 RID: 4496
		public Color TrackColor;

		// Token: 0x04001191 RID: 4497
		public KeyCode Key = KeyCode.Space;

		// Token: 0x04001192 RID: 4498
		public bool Active = true;

		// Token: 0x04001193 RID: 4499
		[MMFReadOnly]
		public MMSequenceTrackStates State;

		// Token: 0x04001194 RID: 4500
		[HideInInspector]
		public bool Initialized;
	}
}
