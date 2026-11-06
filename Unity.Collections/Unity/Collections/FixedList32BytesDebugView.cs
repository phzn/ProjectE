using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000051 RID: 81
	internal sealed class FixedList32BytesDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x0600026E RID: 622 RVA: 0x000079E3 File Offset: 0x00005BE3
		public FixedList32BytesDebugView(FixedList32Bytes<T> list)
		{
			this.m_List = list;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600026F RID: 623 RVA: 0x000079F2 File Offset: 0x00005BF2
		public T[] Items
		{
			get
			{
				return this.m_List.ToArray();
			}
		}

		// Token: 0x040000C5 RID: 197
		private FixedList32Bytes<T> m_List;
	}
}
