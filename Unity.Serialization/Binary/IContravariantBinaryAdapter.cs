using System;

namespace Unity.Serialization.Binary
{
	// Token: 0x0200009D RID: 157
	public interface IContravariantBinaryAdapter<in TValue> : IBinaryAdapter
	{
		// Token: 0x060003D4 RID: 980
		void Serialize(IBinarySerializationContext context, TValue value);

		// Token: 0x060003D5 RID: 981
		object Deserialize(IBinaryDeserializationContext context);
	}
}
