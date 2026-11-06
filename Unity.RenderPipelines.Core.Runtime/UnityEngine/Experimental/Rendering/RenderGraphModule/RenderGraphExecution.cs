using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000011 RID: 17
	public struct RenderGraphExecution : IDisposable
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004236 File Offset: 0x00002436
		internal RenderGraphExecution(RenderGraph renderGraph)
		{
			this.renderGraph = renderGraph;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000423F File Offset: 0x0000243F
		public void Dispose()
		{
			this.renderGraph.Execute();
		}

		// Token: 0x04000062 RID: 98
		private RenderGraph renderGraph;
	}
}
