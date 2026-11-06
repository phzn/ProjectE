using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000C0 RID: 192
	internal sealed class NativeRingQueueDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000909 RID: 2313 RVA: 0x0001B15E File Offset: 0x0001935E
		public NativeRingQueueDebugView(NativeRingQueue<T> data)
		{
			this.Data = data.m_RingQueue;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0001B174 File Offset: 0x00019374
		public unsafe T[] Items
		{
			get
			{
				T[] array = new T[this.Data->Length];
				int read = this.Data->m_Read;
				int capacity = this.Data->m_Capacity;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.Data->Ptr[(IntPtr)((read + i) % capacity) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
				return array;
			}
		}

		// Token: 0x040003EE RID: 1006
		private unsafe UnsafeRingQueue<T>* Data;
	}
}
