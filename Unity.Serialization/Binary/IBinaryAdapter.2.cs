using System;

namespace Unity.Serialization.Binary
{
	// Token: 0x0200009C RID: 156
	public interface IBinaryAdapter<TValue> : IBinaryAdapter
	{
		// Token: 0x060003D2 RID: 978
		void Serialize(in BinarySerializationContext<TValue> context, TValue value);

		// Token: 0x060003D3 RID: 979
		TValue Deserialize(in BinaryDeserializationContext<TValue> context);
	}
}
