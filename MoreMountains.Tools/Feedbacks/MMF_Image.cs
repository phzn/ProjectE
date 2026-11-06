using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001EC RID: 492
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the color of a target Image over time. You can also use it to command one or many MMImageShakers.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Image")]
	public class MMF_Image : MMF_Feedback
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0003723A File Offset: 0x0003543A
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x00037257 File Offset: 0x00035457
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Image.Modes.Instant)
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00037260 File Offset: 0x00035460
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00037263 File Offset: 0x00035463
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00037266 File Offset: 0x00035466
		protected override void AutomateTargetAcquisition()
		{
			this.BoundImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00037274 File Offset: 0x00035474
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.DisableOnInit)
			{
				this.Turn(false);
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00037294 File Offset: 0x00035494
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Image.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialColor = this.BoundImage.color;
			if (this.EnableOnPlay)
			{
				this.Turn(true);
			}
			MMF_Image.Modes mode = this.Mode;
			if (mode != MMF_Image.Modes.OverTime)
			{
				if (mode == MMF_Image.Modes.Instant && this.ModifyColor)
				{
					this.BoundImage.color = this.InstantColor;
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

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003733A File Offset: 0x0003553A
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
			if (this.DisableOnSequenceEnd)
			{
				this.Turn(false);
			}
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00037349 File Offset: 0x00035549
		protected virtual void SetImageValues(float time)
		{
			if (this.ModifyColor)
			{
				this.BoundImage.color = this.ColorOverTime.Evaluate(time);
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003736C File Offset: 0x0003556C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Image.FeedbackTypeAuthorized)
			{
				return;
			}
			this.IsPlaying = false;
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && this.DisableOnStop)
			{
				this.Turn(false);
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this._coroutine = null;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000373CF File Offset: 0x000355CF
		protected virtual void Turn(bool status)
		{
			this.BoundImage.gameObject.SetActive(status);
			this.BoundImage.enabled = status;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000373EE File Offset: 0x000355EE
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Image.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundImage.color = this._initialColor;
		}

		// Token: 0x04000E6A RID: 3690
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E6B RID: 3691
		[MMFInspectorGroup("Image", true, 54, true, false)]
		[Tooltip("the Image to affect when playing the feedback")]
		public Image BoundImage;

		// Token: 0x04000E6C RID: 3692
		[Tooltip("whether the feedback should affect the Image instantly or over a period of time")]
		public MMF_Image.Modes Mode;

		// Token: 0x04000E6D RID: 3693
		[Tooltip("how long the Image should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x04000E6E RID: 3694
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000E6F RID: 3695
		[Tooltip("whether or not to modify the color of the image")]
		public bool ModifyColor = true;

		// Token: 0x04000E70 RID: 3696
		[Tooltip("the colors to apply to the Image over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Gradient ColorOverTime;

		// Token: 0x04000E71 RID: 3697
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color InstantColor;

		// Token: 0x04000E72 RID: 3698
		[Tooltip("whether or not that Image should be turned off on start")]
		[FormerlySerializedAs("StartsOff")]
		public bool DisableOnInit;

		// Token: 0x04000E73 RID: 3699
		[Tooltip("if this is true, the target will be enabled when this feedback gets played")]
		public bool EnableOnPlay = true;

		// Token: 0x04000E74 RID: 3700
		[Tooltip("if this is true, the target disabled after the color over time change ends")]
		public bool DisableOnSequenceEnd;

		// Token: 0x04000E75 RID: 3701
		[Tooltip("if this is true, the target will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04000E76 RID: 3702
		protected Coroutine _coroutine;

		// Token: 0x04000E77 RID: 3703
		protected Color _initialColor;

		// Token: 0x020004B9 RID: 1209
		public enum Modes
		{
			// Token: 0x04002203 RID: 8707
			OverTime,
			// Token: 0x04002204 RID: 8708
			Instant
		}
	}
}
