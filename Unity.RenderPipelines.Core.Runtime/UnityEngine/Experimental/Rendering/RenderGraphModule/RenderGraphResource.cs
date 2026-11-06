using System;
using System.Diagnostics;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200002B RID: 43
	[DebuggerDisplay("Resource ({GetType().Name}:{GetName()})")]
	internal abstract class RenderGraphResource<DescType, ResType> : IRenderGraphResource where DescType : struct where ResType : class
	{
		// Token: 0x060001CB RID: 459 RVA: 0x000092E2 File Offset: 0x000074E2
		public override void Reset(IRenderGraphResourcePool pool)
		{
			base.Reset(pool);
			this.graphicsResource = default(ResType);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000092F7 File Offset: 0x000074F7
		public override bool IsCreated()
		{
			return this.graphicsResource != null;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009307 File Offset: 0x00007507
		public override void ReleaseGraphicsResource()
		{
			this.graphicsResource = default(ResType);
		}

		// Token: 0x040000EE RID: 238
		public DescType desc;

		// Token: 0x040000EF RID: 239
		public ResType graphicsResource;
	}
}
