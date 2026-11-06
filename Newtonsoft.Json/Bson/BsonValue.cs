using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000109 RID: 265
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x00035DB6 File Offset: 0x00033FB6
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00035DCC File Offset: 0x00033FCC
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00035DD4 File Offset: 0x00033FD4
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400043A RID: 1082
		private readonly object _value;

		// Token: 0x0400043B RID: 1083
		private readonly BsonType _type;
	}
}
