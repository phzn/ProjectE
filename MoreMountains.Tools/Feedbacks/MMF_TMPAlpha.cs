using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B8 RID: 696
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the alpha of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Alpha")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPAlpha : MMF_Feedback
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x00054E4A File Offset: 0x0005304A
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x00054E66 File Offset: 0x00053066
		public override float FeedbackDuration
		{
			get
			{
				if (this.AlphaMode != MMF_TMPAlpha.AlphaModes.Instant)
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

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x00054E6F File Offset: 0x0005306F
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00054E72 File Offset: 0x00053072
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00054E80 File Offset: 0x00053080
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00054E83 File Offset: 0x00053083
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialAlpha = this.TargetTMPText.alpha;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00054EAC File Offset: 0x000530AC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPAlpha.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			switch (this.AlphaMode)
			{
			case MMF_TMPAlpha.AlphaModes.Instant:
				this.TargetTMPText.alpha = this.InstantAlpha;
				return;
			case MMF_TMPAlpha.AlphaModes.Interpolate:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeAlpha());
				return;
			case MMF_TMPAlpha.AlphaModes.ToDestination:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				this._initialAlpha = this.TargetTMPText.alpha;
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeAlpha());
				return;
			default:
				return;
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00054F98 File Offset: 0x00053198
		protected virtual IEnumerator ChangeAlpha()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetAlpha(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetAlpha(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00054FA8 File Offset: 0x000531A8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPAlpha.FeedbackTypeAuthorized)
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

		// Token: 0x060013A3 RID: 5027 RVA: 0x00054FF4 File Offset: 0x000531F4
		protected virtual void SetAlpha(float time)
		{
			float num = 0f;
			if (this.AlphaMode == MMF_TMPAlpha.AlphaModes.Interpolate)
			{
				num = MMTween.Tween(time, 0f, 1f, this.CurveRemapZero, this.CurveRemapOne, this.Curve);
			}
			else if (this.AlphaMode == MMF_TMPAlpha.AlphaModes.ToDestination)
			{
				num = MMTween.Tween(time, 0f, 1f, this._initialAlpha, this.DestinationAlpha, this.Curve);
			}
			this.TargetTMPText.alpha = num;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0005506D File Offset: 0x0005326D
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPAlpha.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.alpha = this._initialAlpha;
		}

		// Token: 0x040016A8 RID: 5800
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016A9 RID: 5801
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip(" TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016AA RID: 5802
		[MMFInspectorGroup("Alpha", true, 16, false, false)]
		[Tooltip("the selected color mode :Instant : the alpha will change instantly to the target one,Curve : the alpha will be interpolated along the curve,interpolate : lerps from the current color to the destination one ")]
		public MMF_TMPAlpha.AlphaModes AlphaMode = MMF_TMPAlpha.AlphaModes.Interpolate;

		// Token: 0x040016AB RID: 5803
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("AlphaMode", new int[] { 1, 2 })]
		public float Duration = 0.2f;

		// Token: 0x040016AC RID: 5804
		[Tooltip("the alpha to apply when in instant mode")]
		[MMFEnumCondition("AlphaMode", new int[] { 0 })]
		public float InstantAlpha = 1f;

		// Token: 0x040016AD RID: 5805
		[Tooltip("the curve to use when interpolating towards the destination alpha")]
		[MMFEnumCondition("AlphaMode", new int[] { 1, 2 })]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x040016AE RID: 5806
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("AlphaMode", new int[] { 1 })]
		public float CurveRemapZero;

		// Token: 0x040016AF RID: 5807
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("AlphaMode", new int[] { 1 })]
		public float CurveRemapOne = 1f;

		// Token: 0x040016B0 RID: 5808
		[Tooltip("the alpha to aim towards when in ToDestination mode")]
		[MMFEnumCondition("AlphaMode", new int[] { 2 })]
		public float DestinationAlpha = 1f;

		// Token: 0x040016B1 RID: 5809
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040016B2 RID: 5810
		protected float _initialAlpha;

		// Token: 0x040016B3 RID: 5811
		protected Coroutine _coroutine;

		// Token: 0x02000548 RID: 1352
		public enum AlphaModes
		{
			// Token: 0x04002444 RID: 9284
			Instant,
			// Token: 0x04002445 RID: 9285
			Interpolate,
			// Token: 0x04002446 RID: 9286
			ToDestination
		}
	}
}
