using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Feel
{
	// Token: 0x02000379 RID: 889
	[AddComponentMenu("")]
	public class GettingStartedTutorialHeroReference : MonoBehaviour
	{
		// Token: 0x0600185F RID: 6239 RVA: 0x00067A22 File Offset: 0x00065C22
		private void Awake()
		{
			this._rigidbody = base.gameObject.GetComponent<Rigidbody>();
			Physics.gravity = Vector3.down * 30f;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00067A4C File Offset: 0x00065C4C
		private void Update()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame() && !this._jumping)
			{
				this.Jump();
			}
			if (this._jumping && this._velocityLastFrame < 0f && Mathf.Abs(this._rigidbody.velocity.y) < 0.1f)
			{
				this._jumping = false;
				MMFeedbacks landingFeedback = this.LandingFeedback;
				if (landingFeedback != null)
				{
					landingFeedback.PlayFeedbacks();
				}
				if (this.OnLand != null)
				{
					this.OnLand.Invoke();
				}
			}
			this._velocityLastFrame = this._rigidbody.velocity.y;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00067AE0 File Offset: 0x00065CE0
		private void Jump()
		{
			this._rigidbody.AddForce(Vector3.up * this.JumpForce, ForceMode.Impulse);
			this._jumping = true;
			MMFeedbacks jumpFeedback = this.JumpFeedback;
			if (jumpFeedback != null)
			{
				jumpFeedback.PlayFeedbacks();
			}
			if (this.OnJump != null)
			{
				this.OnJump.Invoke();
			}
		}

		// Token: 0x04001C5B RID: 7259
		[Header("Hero Settings")]
		public KeyCode ActionKey = KeyCode.Space;

		// Token: 0x04001C5C RID: 7260
		public float JumpForce = 8f;

		// Token: 0x04001C5D RID: 7261
		[Header("Feedbacks")]
		public MMFeedbacks JumpFeedback;

		// Token: 0x04001C5E RID: 7262
		public MMFeedbacks LandingFeedback;

		// Token: 0x04001C5F RID: 7263
		[Header("Events")]
		public UnityEvent OnJump;

		// Token: 0x04001C60 RID: 7264
		public UnityEvent OnLand;

		// Token: 0x04001C61 RID: 7265
		private const float _lowVelocity = 0.1f;

		// Token: 0x04001C62 RID: 7266
		private Rigidbody _rigidbody;

		// Token: 0x04001C63 RID: 7267
		private float _velocityLastFrame;

		// Token: 0x04001C64 RID: 7268
		private bool _jumping;
	}
}
