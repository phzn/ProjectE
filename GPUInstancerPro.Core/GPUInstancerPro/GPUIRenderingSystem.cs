using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace GPUInstancerPro
{
	// Token: 0x02000053 RID: 83
	[ExecuteInEditMode]
	[DefaultExecutionOrder(1000)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Debugger_Window")]
	public sealed class GPUIRenderingSystem : MonoBehaviour, IGPUIDisposable, IDisposable
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000F485 File Offset: 0x0000D685
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000F48C File Offset: 0x0000D68C
		public static GPUIRenderingSystem Instance { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000F494 File Offset: 0x0000D694
		public static bool IsActive
		{
			get
			{
				return GPUIRenderingSystem.Instance != null && GPUIRenderingSystem.Instance.IsInitialized;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000F4AF File Offset: 0x0000D6AF
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000F4B7 File Offset: 0x0000D6B7
		public bool IsInitialized { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
		public GPUIMaterialProvider MaterialProvider { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000F4D1 File Offset: 0x0000D6D1
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000F4D9 File Offset: 0x0000D6D9
		public GPUILODGroupDataProvider LODGroupDataProvider { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000F4E2 File Offset: 0x0000D6E2
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000F4EA File Offset: 0x0000D6EA
		public GPUIRenderSourceGroupProvider RenderSourceGroupProvider { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000F4FB File Offset: 0x0000D6FB
		public GPUIRenderSourceProvider RenderSourceProvider { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000F504 File Offset: 0x0000D704
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000F50C File Offset: 0x0000D70C
		public GPUICameraDataProvider CameraDataProvider { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000F515 File Offset: 0x0000D715
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000F51D File Offset: 0x0000D71D
		public GPUITreeProxyProvider TreeProxyProvider { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000F526 File Offset: 0x0000D726
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000F52E File Offset: 0x0000D72E
		public GPUIDataBuffer<float> ParameterBuffer { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000F537 File Offset: 0x0000D737
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000F53F File Offset: 0x0000D73F
		public Dictionary<IGPUIParameterBufferData, int> ParameterBufferIndexes { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000F548 File Offset: 0x0000D748
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000F550 File Offset: 0x0000D750
		public List<GPUIManager> ActiveGPUIManagers { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000F559 File Offset: 0x0000D759
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000F561 File Offset: 0x0000D761
		public float TimeSinceLastDrawCall { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000F56A File Offset: 0x0000D76A
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000F572 File Offset: 0x0000D772
		public bool IsPaused { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000F57B File Offset: 0x0000D77B
		public static MaterialPropertyBlock EmptyMPB
		{
			get
			{
				if (GPUIRenderingSystem._emptyMPB == null)
				{
					GPUIRenderingSystem._emptyMPB = new MaterialPropertyBlock();
				}
				return GPUIRenderingSystem._emptyMPB;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000F593 File Offset: 0x0000D793
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000F59B File Offset: 0x0000D79B
		public GraphicsBuffer DummyGraphicsBuffer { get; private set; }

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		private void Awake()
		{
			if (GPUIRenderingSystem.Instance != null && GPUIRenderingSystem.Instance != this)
			{
				this.DestroyInstance();
				return;
			}
			if (GPUIRenderingSystem.Instance == null)
			{
				GPUIRenderingSystem.Instance = this;
				this.Initialize();
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
		private void OnEnable()
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				GPUIRenderingSystem.Instance = this;
			}
			if (this.CheckIsSingleton())
			{
				this.Initialize();
				this.UpdateCommandBuffers(false);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F60A File Offset: 0x0000D80A
		private void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F614 File Offset: 0x0000D814
		private void LateUpdate()
		{
			if (this._hasTreeCreatorWind)
			{
				this.SetWindZoneValues();
			}
			if (this._requireInstancingBoundsDataRead && !this._instancingBoundsMinMaxBuffer.IsDataRequested())
			{
				this._instancingBoundsMinMaxBuffer.AsyncDataRequest(this._calculateInstancingBoundsCallback, true);
				this._requireInstancingBoundsDataRead = false;
			}
			GPUIRenderingSystem.Threading_Update();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F664 File Offset: 0x0000D864
		private static void CameraOnPreCull(Camera camera)
		{
			GPUIRenderingSystem.Threading_Update();
			if (GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Count == 0)
			{
				return;
			}
			GPUIRenderingSystem.ProcessCamera(camera);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F684 File Offset: 0x0000D884
		private static void CameraOnPostRender(Camera camera)
		{
			GPUICameraData gpuicameraData;
			if (GPUIRenderingSystem.Instance.CameraDataProvider.TryGetData(camera.GetInstanceIDGPUI(), out gpuicameraData))
			{
				gpuicameraData.UpdateHiZTexture(default(ScriptableRenderContext));
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F6B9 File Offset: 0x0000D8B9
		private static void CameraOnBeginRendering(ScriptableRenderContext context, Camera camera)
		{
			GPUIRenderingSystem.CameraOnPreCull(camera);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		private static void CameraOnEndRendering(ScriptableRenderContext context, Camera camera)
		{
			GPUICameraData gpuicameraData;
			if (GPUIRenderingSystem.Instance.CameraDataProvider.TryGetData(camera.GetInstanceIDGPUI(), out gpuicameraData))
			{
				gpuicameraData.UpdateHiZTexture(context);
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		private static void OnEndContextRendering(ScriptableRenderContext context, List<Camera> list)
		{
			int frameCount = Time.frameCount;
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				gpuirenderSourceGroup.UpdateTransformBufferData(frameCount);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F754 File Offset: 0x0000D954
		private static void ProcessCamera(Camera camera)
		{
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.ParameterBuffer.UpdateBufferData(false);
			instance.ExecuteLightProbeUpdates();
			CameraType cameraType = camera.cameraType;
			int instanceIDGPUI = camera.GetInstanceIDGPUI();
			GPUICameraDataProvider cameraDataProvider = instance.CameraDataProvider;
			if (cameraDataProvider.Count == 0)
			{
				cameraDataProvider.RegisterDefaultCamera();
			}
			GPUICameraData gpuicameraData;
			bool flag = cameraDataProvider.TryGetData(instanceIDGPUI, out gpuicameraData);
			if (!flag && !instance._ignoreCameraIIDCollection.Contains(instanceIDGPUI))
			{
				if (cameraType == CameraType.Reflection)
				{
					gpuicameraData = new GPUICameraData(camera);
					cameraDataProvider.AddCameraData(gpuicameraData);
					flag = true;
				}
				else if (GPUIRuntimeSettings.Instance.cameraLoadingType != GPUICameraLoadingType.GPUICameraComponent && camera.CompareTag("MainCamera"))
				{
					gpuicameraData = cameraDataProvider.AddCamera(camera);
					flag = gpuicameraData != null;
				}
				if (flag)
				{
					instance.UpdateCommandBuffers(gpuicameraData, false);
				}
				else
				{
					instance._ignoreCameraIIDCollection.Add(instanceIDGPUI);
				}
			}
			if (!flag)
			{
				return;
			}
			if (gpuicameraData.IsVRCulling && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass && gpuicameraData.ActiveCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
			{
				return;
			}
			instance.ExecuteOnPreCull(gpuicameraData);
			gpuicameraData.UpdateCameraData();
			GPUIRenderingSystem.ProcessCameraData(camera, gpuicameraData, true);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F858 File Offset: 0x0000DA58
		private static void ProcessCameraData(Camera camera, GPUICameraData cameraData, bool invokeEvents)
		{
			if (cameraData._commandBuffer.Buffer == null)
			{
				return;
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			Vector3 vector = cameraData.GetCameraPosition();
			if (invokeEvents)
			{
				instance.ExecuteOnPreRender(cameraData);
				if (Application.isPlaying)
				{
					instance.TreeProxyProvider.SetTreeProxyPosition(cameraData.GetCameraPosition());
				}
			}
			else
			{
				vector = camera.transform.position;
			}
			instance._worldBounds.center = vector;
			instance._worldBounds.size = GPUIRuntimeSettings.Instance.instancingBoundsSize;
			instance._renderParams.camera = camera;
			instance.MakeDrawCalls(cameraData);
			if (invokeEvents)
			{
				instance.ExecuteOnPostRender(cameraData);
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		private void MakeDrawCalls(GPUICameraData cameraData)
		{
			if (cameraData.ActiveCamera == null)
			{
				return;
			}
			if (this._lastDrawCallFrame != Time.frameCount)
			{
				this._lastDrawCallFrame = Time.frameCount;
				this.TimeSinceLastDrawCall = Time.realtimeSinceStartup - this._lastDrawCallTime;
				this._lastDrawCallTime = Time.realtimeSinceStartup;
			}
			int cullingMask = cameraData.ActiveCamera.cullingMask;
			int maximumLODLevel = QualitySettings.maximumLODLevel;
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in this.RenderSourceGroupProvider.Values)
			{
				GPUILODGroupData lodgroupData = gpuirenderSourceGroup.LODGroupData;
				int num;
				if (gpuirenderSourceGroup.BufferSize > 0 && gpuirenderSourceGroup.InstanceCount > 0 && lodgroupData != null && cameraData.TryGetVisibilityBufferIndex(gpuirenderSourceGroup, out num))
				{
					GPUITransformBufferData transformBufferData = gpuirenderSourceGroup.TransformBufferData;
					MaterialPropertyBlock materialPropertyBlock = gpuirenderSourceGroup.GetMaterialPropertyBlock(lodgroupData);
					gpuirenderSourceGroup.ApplyMaterialPropertyOverrides(materialPropertyBlock, -1, -1);
					transformBufferData.SetMPBBuffers(materialPropertyBlock, cameraData);
					materialPropertyBlock.SetInt(GPUIConstants.PROP_rsgCommandStartIndex, (int)cameraData._visibilityBuffer[num].commandStartIndex);
					this._renderParams.matProps = materialPropertyBlock;
					GPUIProfile profile = gpuirenderSourceGroup.Profile;
					bool isShadowCasting = profile.isShadowCasting;
					bool flag = profile.lightProbeSetting > GPUILightProbeSetting.Off;
					if (profile.isCalculateInstancingBounds && transformBufferData.HasInstancingBounds)
					{
						this._renderParams.worldBounds = transformBufferData._instancingBounds;
					}
					else
					{
						this._renderParams.worldBounds = this._worldBounds;
					}
					int length = lodgroupData.Length;
					int maximumLODLevel2 = this.GetMaximumLODLevel(length, profile.maximumLODLevel, maximumLODLevel);
					for (int i = 0; i < length; i++)
					{
						this.RenderLOD(i, cameraData, cullingMask, gpuirenderSourceGroup, lodgroupData, num, materialPropertyBlock, isShadowCasting, length, maximumLODLevel2, 0, flag);
					}
					if (length == 1 && lodgroupData.optionalRendererCount > 0)
					{
						GPUILODData gpuiloddata = lodgroupData[0];
						for (int j = 0; j < lodgroupData.optionalRendererCount; j++)
						{
							this.RenderLOD(0, cameraData, cullingMask, gpuirenderSourceGroup, lodgroupData, num, materialPropertyBlock, isShadowCasting, length, maximumLODLevel2, j + 1, flag);
						}
					}
				}
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000FB1C File Offset: 0x0000DD1C
		private void RenderLOD(int lodNo, GPUICameraData cameraData, int cullingMask, GPUIRenderSourceGroup renderSourceGroup, GPUILODGroupData lodGroupData, int visibilityBufferIndex, MaterialPropertyBlock mpb, bool isProfileShadowCasting, int lodCount, int maximumLODLevel, int optionalRendererNo, bool isProfileAllowLightProbes)
		{
			GPUIProfile profile = renderSourceGroup.Profile;
			if (isProfileShadowCasting && !profile.HasLODLevelShadows(lodNo))
			{
				isProfileShadowCasting = false;
			}
			bool isOverrideShadowLayer = profile.isOverrideShadowLayer;
			int shadowLayerOverride = profile.shadowLayerOverride;
			uint shadowRenderingLayerOverride = profile.shadowRenderingLayerOverride;
			bool flag = true;
			if (isProfileShadowCasting && isOverrideShadowLayer)
			{
				flag = GPUIUtility.IsInLayer(cullingMask, shadowLayerOverride);
			}
			int num = visibilityBufferIndex + lodNo + optionalRendererNo * 2;
			int num2 = (int)cameraData._visibilityBuffer[num].commandStartIndex;
			int num3 = (int)cameraData._visibilityBuffer[num + lodCount].commandStartIndex;
			renderSourceGroup.ApplyMaterialPropertyOverrides(mpb, lodNo, -1);
			GPUILODData gpuiloddata = lodGroupData[lodNo];
			for (int i = 0; i < gpuiloddata.Length; i++)
			{
				GPUIRendererData gpuirendererData = gpuiloddata[i];
				if (gpuirendererData.optionalRendererNo == optionalRendererNo)
				{
					Mesh mesh = gpuirendererData.GetMesh();
					if (mesh != null && GPUIUtility.IsInLayer(cullingMask, gpuirendererData.layer) && lodNo >= maximumLODLevel)
					{
						this._renderParams.receiveShadows = gpuirendererData.receiveShadows;
						this._renderParams.lightProbeUsage = (isProfileAllowLightProbes ? gpuirendererData.lightProbeUsage : LightProbeUsage.Off);
						renderSourceGroup.ApplyMaterialPropertyOverrides(mpb, lodNo, i);
						this._renderParams.layer = gpuirendererData.layer;
						if (gpuirendererData.motionVectorGenerationMode == MotionVectorGenerationMode.Object && !renderSourceGroup.TransformBufferData.HasPreviousFrameTransformBuffer)
						{
							this._renderParams.motionVectorMode = MotionVectorGenerationMode.Camera;
						}
						else
						{
							this._renderParams.motionVectorMode = gpuirendererData.motionVectorGenerationMode;
						}
						this._renderParams.renderingLayerMask = gpuirendererData.renderingLayerMask;
						if (Application.isPlaying && gpuirendererData.replacementMaterials == null)
						{
							gpuirendererData.InitializeReplacementMaterials(this.MaterialProvider);
						}
						for (int j = 0; j < gpuirendererData.rendererMaterials.Length; j++)
						{
							this._renderParams.material = this.GetReplacementMaterial(gpuirendererData, j, renderSourceGroup.ShaderKeywords);
							if (!gpuirendererData.IsShadowsOnly)
							{
								this._renderParams.shadowCastingMode = ShadowCastingMode.Off;
								GPUIUtility.RenderMeshIndirect(in this._renderParams, mesh, cameraData._commandBuffer, 1, num2);
							}
							if (isProfileShadowCasting && gpuirendererData.IsShadowCasting && flag)
							{
								if (isOverrideShadowLayer)
								{
									this._renderParams.layer = shadowLayerOverride;
									this._renderParams.renderingLayerMask = shadowRenderingLayerOverride;
								}
								this._renderParams.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
								GPUIUtility.RenderMeshIndirect(in this._renderParams, mesh, cameraData._commandBuffer, 1, num3);
							}
							num2++;
							num3++;
						}
					}
					else
					{
						num2 += gpuirendererData.rendererMaterials.Length;
						num3 += gpuirendererData.rendererMaterials.Length;
					}
				}
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000FDAA File Offset: 0x0000DFAA
		private int GetMaximumLODLevel(int lodCount, int profileMaximumLODLevel, int qualityMaximumLODLevel)
		{
			if (lodCount <= 1)
			{
				return 0;
			}
			return Mathf.Max(profileMaximumLODLevel, qualityMaximumLODLevel);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		private Material GetReplacementMaterial(GPUIRendererData renderer, int materialIndex, List<string> keywords)
		{
			Material material = null;
			if (Application.isPlaying)
			{
				material = renderer.replacementMaterials[materialIndex];
			}
			if (material == null)
			{
				string text = null;
				if (renderer.isSkinnedMesh)
				{
					text = "CROWD";
				}
				if (this.MaterialProvider.TryGetReplacementMaterial(renderer.rendererMaterials[materialIndex], keywords, text, out material) && Application.isPlaying)
				{
					renderer.replacementMaterials[materialIndex] = material;
				}
			}
			return material;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000FE20 File Offset: 0x0000E020
		private void ExecuteOnPreCull(GPUICameraData cameraData)
		{
			Action<GPUICameraData> onPreCull = this.OnPreCull;
			if (onPreCull != null)
			{
				onPreCull(cameraData);
			}
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.ExecuteOnPreCull(cameraData);
				}
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000FE90 File Offset: 0x0000E090
		private void ExecuteOnPreRender(GPUICameraData cameraData)
		{
			Action<GPUICameraData> onPreRender = this.OnPreRender;
			if (onPreRender != null)
			{
				onPreRender(cameraData);
			}
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.ExecuteOnPreRender(cameraData);
				}
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000FF00 File Offset: 0x0000E100
		private void ExecuteOnPostRender(GPUICameraData cameraData)
		{
			Action<GPUICameraData> onPostRender = this.OnPostRender;
			if (onPostRender != null)
			{
				onPostRender(cameraData);
			}
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.ExecuteOnPostRender(cameraData);
				}
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000FF70 File Offset: 0x0000E170
		public static void OnLightProbesUpdated()
		{
			if (!GPUIRenderingSystem.IsActive || !Application.isPlaying)
			{
				return;
			}
			GPUIRenderingSystem.Instance._pendingLightProbeUpdateFrame = Time.frameCount;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FF90 File Offset: 0x0000E190
		public static bool WillUpdateLightProbes()
		{
			return GPUIRenderingSystem.IsActive && GPUIRenderingSystem.Instance._pendingLightProbeUpdateFrame >= 0;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FFAC File Offset: 0x0000E1AC
		private void ExecuteLightProbeUpdates()
		{
			int frameCount = Time.frameCount;
			if (this._pendingLightProbeUpdateFrame < 0)
			{
				if (this._lightProbesPositions != null && this._lastLightProbeBufferUsedFrame < frameCount - 30)
				{
					this.ReleaseLightProbeBuffers();
				}
				return;
			}
			if (this._pendingLightProbeUpdateFrame + 2 >= frameCount)
			{
				return;
			}
			this._lastLightProbeBufferUsedFrame = frameCount;
			this._pendingLightProbeUpdateFrame = -1;
			foreach (GPUIRenderSource gpuirenderSource in GPUIRenderingSystem.Instance.RenderSourceProvider.Values)
			{
				IGPUILightProbeDataProvider igpuilightProbeDataProvider = gpuirenderSource.source as IGPUILightProbeDataProvider;
				if (igpuilightProbeDataProvider != null)
				{
					igpuilightProbeDataProvider.OnLightProbesUpdated();
				}
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00010058 File Offset: 0x0000E258
		private bool CheckIsSingleton()
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				this.DestroyInstance();
				return false;
			}
			if (GPUIRenderingSystem.Instance != this)
			{
				this.DestroyInstance();
				return false;
			}
			return true;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00010088 File Offset: 0x0000E288
		private void Initialize()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				this.DestroyInstance();
				return;
			}
			if (!this.IsInitialized)
			{
				this.IsInitialized = true;
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
				this.MaterialProvider = new GPUIMaterialProvider();
				this.MaterialProvider.Initialize();
				this.LODGroupDataProvider = new GPUILODGroupDataProvider();
				this.LODGroupDataProvider.Initialize();
				this.RenderSourceGroupProvider = new GPUIRenderSourceGroupProvider();
				this.RenderSourceGroupProvider.Initialize();
				this.RenderSourceProvider = new GPUIRenderSourceProvider();
				this.RenderSourceProvider.Initialize();
				this.CameraDataProvider = new GPUICameraDataProvider();
				this.CameraDataProvider.Initialize();
				this.TreeProxyProvider = new GPUITreeProxyProvider();
				this.TreeProxyProvider.Initialize();
				this.ParameterBuffer = new GPUIDataBuffer<float>("Parameter", 0, GraphicsBuffer.Target.Structured);
				this.ParameterBufferIndexes = new Dictionary<IGPUIParameterBufferData, int>();
				this.ActiveGPUIManagers = new List<GPUIManager>();
				this._renderParams = new RenderParams(GPUIShaderBindings.Instance.ErrorMaterial);
				this._worldBounds = new Bounds(Vector3.zero, GPUIRuntimeSettings.Instance.instancingBoundsSize);
				this._dependentDisposables = new List<IGPUIDisposable>();
				this._activeSystemExtensions = new List<GPUISystemExtension>();
				UnityAction onRenderingSystemInitialized = GPUIRenderingSystem._onRenderingSystemInitialized;
				if (onRenderingSystemInitialized != null)
				{
					onRenderingSystemInitialized();
				}
				if (this.DummyGraphicsBuffer != null)
				{
					this.DummyGraphicsBuffer.Dispose();
				}
				this.DummyGraphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, 4);
				this._ignoreCameraIIDCollection = new HashSet<int>();
				if (GPUIRenderingSystem.prefabRendererList == null)
				{
					GPUIRenderingSystem.prefabRendererList = new List<Renderer>();
				}
				this._instancingBoundsMinMaxBuffer = new GPUIDataBuffer<uint>("InstancingBoundsBuffer", 0, GraphicsBuffer.Target.Structured);
				this._calculateInstancingBoundsCallback = new Action<GPUIDataBuffer<uint>>(this.CalculateInstancingBoundsCallback);
				Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPreCull));
				RenderPipelineManager.beginCameraRendering -= GPUIRenderingSystem.CameraOnBeginRendering;
				if (GPUIRuntimeSettings.Instance.IsBuiltInRP)
				{
					Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPreCull));
				}
				else
				{
					RenderPipelineManager.beginCameraRendering += GPUIRenderingSystem.CameraOnBeginRendering;
				}
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPostRender));
				RenderPipelineManager.endCameraRendering -= GPUIRenderingSystem.CameraOnEndRendering;
				if (GPUIRuntimeSettings.Instance.IsBuiltInRP)
				{
					Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPostRender));
				}
				else
				{
					RenderPipelineManager.endCameraRendering += GPUIRenderingSystem.CameraOnEndRendering;
					if (!GPUIRuntimeSettings.Instance.IsBuiltInRP)
					{
						RenderPipelineManager.endContextRendering -= GPUIRenderingSystem.OnEndContextRendering;
						RenderPipelineManager.endContextRendering += GPUIRenderingSystem.OnEndContextRendering;
					}
				}
				LightProbes.lightProbesUpdated -= GPUIRenderingSystem.OnLightProbesUpdated;
				LightProbes.lightProbesUpdated += GPUIRenderingSystem.OnLightProbesUpdated;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00010354 File Offset: 0x0000E554
		public void Dispose()
		{
			this.IsInitialized = false;
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				gpuisystemExtension.Dispose();
			}
			this._activeSystemExtensions = null;
			if (this.MaterialProvider != null)
			{
				this.MaterialProvider.Dispose();
				this.MaterialProvider = null;
			}
			if (this.LODGroupDataProvider != null)
			{
				this.LODGroupDataProvider.Dispose();
				this.LODGroupDataProvider = null;
			}
			if (this.RenderSourceGroupProvider != null)
			{
				this.RenderSourceGroupProvider.Dispose();
				this.RenderSourceGroupProvider = null;
			}
			if (this.RenderSourceProvider != null)
			{
				this.RenderSourceProvider.Dispose();
				this.RenderSourceProvider = null;
			}
			if (this.CameraDataProvider != null)
			{
				this.CameraDataProvider.Dispose();
				this.CameraDataProvider = null;
			}
			if (this.TreeProxyProvider != null)
			{
				this.TreeProxyProvider.Dispose();
				this.TreeProxyProvider = null;
			}
			if (this.ParameterBuffer != null)
			{
				this.ParameterBuffer.Dispose();
				this.ParameterBuffer = null;
			}
			if (this.DummyGraphicsBuffer != null)
			{
				this.DummyGraphicsBuffer.Dispose();
				this.DummyGraphicsBuffer = null;
			}
			this._ignoreCameraIIDCollection = null;
			this.ReleaseLightProbeBuffers();
			if (this._dependentDisposables != null)
			{
				foreach (IGPUIDisposable igpuidisposable in this._dependentDisposables)
				{
					igpuidisposable.Dispose();
				}
				this._dependentDisposables = null;
			}
			this.ParameterBufferIndexes = null;
			this.ActiveGPUIManagers = null;
			this.OnCommandBufferModified = null;
			this.OnPreCull = null;
			this.OnPreRender = null;
			this.OnPostRender = null;
			if (this._instancingBoundsMinMaxBuffer != null)
			{
				this._instancingBoundsMinMaxBuffer.Dispose();
				this._instancingBoundsMinMaxBuffer = null;
			}
			if (!GPUIRenderingSystem.IsActive)
			{
				Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPreCull));
				RenderPipelineManager.beginCameraRendering -= GPUIRenderingSystem.CameraOnBeginRendering;
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(GPUIRenderingSystem.CameraOnPostRender));
				RenderPipelineManager.endCameraRendering -= GPUIRenderingSystem.CameraOnEndRendering;
				RenderPipelineManager.endContextRendering -= GPUIRenderingSystem.OnEndContextRendering;
				LightProbes.lightProbesUpdated -= GPUIRenderingSystem.OnLightProbesUpdated;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000105B0 File Offset: 0x0000E7B0
		public void ReleaseLightProbeBuffers()
		{
			if (this._lightProbesSphericalHarmonicsBuffer != null)
			{
				this._lightProbesSphericalHarmonicsBuffer.Dispose();
				this._lightProbesSphericalHarmonicsBuffer = null;
			}
			if (this._lightProbesOcclusionProbesBuffer != null)
			{
				this._lightProbesOcclusionProbesBuffer.Dispose();
				this._lightProbesOcclusionProbesBuffer = null;
			}
			this._lightProbesPositions = null;
			this._lightProbesSphericalHarmonics = null;
			this._lightProbesOcclusionProbes = null;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00010606 File Offset: 0x0000E806
		public void ReleaseBuffers()
		{
			if (this.CameraDataProvider != null)
			{
				this.CameraDataProvider.ReleaseBuffers();
			}
			if (this.ParameterBuffer != null)
			{
				this.ParameterBuffer.ReleaseBuffers();
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001062E File Offset: 0x0000E82E
		private void DestroyInstance()
		{
			base.gameObject.DestroyGeneric();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0001063B File Offset: 0x0000E83B
		public static void ResetRenderingSystem()
		{
			if (GPUIRenderingSystem.Instance != null)
			{
				GPUIRenderingSystem.Instance.DestroyInstance();
			}
			GPUIRenderingSystem.InitializeRenderingSystem();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00010659 File Offset: 0x0000E859
		public static void RegenerateRenderers()
		{
			if (GPUIRenderingSystem.Instance != null)
			{
				GPUIRenderingSystem.Instance.LODGroupDataProvider.RegenerateLODGroups();
				GPUIRenderingSystem.Instance.UpdateParameterBufferData();
				GPUIRenderingSystem.Instance.MaterialProvider.Reset();
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00010690 File Offset: 0x0000E890
		public static void InitializeRenderingSystem()
		{
			if (GPUIRenderingSystem.IsActive || !GPUIRuntimeSettings.IsSupportedPlatform())
			{
				return;
			}
			if (GPUIRenderingSystem.Instance == null)
			{
				GameObject gameObject = new GameObject();
				GPUIRenderingSystem.Instance = gameObject.AddComponent<GPUIRenderingSystem>();
				if (GPUIRenderingSystem.Instance == null)
				{
					return;
				}
				gameObject.name = "===GPUI Rendering System [" + GPUIRenderingSystem.Instance.GetInstanceIDGPUI().ToString() + "]===";
				gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			GPUIRenderingSystem.Instance.Initialize();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00010711 File Offset: 0x0000E911
		public static void AddActiveManager(GPUIManager manager)
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			if (!GPUIRenderingSystem.Instance.ActiveGPUIManagers.Contains(manager))
			{
				GPUIRenderingSystem.Instance.ActiveGPUIManagers.Add(manager);
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0001073A File Offset: 0x0000E93A
		public static void RemoveActiveManager(GPUIManager manager)
		{
			if (GPUIRenderingSystem.Instance != null && GPUIRenderingSystem.Instance.IsInitialized)
			{
				GPUIRenderingSystem.Instance.ActiveGPUIManagers.Remove(manager);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00010766 File Offset: 0x0000E966
		public static void AddOnRenderingSystemInitializedListener(UnityAction action)
		{
			GPUIRenderingSystem._onRenderingSystemInitialized = (UnityAction)Delegate.Remove(GPUIRenderingSystem._onRenderingSystemInitialized, action);
			GPUIRenderingSystem._onRenderingSystemInitialized = (UnityAction)Delegate.Combine(GPUIRenderingSystem._onRenderingSystemInitialized, action);
			if (GPUIRenderingSystem.IsActive)
			{
				action();
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0001079F File Offset: 0x0000E99F
		public void AddRenderingSystemExtension(GPUISystemExtension systemExtension)
		{
			if (!this._activeSystemExtensions.Contains(systemExtension))
			{
				this._activeSystemExtensions.Add(systemExtension);
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000107BB File Offset: 0x0000E9BB
		public void RemoveRenderingSystemExtension(GPUISystemExtension systemExtension)
		{
			this._activeSystemExtensions.Remove(systemExtension);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000107CA File Offset: 0x0000E9CA
		public static bool RegisterRenderer(Object source, GameObject prefab, out int rendererKey, int groupID = 0, GPUITransformBufferType transformBufferType = GPUITransformBufferType.Default, List<string> shaderKeywords = null)
		{
			return GPUIRenderingSystem.RegisterRenderer(source, prefab, GPUIProfile.DefaultProfile, out rendererKey, groupID, transformBufferType, shaderKeywords);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000107E0 File Offset: 0x0000E9E0
		public static bool RegisterRenderer(Object source, GameObject prefab, GPUIProfile profile, out int rendererKey, int groupID = 0, GPUITransformBufferType transformBufferType = GPUITransformBufferType.Default, List<string> shaderKeywords = null)
		{
			if (prefab == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Given prefab is null! Can not register renderer.");
				rendererKey = 0;
				return false;
			}
			GPUIPrototype gpuiprototype = new GPUIPrototype(prefab, profile);
			return GPUIRenderingSystem.RegisterRenderer(source, gpuiprototype, out rendererKey, groupID, transformBufferType, shaderKeywords);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00010825 File Offset: 0x0000EA25
		public static bool RegisterRenderer(Object source, GPUIPrototype prototype, out int rendererKey, int groupID = 0, GPUITransformBufferType transformBufferType = GPUITransformBufferType.Default, List<string> shaderKeywords = null)
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			return GPUIRenderingSystem.RegisterRenderer(source, prototype.GetKey(), GPUIRenderingSystem.Instance.LODGroupDataProvider.GetOrCreateLODGroupData(prototype), prototype.profile, out rendererKey, groupID, transformBufferType, shaderKeywords);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010854 File Offset: 0x0000EA54
		public static bool RegisterRenderer(Object source, int prototypeKey, GPUILODGroupData lodGroupData, GPUIProfile profile, out int rendererKey, int groupID, GPUITransformBufferType transformBufferType, List<string> shaderKeywords)
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			rendererKey = 0;
			if (source == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Source is null!");
				return false;
			}
			if (lodGroupData == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "LODGroupData is null!", source);
				return false;
			}
			if (profile == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Profile is null!", source);
				return false;
			}
			if (profile.isLODCrossFade && (shaderKeywords == null || !shaderKeywords.Contains("LOD_FADE_CROSSFADE")))
			{
				if (shaderKeywords == null)
				{
					shaderKeywords = new List<string>();
				}
				shaderKeywords.Add("LOD_FADE_CROSSFADE");
			}
			GPUIRenderSourceGroup orCreateRenderSourceGroup = GPUIRenderingSystem.Instance.RenderSourceGroupProvider.GetOrCreateRenderSourceGroup(prototypeKey, lodGroupData, profile, groupID, transformBufferType, shaderKeywords);
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryCreateRenderSource(source, orCreateRenderSourceGroup, out gpuirenderSource))
			{
				rendererKey = gpuirenderSource.Key;
				return true;
			}
			return false;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010934 File Offset: 0x0000EB34
		public static int GetBufferSize(int renderKey)
		{
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.IsActive && GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				return gpuirenderSource.bufferSize;
			}
			return 0;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00010964 File Offset: 0x0000EB64
		public static bool SetBufferSize(int renderKey, int bufferSize, bool isCopyPreviousData = true)
		{
			if (bufferSize < 0)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Buffer size is not set for renderer with key: " + renderKey.ToString());
				return false;
			}
			if ((long)bufferSize > GPUIConstants.MAX_BUFFER_SIZE)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					bufferSize.ToString("#,0"),
					" exceeds maximum allowed buffer size (",
					GPUIConstants.MAX_BUFFER_SIZE.ToString("#,0"),
					")."
				}));
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.SetBufferSize(bufferSize, isCopyPreviousData);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
			return false;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00010A20 File Offset: 0x0000EC20
		public static int GetInstanceCount(int renderKey)
		{
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.IsActive && GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				return gpuirenderSource.instanceCount;
			}
			return 0;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00010A50 File Offset: 0x0000EC50
		public static bool SetInstanceCount(int renderKey, int instanceCount)
		{
			if (instanceCount < 0)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Instance Count is not set for renderer with key: " + renderKey.ToString());
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.SetInstanceCount(instanceCount);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
			return false;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		public static bool SetTransformBufferData<[IsUnmanaged] T>(int renderKey, NativeArray<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer = true) where T : struct, ValueType
		{
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.SetTransformBufferData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
			return false;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00010B04 File Offset: 0x0000ED04
		public static bool SetTransformBufferData<[IsUnmanaged] T>(int renderKey, T[] matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer = true) where T : struct, ValueType
		{
			if (matrices == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Matrices are not set for renderer with key: " + renderKey.ToString());
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.SetTransformBufferData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
			return false;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010B70 File Offset: 0x0000ED70
		public static bool SetTransformBufferData<[IsUnmanaged] T>(int renderKey, List<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer = true) where T : struct, ValueType
		{
			if (matrices == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Matrices are not set for renderer with key: " + renderKey.ToString());
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.SetTransformBufferData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
			return false;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public static void AddMaterialPropertyOverride(int renderKey, string propertyName, object propertyValue, int lodIndex = -1, int rendererIndex = -1, bool isPersistent = false)
		{
			GPUIRenderingSystem.AddMaterialPropertyOverride(renderKey, Shader.PropertyToID(propertyName), propertyValue, lodIndex, rendererIndex, isPersistent);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010BF0 File Offset: 0x0000EDF0
		public static void AddMaterialPropertyOverride(int renderKey, int nameID, object propertyValue, int lodIndex = -1, int rendererIndex = -1, bool isPersistent = false)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Rendering system is not initialized. Can not override MaterialPropertyBlock.");
				return;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.renderSourceGroup.AddMaterialPropertyOverride(nameID, propertyValue, lodIndex, rendererIndex, isPersistent);
				return;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010C64 File Offset: 0x0000EE64
		public static void AddMaterialPropertyOverrideToRenderSourceGroup(int renderSourceGroupKey, int nameID, object propertyValue, int lodIndex = -1, int rendererIndex = -1, bool isPersistent = false)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Rendering system is not initialized. Can not override MaterialPropertyBlock.");
				return;
			}
			GPUIRenderSourceGroup gpuirenderSourceGroup;
			if (GPUIRenderingSystem.Instance.RenderSourceGroupProvider.TryGetData(renderSourceGroupKey, out gpuirenderSourceGroup))
			{
				gpuirenderSourceGroup.AddMaterialPropertyOverride(nameID, propertyValue, lodIndex, rendererIndex, isPersistent);
				return;
			}
			string log_PREFIX = GPUIConstants.LOG_PREFIX;
			string text = "RenderSourceGroup is not registered with key: ";
			GPUIRenderSourceGroup gpuirenderSourceGroup2 = gpuirenderSourceGroup;
			Debug.LogError(log_PREFIX + text + ((gpuirenderSourceGroup2 != null) ? gpuirenderSourceGroup2.ToString() : null));
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010CD6 File Offset: 0x0000EED6
		public static void RemoveMaterialPropertyOverrides(int renderKey, string propertyName)
		{
			GPUIRenderingSystem.RemoveMaterialPropertyOverrides(renderKey, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		public static void RemoveMaterialPropertyOverrides(int renderKey, int nameID)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				return;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.renderSourceGroup.RemoveMaterialPropertyOverrides(nameID);
				return;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010D20 File Offset: 0x0000EF20
		public static void ClearMaterialPropertyOverrides(int renderKey)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				return;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.renderSourceGroup.ClearMaterialPropertyOverrides();
				return;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010D5C File Offset: 0x0000EF5C
		public static void AddDependentDisposable(IGPUIDisposable gpuiDisposable)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Rendering system is not initialized. Can not add Disposable.");
				return;
			}
			if (!GPUIRenderingSystem.Instance._dependentDisposables.Contains(gpuiDisposable))
			{
				GPUIRenderingSystem.Instance._dependentDisposables.Add(gpuiDisposable);
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010DB0 File Offset: 0x0000EFB0
		public static void AddDependentDisposable(int renderKey, IGPUIDisposable gpuiDisposable)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Rendering system is not initialized. Can not add Disposable.");
				return;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(renderKey, out gpuirenderSource))
			{
				gpuirenderSource.renderSourceGroup.AddDependentDisposable(gpuiDisposable);
				return;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Renderer is not registered with key: " + renderKey.ToString());
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00010E1C File Offset: 0x0000F01C
		public static bool AddDependentDisposableToRenderSourceGroup(int renderSourceGroupKey, IGPUIDisposable gpuiDisposable)
		{
			if (GPUIRenderingSystem.Instance == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Rendering system is not initialized. Can not add Disposable.");
				return false;
			}
			GPUIRenderSourceGroup gpuirenderSourceGroup;
			if (GPUIRenderingSystem.Instance.RenderSourceGroupProvider.TryGetData(renderSourceGroupKey, out gpuirenderSourceGroup))
			{
				gpuirenderSourceGroup.AddDependentDisposable(gpuiDisposable);
				return true;
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Render Source Group is not registered with key: " + renderSourceGroupKey.ToString());
			return false;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010E85 File Offset: 0x0000F085
		public static void DisposeRenderer(int renderKey)
		{
			if (GPUIRenderingSystem.Instance == null || !GPUIRenderingSystem.Instance.IsInitialized)
			{
				return;
			}
			GPUIRenderingSystem.Instance.RenderSourceProvider.DisposeRenderer(renderKey);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		internal void UpdateCommandBuffers(bool forceNew = false)
		{
			if (this.CameraDataProvider == null)
			{
				return;
			}
			foreach (GPUICameraData gpuicameraData in this.CameraDataProvider.Values)
			{
				this.UpdateCommandBuffers(gpuicameraData, forceNew);
			}
			Action onCommandBufferModified = this.OnCommandBufferModified;
			if (onCommandBufferModified == null)
			{
				return;
			}
			onCommandBufferModified();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00010F28 File Offset: 0x0000F128
		internal void UpdateCommandBuffers(GPUICameraData cameraData, bool forceNew = false)
		{
			if (forceNew)
			{
				cameraData.ClearVisibilityData();
			}
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in this.RenderSourceGroupProvider.Values)
			{
				GPUITransformBufferData transformBufferData = gpuirenderSourceGroup.TransformBufferData;
				if (transformBufferData != null)
				{
					transformBufferData.ReleaseInstanceDataBuffers(cameraData);
				}
				gpuirenderSourceGroup.UpdateCommandBuffer(cameraData);
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010F9C File Offset: 0x0000F19C
		internal void UpdateCommandBuffers(GPUIRenderSourceGroup rsg)
		{
			foreach (GPUICameraData gpuicameraData in this.CameraDataProvider.Values)
			{
				rsg.UpdateCommandBuffer(gpuicameraData);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		public static bool TryGetLODGroupData(GPUIPrototype prototype, out GPUILODGroupData lodGroupData)
		{
			if (prototype == null)
			{
				lodGroupData = null;
				return false;
			}
			return GPUIRenderingSystem.TryGetLODGroupData(prototype.GetKey(), out lodGroupData);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001100A File Offset: 0x0000F20A
		public static bool TryGetLODGroupData(int prototypeKey, out GPUILODGroupData lodGroupData)
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				lodGroupData = null;
				return false;
			}
			return GPUIRenderingSystem.Instance.LODGroupDataProvider.TryGetData(prototypeKey, out lodGroupData);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0001102C File Offset: 0x0000F22C
		public static bool TryGetRenderSourceGroup(int runtimeRenderKey, out GPUIRenderSourceGroup renderSourceGroup)
		{
			renderSourceGroup = null;
			if (!GPUIRenderingSystem.IsActive || runtimeRenderKey == 0)
			{
				return false;
			}
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				using (List<GPUIRenderSource>.Enumerator enumerator2 = gpuirenderSourceGroup.RenderSources.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Key == runtimeRenderKey)
						{
							renderSourceGroup = gpuirenderSourceGroup;
							return renderSourceGroup != null;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000110E0 File Offset: 0x0000F2E0
		public static bool TryGetRenderSource(int runtimeRenderKey, out GPUIRenderSource renderSource)
		{
			renderSource = null;
			return GPUIRenderingSystem.IsActive && runtimeRenderKey != 0 && GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(runtimeRenderKey, out renderSource);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011104 File Offset: 0x0000F304
		public static bool TryGetTransformBuffer(int runtimeRenderKey, out GraphicsBuffer transformBuffer, out int bufferStartIndex, GPUICameraData cameraData = null, bool resetCrossFade = true)
		{
			int num;
			return GPUIRenderingSystem.TryGetTransformBuffer(runtimeRenderKey, out transformBuffer, out bufferStartIndex, out num, cameraData, resetCrossFade);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00011120 File Offset: 0x0000F320
		public static bool TryGetTransformBuffer(int runtimeRenderKey, out GraphicsBuffer transformBuffer, out int bufferStartIndex, out int bufferSize, GPUICameraData cameraData = null, bool resetCrossFade = true)
		{
			transformBuffer = null;
			GPUIShaderBuffer gpuishaderBuffer;
			if (GPUIRenderingSystem.TryGetTransformBuffer(runtimeRenderKey, out gpuishaderBuffer, out bufferStartIndex, out bufferSize, cameraData, resetCrossFade))
			{
				transformBuffer = gpuishaderBuffer.Buffer;
				return transformBuffer != null;
			}
			return false;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00011150 File Offset: 0x0000F350
		public static bool TryGetTransformBuffer(int runtimeRenderKey, out GPUIShaderBuffer shaderBuffer, out int bufferStartIndex, GPUICameraData cameraData = null, bool resetCrossFade = true)
		{
			int num;
			return GPUIRenderingSystem.TryGetTransformBuffer(runtimeRenderKey, out shaderBuffer, out bufferStartIndex, out num, cameraData, resetCrossFade);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0001116C File Offset: 0x0000F36C
		public static bool TryGetTransformBuffer(int runtimeRenderKey, out GPUIShaderBuffer shaderBuffer, out int bufferStartIndex, out int bufferSize, GPUICameraData cameraData = null, bool resetCrossFade = true)
		{
			shaderBuffer = null;
			GPUITransformBufferData gpuitransformBufferData;
			if (GPUIRenderingSystem.TryGetTransformBufferData(runtimeRenderKey, out gpuitransformBufferData, out bufferStartIndex, out bufferSize, resetCrossFade))
			{
				shaderBuffer = gpuitransformBufferData.GetTransformBuffer(cameraData);
				return shaderBuffer != null;
			}
			return false;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0001119C File Offset: 0x0000F39C
		public static bool TryGetTransformBufferData(int runtimeRenderKey, out GPUITransformBufferData transformBufferData, out int bufferStartIndex, out int bufferSize, bool resetCrossFade = true)
		{
			transformBufferData = null;
			bufferStartIndex = 0;
			bufferSize = 0;
			if (runtimeRenderKey == 0 || !GPUIRenderingSystem.IsActive)
			{
				return false;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(runtimeRenderKey, out gpuirenderSource) && gpuirenderSource.renderSourceGroup != null)
			{
				bufferStartIndex = gpuirenderSource.bufferStartIndex;
				bufferSize = gpuirenderSource.bufferSize;
				transformBufferData = gpuirenderSource.renderSourceGroup.TransformBufferData;
				if (transformBufferData != null)
				{
					if (resetCrossFade)
					{
						transformBufferData.resetCrossFadeDataFrame = Time.frameCount;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00011210 File Offset: 0x0000F410
		public static void SetLODColorDebuggingEnabled(int runtimeRenderKey, bool enabled, string colorPropertyName = null)
		{
			if (runtimeRenderKey == 0 || !GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(runtimeRenderKey, out gpuirenderSource) && gpuirenderSource.renderSourceGroup != null)
			{
				gpuirenderSource.renderSourceGroup.SetLODColorDebuggingEnabled(enabled, colorPropertyName);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011254 File Offset: 0x0000F454
		internal void OnCreatedRenderSourceGroup(GPUIRenderSourceGroup rsg)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnCreatedRenderSourceGroup(rsg);
				}
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000112B0 File Offset: 0x0000F4B0
		internal void OnRemovedRenderSourceGroup(int key)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnRemovedRenderSourceGroup(key);
				}
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001130C File Offset: 0x0000F50C
		internal void OnRenderSourceGroupBufferSizeChanged(GPUIRenderSourceGroup rsg)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnRenderSourceGroupBufferSizeChanged(rsg);
				}
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011368 File Offset: 0x0000F568
		internal void OnCreatedRenderSource(GPUIRenderSource rs)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnCreatedRenderSource(rs);
				}
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000113C4 File Offset: 0x0000F5C4
		internal void OnRemovedRenderSource(int key)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnRemovedRenderSource(key);
				}
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00011420 File Offset: 0x0000F620
		internal void OnRenderSourceBufferSizeChanged(GPUIRenderSource rs, int previousBufferSize)
		{
			foreach (GPUISystemExtension gpuisystemExtension in this._activeSystemExtensions)
			{
				if (gpuisystemExtension != null)
				{
					gpuisystemExtension.OnRenderSourceBufferSizeChanged(rs, previousBufferSize);
				}
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00011480 File Offset: 0x0000F680
		public void SetOptionalRendererStatusData(int runtimeRenderKey, NativeArray<uint> optionalRendererStatusData)
		{
			GPUIRenderSource gpuirenderSource;
			if (this.RenderSourceProvider.TryGetData(runtimeRenderKey, out gpuirenderSource) && gpuirenderSource.renderSourceGroup != null)
			{
				gpuirenderSource.renderSourceGroup.TransformBufferData.SetOptionalRendererStatusBufferData(optionalRendererStatusData, gpuirenderSource.bufferStartIndex);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000114BC File Offset: 0x0000F6BC
		internal unsafe void CalculateInterpolatedLightAndOcclusionProbes(GPUITransformBufferData transformBufferData, void* p_matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int rsgBufferSize, Vector3 positionOffset)
		{
			this._lastLightProbeBufferUsedFrame = Time.frameCount;
			if (this._lightProbesPositions == null)
			{
				this._lightProbesPositions = new List<Vector3>(count);
			}
			if (this._lightProbesSphericalHarmonics == null)
			{
				this._lightProbesSphericalHarmonics = new List<SphericalHarmonicsL2>(count);
			}
			if (this._lightProbesOcclusionProbes == null)
			{
				this._lightProbesOcclusionProbes = new List<Vector4>(count);
			}
			if (this._lightProbesSphericalHarmonicsBuffer == null || this._lightProbesSphericalHarmonicsBuffer.count < count)
			{
				if (this._lightProbesSphericalHarmonicsBuffer != null)
				{
					this._lightProbesSphericalHarmonicsBuffer.Dispose();
				}
				this._lightProbesSphericalHarmonicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, sizeof(SphericalHarmonicsL2));
			}
			if (this._lightProbesOcclusionProbesBuffer == null || this._lightProbesOcclusionProbesBuffer.count < count)
			{
				if (this._lightProbesOcclusionProbesBuffer != null)
				{
					this._lightProbesOcclusionProbesBuffer.Dispose();
				}
				this._lightProbesOcclusionProbesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 16);
			}
			GPUIUtility.CalculateInterpolatedLightAndOcclusionProbes(ref transformBufferData._perInstanceLightProbesBuffer, p_matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, rsgBufferSize, this._lightProbesPositions, this._lightProbesSphericalHarmonics, this._lightProbesOcclusionProbes, this._lightProbesSphericalHarmonicsBuffer, this._lightProbesOcclusionProbesBuffer, positionOffset);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000115C0 File Offset: 0x0000F7C0
		public Transform GetInstanceTransform(int runtimeRenderKey, int bufferIndex)
		{
			GPUIRenderSource gpuirenderSource;
			if (runtimeRenderKey != 0 && GPUIRenderingSystem.Instance.RenderSourceProvider.TryGetData(runtimeRenderKey, out gpuirenderSource))
			{
				IGPUIInstanceTransformProvider igpuiinstanceTransformProvider = gpuirenderSource.source as IGPUIInstanceTransformProvider;
				if (igpuiinstanceTransformProvider != null)
				{
					return igpuiinstanceTransformProvider.GetInstanceTransformWithRenderKey(runtimeRenderKey, bufferIndex);
				}
			}
			return null;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00011600 File Offset: 0x0000F800
		public Transform GetInstanceTransformFromRSG(int rsgKey, int rsgBufferIndex)
		{
			GPUIRenderSourceGroup gpuirenderSourceGroup;
			if (rsgKey == 0 || !GPUIRenderingSystem.Instance.RenderSourceGroupProvider.TryGetData(rsgKey, out gpuirenderSourceGroup))
			{
				return null;
			}
			foreach (GPUIRenderSource gpuirenderSource in gpuirenderSourceGroup.RenderSources)
			{
				if (rsgBufferIndex >= gpuirenderSource.bufferStartIndex && rsgBufferIndex < gpuirenderSource.bufferStartIndex + gpuirenderSource.bufferSize)
				{
					IGPUIInstanceTransformProvider igpuiinstanceTransformProvider = gpuirenderSource.source as IGPUIInstanceTransformProvider;
					if (igpuiinstanceTransformProvider == null)
					{
						return null;
					}
					return igpuiinstanceTransformProvider.GetInstanceTransformWithRenderKey(gpuirenderSource.Key, rsgBufferIndex - gpuirenderSource.bufferStartIndex);
				}
			}
			return null;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000116B0 File Offset: 0x0000F8B0
		internal static void AddCameraData(GPUICameraData cameraData)
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIRenderingSystem.Instance.CameraDataProvider.AddCameraData(cameraData);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000116C8 File Offset: 0x0000F8C8
		internal void UpdateParameterBufferData()
		{
			foreach (IGPUIParameterBufferData igpuiparameterBufferData in this.ParameterBufferIndexes.Keys)
			{
				igpuiparameterBufferData.SetParameterBufferData();
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00011720 File Offset: 0x0000F920
		[Obsolete("SetGlobalWindVector is deprecated and will be removed in a future update. Please use SetTreeCreatorWindParams instead")]
		public static bool SetGlobalWindVector()
		{
			WindZone[] array = GPUIUtility.FindObjectsByType<WindZone>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].mode == WindZoneMode.Directional)
				{
					Shader.SetGlobalVector("_Wind", new Vector4(array[i].windTurbulence, array[i].windPulseMagnitude, array[i].windPulseFrequency, array[i].windMain));
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001177D File Offset: 0x0000F97D
		public static bool SetTreeCreatorWindParams()
		{
			return GPUIRenderingSystem.IsActive && GPUIRenderingSystem.Instance.SetWindZoneValues();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00011794 File Offset: 0x0000F994
		public bool SetWindZoneValues()
		{
			this._hasTreeCreatorWind = true;
			bool flag = false;
			if (this.windZone != null && this.windZone.gameObject.activeInHierarchy)
			{
				flag = true;
			}
			else
			{
				WindZone[] array = GPUIUtility.FindObjectsByType<WindZone>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].mode == WindZoneMode.Directional)
					{
						this.windZone = array[i];
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Vector4 vector = new Vector4(this.windZone.windTurbulence, this.windZone.windPulseMagnitude, this.windZone.windPulseFrequency, this.windZone.windMain);
				if (this._windZoneValues != vector)
				{
					this._windZoneValues = vector;
					Shader.SetGlobalVector(GPUIConstants.PROP_GPUIWindZone, this._windZoneValues);
				}
				Vector3 forward = this.windZone.transform.forward;
				if (this._windDirection != forward)
				{
					this._windDirection = forward;
					Shader.SetGlobalVector(GPUIConstants.PROP_GPUIWindDirection, this._windDirection);
				}
			}
			return flag;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00011896 File Offset: 0x0000FA96
		private static int DecodeIBValue(uint u)
		{
			return (int)(u ^ 2147483648U);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000118A0 File Offset: 0x0000FAA0
		private void CalculateInstancingBoundsCallback(GPUIDataBuffer<uint> buffer)
		{
			if (!this.IsInitialized)
			{
				return;
			}
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				GPUITransformBufferData gpuitransformBufferData = ((gpuirenderSourceGroup != null) ? gpuirenderSourceGroup.TransformBufferData : null);
				if (gpuitransformBufferData != null)
				{
					int instancingBoundsIndex = gpuitransformBufferData._instancingBoundsIndex;
					if (instancingBoundsIndex >= 0 && !gpuitransformBufferData.HasInstancingBounds && instancingBoundsIndex + 5 < this._instancingBoundsMinMaxBuffer.Length)
					{
						Vector3 vector = new Vector3((float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex]), (float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex + 1]), (float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex + 2]));
						Vector3 vector2 = new Vector3((float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex + 3]), (float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex + 4]), (float)GPUIRenderingSystem.DecodeIBValue(this._instancingBoundsMinMaxBuffer[instancingBoundsIndex + 5]));
						if (vector.x > vector2.x || vector.y > vector2.y || vector.z > vector2.z)
						{
							gpuitransformBufferData.RequireInstancingBoundsUpdate();
						}
						else
						{
							Vector3 boundsOffset = gpuirenderSourceGroup.Profile.boundsOffset;
							vector -= boundsOffset;
							vector2 += boundsOffset;
							gpuitransformBufferData.HasInstancingBounds = true;
							gpuitransformBufferData._instancingBounds.SetMinMax(vector, vector2);
						}
					}
				}
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00011A44 File Offset: 0x0000FC44
		public static void Threading_RunOnMain(Action action)
		{
			if (action == null)
			{
				return;
			}
			GPUIRenderingSystem._threadingCallbackQueue.Enqueue(action);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00011A58 File Offset: 0x0000FC58
		private static void Threading_Update()
		{
			Action action;
			while (GPUIRenderingSystem._threadingCallbackQueue.TryDequeue(out action))
			{
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00011A7E File Offset: 0x0000FC7E
		private static void Threading_InvalidateAll()
		{
			GPUIRenderingSystem._threadingGeneration += 1UL;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00011A8D File Offset: 0x0000FC8D
		public static ulong Threading_GetGeneration()
		{
			return GPUIRenderingSystem._threadingGeneration;
		}

		// Token: 0x0400022E RID: 558
		[NonSerialized]
		private RenderParams _renderParams;

		// Token: 0x0400022F RID: 559
		[NonSerialized]
		private Bounds _worldBounds;

		// Token: 0x04000230 RID: 560
		[NonSerialized]
		private List<IGPUIDisposable> _dependentDisposables;

		// Token: 0x04000231 RID: 561
		[NonSerialized]
		private int _lastDrawCallFrame;

		// Token: 0x04000232 RID: 562
		[NonSerialized]
		private float _lastDrawCallTime;

		// Token: 0x04000235 RID: 565
		public Action OnCommandBufferModified;

		// Token: 0x04000236 RID: 566
		public Action<GPUICameraData> OnPreCull;

		// Token: 0x04000237 RID: 567
		public Action<GPUICameraData> OnPreRender;

		// Token: 0x04000238 RID: 568
		public Action<GPUICameraData> OnPostRender;

		// Token: 0x04000239 RID: 569
		private static UnityAction _onRenderingSystemInitialized;

		// Token: 0x0400023A RID: 570
		private List<GPUISystemExtension> _activeSystemExtensions;

		// Token: 0x0400023B RID: 571
		private static MaterialPropertyBlock _emptyMPB;

		// Token: 0x0400023C RID: 572
		public static GPUIRenderingSystem.OnBufferDataModifiedCallback OnBufferDataModified;

		// Token: 0x0400023D RID: 573
		private GraphicsBuffer _lightProbesSphericalHarmonicsBuffer;

		// Token: 0x0400023E RID: 574
		private GraphicsBuffer _lightProbesOcclusionProbesBuffer;

		// Token: 0x0400023F RID: 575
		private List<Vector3> _lightProbesPositions;

		// Token: 0x04000240 RID: 576
		private List<SphericalHarmonicsL2> _lightProbesSphericalHarmonics;

		// Token: 0x04000241 RID: 577
		private List<Vector4> _lightProbesOcclusionProbes;

		// Token: 0x04000242 RID: 578
		private int _pendingLightProbeUpdateFrame;

		// Token: 0x04000243 RID: 579
		private int _lastLightProbeBufferUsedFrame;

		// Token: 0x04000245 RID: 581
		private HashSet<int> _ignoreCameraIIDCollection;

		// Token: 0x04000246 RID: 582
		public static List<Renderer> prefabRendererList = new List<Renderer>();

		// Token: 0x04000247 RID: 583
		[NonSerialized]
		public WindZone windZone;

		// Token: 0x04000248 RID: 584
		[NonSerialized]
		internal bool _hasTreeCreatorWind;

		// Token: 0x04000249 RID: 585
		[NonSerialized]
		private Vector4 _windZoneValues;

		// Token: 0x0400024A RID: 586
		[NonSerialized]
		private Vector3 _windDirection;

		// Token: 0x0400024B RID: 587
		internal GPUIDataBuffer<uint> _instancingBoundsMinMaxBuffer;

		// Token: 0x0400024C RID: 588
		private Action<GPUIDataBuffer<uint>> _calculateInstancingBoundsCallback;

		// Token: 0x0400024D RID: 589
		internal bool _requireInstancingBoundsDataRead;

		// Token: 0x0400024E RID: 590
		private static readonly ConcurrentQueue<Action> _threadingCallbackQueue = new ConcurrentQueue<Action>();

		// Token: 0x0400024F RID: 591
		private static ulong _threadingGeneration;

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x06000331 RID: 817
		public delegate void OnBufferDataModifiedCallback(GPUITransformBufferData transformBufferData);
	}
}
