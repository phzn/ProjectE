using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200011B RID: 283
	internal sealed class UnsafeListTDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x000242DE File Offset: 0x000224DE
		public UnsafeListTDebugView(UnsafeList<T> data)
		{
			this.Data = data;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000242F0 File Offset: 0x000224F0
		public unsafe T[] Items
		{
			get
			{
				T[] array = new T[this.Data.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.Data.Ptr[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
				return array;
			}
		}

		// Token: 0x040004DA RID: 1242
		private UnsafeList<T> Data;
	}
}
