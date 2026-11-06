using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000108 RID: 264
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06000D8A RID: 3466 RVA: 0x00035D86 File Offset: 0x00033F86
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00035D95 File Offset: 0x00033F95
		public override BsonType Type { get; }

		// Token: 0x04000437 RID: 1079
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x04000438 RID: 1080
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
