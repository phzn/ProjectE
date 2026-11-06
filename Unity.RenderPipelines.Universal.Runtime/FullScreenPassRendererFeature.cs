using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x02000004 RID: 4
public class FullScreenPassRendererFeature : ScriptableRendererFeature, ISerializationCallbackReceiver
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	public override void Create()
	{
		this.m_FullScreenPass = new FullScreenPassRendererFeature.FullScreenRenderPass(base.name);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002073 File Offset: 0x00000273
	internal override bool RequireRenderingLayers(bool isDeferred, bool needsGBufferAccurateNormals, out RenderingLayerUtils.Event atEvent, out RenderingLayerUtils.MaskSize maskSize)
	{
		atEvent = RenderingLayerUtils.Event.Opaque;
		maskSize = RenderingLayerUtils.MaskSize.Bits8;
		return false;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002080 File Offset: 0x00000280
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData) || renderingData.cameraData.cameraType == CameraType.Preview || renderingData.cameraData.cameraType == CameraType.Reflection)
		{
			return;
		}
		if (this.passMaterial == null)
		{
			Debug.LogWarningFormat("The full screen feature \"{0}\" will not execute - no material is assigned. Please make sure a material is assigned for this feature on the renderer asset.", new object[] { base.name });
			return;
		}
		if (this.passIndex < 0 || this.passIndex >= this.passMaterial.passCount)
		{
			Debug.LogWarningFormat("The full screen feature \"{0}\" will not execute - the pass index is out of bounds for the material.", new object[] { base.name });
			return;
		}
		this.m_FullScreenPass.renderPassEvent = (RenderPassEvent)this.injectionPoint;
		this.m_FullScreenPass.ConfigureInput(this.requirements);
		this.m_FullScreenPass.SetupMembers(this.passMaterial, this.passIndex, this.fetchColorBuffer, this.bindDepthStencilAttachment);
		renderer.EnqueuePass(this.m_FullScreenPass);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002167 File Offset: 0x00000367
	protected override void Dispose(bool disposing)
	{
		this.m_FullScreenPass.Dispose();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002174 File Offset: 0x00000374
	private void UpgradeIfNeeded()
	{
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002176 File Offset: 0x00000376
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		if (this.m_Version == FullScreenPassRendererFeature.Version.Uninitialised)
		{
			this.m_Version = FullScreenPassRendererFeature.Version.AddFetchColorBufferCheckbox;
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002188 File Offset: 0x00000388
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		if (this.m_Version == FullScreenPassRendererFeature.Version.Uninitialised)
		{
			this.m_Version = FullScreenPassRendererFeature.Version.Initial;
		}
		this.UpgradeIfNeeded();
	}

	// Token: 0x04000001 RID: 1
	public FullScreenPassRendererFeature.InjectionPoint injectionPoint = FullScreenPassRendererFeature.InjectionPoint.AfterRenderingPostProcessing;

	// Token: 0x04000002 RID: 2
	public bool fetchColorBuffer = true;

	// Token: 0x04000003 RID: 3
	public ScriptableRenderPassInput requirements;

	// Token: 0x04000004 RID: 4
	public Material passMaterial;

	// Token: 0x04000005 RID: 5
	internal bool showAdditionalProperties;

	// Token: 0x04000006 RID: 6
	public int passIndex;

	// Token: 0x04000007 RID: 7
	public bool bindDepthStencilAttachment;

	// Token: 0x04000008 RID: 8
	private FullScreenPassRendererFeature.FullScreenRenderPass m_FullScreenPass;

	// Token: 0x04000009 RID: 9
	[SerializeField]
	[HideInInspector]
	private FullScreenPassRendererFeature.Version m_Version = FullScreenPassRendererFeature.Version.Uninitialised;

	// Token: 0x02000142 RID: 322
	public enum InjectionPoint
	{
		// Token: 0x04000929 RID: 2345
		BeforeRenderingTransparents = 450,
		// Token: 0x0400092A RID: 2346
		BeforeRenderingPostProcessing = 550,
		// Token: 0x0400092B RID: 2347
		AfterRenderingPostProcessing = 600
	}

	// Token: 0x02000143 RID: 323
	internal class FullScreenRenderPass : ScriptableRenderPass
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x00046DF8 File Offset: 0x00044FF8
		public FullScreenRenderPass(string passName)
		{
			base.profilingSampler = new ProfilingSampler(passName);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00046E0C File Offset: 0x0004500C
		public void SetupMembers(Material material, int passIndex, bool copyActiveColor, bool bindDepthStencilAttachment)
		{
			this.m_Material = material;
			this.m_PassIndex = passIndex;
			this.m_CopyActiveColor = copyActiveColor;
			this.m_BindDepthStencilAttachment = bindDepthStencilAttachment;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00046E2B File Offset: 0x0004502B
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			base.ResetTarget();
			if (this.m_CopyActiveColor)
			{
				this.ReAllocate(renderingData.cameraData.cameraTargetDescriptor);
			}
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00046E4C File Offset: 0x0004504C
		internal void ReAllocate(RenderTextureDescriptor desc)
		{
			desc.msaaSamples = 1;
			desc.depthBufferBits = 0;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_CopiedColor, in desc, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_FullscreenPassColorCopy");
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00046E7A File Offset: 0x0004507A
		public void Dispose()
		{
			RTHandle copiedColor = this.m_CopiedColor;
			if (copiedColor == null)
			{
				return;
			}
			copiedColor.Release();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00046E8C File Offset: 0x0004508C
		private static void ExecuteCopyColorPass(CommandBuffer cmd, RTHandle sourceTexture)
		{
			Blitter.BlitTexture(cmd, sourceTexture, new Vector4(1f, 1f, 0f, 0f), 0f, false);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00046EB4 File Offset: 0x000450B4
		private static void ExecuteMainPass(CommandBuffer cmd, RTHandle sourceTexture, Material material, int passIndex)
		{
			FullScreenPassRendererFeature.FullScreenRenderPass.s_SharedPropertyBlock.Clear();
			if (sourceTexture != null)
			{
				FullScreenPassRendererFeature.FullScreenRenderPass.s_SharedPropertyBlock.SetTexture(ShaderPropertyId.blitTexture, sourceTexture);
			}
			FullScreenPassRendererFeature.FullScreenRenderPass.s_SharedPropertyBlock.SetVector(ShaderPropertyId.blitScaleBias, new Vector4(1f, 1f, 0f, 0f));
			cmd.DrawProcedural(Matrix4x4.identity, material, passIndex, MeshTopology.Triangles, 3, 1, FullScreenPassRendererFeature.FullScreenRenderPass.s_SharedPropertyBlock);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00046F20 File Offset: 0x00045120
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, base.profilingSampler))
			{
				if (this.m_CopyActiveColor)
				{
					CoreUtils.SetRenderTarget(commandBuffer, this.m_CopiedColor, ClearFlag.None, 0, CubemapFace.Unknown, -1);
					FullScreenPassRendererFeature.FullScreenRenderPass.ExecuteCopyColorPass(commandBuffer, ptr.renderer.cameraColorTargetHandle);
				}
				if (this.m_BindDepthStencilAttachment)
				{
					CoreUtils.SetRenderTarget(commandBuffer, ptr.renderer.cameraColorTargetHandle, ptr.renderer.cameraDepthTargetHandle, 0, CubemapFace.Unknown, -1);
				}
				else
				{
					CoreUtils.SetRenderTarget(commandBuffer, ptr.renderer.cameraColorTargetHandle, ClearFlag.None, 0, CubemapFace.Unknown, -1);
				}
				FullScreenPassRendererFeature.FullScreenRenderPass.ExecuteMainPass(commandBuffer, this.m_CopyActiveColor ? this.m_CopiedColor : null, this.m_Material, this.m_PassIndex);
			}
		}

		// Token: 0x0400092C RID: 2348
		private Material m_Material;

		// Token: 0x0400092D RID: 2349
		private int m_PassIndex;

		// Token: 0x0400092E RID: 2350
		private bool m_CopyActiveColor;

		// Token: 0x0400092F RID: 2351
		private bool m_BindDepthStencilAttachment;

		// Token: 0x04000930 RID: 2352
		private RTHandle m_CopiedColor;

		// Token: 0x04000931 RID: 2353
		private static MaterialPropertyBlock s_SharedPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x02000144 RID: 324
	private enum Version
	{
		// Token: 0x04000933 RID: 2355
		Uninitialised = -1,
		// Token: 0x04000934 RID: 2356
		Initial,
		// Token: 0x04000935 RID: 2357
		AddFetchColorBufferCheckbox,
		// Token: 0x04000936 RID: 2358
		Count,
		// Token: 0x04000937 RID: 2359
		Latest = 1
	}
}
