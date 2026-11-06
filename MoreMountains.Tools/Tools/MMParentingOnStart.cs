using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200000F RID: 15
	public class MMParentingOnStart : MonoBehaviour
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002A77 File Offset: 0x00000C77
		protected virtual void Awake()
		{
			if (this.Mode == MMParentingOnStart.Modes.Awake)
			{
				this.Parent();
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A87 File Offset: 0x00000C87
		protected virtual void Start()
		{
			if (this.Mode == MMParentingOnStart.Modes.Start)
			{
				this.Parent();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A98 File Offset: 0x00000C98
		public virtual void Parent()
		{
			base.transform.SetParent(this.TargetParent);
		}

		// Token: 0x04000030 RID: 48
		public MMParentingOnStart.Modes Mode;

		// Token: 0x04000031 RID: 49
		public Transform TargetParent;

		// Token: 0x0200039A RID: 922
		public enum Modes
		{
			// Token: 0x04001D3C RID: 7484
			Awake,
			// Token: 0x04001D3D RID: 7485
			Start,
			// Token: 0x04001D3E RID: 7486
			Script
		}
	}
}
