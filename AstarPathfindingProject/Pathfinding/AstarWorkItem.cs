using System;

namespace Pathfinding
{
	// Token: 0x02000094 RID: 148
	public struct AstarWorkItem
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x00019358 File Offset: 0x00017558
		public AstarWorkItem(Func<bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = null;
			this.update = update;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00019376 File Offset: 0x00017576
		public AstarWorkItem(Func<IWorkItemContext, bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = update;
			this.update = null;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00019394 File Offset: 0x00017594
		public AstarWorkItem(Action init, Func<bool, bool> update = null)
		{
			this.init = init;
			this.initWithContext = null;
			this.update = update;
			this.updateWithContext = null;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000193B2 File Offset: 0x000175B2
		public AstarWorkItem(Action<IWorkItemContext> init, Func<IWorkItemContext, bool, bool> update = null)
		{
			this.init = null;
			this.initWithContext = init;
			this.update = null;
			this.updateWithContext = update;
		}

		// Token: 0x0400032D RID: 813
		public Action init;

		// Token: 0x0400032E RID: 814
		public Action<IWorkItemContext> initWithContext;

		// Token: 0x0400032F RID: 815
		public Func<bool, bool> update;

		// Token: 0x04000330 RID: 816
		public Func<IWorkItemContext, bool, bool> updateWithContext;
	}
}
