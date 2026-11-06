using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200038A RID: 906
	[AddComponentMenu("")]
	public class Tactical : MonoBehaviour
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x00069571 File Offset: 0x00067771
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x00069579 File Offset: 0x00067779
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressed())
			{
				this.Shoot();
			}
			if (FeelDemosInputHelper.CheckMainActionInputUpThisFrame())
			{
				this.ShootStop();
			}
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00069598 File Offset: 0x00067798
		protected virtual void Shoot()
		{
			if (Time.time - this._lastJumpStartedAt > this.CooldownDuration)
			{
				float num = (float)Random.Range(20, 200);
				MMFeedbacks shootFeedback = this.ShootFeedback;
				if (shootFeedback != null)
				{
					shootFeedback.PlayFeedbacks(this.ImpactPosition.position, num, false);
				}
				this._lastJumpStartedAt = Time.time;
				this._magazine--;
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000695FE File Offset: 0x000677FE
		protected virtual void ShootStop()
		{
			MMFeedbacks shootStopFeedback = this.ShootStopFeedback;
			if (shootStopFeedback != null)
			{
				shootStopFeedback.PlayFeedbacks();
			}
			if (this._magazine < 0)
			{
				MMFeedbacks reloadFeedback = this.ReloadFeedback;
				if (reloadFeedback != null)
				{
					reloadFeedback.PlayFeedbacks();
				}
				this._magazine = 15;
			}
		}

		// Token: 0x04001CE6 RID: 7398
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two shots, during which shots are prevented")]
		public float CooldownDuration = 0.1f;

		// Token: 0x04001CE7 RID: 7399
		[Header("Bindings")]
		[Tooltip("the position of the shot's impact")]
		public Transform ImpactPosition;

		// Token: 0x04001CE8 RID: 7400
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when shooting")]
		public MMFeedbacks ShootFeedback;

		// Token: 0x04001CE9 RID: 7401
		[Tooltip("a feedback to call when shooting stops")]
		public MMFeedbacks ShootStopFeedback;

		// Token: 0x04001CEA RID: 7402
		[Tooltip("a feedback to call when a reload happens")]
		public MMFeedbacks ReloadFeedback;

		// Token: 0x04001CEB RID: 7403
		protected float _lastJumpStartedAt = -100f;

		// Token: 0x04001CEC RID: 7404
		protected int _magazine = 15;
	}
}
