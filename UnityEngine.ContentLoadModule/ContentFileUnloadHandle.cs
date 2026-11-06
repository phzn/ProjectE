using System;

namespace Unity.Loading
{
	// Token: 0x02000004 RID: 4
	public struct ContentFileUnloadHandle
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public bool IsCompleted
		{
			get
			{
				return ContentLoadInterface.ContentFile_IsUnloadComplete(this.Id);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public bool WaitForCompletion(int timeoutMs)
		{
			return ContentLoadInterface.WaitForUnloadCompletion(this.Id, timeoutMs);
		}

		// Token: 0x04000008 RID: 8
		internal ContentFile Id;
	}
}
