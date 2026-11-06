using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000EA RID: 234
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	[ImageEffectAllowedInSceneView]
	[ExecuteAlways]
	public class UniversalAdditionalCameraData : MonoBehaviour, ISerializationCallbackReceiver, IAdditionalData
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002CBB1 File Offset: 0x0002ADB1
		public float version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0002CBB9 File Offset: 0x0002ADB9
		internal static UniversalAdditionalCameraData defaultAdditionalCameraData
		{
			get
			{
				if (UniversalAdditionalCameraData.s_DefaultAdditionalCameraData == null)
				{
					UniversalAdditionalCameraData.s_DefaultAdditionalCameraData = new UniversalAdditionalCameraData();
				}
				return UniversalAdditionalCameraData.s_DefaultAdditionalCameraData;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002CBD7 File Offset: 0x0002ADD7
		internal Camera camera
		{
			get
			{
				if (!this.m_Camera)
				{
					base.gameObject.TryGetComponent<Camera>(out this.m_Camera);
				}
				return this.m_Camera;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0002CBFE File Offset: 0x0002ADFE
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0002CC06 File Offset: 0x0002AE06
		public bool renderShadows
		{
			get
			{
				return this.m_RenderShadows;
			}
			set
			{
				this.m_RenderShadows = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0002CC0F File Offset: 0x0002AE0F
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0002CC17 File Offset: 0x0002AE17
		public CameraOverrideOption requiresDepthOption
		{
			get
			{
				return this.m_RequiresDepthTextureOption;
			}
			set
			{
				this.m_RequiresDepthTextureOption = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0002CC20 File Offset: 0x0002AE20
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0002CC28 File Offset: 0x0002AE28
		public CameraOverrideOption requiresColorOption
		{
			get
			{
				return this.m_RequiresOpaqueTextureOption;
			}
			set
			{
				this.m_RequiresOpaqueTextureOption = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0002CC31 File Offset: 0x0002AE31
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0002CC39 File Offset: 0x0002AE39
		public CameraRenderType renderType
		{
			get
			{
				return this.m_CameraType;
			}
			set
			{
				this.m_CameraType = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0002CC44 File Offset: 0x0002AE44
		public List<Camera> cameraStack
		{
			get
			{
				if (this.renderType != CameraRenderType.Base)
				{
					Camera component = base.gameObject.GetComponent<Camera>();
					Debug.LogWarning(string.Format("{0}: This camera is of {1} type. Only Base cameras can have a camera stack.", component.name, this.renderType));
					return null;
				}
				if (!this.scriptableRenderer.SupportsCameraStackingType(CameraRenderType.Base))
				{
					Camera component2 = base.gameObject.GetComponent<Camera>();
					Debug.LogWarning(string.Format("{0}: This camera has a ScriptableRenderer that doesn't support camera stacking. Camera stack is null.", component2.name));
					return null;
				}
				return this.m_Cameras;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002CCC0 File Offset: 0x0002AEC0
		internal void UpdateCameraStack()
		{
			int count = this.m_Cameras.Count;
			this.m_Cameras.RemoveAll((Camera cam) => cam == null);
			int count2 = this.m_Cameras.Count;
			int num = count - count2;
			if (num != 0)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					base.name,
					": ",
					num.ToString(),
					" camera overlay",
					(num > 1) ? "s" : "",
					" no longer exists and will be removed from the camera stack."
				}));
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0002CD63 File Offset: 0x0002AF63
		public bool clearDepth
		{
			get
			{
				return this.m_ClearDepth;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0002CD6B File Offset: 0x0002AF6B
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0002CD8A File Offset: 0x0002AF8A
		public bool requiresDepthTexture
		{
			get
			{
				if (this.m_RequiresDepthTextureOption == CameraOverrideOption.UsePipelineSettings)
				{
					return UniversalRenderPipeline.asset.supportsCameraDepthTexture;
				}
				return this.m_RequiresDepthTextureOption == CameraOverrideOption.On;
			}
			set
			{
				this.m_RequiresDepthTextureOption = (value ? CameraOverrideOption.On : CameraOverrideOption.Off);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002CD99 File Offset: 0x0002AF99
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
		public bool requiresColorTexture
		{
			get
			{
				if (this.m_RequiresOpaqueTextureOption == CameraOverrideOption.UsePipelineSettings)
				{
					return UniversalRenderPipeline.asset.supportsCameraOpaqueTexture;
				}
				return this.m_RequiresOpaqueTextureOption == CameraOverrideOption.On;
			}
			set
			{
				this.m_RequiresOpaqueTextureOption = (value ? CameraOverrideOption.On : CameraOverrideOption.Off);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0002CDC8 File Offset: 0x0002AFC8
		public ScriptableRenderer scriptableRenderer
		{
			get
			{
				if (UniversalRenderPipeline.asset == null)
				{
					return null;
				}
				if (!UniversalRenderPipeline.asset.ValidateRendererData(this.m_RendererIndex))
				{
					int defaultRendererIndex = UniversalRenderPipeline.asset.m_DefaultRendererIndex;
					Debug.LogWarning(string.Concat(new string[]
					{
						"Renderer at <b>index ",
						this.m_RendererIndex.ToString(),
						"</b> is missing for camera <b>",
						this.camera.name,
						"</b>, falling back to Default Renderer. <b>",
						UniversalRenderPipeline.asset.m_RendererDataList[defaultRendererIndex].name,
						"</b>"
					}), UniversalRenderPipeline.asset);
					return UniversalRenderPipeline.asset.GetRenderer(defaultRendererIndex);
				}
				return UniversalRenderPipeline.asset.GetRenderer(this.m_RendererIndex);
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002CE7C File Offset: 0x0002B07C
		public void SetRenderer(int index)
		{
			this.m_RendererIndex = index;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0002CE85 File Offset: 0x0002B085
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0002CE8D File Offset: 0x0002B08D
		public LayerMask volumeLayerMask
		{
			get
			{
				return this.m_VolumeLayerMask;
			}
			set
			{
				this.m_VolumeLayerMask = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0002CE96 File Offset: 0x0002B096
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0002CE9E File Offset: 0x0002B09E
		public Transform volumeTrigger
		{
			get
			{
				return this.m_VolumeTrigger;
			}
			set
			{
				this.m_VolumeTrigger = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0002CEA7 File Offset: 0x0002B0A7
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0002CEAF File Offset: 0x0002B0AF
		internal VolumeFrameworkUpdateMode volumeFrameworkUpdateMode
		{
			get
			{
				return this.m_VolumeFrameworkUpdateModeOption;
			}
			set
			{
				this.m_VolumeFrameworkUpdateModeOption = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002CEB8 File Offset: 0x0002B0B8
		public bool requiresVolumeFrameworkUpdate
		{
			get
			{
				if (this.m_VolumeFrameworkUpdateModeOption == VolumeFrameworkUpdateMode.UsePipelineSettings)
				{
					return UniversalRenderPipeline.asset.volumeFrameworkUpdateMode != VolumeFrameworkUpdateMode.ViaScripting;
				}
				return this.m_VolumeFrameworkUpdateModeOption == VolumeFrameworkUpdateMode.EveryFrame;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002CEDD File Offset: 0x0002B0DD
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0002CEE5 File Offset: 0x0002B0E5
		public VolumeStack volumeStack
		{
			get
			{
				return this.m_VolumeStack;
			}
			set
			{
				if (value == null && this.m_VolumeStack != null)
				{
					if (UniversalAdditionalCameraData.s_CachedVolumeStacks == null)
					{
						UniversalAdditionalCameraData.s_CachedVolumeStacks = new List<VolumeStack>(4);
					}
					UniversalAdditionalCameraData.s_CachedVolumeStacks.Add(this.m_VolumeStack);
				}
				this.m_VolumeStack = value;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002CF1C File Offset: 0x0002B11C
		internal void GetOrCreateVolumeStack()
		{
			if (UniversalAdditionalCameraData.s_CachedVolumeStacks != null && UniversalAdditionalCameraData.s_CachedVolumeStacks.Count > 0)
			{
				int num = UniversalAdditionalCameraData.s_CachedVolumeStacks.Count - 1;
				this.volumeStack = UniversalAdditionalCameraData.s_CachedVolumeStacks[num];
				UniversalAdditionalCameraData.s_CachedVolumeStacks.RemoveAt(num);
			}
			if (this.volumeStack == null)
			{
				this.volumeStack = VolumeManager.instance.CreateStack();
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0002CF7E File Offset: 0x0002B17E
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0002CF86 File Offset: 0x0002B186
		public bool renderPostProcessing
		{
			get
			{
				return this.m_RenderPostProcessing;
			}
			set
			{
				this.m_RenderPostProcessing = value;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0002CF8F File Offset: 0x0002B18F
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0002CF97 File Offset: 0x0002B197
		public AntialiasingMode antialiasing
		{
			get
			{
				return this.m_Antialiasing;
			}
			set
			{
				this.m_Antialiasing = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0002CFA0 File Offset: 0x0002B1A0
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0002CFA8 File Offset: 0x0002B1A8
		public AntialiasingQuality antialiasingQuality
		{
			get
			{
				return this.m_AntialiasingQuality;
			}
			set
			{
				this.m_AntialiasingQuality = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0002CFB1 File Offset: 0x0002B1B1
		public ref TemporalAA.Settings taaSettings
		{
			get
			{
				return ref this.m_TaaSettings;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0002CFB9 File Offset: 0x0002B1B9
		internal TaaPersistentData taaPersistentData
		{
			get
			{
				return this.m_TaaPersistentData;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0002CFC1 File Offset: 0x0002B1C1
		internal MotionVectorsPersistentData motionVectorsPersistentData
		{
			get
			{
				return this.m_MotionVectorsPersistentData;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002CFC9 File Offset: 0x0002B1C9
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0002CFD9 File Offset: 0x0002B1D9
		public bool resetHistory
		{
			get
			{
				return this.m_TaaSettings.resetHistoryFrames != 0;
			}
			set
			{
				this.m_TaaSettings.resetHistoryFrames = this.m_TaaSettings.resetHistoryFrames + (value ? 1 : 0);
				this.m_MotionVectorsPersistentData.Reset();
				this.m_TaaSettings.jitterFrameCountOffset = -Time.frameCount;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002D00D File Offset: 0x0002B20D
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0002D015 File Offset: 0x0002B215
		public bool stopNaN
		{
			get
			{
				return this.m_StopNaN;
			}
			set
			{
				this.m_StopNaN = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002D01E File Offset: 0x0002B21E
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0002D026 File Offset: 0x0002B226
		public bool dithering
		{
			get
			{
				return this.m_Dithering;
			}
			set
			{
				this.m_Dithering = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002D02F File Offset: 0x0002B22F
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0002D037 File Offset: 0x0002B237
		public bool allowXRRendering
		{
			get
			{
				return this.m_AllowXRRendering;
			}
			set
			{
				this.m_AllowXRRendering = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002D040 File Offset: 0x0002B240
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0002D048 File Offset: 0x0002B248
		public bool useScreenCoordOverride
		{
			get
			{
				return this.m_UseScreenCoordOverride;
			}
			set
			{
				this.m_UseScreenCoordOverride = value;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002D051 File Offset: 0x0002B251
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0002D059 File Offset: 0x0002B259
		public Vector4 screenSizeOverride
		{
			get
			{
				return this.m_ScreenSizeOverride;
			}
			set
			{
				this.m_ScreenSizeOverride = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0002D062 File Offset: 0x0002B262
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0002D06A File Offset: 0x0002B26A
		public Vector4 screenCoordScaleBias
		{
			get
			{
				return this.m_ScreenCoordScaleBias;
			}
			set
			{
				this.m_ScreenCoordScaleBias = value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0002D073 File Offset: 0x0002B273
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0002D07B File Offset: 0x0002B27B
		public bool allowHDROutput
		{
			get
			{
				return this.m_AllowHDROutput;
			}
			set
			{
				this.m_AllowHDROutput = value;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002D084 File Offset: 0x0002B284
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002D086 File Offset: 0x0002B286
		public void OnAfterDeserialize()
		{
			if (this.version <= 1f)
			{
				this.m_RequiresDepthTextureOption = (this.m_RequiresDepthTexture ? CameraOverrideOption.On : CameraOverrideOption.Off);
				this.m_RequiresOpaqueTextureOption = (this.m_RequiresColorTexture ? CameraOverrideOption.On : CameraOverrideOption.Off);
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		public void OnDrawGizmos()
		{
			string text = "";
			Color white = Color.white;
			if (this.m_CameraType == CameraRenderType.Base)
			{
				text = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_Base.png";
			}
			else if (this.m_CameraType == CameraRenderType.Overlay)
			{
				text = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_Base.png";
			}
			if (!string.IsNullOrEmpty(text))
			{
				Gizmos.DrawIcon(base.transform.position, text, true, white);
			}
			if (this.renderPostProcessing)
			{
				Gizmos.DrawIcon(base.transform.position, "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_PostProcessing.png", true, white);
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002D12E File Offset: 0x0002B32E
		public void OnDestroy()
		{
			this.m_Camera.DestroyVolumeStack(this);
			if (this.camera.cameraType != CameraType.SceneView)
			{
				ScriptableRenderer rawRenderer = this.GetRawRenderer();
				if (rawRenderer != null)
				{
					rawRenderer.ReleaseRenderTargets();
				}
			}
			TaaPersistentData taaPersistentData = this.m_TaaPersistentData;
			if (taaPersistentData == null)
			{
				return;
			}
			taaPersistentData.DeallocateTargets();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002D16C File Offset: 0x0002B36C
		private unsafe ScriptableRenderer GetRawRenderer()
		{
			if (UniversalRenderPipeline.asset == null)
			{
				return null;
			}
			ReadOnlySpan<ScriptableRenderer> renderers = UniversalRenderPipeline.asset.renderers;
			if (renderers == null || renderers.IsEmpty)
			{
				return null;
			}
			if (this.m_RendererIndex >= renderers.Length || this.m_RendererIndex < 0)
			{
				return null;
			}
			return *renderers[this.m_RendererIndex];
		}

		// Token: 0x040005B6 RID: 1462
		private const string k_GizmoPath = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/";

		// Token: 0x040005B7 RID: 1463
		private const string k_BaseCameraGizmoPath = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_Base.png";

		// Token: 0x040005B8 RID: 1464
		private const string k_OverlayCameraGizmoPath = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_Base.png";

		// Token: 0x040005B9 RID: 1465
		private const string k_PostProcessingGizmoPath = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/Camera_PostProcessing.png";

		// Token: 0x040005BA RID: 1466
		[FormerlySerializedAs("renderShadows")]
		[SerializeField]
		private bool m_RenderShadows = true;

		// Token: 0x040005BB RID: 1467
		[SerializeField]
		private CameraOverrideOption m_RequiresDepthTextureOption = CameraOverrideOption.UsePipelineSettings;

		// Token: 0x040005BC RID: 1468
		[SerializeField]
		private CameraOverrideOption m_RequiresOpaqueTextureOption = CameraOverrideOption.UsePipelineSettings;

		// Token: 0x040005BD RID: 1469
		[SerializeField]
		private CameraRenderType m_CameraType;

		// Token: 0x040005BE RID: 1470
		[SerializeField]
		private List<Camera> m_Cameras = new List<Camera>();

		// Token: 0x040005BF RID: 1471
		[SerializeField]
		private int m_RendererIndex = -1;

		// Token: 0x040005C0 RID: 1472
		[SerializeField]
		private LayerMask m_VolumeLayerMask = 1;

		// Token: 0x040005C1 RID: 1473
		[SerializeField]
		private Transform m_VolumeTrigger;

		// Token: 0x040005C2 RID: 1474
		[SerializeField]
		private VolumeFrameworkUpdateMode m_VolumeFrameworkUpdateModeOption = VolumeFrameworkUpdateMode.UsePipelineSettings;

		// Token: 0x040005C3 RID: 1475
		[SerializeField]
		private bool m_RenderPostProcessing;

		// Token: 0x040005C4 RID: 1476
		[SerializeField]
		private AntialiasingMode m_Antialiasing;

		// Token: 0x040005C5 RID: 1477
		[SerializeField]
		private AntialiasingQuality m_AntialiasingQuality = AntialiasingQuality.High;

		// Token: 0x040005C6 RID: 1478
		[SerializeField]
		private bool m_StopNaN;

		// Token: 0x040005C7 RID: 1479
		[SerializeField]
		private bool m_Dithering;

		// Token: 0x040005C8 RID: 1480
		[SerializeField]
		private bool m_ClearDepth = true;

		// Token: 0x040005C9 RID: 1481
		[SerializeField]
		private bool m_AllowXRRendering = true;

		// Token: 0x040005CA RID: 1482
		[SerializeField]
		private bool m_AllowHDROutput = true;

		// Token: 0x040005CB RID: 1483
		[SerializeField]
		private bool m_UseScreenCoordOverride;

		// Token: 0x040005CC RID: 1484
		[SerializeField]
		private Vector4 m_ScreenSizeOverride;

		// Token: 0x040005CD RID: 1485
		[SerializeField]
		private Vector4 m_ScreenCoordScaleBias;

		// Token: 0x040005CE RID: 1486
		[NonSerialized]
		private Camera m_Camera;

		// Token: 0x040005CF RID: 1487
		[FormerlySerializedAs("requiresDepthTexture")]
		[SerializeField]
		private bool m_RequiresDepthTexture;

		// Token: 0x040005D0 RID: 1488
		[FormerlySerializedAs("requiresColorTexture")]
		[SerializeField]
		private bool m_RequiresColorTexture;

		// Token: 0x040005D1 RID: 1489
		[HideInInspector]
		[SerializeField]
		private float m_Version = 2f;

		// Token: 0x040005D2 RID: 1490
		[NonSerialized]
		private MotionVectorsPersistentData m_MotionVectorsPersistentData = new MotionVectorsPersistentData();

		// Token: 0x040005D3 RID: 1491
		[NonSerialized]
		private TaaPersistentData m_TaaPersistentData = new TaaPersistentData();

		// Token: 0x040005D4 RID: 1492
		[SerializeField]
		internal TemporalAA.Settings m_TaaSettings = TemporalAA.Settings.Create();

		// Token: 0x040005D5 RID: 1493
		private static UniversalAdditionalCameraData s_DefaultAdditionalCameraData;

		// Token: 0x040005D6 RID: 1494
		private static List<VolumeStack> s_CachedVolumeStacks;

		// Token: 0x040005D7 RID: 1495
		private VolumeStack m_VolumeStack;
	}
}
