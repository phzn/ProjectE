using System;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x02000002 RID: 2
public class AzureFogScatteringFeature : ScriptableRendererFeature
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public RenderPassEvent renderPassEvent
	{
		get
		{
			return this.m_renderPassEvent;
		}
		set
		{
			this.m_renderPassEvent = value;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
	// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
	public Material fogRendererMaterial
	{
		get
		{
			return this.m_fogRendererMaterial;
		}
		set
		{
			this.m_fogRendererMaterial = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
	// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
	public Material fogComputationMaterial
	{
		get
		{
			return this.m_fogComputationMaterial;
		}
		set
		{
			this.m_fogComputationMaterial = value;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
	public RenderTexture fogScatteringRT
	{
		get
		{
			AzureFogScatteringFeature.AzureFogScatteringPass azureFogScatteringPass = this.m_azureFogScatteringPass;
			if (azureFogScatteringPass == null)
			{
				return null;
			}
			return azureFogScatteringPass.fogScatteringRT;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000008 RID: 8 RVA: 0x00002096 File Offset: 0x00000296
	public int fogRenderTextureWidth
	{
		get
		{
			return this.m_fogRenderTextureWidth;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000009 RID: 9 RVA: 0x0000209E File Offset: 0x0000029E
	public int fogRenderTextureHeight
	{
		get
		{
			return this.m_fogRenderTextureHeight;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000020A8 File Offset: 0x000002A8
	public override void Create()
	{
		AzureFogScatteringFeature.AzureFogScatteringPass azureFogScatteringPass = this.m_azureFogScatteringPass;
		if (azureFogScatteringPass != null)
		{
			azureFogScatteringPass.Dispose();
		}
		this.m_azureFogScatteringPass = new AzureFogScatteringFeature.AzureFogScatteringPass(this.m_fogRenderTextureWidth, this.m_fogRenderTextureHeight);
		this.m_azureFogScatteringPass.fogRendererMaterial = this.m_fogRendererMaterial;
		this.m_azureFogScatteringPass.fogComputationMaterial = this.m_fogComputationMaterial;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002100 File Offset: 0x00000300
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (AzureWeatherPropertyLinker.SuppressPrecipitation || Shader.GetGlobalFloat(AzureFogScatteringFeature.ProjectFogEnabled) < 0.5f)
		{
			return;
		}
		CameraData cameraData = renderingData.cameraData;
		if (cameraData.cameraType != CameraType.Game || cameraData.renderType != CameraRenderType.Base || cameraData.camera == null || !cameraData.camera.CompareTag("MainCamera"))
		{
			return;
		}
		if (this.fogComputationMaterial == null)
		{
			Debug.LogWarningFormat("Missing the Fog Computation Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", new object[] { base.GetType().Name });
			return;
		}
		if (this.m_fogRendererMaterial == null)
		{
			Debug.LogWarningFormat("Missing the Fog Renderer Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", new object[] { base.GetType().Name });
			return;
		}
		this.m_azureFogScatteringPass.renderPassEvent = this.m_renderPassEvent;
		renderer.EnqueuePass(this.m_azureFogScatteringPass);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021D6 File Offset: 0x000003D6
	protected override void Dispose(bool disposing)
	{
		AzureFogScatteringFeature.AzureFogScatteringPass azureFogScatteringPass = this.m_azureFogScatteringPass;
		if (azureFogScatteringPass != null)
		{
			azureFogScatteringPass.Dispose();
		}
		this.m_azureFogScatteringPass = null;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000021F0 File Offset: 0x000003F0
	public void SetFogScatteringResolution(int width, int height)
	{
		this.m_fogRenderTextureWidth = Mathf.Max(1, width);
		this.m_fogRenderTextureHeight = Mathf.Max(1, height);
		AzureFogScatteringFeature.AzureFogScatteringPass azureFogScatteringPass = this.m_azureFogScatteringPass;
		if (azureFogScatteringPass == null)
		{
			return;
		}
		azureFogScatteringPass.SetMaxResolution(this.m_fogRenderTextureWidth, this.m_fogRenderTextureHeight);
	}

	// Token: 0x04000001 RID: 1
	private static readonly int ProjectFogEnabled = Shader.PropertyToID("_Project_AzureFogScatteringEnabled");

	// Token: 0x04000002 RID: 2
	[SerializeField]
	private RenderPassEvent m_renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private Material m_fogRendererMaterial;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private Material m_fogComputationMaterial;

	// Token: 0x04000005 RID: 5
	[SerializeField]
	private int m_fogRenderTextureWidth = 1920;

	// Token: 0x04000006 RID: 6
	[SerializeField]
	private int m_fogRenderTextureHeight = 1080;

	// Token: 0x04000007 RID: 7
	private AzureFogScatteringFeature.AzureFogScatteringPass m_azureFogScatteringPass;

	// Token: 0x0200002C RID: 44
	private class AzureFogScatteringPass : ScriptableRenderPass
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000A853 File Offset: 0x00008A53
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000A85B File Offset: 0x00008A5B
		public Camera camera
		{
			get
			{
				return this.m_camera;
			}
			set
			{
				this.m_camera = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000A864 File Offset: 0x00008A64
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000A86C File Offset: 0x00008A6C
		public Material fogRendererMaterial
		{
			get
			{
				return this.m_fogRendererMaterial;
			}
			set
			{
				this.m_fogRendererMaterial = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000A875 File Offset: 0x00008A75
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000A87D File Offset: 0x00008A7D
		public Material fogComputationMaterial
		{
			get
			{
				return this.m_fogComputationMaterial;
			}
			set
			{
				this.m_fogComputationMaterial = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A886 File Offset: 0x00008A86
		public RenderTexture fogScatteringRT
		{
			get
			{
				RTHandle fogScatteringRT = this.m_fogScatteringRT;
				if (fogScatteringRT == null)
				{
					return null;
				}
				return fogScatteringRT.rt;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A89C File Offset: 0x00008A9C
		public AzureFogScatteringPass(int maxFogWidth, int maxFogHeight)
		{
			this.SetMaxResolution(maxFogWidth, maxFogHeight);
			base.ConfigureInput(ScriptableRenderPassInput.Depth);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		public void SetMaxResolution(int width, int height)
		{
			this.m_maxFogWidth = Mathf.Max(1, width);
			this.m_maxFogHeight = Mathf.Max(1, height);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A910 File Offset: 0x00008B10
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			this.m_sourceColor = renderingData.cameraData.renderer.cameraColorTargetHandle;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.depthBufferBits = 0;
			cameraTargetDescriptor.msaaSamples = 1;
			cameraTargetDescriptor.bindMS = false;
			cameraTargetDescriptor.useMipMap = false;
			cameraTargetDescriptor.autoGenerateMips = false;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_sourceCopy, in cameraTargetDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_AzureFogSourceCopy");
			RenderTextureDescriptor renderTextureDescriptor = cameraTargetDescriptor;
			renderTextureDescriptor.width = Mathf.Min(cameraTargetDescriptor.width, this.m_maxFogWidth);
			renderTextureDescriptor.height = Mathf.Min(cameraTargetDescriptor.height, this.m_maxFogHeight);
			renderTextureDescriptor.colorFormat = RenderTextureFormat.ARGB32;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_fogScatteringRT, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_AzureFogScatteringData");
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A9DC File Offset: 0x00008BDC
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this.m_fogRendererMaterial == null || this.m_fogComputationMaterial == null)
			{
				return;
			}
			this.m_camera = renderingData.cameraData.camera;
			if (this.m_camera == null)
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get("Azure Fog Scattering");
			this.m_camera.CalculateFrustumCorners(this.m_viewRect, this.m_camera.farClipPlane, this.m_camera.stereoActiveEye, this.m_frustumCorners);
			this.m_frustumCornersArray = Matrix4x4.identity;
			this.m_frustumCornersArray.SetRow(0, this.m_camera.transform.TransformVector(this.m_frustumCorners[0]));
			this.m_frustumCornersArray.SetRow(2, this.m_camera.transform.TransformVector(this.m_frustumCorners[1]));
			this.m_frustumCornersArray.SetRow(3, this.m_camera.transform.TransformVector(this.m_frustumCorners[2]));
			this.m_frustumCornersArray.SetRow(1, this.m_camera.transform.TransformVector(this.m_frustumCorners[3]));
			this.m_fogComputationMaterial.SetMatrix(AzureShaderUniforms.FrustumCornersMatrix, this.m_frustumCornersArray);
			this.m_fogRendererMaterial.SetMatrix(AzureShaderUniforms.FrustumCornersMatrix, this.m_frustumCornersArray);
			Blitter.BlitCameraTexture(commandBuffer, this.m_sourceColor, this.m_sourceCopy, 0f, false);
			Blitter.BlitCameraTexture(commandBuffer, this.m_sourceCopy, this.m_fogScatteringRT, this.m_fogComputationMaterial, 0);
			commandBuffer.SetGlobalTexture(AzureShaderUniforms.FogScatteringDataTexture, this.m_fogScatteringRT.nameID);
			CoreUtils.SetRenderTarget(commandBuffer, this.m_sourceColor, renderingData.cameraData.renderer.cameraDepthTargetHandle, 0, CubemapFace.Unknown, -1);
			Vector2 vector = (this.m_sourceCopy.useScaling ? new Vector2(this.m_sourceCopy.rtHandleProperties.rtHandleScale.x, this.m_sourceCopy.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			Blitter.BlitTexture(commandBuffer, this.m_sourceCopy, vector, this.m_fogRendererMaterial, 0);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000AC1A File Offset: 0x00008E1A
		public void Dispose()
		{
			RTHandle sourceCopy = this.m_sourceCopy;
			if (sourceCopy != null)
			{
				sourceCopy.Release();
			}
			this.m_sourceCopy = null;
			RTHandle fogScatteringRT = this.m_fogScatteringRT;
			if (fogScatteringRT != null)
			{
				fogScatteringRT.Release();
			}
			this.m_fogScatteringRT = null;
		}

		// Token: 0x040001C9 RID: 457
		private Camera m_camera;

		// Token: 0x040001CA RID: 458
		[SerializeField]
		private Material m_fogRendererMaterial;

		// Token: 0x040001CB RID: 459
		[SerializeField]
		private Material m_fogComputationMaterial;

		// Token: 0x040001CC RID: 460
		private RTHandle m_fogScatteringRT;

		// Token: 0x040001CD RID: 461
		private Vector3[] m_frustumCorners = new Vector3[4];

		// Token: 0x040001CE RID: 462
		private Rect m_viewRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040001CF RID: 463
		private Matrix4x4 m_frustumCornersArray = Matrix4x4.identity;

		// Token: 0x040001D0 RID: 464
		private RTHandle m_sourceColor;

		// Token: 0x040001D1 RID: 465
		private RTHandle m_sourceCopy;

		// Token: 0x040001D2 RID: 466
		private int m_maxFogWidth;

		// Token: 0x040001D3 RID: 467
		private int m_maxFogHeight;
	}
}
