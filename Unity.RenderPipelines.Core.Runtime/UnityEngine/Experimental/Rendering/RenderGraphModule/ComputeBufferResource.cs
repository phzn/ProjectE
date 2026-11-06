using System;
using System.Diagnostics;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000021 RID: 33
	[DebuggerDisplay("ComputeBufferResource ({desc.name})")]
	internal class ComputeBufferResource : RenderGraphResource<ComputeBufferDesc, ComputeBuffer>
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00007BC8 File Offset: 0x00005DC8
		public override string GetName()
		{
			if (this.imported)
			{
				return "ImportedComputeBuffer";
			}
			return this.desc.name;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public override void CreatePooledGraphicsResource()
		{
			int hashCode = this.desc.GetHashCode();
			if (this.graphicsResource != null)
			{
				throw new InvalidOperationException(string.Format("ComputeBufferResource: Trying to create an already created resource ({0}). Resource was probably declared for writing more than once in the same pass.", this.GetName()));
			}
			ComputeBufferPool computeBufferPool = this.m_Pool as ComputeBufferPool;
			if (!computeBufferPool.TryGetResource(hashCode, out this.graphicsResource))
			{
				this.CreateGraphicsResource(this.desc.name);
			}
			this.cachedHash = hashCode;
			computeBufferPool.RegisterFrameAllocation(this.cachedHash, this.graphicsResource);
			this.graphicsResource.name = this.desc.name;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007C7C File Offset: 0x00005E7C
		public override void ReleasePooledGraphicsResource(int frameIndex)
		{
			if (this.graphicsResource == null)
			{
				throw new InvalidOperationException("ComputeBufferResource: Tried to release a resource (" + this.GetName() + ") that was never created. Check that there is at least one pass writing to it first.");
			}
			ComputeBufferPool computeBufferPool = this.m_Pool as ComputeBufferPool;
			if (computeBufferPool != null)
			{
				computeBufferPool.ReleaseResource(this.cachedHash, this.graphicsResource, frameIndex);
				computeBufferPool.UnregisterFrameAllocation(this.cachedHash, this.graphicsResource);
			}
			this.Reset(null);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007CE8 File Offset: 0x00005EE8
		public override void CreateGraphicsResource(string name = "")
		{
			this.graphicsResource = new ComputeBuffer(this.desc.count, this.desc.stride, this.desc.type);
			this.graphicsResource.name = ((name == "") ? string.Format("RenderGraphComputeBuffer_{0}_{1}_{2}", this.desc.count, this.desc.stride, this.desc.type) : name);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007D76 File Offset: 0x00005F76
		public override void ReleaseGraphicsResource()
		{
			if (this.graphicsResource != null)
			{
				this.graphicsResource.Release();
			}
			base.ReleaseGraphicsResource();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007D91 File Offset: 0x00005F91
		public override void LogCreation(RenderGraphLogger logger)
		{
			logger.LogLine("Created ComputeBuffer: " + this.desc.name, Array.Empty<object>());
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007DB3 File Offset: 0x00005FB3
		public override void LogRelease(RenderGraphLogger logger)
		{
			logger.LogLine("Released ComputeBuffer: " + this.desc.name, Array.Empty<object>());
		}
	}
}
