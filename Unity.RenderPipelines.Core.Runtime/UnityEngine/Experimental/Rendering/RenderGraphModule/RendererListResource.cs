using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000027 RID: 39
	internal struct RendererListResource
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00009133 File Offset: 0x00007333
		internal RendererListResource(in RendererListDesc desc)
		{
			this.desc = desc;
			this.rendererList = default(RendererList);
		}

		// Token: 0x040000D9 RID: 217
		public RendererListDesc desc;

		// Token: 0x040000DA RID: 218
		public RendererList rendererList;
	}
}
