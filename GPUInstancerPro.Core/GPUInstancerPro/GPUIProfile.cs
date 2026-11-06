using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000024 RID: 36
	[CreateAssetMenu(menuName = "Rendering/GPU Instancer Pro/Profile", order = 511)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#Profile_Settings")]
	public class GPUIProfile : ScriptableObject, IGPUIParameterBufferData
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000074B5 File Offset: 0x000056B5
		public static GPUIProfile DefaultProfile
		{
			get
			{
				if (GPUIProfile._defaultProfile == null)
				{
					GPUIProfile._defaultProfile = ScriptableObject.CreateInstance<GPUIProfile>();
					GPUIProfile._defaultProfile.isDefaultProfile = true;
				}
				return GPUIProfile._defaultProfile;
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000074E0 File Offset: 0x000056E0
		public static GPUIProfile CreateNewProfile(string name, GPUIProfile copyFrom = null)
		{
			GPUIProfile gpuiprofile;
			if (copyFrom == null)
			{
				gpuiprofile = ScriptableObject.CreateInstance<GPUIProfile>();
			}
			else
			{
				gpuiprofile = Object.Instantiate<GPUIProfile>(copyFrom);
			}
			gpuiprofile.name = (string.IsNullOrEmpty(name) ? "New GPUI Profile" : (name + " Profile"));
			gpuiprofile.isDefaultProfile = false;
			return gpuiprofile;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007530 File Offset: 0x00005730
		public void CopyValuesFrom(GPUIProfile copyFrom)
		{
			this.isShadowCasting = copyFrom.isShadowCasting;
			this.isDistanceCulling = copyFrom.isDistanceCulling;
			this.isFrustumCulling = copyFrom.isFrustumCulling;
			this.isOcclusionCulling = copyFrom.isOcclusionCulling;
			this.isShadowFrustumCulling = copyFrom.isShadowFrustumCulling;
			this.isShadowOcclusionCulling = copyFrom.isShadowOcclusionCulling;
			this.isShadowDistanceCulling = copyFrom.isShadowDistanceCulling;
			this.isLODCrossFade = copyFrom.isLODCrossFade;
			this.isAnimateCrossFade = copyFrom.isAnimateCrossFade;
			this.minCullingDistance = copyFrom.minCullingDistance;
			this.minShadowCullingDistance = copyFrom.minShadowCullingDistance;
			this.minMaxDistance = copyFrom.minMaxDistance;
			this.frustumOffset = copyFrom.frustumOffset;
			this.shadowFrustumOffset = copyFrom.shadowFrustumOffset;
			this.shadowOcclusionOffset = copyFrom.shadowOcclusionOffset;
			this.occlusionAccuracy = copyFrom.occlusionAccuracy;
			this.boundsOffset = copyFrom.boundsOffset;
			this.lodBiasAdjustment = copyFrom.lodBiasAdjustment;
			this.customShadowDistance = copyFrom.customShadowDistance;
			this.shadowLODMap = copyFrom.shadowLODMap;
			this.lodCrossFadeAnimateSpeed = copyFrom.lodCrossFadeAnimateSpeed;
			this.maximumLODLevel = copyFrom.maximumLODLevel;
			this.enablePerObjectMotionVectors = copyFrom.enablePerObjectMotionVectors;
			this.lightProbeSetting = copyFrom.lightProbeSetting;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007660 File Offset: 0x00005860
		public void SetParameterBufferData()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIDataBuffer<float> parameterBuffer = GPUIRenderingSystem.Instance.ParameterBuffer;
			int num;
			if (this.TryGetParameterBufferIndex(out num))
			{
				parameterBuffer[num] = this.minCullingDistance;
				parameterBuffer[num + 1] = this.minMaxDistance.x;
				parameterBuffer[num + 2] = this.GetMaxDistance();
				parameterBuffer[num + 3] = this.GetFrustumOffset();
				parameterBuffer[num + 4] = this.GetOcclusionOffset();
				parameterBuffer[num + 5] = (float)this.occlusionAccuracy;
				parameterBuffer[num + 6] = this.boundsOffset.x;
				parameterBuffer[num + 7] = this.boundsOffset.y;
				parameterBuffer[num + 8] = this.boundsOffset.z;
				parameterBuffer[num + 9] = this.GetLODBiasAdjustment();
				parameterBuffer[num + 10] = this.GetShadowDistance();
				for (int i = 0; i < 8; i++)
				{
					parameterBuffer[num + 11 + i] = this.shadowLODMap[i];
				}
				parameterBuffer[num + 20] = this.lodCrossFadeAnimateSpeed;
				parameterBuffer[num + 21] = this.GetShadowFrustumOffset();
				parameterBuffer[num + 22] = this.GetShadowOcclusionOffset();
				parameterBuffer[num + 23] = this.minShadowCullingDistance;
				parameterBuffer[num + 24] = this.occlusionOffsetSizeMultiplier;
				parameterBuffer[num + 25] = this.shadowOcclusionOffsetSizeMultiplier;
				return;
			}
			GPUIRenderingSystem.Instance.ParameterBufferIndexes.Add(this, parameterBuffer.Length);
			parameterBuffer.Add(new float[]
			{
				this.minCullingDistance,
				this.minMaxDistance.x,
				this.GetMaxDistance(),
				this.GetFrustumOffset(),
				this.GetOcclusionOffset(),
				(float)this.occlusionAccuracy,
				this.boundsOffset.x,
				this.boundsOffset.y,
				this.boundsOffset.z,
				this.GetLODBiasAdjustment(),
				this.GetShadowDistance()
			});
			parameterBuffer.Add(this.shadowLODMap);
			parameterBuffer.Add(new float[]
			{
				0f,
				this.lodCrossFadeAnimateSpeed,
				this.GetShadowFrustumOffset(),
				this.GetShadowOcclusionOffset(),
				this.minShadowCullingDistance,
				this.occlusionOffsetSizeMultiplier,
				this.shadowOcclusionOffsetSizeMultiplier
			});
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000359B File Offset: 0x0000179B
		public bool TryGetParameterBufferIndex(out int index)
		{
			return GPUIRenderingSystem.Instance.ParameterBufferIndexes.TryGetValue(this, out index);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000078B3 File Offset: 0x00005AB3
		public float GetMaxDistance()
		{
			if (this.isDistanceCulling)
			{
				return this.minMaxDistance.y;
			}
			return -1f;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000078CE File Offset: 0x00005ACE
		public float GetLODBiasAdjustment()
		{
			if (this.lodBiasAdjustment <= 0f)
			{
				return 1f;
			}
			return this.lodBiasAdjustment;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000078E9 File Offset: 0x00005AE9
		public float GetShadowDistance()
		{
			if (!this.isShadowDistanceCulling)
			{
				return 0f;
			}
			if (this.customShadowDistance > 0f)
			{
				return this.customShadowDistance;
			}
			return GPUIRuntimeSettings.Instance.GetDefaultShadowDistance();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007917 File Offset: 0x00005B17
		public float GetFrustumOffset()
		{
			if (!this.isFrustumCulling)
			{
				return -1f;
			}
			return this.frustumOffset;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000792D File Offset: 0x00005B2D
		public float GetOcclusionOffset()
		{
			if (!this.isOcclusionCulling)
			{
				return -1f;
			}
			return this.occlusionOffset;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007943 File Offset: 0x00005B43
		public float GetShadowFrustumOffset()
		{
			if (!this.isShadowFrustumCulling)
			{
				return -1f;
			}
			return this.shadowFrustumOffset;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007959 File Offset: 0x00005B59
		public float GetShadowOcclusionOffset()
		{
			if (!this.isShadowOcclusionCulling)
			{
				return -1f;
			}
			return this.shadowOcclusionOffset;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000796F File Offset: 0x00005B6F
		public void SetShadowFrustumCulling(bool value)
		{
			this.isShadowFrustumCulling = value;
			this.SetParameterBufferData();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000797E File Offset: 0x00005B7E
		public void SetShadowFrustumOffset(float value)
		{
			this.shadowFrustumOffset = value;
			this.SetParameterBufferData();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000798D File Offset: 0x00005B8D
		public void SetShadowOcclusionCulling(bool value)
		{
			this.isShadowOcclusionCulling = value;
			this.SetParameterBufferData();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000799C File Offset: 0x00005B9C
		public void SetShadowOcclusionOffset(float value)
		{
			this.shadowOcclusionOffset = value;
			this.SetParameterBufferData();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000079AB File Offset: 0x00005BAB
		public void SetShadowMinCullingDistance(float value)
		{
			this.minShadowCullingDistance = value;
			this.SetParameterBufferData();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000079BA File Offset: 0x00005BBA
		public void SetLightProbeSetting(int value)
		{
			if (value < 0 || value > 2)
			{
				return;
			}
			this.lightProbeSetting = (GPUILightProbeSetting)value;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000079CC File Offset: 0x00005BCC
		public bool HasLODLevelShadows(int lodLevel)
		{
			for (int i = 0; i < 8; i++)
			{
				if (this.shadowLODMap[i] == (float)lodLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000AA RID: 170
		public bool isShadowCasting = true;

		// Token: 0x040000AB RID: 171
		public bool isDistanceCulling = true;

		// Token: 0x040000AC RID: 172
		public bool isFrustumCulling = true;

		// Token: 0x040000AD RID: 173
		public bool isOcclusionCulling = true;

		// Token: 0x040000AE RID: 174
		public bool isShadowFrustumCulling;

		// Token: 0x040000AF RID: 175
		public bool isShadowOcclusionCulling;

		// Token: 0x040000B0 RID: 176
		public bool isShadowDistanceCulling = true;

		// Token: 0x040000B1 RID: 177
		public bool isLODCrossFade;

		// Token: 0x040000B2 RID: 178
		public bool isAnimateCrossFade = true;

		// Token: 0x040000B3 RID: 179
		public bool isOverrideShadowLayer;

		// Token: 0x040000B4 RID: 180
		public bool isCalculateInstancingBounds;

		// Token: 0x040000B5 RID: 181
		[Range(0f, 100f)]
		public float minCullingDistance;

		// Token: 0x040000B6 RID: 182
		[Range(0f, 100f)]
		public float minShadowCullingDistance = 20f;

		// Token: 0x040000B7 RID: 183
		public Vector2 minMaxDistance = new Vector2(0f, 500f);

		// Token: 0x040000B8 RID: 184
		[Range(0f, 10f)]
		public float frustumOffset = 0.01f;

		// Token: 0x040000B9 RID: 185
		[Range(0f, 0.01f)]
		public float occlusionOffset = 0.0001f;

		// Token: 0x040000BA RID: 186
		[Range(0f, 5f)]
		public float occlusionOffsetSizeMultiplier;

		// Token: 0x040000BB RID: 187
		[Range(0f, 100f)]
		public float shadowFrustumOffset = 10f;

		// Token: 0x040000BC RID: 188
		[Range(0f, 0.01f)]
		public float shadowOcclusionOffset = 0.001f;

		// Token: 0x040000BD RID: 189
		[Range(0f, 5f)]
		public float shadowOcclusionOffsetSizeMultiplier = 0.5f;

		// Token: 0x040000BE RID: 190
		[Range(1f, 3f)]
		public int occlusionAccuracy = 1;

		// Token: 0x040000BF RID: 191
		public Vector3 boundsOffset;

		// Token: 0x040000C0 RID: 192
		[Range(0.01f, 10f)]
		public float lodBiasAdjustment = 1f;

		// Token: 0x040000C1 RID: 193
		[Range(0f, 1000f)]
		public float customShadowDistance;

		// Token: 0x040000C2 RID: 194
		public float[] shadowLODMap = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f };

		// Token: 0x040000C3 RID: 195
		[Range(0.1f, 20f)]
		public float lodCrossFadeAnimateSpeed = 4f;

		// Token: 0x040000C4 RID: 196
		[Range(0f, 7f)]
		public int maximumLODLevel;

		// Token: 0x040000C5 RID: 197
		public int shadowLayerOverride;

		// Token: 0x040000C6 RID: 198
		public uint shadowRenderingLayerOverride;

		// Token: 0x040000C7 RID: 199
		public bool enablePerObjectMotionVectors;

		// Token: 0x040000C8 RID: 200
		public GPUILightProbeSetting lightProbeSetting = GPUILightProbeSetting.Single;

		// Token: 0x040000C9 RID: 201
		public Vector3 lightProbePositionOffset;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		[HideInInspector]
		public bool isDefaultProfile;

		// Token: 0x040000CB RID: 203
		private static GPUIProfile _defaultProfile;

		// Token: 0x040000CC RID: 204
		public static GPUIProfile defaultGPUSkinningProfile;

		// Token: 0x02000025 RID: 37
		public enum DepthSortMode
		{
			// Token: 0x040000CE RID: 206
			None,
			// Token: 0x040000CF RID: 207
			FrontToBack,
			// Token: 0x040000D0 RID: 208
			BackToFront
		}
	}
}
