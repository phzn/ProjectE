using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x020000AB RID: 171
	internal sealed class NativeParallelHashSetDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x00018CC1 File Offset: 0x00016EC1
		public NativeParallelHashSetDebuggerTypeProxy(NativeParallelHashSet<T> data)
		{
			this.Data = data;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00018CD0 File Offset: 0x00016ED0
		public List<T> Items
		{
			get
			{
				List<T> list = new List<T>();
				using (NativeArray<T> nativeArray = this.Data.ToNativeArray(Allocator.Temp))
				{
					for (int i = 0; i < nativeArray.Length; i++)
					{
						list.Add(nativeArray[i]);
					}
				}
				return list;
			}
		}

		// Token: 0x040003D3 RID: 979
		private NativeParallelHashSet<T> Data;
	}
}
