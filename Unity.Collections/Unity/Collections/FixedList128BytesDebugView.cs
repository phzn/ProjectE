using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000059 RID: 89
	internal sealed class FixedList128BytesDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00008F5B File Offset: 0x0000715B
		public FixedList128BytesDebugView(FixedList128Bytes<T> list)
		{
			this.m_List = list;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00008F6A File Offset: 0x0000716A
		public T[] Items
		{
			get
			{
				return this.m_List.ToArray();
			}
		}

		// Token: 0x040000CD RID: 205
		private FixedList128Bytes<T> m_List;
	}
}
