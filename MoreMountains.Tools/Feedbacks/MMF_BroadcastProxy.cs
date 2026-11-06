using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000294 RID: 660
	public class MMF_BroadcastProxy : MonoBehaviour
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0004FFBE File Offset: 0x0004E1BE
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x0004FFC6 File Offset: 0x0004E1C6
		public virtual float ThisLevel { get; set; }

		// Token: 0x0600128E RID: 4750 RVA: 0x0004FFCF File Offset: 0x0004E1CF
		protected virtual void Update()
		{
			this.ProcessBroadcast();
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0004FFD8 File Offset: 0x0004E1D8
		protected virtual void ProcessBroadcast()
		{
			this.BroadcastInProgress = false;
			if (this.ThisLevel != this._levelLastFrame)
			{
				MMRadioLevelEvent.Trigger(this.Channel, this.ThisLevel);
				this.BroadcastInProgress = true;
			}
			this.DebugLevel = this.ThisLevel;
			this._levelLastFrame = this.ThisLevel;
		}

		// Token: 0x040014E7 RID: 5351
		[Tooltip("the channel on which to broadcast")]
		[MMReadOnly]
		public int Channel;

		// Token: 0x040014E8 RID: 5352
		[Tooltip("a debug view of the current level being broadcasted")]
		[MMReadOnly]
		public float DebugLevel;

		// Token: 0x040014E9 RID: 5353
		[Tooltip("whether or not a broadcast is in progress (will be false while the value is not changing, and thus not broadcasting)")]
		[MMReadOnly]
		public bool BroadcastInProgress;

		// Token: 0x040014EB RID: 5355
		protected float _levelLastFrame;
	}
}
