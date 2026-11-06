using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000013 RID: 19
	[AddComponentMenu("More Mountains/Tools/Activation/MMTimedDestruction")]
	public class MMTimedDestruction : MonoBehaviour
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00002E48 File Offset: 0x00001048
		protected virtual void Start()
		{
			base.StartCoroutine(this.Destruction());
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002E57 File Offset: 0x00001057
		protected virtual IEnumerator Destruction()
		{
			yield return MMCoroutine.WaitFor(this.TimeBeforeDestruction);
			if (this.TimeDestructionMode == MMTimedDestruction.TimedDestructionModes.Destroy)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x04000059 RID: 89
		public MMTimedDestruction.TimedDestructionModes TimeDestructionMode;

		// Token: 0x0400005A RID: 90
		public float TimeBeforeDestruction = 2f;

		// Token: 0x020003A2 RID: 930
		public enum TimedDestructionModes
		{
			// Token: 0x04001D5E RID: 7518
			Destroy,
			// Token: 0x04001D5F RID: 7519
			Disable
		}
	}
}
