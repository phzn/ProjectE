using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000127 RID: 295
	[DebuggerDisplay("Key = {Key}, Value = {Value}")]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct KeyValue<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00025760 File Offset: 0x00023960
		public static KeyValue<TKey, TValue> Null
		{
			get
			{
				return new KeyValue<TKey, TValue>
				{
					m_Index = -1
				};
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00025780 File Offset: 0x00023980
		public unsafe TKey Key
		{
			get
			{
				if (this.m_Index != -1)
				{
					return UnsafeUtility.ReadArrayElement<TKey>((void*)this.m_Buffer->keys, this.m_Index);
				}
				return default(TKey);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000257B6 File Offset: 0x000239B6
		public unsafe ref TValue Value
		{
			get
			{
				return UnsafeUtility.AsRef<TValue>((void*)(this.m_Buffer->values + UnsafeUtility.SizeOf<TValue>() * this.m_Index));
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000257D8 File Offset: 0x000239D8
		public unsafe bool GetKeyValue(out TKey key, out TValue value)
		{
			if (this.m_Index != -1)
			{
				key = UnsafeUtility.ReadArrayElement<TKey>((void*)this.m_Buffer->keys, this.m_Index);
				value = UnsafeUtility.ReadArrayElement<TValue>((void*)this.m_Buffer->values, this.m_Index);
				return true;
			}
			key = default(TKey);
			value = default(TValue);
			return false;
		}

		// Token: 0x040004FA RID: 1274
		internal unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x040004FB RID: 1275
		internal int m_Index;

		// Token: 0x040004FC RID: 1276
		internal int m_Next;
	}
}
