using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F2 RID: 498
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the color and intensity of a Light in your scene for a certain duration (or instantly).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Lights/Light")]
	public class MMF_Light : MMF_Feedback
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00038077 File Offset: 0x00036277
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x00038094 File Offset: 0x00036294
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Light.Modes.Instant)
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0003809D File Offset: 0x0003629D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x000380A0 File Offset: 0x000362A0
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x000380A3 File Offset: 0x000362A3
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000380A6 File Offset: 0x000362A6
		protected override void AutomateTargetAcquisition()
		{
			this.BoundLight = this.FindAutomatedTarget<Light>();
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000380B4 File Offset: 0x000362B4
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.BoundLight == null)
			{
				return;
			}
			this._initialRange = this.BoundLight.range;
			this._initialShadowStrength = this.BoundLight.shadowStrength;
			this._initialIntensity = this.BoundLight.intensity;
			this._initialColor = this.BoundLight.color;
			if (this.EventOriginTransform == null)
			{
				this.EventOriginTransform = owner.transform;
			}
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003814C File Offset: 0x0003634C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Light.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Mode == MMF_Light.Modes.ToDestination)
			{
				this._initialRange = this.BoundLight.range;
				this._initialShadowStrength = this.BoundLight.shadowStrength;
				this._initialIntensity = this.BoundLight.intensity;
				this._initialColor = this.BoundLight.color;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.Turn(true);
			switch (this.Mode)
			{
			case MMF_Light.Modes.OverTime:
			case MMF_Light.Modes.ToDestination:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.LightSequence(num));
				return;
			case MMF_Light.Modes.Instant:
				this.BoundLight.intensity = this.InstantIntensity * num;
				this.BoundLight.shadowStrength = this.InstantShadowStrength;
				this.BoundLight.range = this.InstantRange;
				if (this.ModifyColor)
				{
					this.BoundLight.color = this.InstantColor;
					return;
				}
				break;
			case MMF_Light.Modes.ShakerEvent:
				MMLightShakeEvent.Trigger(this.FeedbackDuration, this.RelativeValues, this.ModifyColor, this.ColorOverTime, this.IntensityCurve, this.RemapIntensityZero, this.RemapIntensityOne, this.RangeCurve, this.RemapRangeZero * num, this.RemapRangeOne * num, this.ShadowStrengthCurve, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne, feedbacksIntensity, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.OnlyBroadcastInRange, this.EventRange, this.EventOriginTransform.position);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000382FA File Offset: 0x000364FA
		protected virtual IEnumerator LightSequence(float intensityMultiplier)
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetLightValues(num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetLightValues(this.FinalNormalizedTime, intensityMultiplier);
			if (this.DisableOnStop)
			{
				this.Turn(false);
			}
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00038310 File Offset: 0x00036510
		protected virtual void SetLightValues(float time, float intensityMultiplier)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			MMF_Light.Modes mode = this.Mode;
			if (mode != MMF_Light.Modes.OverTime)
			{
				if (mode == MMF_Light.Modes.ToDestination)
				{
					num = Mathf.Lerp(this._initialIntensity, this.ToDestinationIntensity, this.IntensityCurve.Evaluate(time));
					num2 = Mathf.Lerp(this._initialRange, this.ToDestinationRange, this.RangeCurve.Evaluate(time));
					num3 = Mathf.Lerp(this._initialShadowStrength, this.ToDestinationShadowStrength, this.ShadowStrengthCurve.Evaluate(time));
					this._targetColor = Color.Lerp(this._initialColor, this.ToDestinationColor, time);
				}
			}
			else
			{
				num = MMFeedbacksHelpers.Remap(this.IntensityCurve.Evaluate(time), 0f, 1f, this.RemapIntensityZero, this.RemapIntensityOne);
				num2 = MMFeedbacksHelpers.Remap(this.RangeCurve.Evaluate(time), 0f, 1f, this.RemapRangeZero, this.RemapRangeOne);
				num3 = MMFeedbacksHelpers.Remap(this.ShadowStrengthCurve.Evaluate(time), 0f, 1f, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne);
				this._targetColor = this.ColorOverTime.Evaluate(time);
			}
			if (this.RelativeValues && this.Mode != MMF_Light.Modes.ToDestination)
			{
				num += this._initialIntensity;
				num3 += this._initialShadowStrength;
				num2 += this._initialRange;
			}
			if (this.ModifyIntensity)
			{
				this.BoundLight.intensity = num * intensityMultiplier;
			}
			if (this.ModifyRange)
			{
				this.BoundLight.range = num2;
			}
			if (this.ModifyShadowStrength)
			{
				this.BoundLight.shadowStrength = Mathf.Clamp01(num3);
			}
			if (this.ModifyColor)
			{
				this.BoundLight.color = this._targetColor;
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000384CC File Offset: 0x000366CC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!MMF_Light.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			if (this.Active && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			if (this.Active && this.DisableOnStop)
			{
				this.Turn(false);
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0003852F File Offset: 0x0003672F
		protected virtual void Turn(bool status)
		{
			this.BoundLight.enabled = status;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00038540 File Offset: 0x00036740
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Light.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundLight.range = this._initialRange;
			this.BoundLight.shadowStrength = this._initialShadowStrength;
			this.BoundLight.intensity = this._initialIntensity;
			this.BoundLight.color = this._initialColor;
			if (this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x04000EB4 RID: 3764
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000EB5 RID: 3765
		[MMFInspectorGroup("Light", true, 37, true, false)]
		[Tooltip("the light to affect when playing the feedback")]
		public Light BoundLight;

		// Token: 0x04000EB6 RID: 3766
		[Tooltip("whether the feedback should affect the light instantly or over a period of time")]
		public MMF_Light.Modes Mode;

		// Token: 0x04000EB7 RID: 3767
		[Tooltip("how long the light should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public float Duration = 0.2f;

		// Token: 0x04000EB8 RID: 3768
		[Tooltip("whether or not that light should be turned off on start")]
		public bool StartsOff = true;

		// Token: 0x04000EB9 RID: 3769
		[Tooltip("if this is true, the light will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04000EBA RID: 3770
		[Tooltip("whether or not the values should be relative or not")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 1 })]
		public bool RelativeValues = true;

		// Token: 0x04000EBB RID: 3771
		[Tooltip("whether or not to reset shaker values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000EBC RID: 3772
		[Tooltip("whether or not to reset the target's values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000EBD RID: 3773
		[Tooltip("whether or not to broadcast a range to only affect certain shakers")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool OnlyBroadcastInRange;

		// Token: 0x04000EBE RID: 3774
		[Tooltip("the range of the event, in units")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float EventRange = 100f;

		// Token: 0x04000EBF RID: 3775
		[Tooltip("the transform to use to broadcast the event as origin point")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Transform EventOriginTransform;

		// Token: 0x04000EC0 RID: 3776
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000EC1 RID: 3777
		[MMFInspectorGroup("Color", true, 38, true, false)]
		[Tooltip("whether or not to modify the color of the light")]
		public bool ModifyColor = true;

		// Token: 0x04000EC2 RID: 3778
		[Tooltip("the colors to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public Gradient ColorOverTime;

		// Token: 0x04000EC3 RID: 3779
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public Color InstantColor = Color.red;

		// Token: 0x04000EC4 RID: 3780
		[Tooltip("the color to move to in destination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public Color ToDestinationColor = Color.red;

		// Token: 0x04000EC5 RID: 3781
		[MMFInspectorGroup("Intensity", true, 39, true, false)]
		[Tooltip("whether or not to modify the intensity of the light")]
		public bool ModifyIntensity = true;

		// Token: 0x04000EC6 RID: 3782
		[Tooltip("the curve to tween the intensity on")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000EC7 RID: 3783
		[Tooltip("the value to remap the intensity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapIntensityZero;

		// Token: 0x04000EC8 RID: 3784
		[Tooltip("the value to remap the intensity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapIntensityOne = 1f;

		// Token: 0x04000EC9 RID: 3785
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantIntensity = 1f;

		// Token: 0x04000ECA RID: 3786
		[Tooltip("the value to move the intensity to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationIntensity = 1f;

		// Token: 0x04000ECB RID: 3787
		[MMFInspectorGroup("Range", true, 40, true, false)]
		[Tooltip("whether or not to modify the range of the light")]
		public bool ModifyRange = true;

		// Token: 0x04000ECC RID: 3788
		[Tooltip("the range to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve RangeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000ECD RID: 3789
		[Tooltip("the value to remap the range curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapRangeZero;

		// Token: 0x04000ECE RID: 3790
		[Tooltip("the value to remap the range curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapRangeOne = 10f;

		// Token: 0x04000ECF RID: 3791
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantRange = 10f;

		// Token: 0x04000ED0 RID: 3792
		[Tooltip("the value to move the intensity to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationRange = 10f;

		// Token: 0x04000ED1 RID: 3793
		[MMFInspectorGroup("Shadow Strength", true, 41, true, false)]
		[Tooltip("whether or not to modify the shadow strength of the light")]
		public bool ModifyShadowStrength = true;

		// Token: 0x04000ED2 RID: 3794
		[Tooltip("the range to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve ShadowStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000ED3 RID: 3795
		[Tooltip("the value to remap the shadow strength's curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapShadowStrengthZero;

		// Token: 0x04000ED4 RID: 3796
		[Tooltip("the value to remap the shadow strength's curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapShadowStrengthOne = 1f;

		// Token: 0x04000ED5 RID: 3797
		[Tooltip("the value to move the shadow strength to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantShadowStrength = 1f;

		// Token: 0x04000ED6 RID: 3798
		[Tooltip("the value to move the shadow strength to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationShadowStrength = 1f;

		// Token: 0x04000ED7 RID: 3799
		protected float _initialRange;

		// Token: 0x04000ED8 RID: 3800
		protected float _initialShadowStrength;

		// Token: 0x04000ED9 RID: 3801
		protected float _initialIntensity;

		// Token: 0x04000EDA RID: 3802
		protected Coroutine _coroutine;

		// Token: 0x04000EDB RID: 3803
		protected Color _initialColor;

		// Token: 0x04000EDC RID: 3804
		protected Color _targetColor;

		// Token: 0x020004C2 RID: 1218
		public enum Modes
		{
			// Token: 0x04002225 RID: 8741
			OverTime,
			// Token: 0x04002226 RID: 8742
			Instant,
			// Token: 0x04002227 RID: 8743
			ShakerEvent,
			// Token: 0x04002228 RID: 8744
			ToDestination
		}
	}
}
