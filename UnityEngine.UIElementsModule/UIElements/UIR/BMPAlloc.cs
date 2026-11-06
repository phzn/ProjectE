using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000476 RID: 1142
	internal struct BMPAlloc
	{
		// Token: 0x06002354 RID: 9044 RVA: 0x00089650 File Offset: 0x00087850
		public bool Equals(BMPAlloc other)
		{
			return this.page == other.page && this.pageLine == other.pageLine && this.bitIndex == other.bitIndex;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x00089690 File Offset: 0x00087890
		public bool IsValid()
		{
			return this.page >= 0;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000896B0 File Offset: 0x000878B0
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.page, this.pageLine, this.bitIndex);
		}

		// Token: 0x04001079 RID: 4217
		public static readonly BMPAlloc Invalid = new BMPAlloc
		{
			page = -1
		};

		// Token: 0x0400107A RID: 4218
		public int page;

		// Token: 0x0400107B RID: 4219
		public ushort pageLine;

		// Token: 0x0400107C RID: 4220
		public byte bitIndex;

		// Token: 0x0400107D RID: 4221
		public OwnedState ownedState;
	}
}
