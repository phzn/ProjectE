using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029C RID: 668
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you animate the density, color, end and start distance of your scene's fog")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Renderer/Fog")]
	public class MMF_Fog : MMF_Feedback
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00050D15 File Offset: 0x0004EF15
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00050D18 File Offset: 0x0004EF18
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00050D1B File Offset: 0x0004EF1B
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x00050D38 File Offset: 0x0004EF38
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Fog.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				if (this.Mode != MMF_Fog.Modes.Instant)
				{
					this.Duration = value;
				}
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00050D4C File Offset: 0x0004EF4C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Fog.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialColor = RenderSettings.fogColor;
			this._initialStartDistance = RenderSettings.fogStartDistance;
			this._initialEndDistance = RenderSettings.fogEndDistance;
			this._initialDensity = RenderSettings.fogDensity;
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMF_Fog.Modes mode = this.Mode;
			if (mode != MMF_Fog.Modes.OverTime)
			{
				if (mode == MMF_Fog.Modes.Instant)
				{
					if (this.ModifyColor)
					{
						RenderSettings.fogColor = this.InstantColor;
					}
					if (this.ModifyStartDistance)
					{
						RenderSettings.fogStartDistance = this.StartDistanceInstantChange;
					}
					if (this.ModifyEndDistance)
					{
						RenderSettings.fogEndDistance = this.EndDistanceInstantChange;
					}
					if (this.ModifyFogDensity)
					{
						RenderSettings.fogDensity = this.DensityInstantChange * num;
						return;
					}
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
				this._coroutine = this.Owner.StartCoroutine(this.FogSequence(num));
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00050E40 File Offset: 0x0004F040
		protected virtual IEnumerator FogSequence(float intensityMultiplier)
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetFogValues(num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetFogValues(this.FinalNormalizedTime, intensityMultiplier);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00050E58 File Offset: 0x0004F058
		protected virtual void SetFogValues(float time, float intensityMultiplier)
		{
			if (this.ModifyColor)
			{
				RenderSettings.fogColor = this.ColorOverTime.Evaluate(time);
			}
			if (this.ModifyFogDensity)
			{
				RenderSettings.fogDensity = MMTween.Tween(time, 0f, 1f, this.DensityRemapZero, this.DensityRemapOne, this.DensityCurve) * intensityMultiplier;
			}
			if (this.ModifyStartDistance)
			{
				RenderSettings.fogStartDistance = MMTween.Tween(time, 0f, 1f, this.StartDistanceRemapZero, this.StartDistanceRemapOne, this.StartDistanceCurve);
			}
			if (this.ModifyEndDistance)
			{
				RenderSettings.fogEndDistance = MMTween.Tween(time, 0f, 1f, this.EndDistanceRemapZero, this.EndDistanceRemapOne, this.EndDistanceCurve);
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00050F10 File Offset: 0x0004F110
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Fog.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00050F5C File Offset: 0x0004F15C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Fog.FeedbackTypeAuthorized)
			{
				return;
			}
			RenderSettings.fogColor = this._initialColor;
			RenderSettings.fogStartDistance = this._initialStartDistance;
			RenderSettings.fogEndDistance = this._initialEndDistance;
			RenderSettings.fogDensity = this._initialDensity;
		}

		// Token: 0x0400152D RID: 5421
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400152E RID: 5422
		[MMFInspectorGroup("Fog", true, 24, false, false)]
		[Tooltip("whether the feedback should affect the sprite renderer instantly or over a period of time")]
		public MMF_Fog.Modes Mode;

		// Token: 0x0400152F RID: 5423
		[Tooltip("how long the sprite renderer should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 2f;

		// Token: 0x04001530 RID: 5424
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001531 RID: 5425
		[MMFInspectorGroup("Fog Density", true, 25, false, false)]
		[Tooltip("whether or not to modify the fog's density")]
		public bool ModifyFogDensity = true;

		// Token: 0x04001532 RID: 5426
		[Tooltip("a curve to use to animate the fog's density over time")]
		public MMTweenType DensityCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001533 RID: 5427
		[Tooltip("the value to remap the fog's density curve zero value to")]
		public float DensityRemapZero = 0.01f;

		// Token: 0x04001534 RID: 5428
		[Tooltip("the value to remap the fog's density curve one value to")]
		public float DensityRemapOne = 0.05f;

		// Token: 0x04001535 RID: 5429
		[Tooltip("the value to change the fog's density to when in instant mode")]
		public float DensityInstantChange;

		// Token: 0x04001536 RID: 5430
		[MMFInspectorGroup("Fog Start Distance", true, 26, false, false)]
		[Tooltip("whether or not to modify the fog's start distance")]
		public bool ModifyStartDistance = true;

		// Token: 0x04001537 RID: 5431
		[Tooltip("a curve to use to animate the fog's start distance over time")]
		public MMTweenType StartDistanceCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001538 RID: 5432
		[Tooltip("the value to remap the fog's start distance curve zero value to")]
		public float StartDistanceRemapZero;

		// Token: 0x04001539 RID: 5433
		[Tooltip("the value to remap the fog's start distance curve one value to")]
		public float StartDistanceRemapOne;

		// Token: 0x0400153A RID: 5434
		[Tooltip("the value to change the fog's start distance to when in instant mode")]
		public float StartDistanceInstantChange;

		// Token: 0x0400153B RID: 5435
		[MMFInspectorGroup("Fog End Distance", true, 27, false, false)]
		[Tooltip("whether or not to modify the fog's end distance")]
		public bool ModifyEndDistance = true;

		// Token: 0x0400153C RID: 5436
		[Tooltip("a curve to use to animate the fog's end distance over time")]
		public MMTweenType EndDistanceCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x0400153D RID: 5437
		[Tooltip("the value to remap the fog's end distance curve zero value to")]
		public float EndDistanceRemapZero;

		// Token: 0x0400153E RID: 5438
		[Tooltip("the value to remap the fog's end distance curve one value to")]
		public float EndDistanceRemapOne = 300f;

		// Token: 0x0400153F RID: 5439
		[Tooltip("the value to change the fog's end distance to when in instant mode")]
		public float EndDistanceInstantChange;

		// Token: 0x04001540 RID: 5440
		[MMFInspectorGroup("Fog Color", true, 28, false, false)]
		[Tooltip("whether or not to modify the fog's color")]
		public bool ModifyColor = true;

		// Token: 0x04001541 RID: 5441
		[Tooltip("the colors to apply to the sprite renderer over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Gradient ColorOverTime;

		// Token: 0x04001542 RID: 5442
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color InstantColor;

		// Token: 0x04001543 RID: 5443
		protected Coroutine _coroutine;

		// Token: 0x04001544 RID: 5444
		protected Color _initialColor;

		// Token: 0x04001545 RID: 5445
		protected float _initialStartDistance;

		// Token: 0x04001546 RID: 5446
		protected float _initialEndDistance;

		// Token: 0x04001547 RID: 5447
		protected float _initialDensity;

		// Token: 0x02000532 RID: 1330
		public enum Modes
		{
			// Token: 0x040023DB RID: 9179
			OverTime,
			// Token: 0x040023DC RID: 9180
			Instant
		}
	}
}
