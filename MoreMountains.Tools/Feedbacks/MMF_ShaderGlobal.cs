using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020C RID: 524
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to set global properties on your shader, or enable/disable keywords.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Shader Global")]
	public class MMF_ShaderGlobal : MMF_Feedback
	{
		// Token: 0x06000E37 RID: 3639 RVA: 0x0003F2D4 File Offset: 0x0003D4D4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ShaderGlobal.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_ShaderGlobal.Modes.SetGlobalColor:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalColor(this.PropertyNameID, this.GlobalColor);
					return;
				}
				Shader.SetGlobalColor(this.PropertyName, this.GlobalColor);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalFloat:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalFloat(this.PropertyNameID, this.GlobalFloat);
					return;
				}
				Shader.SetGlobalFloat(this.PropertyName, this.GlobalFloat);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalInt:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalInt(this.PropertyNameID, this.GlobalInt);
					return;
				}
				Shader.SetGlobalInt(this.PropertyName, this.GlobalInt);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalMatrix:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalMatrix(this.PropertyNameID, this.GlobalMatrix);
					return;
				}
				Shader.SetGlobalMatrix(this.PropertyName, this.GlobalMatrix);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalTexture:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalTexture(this.PropertyNameID, this.GlobalTexture);
					return;
				}
				Shader.SetGlobalTexture(this.PropertyName, this.GlobalTexture);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalVector:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalVector(this.PropertyNameID, this.GlobalVector);
					return;
				}
				Shader.SetGlobalVector(this.PropertyName, this.GlobalVector);
				return;
			case MMF_ShaderGlobal.Modes.EnableKeyword:
				Shader.EnableKeyword(this.Keyword);
				return;
			case MMF_ShaderGlobal.Modes.DisableKeyword:
				Shader.DisableKeyword(this.Keyword);
				return;
			case MMF_ShaderGlobal.Modes.WarmupAllShaders:
				Shader.WarmupAllShaders();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0003F484 File Offset: 0x0003D684
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ShaderGlobal.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_ShaderGlobal.Modes.SetGlobalColor:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalColor(this.PropertyNameID, this._initialColor);
					return;
				}
				Shader.SetGlobalColor(this.PropertyName, this._initialColor);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalFloat:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalFloat(this.PropertyNameID, this._initialFloat);
					return;
				}
				Shader.SetGlobalFloat(this.PropertyName, this._initialFloat);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalInt:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalInt(this.PropertyNameID, this._initialInt);
					return;
				}
				Shader.SetGlobalInt(this.PropertyName, this._initialInt);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalMatrix:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalMatrix(this.PropertyNameID, this._initialMatrix);
					return;
				}
				Shader.SetGlobalMatrix(this.PropertyName, this._initialMatrix);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalTexture:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalTexture(this.PropertyNameID, this._initialTexture);
					return;
				}
				Shader.SetGlobalTexture(this.PropertyName, this._initialTexture);
				return;
			case MMF_ShaderGlobal.Modes.SetGlobalVector:
				if (this.PropertyName == "")
				{
					Shader.SetGlobalVector(this.PropertyNameID, this._initialVector);
					return;
				}
				Shader.SetGlobalVector(this.PropertyName, this._initialVector);
				return;
			default:
				return;
			}
		}

		// Token: 0x0400106E RID: 4206
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400106F RID: 4207
		[MMFInspectorGroup("Shader Global", true, 24, false, false)]
		[Tooltip("the selected mode for this feedback")]
		public MMF_ShaderGlobal.Modes Mode = MMF_ShaderGlobal.Modes.SetGlobalFloat;

		// Token: 0x04001070 RID: 4208
		[Tooltip("the name of the global property")]
		[MMFEnumCondition("Mode", new int[] { 0, 1, 2, 3, 4, 5 })]
		public string PropertyName = "";

		// Token: 0x04001071 RID: 4209
		[Tooltip("the name ID of the property retrieved by Shader.PropertyToID")]
		[MMFEnumCondition("Mode", new int[] { 0, 1, 2, 3, 4, 5 })]
		public int PropertyNameID;

		// Token: 0x04001072 RID: 4210
		[Tooltip("a global color property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Color GlobalColor = Color.yellow;

		// Token: 0x04001073 RID: 4211
		[Tooltip("a global float property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float GlobalFloat = 1f;

		// Token: 0x04001074 RID: 4212
		[Tooltip("a global int property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public int GlobalInt = 1;

		// Token: 0x04001075 RID: 4213
		[Tooltip("a global matrix property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public Matrix4x4 GlobalMatrix = Matrix4x4.identity;

		// Token: 0x04001076 RID: 4214
		[Tooltip("a global texture property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 4 })]
		public RenderTexture GlobalTexture;

		// Token: 0x04001077 RID: 4215
		[Tooltip("a global vector property for all shaders")]
		[MMFEnumCondition("Mode", new int[] { 5 })]
		public Vector4 GlobalVector;

		// Token: 0x04001078 RID: 4216
		[Tooltip("a global shader keyword")]
		[MMFEnumCondition("Mode", new int[] { 6, 7 })]
		public string Keyword;

		// Token: 0x04001079 RID: 4217
		protected Color _initialColor;

		// Token: 0x0400107A RID: 4218
		protected float _initialFloat;

		// Token: 0x0400107B RID: 4219
		protected int _initialInt;

		// Token: 0x0400107C RID: 4220
		protected Matrix4x4 _initialMatrix;

		// Token: 0x0400107D RID: 4221
		protected RenderTexture _initialTexture;

		// Token: 0x0400107E RID: 4222
		protected Vector4 _initialVector;

		// Token: 0x020004ED RID: 1261
		public enum Modes
		{
			// Token: 0x04002301 RID: 8961
			SetGlobalColor,
			// Token: 0x04002302 RID: 8962
			SetGlobalFloat,
			// Token: 0x04002303 RID: 8963
			SetGlobalInt,
			// Token: 0x04002304 RID: 8964
			SetGlobalMatrix,
			// Token: 0x04002305 RID: 8965
			SetGlobalTexture,
			// Token: 0x04002306 RID: 8966
			SetGlobalVector,
			// Token: 0x04002307 RID: 8967
			EnableKeyword,
			// Token: 0x04002308 RID: 8968
			DisableKeyword,
			// Token: 0x04002309 RID: 8969
			WarmupAllShaders
		}
	}
}
