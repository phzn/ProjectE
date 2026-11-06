using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000333 RID: 819
	[AddComponentMenu("")]
	public class MMF_UIToolkitColorBase : MMF_UIToolkit
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x0005F556 File Offset: 0x0005D756
		// (set) Token: 0x0600168B RID: 5771 RVA: 0x0005F573 File Offset: 0x0005D773
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_UIToolkitColorBase.Modes.Instant)
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

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x0005F57C File Offset: 0x0005D77C
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0005F57F File Offset: 0x0005D77F
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.HandleApplyInitialColors();
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0005F590 File Offset: 0x0005D790
		protected virtual void HandleApplyInitialColors()
		{
			GradientColorKey[] colorKeys = this.ColorOverTime.colorKeys;
			GradientAlphaKey[] alphaKeys = this.ColorOverTime.alphaKeys;
			if (this.ApplyInitialColorToGradientStart)
			{
				colorKeys[0] = new GradientColorKey(this.GetInitialColor(), 0f);
				alphaKeys[0] = new GradientAlphaKey(this.GetInitialColor().a, 0f);
			}
			if (this.ApplyInitialColorToGradientEnd)
			{
				int num = this.ColorOverTime.colorKeys.Length - 1;
				colorKeys[num] = new GradientColorKey(this.GetInitialColor(), 1f);
				alphaKeys[num] = new GradientAlphaKey(this.GetInitialColor().a, 1f);
			}
			if (this.ApplyInitialColorToGradientEnd || this.ApplyInitialColorToGradientStart)
			{
				this.ColorOverTime.SetKeys(colorKeys, alphaKeys);
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0005F658 File Offset: 0x0005D858
		protected virtual void ApplyColor(Color newColor)
		{
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0005F65A File Offset: 0x0005D85A
		protected virtual Color GetInitialColor()
		{
			return Color.white;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0005F664 File Offset: 0x0005D864
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialColor = this.GetInitialColor();
			if (this.ApplyInitialColorsOnPlay)
			{
				this.HandleApplyInitialColors();
			}
			MMF_UIToolkitColorBase.Modes mode = this.Mode;
			if (mode != MMF_UIToolkitColorBase.Modes.OverTime)
			{
				if (mode == MMF_UIToolkitColorBase.Modes.Instant && this.ModifyColor)
				{
					this.ApplyColor(this.InstantColor);
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
				this._coroutine = this.Owner.StartCoroutine(this.ImageSequence());
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0005F6FF File Offset: 0x0005D8FF
		protected virtual IEnumerator ImageSequence()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetImageValues(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetImageValues(this.FinalNormalizedTime);
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0005F70E File Offset: 0x0005D90E
		protected virtual void SetImageValues(float time)
		{
			if (this.ModifyColor)
			{
				this.ApplyColor(this.ColorOverTime.Evaluate(time));
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0005F72A File Offset: 0x0005D92A
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.IsPlaying = false;
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._coroutine = null;
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0005F752 File Offset: 0x0005D952
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.ApplyColor(this._initialColor);
		}

		// Token: 0x040019F6 RID: 6646
		[MMFInspectorGroup("Color", true, 55, true, false)]
		[Tooltip("whether the feedback should affect the Image instantly or over a period of time")]
		public MMF_UIToolkitColorBase.Modes Mode;

		// Token: 0x040019F7 RID: 6647
		[Tooltip("how long the Image should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x040019F8 RID: 6648
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040019F9 RID: 6649
		[Tooltip("whether or not to modify the color of the image")]
		public bool ModifyColor = true;

		// Token: 0x040019FA RID: 6650
		[Tooltip("the colors to apply to the Image over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Gradient ColorOverTime = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.white, 0f),
				new GradientColorKey(Color.red, 0.5f),
				new GradientColorKey(Color.white, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(1f, 0.5f),
				new GradientAlphaKey(1f, 1f)
			}
		};

		// Token: 0x040019FB RID: 6651
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color InstantColor;

		// Token: 0x040019FC RID: 6652
		[Tooltip("if this is true, the initial color will be applied to the gradient start")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public bool ApplyInitialColorToGradientStart;

		// Token: 0x040019FD RID: 6653
		[Tooltip("if this is true, the initial color will be applied to the gradient end")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public bool ApplyInitialColorToGradientEnd;

		// Token: 0x040019FE RID: 6654
		[FormerlySerializedAs("GrabInitialColorsOnPlay")]
		[Tooltip("if this is true, the initial color will be applied to the gradient start and end on play")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public bool ApplyInitialColorsOnPlay = true;

		// Token: 0x040019FF RID: 6655
		protected Coroutine _coroutine;

		// Token: 0x04001A00 RID: 6656
		protected Color _initialColor;

		// Token: 0x0200057A RID: 1402
		public enum Modes
		{
			// Token: 0x040024B4 RID: 9396
			OverTime,
			// Token: 0x040024B5 RID: 9397
			Instant
		}
	}
}
