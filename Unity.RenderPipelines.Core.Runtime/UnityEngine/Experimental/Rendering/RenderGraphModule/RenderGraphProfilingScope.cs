using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000016 RID: 22
	public struct RenderGraphProfilingScope : IDisposable
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00006D58 File Offset: 0x00004F58
		public RenderGraphProfilingScope(RenderGraph renderGraph, ProfilingSampler sampler)
		{
			this.m_RenderGraph = renderGraph;
			this.m_Sampler = sampler;
			this.m_Disposed = false;
			renderGraph.BeginProfilingSampler(sampler);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006D76 File Offset: 0x00004F76
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006D7F File Offset: 0x00004F7F
		private void Dispose(bool disposing)
		{
			if (this.m_Disposed)
			{
				return;
			}
			if (disposing)
			{
				this.m_RenderGraph.EndProfilingSampler(this.m_Sampler);
			}
			this.m_Disposed = true;
		}

		// Token: 0x0400008C RID: 140
		private bool m_Disposed;

		// Token: 0x0400008D RID: 141
		private ProfilingSampler m_Sampler;

		// Token: 0x0400008E RID: 142
		private RenderGraph m_RenderGraph;
	}
}
