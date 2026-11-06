using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010E RID: 270
	internal class BsonProperty
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00035E97 File Offset: 0x00034097
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00035E9F File Offset: 0x0003409F
		public BsonString Name { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00035EA8 File Offset: 0x000340A8
		// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x00035EB0 File Offset: 0x000340B0
		public BsonToken Value { get; set; }
	}
}
