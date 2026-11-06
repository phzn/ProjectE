using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000073 RID: 115
	[Flags]
	internal enum JsonType
	{
		// Token: 0x0400015C RID: 348
		Undefined = 1,
		// Token: 0x0400015D RID: 349
		BeginObject = 2,
		// Token: 0x0400015E RID: 350
		EndObject = 4,
		// Token: 0x0400015F RID: 351
		BeginArray = 8,
		// Token: 0x04000160 RID: 352
		EndArray = 16,
		// Token: 0x04000161 RID: 353
		MemberSeparator = 32,
		// Token: 0x04000162 RID: 354
		ValueSeparator = 64,
		// Token: 0x04000163 RID: 355
		String = 128,
		// Token: 0x04000164 RID: 356
		Number = 256,
		// Token: 0x04000165 RID: 357
		Negative = 512,
		// Token: 0x04000166 RID: 358
		NaN = 1024,
		// Token: 0x04000167 RID: 359
		Infinity = 2048,
		// Token: 0x04000168 RID: 360
		True = 4096,
		// Token: 0x04000169 RID: 361
		False = 8192,
		// Token: 0x0400016A RID: 362
		Null = 16384,
		// Token: 0x0400016B RID: 363
		EOF = 32768,
		// Token: 0x0400016C RID: 364
		Value = 32650
	}
}
