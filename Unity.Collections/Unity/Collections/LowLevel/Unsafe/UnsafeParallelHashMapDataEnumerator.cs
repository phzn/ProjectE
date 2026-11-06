using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000128 RID: 296
	internal struct UnsafeParallelHashMapDataEnumerator
	{
		// Token: 0x06000C66 RID: 3174 RVA: 0x00025837 File Offset: 0x00023A37
		internal unsafe UnsafeParallelHashMapDataEnumerator(UnsafeParallelHashMapData* data)
		{
			this.m_Buffer = data;
			this.m_Index = -1;
			this.m_BucketIndex = 0;
			this.m_NextIndex = -1;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00025855 File Offset: 0x00023A55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool MoveNext()
		{
			return UnsafeParallelHashMapData.MoveNext(this.m_Buffer, ref this.m_BucketIndex, ref this.m_NextIndex, out this.m_Index);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00025874 File Offset: 0x00023A74
		internal void Reset()
		{
			this.m_Index = -1;
			this.m_BucketIndex = 0;
			this.m_NextIndex = -1;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002588C File Offset: 0x00023A8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal KeyValue<TKey, TValue> GetCurrent<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue>() where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
		{
			return new KeyValue<TKey, TValue>
			{
				m_Buffer = this.m_Buffer,
				m_Index = this.m_Index
			};
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000258BC File Offset: 0x00023ABC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe TKey GetCurrentKey<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey>() where TKey : struct, ValueType, IEquatable<TKey>
		{
			if (this.m_Index != -1)
			{
				return UnsafeUtility.ReadArrayElement<TKey>((void*)this.m_Buffer->keys, this.m_Index);
			}
			return default(TKey);
		}

		// Token: 0x040004FD RID: 1277
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x040004FE RID: 1278
		internal int m_Index;

		// Token: 0x040004FF RID: 1279
		internal int m_BucketIndex;

		// Token: 0x04000500 RID: 1280
		internal int m_NextIndex;
	}
}
