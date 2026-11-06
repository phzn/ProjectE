using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A0 RID: 416
	[AddComponentMenu("")]
	public class DemoBall : MonoBehaviour
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x0002DE4F File Offset: 0x0002C04F
		protected virtual void Start()
		{
			base.StartCoroutine(this.ProgrammedDeath());
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002DE5E File Offset: 0x0002C05E
		protected virtual IEnumerator ProgrammedDeath()
		{
			yield return MMCoroutine.WaitFor(this.LifeSpan);
			MMFeedbacks deathFeedback = this.DeathFeedback;
			if (deathFeedback != null)
			{
				deathFeedback.PlayFeedbacks();
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000C14 RID: 3092
		public float LifeSpan = 2f;

		// Token: 0x04000C15 RID: 3093
		public MMFeedbacks DeathFeedback;
	}
}
