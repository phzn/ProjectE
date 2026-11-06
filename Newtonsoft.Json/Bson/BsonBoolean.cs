using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010A RID: 266
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x00035DDC File Offset: 0x00033FDC
		private BsonBoolean(bool value)
			: base(value, BsonType.Boolean)
		{
		}

		// Token: 0x0400043C RID: 1084
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x0400043D RID: 1085
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
