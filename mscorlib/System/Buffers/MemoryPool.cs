using System;

namespace System.Buffers
{
	// Token: 0x02000AE3 RID: 2787
	public abstract class MemoryPool<T> : IDisposable
	{
		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06006319 RID: 25369 RVA: 0x0014B669 File Offset: 0x00149869
		public static MemoryPool<T> Shared
		{
			get
			{
				return MemoryPool<T>.s_shared;
			}
		}

		// Token: 0x0600631A RID: 25370
		public abstract IMemoryOwner<T> Rent(int minBufferSize = -1);

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x0600631B RID: 25371
		public abstract int MaxBufferSize { get; }

		// Token: 0x0600631D RID: 25373 RVA: 0x0014B670 File Offset: 0x00149870
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600631E RID: 25374
		protected abstract void Dispose(bool disposing);

		// Token: 0x04003A5B RID: 14939
		private static readonly MemoryPool<T> s_shared = new ArrayMemoryPool<T>();
	}
}
