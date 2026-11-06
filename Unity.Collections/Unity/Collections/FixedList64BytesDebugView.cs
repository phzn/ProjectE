using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000055 RID: 85
	internal sealed class FixedList64BytesDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000849F File Offset: 0x0000669F
		public FixedList64BytesDebugView(FixedList64Bytes<T> list)
		{
			this.m_List = list;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002BD RID: 701 RVA: 0x000084AE File Offset: 0x000066AE
		public T[] Items
		{
			get
			{
				return this.m_List.ToArray();
			}
		}

		// Token: 0x040000C9 RID: 201
		private FixedList64Bytes<T> m_List;
	}
}
