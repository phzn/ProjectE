using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x0200005D RID: 93
	internal sealed class FixedList512BytesDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00009A17 File Offset: 0x00007C17
		public FixedList512BytesDebugView(FixedList512Bytes<T> list)
		{
			this.m_List = list;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00009A26 File Offset: 0x00007C26
		public T[] Items
		{
			get
			{
				return this.m_List.ToArray();
			}
		}

		// Token: 0x040000D1 RID: 209
		private FixedList512Bytes<T> m_List;
	}
}
