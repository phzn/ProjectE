using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000023 RID: 35
	internal abstract class IRenderGraphResourcePool
	{
		// Token: 0x06000163 RID: 355
		public abstract void PurgeUnusedResources(int currentFrameIndex);

		// Token: 0x06000164 RID: 356
		public abstract void Cleanup();

		// Token: 0x06000165 RID: 357
		public abstract void CheckFrameAllocation(bool onException, int frameIndex);

		// Token: 0x06000166 RID: 358
		public abstract void LogResources(RenderGraphLogger logger);
	}
}
