using System;
using Unity.Jobs;

namespace Pathfinding.Sync
{
	// Token: 0x0200023C RID: 572
	public struct Promise<T> : IProgress, IDisposable where T : IProgress, IDisposable
	{
		// Token: 0x06000E60 RID: 3680 RVA: 0x00059034 File Offset: 0x00057234
		public Promise(JobHandle handle, T result)
		{
			this.handle = handle;
			this.result = result;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00059044 File Offset: 0x00057244
		public bool IsCompleted
		{
			get
			{
				return this.handle.IsCompleted;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x00059051 File Offset: 0x00057251
		public float Progress
		{
			get
			{
				return this.result.Progress;
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00059064 File Offset: 0x00057264
		public T GetValue()
		{
			return this.result;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0005906C File Offset: 0x0005726C
		public T Complete()
		{
			this.handle.Complete();
			return this.result;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0005907F File Offset: 0x0005727F
		public void Dispose()
		{
			this.Complete();
			this.result.Dispose();
		}

		// Token: 0x04000AD2 RID: 2770
		public JobHandle handle;

		// Token: 0x04000AD3 RID: 2771
		private T result;
	}
}
