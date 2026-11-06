using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010B RID: 267
	internal class BsonString : BsonValue
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00035E03 File Offset: 0x00034003
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x00035E0B File Offset: 0x0003400B
		public int ByteCount { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00035E14 File Offset: 0x00034014
		public bool IncludeLength { get; }

		// Token: 0x06000D95 RID: 3477 RVA: 0x00035E1C File Offset: 0x0003401C
		public BsonString(object value, bool includeLength)
			: base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
