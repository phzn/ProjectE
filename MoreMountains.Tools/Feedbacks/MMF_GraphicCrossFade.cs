using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001EA RID: 490
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you trigger cross fades on a target Graphic.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Graphic CrossFade")]
	public class MMF_GraphicCrossFade : MMF_Feedback
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00036FE7 File Offset: 0x000351E7
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x00036FF5 File Offset: 0x000351F5
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00036FFE File Offset: 0x000351FE
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00037001 File Offset: 0x00035201
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00037004 File Offset: 0x00035204
		protected override void AutomateTargetAcquisition()
		{
			this.TargetGraphic = this.FindAutomatedTarget<Graphic>();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00037012 File Offset: 0x00035212
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.TargetGraphic != null)
			{
				this._initialColor = this.TargetGraphic.color;
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003703C File Offset: 0x0003523C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_GraphicCrossFade.FeedbackTypeAuthorized || this.TargetGraphic == null)
			{
				return;
			}
			this.Turn(true);
			bool flag = !this.InScaledTimescaleMode;
			MMF_GraphicCrossFade.Modes mode = this.Mode;
			if (mode == MMF_GraphicCrossFade.Modes.Alpha)
			{
				this._initialColor.a = (float)(this.NormalPlayDirection ? 1 : 0);
				this.TargetGraphic.color = (this.NormalPlayDirection ? this._initialColor : this.TargetColor);
				this.TargetGraphic.CrossFadeAlpha(this.NormalPlayDirection ? 0f : 1f, 0f, true);
				this.TargetGraphic.CrossFadeAlpha(this.NormalPlayDirection ? this.TargetAlpha : this._initialColor.a, this.Duration, flag);
				return;
			}
			if (mode != MMF_GraphicCrossFade.Modes.Color)
			{
				return;
			}
			this.TargetGraphic.CrossFadeColor(this.NormalPlayDirection ? this.TargetColor : this._initialColor, this.Duration, flag, this.UseAlpha);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00037144 File Offset: 0x00035344
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_GraphicCrossFade.FeedbackTypeAuthorized)
			{
				return;
			}
			this.IsPlaying = false;
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && this.DisableOnStop)
			{
				this.Turn(false);
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003717C File Offset: 0x0003537C
		protected virtual void Turn(bool status)
		{
			this.TargetGraphic.gameObject.SetActive(status);
			this.TargetGraphic.enabled = status;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003719B File Offset: 0x0003539B
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_GraphicCrossFade.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetGraphic.color = this._initialColor;
		}

		// Token: 0x04000E60 RID: 3680
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E61 RID: 3681
		[MMFInspectorGroup("Graphic Cross Fade", true, 54, true, false)]
		[Tooltip("the Graphic to affect when playing the feedback")]
		public Graphic TargetGraphic;

		// Token: 0x04000E62 RID: 3682
		[Tooltip("whether the feedback should affect the Image instantly or over a period of time")]
		public MMF_GraphicCrossFade.Modes Mode;

		// Token: 0x04000E63 RID: 3683
		[Tooltip("how long the Graphic should change over time")]
		public float Duration = 0.2f;

		// Token: 0x04000E64 RID: 3684
		[Tooltip("the target alpha")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float TargetAlpha = 0.2f;

		// Token: 0x04000E65 RID: 3685
		[Tooltip("the target color")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color TargetColor = Color.red;

		// Token: 0x04000E66 RID: 3686
		[Tooltip("whether or not the crossfade should also tween the alpha channel")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool UseAlpha = true;

		// Token: 0x04000E67 RID: 3687
		[Tooltip("if this is true, the target will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04000E68 RID: 3688
		protected Coroutine _coroutine;

		// Token: 0x04000E69 RID: 3689
		protected Color _initialColor;

		// Token: 0x020004B8 RID: 1208
		public enum Modes
		{
			// Token: 0x04002200 RID: 8704
			Alpha,
			// Token: 0x04002201 RID: 8705
			Color
		}
	}
}
