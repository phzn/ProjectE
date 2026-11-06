using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000121 RID: 289
	internal sealed class UnsafePtrListDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x000247CE File Offset: 0x000229CE
		public UnsafePtrListDebugView(UnsafePtrList<T> data)
		{
			this.Data = data;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x000247E0 File Offset: 0x000229E0
		public unsafe T*[] Items
		{
			get
			{
				T*[] array = new T*[this.Data.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = *(IntPtr*)(this.Data.Ptr + (IntPtr)i * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*));
				}
				return array;
			}
		}

		// Token: 0x040004E6 RID: 1254
		private UnsafePtrList<T> Data;
	}
}
