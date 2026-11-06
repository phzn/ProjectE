using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200013D RID: 317
	internal sealed class UnsafeRingQueueDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x000298B7 File Offset: 0x00027AB7
		public UnsafeRingQueueDebugView(UnsafeRingQueue<T> data)
		{
			this.Data = data;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x000298C8 File Offset: 0x00027AC8
		public unsafe T[] Items
		{
			get
			{
				T[] array = new T[this.Data.Length];
				int read = this.Data.m_Read;
				int capacity = this.Data.m_Capacity;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.Data.Ptr[(IntPtr)((read + i) % capacity) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
				return array;
			}
		}

		// Token: 0x04000523 RID: 1315
		private UnsafeRingQueue<T> Data;
	}
}
