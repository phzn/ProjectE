using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000134 RID: 308
	internal sealed class UnsafeParallelHashSetDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000CC2 RID: 3266 RVA: 0x00026019 File Offset: 0x00024219
		public UnsafeParallelHashSetDebuggerTypeProxy(UnsafeParallelHashSet<T> data)
		{
			this.Data = data;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00026028 File Offset: 0x00024228
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

		// Token: 0x04000510 RID: 1296
		private UnsafeParallelHashSet<T> Data;
	}
}
