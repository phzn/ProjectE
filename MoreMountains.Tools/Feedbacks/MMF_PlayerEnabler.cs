using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C7 RID: 455
	[AddComponentMenu("")]
	public class MMF_PlayerEnabler : MonoBehaviour
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0003268A File Offset: 0x0003088A
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x00032692 File Offset: 0x00030892
		public virtual MMF_Player TargetMmfPlayer { get; set; }

		// Token: 0x06000B5F RID: 2911 RVA: 0x0003269B File Offset: 0x0003089B
		protected virtual void OnEnable()
		{
			if (this.TargetMmfPlayer != null && !this.TargetMmfPlayer.enabled && this.TargetMmfPlayer.AutoPlayOnEnable)
			{
				this.TargetMmfPlayer.enabled = true;
			}
		}
	}
}
