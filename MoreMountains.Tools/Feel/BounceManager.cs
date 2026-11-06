using System;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000373 RID: 883
	[AddComponentMenu("")]
	public class BounceManager : MonoBehaviour
	{
		// Token: 0x06001849 RID: 6217 RVA: 0x000671E2 File Offset: 0x000653E2
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000671EA File Offset: 0x000653EA
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Jump();
			}
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x000671FC File Offset: 0x000653FC
		protected virtual void Jump()
		{
			if (Time.time - this._lastJumpStartedAt >= this.CooldownDuration)
			{
				if (this.FeedbackAnimator.isActiveAndEnabled)
				{
					this.FeedbackAnimator.SetTrigger("Jump");
				}
				if (this.NoFeedbackAnimator.isActiveAndEnabled)
				{
					this.NoFeedbackAnimator.SetTrigger("Jump");
				}
				this._lastJumpStartedAt = Time.time;
			}
		}

		// Token: 0x04001C27 RID: 7207
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two jumps, during which jumps are prevented")]
		public float CooldownDuration = 1f;

		// Token: 0x04001C28 RID: 7208
		[Header("Bindings")]
		[Tooltip("the animator of the 'no feedback' version")]
		public Animator NoFeedbackAnimator;

		// Token: 0x04001C29 RID: 7209
		[Tooltip("the animator of the 'feedback' version")]
		public Animator FeedbackAnimator;

		// Token: 0x04001C2A RID: 7210
		protected float _lastJumpStartedAt = -100f;
	}
}
