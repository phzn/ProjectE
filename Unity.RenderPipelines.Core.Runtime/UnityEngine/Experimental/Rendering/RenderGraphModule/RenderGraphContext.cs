using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200000F RID: 15
	public class RenderGraphContext
	{
		// Token: 0x04000059 RID: 89
		public ScriptableRenderContext renderContext;

		// Token: 0x0400005A RID: 90
		public CommandBuffer cmd;

		// Token: 0x0400005B RID: 91
		public RenderGraphObjectPool renderGraphPool;

		// Token: 0x0400005C RID: 92
		public RenderGraphDefaultResources defaultResources;
	}
}
