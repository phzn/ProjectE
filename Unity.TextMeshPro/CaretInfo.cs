using System;

namespace TMPro
{
	// Token: 0x02000070 RID: 112
	public struct CaretInfo
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x0003646C File Offset: 0x0003466C
		public CaretInfo(int index, CaretPosition position)
		{
			this.index = index;
			this.position = position;
		}

		// Token: 0x04000555 RID: 1365
		public int index;

		// Token: 0x04000556 RID: 1366
		public CaretPosition position;
	}
}
