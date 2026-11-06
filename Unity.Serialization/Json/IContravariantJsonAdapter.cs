using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200001C RID: 28
	public interface IContravariantJsonAdapter<in TValue> : IJsonAdapter
	{
		// Token: 0x0600003D RID: 61
		void Serialize(IJsonSerializationContext context, TValue value);

		// Token: 0x0600003E RID: 62
		object Deserialize(IJsonDeserializationContext context);
	}
}
