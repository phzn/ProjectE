using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E0 RID: 480
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you enable/disable/toggle a target collider 2D, or change its trigger status")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Collider2D")]
	public class MMF_Collider2D : MMF_Feedback
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x000354DD File Offset: 0x000336DD
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000354E0 File Offset: 0x000336E0
		protected override void AutomateTargetAcquisition()
		{
			this.TargetCollider2D = this.FindAutomatedTarget<Collider2D>();
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000354EE File Offset: 0x000336EE
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Collider2D.FeedbackTypeAuthorized || this.TargetCollider2D == null)
			{
				return;
			}
			this.ApplyChanges(this.Mode);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0003551C File Offset: 0x0003371C
		protected virtual void ApplyChanges(MMF_Collider2D.Modes mode)
		{
			switch (mode)
			{
			case MMF_Collider2D.Modes.Enable:
				this._initialState = this.TargetCollider2D.enabled;
				this.TargetCollider2D.enabled = true;
				return;
			case MMF_Collider2D.Modes.Disable:
				this._initialState = this.TargetCollider2D.enabled;
				this.TargetCollider2D.enabled = false;
				return;
			case MMF_Collider2D.Modes.ToggleActive:
				this._initialState = this.TargetCollider2D.enabled;
				this.TargetCollider2D.enabled = !this.TargetCollider2D.enabled;
				return;
			case MMF_Collider2D.Modes.Trigger:
				this._initialState = this.TargetCollider2D.isTrigger;
				this.TargetCollider2D.isTrigger = true;
				return;
			case MMF_Collider2D.Modes.NonTrigger:
				this._initialState = this.TargetCollider2D.isTrigger;
				this.TargetCollider2D.isTrigger = false;
				return;
			case MMF_Collider2D.Modes.ToggleTrigger:
				this._initialState = this.TargetCollider2D.isTrigger;
				this.TargetCollider2D.isTrigger = !this.TargetCollider2D.isTrigger;
				return;
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003562C File Offset: 0x0003382C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Collider2D.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_Collider2D.Modes.Enable:
				this.TargetCollider2D.enabled = this._initialState;
				return;
			case MMF_Collider2D.Modes.Disable:
				this.TargetCollider2D.enabled = this._initialState;
				return;
			case MMF_Collider2D.Modes.ToggleActive:
				this.TargetCollider2D.enabled = this._initialState;
				return;
			case MMF_Collider2D.Modes.Trigger:
				this.TargetCollider2D.isTrigger = this._initialState;
				return;
			case MMF_Collider2D.Modes.NonTrigger:
				this.TargetCollider2D.isTrigger = this._initialState;
				return;
			case MMF_Collider2D.Modes.ToggleTrigger:
				this.TargetCollider2D.isTrigger = this._initialState;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000DE9 RID: 3561
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DEA RID: 3562
		[MMFInspectorGroup("Collider 2D", true, 12, true, false)]
		[Tooltip("the collider to act upon")]
		public Collider2D TargetCollider2D;

		// Token: 0x04000DEB RID: 3563
		public MMF_Collider2D.Modes Mode = MMF_Collider2D.Modes.Disable;

		// Token: 0x04000DEC RID: 3564
		protected bool _initialState;

		// Token: 0x020004AE RID: 1198
		public enum Modes
		{
			// Token: 0x040021D3 RID: 8659
			Enable,
			// Token: 0x040021D4 RID: 8660
			Disable,
			// Token: 0x040021D5 RID: 8661
			ToggleActive,
			// Token: 0x040021D6 RID: 8662
			Trigger,
			// Token: 0x040021D7 RID: 8663
			NonTrigger,
			// Token: 0x040021D8 RID: 8664
			ToggleTrigger
		}
	}
}
