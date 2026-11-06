using System;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x0200005C RID: 92
	public struct DownloadStatus
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000927B File Offset: 0x0000747B
		public float Percent
		{
			get
			{
				if (this.TotalBytes > 0L)
				{
					return (float)this.DownloadedBytes / (float)this.TotalBytes;
				}
				if (!this.IsDone)
				{
					return 0f;
				}
				return 1f;
			}
		}

		// Token: 0x04000105 RID: 261
		public long TotalBytes;

		// Token: 0x04000106 RID: 262
		public long DownloadedBytes;

		// Token: 0x04000107 RID: 263
		public bool IsDone;
	}
}
