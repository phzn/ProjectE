using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000214 RID: 532
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control the texture scale of a target material over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Texture Scale")]
	public class MMF_TextureScale : MMF_Feedback
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00040E95 File Offset: 0x0003F095
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00040E98 File Offset: 0x0003F098
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRenderer = this.FindAutomatedTarget<Renderer>();
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00040EA6 File Offset: 0x0003F0A6
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x00040EC3 File Offset: 0x0003F0C3
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_TextureScale.Modes.Instant)
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

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00040ECC File Offset: 0x0003F0CC
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00040ED0 File Offset: 0x0003F0D0
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.UseMaterialPropertyBlocks)
			{
				this._propertyBlock = new MaterialPropertyBlock();
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlockVector.x = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).w;
				this._propertyBlockVector.y = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).z;
				this._initialValue.x = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).x;
				this._initialValue.y = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).y;
				return;
			}
			this._initialValue = this.TargetRenderer.materials[this.MaterialIndex].GetTextureScale(this.MaterialPropertyName);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00040FE4 File Offset: 0x0003F1E4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TextureScale.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMF_TextureScale.Modes mode = this.Mode;
			if (mode != MMF_TextureScale.Modes.OverTime)
			{
				if (mode == MMF_TextureScale.Modes.Instant)
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

		// Token: 0x06000E9C RID: 3740 RVA: 0x0004106D File Offset: 0x0003F26D
		protected virtual IEnumerator TransitionCo(float intensityMultiplier)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetMaterialValues(num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetMaterialValues(this.FinalNormalizedTime, intensityMultiplier);
			this.IsPlaying = true;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00041084 File Offset: 0x0003F284
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

		// Token: 0x06000E9E RID: 3742 RVA: 0x0004113C File Offset: 0x0003F33C
		protected virtual void ApplyValue(Vector2 newValue)
		{
			if (this.UseMaterialPropertyBlocks)
			{
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlockVector.x = newValue.x;
				this._propertyBlockVector.y = newValue.y;
				this._propertyBlock.SetVector(this.MaterialPropertyName, this._propertyBlockVector);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.MaterialIndex);
				return;
			}
			this.TargetRenderer.materials[this.MaterialIndex].SetTextureScale(this.MaterialPropertyName, newValue);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x000411D4 File Offset: 0x0003F3D4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TextureScale.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00041220 File Offset: 0x0003F420
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TextureScale.FeedbackTypeAuthorized)
			{
				return;
			}
			this.ApplyValue(this._initialValue);
		}

		// Token: 0x040010DE RID: 4318
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010DF RID: 4319
		[MMFInspectorGroup("Texture Scale", true, 62, true, false)]
		[Tooltip("the renderer on which to change texture scale on")]
		public Renderer TargetRenderer;

		// Token: 0x040010E0 RID: 4320
		[Tooltip("the material index")]
		public int MaterialIndex;

		// Token: 0x040010E1 RID: 4321
		[Tooltip("the property name, for example _MainTex_ST, or _MainTex if you don't have UseMaterialPropertyBlocks set to true")]
		public string MaterialPropertyName = "_MainTex_ST";

		// Token: 0x040010E2 RID: 4322
		[Tooltip("whether the feedback should affect the material instantly or over a period of time")]
		public MMF_TextureScale.Modes Mode;

		// Token: 0x040010E3 RID: 4323
		[Tooltip("how long the material should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x040010E4 RID: 4324
		[Tooltip("whether or not the values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x040010E5 RID: 4325
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040010E6 RID: 4326
		[Tooltip("if this is true, this component will use material property blocks instead of working on an instance of the material.")]
		public bool UseMaterialPropertyBlocks;

		// Token: 0x040010E7 RID: 4327
		[MMFInspectorGroup("Intensity", true, 64, false, false)]
		[Tooltip("the curve to tween the scale on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040010E8 RID: 4328
		[Tooltip("the value to remap the scale curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x040010E9 RID: 4329
		[Tooltip("the value to remap the scale curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;

		// Token: 0x040010EA RID: 4330
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Vector2 InstantScale;

		// Token: 0x040010EB RID: 4331
		protected Vector2 _initialValue;

		// Token: 0x040010EC RID: 4332
		protected Coroutine _coroutine;

		// Token: 0x040010ED RID: 4333
		protected Vector2 _newValue;

		// Token: 0x040010EE RID: 4334
		protected MaterialPropertyBlock _propertyBlock;

		// Token: 0x040010EF RID: 4335
		protected Vector4 _propertyBlockVector;

		// Token: 0x02000500 RID: 1280
		public enum Modes
		{
			// Token: 0x04002363 RID: 9059
			OverTime,
			// Token: 0x04002364 RID: 9060
			Instant
		}
	}
}
