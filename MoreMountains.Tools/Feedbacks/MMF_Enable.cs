using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E3 RID: 483
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to change the state of a behaviour on a target gameobject from active to inactive (or the opposite), on init, play, stop or reset. For each of these you can specify if you want to force a state (enabled or disabled), or toggle it (enabled becomes disabled, disabled becomes enabled).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Enable Behaviour")]
	public class MMF_Enable : MMF_Feedback
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x00035C8C File Offset: 0x00033E8C
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.TargetBehaviour != null && this.SetStateOnInit)
			{
				this.SetStatus(this.StateOnInit);
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00035CBF File Offset: 0x00033EBF
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Enable.FeedbackTypeAuthorized || this.TargetBehaviour == null)
			{
				return;
			}
			if (this.SetStateOnPlay)
			{
				this.SetStatus(this.StateOnPlay);
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00035CF3 File Offset: 0x00033EF3
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Enable.FeedbackTypeAuthorized || this.TargetBehaviour == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.SetStateOnStop)
			{
				this.SetStatus(this.StateOnStop);
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00035D30 File Offset: 0x00033F30
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.InCooldown)
			{
				return;
			}
			if (!this.Active || !MMF_Enable.FeedbackTypeAuthorized || this.TargetBehaviour == null)
			{
				return;
			}
			if (this.SetStateOnReset)
			{
				this.SetStatus(this.StateOnReset);
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00035D80 File Offset: 0x00033F80
		protected virtual void SetStatus(MMF_Enable.PossibleStates state)
		{
			this.SetStatus(state, this.TargetBehaviour);
			foreach (Behaviour behaviour in this.ExtraTargetBehaviours)
			{
				this.SetStatus(state, behaviour);
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00035DE4 File Offset: 0x00033FE4
		protected virtual void SetStatus(MMF_Enable.PossibleStates state, Behaviour target)
		{
			this._initialState = target.enabled;
			switch (state)
			{
			case MMF_Enable.PossibleStates.Enabled:
				target.enabled = this.NormalPlayDirection;
				return;
			case MMF_Enable.PossibleStates.Disabled:
				target.enabled = !this.NormalPlayDirection;
				return;
			case MMF_Enable.PossibleStates.Toggle:
				target.enabled = !target.enabled;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00035E48 File Offset: 0x00034048
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Enable.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetBehaviour.enabled = this._initialState;
			foreach (Behaviour behaviour in this.ExtraTargetBehaviours)
			{
				behaviour.enabled = this._initialState;
			}
		}

		// Token: 0x04000E1B RID: 3611
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E1C RID: 3612
		[MMFInspectorGroup("Enable Target Monobehaviour", true, 86, true, false)]
		[Tooltip("the gameobject we want to change the active state of")]
		public Behaviour TargetBehaviour;

		// Token: 0x04000E1D RID: 3613
		[Tooltip("a list of extra gameobjects we want to change the active state of")]
		public List<Behaviour> ExtraTargetBehaviours;

		// Token: 0x04000E1E RID: 3614
		[Tooltip("whether or not we should alter the state of the target object on init")]
		public bool SetStateOnInit;

		// Token: 0x04000E1F RID: 3615
		[MMFCondition("SetStateOnInit", true)]
		[Tooltip("how to change the state on init")]
		public MMF_Enable.PossibleStates StateOnInit = MMF_Enable.PossibleStates.Disabled;

		// Token: 0x04000E20 RID: 3616
		[Tooltip("whether or not we should alter the state of the target object on play")]
		public bool SetStateOnPlay;

		// Token: 0x04000E21 RID: 3617
		[MMFCondition("SetStateOnPlay", true)]
		[Tooltip("how to change the state on play")]
		public MMF_Enable.PossibleStates StateOnPlay = MMF_Enable.PossibleStates.Disabled;

		// Token: 0x04000E22 RID: 3618
		[Tooltip("whether or not we should alter the state of the target object on stop")]
		public bool SetStateOnStop;

		// Token: 0x04000E23 RID: 3619
		[Tooltip("how to change the state on stop")]
		[MMFCondition("SetStateOnStop", true)]
		public MMF_Enable.PossibleStates StateOnStop = MMF_Enable.PossibleStates.Disabled;

		// Token: 0x04000E24 RID: 3620
		[Tooltip("whether or not we should alter the state of the target object on reset")]
		public bool SetStateOnReset;

		// Token: 0x04000E25 RID: 3621
		[Tooltip("how to change the state on reset")]
		[MMFCondition("SetStateOnReset", true)]
		public MMF_Enable.PossibleStates StateOnReset = MMF_Enable.PossibleStates.Disabled;

		// Token: 0x04000E26 RID: 3622
		protected bool _initialState;

		// Token: 0x020004B2 RID: 1202
		public enum PossibleStates
		{
			// Token: 0x040021E5 RID: 8677
			Enabled,
			// Token: 0x040021E6 RID: 8678
			Disabled,
			// Token: 0x040021E7 RID: 8679
			Toggle
		}
	}
}
