using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Binary
{
	// Token: 0x02000097 RID: 151
	public interface IBinarySerializationContext
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003BC RID: 956
		unsafe UnsafeAppendBuffer* Writer { get; }

		// Token: 0x060003BD RID: 957
		void ContinueVisitation();

		// Token: 0x060003BE RID: 958
		void ContinueVisitationWithoutAdapters();

		// Token: 0x060003BF RID: 959
		void SerializeValue<T>(T value);
	}
}
