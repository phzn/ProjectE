using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200008F RID: 143
	[DebuggerDisplay("Key = {Key}, Value = {Value}")]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct KVPair<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00017650 File Offset: 0x00015850
		public static KVPair<TKey, TValue> Null
		{
			get
			{
				return new KVPair<TKey, TValue>
				{
					m_Index = -1
				};
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00017670 File Offset: 0x00015870
		public unsafe TKey Key
		{
			get
			{
				if (this.m_Index != -1)
				{
					return this.m_Data->Keys[(IntPtr)this.m_Index * (IntPtr)sizeof(TKey) / (IntPtr)sizeof(TKey)];
				}
				return default(TKey);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x000176AF File Offset: 0x000158AF
		public unsafe ref TValue Value
		{
			get
			{
				return UnsafeUtility.AsRef<TValue>((void*)(this.m_Data->Ptr + sizeof(TValue) * this.m_Index));
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x000176D0 File Offset: 0x000158D0
		public unsafe bool GetKeyValue(out TKey key, out TValue value)
		{
			if (this.m_Index != -1)
			{
				key = this.m_Data->Keys[(IntPtr)this.m_Index * (IntPtr)sizeof(TKey) / (IntPtr)sizeof(TKey)];
				value = UnsafeUtility.ReadArrayElement<TValue>((void*)this.m_Data->Ptr, this.m_Index);
				return true;
			}
			key = default(TKey);
			value = default(TValue);
			return false;
		}

		// Token: 0x040003B8 RID: 952
		internal unsafe HashMapHelper<TKey>* m_Data;

		// Token: 0x040003B9 RID: 953
		internal int m_Index;

		// Token: 0x040003BA RID: 954
		internal int m_Next;
	}
}
