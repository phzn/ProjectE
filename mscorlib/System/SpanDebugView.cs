using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x0200017F RID: 383
	internal sealed class SpanDebugView<T>
	{
		// Token: 0x06000F5E RID: 3934 RVA: 0x0003D935 File Offset: 0x0003BB35
		public SpanDebugView(Span<T> span)
		{
			this._array = span.ToArray();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0003D94A File Offset: 0x0003BB4A
		public SpanDebugView(ReadOnlySpan<T> span)
		{
			this._array = span.ToArray();
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0003D95F File Offset: 0x0003BB5F
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x040012E7 RID: 4839
		private readonly T[] _array;
	}
}
