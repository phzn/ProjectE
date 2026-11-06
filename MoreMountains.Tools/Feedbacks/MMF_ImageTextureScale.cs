using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F0 RID: 496
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control the texture scale of a target UI Image over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Image Texture Scale")]
	public class MMF_ImageTextureScale : MMF_Feedback
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00037899 File Offset: 0x00035A99
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003789C File Offset: 0x00035A9C
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0003789F File Offset: 0x00035A9F
		protected override void AutomateTargetAcquisition()
		{
			this.TargetImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000378AD File Offset: 0x00035AAD
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x000378CA File Offset: 0x00035ACA
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_ImageTextureScale.Modes.Instant)
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

		// Token: 0x06000CDE RID: 3294 RVA: 0x000378D4 File Offset: 0x00035AD4
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._material = this.TargetImage.material;
			MMF_ImageTextureScale.MaterialPropertyTypes materialPropertyType = this.MaterialPropertyType;
			if (materialPropertyType == MMF_ImageTextureScale.MaterialPropertyTypes.Main)
			{
				this._initialValue = this._material.mainTextureScale;
				return;
			}
			if (materialPropertyType != MMF_ImageTextureScale.MaterialPropertyTypes.TextureID)
			{
				return;
			}
			this._initialValue = this._material.GetTextureScale(this.MaterialPropertyName);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00037934 File Offset: 0x00035B34
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageTextureScale.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMF_ImageTextureScale.Modes mode = this.Mode;
			if (mode != MMF_ImageTextureScale.Modes.OverTime)
			{
				if (mode == MMF_ImageTextureScale.Modes.Instant)
				{
					this.ApplyValue(this.InstantScale * num);
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
				this._coroutine = this.Owner.StartCoroutine(this.TransitionCo(num));
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000379BD File Offset: 0x00035BBD
		protected virtual IEnumerator TransitionCo(float intensityMultiplier)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetMaterialValues(num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetMaterialValues(this.FinalNormalizedTime, intensityMultiplier);
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000379D4 File Offset: 0x00035BD4
		protected virtual void SetMaterialValues(float time, float intensityMultiplier)
		{
			this._newValue.x = MMFeedbacksHelpers.Remap(this.ScaleCurve.Evaluate(time), 0f, 1f, this.RemapZero.x, this.RemapOne.x);
			this._newValue.y = MMFeedbacksHelpers.Remap(this.ScaleCurve.Evaluate(time), 0f, 1f, this.RemapZero.y, this.RemapOne.y);
			if (this.RelativeValues)
			{
				this._newValue += this._initialValue;
			}
			this.ApplyValue(this._newValue * intensityMultiplier);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00037A8C File Offset: 0x00035C8C
		protected virtual void ApplyValue(Vector2 newValue)
		{
			MMF_ImageTextureScale.MaterialPropertyTypes materialPropertyType = this.MaterialPropertyType;
			if (materialPropertyType == MMF_ImageTextureScale.MaterialPropertyTypes.Main)
			{
				this._material.mainTextureScale = newValue;
				return;
			}
			if (materialPropertyType != MMF_ImageTextureScale.MaterialPropertyTypes.TextureID)
			{
				return;
			}
			this._material.SetTextureScale(this.MaterialPropertyName, newValue);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00037AC8 File Offset: 0x00035CC8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageTextureScale.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x04000E90 RID: 3728
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E91 RID: 3729
		[MMFInspectorGroup("Texture Scale", true, 63, true, false)]
		[Tooltip("the UI Image on which to change texture scale on")]
		public Image TargetImage;

		// Token: 0x04000E92 RID: 3730
		[Tooltip("whether to target the main texture property, or one specified in MaterialPropertyName")]
		public MMF_ImageTextureScale.MaterialPropertyTypes MaterialPropertyType;

		// Token: 0x04000E93 RID: 3731
		[Tooltip("the property name, for example _MainTex_ST, or _MainTex if you don't have UseMaterialPropertyBlocks set to true")]
		[MMEnumCondition("MaterialPropertyType", new int[] { 1 })]
		public string MaterialPropertyName = "_MainTex_ST";

		// Token: 0x04000E94 RID: 3732
		[Tooltip("whether the feedback should affect the material instantly or over a period of time")]
		public MMF_ImageTextureScale.Modes Mode;

		// Token: 0x04000E95 RID: 3733
		[Tooltip("how long the material should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x04000E96 RID: 3734
		[Tooltip("whether or not the values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x04000E97 RID: 3735
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000E98 RID: 3736
		[MMFInspectorGroup("Intensity", true, 65, false, false)]
		[Tooltip("the curve to tween the scale on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000E99 RID: 3737
		[Tooltip("the value to remap the scale curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x04000E9A RID: 3738
		[Tooltip("the value to remap the scale curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;

		// Token: 0x04000E9B RID: 3739
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Vector2 InstantScale;

		// Token: 0x04000E9C RID: 3740
		protected Vector2 _initialValue;

		// Token: 0x04000E9D RID: 3741
		protected Coroutine _coroutine;

		// Token: 0x04000E9E RID: 3742
		protected Vector2 _newValue;

		// Token: 0x04000E9F RID: 3743
		protected Material _material;

		// Token: 0x020004BE RID: 1214
		public enum Modes
		{
			// Token: 0x04002215 RID: 8725
			OverTime,
			// Token: 0x04002216 RID: 8726
			Instant
		}

		// Token: 0x020004BF RID: 1215
		public enum MaterialPropertyTypes
		{
			// Token: 0x04002218 RID: 8728
			Main,
			// Token: 0x04002219 RID: 8729
			TextureID
		}
	}
}
