using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000213 RID: 531
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control the texture offset of a target material over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Texture Offset")]
	public class MMF_TextureOffset : MMF_Feedback
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x00040A4A File Offset: 0x0003EC4A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00040A4D File Offset: 0x0003EC4D
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRenderer = this.FindAutomatedTarget<Renderer>();
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00040A5B File Offset: 0x0003EC5B
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x00040A78 File Offset: 0x0003EC78
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_TextureOffset.Modes.Instant)
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

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00040A81 File Offset: 0x0003EC81
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00040A84 File Offset: 0x0003EC84
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.UseMaterialPropertyBlocks)
			{
				this._propertyBlock = new MaterialPropertyBlock();
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlockVector.x = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).x;
				this._propertyBlockVector.y = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).y;
				this._initialValue.x = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).z;
				this._initialValue.y = this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).w;
				return;
			}
			this._initialValue = this.TargetRenderer.materials[this.MaterialIndex].GetTextureOffset(this.MaterialPropertyName);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00040B98 File Offset: 0x0003ED98
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TextureOffset.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMF_TextureOffset.Modes mode = this.Mode;
			if (mode != MMF_TextureOffset.Modes.OverTime)
			{
				if (mode == MMF_TextureOffset.Modes.Instant)
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

		// Token: 0x06000E8E RID: 3726 RVA: 0x00040C21 File Offset: 0x0003EE21
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

		// Token: 0x06000E8F RID: 3727 RVA: 0x00040C38 File Offset: 0x0003EE38
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

		// Token: 0x06000E90 RID: 3728 RVA: 0x00040CF0 File Offset: 0x0003EEF0
		protected virtual void ApplyValue(Vector2 newValue)
		{
			if (this.UseMaterialPropertyBlocks)
			{
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlockVector.z = newValue.x;
				this._propertyBlockVector.w = newValue.y;
				this._propertyBlock.SetVector(this.MaterialPropertyName, this._propertyBlockVector);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.MaterialIndex);
				return;
			}
			this.TargetRenderer.materials[this.MaterialIndex].SetTextureOffset(this.MaterialPropertyName, newValue);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00040D88 File Offset: 0x0003EF88
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TextureOffset.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00040DD4 File Offset: 0x0003EFD4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TextureOffset.FeedbackTypeAuthorized)
			{
				return;
			}
			this.ApplyValue(this._initialValue);
		}

		// Token: 0x040010CC RID: 4300
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010CD RID: 4301
		[MMFInspectorGroup("Texture Scale", true, 63, true, false)]
		[Tooltip("the renderer on which to change texture offset on")]
		public Renderer TargetRenderer;

		// Token: 0x040010CE RID: 4302
		[Tooltip("the material index")]
		public int MaterialIndex;

		// Token: 0x040010CF RID: 4303
		[Tooltip("the property name, for example _MainTex_ST, or _MainTex if you don't have UseMaterialPropertyBlocks set to true")]
		public string MaterialPropertyName = "_MainTex_ST";

		// Token: 0x040010D0 RID: 4304
		[Tooltip("whether the feedback should affect the material instantly or over a period of time")]
		public MMF_TextureOffset.Modes Mode;

		// Token: 0x040010D1 RID: 4305
		[Tooltip("how long the material should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float Duration = 0.2f;

		// Token: 0x040010D2 RID: 4306
		[Tooltip("whether or not the values should be relative")]
		public bool RelativeValues = true;

		// Token: 0x040010D3 RID: 4307
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040010D4 RID: 4308
		[Tooltip("if this is true, this component will use material property blocks instead of working on an instance of the material.")]
		public bool UseMaterialPropertyBlocks;

		// Token: 0x040010D5 RID: 4309
		[MMFInspectorGroup("Intensity", true, 65, false, false)]
		[Tooltip("the curve to tween the offset on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve OffsetCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040010D6 RID: 4310
		[Tooltip("the value to remap the offset curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x040010D7 RID: 4311
		[Tooltip("the value to remap the offset curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;

		// Token: 0x040010D8 RID: 4312
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Vector2 InstantOffset;

		// Token: 0x040010D9 RID: 4313
		protected Vector2 _initialValue;

		// Token: 0x040010DA RID: 4314
		protected Coroutine _coroutine;

		// Token: 0x040010DB RID: 4315
		protected Vector2 _newValue;

		// Token: 0x040010DC RID: 4316
		protected MaterialPropertyBlock _propertyBlock;

		// Token: 0x040010DD RID: 4317
		protected Vector4 _propertyBlockVector;

		// Token: 0x020004FE RID: 1278
		public enum Modes
		{
			// Token: 0x0400235B RID: 9051
			OverTime,
			// Token: 0x0400235C RID: 9052
			Instant
		}
	}
}
