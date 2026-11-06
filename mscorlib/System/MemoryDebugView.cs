using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x02000158 RID: 344
	internal sealed class MemoryDebugView<T>
	{
		// Token: 0x06000D66 RID: 3430 RVA: 0x00033E67 File Offset: 0x00032067
		public MemoryDebugView(Memory<T> memory)
		{
			this._memory = memory;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00033E7B File Offset: 0x0003207B
		public MemoryDebugView(ReadOnlyMemory<T> memory)
		{
			this._memory = memory;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00033E8A File Offset: 0x0003208A
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this._memory.ToArray();
			}
		}

		// Token: 0x04001280 RID: 4736
		private readonly ReadOnlyMemory<T> _memory;
	}
}
