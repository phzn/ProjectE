using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x020000AD RID: 173
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeParallelMultiHashMapIterator<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey> where TKey : struct, ValueType
	{
		// Token: 0x06000884 RID: 2180 RVA: 0x0001A650 File Offset: 0x00018850
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetEntryIndex()
		{
			return this.EntryIndex;
		}

		// Token: 0x040003D4 RID: 980
		internal TKey key;

		// Token: 0x040003D5 RID: 981
		internal int NextEntryIndex;

		// Token: 0x040003D6 RID: 982
		internal int EntryIndex;
	}
}
