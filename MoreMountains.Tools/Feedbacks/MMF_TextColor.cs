using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000212 RID: 530
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the color of a target Text over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Text Color")]
	public class MMF_TextColor : MMF_Feedback
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x000407B0 File Offset: 0x0003E9B0
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x000407CC File Offset: 0x0003E9CC
		public override float FeedbackDuration
		{
			get
			{
				if (this.ColorMode != MMF_TextColor.ColorModes.Instant)
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x000407D5 File Offset: 0x0003E9D5
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000407D8 File Offset: 0x0003E9D8
		protected override void AutomateTargetAcquisition()
		{
			this.TargetText = this.FindAutomatedTarget<Text>();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000407E6 File Offset: 0x0003E9E6
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.TargetText == null)
			{
				return;
			}
			this._initialColor = this.TargetText.color;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00040810 File Offset: 0x0003EA10
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TextColor.FeedbackTypeAuthorized || this.TargetText == null)
			{
				return;
			}
			switch (this.ColorMode)
			{
			case MMF_TextColor.ColorModes.Instant:
				this.TargetText.color = this.InstantColor;
				return;
			case MMF_TextColor.ColorModes.Gradient:
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
			case MMF_TextColor.ColorModes.Interpolate:
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

		// Token: 0x06000E81 RID: 3713 RVA: 0x000408EA File Offset: 0x0003EAEA
		protected virtual IEnumerator ChangeColor()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
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

		// Token: 0x06000E82 RID: 3714 RVA: 0x000408FC File Offset: 0x0003EAFC
		protected virtual void SetColor(float time)
		{
			if (this.ColorMode == MMF_TextColor.ColorModes.Gradient)
			{
				this.TargetText.color = this.ColorGradient.Evaluate(time);
				return;
			}
			if (this.ColorMode == MMF_TextColor.ColorModes.Interpolate)
			{
				float num = this.ColorCurve.Evaluate(time);
				this.TargetText.color = Color.LerpUnclamped(this._initialColor, this.DestinationColor, num);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0004095D File Offset: 0x0003EB5D
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0004098F File Offset: 0x0003EB8F
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TextColor.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetText.color = this._initialColor;
		}

		// Token: 0x040010C1 RID: 4289
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010C2 RID: 4290
		[MMFInspectorGroup("Target", true, 58, true, false)]
		[Tooltip(" Text component to control")]
		public Text TargetText;

		// Token: 0x040010C3 RID: 4291
		[MMFInspectorGroup("Color", true, 36, false, false)]
		[Tooltip("the selected color mode :None : nothing will happen,gradient : evaluates the color over time on that gradient, from left to right,interpolate : lerps from the current color to the destination one ")]
		public MMF_TextColor.ColorModes ColorMode = MMF_TextColor.ColorModes.Interpolate;

		// Token: 0x040010C4 RID: 4292
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("ColorMode", new int[] { 2, 1 })]
		public float Duration = 0.2f;

		// Token: 0x040010C5 RID: 4293
		[Tooltip("the color to apply")]
		[MMFEnumCondition("ColorMode", new int[] { 0 })]
		public Color InstantColor = Color.yellow;

		// Token: 0x040010C6 RID: 4294
		[Tooltip("the gradient to use to animate the color over time")]
		[MMFEnumCondition("ColorMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorGradient;

		// Token: 0x040010C7 RID: 4295
		[Tooltip("the destination color when in interpolate mode")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public Color DestinationColor = Color.yellow;

		// Token: 0x040010C8 RID: 4296
		[Tooltip("the curve to use when interpolating towards the destination color")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040010C9 RID: 4297
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040010CA RID: 4298
		protected Color _initialColor;

		// Token: 0x040010CB RID: 4299
		protected Coroutine _coroutine;

		// Token: 0x020004FC RID: 1276
		public enum ColorModes
		{
			// Token: 0x04002353 RID: 9043
			Instant,
			// Token: 0x04002354 RID: 9044
			Gradient,
			// Token: 0x04002355 RID: 9045
			Interpolate
		}
	}
}
