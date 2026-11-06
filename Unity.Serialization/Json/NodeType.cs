using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200004B RID: 75
	[Flags]
	public enum NodeType
	{
		// Token: 0x040000C7 RID: 199
		None = 0,
		// Token: 0x040000C8 RID: 200
		BeginObject = 1,
		// Token: 0x040000C9 RID: 201
		ObjectKey = 2,
		// Token: 0x040000CA RID: 202
		EndObject = 4,
		// Token: 0x040000CB RID: 203
		BeginArray = 8,
		// Token: 0x040000CC RID: 204
		EndArray = 16,
		// Token: 0x040000CD RID: 205
		String = 32,
		// Token: 0x040000CE RID: 206
		Primitive = 64,
		// Token: 0x040000CF RID: 207
		Comment = 128,
		// Token: 0x040000D0 RID: 208
		Any = -1
	}
}
