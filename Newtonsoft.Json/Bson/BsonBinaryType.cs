using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000101 RID: 257
	internal enum BsonBinaryType : byte
	{
		// Token: 0x04000418 RID: 1048
		Binary,
		// Token: 0x04000419 RID: 1049
		Function,
		// Token: 0x0400041A RID: 1050
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x0400041B RID: 1051
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x0400041C RID: 1052
		Uuid,
		// Token: 0x0400041D RID: 1053
		Md5,
		// Token: 0x0400041E RID: 1054
		UserDefined = 128
	}
}
