using System;
using System.Collections.Generic;

namespace Unity.Entities
{
	// Token: 0x02000070 RID: 112
	internal struct BlobAssetPtrHashComparer : IComparer<BlobAssetPtr>
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0000C248 File Offset: 0x0000A448
		public int Compare(BlobAssetPtr x, BlobAssetPtr y)
		{
			return x.Hash.CompareTo(y.Hash);
		}
	}
}
