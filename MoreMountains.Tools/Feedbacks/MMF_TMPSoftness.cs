using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C3 RID: 707
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you tweak the softness of a TMP text over time.")]
	[FeedbackPath("TextMesh Pro/TMP Softness")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPSoftness : MMF_Feedback
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00056376 File Offset: 0x00054576
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00056379 File Offset: 0x00054579
		// (set) Token: 0x06001401 RID: 5121 RVA: 0x00056396 File Offset: 0x00054596
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
				this.Duration = value;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x0005639F File Offset: 0x0005459F
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000563A2 File Offset: 0x000545A2
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000563B0 File Offset: 0x000545B0
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (!this.Active)
			{
				return;
			}
			this._initialSoftness = this.TargetTMPText.fontMaterial.GetFloat(ShaderUtilities.ID_OutlineSoftness);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000563E0 File Offset: 0x000545E0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.TargetTMPText == null)
			{
				return;
			}
			if (this.Active && MMF_TMPSoftness.FeedbackTypeAuthorized)
			{
				MMFeedbackBase.Modes mode = this.Mode;
				if (mode != MMFeedbackBase.Modes.OverTime)
				{
					if (mode == MMFeedbackBase.Modes.Instant)
					{
						this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, this.InstantSoftness);
						this.TargetTMPText.UpdateMeshPadding();
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
					this._coroutine = this.Owner.StartCoroutine(this.ApplyValueOverTime());
				}
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00056484 File Offset: 0x00054684
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPSoftness.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000564D0 File Offset: 0x000546D0
		protected virtual IEnumerator ApplyValueOverTime()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValue(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValue(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000564E0 File Offset: 0x000546E0
		protected virtual void SetValue(float time)
		{
			float num = MMTween.Tween(time, 0f, 1f, this.RemapZero, this.RemapOne, this.SoftnessCurve);
			if (this.RelativeValues)
			{
				num += this._initialSoftness;
			}
			this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, num);
			this.TargetTMPText.UpdateMeshPadding();
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00056542 File Offset: 0x00054742
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPSoftness.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, this._initialSoftness);
			this.TargetTMPText.UpdateMeshPadding();
		}

		// Token: 0x0400170A RID: 5898
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400170B RID: 5899
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x0400170C RID: 5900
		[MMFInspectorGroup("Softness", true, 13, false, false)]
		[Tooltip("whether or not values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x0400170D RID: 5901
		[Tooltip("the selected mode")]
		public MMFeedbackBase.Modes Mode;

		// Token: 0x0400170E RID: 5902
		[Tooltip("the duration of the feedback, in seconds")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.5f;

		// Token: 0x0400170F RID: 5903
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType SoftnessCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001710 RID: 5904
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x04001711 RID: 5905
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x04001712 RID: 5906
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantSoftness;

		// Token: 0x04001713 RID: 5907
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001714 RID: 5908
		protected float _initialSoftness;

		// Token: 0x04001715 RID: 5909
		protected Coroutine _coroutine;
	}
}
