using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000FB RID: 251
	public struct CameraData
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x00033847 File Offset: 0x00031A47
		internal void SetViewAndProjectionMatrix(Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
		{
			this.m_ViewMatrix = viewMatrix;
			this.m_ProjectionMatrix = projectionMatrix;
			this.m_JitterMatrix = Matrix4x4.identity;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00033862 File Offset: 0x00031A62
		internal void SetViewProjectionAndJitterMatrix(Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, Matrix4x4 jitterMatrix)
		{
			this.m_ViewMatrix = viewMatrix;
			this.m_ProjectionMatrix = projectionMatrix;
			this.m_JitterMatrix = jitterMatrix;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0003387C File Offset: 0x00031A7C
		internal void PushBuiltinShaderConstantsXR(CommandBuffer cmd, bool renderIntoTexture)
		{
			if (this.xr.enabled)
			{
				cmd.SetViewProjectionMatrices(this.GetViewMatrix(0), this.GetProjectionMatrix(0));
				if (this.xr.singlePassEnabled)
				{
					for (int i = 0; i < this.xr.viewCount; i++)
					{
						XRBuiltinShaderConstants.UpdateBuiltinShaderConstants(this.GetViewMatrix(i), this.GetProjectionMatrix(i), renderIntoTexture, i);
					}
					XRBuiltinShaderConstants.SetBuiltinShaderConstants(cmd);
					return;
				}
				Matrix4x4 viewMatrix = this.GetViewMatrix(0);
				Vector3 vector = Matrix4x4.Inverse(this.GetViewMatrix(0)).GetColumn(3);
				cmd.SetGlobalVector(ShaderPropertyId.worldSpaceCameraPos, vector);
				Matrix4x4 gpuprojectionMatrix = this.GetGPUProjectionMatrix(renderIntoTexture, 0);
				gpuprojectionMatrix * viewMatrix;
				Matrix4x4 matrix4x = Matrix4x4.Inverse(viewMatrix);
				Matrix4x4 matrix4x2 = Matrix4x4.Inverse(gpuprojectionMatrix);
				Matrix4x4 matrix4x3 = matrix4x * matrix4x2;
				Matrix4x4 matrix4x4 = Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * viewMatrix;
				Matrix4x4 inverse = matrix4x4.inverse;
				cmd.SetGlobalMatrix(ShaderPropertyId.worldToCameraMatrix, matrix4x4);
				cmd.SetGlobalMatrix(ShaderPropertyId.cameraToWorldMatrix, inverse);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewMatrix, matrix4x);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseProjectionMatrix, matrix4x2);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewAndProjectionMatrix, matrix4x3);
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000339B3 File Offset: 0x00031BB3
		public Matrix4x4 GetViewMatrix(int viewIndex = 0)
		{
			if (this.xr.enabled)
			{
				return this.xr.GetViewMatrix(viewIndex);
			}
			return this.m_ViewMatrix;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000339D5 File Offset: 0x00031BD5
		public Matrix4x4 GetProjectionMatrix(int viewIndex = 0)
		{
			if (this.xr.enabled)
			{
				return this.m_JitterMatrix * this.xr.GetProjMatrix(viewIndex);
			}
			return this.m_JitterMatrix * this.m_ProjectionMatrix;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00033A0D File Offset: 0x00031C0D
		internal Matrix4x4 GetProjectionMatrixNoJitter(int viewIndex = 0)
		{
			if (this.xr.enabled)
			{
				return this.xr.GetProjMatrix(viewIndex);
			}
			return this.m_ProjectionMatrix;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00033A2F File Offset: 0x00031C2F
		public Matrix4x4 GetGPUProjectionMatrix(int viewIndex = 0)
		{
			return this.m_JitterMatrix * GL.GetGPUProjectionMatrix(this.GetProjectionMatrixNoJitter(viewIndex), this.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00033A4E File Offset: 0x00031C4E
		public Matrix4x4 GetGPUProjectionMatrixNoJitter(int viewIndex = 0)
		{
			return GL.GetGPUProjectionMatrix(this.GetProjectionMatrixNoJitter(viewIndex), this.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00033A62 File Offset: 0x00031C62
		internal Matrix4x4 GetGPUProjectionMatrix(bool renderIntoTexture, int viewIndex = 0)
		{
			return this.m_JitterMatrix * GL.GetGPUProjectionMatrix(this.GetProjectionMatrix(viewIndex), renderIntoTexture);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00033A7C File Offset: 0x00031C7C
		public int scaledWidth
		{
			get
			{
				return Mathf.Max(1, (int)((float)this.camera.pixelWidth * this.renderScale));
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00033A98 File Offset: 0x00031C98
		public int scaledHeight
		{
			get
			{
				return Mathf.Max(1, (int)((float)this.camera.pixelHeight * this.renderScale));
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00033AB4 File Offset: 0x00031CB4
		internal bool requireSrgbConversion
		{
			get
			{
				if (this.xr.enabled)
				{
					return !this.xr.renderTargetDesc.sRGB && (this.xr.renderTargetDesc.graphicsFormat == GraphicsFormat.R8G8B8A8_UNorm || this.xr.renderTargetDesc.graphicsFormat == GraphicsFormat.B8G8R8A8_UNorm) && QualitySettings.activeColorSpace == ColorSpace.Linear;
				}
				return this.targetTexture == null && Display.main.requiresSrgbBlitToBackbuffer;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00033B35 File Offset: 0x00031D35
		public bool isSceneViewCamera
		{
			get
			{
				return this.cameraType == CameraType.SceneView;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00033B40 File Offset: 0x00031D40
		public bool isPreviewCamera
		{
			get
			{
				return this.cameraType == CameraType.Preview;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00033B4B File Offset: 0x00031D4B
		internal bool isRenderPassSupportedCamera
		{
			get
			{
				return this.cameraType == CameraType.Game || this.cameraType == CameraType.Reflection;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00033B62 File Offset: 0x00031D62
		internal bool resolveToScreen
		{
			get
			{
				return this.targetTexture == null && this.resolveFinalTarget && (this.cameraType == CameraType.Game || this.camera.cameraType == CameraType.VR);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00033B98 File Offset: 0x00031D98
		public bool isHDROutputActive
		{
			get
			{
				bool flag = UniversalRenderPipeline.HDROutputForMainDisplayIsActive();
				if (this.xr.enabled)
				{
					flag = this.xr.isHDRDisplayOutputActive;
				}
				return flag && this.allowHDROutput && this.resolveToScreen;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00033BD8 File Offset: 0x00031DD8
		public HDROutputUtils.HDRDisplayInformation hdrDisplayInformation
		{
			get
			{
				HDROutputUtils.HDRDisplayInformation hdrDisplayOutputInformation;
				if (this.xr.enabled)
				{
					hdrDisplayOutputInformation = this.xr.hdrDisplayOutputInformation;
				}
				else
				{
					HDROutputSettings main = HDROutputSettings.main;
					hdrDisplayOutputInformation = new HDROutputUtils.HDRDisplayInformation(main.maxFullFrameToneMapLuminance, main.maxToneMapLuminance, main.minToneMapLuminance, main.paperWhiteNits);
				}
				return hdrDisplayOutputInformation;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00033C26 File Offset: 0x00031E26
		public ColorGamut hdrDisplayColorGamut
		{
			get
			{
				if (this.xr.enabled)
				{
					return this.xr.hdrDisplayOutputColorGamut;
				}
				return HDROutputSettings.main.displayColorGamut;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00033C4B File Offset: 0x00031E4B
		public bool rendersOverlayUI
		{
			get
			{
				return SupportedRenderingFeatures.active.rendersUIOverlay && this.resolveToScreen;
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00033C64 File Offset: 0x00031E64
		public bool IsHandleYFlipped(RTHandle handle)
		{
			if (!SystemInfo.graphicsUVStartsAtTop)
			{
				return false;
			}
			if (this.cameraType == CameraType.SceneView || this.cameraType == CameraType.Preview)
			{
				return true;
			}
			RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(handle.nameID, 0, CubemapFace.Unknown, 0);
			bool flag = renderTargetIdentifier == BuiltinRenderTextureType.CameraTarget;
			if (this.xr.enabled)
			{
				flag |= renderTargetIdentifier == new RenderTargetIdentifier(this.xr.renderTarget, 0, CubemapFace.Unknown, 0);
			}
			return !flag;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00033CD8 File Offset: 0x00031ED8
		public bool IsCameraProjectionMatrixFlipped()
		{
			if (!SystemInfo.graphicsUVStartsAtTop)
			{
				return false;
			}
			ScriptableRenderer current = ScriptableRenderer.current;
			if (current != null)
			{
				RTHandle cameraColorTargetHandle = current.cameraColorTargetHandle;
				bool flag;
				if (cameraColorTargetHandle == null)
				{
					if (this.cameraType == CameraType.SceneView)
					{
						flag = true;
					}
					else
					{
						RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(current.cameraColorTarget, 0, CubemapFace.Unknown, 0);
						bool flag2 = renderTargetIdentifier == BuiltinRenderTextureType.CameraTarget;
						if (this.xr.enabled)
						{
							flag2 |= renderTargetIdentifier == new RenderTargetIdentifier(this.xr.renderTarget, 0, CubemapFace.Unknown, 0);
						}
						flag = !flag2;
					}
				}
				else
				{
					flag = this.IsHandleYFlipped(cameraColorTargetHandle);
				}
				return flag || this.targetTexture != null;
			}
			return true;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00033D7D File Offset: 0x00031F7D
		public bool IsRenderTargetProjectionMatrixFlipped(RTHandle color, RTHandle depth = null)
		{
			return SystemInfo.graphicsUVStartsAtTop && (this.targetTexture != null || this.IsHandleYFlipped(color ?? depth));
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00033DA4 File Offset: 0x00031FA4
		internal bool IsTemporalAAEnabled()
		{
			UniversalAdditionalCameraData universalAdditionalCameraData;
			this.camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
			return this.antialiasing == AntialiasingMode.TemporalAntiAliasing && this.taaPersistentData != null && this.cameraTargetDescriptor.msaaSamples == 1 && (universalAdditionalCameraData == null || universalAdditionalCameraData.renderType != CameraRenderType.Overlay) && (universalAdditionalCameraData == null || universalAdditionalCameraData.cameraStack.Count <= 0) && !this.camera.allowDynamicResolution && this.postProcessEnabled;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00033E10 File Offset: 0x00032010
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x00033E18 File Offset: 0x00032018
		public XRPass xr { readonly get; internal set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00033E21 File Offset: 0x00032021
		internal XRPassUniversal xrUniversal
		{
			get
			{
				return this.xr as XRPassUniversal;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00033E2E File Offset: 0x0003202E
		internal bool resetHistory
		{
			get
			{
				return this.taaSettings.resetHistoryFrames != 0;
			}
		}

		// Token: 0x04000687 RID: 1671
		private Matrix4x4 m_ViewMatrix;

		// Token: 0x04000688 RID: 1672
		private Matrix4x4 m_ProjectionMatrix;

		// Token: 0x04000689 RID: 1673
		private Matrix4x4 m_JitterMatrix;

		// Token: 0x0400068A RID: 1674
		public Camera camera;

		// Token: 0x0400068B RID: 1675
		public CameraRenderType renderType;

		// Token: 0x0400068C RID: 1676
		public RenderTexture targetTexture;

		// Token: 0x0400068D RID: 1677
		public RenderTextureDescriptor cameraTargetDescriptor;

		// Token: 0x0400068E RID: 1678
		internal Rect pixelRect;

		// Token: 0x0400068F RID: 1679
		internal bool useScreenCoordOverride;

		// Token: 0x04000690 RID: 1680
		internal Vector4 screenSizeOverride;

		// Token: 0x04000691 RID: 1681
		internal Vector4 screenCoordScaleBias;

		// Token: 0x04000692 RID: 1682
		internal int pixelWidth;

		// Token: 0x04000693 RID: 1683
		internal int pixelHeight;

		// Token: 0x04000694 RID: 1684
		internal float aspectRatio;

		// Token: 0x04000695 RID: 1685
		public float renderScale;

		// Token: 0x04000696 RID: 1686
		internal ImageScalingMode imageScalingMode;

		// Token: 0x04000697 RID: 1687
		internal ImageUpscalingFilter upscalingFilter;

		// Token: 0x04000698 RID: 1688
		internal bool fsrOverrideSharpness;

		// Token: 0x04000699 RID: 1689
		internal float fsrSharpness;

		// Token: 0x0400069A RID: 1690
		internal HDRColorBufferPrecision hdrColorBufferPrecision;

		// Token: 0x0400069B RID: 1691
		public bool clearDepth;

		// Token: 0x0400069C RID: 1692
		public CameraType cameraType;

		// Token: 0x0400069D RID: 1693
		public bool isDefaultViewport;

		// Token: 0x0400069E RID: 1694
		public bool isHdrEnabled;

		// Token: 0x0400069F RID: 1695
		public bool allowHDROutput;

		// Token: 0x040006A0 RID: 1696
		public bool requiresDepthTexture;

		// Token: 0x040006A1 RID: 1697
		public bool requiresOpaqueTexture;

		// Token: 0x040006A2 RID: 1698
		public bool postProcessingRequiresDepthTexture;

		// Token: 0x040006A3 RID: 1699
		public bool xrRendering;

		// Token: 0x040006A4 RID: 1700
		internal bool stackLastCameraOutputToHDR;

		// Token: 0x040006A5 RID: 1701
		public SortingCriteria defaultOpaqueSortFlags;

		// Token: 0x040006A7 RID: 1703
		[Obsolete("Please use xr.enabled instead.", true)]
		public bool isStereoEnabled;

		// Token: 0x040006A8 RID: 1704
		public float maxShadowDistance;

		// Token: 0x040006A9 RID: 1705
		public bool postProcessEnabled;

		// Token: 0x040006AA RID: 1706
		internal bool stackAnyPostProcessingEnabled;

		// Token: 0x040006AB RID: 1707
		public IEnumerator<Action<RenderTargetIdentifier, CommandBuffer>> captureActions;

		// Token: 0x040006AC RID: 1708
		public LayerMask volumeLayerMask;

		// Token: 0x040006AD RID: 1709
		public Transform volumeTrigger;

		// Token: 0x040006AE RID: 1710
		public bool isStopNaNEnabled;

		// Token: 0x040006AF RID: 1711
		public bool isDitheringEnabled;

		// Token: 0x040006B0 RID: 1712
		public AntialiasingMode antialiasing;

		// Token: 0x040006B1 RID: 1713
		public AntialiasingQuality antialiasingQuality;

		// Token: 0x040006B2 RID: 1714
		public ScriptableRenderer renderer;

		// Token: 0x040006B3 RID: 1715
		public bool resolveFinalTarget;

		// Token: 0x040006B4 RID: 1716
		public Vector3 worldSpaceCameraPos;

		// Token: 0x040006B5 RID: 1717
		public Color backgroundColor;

		// Token: 0x040006B6 RID: 1718
		internal TaaPersistentData taaPersistentData;

		// Token: 0x040006B7 RID: 1719
		internal TemporalAA.Settings taaSettings;

		// Token: 0x040006B8 RID: 1720
		public Camera baseCamera;
	}
}
