using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001CE RID: 462
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the speed of a target animator, either once, or instantly and then reset it, or interpolate it over time")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Animation/Animator Speed")]
	public class MMF_AnimatorSpeed : MMF_Feedback
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00033561 File Offset: 0x00031761
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00033564 File Offset: 0x00031764
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00033567 File Offset: 0x00031767
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0003356A File Offset: 0x0003176A
		protected override void AutomateTargetAcquisition()
		{
			this.BoundAnimator = this.FindAutomatedTarget<Animator>();
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00033578 File Offset: 0x00031778
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AnimatorSpeed.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.BoundAnimator == null)
			{
				Debug.LogWarning("No animator was set for " + this.Owner.name);
				return;
			}
			if (!this.IsPlaying)
			{
				this._initialSpeed = this.BoundAnimator.speed;
			}
			if (this.Mode == MMF_AnimatorSpeed.SpeedModes.Once)
			{
				this.BoundAnimator.speed = this.ComputeIntensity(this.DetermineNewSpeed(), position);
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this._coroutine = this.Owner.StartCoroutine(this.ChangeSpeedCo());
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00033628 File Offset: 0x00031828
		protected virtual IEnumerator ChangeSpeedCo()
		{
			if (this.Mode == MMF_AnimatorSpeed.SpeedModes.InstantThenReset)
			{
				this.IsPlaying = true;
				this.BoundAnimator.speed = this.DetermineNewSpeed();
				yield return MMCoroutine.WaitFor(this.Duration);
				this.BoundAnimator.speed = this._initialSpeed;
				this.IsPlaying = false;
			}
			else if (this.Mode == MMF_AnimatorSpeed.SpeedModes.OverTime)
			{
				this.IsPlaying = true;
				this._startedAt = this.FeedbackTime;
				float newTargetSpeed = this.DetermineNewSpeed();
				while (this.FeedbackTime - this._startedAt < this.Duration)
				{
					float num = MMFeedbacksHelpers.Remap(this.FeedbackTime - this._startedAt, 0f, this.Duration, 0f, 1f);
					float num2 = this.Curve.Evaluate(num);
					this.BoundAnimator.speed = Mathf.Max(0f, MMFeedbacksHelpers.Remap(num2, 0f, 1f, this._initialSpeed, newTargetSpeed));
					yield return null;
				}
				this.BoundAnimator.speed = this._initialSpeed;
				this.IsPlaying = false;
			}
			yield break;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00033637 File Offset: 0x00031837
		protected virtual float DetermineNewSpeed()
		{
			return Mathf.Abs(Random.Range(this.NewSpeedMin, this.NewSpeedMax));
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0003364F File Offset: 0x0003184F
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AnimatorSpeed.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this.BoundAnimator.speed = this._initialSpeed;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0003368B File Offset: 0x0003188B
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AnimatorSpeed.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundAnimator.speed = this._initialSpeed;
		}

		// Token: 0x04000D63 RID: 3427
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D64 RID: 3428
		[MMFInspectorGroup("Animation", true, 12, true, false)]
		[Tooltip("the animator whose parameters you want to update")]
		public Animator BoundAnimator;

		// Token: 0x04000D65 RID: 3429
		[MMFInspectorGroup("Speed", true, 14, true, false)]
		[Tooltip("whether to change the speed of the target animator once, instantly and reset it later, or have it change over time")]
		public MMF_AnimatorSpeed.SpeedModes Mode;

		// Token: 0x04000D66 RID: 3430
		[Tooltip("the new minimum speed at which to set the animator - value will be randomized between min and max")]
		public float NewSpeedMin;

		// Token: 0x04000D67 RID: 3431
		[Tooltip("the new maximum speed at which to set the animator - value will be randomized between min and max")]
		public float NewSpeedMax;

		// Token: 0x04000D68 RID: 3432
		[Tooltip("when in instant then reset or over time modes, the duration of the effect")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public float Duration = 1f;

		// Token: 0x04000D69 RID: 3433
		[Tooltip("when in over time mode, the curve against which to evaluate the new speed")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000D6A RID: 3434
		protected Coroutine _coroutine;

		// Token: 0x04000D6B RID: 3435
		protected float _initialSpeed;

		// Token: 0x04000D6C RID: 3436
		protected float _startedAt;

		// Token: 0x020004AA RID: 1194
		public enum SpeedModes
		{
			// Token: 0x040021BF RID: 8639
			Once,
			// Token: 0x040021C0 RID: 8640
			InstantThenReset,
			// Token: 0x040021C1 RID: 8641
			OverTime
		}
	}
}
