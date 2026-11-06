using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DF RID: 479
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you enable/disable/toggle a target collider, or change its trigger status")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Collider")]
	public class MMF_Collider : MMF_Feedback
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x000352C8 File Offset: 0x000334C8
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x000352CB File Offset: 0x000334CB
		protected override void AutomateTargetAcquisition()
		{
			this.TargetCollider = this.FindAutomatedTarget<Collider>();
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000352D9 File Offset: 0x000334D9
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Collider.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetCollider != null)
			{
				this.ApplyChanges(this.Mode);
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00035308 File Offset: 0x00033508
		protected virtual void ApplyChanges(MMF_Collider.Modes mode)
		{
			switch (mode)
			{
			case MMF_Collider.Modes.Enable:
				this._initialState = this.TargetCollider.enabled;
				this.TargetCollider.enabled = true;
				return;
			case MMF_Collider.Modes.Disable:
				this._initialState = this.TargetCollider.enabled;
				this.TargetCollider.enabled = false;
				return;
			case MMF_Collider.Modes.ToggleActive:
				this._initialState = this.TargetCollider.enabled;
				this.TargetCollider.enabled = !this.TargetCollider.enabled;
				return;
			case MMF_Collider.Modes.Trigger:
				this._initialState = this.TargetCollider.isTrigger;
				this.TargetCollider.isTrigger = true;
				return;
			case MMF_Collider.Modes.NonTrigger:
				this._initialState = this.TargetCollider.isTrigger;
				this.TargetCollider.isTrigger = false;
				return;
			case MMF_Collider.Modes.ToggleTrigger:
				this._initialState = this.TargetCollider.isTrigger;
				this.TargetCollider.isTrigger = !this.TargetCollider.isTrigger;
				return;
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00035418 File Offset: 0x00033618
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Collider.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_Collider.Modes.Enable:
				this.TargetCollider.enabled = this._initialState;
				return;
			case MMF_Collider.Modes.Disable:
				this.TargetCollider.enabled = this._initialState;
				return;
			case MMF_Collider.Modes.ToggleActive:
				this.TargetCollider.enabled = this._initialState;
				return;
			case MMF_Collider.Modes.Trigger:
				this.TargetCollider.isTrigger = this._initialState;
				return;
			case MMF_Collider.Modes.NonTrigger:
				this.TargetCollider.isTrigger = this._initialState;
				return;
			case MMF_Collider.Modes.ToggleTrigger:
				this.TargetCollider.isTrigger = this._initialState;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000DE5 RID: 3557
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DE6 RID: 3558
		[MMFInspectorGroup("Collider", true, 12, true, false)]
		[Tooltip("the collider to act upon")]
		public Collider TargetCollider;

		// Token: 0x04000DE7 RID: 3559
		public MMF_Collider.Modes Mode = MMF_Collider.Modes.Disable;

		// Token: 0x04000DE8 RID: 3560
		protected bool _initialState;

		// Token: 0x020004AD RID: 1197
		public enum Modes
		{
			// Token: 0x040021CC RID: 8652
			Enable,
			// Token: 0x040021CD RID: 8653
			Disable,
			// Token: 0x040021CE RID: 8654
			ToggleActive,
			// Token: 0x040021CF RID: 8655
			Trigger,
			// Token: 0x040021D0 RID: 8656
			NonTrigger,
			// Token: 0x040021D1 RID: 8657
			ToggleTrigger
		}
	}
}
