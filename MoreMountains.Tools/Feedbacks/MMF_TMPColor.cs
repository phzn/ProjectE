using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BA RID: 698
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the color of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Color")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPColor : MMF_Feedback
	{
		// Token: 0x060013AC RID: 5036 RVA: 0x00055216 File Offset: 0x00053416
		public override bool EvaluateRequiresSetup()
		{
			return this.TargetTMPText == null;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x00055224 File Offset: 0x00053424
		public override string RequiredTargetText
		{
			get
			{
				if (!(this.TargetTMPText != null))
				{
					return "";
				}
				return this.TargetTMPText.name;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00055245 File Offset: 0x00053445
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x00055261 File Offset: 0x00053461
		public override float FeedbackDuration
		{
			get
			{
				if (this.ColorMode != MMF_TMPColor.ColorModes.Instant)
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

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0005526A File Offset: 0x0005346A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0005526D File Offset: 0x0005346D
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0005527B File Offset: 0x0005347B
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialColor = this.TargetTMPText.color;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000552A4 File Offset: 0x000534A4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPColor.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			switch (this.ColorMode)
			{
			case MMF_TMPColor.ColorModes.Instant:
				this.TargetTMPText.color = this.InstantColor;
				return;
			case MMF_TMPColor.ColorModes.Gradient:
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
			case MMF_TMPColor.ColorModes.Interpolate:
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

		// Token: 0x060013B4 RID: 5044 RVA: 0x0005537F File Offset: 0x0005357F
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

		// Token: 0x060013B5 RID: 5045 RVA: 0x00055390 File Offset: 0x00053590
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPColor.FeedbackTypeAuthorized)
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

		// Token: 0x060013B6 RID: 5046 RVA: 0x000553DC File Offset: 0x000535DC
		protected virtual void SetColor(float time)
		{
			if (this.ColorMode == MMF_TMPColor.ColorModes.Gradient)
			{
				this.TargetTMPText.color = this.ColorGradient.Evaluate(time);
				return;
			}
			if (this.ColorMode == MMF_TMPColor.ColorModes.Interpolate)
			{
				float num = this.ColorCurve.Evaluate(time);
				this.TargetTMPText.color = Color.LerpUnclamped(this._initialColor, this.DestinationColor, num);
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0005543D File Offset: 0x0005363D
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPColor.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.color = this._initialColor;
		}

		// Token: 0x040016B9 RID: 5817
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016BA RID: 5818
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip(" TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016BB RID: 5819
		[MMFInspectorGroup("Color", true, 16, false, false)]
		[Tooltip("the selected color mode :None : nothing will happen,gradient : evaluates the color over time on that gradient, from left to right,interpolate : lerps from the current color to the destination one ")]
		public MMF_TMPColor.ColorModes ColorMode = MMF_TMPColor.ColorModes.Interpolate;

		// Token: 0x040016BC RID: 5820
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("ColorMode", new int[] { 2, 1 })]
		public float Duration = 0.2f;

		// Token: 0x040016BD RID: 5821
		[Tooltip("the color to apply")]
		[MMFEnumCondition("ColorMode", new int[] { 0 })]
		public Color InstantColor = Color.yellow;

		// Token: 0x040016BE RID: 5822
		[Tooltip("the gradient to use to animate the color over time")]
		[MMFEnumCondition("ColorMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorGradient;

		// Token: 0x040016BF RID: 5823
		[Tooltip("the destination color when in interpolate mode")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public Color DestinationColor = Color.yellow;

		// Token: 0x040016C0 RID: 5824
		[Tooltip("the curve to use when interpolating towards the destination color")]
		[MMFEnumCondition("ColorMode", new int[] { 2 })]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040016C1 RID: 5825
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040016C2 RID: 5826
		protected Color _initialColor;

		// Token: 0x040016C3 RID: 5827
		protected Coroutine _coroutine;

		// Token: 0x0200054A RID: 1354
		public enum ColorModes
		{
			// Token: 0x0400244C RID: 9292
			Instant,
			// Token: 0x0400244D RID: 9293
			Gradient,
			// Token: 0x0400244E RID: 9294
			Interpolate
		}
	}
}
