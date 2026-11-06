using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000061 RID: 97
	internal sealed class FixedList4096BytesDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x0000A4D3 File Offset: 0x000086D3
		public FixedList4096BytesDebugView(FixedList4096Bytes<T> list)
		{
			this.m_List = list;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000A4E2 File Offset: 0x000086E2
		public T[] Items
		{
			get
			{
				return this.m_List.ToArray();
			}
		}

		// Token: 0x040000D5 RID: 213
		private FixedList4096Bytes<T> m_List;
	}
}
