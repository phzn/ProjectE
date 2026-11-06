using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000201 RID: 513
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you apply forces and torques (relative or not) to a Rigidbody.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Rigidbody")]
	public class MMF_Rigidbody : MMF_Feedback
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0003C615 File Offset: 0x0003A815
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003C618 File Offset: 0x0003A818
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRigidbody = this.FindAutomatedTarget<Rigidbody>();
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0003C628 File Offset: 0x0003A828
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Rigidbody.FeedbackTypeAuthorized || this.TargetRigidbody == null)
			{
				return;
			}
			this._force.x = Random.Range(this.MinForce.x, this.MaxForce.x);
			this._force.y = Random.Range(this.MinForce.y, this.MaxForce.y);
			this._force.z = Random.Range(this.MinForce.z, this.MaxForce.z);
			if (!this.Timing.ConstantIntensity)
			{
				this._force *= feedbacksIntensity;
			}
			this.ApplyForce(this.TargetRigidbody);
			foreach (Rigidbody rigidbody in this.ExtraTargetRigidbodies)
			{
				this.ApplyForce(rigidbody);
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0003C738 File Offset: 0x0003A938
		protected virtual void ApplyForce(Rigidbody rb)
		{
			if (this.ResetVelocityOnPlay)
			{
				rb.velocity = Vector3.zero;
			}
			if (this.ForwardForce)
			{
				this._force = this._force.magnitude * rb.transform.forward;
			}
			switch (this.Mode)
			{
			case MMF_Rigidbody.Modes.AddForce:
				rb.AddForce(this._force, this.AppliedForceMode);
				return;
			case MMF_Rigidbody.Modes.AddRelativeForce:
				rb.AddRelativeForce(this._force, this.AppliedForceMode);
				return;
			case MMF_Rigidbody.Modes.AddTorque:
				rb.AddTorque(this._force, this.AppliedForceMode);
				return;
			case MMF_Rigidbody.Modes.AddRelativeTorque:
				rb.AddRelativeTorque(this._force, this.AppliedForceMode);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000FB3 RID: 4019
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FB4 RID: 4020
		[MMFInspectorGroup("Rigidbody", true, 61, true, false)]
		[Tooltip("the rigidbody to target on play")]
		public Rigidbody TargetRigidbody;

		// Token: 0x04000FB5 RID: 4021
		[Tooltip("a list of extra rigidbodies to target on play")]
		public List<Rigidbody> ExtraTargetRigidbodies;

		// Token: 0x04000FB6 RID: 4022
		[Tooltip("the selected mode for this feedback")]
		public MMF_Rigidbody.Modes Mode;

		// Token: 0x04000FB7 RID: 4023
		[Tooltip("the min force or torque to apply")]
		public Vector3 MinForce;

		// Token: 0x04000FB8 RID: 4024
		[Tooltip("the max force or torque to apply")]
		public Vector3 MaxForce;

		// Token: 0x04000FB9 RID: 4025
		[Tooltip("the force mode to apply")]
		public ForceMode AppliedForceMode = ForceMode.Impulse;

		// Token: 0x04000FBA RID: 4026
		[Tooltip("if this is true, the velocity of the rigidbody will be reset before applying the new force")]
		public bool ResetVelocityOnPlay;

		// Token: 0x04000FBB RID: 4027
		[Tooltip("if this is true, the magnitude of the min/max force will be applied in the target transform's forward direction")]
		public bool ForwardForce;

		// Token: 0x04000FBC RID: 4028
		protected Vector3 _force;

		// Token: 0x020004DC RID: 1244
		public enum Modes
		{
			// Token: 0x040022AA RID: 8874
			AddForce,
			// Token: 0x040022AB RID: 8875
			AddRelativeForce,
			// Token: 0x040022AC RID: 8876
			AddTorque,
			// Token: 0x040022AD RID: 8877
			AddRelativeTorque
		}
	}
}
