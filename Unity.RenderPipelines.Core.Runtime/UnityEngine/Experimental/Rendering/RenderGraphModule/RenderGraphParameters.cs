using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000010 RID: 16
	public struct RenderGraphParameters
	{
		// Token: 0x0400005D RID: 93
		public string executionName;

		// Token: 0x0400005E RID: 94
		public int currentFrameIndex;

		// Token: 0x0400005F RID: 95
		public bool rendererListCulling;

		// Token: 0x04000060 RID: 96
		public ScriptableRenderContext scriptableRenderContext;

		// Token: 0x04000061 RID: 97
		public CommandBuffer commandBuffer;
	}
}
