using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B5 RID: 693
	public abstract class MMFeedbackBase : MMFeedback
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x0005459A File Offset: 0x0005279A
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x000545B7 File Offset: 0x000527B7
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMFeedbackBase.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				if (this.Mode != MMFeedbackBase.Modes.Instant)
				{
					this.Duration = value;
				}
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000545C9 File Offset: 0x000527C9
		protected override void CustomInitialization(GameObject owner)
		{
			base.CustomInitialization(owner);
			this.PrepareTargets();
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x000545EF File Offset: 0x000527EF
		protected virtual void PrepareTargets()
		{
			this._targets = new List<MMFeedbackBaseTarget>();
			this.FillTargets();
			this.InitializeTargets();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00054608 File Offset: 0x00052808
		protected virtual void OnValidate()
		{
			this.PrepareTargets();
		}

		// Token: 0x0600137C RID: 4988
		protected abstract void FillTargets();

		// Token: 0x0600137D RID: 4989 RVA: 0x00054610 File Offset: 0x00052810
		protected virtual void InitializeTargets()
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMFeedbackBaseTarget mmfeedbackBaseTarget in this._targets)
			{
				mmfeedbackBaseTarget.Target.Initialization(base.gameObject);
				mmfeedbackBaseTarget.InitialLevel = mmfeedbackBaseTarget.Target.Level;
			}
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0005468C File Offset: 0x0005288C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMFeedbackBase.FeedbackTypeAuthorized)
			{
				this.Turn(true);
				MMFeedbackBase.Modes mode = this.Mode;
				if (mode != MMFeedbackBase.Modes.OverTime)
				{
					if (mode == MMFeedbackBase.Modes.Instant)
					{
						this.Instant();
						return;
					}
				}
				else
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					this._coroutine = base.StartCoroutine(this.UpdateValueSequence(feedbacksIntensity));
				}
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000546E8 File Offset: 0x000528E8
		protected virtual void Instant()
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMFeedbackBaseTarget mmfeedbackBaseTarget in this._targets)
			{
				mmfeedbackBaseTarget.Target.SetLevel(mmfeedbackBaseTarget.InstantLevel);
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00054754 File Offset: 0x00052954
		protected virtual IEnumerator UpdateValueSequence(float feedbacksIntensity)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValues(num, feedbacksIntensity);
				journey += (this.NormalPlayDirection ? base.FeedbackDeltaTime : (-base.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValues(this.FinalNormalizedTime, feedbacksIntensity);
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0005476C File Offset: 0x0005296C
		protected virtual void SetValues(float time, float feedbacksIntensity)
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			float num = (this.Timing.ConstantIntensity ? 1f : feedbacksIntensity);
			foreach (MMFeedbackBaseTarget mmfeedbackBaseTarget in this._targets)
			{
				float num2 = MMTween.Tween(time, 0f, 1f, mmfeedbackBaseTarget.RemapLevelZero, mmfeedbackBaseTarget.RemapLevelOne, mmfeedbackBaseTarget.LevelCurve);
				if (this.RelativeValues)
				{
					num2 += mmfeedbackBaseTarget.InitialLevel;
				}
				mmfeedbackBaseTarget.Target.SetLevel(num2 * num);
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00054820 File Offset: 0x00052A20
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active)
			{
				if (this._coroutine != null)
				{
					base.StopCoroutine(this._coroutine);
					this._coroutine = null;
				}
				this.IsPlaying = false;
				if (this.DisableOnStop)
				{
					this.Turn(false);
				}
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00054870 File Offset: 0x00052A70
		protected virtual void Turn(bool status)
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMFeedbackBaseTarget mmfeedbackBaseTarget in this._targets)
			{
				if (mmfeedbackBaseTarget.Target.TargetComponent.gameObject != null)
				{
					mmfeedbackBaseTarget.Target.TargetComponent.gameObject.SetActive(status);
				}
			}
		}

		// Token: 0x0400168E RID: 5774
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400168F RID: 5775
		[Header("Mode")]
		[Tooltip("whether the feedback should affect the target property instantly or over a period of time")]
		public MMFeedbackBase.Modes Mode;

		// Token: 0x04001690 RID: 5776
		[Tooltip("how long the target property should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x04001691 RID: 5777
		[Tooltip("whether or not that target property should be turned off on start")]
		public bool StartsOff;

		// Token: 0x04001692 RID: 5778
		[Tooltip("whether or not the values should be relative or not")]
		public bool RelativeValues = true;

		// Token: 0x04001693 RID: 5779
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001694 RID: 5780
		[Tooltip("if this is true, the target object will be disabled on stop")]
		public bool DisableOnStop;

		// Token: 0x04001695 RID: 5781
		protected List<MMFeedbackBaseTarget> _targets;

		// Token: 0x04001696 RID: 5782
		protected Coroutine _coroutine;

		// Token: 0x02000544 RID: 1348
		public enum Modes
		{
			// Token: 0x04002433 RID: 9267
			OverTime,
			// Token: 0x04002434 RID: 9268
			Instant
		}
	}
}
