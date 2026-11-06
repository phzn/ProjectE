using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001CD RID: 461
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will allow you to cross fade a target Animator to the specified state.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Animation/Animation Crossfade")]
	public class MMF_AnimationCrossfade : MMF_Feedback
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x000333E7 File Offset: 0x000315E7
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x000333F5 File Offset: 0x000315F5
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.DeclaredDuration);
			}
			set
			{
				this.DeclaredDuration = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x000333FE File Offset: 0x000315FE
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00033401 File Offset: 0x00031601
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00033404 File Offset: 0x00031604
		protected override void AutomateTargetAcquisition()
		{
			this.BoundAnimator = this.FindAutomatedTarget<Animator>();
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00033412 File Offset: 0x00031612
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._stateHashName = Animator.StringToHash(this.StateName);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0003342C File Offset: 0x0003162C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AnimationCrossfade.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.BoundAnimator == null)
			{
				Debug.LogWarning("No animator was set for " + this.Owner.name);
				return;
			}
			this.CrossFade(this.BoundAnimator);
			foreach (Animator animator in this.ExtraBoundAnimators)
			{
				this.CrossFade(animator);
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000334C4 File Offset: 0x000316C4
		protected virtual void CrossFade(Animator targetAnimator)
		{
			MMF_AnimationCrossfade.Modes mode = this.Mode;
			if (mode == MMF_AnimationCrossfade.Modes.Seconds)
			{
				targetAnimator.CrossFadeInFixedTime(this._stateHashName, this.TransitionDuration, this.Layer, this.TimeOffset, this.NormalizedTransitionTime);
				return;
			}
			if (mode != MMF_AnimationCrossfade.Modes.Normalized)
			{
				return;
			}
			targetAnimator.CrossFade(this._stateHashName, this.NormalizedTransitionDuration, this.Layer, this.NormalizedTimeOffset, this.NormalizedTransitionTime);
		}

		// Token: 0x04000D56 RID: 3414
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D57 RID: 3415
		[MMFInspectorGroup("Animation", true, 12, true, false)]
		[Tooltip("the animator whose parameters you want to update")]
		public Animator BoundAnimator;

		// Token: 0x04000D58 RID: 3416
		[Tooltip("the list of extra animators whose parameters you want to update")]
		public List<Animator> ExtraBoundAnimators;

		// Token: 0x04000D59 RID: 3417
		[Tooltip("the duration for the player to consider. This won't impact your animation, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual animation, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04000D5A RID: 3418
		[MMFInspectorGroup("CrossFade", true, 16, false, false)]
		[Tooltip("the name of the state towards which to transition. That's the name of the yellow or gray box in your Animator")]
		public string StateName = "NewState";

		// Token: 0x04000D5B RID: 3419
		[Tooltip("the ID of the Animator layer you want the crossfade to occur on")]
		public int Layer = -1;

		// Token: 0x04000D5C RID: 3420
		[Tooltip("whether to specify timing data for the crossfade in seconds or in normalized (0-1) values")]
		public MMF_AnimationCrossfade.Modes Mode;

		// Token: 0x04000D5D RID: 3421
		[Tooltip("in Seconds mode, the duration of the transition, in seconds")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float TransitionDuration = 0.1f;

		// Token: 0x04000D5E RID: 3422
		[Tooltip("in Seconds mode, the offset at which to transition to, in seconds")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float TimeOffset;

		// Token: 0x04000D5F RID: 3423
		[Tooltip("in Normalized mode, the duration of the transition, normalized between 0 and 1")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float NormalizedTransitionDuration = 0.1f;

		// Token: 0x04000D60 RID: 3424
		[Tooltip("in Normalized mode, the offset at which to transition to, normalized between 0 and 1")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float NormalizedTimeOffset;

		// Token: 0x04000D61 RID: 3425
		[Tooltip("according to Unity's docs, 'the time of the transition, normalized'. Really nobody's sure what this does. It's optional.")]
		public float NormalizedTransitionTime;

		// Token: 0x04000D62 RID: 3426
		protected int _stateHashName;

		// Token: 0x020004A7 RID: 1191
		public enum TriggerModes
		{
			// Token: 0x040021B4 RID: 8628
			SetTrigger,
			// Token: 0x040021B5 RID: 8629
			ResetTrigger
		}

		// Token: 0x020004A8 RID: 1192
		public enum ValueModes
		{
			// Token: 0x040021B7 RID: 8631
			None,
			// Token: 0x040021B8 RID: 8632
			Constant,
			// Token: 0x040021B9 RID: 8633
			Random,
			// Token: 0x040021BA RID: 8634
			Incremental
		}

		// Token: 0x020004A9 RID: 1193
		public enum Modes
		{
			// Token: 0x040021BC RID: 8636
			Seconds,
			// Token: 0x040021BD RID: 8637
			Normalized
		}
	}
}
