using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200038B RID: 907
	[AddComponentMenu("")]
	public class Wheel : MonoBehaviour
	{
		// Token: 0x060018C4 RID: 6340 RVA: 0x00069659 File Offset: 0x00067859
		protected virtual void Update()
		{
			this.HandleInput();
			this.HandleWheel();
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00069667 File Offset: 0x00067867
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Turn();
			}
			if (FeelDemosInputHelper.CheckMainActionInputUpThisFrame())
			{
				this.TurnStop();
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x00069683 File Offset: 0x00067883
		protected virtual void HandleWheel()
		{
			if (this._turning)
			{
				this.RotatingPart.transform.Rotate(base.transform.right, this.RotationSpeed * Time.deltaTime);
			}
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x000696B4 File Offset: 0x000678B4
		protected virtual void Turn()
		{
			if (!this._turning)
			{
				MMFeedbacks turnFeedback = this.TurnFeedback;
				if (turnFeedback != null)
				{
					turnFeedback.PlayFeedbacks();
				}
			}
			this._turning = true;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x000696D6 File Offset: 0x000678D6
		protected virtual void TurnStop()
		{
			MMFeedbacks turnFeedback = this.TurnFeedback;
			if (turnFeedback != null)
			{
				turnFeedback.StopFeedbacks();
			}
			MMFeedbacks turnStopFeedback = this.TurnStopFeedback;
			if (turnStopFeedback != null)
			{
				turnStopFeedback.PlayFeedbacks();
			}
			this._turning = false;
		}

		// Token: 0x04001CED RID: 7405
		[Header("Binding")]
		[Tooltip("the part of the wheel that rotates")]
		public Transform RotatingPart;

		// Token: 0x04001CEE RID: 7406
		[Header("Settings")]
		[Tooltip("the speed at which the wheel should rotate")]
		public float RotationSpeed = 20f;

		// Token: 0x04001CEF RID: 7407
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when the wheel starts turning")]
		public MMFeedbacks TurnFeedback;

		// Token: 0x04001CF0 RID: 7408
		[Tooltip("a feedback to call when the wheel stops turning")]
		public MMFeedbacks TurnStopFeedback;

		// Token: 0x04001CF1 RID: 7409
		protected bool _turning;
	}
}
