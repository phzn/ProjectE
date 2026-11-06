using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000112 RID: 274
	internal sealed class UnsafeHashSetDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x000235AD File Offset: 0x000217AD
		public UnsafeHashSetDebuggerTypeProxy(UnsafeHashSet<T> data)
		{
			this.Data = data.m_Data;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x000235C4 File Offset: 0x000217C4
		public List<T> Items
		{
			get
			{
				List<T> list = new List<T>();
				using (NativeArray<T> keyArray = this.Data.GetKeyArray(Allocator.Temp))
				{
					for (int i = 0; i < keyArray.Length; i++)
					{
						list.Add(keyArray[i]);
					}
				}
				return list;
			}
		}

		// Token: 0x040004C5 RID: 1221
		private HashMapHelper<T> Data;
	}
}
