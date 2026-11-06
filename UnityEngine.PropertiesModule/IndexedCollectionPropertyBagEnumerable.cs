using System;

namespace Unity.Properties
{
	// Token: 0x02000039 RID: 57
	internal readonly struct IndexedCollectionPropertyBagEnumerable<TContainer>
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00005389 File Offset: 0x00003589
		public IndexedCollectionPropertyBagEnumerable(IIndexedCollectionPropertyBagEnumerator<TContainer> impl, TContainer container)
		{
			this.m_Impl = impl;
			this.m_Container = container;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000539A File Offset: 0x0000359A
		public IndexedCollectionPropertyBagEnumerator<TContainer> GetEnumerator()
		{
			return new IndexedCollectionPropertyBagEnumerator<TContainer>(this.m_Impl, this.m_Container);
		}

		// Token: 0x0400005B RID: 91
		private readonly IIndexedCollectionPropertyBagEnumerator<TContainer> m_Impl;

		// Token: 0x0400005C RID: 92
		private readonly TContainer m_Container;
	}
}
