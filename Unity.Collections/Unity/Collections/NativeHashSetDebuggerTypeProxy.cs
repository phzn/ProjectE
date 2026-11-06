using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000097 RID: 151
	internal sealed class NativeHashSetDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x00017DA4 File Offset: 0x00015FA4
		public NativeHashSetDebuggerTypeProxy(NativeHashSet<T> data)
		{
			this.Data = data.m_Data;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00017DB8 File Offset: 0x00015FB8
		public unsafe List<T> Items
		{
			get
			{
				if (this.Data == null)
				{
					return null;
				}
				List<T> list = new List<T>();
				using (NativeArray<T> keyArray = this.Data->GetKeyArray(Allocator.Temp))
				{
					for (int i = 0; i < keyArray.Length; i++)
					{
						list.Add(keyArray[i]);
					}
				}
				return list;
			}
		}

		// Token: 0x040003C2 RID: 962
		private unsafe HashMapHelper<T>* Data;
	}
}
