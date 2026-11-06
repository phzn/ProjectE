using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities
{
	// Token: 0x0200020A RID: 522
	internal sealed class DynamicBufferDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000C44 RID: 3140 RVA: 0x0003877E File Offset: 0x0003697E
		public DynamicBufferDebugView(DynamicBuffer<T> source)
		{
			this._buffer = source;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00038790 File Offset: 0x00036990
		public T[] Items
		{
			get
			{
				return this._buffer.AsNativeArray().ToArray();
			}
		}

		// Token: 0x0400055D RID: 1373
		private DynamicBuffer<T> _buffer;
	}
}
