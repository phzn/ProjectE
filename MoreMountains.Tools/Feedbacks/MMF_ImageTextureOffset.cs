using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001EF RID: 495
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control the texture offset of a target UI Image over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Image Texture Offset")]
	public class MMF_ImageTextureOffset : MMF_Feedback
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0003757B File Offset: 0x0003577B
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0003757E File Offset: 0x0003577E
		protected override void AutomateTargetAcquisition()
		{
			this.TargetImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003758C File Offset: 0x0003578C
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x000375A9 File Offset: 0x000357A9
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_ImageTextureOffset.Modes.Instant)
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x000375B2 File Offset: 0x000357B2
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000375B8 File Offset: 0x000357B8
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._material = this.TargetImage.materialForRendering;
			MMF_ImageTextureOffset.MaterialPropertyTypes materialPropertyType = this.MaterialPropertyType;
			if (materialPropertyType == MMF_ImageTextureOffset.MaterialPropertyTypes.Main)
			{
				this._initialValue = this._material.mainTextureOffset;
				return;
			}
			if (materialPropertyType != MMF_ImageTextureOffset.MaterialPropertyTypes.TextureID)
			{
				return;
			}
			this._initialValue = this._material.GetTextureOffset(this.MaterialPropertyName);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00037618 File Offset: 0x00035818
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageTextureOffset.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMF_ImageTextureOffset.Modes mode = this.Mode;
			if (mode != MMF_ImageTextureOffset.Modes.OverTime)
			{
				if (mode == MMF_ImageTextureOffset.Modes.Instant)
				{
					this.ApplyValue(this.InstantOffset * num);
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

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000376A1 File Offset: 0x000358A1
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

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000376B8 File Offset: 0x000358B8
		protected virtual void SetMaterialValues(float time, float intensityMultiplier)
		{
			this._newValue.x = MMFeedbacksHelpers.Remap(this.OffsetCurve.Evaluate(time), 0f, 1f, this.RemapZero.x, this.RemapOne.x);
			this._newValue.y = MMFeedbacksHelpers.Remap(this.OffsetCurve.Evaluate(time), 0f, 1f, this.RemapZero.y, this.RemapOne.y);
			if (this.RelativeValues)
			{
				this._newValue += this._initialValue;
			}
			this.ApplyValue(this._newValue * intensityMultiplier);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00037770 File Offset: 0x00035970
		protected virtual void ApplyValue(Vector2 newValue)
		{
			MMF_ImageTextureOffset.MaterialPropertyTypes materialPropertyType = this.MaterialPropertyType;
			if (materialPropertyType == MMF_ImageTextureOffset.MaterialPropertyTypes.Main)
			{
				this._material.mainTextureOffset = newValue;
				return;
			}
			if (materialPropertyType != MMF_ImageTextureOffset.MaterialPropertyTypes.TextureID)
			{
				return;
			}
			this._material.SetTextureOffset(this.MaterialPropertyName, newValue);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000377AC File Offset: 0x000359AC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageTextureOffset.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x04000E80 RID: 3712
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E81 RID: 3713
		[MMFInspectorGroup("Texture Scale", true, 63, true, false)]
		[Tooltip("the UI Image on which to change texture offset on")]
		public Image TargetImage;

		// Token: 0x04000E82 RID: 3714
		[Tooltip("whether to target the main texture property, or one specified in MaterialPropertyName")]
		public MMF_ImageTextureOffset.MaterialPropertyTypes MaterialPropertyType;

		// Token: 0x04000E83 RID: 3715
		[Tooltip("the property name, for example _MainTex_ST, or _MainTex if you don't have UseMaterialPropertyBlocks set to true")]
		[MMEnumCondition("MaterialPropertyType", new int[] { 1 })]
		public string MaterialPropertyName = "_MainTex_ST";

		// Token: 0x04000E84 RID: 3716
		[Tooltip("whether the feedback should affect the material instantly or over a period of time")]
		public MMF_ImageTextureOffset.Modes Mode;

		// Token: 0x04000E85 RID: 3717
		[Tooltip("how long the material should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x04000E86 RID: 3718
		[Tooltip("whether or not the values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x04000E87 RID: 3719
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000E88 RID: 3720
		[MMFInspectorGroup("Intensity", true, 65, false, false)]
		[Tooltip("the curve to tween the offset on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve OffsetCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000E89 RID: 3721
		[Tooltip("the value to remap the offset curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x04000E8A RID: 3722
		[Tooltip("the value to remap the offset curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;

		// Token: 0x04000E8B RID: 3723
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Vector2 InstantOffset;

		// Token: 0x04000E8C RID: 3724
		protected Vector2 _initialValue;

		// Token: 0x04000E8D RID: 3725
		protected Coroutine _coroutine;

		// Token: 0x04000E8E RID: 3726
		protected Vector2 _newValue;

		// Token: 0x04000E8F RID: 3727
		protected Material _material;

		// Token: 0x020004BB RID: 1211
		public enum Modes
		{
			// Token: 0x0400220A RID: 8714
			OverTime,
			// Token: 0x0400220B RID: 8715
			Instant
		}

		// Token: 0x020004BC RID: 1212
		public enum MaterialPropertyTypes
		{
			// Token: 0x0400220D RID: 8717
			Main,
			// Token: 0x0400220E RID: 8718
			TextureID
		}
	}
}
