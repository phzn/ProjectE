using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010D RID: 269
	internal class BsonRegex : BsonToken
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00035E4F File Offset: 0x0003404F
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00035E57 File Offset: 0x00034057
		public BsonString Pattern { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00035E60 File Offset: 0x00034060
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x00035E68 File Offset: 0x00034068
		public BsonString Options { get; set; }

		// Token: 0x06000D9D RID: 3485 RVA: 0x00035E71 File Offset: 0x00034071
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00035E93 File Offset: 0x00034093
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
