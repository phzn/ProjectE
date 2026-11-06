using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010C RID: 268
	internal class BsonBinary : BsonValue
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00035E2D File Offset: 0x0003402D
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00035E35 File Offset: 0x00034035
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06000D98 RID: 3480 RVA: 0x00035E3E File Offset: 0x0003403E
		public BsonBinary(byte[] value, BsonBinaryType binaryType)
			: base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
