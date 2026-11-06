using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200001B RID: 27
	public interface IJsonAdapter<TValue> : IJsonAdapter
	{
		// Token: 0x0600003B RID: 59
		void Serialize(in JsonSerializationContext<TValue> context, TValue value);

		// Token: 0x0600003C RID: 60
		TValue Deserialize(in JsonDeserializationContext<TValue> context);
	}
}
