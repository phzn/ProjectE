using System;
using System.Diagnostics;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000030 RID: 48
	[DebuggerDisplay("TextureResource ({desc.name})")]
	internal class TextureResource : RenderGraphResource<TextureDesc, RTHandle>
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00009608 File Offset: 0x00007808
		public override string GetName()
		{
			if (!this.imported || this.shared)
			{
				return this.desc.name;
			}
			if (this.graphicsResource == null)
			{
				return "null resource";
			}
			return this.graphicsResource.name;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009640 File Offset: 0x00007840
		public override void CreatePooledGraphicsResource()
		{
			int hashCode = this.desc.GetHashCode();
			if (this.graphicsResource != null)
			{
				throw new InvalidOperationException(string.Format("TextureResource: Trying to create an already created resource ({0}). Resource was probably declared for writing more than once in the same pass.", this.GetName()));
			}
			TexturePool texturePool = this.m_Pool as TexturePool;
			if (!texturePool.TryGetResource(hashCode, out this.graphicsResource))
			{
				this.CreateGraphicsResource(this.desc.name);
			}
			this.cachedHash = hashCode;
			texturePool.RegisterFrameAllocation(this.cachedHash, this.graphicsResource);
			this.graphicsResource.m_Name = this.desc.name;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000096D8 File Offset: 0x000078D8
		public override void ReleasePooledGraphicsResource(int frameIndex)
		{
			if (this.graphicsResource == null)
			{
				throw new InvalidOperationException("TextureResource: Tried to release a resource (" + this.GetName() + ") that was never created. Check that there is at least one pass writing to it first.");
			}
			TexturePool texturePool = this.m_Pool as TexturePool;
			if (texturePool != null)
			{
				texturePool.ReleaseResource(this.cachedHash, this.graphicsResource, frameIndex);
				texturePool.UnregisterFrameAllocation(this.cachedHash, this.graphicsResource);
			}
			this.Reset(null);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009744 File Offset: 0x00007944
		public override void CreateGraphicsResource(string name = "")
		{
			if (name == "")
			{
				name = string.Format("RenderGraphTexture_{0}", TextureResource.m_TextureCreationIndex++);
			}
			switch (this.desc.sizeMode)
			{
			case TextureSizeMode.Explicit:
				this.graphicsResource = RTHandles.Alloc(this.desc.width, this.desc.height, this.desc.slices, this.desc.depthBufferBits, this.desc.colorFormat, this.desc.filterMode, this.desc.wrapMode, this.desc.dimension, this.desc.enableRandomWrite, this.desc.useMipMap, this.desc.autoGenerateMips, this.desc.isShadowMap, this.desc.anisoLevel, this.desc.mipMapBias, this.desc.msaaSamples, this.desc.bindTextureMS, this.desc.useDynamicScale, this.desc.memoryless, this.desc.vrUsage, name);
				return;
			case TextureSizeMode.Scale:
				this.graphicsResource = RTHandles.Alloc(this.desc.scale, this.desc.slices, this.desc.depthBufferBits, this.desc.colorFormat, this.desc.filterMode, this.desc.wrapMode, this.desc.dimension, this.desc.enableRandomWrite, this.desc.useMipMap, this.desc.autoGenerateMips, this.desc.isShadowMap, this.desc.anisoLevel, this.desc.mipMapBias, this.desc.msaaSamples, this.desc.bindTextureMS, this.desc.useDynamicScale, this.desc.memoryless, this.desc.vrUsage, name);
				return;
			case TextureSizeMode.Functor:
				this.graphicsResource = RTHandles.Alloc(this.desc.func, this.desc.slices, this.desc.depthBufferBits, this.desc.colorFormat, this.desc.filterMode, this.desc.wrapMode, this.desc.dimension, this.desc.enableRandomWrite, this.desc.useMipMap, this.desc.autoGenerateMips, this.desc.isShadowMap, this.desc.anisoLevel, this.desc.mipMapBias, this.desc.msaaSamples, this.desc.bindTextureMS, this.desc.useDynamicScale, this.desc.memoryless, this.desc.vrUsage, name);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009A1E File Offset: 0x00007C1E
		public override void ReleaseGraphicsResource()
		{
			if (this.graphicsResource != null)
			{
				this.graphicsResource.Release();
			}
			base.ReleaseGraphicsResource();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009A39 File Offset: 0x00007C39
		public override void LogCreation(RenderGraphLogger logger)
		{
			logger.LogLine(string.Format("Created Texture: {0} (Cleared: {1})", this.desc.name, this.desc.clearBuffer), Array.Empty<object>());
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009A6B File Offset: 0x00007C6B
		public override void LogRelease(RenderGraphLogger logger)
		{
			logger.LogLine("Released Texture: " + this.desc.name, Array.Empty<object>());
		}

		// Token: 0x04000115 RID: 277
		private static int m_TextureCreationIndex;
	}
}
