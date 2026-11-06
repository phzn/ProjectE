using System;

namespace Unity.Serialization
{
	// Token: 0x0200000E RID: 14
	internal interface ISerializedTypeProvider
	{
		// Token: 0x06000018 RID: 24
		Type GetSerializedType();

		// Token: 0x06000019 RID: 25
		int GetArrayLength();

		// Token: 0x0600001A RID: 26
		object GetDefaultObject();
	}
}
