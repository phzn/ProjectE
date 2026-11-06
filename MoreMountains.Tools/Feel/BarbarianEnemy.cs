using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000370 RID: 880
	[AddComponentMenu("")]
	public class BarbarianEnemy : MonoBehaviour
	{
		// Token: 0x0600183F RID: 6207 RVA: 0x000670C2 File Offset: 0x000652C2
		public virtual void TakeDamage(int damage)
		{
			if (Time.time - this._lastDamageTakenAt < this.DamageCooldown)
			{
				return;
			}
			this._lastDamageTakenAt = Time.time;
			MMFeedbacks damageFeedback = this.DamageFeedback;
			if (damageFeedback == null)
			{
				return;
			}
			damageFeedback.PlayFeedbacks(base.transform.position, (float)damage, false);
		}

		// Token: 0x04001C1D RID: 7197
		public MMFeedbacks DamageFeedback;

		// Token: 0x04001C1E RID: 7198
		public float DamageCooldown = 1f;

		// Token: 0x04001C1F RID: 7199
		protected float _lastDamageTakenAt = -10f;
	}
}
