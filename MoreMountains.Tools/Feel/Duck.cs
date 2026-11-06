using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000378 RID: 888
	[AddComponentMenu("")]
	public class Duck : MonoBehaviour
	{
		// Token: 0x0600185A RID: 6234 RVA: 0x0006798C File Offset: 0x00065B8C
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00067994 File Offset: 0x00065B94
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Jump();
			}
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x000679A4 File Offset: 0x00065BA4
		protected virtual void Jump()
		{
			if (Time.time - this._lastJumpStartedAt >= this.CooldownDuration)
			{
				MMFeedbacks jumpFeedback = this.JumpFeedback;
				if (jumpFeedback != null)
				{
					jumpFeedback.PlayFeedbacks();
				}
				this._lastJumpStartedAt = Time.time;
				return;
			}
			MMFeedbacks deniedFeedback = this.DeniedFeedback;
			if (deniedFeedback == null)
			{
				return;
			}
			deniedFeedback.PlayFeedbacks();
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000679F2 File Offset: 0x00065BF2
		public virtual void Land()
		{
			MMFeedbacks landingFeedback = this.LandingFeedback;
			if (landingFeedback == null)
			{
				return;
			}
			landingFeedback.PlayFeedbacks();
		}

		// Token: 0x04001C56 RID: 7254
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two jumps, during which jumps are prevented")]
		public float CooldownDuration = 1f;

		// Token: 0x04001C57 RID: 7255
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when jumping")]
		public MMFeedbacks JumpFeedback;

		// Token: 0x04001C58 RID: 7256
		[Tooltip("a feedback to call when landing")]
		public MMFeedbacks LandingFeedback;

		// Token: 0x04001C59 RID: 7257
		[Tooltip("a feedback to call when trying to jump while in cooldown")]
		public MMFeedbacks DeniedFeedback;

		// Token: 0x04001C5A RID: 7258
		protected float _lastJumpStartedAt = -100f;
	}
}
