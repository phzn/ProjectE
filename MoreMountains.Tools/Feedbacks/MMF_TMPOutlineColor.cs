using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C0 RID: 704
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the color of a target TMP's outline over time.")]
	[FeedbackPath("TextMesh Pro/TMP Outline Color")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPOutlineColor : MMF_Feedback
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00055DD6 File Offset: 0x00053FD6
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x00055DF2 File Offset: 0x00053FF2
		public override float FeedbackDuration
		{
			get
			{
				if (this.ColorMode != MMF_TMPOutlineColor.ColorModes.Instant)
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00055DFB File Offset: 0x00053FFB
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00055DFE File Offset: 0x00053FFE
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00055E0C File Offset: 0x0005400C
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialColor = this.TargetTMPText.outlineColor;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00055E3C File Offset: 0x0005403C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPOutlineColor.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			switch (this.ColorMode)
			{
			case MMF_TMPOutlineColor.ColorModes.Instant:
				this.TargetTMPText.outlineColor = this.InstantColor;
				return;
			case MMF_TMPOutlineColor.ColorModes.Gradient:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeColor());
				return;
			case MMF_TMPOutlineColor.ColorModes.Interpolate:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeColor());
				return;
			default:
				return;
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00055F17 File Offset: 0x00054117
		protected virtual IEnumerator ChangeColor()
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetColor(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetColor(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00055F28 File Offset: 0x00054128
		protected virtual void SetColor(float time)
		{
			if (this.ColorMode == MMF_TMPOutlineColor.ColorModes.Gradient)
			{
				this.TargetTMPText.gameObject.SetActive(false);
				this.TargetTMPText.outlineColor = this.ColorGradient.Evaluate(time);
				this.TargetTMPText.gameObject.SetActive(true);
				return;
			}
			if (this.ColorMode == MMF_TMPOutlineColor.ColorModes.Interpolate)
			{
				float num = this.ColorCurve.Evaluate(time);
				this.TargetTMPText.gameObject.SetActive(false);
				this.TargetTMPText.outlineColor = Color.LerpUnclamped(this._initialColor, this.DestinationColor, num);
				this.TargetTMPText.gameObject.SetActive(true);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00055FDC File Offset: 0x000541DC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPOutlineColor.FeedbackTypeAuthorized)
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

		// Token: 0x060013F2 RID: 5106 RVA: 0x00056028 File Offset: 0x00054228
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPOutlineColor.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.gameObject.SetActive(false);
			this.TargetTMPText.outlineColor = this._initialColor;
			this.TargetTMPText.gameObject.SetActive(true);
		}

		// Token: 0x040016F5 RID: 5877
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016F6 RID: 5878
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016F7 RID: 5879
		[MMFInspectorGroup("Outline Color", true, 16, false, false)]
		[Tooltip("the selected color mode :None : nothing will happen,gradient : evaluates the color over time on that gradient, from left to right,interpolate : lerps from the current color to the destination one ")]
		public MMF_TMPOutlineColor.ColorModes ColorMode = MMF_TMPOutlineColor.ColorModes.Interpolate;

		// Token: 0x040016F8 RID: 5880
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("ColorMode", new int[] { 2, 1 })]
		public float Duration = 0.2f;

		// Token: 0x040016F9 RID: 5881
		[Tooltip("the color to apply")]
		[MMFEnumCondition("ColorMode", new int[] { 0 })]
		public Color32 InstantColor = Color.yellow;

		// Token: 0x040016FA RID: 5882
		[Tooltip("the gradient to use to animate the color over time")]
		[MMFEnumCondition("ColorMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorGradient;

		// Token: 0x040016FB RID: 5883
		[Tooltip("the destination color when in interpolate mode")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public Color32 DestinationColor = Color.yellow;

		// Token: 0x040016FC RID: 5884
		[Tooltip("the curve to use when interpolating towards the destination color")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040016FD RID: 5885
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040016FE RID: 5886
		protected Color _initialColor;

		// Token: 0x040016FF RID: 5887
		protected Coroutine _coroutine;

		// Token: 0x0200054F RID: 1359
		public enum ColorModes
		{
			// Token: 0x0400245E RID: 9310
			Instant,
			// Token: 0x0400245F RID: 9311
			Gradient,
			// Token: 0x04002460 RID: 9312
			Interpolate
		}
	}
}
