using System;
using System.Diagnostics;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200001D RID: 29
	[DebuggerDisplay("RenderPass: {name} (Index:{index} Async:{enableAsyncCompute})")]
	internal sealed class RenderGraphPass<PassData> : RenderGraphPass where PassData : class, new()
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00007A8E File Offset: 0x00005C8E
		public override void Execute(RenderGraphContext renderGraphContext)
		{
			base.GetExecuteDelegate<PassData>()(this.data, renderGraphContext);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007AA2 File Offset: 0x00005CA2
		public void Initialize(int passIndex, PassData passData, string passName, ProfilingSampler sampler)
		{
			base.Clear();
			base.index = passIndex;
			this.data = passData;
			base.name = passName;
			base.customSampler = sampler;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007AC7 File Offset: 0x00005CC7
		public override void Release(RenderGraphObjectPool pool)
		{
			base.Clear();
			pool.Release<PassData>(this.data);
			this.data = default(PassData);
			this.renderFunc = null;
			pool.Release<RenderGraphPass<PassData>>(this);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007AF5 File Offset: 0x00005CF5
		public override bool HasRenderFunc()
		{
			return this.renderFunc != null;
		}

		// Token: 0x040000B9 RID: 185
		internal PassData data;

		// Token: 0x040000BA RID: 186
		internal RenderFunc<PassData> renderFunc;
	}
}
