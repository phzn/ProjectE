using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000017 RID: 23
	public struct RenderGraphBuilder : IDisposable
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00006DA5 File Offset: 0x00004FA5
		public TextureHandle UseColorBuffer(in TextureHandle input, int index)
		{
			this.CheckResource(in input.handle, true);
			this.m_Resources.IncrementWriteCount(in input.handle);
			this.m_RenderPass.SetColorBuffer(input, index);
			return input;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public TextureHandle UseDepthBuffer(in TextureHandle input, DepthAccess flags)
		{
			this.CheckResource(in input.handle, true);
			if ((flags & DepthAccess.Write) != (DepthAccess)0)
			{
				this.m_Resources.IncrementWriteCount(in input.handle);
			}
			if ((flags & DepthAccess.Read) != (DepthAccess)0 && !this.m_Resources.IsRenderGraphResourceImported(in input.handle) && this.m_Resources.TextureNeedsFallback(in input))
			{
				this.WriteTexture(in input);
			}
			this.m_RenderPass.SetDepthBuffer(input, flags);
			return input;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006E58 File Offset: 0x00005058
		public TextureHandle ReadTexture(in TextureHandle input)
		{
			this.CheckResource(in input.handle, false);
			if (!this.m_Resources.IsRenderGraphResourceImported(in input.handle) && this.m_Resources.TextureNeedsFallback(in input))
			{
				TextureResource textureResource = this.m_Resources.GetTextureResource(in input.handle);
				textureResource.desc.clearBuffer = true;
				textureResource.desc.clearColor = Color.black;
				TextureHandle textureHandle;
				if (this.m_RenderGraph.GetImportedFallback(textureResource.desc, out textureHandle))
				{
					return textureHandle;
				}
				this.WriteTexture(in input);
			}
			this.m_RenderPass.AddResourceRead(in input.handle);
			return input;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006EF7 File Offset: 0x000050F7
		public TextureHandle WriteTexture(in TextureHandle input)
		{
			this.CheckResource(in input.handle, false);
			this.m_Resources.IncrementWriteCount(in input.handle);
			this.m_RenderPass.AddResourceWrite(in input.handle);
			return input;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006F30 File Offset: 0x00005130
		public TextureHandle ReadWriteTexture(in TextureHandle input)
		{
			this.CheckResource(in input.handle, false);
			this.m_Resources.IncrementWriteCount(in input.handle);
			this.m_RenderPass.AddResourceWrite(in input.handle);
			this.m_RenderPass.AddResourceRead(in input.handle);
			return input;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006F84 File Offset: 0x00005184
		public TextureHandle CreateTransientTexture(in TextureDesc desc)
		{
			TextureHandle textureHandle = this.m_Resources.CreateTexture(in desc, this.m_RenderPass.index);
			this.m_RenderPass.AddTransientResource(in textureHandle.handle);
			return textureHandle;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006FBC File Offset: 0x000051BC
		public TextureHandle CreateTransientTexture(in TextureHandle texture)
		{
			TextureDesc textureResourceDesc = this.m_Resources.GetTextureResourceDesc(in texture.handle);
			TextureHandle textureHandle = this.m_Resources.CreateTexture(in textureResourceDesc, this.m_RenderPass.index);
			this.m_RenderPass.AddTransientResource(in textureHandle.handle);
			return textureHandle;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007007 File Offset: 0x00005207
		public RendererListHandle UseRendererList(in RendererListHandle input)
		{
			this.m_RenderPass.UseRendererList(input);
			return input;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007020 File Offset: 0x00005220
		public ComputeBufferHandle ReadComputeBuffer(in ComputeBufferHandle input)
		{
			this.CheckResource(in input.handle, false);
			this.m_RenderPass.AddResourceRead(in input.handle);
			return input;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007046 File Offset: 0x00005246
		public ComputeBufferHandle WriteComputeBuffer(in ComputeBufferHandle input)
		{
			this.CheckResource(in input.handle, false);
			this.m_RenderPass.AddResourceWrite(in input.handle);
			this.m_Resources.IncrementWriteCount(in input.handle);
			return input;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007080 File Offset: 0x00005280
		public ComputeBufferHandle CreateTransientComputeBuffer(in ComputeBufferDesc desc)
		{
			ComputeBufferHandle computeBufferHandle = this.m_Resources.CreateComputeBuffer(in desc, this.m_RenderPass.index);
			this.m_RenderPass.AddTransientResource(in computeBufferHandle.handle);
			return computeBufferHandle;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000070B8 File Offset: 0x000052B8
		public ComputeBufferHandle CreateTransientComputeBuffer(in ComputeBufferHandle computebuffer)
		{
			ComputeBufferDesc computeBufferResourceDesc = this.m_Resources.GetComputeBufferResourceDesc(in computebuffer.handle);
			ComputeBufferHandle computeBufferHandle = this.m_Resources.CreateComputeBuffer(in computeBufferResourceDesc, this.m_RenderPass.index);
			this.m_RenderPass.AddTransientResource(in computeBufferHandle.handle);
			return computeBufferHandle;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007103 File Offset: 0x00005303
		public void SetRenderFunc<PassData>(RenderFunc<PassData> renderFunc) where PassData : class, new()
		{
			((RenderGraphPass<PassData>)this.m_RenderPass).renderFunc = renderFunc;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007116 File Offset: 0x00005316
		public void EnableAsyncCompute(bool value)
		{
			this.m_RenderPass.EnableAsyncCompute(value);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007124 File Offset: 0x00005324
		public void AllowPassCulling(bool value)
		{
			this.m_RenderPass.AllowPassCulling(value);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007132 File Offset: 0x00005332
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000713B File Offset: 0x0000533B
		public void AllowRendererListCulling(bool value)
		{
			this.m_RenderPass.AllowRendererListCulling(value);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007149 File Offset: 0x00005349
		public RendererListHandle DependsOn(in RendererListHandle input)
		{
			this.m_RenderPass.UseRendererList(input);
			return input;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007162 File Offset: 0x00005362
		internal RenderGraphBuilder(RenderGraphPass renderPass, RenderGraphResourceRegistry resources, RenderGraph renderGraph)
		{
			this.m_RenderPass = renderPass;
			this.m_Resources = resources;
			this.m_RenderGraph = renderGraph;
			this.m_Disposed = false;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007180 File Offset: 0x00005380
		private void Dispose(bool disposing)
		{
			if (this.m_Disposed)
			{
				return;
			}
			this.m_RenderGraph.OnPassAdded(this.m_RenderPass);
			this.m_Disposed = true;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000071A3 File Offset: 0x000053A3
		private void CheckResource(in ResourceHandle res, bool dontCheckTransientReadWrite = false)
		{
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000071A5 File Offset: 0x000053A5
		internal void GenerateDebugData(bool value)
		{
			this.m_RenderPass.GenerateDebugData(value);
		}

		// Token: 0x0400008F RID: 143
		private RenderGraphPass m_RenderPass;

		// Token: 0x04000090 RID: 144
		private RenderGraphResourceRegistry m_Resources;

		// Token: 0x04000091 RID: 145
		private RenderGraph m_RenderGraph;

		// Token: 0x04000092 RID: 146
		private bool m_Disposed;
	}
}
