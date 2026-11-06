using System;

namespace System.Threading
{
	// Token: 0x02000363 RID: 867
	internal class ReaderWriterCount
	{
		// Token: 0x04000CA8 RID: 3240
		public long lockID;

		// Token: 0x04000CA9 RID: 3241
		public int readercount;

		// Token: 0x04000CAA RID: 3242
		public int writercount;

		// Token: 0x04000CAB RID: 3243
		public int upgradecount;

		// Token: 0x04000CAC RID: 3244
		public ReaderWriterCount next;
	}
}
