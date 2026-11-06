using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E9 RID: 489
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the color of a target Graphic over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Graphic")]
	public class MMF_Graphic : MMF_Feedback
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00036DF6 File Offset: 0x00034FF6
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x00036E13 File Offset: 0x00035013
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Graphic.Modes.Instant)
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00036E1C File Offset: 0x0003501C
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00036E1F File Offset: 0x0003501F
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00036E22 File Offset: 0x00035022
		protected override void AutomateTargetAcquisition()
		{
			this.TargetGraphic = this.FindAutomatedTarget<Graphic>();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00036E30 File Offset: 0x00035030
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00036E50 File Offset: 0x00035050
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Graphic.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialColor = this.TargetGraphic.color;
			this.Turn(true);
			MMF_Graphic.Modes mode = this.Mode;
			if (mode != MMF_Graphic.Modes.OverTime)
			{
				if (mode == MMF_Graphic.Modes.Instant && this.ModifyColor)
				{
					this.TargetGraphic.color = this.InstantColor;
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
				this._coroutine = this.Owner.StartCoroutine(this.GraphicSequence());
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00036EEE File Offset: 0x000350EE
		protected virtual IEnumerator GraphicSequence()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetGraphicValues(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetGraphicValues(this.FinalNormalizedTime);
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this.IsPlaying = false;
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00036EFD File Offset: 0x000350FD
		protected virtual void SetGraphicValues(float time)
		{
			if (this.ModifyColor)
			{
				this.TargetGraphic.color = this.ColorOverTime.Evaluate(time);
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00036F20 File Offset: 0x00035120
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Graphic.FeedbackTypeAuthorized)
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

		// Token: 0x06000C9D RID: 3229 RVA: 0x00036F83 File Offset: 0x00035183
		protected virtual void Turn(bool status)
		{
			this.TargetGraphic.gameObject.SetActive(status);
			this.TargetGraphic.enabled = status;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00036FA2 File Offset: 0x000351A2
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Graphic.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetGraphic.color = this._initialColor;
		}

		// Token: 0x04000E54 RID: 3668
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E55 RID: 3669
		[MMFInspectorGroup("Graphic", true, 54, true, false)]
		[Tooltip("the Graphic to affect when playing the feedback")]
		public Graphic TargetGraphic;

		// Token: 0x04000E56 RID: 3670
		[Tooltip("whether the feedback should affect the Graphic instantly or over a period of time")]
		public MMF_Graphic.Modes Mode;

		// Token: 0x04000E57 RID: 3671
		[Tooltip("how long the Graphic should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x04000E58 RID: 3672
		[Tooltip("whether or not that Graphic should be turned off on start")]
		public bool StartsOff;

		// Token: 0x04000E59 RID: 3673
		[Tooltip("if this is true, the target will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04000E5A RID: 3674
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000E5B RID: 3675
		[Tooltip("whether or not to modify the color of the Graphic")]
		public bool ModifyColor = true;

		// Token: 0x04000E5C RID: 3676
		[Tooltip("the colors to apply to the Graphic over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Gradient ColorOverTime;

		// Token: 0x04000E5D RID: 3677
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color InstantColor;

		// Token: 0x04000E5E RID: 3678
		protected Coroutine _coroutine;

		// Token: 0x04000E5F RID: 3679
		protected Color _initialColor;

		// Token: 0x020004B6 RID: 1206
		public enum Modes
		{
			// Token: 0x040021F9 RID: 8697
			OverTime,
			// Token: 0x040021FA RID: 8698
			Instant
		}
	}
}
