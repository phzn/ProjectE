using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000025 RID: 37
	public interface IJsonDeserializationContext
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000085 RID: 133
		SerializedValueView SerializedValue { get; }

		// Token: 0x06000086 RID: 134
		object GetInstance();

		// Token: 0x06000087 RID: 135
		object ContinueVisitation();

		// Token: 0x06000088 RID: 136
		object ContinueVisitationWithoutAdapters();

		// Token: 0x06000089 RID: 137
		T DeserializeValue<T>(SerializedValueView view);
	}
}
