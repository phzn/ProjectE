using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

namespace GPUInstancerPro
{
	// Token: 0x02000017 RID: 23
	public class GPUICameraData : IGPUIDisposable, IDisposable
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000037BB File Offset: 0x000019BB
		public Camera ActiveCamera
		{
			get
			{
				return this._camera;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000037C3 File Offset: 0x000019C3
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000037CB File Offset: 0x000019CB
		public GPUIOcclusionCullingData OcclusionCullingData { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000037D4 File Offset: 0x000019D4
		public RenderTexture HiZDepthTexture
		{
			get
			{
				if (this.OcclusionCullingData != null)
				{
					return this.OcclusionCullingData.HiZDepthTexture;
				}
				return null;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000037EB File Offset: 0x000019EB
		public Vector2Int HiZTextureSize
		{
			get
			{
				if (this.OcclusionCullingData != null)
				{
					return this.OcclusionCullingData.HiZTextureSize;
				}
				return Vector2Int.zero;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003806 File Offset: 0x00001A06
		public bool IsVRCulling
		{
			get
			{
				return this._isVRCulling;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003810 File Offset: 0x00001A10
		public GPUICameraData(Camera camera)
		{
			this._camera = camera;
			this._cachedTransform = this._camera.transform;
			this.name = camera.name;
			this._CS_CommandBufferUtility = GPUIConstants.CS_CommandBufferUtility;
			this._CS_CameraVisibility = GPUIConstants.CS_CameraVisibility;
			this._CS_OptionalRenderer = GPUIConstants.CS_OptionalRenderer;
			this._visibilityBuffer = new GPUIVisibilityBuffer(this, "Visibility", GraphicsBuffer.Target.Structured);
			this._visibilityBufferIndexes = new Dictionary<int, int>();
			this._commandBuffer = new GPUIDataBuffer<GraphicsBuffer.IndirectDrawIndexedArgs>("Command", 0, GraphicsBuffer.Target.IndirectArguments);
			this._sizeAndIndexes = new int[4];
			this._sizeAndIndexes2 = new int[4];
			this._hiZTextureSize = new int[4];
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000038C8 File Offset: 0x00001AC8
		public void ReleaseBuffers()
		{
			this.DisableOcclusionCulling();
			if (this._visibilityBuffer != null)
			{
				this._visibilityBuffer.ReleaseBuffers();
			}
			if (this._visibilityBufferIndexes != null)
			{
				this._visibilityBufferIndexes.Clear();
			}
			if (this._commandBuffer != null)
			{
				this._commandBuffer.ReleaseBuffers();
			}
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.RenderSourceGroupProvider.DisposeCameraData(this);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000392B File Offset: 0x00001B2B
		public void Dispose()
		{
			this.ReleaseBuffers();
			if (this.OcclusionCullingData != null)
			{
				this.OcclusionCullingData.Dispose();
				this.OcclusionCullingData = null;
			}
			this._isCullingInitialized = false;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003954 File Offset: 0x00001B54
		internal bool InitializeCulling()
		{
			GPUIRuntimeSettings.Instance.SetRuntimeSettings();
			this._instanceCountMultiplier = 1;
			this._isVRCulling = false;
			if (GPUIRuntimeSettings.Instance.IsVREnabled)
			{
				this._isVRCulling = this._camera.stereoTargetEye == StereoTargetEyeMask.Both && this.IsStereoEnabledForCamera(this._camera) && !GPUIRuntimeSettings.Instance.IsHDRP;
				if (this._isVRCulling)
				{
					this._CS_CameraVisibility = GPUIConstants.CS_CameraVisibilityXR;
					if (this._CS_CameraVisibility == null)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find XR visibility compute shader for camera. Make sure to import the XR support package by selecting Tools -> GPU Instancer Pro -> Reimport Packages or by manually importing the unity package under Packages/GPU Instancer Pro - Core/Editor/Extras/XR_Support_GPUIPro.", this._camera);
						return false;
					}
				}
			}
			this._isCullingInitialized = true;
			return true;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000039FC File Offset: 0x00001BFC
		private bool IsStereoEnabledForCamera(Camera cam)
		{
			if (GPUIRuntimeSettings.Instance.IsURP)
			{
				UniversalAdditionalCameraData universalAdditionalCameraData;
				if (!cam.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
				{
					return false;
				}
				if (!universalAdditionalCameraData.allowXRRendering)
				{
					return false;
				}
			}
			return cam.stereoEnabled;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003A34 File Offset: 0x00001C34
		private void InitializeOcclusionCulling()
		{
			if (!this._isCullingInitialized && !this.InitializeCulling())
			{
				return;
			}
			this.autoInitializeOcclusionCulling = false;
			if (GPUIRuntimeSettings.Instance.DisableOcclusionCulling || GPUIRuntimeSettings.Instance.occlusionCullingCondition == GPUIOcclusionCullingCondition.Never)
			{
				return;
			}
			if (GPUIRuntimeSettings.Instance.occlusionCullingCondition == GPUIOcclusionCullingCondition.IfDepthAvailable && !this.ActiveCamera.IsDepthTextureAvailable())
			{
				return;
			}
			if (GPUIRuntimeSettings.Instance.IsVREnabled && GPUIRuntimeSettings.Instance.IsHDRP)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "GPU Instancer Pro currently does not support Occlusion Culling for VR devices on HDRP. Occlusion culling will be disabled.");
				return;
			}
			if (this.OcclusionCullingData == null)
			{
				this.OcclusionCullingData = new GPUIOcclusionCullingData(this.ActiveCamera, GPUIRuntimeSettings.Instance.occlusionCullingMode, this._isVRCulling);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public void DisableOcclusionCulling()
		{
			this.autoInitializeOcclusionCulling = false;
			if (this.OcclusionCullingData != null)
			{
				this.OcclusionCullingData.Dispose();
				this.OcclusionCullingData = null;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003B08 File Offset: 0x00001D08
		internal void UpdateHiZTexture(ScriptableRenderContext context)
		{
			if (this.OcclusionCullingData == null)
			{
				return;
			}
			this.OcclusionCullingData.UpdateHiZTexture(context);
			this._hiZTextureSize[0] = this.OcclusionCullingData.HiZTextureSize.x;
			this._hiZTextureSize[1] = this.OcclusionCullingData.HiZTextureSize.y;
			if (this.OcclusionCullingData.HiZDepthTexture != null)
			{
				this._hiZTextureSize[2] = this.OcclusionCullingData.HiZDepthTexture.width;
				this._hiZTextureSize[3] = this.OcclusionCullingData.HiZDepthTexture.height;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003BA3 File Offset: 0x00001DA3
		internal void UpdateHiZTextureOnBeginRendering(Camera camera, ScriptableRenderContext context)
		{
			if (this.OcclusionCullingData == null)
			{
				return;
			}
			this.OcclusionCullingData.UpdateHiZTextureOnBeginRendering(camera, context);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003BBC File Offset: 0x00001DBC
		public virtual void UpdateCameraData()
		{
			if (!this._isCullingInitialized && !this.InitializeCulling())
			{
				return;
			}
			if (this.autoInitializeOcclusionCulling)
			{
				this.InitializeOcclusionCulling();
			}
			this._commandBuffer.UpdateBufferData(false);
			Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
			if (GPUIRuntimeSettings.Instance.IsVREnabled)
			{
				if (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
				{
					this._instanceCountMultiplier = 2;
				}
				else
				{
					this._instanceCountMultiplier = 1;
				}
				if (this._camera.stereoEnabled)
				{
					if (this._isVRCulling)
					{
						this._mvpMatrix = this._camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * worldToCameraMatrix;
						this._mvpMatrix2 = this._camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * worldToCameraMatrix;
					}
					else if (this._camera.stereoTargetEye == StereoTargetEyeMask.Left)
					{
						this._mvpMatrix = this._camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * worldToCameraMatrix;
					}
					else if (this._camera.stereoTargetEye == StereoTargetEyeMask.Right)
					{
						this._mvpMatrix = this._camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * worldToCameraMatrix;
					}
					else
					{
						this._mvpMatrix = this._camera.projectionMatrix * worldToCameraMatrix;
					}
				}
				else
				{
					this._mvpMatrix = this._camera.projectionMatrix * worldToCameraMatrix;
				}
			}
			else
			{
				this._mvpMatrix = this._camera.projectionMatrix * worldToCameraMatrix;
			}
			Vector3 position = this._cachedTransform.position;
			if (this.OcclusionCullingData != null)
			{
				if (this._dynamicOcclusionOffsetIntensity > 0f)
				{
					Quaternion rotation = this._cachedTransform.rotation;
					this._additionalValues.y = MathF.Max(Vector3.Distance(position, this._cameraPositionAndHalfAngle) * 0.01f, (1f - Mathf.Abs(Quaternion.Dot(rotation, this._cameraRotation))) * 100f) * this._dynamicOcclusionOffsetIntensity;
					this._cameraRotation = rotation;
				}
				else
				{
					this._additionalValues.y = 0f;
				}
				this.OcclusionCullingData.CheckScreenSize();
			}
			this._cameraPositionAndHalfAngle.x = position.x;
			this._cameraPositionAndHalfAngle.y = position.y;
			this._cameraPositionAndHalfAngle.z = position.z;
			this._cameraPositionAndHalfAngle.w = Mathf.Tan(0.017453292f * this._camera.fieldOfView * 0.25f);
			this.MakeVisibilityCalculations();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003E10 File Offset: 0x00002010
		protected void MakeVisibilityCalculations()
		{
			bool flag = this._visibilityBuffer.UpdateBufferData(false);
			if (this._visibilityBuffer.Buffer == null)
			{
				return;
			}
			if (GPUIRenderingSystem.Instance.ParameterBuffer.Buffer == null)
			{
				return;
			}
			int frameCount = Time.frameCount;
			if (!flag)
			{
				if (this._visibilityBuffer.IsDataRequested())
				{
					this._visibilityBuffer.WaitForReadbackCompletion();
				}
				this._CS_CommandBufferUtility.SetBuffer<GPUIVisibilityData>(0, GPUIConstants.PROP_visibilityBuffer, this._visibilityBuffer);
				this._CS_CommandBufferUtility.SetInt(GPUIConstants.PROP_bufferSize, this._visibilityBuffer.Length);
				this._CS_CommandBufferUtility.DispatchX(0, this._visibilityBuffer.Length);
			}
			this._CS_CameraVisibility.SetMatrix(GPUIConstants.PROP_mvpMatrix, this._mvpMatrix);
			this._CS_CameraVisibility.SetVector(GPUIConstants.PROP_cameraPositionAndHalfAngle, this._cameraPositionAndHalfAngle);
			this._CS_CameraVisibility.SetBuffer<float>(0, GPUIConstants.PROP_parameterBuffer, GPUIRenderingSystem.Instance.ParameterBuffer);
			this._CS_CameraVisibility.SetBuffer<GPUIVisibilityData>(0, GPUIConstants.PROP_visibilityBuffer, this._visibilityBuffer);
			bool flag2 = this.OcclusionCullingData != null && this.OcclusionCullingData.IsHiZDepthUpdated && this._hiZTextureSize[2] > 0 && this._hiZTextureSize[3] > 0;
			int maximumLODLevel = QualitySettings.maximumLODLevel;
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				GPUILODGroupData lodgroupData = gpuirenderSourceGroup.LODGroupData;
				if (gpuirenderSourceGroup.BufferSize > 0 && gpuirenderSourceGroup.InstanceCount > 0 && !(lodgroupData == null) && this._visibilityBufferIndexes.TryGetValue(gpuirenderSourceGroup.Key, out this._sizeAndIndexes[1]) && gpuirenderSourceGroup.Profile.TryGetParameterBufferIndex(out this._sizeAndIndexes[2]) && lodgroupData.TryGetParameterBufferIndex(out this._sizeAndIndexes[3]))
				{
					bool flag3 = lodgroupData.Length > 1 || lodgroupData.transitionValues[0] > 0f;
					this._sizeAndIndexes[0] = gpuirenderSourceGroup.BufferSize;
					GPUITransformBufferData transformBufferData = gpuirenderSourceGroup.TransformBufferData;
					transformBufferData.ApplyTransformDataUpdates();
					GPUIShaderBuffer transformBuffer = transformBufferData.GetTransformBuffer(this);
					if (transformBuffer.Buffer != null)
					{
						this._additionalValues.z = (float)Mathf.Max(maximumLODLevel, gpuirenderSourceGroup.Profile.maximumLODLevel);
						GPUIShaderBuffer instanceDataBuffer = transformBufferData.GetInstanceDataBuffer(this);
						this._CS_CameraVisibility.SetBuffer(0, GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer.Buffer);
						instanceDataBuffer.SetBuffer(this._CS_CameraVisibility, 0, GPUIConstants.PROP_gpuiInstanceDataBuffer);
						if (flag3)
						{
							if (gpuirenderSourceGroup.Profile.isLODCrossFade)
							{
								if (gpuirenderSourceGroup.Profile.isAnimateCrossFade && !transformBufferData.IsCameraBasedBuffer)
								{
									this._CS_CameraVisibility.DisableKeyword("GPUI_LOD");
									this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE");
									this._CS_CameraVisibility.EnableKeyword("GPUI_LOD_CROSSFADE_ANIMATE");
									this._additionalValues.x = ((transformBufferData.resetCrossFadeDataFrame == frameCount) ? 0f : GPUIRenderingSystem.Instance.TimeSinceLastDrawCall);
								}
								else
								{
									this._CS_CameraVisibility.DisableKeyword("GPUI_LOD");
									this._CS_CameraVisibility.EnableKeyword("GPUI_LOD_CROSSFADE");
									this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE_ANIMATE");
								}
							}
							else
							{
								this._CS_CameraVisibility.EnableKeyword("GPUI_LOD");
								this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE");
								this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE_ANIMATE");
							}
						}
						else
						{
							this._CS_CameraVisibility.DisableKeyword("GPUI_LOD");
							this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE");
							this._CS_CameraVisibility.DisableKeyword("GPUI_LOD_CROSSFADE_ANIMATE");
						}
						if (gpuirenderSourceGroup.Profile.isShadowCasting)
						{
							if (gpuirenderSourceGroup.Profile.isShadowFrustumCulling || gpuirenderSourceGroup.Profile.isShadowOcclusionCulling)
							{
								this._CS_CameraVisibility.DisableKeyword("GPUI_SHADOWCASTING");
								this._CS_CameraVisibility.EnableKeyword("GPUI_SHADOWCULLED");
							}
							else
							{
								this._CS_CameraVisibility.EnableKeyword("GPUI_SHADOWCASTING");
								this._CS_CameraVisibility.DisableKeyword("GPUI_SHADOWCULLED");
							}
						}
						else
						{
							this._CS_CameraVisibility.DisableKeyword("GPUI_SHADOWCASTING");
							this._CS_CameraVisibility.DisableKeyword("GPUI_SHADOWCULLED");
						}
						if (gpuirenderSourceGroup.Profile.isOcclusionCulling && flag2)
						{
							this._CS_CameraVisibility.EnableKeyword("GPUI_OCCLUSION_CULLING");
							this._CS_CameraVisibility.SetTexture(0, GPUIConstants.PROP_hiZMap, this.OcclusionCullingData.HiZDepthTexture);
							this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_hiZTxtrSize, this._hiZTextureSize);
						}
						else
						{
							this._CS_CameraVisibility.DisableKeyword("GPUI_OCCLUSION_CULLING");
						}
						if (this._isVRCulling)
						{
							this._CS_CameraVisibility.SetMatrix(GPUIConstants.PROP_mvpMatrix2, this._mvpMatrix2);
						}
						this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_sizeAndIndexes, this._sizeAndIndexes);
						this._CS_CameraVisibility.SetVector(GPUIConstants.PROP_additionalValues, this._additionalValues);
						if (transformBufferData.IsCameraBasedBuffer)
						{
							this._sizeAndIndexes2[0] = gpuirenderSourceGroup.BufferSize;
							this._sizeAndIndexes2[1] = 0;
							this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_sizeAndIndexes2, this._sizeAndIndexes2);
							this._CS_CameraVisibility.DispatchXHeavy(0, gpuirenderSourceGroup.BufferSize);
						}
						else
						{
							int count = gpuirenderSourceGroup.RenderSources.Count;
							int num = 0;
							int num2 = 0;
							int num3 = 0;
							bool flag4 = false;
							for (int i = 0; i < count; i++)
							{
								GPUIRenderSource gpuirenderSource = gpuirenderSourceGroup.RenderSources[i];
								int instanceCount = gpuirenderSource.instanceCount;
								int bufferStartIndex = gpuirenderSource.bufferStartIndex;
								if (instanceCount > 0 && bufferStartIndex >= 0)
								{
									bool flag5 = instanceCount == gpuirenderSource.bufferSize;
									if (!flag4)
									{
										num = bufferStartIndex;
										num2 = instanceCount;
										num3 = bufferStartIndex + instanceCount;
										flag4 = true;
									}
									else if (bufferStartIndex == num3)
									{
										num2 += instanceCount;
										num3 += instanceCount;
									}
									else
									{
										this._sizeAndIndexes2[0] = num2;
										this._sizeAndIndexes2[1] = num;
										this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_sizeAndIndexes2, this._sizeAndIndexes2);
										this._CS_CameraVisibility.DispatchXHeavy(0, num2);
										num = bufferStartIndex;
										num2 = instanceCount;
										num3 = bufferStartIndex + instanceCount;
										flag4 = true;
									}
									if (!flag5)
									{
										this._sizeAndIndexes2[0] = num2;
										this._sizeAndIndexes2[1] = num;
										this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_sizeAndIndexes2, this._sizeAndIndexes2);
										this._CS_CameraVisibility.DispatchXHeavy(0, num2);
										flag4 = false;
										num2 = 0;
									}
								}
							}
							if (flag4 && num2 > 0)
							{
								this._sizeAndIndexes2[0] = num2;
								this._sizeAndIndexes2[1] = num;
								this._CS_CameraVisibility.SetInts(GPUIConstants.PROP_sizeAndIndexes2, this._sizeAndIndexes2);
								this._CS_CameraVisibility.DispatchXHeavy(0, num2);
							}
						}
						this.SetOptionalRendererInstanceData(gpuirenderSourceGroup, lodgroupData, instanceDataBuffer);
						instanceDataBuffer.OnDataModified();
					}
				}
			}
			this.SetCommandBufferInstanceCounts(this._instanceCountMultiplier);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004504 File Offset: 0x00002704
		private void SetOptionalRendererInstanceData(GPUIRenderSourceGroup rsg, GPUILODGroupData lodGroupData, GPUIShaderBuffer instanceDataBuffer)
		{
			if (lodGroupData.optionalRendererCount <= 0)
			{
				return;
			}
			GraphicsBuffer optionalRendererStatusBuffer = rsg.TransformBufferData.GetOptionalRendererStatusBuffer();
			if (optionalRendererStatusBuffer != null)
			{
				int num = 0;
				this._sizeAndIndexes[2] = lodGroupData.optionalRendererCount;
				this._CS_OptionalRenderer.SetInts(GPUIConstants.PROP_sizeAndIndexes, this._sizeAndIndexes);
				if (rsg.Profile.isShadowCasting)
				{
					this._CS_OptionalRenderer.EnableKeyword("GPUI_SHADOWCASTING");
				}
				else
				{
					this._CS_OptionalRenderer.DisableKeyword("GPUI_SHADOWCASTING");
				}
				this._CS_OptionalRenderer.SetBuffer<GPUIVisibilityData>(num, GPUIConstants.PROP_visibilityBuffer, this._visibilityBuffer);
				instanceDataBuffer.SetBuffer(this._CS_OptionalRenderer, num, GPUIConstants.PROP_gpuiInstanceDataBuffer);
				this._CS_OptionalRenderer.SetBuffer(num, GPUIConstants.PROP_optionalRendererStatusBuffer, optionalRendererStatusBuffer);
				this._CS_OptionalRenderer.DispatchXY(num, this._sizeAndIndexes[0], lodGroupData.optionalRendererCount);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000045D8 File Offset: 0x000027D8
		internal void SetCommandBufferInstanceCounts(int instanceCountMultiplier)
		{
			if (this._commandBuffer.Length == 0)
			{
				return;
			}
			this._CS_CommandBufferUtility.SetBuffer<GPUIVisibilityData>(1, GPUIConstants.PROP_visibilityBuffer, this._visibilityBuffer);
			this._CS_CommandBufferUtility.SetBuffer<GraphicsBuffer.IndirectDrawIndexedArgs>(1, GPUIConstants.PROP_commandBuffer, this._commandBuffer);
			this._CS_CommandBufferUtility.SetInt(GPUIConstants.PROP_bufferSize, this._visibilityBuffer.Length);
			this._CS_CommandBufferUtility.SetInt(GPUIConstants.PROP_multiplier, instanceCountMultiplier);
			this._CS_CommandBufferUtility.DispatchX(1, this._visibilityBuffer.Length);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004664 File Offset: 0x00002864
		internal void ClearVisibilityData()
		{
			this._visibilityBuffer.ReleaseBuffers();
			this._visibilityBufferIndexes.Clear();
			this._commandBuffer.ReleaseBuffers();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004687 File Offset: 0x00002887
		public GPUIVisibilityData[] GetVisibilityDataArray()
		{
			return this._visibilityBuffer.GetBufferData();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004694 File Offset: 0x00002894
		public bool TryGetVisibilityBufferIndex(GPUIRenderSourceGroup renderSourceGroup, out int visibilityBufferIndex)
		{
			return this.TryGetVisibilityBufferIndex(renderSourceGroup.Key, out visibilityBufferIndex);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000046A3 File Offset: 0x000028A3
		public bool TryGetVisibilityBufferIndex(int renderSourceGroupKey, out int visibilityBufferIndex)
		{
			visibilityBufferIndex = -1;
			return this._visibilityBuffer.Length != 0 && this._visibilityBufferIndexes.TryGetValue(renderSourceGroupKey, out visibilityBufferIndex);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000046C9 File Offset: 0x000028C9
		public bool TryGetShaderBuffer(GPUIManager manager, int prototypeIndex, out GPUIShaderBuffer shaderBuffer)
		{
			if (manager == null)
			{
				shaderBuffer = null;
				return false;
			}
			return this.TryGetShaderBuffer(manager.GetRenderKey(prototypeIndex), out shaderBuffer);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046E8 File Offset: 0x000028E8
		public bool TryGetShaderBuffer(int renderKey, out GPUIShaderBuffer shaderBuffer)
		{
			shaderBuffer = null;
			if (renderKey == 0)
			{
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (!GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				return false;
			}
			GPUIRenderSourceGroup renderSourceGroup = gpuirenderSource.renderSourceGroup;
			if (renderSourceGroup != null && renderSourceGroup.TransformBufferData != null)
			{
				shaderBuffer = renderSourceGroup.TransformBufferData.GetTransformBuffer(this);
				if (shaderBuffer != null && shaderBuffer.BufferSize > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004744 File Offset: 0x00002944
		public GPUIDataBuffer<GPUIVisibilityData> GetVisibilityBuffer()
		{
			this._visibilityBuffer.UpdateBufferData(false);
			return this._visibilityBuffer;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004759 File Offset: 0x00002959
		public Vector3 GetCameraPosition()
		{
			return this._cameraPositionAndHalfAngle;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004766 File Offset: 0x00002966
		public GraphicsBuffer.IndirectDrawIndexedArgs GetCommandDataAtIndex(int index)
		{
			return this._commandBuffer[index];
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004774 File Offset: 0x00002974
		public int GetCommandDataLength()
		{
			return this._commandBuffer.Length;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004781 File Offset: 0x00002981
		public void SetDynamicOcclusionOffsetIntensity(float intensity)
		{
			this._dynamicOcclusionOffsetIntensity = intensity;
		}

		// Token: 0x04000044 RID: 68
		private Camera _camera;

		// Token: 0x04000045 RID: 69
		private bool _isCullingInitialized;

		// Token: 0x04000046 RID: 70
		internal GPUIVisibilityBuffer _visibilityBuffer;

		// Token: 0x04000047 RID: 71
		internal GPUIDataBuffer<GraphicsBuffer.IndirectDrawIndexedArgs> _commandBuffer;

		// Token: 0x04000048 RID: 72
		internal Dictionary<int, int> _visibilityBufferIndexes;

		// Token: 0x04000049 RID: 73
		protected Transform _cachedTransform;

		// Token: 0x0400004A RID: 74
		protected ComputeShader _CS_CommandBufferUtility;

		// Token: 0x0400004B RID: 75
		protected ComputeShader _CS_CameraVisibility;

		// Token: 0x0400004C RID: 76
		protected ComputeShader _CS_OptionalRenderer;

		// Token: 0x0400004D RID: 77
		private Matrix4x4 _mvpMatrix;

		// Token: 0x0400004E RID: 78
		private Vector4 _cameraPositionAndHalfAngle;

		// Token: 0x0400004F RID: 79
		private Vector4 _additionalValues;

		// Token: 0x04000050 RID: 80
		private Quaternion _cameraRotation;

		// Token: 0x04000051 RID: 81
		private int[] _sizeAndIndexes;

		// Token: 0x04000052 RID: 82
		private int[] _sizeAndIndexes2;

		// Token: 0x04000053 RID: 83
		public string name;

		// Token: 0x04000054 RID: 84
		internal int _instanceCountMultiplier = 1;

		// Token: 0x04000055 RID: 85
		public bool autoInitializeOcclusionCulling;

		// Token: 0x04000057 RID: 87
		private float _dynamicOcclusionOffsetIntensity;

		// Token: 0x04000058 RID: 88
		private int[] _hiZTextureSize;

		// Token: 0x04000059 RID: 89
		private Matrix4x4 _mvpMatrix2;

		// Token: 0x0400005A RID: 90
		private bool _isVRCulling;
	}
}
