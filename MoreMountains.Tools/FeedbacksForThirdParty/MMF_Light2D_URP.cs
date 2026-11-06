using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034F RID: 847
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control a 2D light's intensity, color, falloff, shadow strength and volumetric intensity over time, or instantly.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Lights/Light2D_URP")]
	public class MMF_Light2D_URP : MMF_Feedback
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x000621AE File Offset: 0x000603AE
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x000621CB File Offset: 0x000603CB
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Light2D_URP.Modes.Instant)
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

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x000621D4 File Offset: 0x000603D4
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x000621D7 File Offset: 0x000603D7
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x000621DA File Offset: 0x000603DA
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000621DD File Offset: 0x000603DD
		protected override void AutomateTargetAcquisition()
		{
			this.BoundLight = this.FindAutomatedTarget<Light2D>();
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x000621EC File Offset: 0x000603EC
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.BoundLight == null)
			{
				return;
			}
			this._initialFalloff = this.BoundLight.shapeLightFalloffSize;
			this._initialShadowStrength = this.BoundLight.shadowIntensity;
			this._initialIntensity = this.BoundLight.intensity;
			this._initialVolumetricIntensity = this.BoundLight.volumeIntensity;
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

		// Token: 0x0600173A RID: 5946 RVA: 0x00062298 File Offset: 0x00060498
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Light2D_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Mode == MMF_Light2D_URP.Modes.ToDestination)
			{
				this._initialFalloff = this.BoundLight.shapeLightFalloffSize;
				this._initialShadowStrength = this.BoundLight.shadowIntensity;
				this._initialIntensity = this.BoundLight.intensity;
				this._initialIntensity = this.BoundLight.volumeIntensity;
				this._initialColor = this.BoundLight.color;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.Turn(true);
			switch (this.Mode)
			{
			case MMF_Light2D_URP.Modes.OverTime:
			case MMF_Light2D_URP.Modes.ToDestination:
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
			case MMF_Light2D_URP.Modes.Instant:
				this.BoundLight.intensity = this.InstantIntensity * num;
				this.BoundLight.intensity = this.InstantVolumetricIntensity * num;
				this.BoundLight.shadowIntensity = this.InstantShadowStrength;
				this.BoundLight.shapeLightFalloffSize = this.InstantFalloff;
				if (this.ModifyColor)
				{
					this.BoundLight.color = this.InstantColor;
					return;
				}
				break;
			case MMF_Light2D_URP.Modes.ShakerEvent:
				if (this.EventOriginTransform == null)
				{
					this.EventOriginTransform = this.Owner.transform;
				}
				MMLight2DShakeEvent.Trigger(this.FeedbackDuration, this.RelativeValues, this.ModifyColor, this.ColorOverTime, this.IntensityCurve, this.RemapIntensityZero, this.RemapIntensityOne, this.FalloffCurve, this.RemapFalloffZero * num, this.RemapFalloffOne * num, this.ShadowStrengthCurve, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne, feedbacksIntensity, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.OnlyBroadcastInRange, this.EventRange, this.EventOriginTransform.position);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00062489 File Offset: 0x00060689
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

		// Token: 0x0600173C RID: 5948 RVA: 0x000624A0 File Offset: 0x000606A0
		protected virtual void SetLightValues(float time, float intensityMultiplier)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			MMF_Light2D_URP.Modes mode = this.Mode;
			if (mode != MMF_Light2D_URP.Modes.OverTime)
			{
				if (mode == MMF_Light2D_URP.Modes.ToDestination)
				{
					num = Mathf.Lerp(this._initialIntensity, this.ToDestinationIntensity, this.IntensityCurve.Evaluate(time));
					num2 = Mathf.Lerp(this._initialVolumetricIntensity, this.ToDestinationVolumetricIntensity, this.VolumetricIntensityCurve.Evaluate(time));
					num3 = Mathf.Lerp(this._initialFalloff, this.ToDestinationFalloff, this.FalloffCurve.Evaluate(time));
					num4 = Mathf.Lerp(this._initialShadowStrength, this.ToDestinationShadowStrength, this.ShadowStrengthCurve.Evaluate(time));
					this._targetColor = Color.Lerp(this._initialColor, this.ToDestinationColor, time);
				}
			}
			else
			{
				num = MMFeedbacksHelpers.Remap(this.IntensityCurve.Evaluate(time), 0f, 1f, this.RemapIntensityZero, this.RemapIntensityOne);
				num2 = MMFeedbacksHelpers.Remap(this.VolumetricIntensityCurve.Evaluate(time), 0f, 1f, this.RemapVolumetricIntensityZero, this.RemapVolumetricIntensityOne);
				num3 = MMFeedbacksHelpers.Remap(this.FalloffCurve.Evaluate(time), 0f, 1f, this.RemapFalloffZero, this.RemapFalloffOne);
				num4 = MMFeedbacksHelpers.Remap(this.ShadowStrengthCurve.Evaluate(time), 0f, 1f, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne);
				this._targetColor = this.ColorOverTime.Evaluate(time);
			}
			if (this.RelativeValues && this.Mode != MMF_Light2D_URP.Modes.ToDestination)
			{
				num += this._initialIntensity;
				num2 += this._initialVolumetricIntensity;
				num4 += this._initialShadowStrength;
				num3 += this._initialFalloff;
			}
			if (this.ModifyIntensity)
			{
				this.BoundLight.intensity = num * intensityMultiplier;
			}
			if (this.ModifyVolumetricIntensity)
			{
				this.BoundLight.volumeIntensity = num2 * intensityMultiplier;
			}
			if (this.ModifyFalloff)
			{
				this.BoundLight.shapeLightFalloffSize = num3;
			}
			if (this.ModifyShadowStrength)
			{
				this.BoundLight.shadowIntensity = Mathf.Clamp01(num4);
			}
			if (this.ModifyColor)
			{
				this.BoundLight.color = this._targetColor;
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000626CC File Offset: 0x000608CC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!MMF_Light2D_URP.FeedbackTypeAuthorized)
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

		// Token: 0x0600173E RID: 5950 RVA: 0x0006272F File Offset: 0x0006092F
		protected virtual void Turn(bool status)
		{
			if (this.BoundLight == null)
			{
				return;
			}
			this.BoundLight.enabled = status;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0006274C File Offset: 0x0006094C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Light2D_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundLight.shapeLightFalloffSize = this._initialFalloff;
			this.BoundLight.shadowIntensity = this._initialShadowStrength;
			this.BoundLight.intensity = this._initialIntensity;
			this.BoundLight.volumeIntensity = this._initialVolumetricIntensity;
			this.BoundLight.color = this._initialColor;
			if (this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x04001A94 RID: 6804
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A95 RID: 6805
		[MMFInspectorGroup("Light", true, 37, true, false)]
		[Tooltip("the light to affect when playing the feedback")]
		public Light2D BoundLight;

		// Token: 0x04001A96 RID: 6806
		[Tooltip("whether the feedback should affect the light instantly or over a period of time")]
		public MMF_Light2D_URP.Modes Mode;

		// Token: 0x04001A97 RID: 6807
		[Tooltip("how long the light should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public float Duration = 0.2f;

		// Token: 0x04001A98 RID: 6808
		[Tooltip("whether or not that light should be turned off on start")]
		public bool StartsOff = true;

		// Token: 0x04001A99 RID: 6809
		[Tooltip("if this is true, the light will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04001A9A RID: 6810
		[Tooltip("whether or not the values should be relative or not")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 1 })]
		public bool RelativeValues = true;

		// Token: 0x04001A9B RID: 6811
		[Tooltip("whether or not to reset shaker values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A9C RID: 6812
		[Tooltip("whether or not to reset the target's values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A9D RID: 6813
		[Tooltip("whether or not to broadcast a range to only affect certain shakers")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool OnlyBroadcastInRange;

		// Token: 0x04001A9E RID: 6814
		[Tooltip("the range of the event, in units")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float EventRange = 100f;

		// Token: 0x04001A9F RID: 6815
		[Tooltip("the transform to use to broadcast the event as origin point")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Transform EventOriginTransform;

		// Token: 0x04001AA0 RID: 6816
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001AA1 RID: 6817
		[MMFInspectorGroup("Color", true, 38, true, false)]
		[Tooltip("whether or not to modify the color of the light")]
		public bool ModifyColor = true;

		// Token: 0x04001AA2 RID: 6818
		[Tooltip("the colors to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public Gradient ColorOverTime;

		// Token: 0x04001AA3 RID: 6819
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public Color InstantColor = Color.red;

		// Token: 0x04001AA4 RID: 6820
		[Tooltip("the color to move to in destination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public Color ToDestinationColor = Color.red;

		// Token: 0x04001AA5 RID: 6821
		[MMFInspectorGroup("Intensity", true, 39, true, false)]
		[Tooltip("whether or not to modify the intensity of the light")]
		public bool ModifyIntensity = true;

		// Token: 0x04001AA6 RID: 6822
		[Tooltip("the curve to tween the intensity on")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AA7 RID: 6823
		[Tooltip("the value to remap the intensity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapIntensityZero;

		// Token: 0x04001AA8 RID: 6824
		[Tooltip("the value to remap the intensity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001AA9 RID: 6825
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantIntensity = 1f;

		// Token: 0x04001AAA RID: 6826
		[Tooltip("the value to move the intensity to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationIntensity = 1f;

		// Token: 0x04001AAB RID: 6827
		[MMFInspectorGroup("Falloff", true, 40, true, false)]
		[Tooltip("whether or not to modify the falloff of the light")]
		public bool ModifyFalloff = true;

		// Token: 0x04001AAC RID: 6828
		[Tooltip("the falloff to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve FalloffCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AAD RID: 6829
		[Tooltip("the value to remap the falloff curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapFalloffZero;

		// Token: 0x04001AAE RID: 6830
		[Tooltip("the value to remap the falloff curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapFalloffOne = 10f;

		// Token: 0x04001AAF RID: 6831
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFalloff = 10f;

		// Token: 0x04001AB0 RID: 6832
		[Tooltip("the value to move the intensity to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationFalloff = 10f;

		// Token: 0x04001AB1 RID: 6833
		[MMFInspectorGroup("Shadow Strength", true, 41, true, false)]
		[Tooltip("whether or not to modify the shadow strength of the light")]
		public bool ModifyShadowStrength = true;

		// Token: 0x04001AB2 RID: 6834
		[Tooltip("the shadow strength to apply to the light over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve ShadowStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AB3 RID: 6835
		[Tooltip("the value to remap the shadow strength's curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapShadowStrengthZero;

		// Token: 0x04001AB4 RID: 6836
		[Tooltip("the value to remap the shadow strength's curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapShadowStrengthOne = 1f;

		// Token: 0x04001AB5 RID: 6837
		[Tooltip("the value to move the shadow strength to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantShadowStrength = 1f;

		// Token: 0x04001AB6 RID: 6838
		[Tooltip("the value to move the shadow strength to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationShadowStrength = 1f;

		// Token: 0x04001AB7 RID: 6839
		[MMFInspectorGroup("Volumetric Intensity", true, 39, true, false)]
		[Tooltip("whether or not to modify the volumetric intensity of the light")]
		public bool ModifyVolumetricIntensity = true;

		// Token: 0x04001AB8 RID: 6840
		[Tooltip("the curve to tween the volumetric intensity on")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3 })]
		public AnimationCurve VolumetricIntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AB9 RID: 6841
		[Tooltip("the value to remap the volumetric intensity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapVolumetricIntensityZero;

		// Token: 0x04001ABA RID: 6842
		[Tooltip("the value to remap the volumetric intensity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float RemapVolumetricIntensityOne = 1f;

		// Token: 0x04001ABB RID: 6843
		[Tooltip("the value to move the volumetric intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantVolumetricIntensity = 1f;

		// Token: 0x04001ABC RID: 6844
		[Tooltip("the value to move the volumetric intensity to in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public float ToDestinationVolumetricIntensity = 1f;

		// Token: 0x04001ABD RID: 6845
		protected float _initialFalloff;

		// Token: 0x04001ABE RID: 6846
		protected float _initialShadowStrength;

		// Token: 0x04001ABF RID: 6847
		protected float _initialIntensity;

		// Token: 0x04001AC0 RID: 6848
		protected float _initialVolumetricIntensity;

		// Token: 0x04001AC1 RID: 6849
		protected Coroutine _coroutine;

		// Token: 0x04001AC2 RID: 6850
		protected Color _initialColor;

		// Token: 0x04001AC3 RID: 6851
		protected Color _targetColor;

		// Token: 0x02000584 RID: 1412
		public enum Modes
		{
			// Token: 0x040024DA RID: 9434
			OverTime,
			// Token: 0x040024DB RID: 9435
			Instant,
			// Token: 0x040024DC RID: 9436
			ShakerEvent,
			// Token: 0x040024DD RID: 9437
			ToDestination
		}
	}
}
