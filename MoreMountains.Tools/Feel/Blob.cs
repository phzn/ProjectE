using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000371 RID: 881
	[AddComponentMenu("")]
	public class Blob : MonoBehaviour
	{
		// Token: 0x06001841 RID: 6209 RVA: 0x00067120 File Offset: 0x00065320
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00067128 File Offset: 0x00065328
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Move();
			}
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00067138 File Offset: 0x00065338
		protected virtual void Move()
		{
			if (Time.time - this._lastMoveStartedAt >= this.CooldownDuration)
			{
				MMFeedbacks moveFeedback = this.MoveFeedback;
				if (moveFeedback != null)
				{
					moveFeedback.PlayFeedbacks();
				}
				this._lastMoveStartedAt = Time.time;
				return;
			}
			MMFeedbacks deniedFeedback = this.DeniedFeedback;
			if (deniedFeedback == null)
			{
				return;
			}
			deniedFeedback.PlayFeedbacks();
		}

		// Token: 0x04001C20 RID: 7200
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two moves, during which moves are prevented")]
		public float CooldownDuration = 1f;

		// Token: 0x04001C21 RID: 7201
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when moving")]
		public MMFeedbacks MoveFeedback;

		// Token: 0x04001C22 RID: 7202
		[Tooltip("a feedback to call when trying to move while in cooldown")]
		public MMFeedbacks DeniedFeedback;

		// Token: 0x04001C23 RID: 7203
		protected float _lastMoveStartedAt = -100f;
	}
}
