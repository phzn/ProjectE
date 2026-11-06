using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B7 RID: 695
	public abstract class MMF_FeedbackBase : MMF_Feedback
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00054992 File Offset: 0x00052B92
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x000549AF File Offset: 0x00052BAF
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_FeedbackBase.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				if (this.Mode != MMF_FeedbackBase.Modes.Instant)
				{
					this.Duration = value;
				}
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x000549C1 File Offset: 0x00052BC1
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x000549C4 File Offset: 0x00052BC4
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000549C7 File Offset: 0x00052BC7
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.PrepareTargets();
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x000549ED File Offset: 0x00052BED
		public virtual void PrepareTargets()
		{
			this._targets = new List<MMF_FeedbackBaseTarget>();
			this.FillTargets();
			this.InitializeTargets();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00054A06 File Offset: 0x00052C06
		public override void OnValidate()
		{
			base.OnValidate();
			this.PrepareTargets();
		}

		// Token: 0x0600138E RID: 5006
		protected abstract void FillTargets();

		// Token: 0x0600138F RID: 5007 RVA: 0x00054A14 File Offset: 0x00052C14
		protected virtual void InitializeTargets()
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				mmf_FeedbackBaseTarget.Target.Initialization(this.Owner.gameObject);
				mmf_FeedbackBaseTarget.InitialLevel = mmf_FeedbackBaseTarget.Target.Level;
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00054A94 File Offset: 0x00052C94
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_FeedbackBase.FeedbackTypeAuthorized)
			{
				if (!this.CanPlay())
				{
					return;
				}
				this.Turn(true);
				MMF_FeedbackBase.Modes mode = this.Mode;
				if (mode != MMF_FeedbackBase.Modes.OverTime)
				{
					if (mode == MMF_FeedbackBase.Modes.Instant)
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
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.UpdateValueSequence(feedbacksIntensity, position));
				}
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00054B18 File Offset: 0x00052D18
		protected virtual void Instant()
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				mmf_FeedbackBaseTarget.Target.SetLevel(mmf_FeedbackBaseTarget.InstantLevel);
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00054B84 File Offset: 0x00052D84
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_FeedbackBase.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				mmf_FeedbackBaseTarget.Target.SetLevel(mmf_FeedbackBaseTarget.InitialLevel);
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00054C00 File Offset: 0x00052E00
		protected virtual IEnumerator UpdateValueSequence(float feedbacksIntensity, Vector3 position)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValues(num, feedbacksIntensity, position);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValues(this.FinalNormalizedTime, feedbacksIntensity, position);
			if (this.EndsOff)
			{
				this.Turn(false);
			}
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00054C20 File Offset: 0x00052E20
		protected virtual void SetValues(float time, float feedbacksIntensity, Vector3 position)
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				float num2 = MMTween.Tween(time, 0f, 1f, mmf_FeedbackBaseTarget.RemapLevelZero, mmf_FeedbackBaseTarget.RemapLevelOne, mmf_FeedbackBaseTarget.LevelCurve);
				if (this.RelativeValues)
				{
					num2 += mmf_FeedbackBaseTarget.InitialLevel;
				}
				mmf_FeedbackBaseTarget.Target.SetLevel(num2 * num);
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00054CC8 File Offset: 0x00052EC8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active)
			{
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
					this._coroutine = null;
				}
				this.IsPlaying = false;
				if (this.DisableOnStop)
				{
					this.Turn(false);
				}
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00054D1C File Offset: 0x00052F1C
		protected virtual void Turn(bool status)
		{
			if (this._targets.Count == 0)
			{
				return;
			}
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				if (mmf_FeedbackBaseTarget.Target.TargetComponent.gameObject != null)
				{
					mmf_FeedbackBaseTarget.Target.TargetComponent.gameObject.SetActive(status);
				}
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00054DA4 File Offset: 0x00052FA4
		protected virtual bool CanPlay()
		{
			if (this._targets.Count == 0)
			{
				return false;
			}
			foreach (MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget in this._targets)
			{
				if (this.OnlyPlayIfTargetIsActive && !mmf_FeedbackBaseTarget.Target.TargetComponent.gameObject.activeInHierarchy)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400169D RID: 5789
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400169E RID: 5790
		[MMFInspectorGroup("Mode", true, 64, false, false)]
		[Tooltip("whether the feedback should affect the target property instantly or over a period of time")]
		public MMF_FeedbackBase.Modes Mode;

		// Token: 0x0400169F RID: 5791
		[Tooltip("how long the target property should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x040016A0 RID: 5792
		[Tooltip("whether or not that target property should be turned off on start")]
		public bool StartsOff;

		// Token: 0x040016A1 RID: 5793
		[Tooltip("whether or not that target property should be turned off once the feedback is done playing")]
		public bool EndsOff;

		// Token: 0x040016A2 RID: 5794
		[Tooltip("whether or not the values should be relative or not")]
		public bool RelativeValues = true;

		// Token: 0x040016A3 RID: 5795
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040016A4 RID: 5796
		[Tooltip("if this is true, the target object will be disabled on stop")]
		public bool DisableOnStop;

		// Token: 0x040016A5 RID: 5797
		[Tooltip("if this is true, this feedback will only play if its target is active in hierarchy")]
		public bool OnlyPlayIfTargetIsActive;

		// Token: 0x040016A6 RID: 5798
		protected List<MMF_FeedbackBaseTarget> _targets;

		// Token: 0x040016A7 RID: 5799
		protected Coroutine _coroutine;

		// Token: 0x02000546 RID: 1350
		public enum Modes
		{
			// Token: 0x0400243B RID: 9275
			OverTime,
			// Token: 0x0400243C RID: 9276
			Instant
		}
	}
}
