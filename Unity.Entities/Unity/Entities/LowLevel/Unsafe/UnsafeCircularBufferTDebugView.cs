using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities.LowLevel.Unsafe
{
	// Token: 0x020004CE RID: 1230
	internal sealed class UnsafeCircularBufferTDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06001DB6 RID: 7606 RVA: 0x00067610 File Offset: 0x00065810
		public UnsafeCircularBufferTDebugView(UnsafeCircularBuffer<T> buffer)
		{
			this.m_Buffer = buffer;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0006761F File Offset: 0x0006581F
		public T[] Items
		{
			get
			{
				return this.m_Buffer.ToArray();
			}
		}

		// Token: 0x04000AA1 RID: 2721
		private UnsafeCircularBuffer<T> m_Buffer;
	}
}
