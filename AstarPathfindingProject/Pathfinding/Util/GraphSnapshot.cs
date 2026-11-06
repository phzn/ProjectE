using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x02000291 RID: 657
	public struct GraphSnapshot : IGraphSnapshot, IDisposable
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x00063108 File Offset: 0x00061308
		internal GraphSnapshot(List<IGraphSnapshot> inner)
		{
			this.inner = inner;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00063114 File Offset: 0x00061314
		public void Restore(IGraphUpdateContext ctx)
		{
			for (int i = 0; i < this.inner.Count; i++)
			{
				this.inner[i].Restore(ctx);
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0006314C File Offset: 0x0006134C
		public void Dispose()
		{
			if (this.inner != null)
			{
				for (int i = 0; i < this.inner.Count; i++)
				{
					this.inner[i].Dispose();
				}
				this.inner = null;
			}
		}

		// Token: 0x04000BE9 RID: 3049
		private List<IGraphSnapshot> inner;
	}
}
