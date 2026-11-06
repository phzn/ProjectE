using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000105 RID: 261
	internal abstract class BsonToken
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000D7A RID: 3450
		public abstract BsonType Type { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00035CBA File Offset: 0x00033EBA
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x00035CC2 File Offset: 0x00033EC2
		public BsonToken Parent { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00035CCB File Offset: 0x00033ECB
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x00035CD3 File Offset: 0x00033ED3
		public int CalculatedSize { get; set; }
	}
}
