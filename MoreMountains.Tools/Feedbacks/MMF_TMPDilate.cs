using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BD RID: 701
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you dilate a TMP text over time.")]
	[FeedbackPath("TextMesh Pro/TMP Dilate")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPDilate : MMF_Feedback
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x000558D9 File Offset: 0x00053AD9
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x000558DC File Offset: 0x00053ADC
		// (set) Token: 0x060013D4 RID: 5076 RVA: 0x000558F9 File Offset: 0x00053AF9
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00055902 File Offset: 0x00053B02
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00055905 File Offset: 0x00053B05
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00055913 File Offset: 0x00053B13
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (!this.Active)
			{
				return;
			}
			this._initialDilate = this.TargetTMPText.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00055940 File Offset: 0x00053B40
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPDilate.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			if (this.Active)
			{
				MMFeedbackBase.Modes mode = this.Mode;
				if (mode != MMFeedbackBase.Modes.OverTime)
				{
					if (mode == MMFeedbackBase.Modes.Instant)
					{
						this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, this.InstantDilate);
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

		// Token: 0x060013D9 RID: 5081 RVA: 0x000559EA File Offset: 0x00053BEA
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

		// Token: 0x060013DA RID: 5082 RVA: 0x000559FC File Offset: 0x00053BFC
		protected virtual void SetValue(float time)
		{
			float num = MMTween.Tween(time, 0f, 1f, this.RemapZero, this.RemapOne, this.DilateCurve);
			if (this.RelativeValues)
			{
				num += this._initialDilate;
			}
			this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, num);
			this.TargetTMPText.UpdateMeshPadding();
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00055A60 File Offset: 0x00053C60
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPDilate.FeedbackTypeAuthorized)
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

		// Token: 0x060013DC RID: 5084 RVA: 0x00055AAC File Offset: 0x00053CAC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPDilate.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, this._initialDilate);
			this.TargetTMPText.UpdateMeshPadding();
		}

		// Token: 0x040016DF RID: 5855
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016E0 RID: 5856
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016E1 RID: 5857
		[MMFInspectorGroup("Dilate", true, 16, false, false)]
		[Tooltip("whether or not values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x040016E2 RID: 5858
		[Tooltip("the selected mode")]
		public MMFeedbackBase.Modes Mode;

		// Token: 0x040016E3 RID: 5859
		[Tooltip("the duration of the feedback, in seconds")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.5f;

		// Token: 0x040016E4 RID: 5860
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType DilateCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0.5f)
		}));

		// Token: 0x040016E5 RID: 5861
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero = -1f;

		// Token: 0x040016E6 RID: 5862
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x040016E7 RID: 5863
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantDilate;

		// Token: 0x040016E8 RID: 5864
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040016E9 RID: 5865
		protected float _initialDilate;

		// Token: 0x040016EA RID: 5866
		protected Coroutine _coroutine;
	}
}
