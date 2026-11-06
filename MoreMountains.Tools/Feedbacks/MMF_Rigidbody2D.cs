using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000202 RID: 514
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you apply forces and torques (relative or not) to a Rigidbody.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Rigidbody2D")]
	public class MMF_Rigidbody2D : MMF_Feedback
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0003C801 File Offset: 0x0003AA01
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0003C804 File Offset: 0x0003AA04
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRigidbody2D = this.FindAutomatedTarget<Rigidbody2D>();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003C814 File Offset: 0x0003AA14
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Rigidbody2D.FeedbackTypeAuthorized || this.TargetRigidbody2D == null)
			{
				return;
			}
			this.ApplyForce(this.TargetRigidbody2D, feedbacksIntensity);
			foreach (Rigidbody2D rigidbody2D in this.ExtraTargetRigidbodies2D)
			{
				this.ApplyForce(rigidbody2D, feedbacksIntensity);
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003C894 File Offset: 0x0003AA94
		protected virtual void ApplyForce(Rigidbody2D rb, float feedbacksIntensity)
		{
			if (this.ResetVelocityOnPlay)
			{
				rb.velocity = Vector2.zero;
			}
			switch (this.Mode)
			{
			case MMF_Rigidbody2D.Modes.AddForce:
				this._force.x = Random.Range(this.MinForce.x, this.MaxForce.x);
				this._force.y = Random.Range(this.MinForce.y, this.MaxForce.y);
				if (!this.Timing.ConstantIntensity)
				{
					this._force *= feedbacksIntensity;
				}
				rb.AddForce(this._force, this.AppliedForceMode);
				return;
			case MMF_Rigidbody2D.Modes.AddRelativeForce:
				this._force.x = Random.Range(this.MinForce.x, this.MaxForce.x);
				this._force.y = Random.Range(this.MinForce.y, this.MaxForce.y);
				if (!this.Timing.ConstantIntensity)
				{
					this._force *= feedbacksIntensity;
				}
				rb.AddRelativeForce(this._force, this.AppliedForceMode);
				return;
			case MMF_Rigidbody2D.Modes.AddTorque:
				this._torque = Random.Range(this.MinTorque, this.MaxTorque);
				if (!this.Timing.ConstantIntensity)
				{
					this._torque *= feedbacksIntensity;
				}
				rb.AddTorque(this._torque, this.AppliedForceMode);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000FBD RID: 4029
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FBE RID: 4030
		[MMFInspectorGroup("Rigidbody2D", true, 32, true, false)]
		[Tooltip("the rigidbody to target on play")]
		public Rigidbody2D TargetRigidbody2D;

		// Token: 0x04000FBF RID: 4031
		[Tooltip("an extra list of rigidbodies to target on play")]
		public List<Rigidbody2D> ExtraTargetRigidbodies2D;

		// Token: 0x04000FC0 RID: 4032
		[Tooltip("the selected mode for this feedback")]
		public MMF_Rigidbody2D.Modes Mode;

		// Token: 0x04000FC1 RID: 4033
		[Tooltip("the min force or torque to apply")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 MinForce;

		// Token: 0x04000FC2 RID: 4034
		[Tooltip("the max force or torque to apply")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 MaxForce;

		// Token: 0x04000FC3 RID: 4035
		[Tooltip("the min torque to apply to this rigidbody on play")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float MinTorque;

		// Token: 0x04000FC4 RID: 4036
		[Tooltip("the max torque to apply to this rigidbody on play")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float MaxTorque;

		// Token: 0x04000FC5 RID: 4037
		[Tooltip("the force mode to apply")]
		public ForceMode2D AppliedForceMode = ForceMode2D.Impulse;

		// Token: 0x04000FC6 RID: 4038
		[Tooltip("if this is true, the velocity of the rigidbody will be reset before applying the new force")]
		public bool ResetVelocityOnPlay;

		// Token: 0x04000FC7 RID: 4039
		protected Vector2 _force;

		// Token: 0x04000FC8 RID: 4040
		protected float _torque;

		// Token: 0x020004DD RID: 1245
		public enum Modes
		{
			// Token: 0x040022AF RID: 8879
			AddForce,
			// Token: 0x040022B0 RID: 8880
			AddRelativeForce,
			// Token: 0x040022B1 RID: 8881
			AddTorque
		}
	}
}
