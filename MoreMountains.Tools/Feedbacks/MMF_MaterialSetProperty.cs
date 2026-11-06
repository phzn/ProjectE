using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F7 RID: 503
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you set a property on the target renderer's material")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Material Set Property")]
	public class MMF_MaterialSetProperty : MMF_Feedback
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00039165 File Offset: 0x00037365
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00039168 File Offset: 0x00037368
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0003916B File Offset: 0x0003736B
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003916E File Offset: 0x0003736E
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRenderer = this.FindAutomatedTarget<Renderer>();
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x0003917C File Offset: 0x0003737C
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00039198 File Offset: 0x00037398
		public override float FeedbackDuration
		{
			get
			{
				if (!this.InterpolateValue)
				{
					return 0f;
				}
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				if (this.InterpolateValue)
				{
					this.Duration = value;
				}
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000391AC File Offset: 0x000373AC
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._propertyID = Shader.PropertyToID(this.PropertyID);
			if (this.Active)
			{
				switch (this.PropertyType)
				{
				case MMF_MaterialSetProperty.PropertyTypes.Color:
					this._initialColor = this.TargetRenderer.materials[this.MaterialID].GetColor(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.Float:
					this._initialFloat = this.TargetRenderer.materials[this.MaterialID].GetFloat(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.Integer:
					this._initialInt = this.TargetRenderer.materials[this.MaterialID].GetInt(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.Texture:
					this._initialTexture = this.TargetRenderer.materials[this.MaterialID].GetTexture(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.TextureOffset:
					this._initialOffset = this.TargetRenderer.materials[this.MaterialID].GetTextureOffset(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.TextureScale:
					this._initialScale = this.TargetRenderer.materials[this.MaterialID].GetTextureScale(this._propertyID);
					return;
				case MMF_MaterialSetProperty.PropertyTypes.Vector:
					this._initialVector = this.TargetRenderer.materials[this.MaterialID].GetVector(this._propertyID);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00039304 File Offset: 0x00037504
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MaterialSetProperty.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetRenderer == null)
			{
				return;
			}
			if (this.InterpolateValue)
			{
				this.Owner.StartCoroutine(this.InterpolationSequence(feedbacksIntensity));
				return;
			}
			switch (this.PropertyType)
			{
			case MMF_MaterialSetProperty.PropertyTypes.Color:
				this.TargetRenderer.materials[this.MaterialID].SetColor(this._propertyID, this.NewColor);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Float:
				this.TargetRenderer.materials[this.MaterialID].SetFloat(this._propertyID, this.NewFloat);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Integer:
				this.TargetRenderer.materials[this.MaterialID].SetInt(this._propertyID, this.NewInt);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Texture:
				this.TargetRenderer.materials[this.MaterialID].SetTexture(this._propertyID, this.NewTexture);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureOffset:
				this.TargetRenderer.materials[this.MaterialID].SetTextureOffset(this._propertyID, this.NewOffset);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureScale:
				this.TargetRenderer.materials[this.MaterialID].SetTextureScale(this._propertyID, this.NewScale);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Vector:
				this.TargetRenderer.materials[this.MaterialID].SetVector(this._propertyID, this.NewVector);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00039471 File Offset: 0x00037671
		protected virtual IEnumerator InterpolationSequence(float intensityMultiplier)
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValueAtTime(num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValueAtTime(this.FinalNormalizedTime, intensityMultiplier);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00039488 File Offset: 0x00037688
		protected virtual void SetValueAtTime(float t, float intensityMultiplier)
		{
			switch (this.PropertyType)
			{
			case MMF_MaterialSetProperty.PropertyTypes.Color:
			{
				float num = MMTween.Tween(t, 0f, 1f, this._initialFloat, this.NewFloat, this.InterpolationCurve);
				this._newColor = Color.Lerp(this._initialColor, this.NewColor, num);
				this.TargetRenderer.materials[this.MaterialID].SetColor(this._propertyID, this._newColor);
				return;
			}
			case MMF_MaterialSetProperty.PropertyTypes.Float:
			{
				float num2 = MMTween.Tween(t, 0f, 1f, this._initialFloat, this.NewFloat, this.InterpolationCurve);
				this.TargetRenderer.materials[this.MaterialID].SetFloat(this._propertyID, num2);
				return;
			}
			case MMF_MaterialSetProperty.PropertyTypes.Integer:
			{
				int num3 = (int)MMTween.Tween(t, 0f, 1f, (long)this._initialInt, (long)this.NewInt, this.InterpolationCurve);
				this.TargetRenderer.materials[this.MaterialID].SetInt(this._propertyID, num3);
				return;
			}
			case MMF_MaterialSetProperty.PropertyTypes.Texture:
				this.TargetRenderer.materials[this.MaterialID].SetTexture(this._propertyID, this.NewTexture);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureOffset:
				this._newVector2 = MMTween.Tween(t, 0f, 1f, this._initialOffset, this.NewOffset, this.InterpolationCurve);
				this.TargetRenderer.materials[this.MaterialID].SetTextureOffset(this._propertyID, this._newVector2);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureScale:
				this._newVector2 = MMTween.Tween(t, 0f, 1f, this._initialScale, this.NewScale, this.InterpolationCurve);
				this.TargetRenderer.materials[this.MaterialID].SetTextureScale(this._propertyID, this._newVector2);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Vector:
				this._newVector4 = MMTween.Tween(t, 0f, 1f, this._initialVector, this.NewVector, this.InterpolationCurve);
				this.TargetRenderer.materials[this.MaterialID].SetVector(this._propertyID, this._newVector4);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000396B8 File Offset: 0x000378B8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MaterialSetProperty.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00039704 File Offset: 0x00037904
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_MaterialSetProperty.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.PropertyType)
			{
			case MMF_MaterialSetProperty.PropertyTypes.Color:
				this.TargetRenderer.materials[this.MaterialID].SetColor(this._propertyID, this._initialColor);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Float:
				this.TargetRenderer.materials[this.MaterialID].SetFloat(this._propertyID, this._initialFloat);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Integer:
				this.TargetRenderer.materials[this.MaterialID].SetInt(this._propertyID, this._initialInt);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Texture:
				this.TargetRenderer.materials[this.MaterialID].SetTexture(this._propertyID, this._initialTexture);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureOffset:
				this.TargetRenderer.materials[this.MaterialID].SetTextureOffset(this._propertyID, this._initialOffset);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.TextureScale:
				this.TargetRenderer.materials[this.MaterialID].SetTextureScale(this._propertyID, this._initialScale);
				return;
			case MMF_MaterialSetProperty.PropertyTypes.Vector:
				this.TargetRenderer.materials[this.MaterialID].SetVector(this._propertyID, this._initialVector);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000F0B RID: 3851
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F0C RID: 3852
		[MMFInspectorGroup("Material", true, 12, true, false)]
		[Tooltip("the renderer to change the material on")]
		public Renderer TargetRenderer;

		// Token: 0x04000F0D RID: 3853
		[Tooltip("the ID of the material to target on the renderer")]
		public int MaterialID;

		// Token: 0x04000F0E RID: 3854
		[Tooltip("the ID of the property to set, as exposed by the Visual Effect Graph")]
		public string PropertyID;

		// Token: 0x04000F0F RID: 3855
		[Tooltip("the type of the property to set")]
		public MMF_MaterialSetProperty.PropertyTypes PropertyType = MMF_MaterialSetProperty.PropertyTypes.Float;

		// Token: 0x04000F10 RID: 3856
		[Tooltip("if the property is a color, the new color to set")]
		[MMFEnumCondition("PropertyType", new int[] { 0 })]
		public Color NewColor = Color.red;

		// Token: 0x04000F11 RID: 3857
		[Tooltip("if the property is a float, the new float to set")]
		[MMFEnumCondition("PropertyType", new int[] { 1 })]
		public float NewFloat = 1f;

		// Token: 0x04000F12 RID: 3858
		[Tooltip("if the property is an int, the new int to set")]
		[MMFEnumCondition("PropertyType", new int[] { 2 })]
		public int NewInt;

		// Token: 0x04000F13 RID: 3859
		[Tooltip("if the property is a texture, the new texture to set")]
		[MMFEnumCondition("PropertyType", new int[] { 3 })]
		public Texture NewTexture;

		// Token: 0x04000F14 RID: 3860
		[Tooltip("if the property is a texture offset, the new offset to set")]
		[MMFEnumCondition("PropertyType", new int[] { 4 })]
		public Vector2 NewOffset;

		// Token: 0x04000F15 RID: 3861
		[Tooltip("if the property is a texture scale, the new scale to set")]
		[MMFEnumCondition("PropertyType", new int[] { 5 })]
		public Vector2 NewScale;

		// Token: 0x04000F16 RID: 3862
		[Tooltip("if the property is a vector4, the new vector4 to set")]
		[MMFEnumCondition("PropertyType", new int[] { 6 })]
		public Vector4 NewVector;

		// Token: 0x04000F17 RID: 3863
		[Header("Interpolation")]
		[Tooltip("whether or not to interpolate the value over time. If set to false, the change will be instant")]
		public bool InterpolateValue;

		// Token: 0x04000F18 RID: 3864
		[Tooltip("the duration of the interpolation")]
		[MMFCondition("InterpolateValue", true)]
		public float Duration = 2f;

		// Token: 0x04000F19 RID: 3865
		[Tooltip("the curve over which to interpolate the value")]
		[MMFCondition("InterpolateValue", true)]
		public MMTweenType InterpolationCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000F1A RID: 3866
		protected int _propertyID;

		// Token: 0x04000F1B RID: 3867
		protected Color _initialColor;

		// Token: 0x04000F1C RID: 3868
		protected float _initialFloat;

		// Token: 0x04000F1D RID: 3869
		protected int _initialInt;

		// Token: 0x04000F1E RID: 3870
		protected Texture _initialTexture;

		// Token: 0x04000F1F RID: 3871
		protected Vector2 _initialOffset;

		// Token: 0x04000F20 RID: 3872
		protected Vector2 _initialScale;

		// Token: 0x04000F21 RID: 3873
		protected Vector4 _initialVector;

		// Token: 0x04000F22 RID: 3874
		protected Coroutine _coroutine;

		// Token: 0x04000F23 RID: 3875
		protected Color _newColor;

		// Token: 0x04000F24 RID: 3876
		protected Vector2 _newVector2;

		// Token: 0x04000F25 RID: 3877
		protected Vector2 _newVector4;

		// Token: 0x020004CA RID: 1226
		public enum PropertyTypes
		{
			// Token: 0x04002248 RID: 8776
			Color,
			// Token: 0x04002249 RID: 8777
			Float,
			// Token: 0x0400224A RID: 8778
			Integer,
			// Token: 0x0400224B RID: 8779
			Texture,
			// Token: 0x0400224C RID: 8780
			TextureOffset,
			// Token: 0x0400224D RID: 8781
			TextureScale,
			// Token: 0x0400224E RID: 8782
			Vector
		}
	}
}
