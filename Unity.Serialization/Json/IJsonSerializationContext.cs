using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000022 RID: 34
	public interface IJsonSerializationContext
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600006F RID: 111
		JsonWriter Writer { get; }

		// Token: 0x06000070 RID: 112
		void ContinueVisitation();

		// Token: 0x06000071 RID: 113
		void ContinueVisitationWithoutAdapters();

		// Token: 0x06000072 RID: 114
		void SerializeValue<T>(T value);

		// Token: 0x06000073 RID: 115
		void SerializeValue<T>(string key, T value);
	}
}
