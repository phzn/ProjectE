using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Binary
{
	// Token: 0x02000099 RID: 153
	public interface IBinaryDeserializationContext
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003C5 RID: 965
		unsafe UnsafeAppendBuffer.Reader* Reader { get; }

		// Token: 0x060003C6 RID: 966
		object ContinueVisitation();

		// Token: 0x060003C7 RID: 967
		object ContinueVisitationWithoutAdapters();

		// Token: 0x060003C8 RID: 968
		T DeserializeValue<T>();
	}
}
