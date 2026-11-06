using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x0200003A RID: 58
	internal struct IndexedCollectionPropertyBagEnumerator<TContainer> : IEnumerator<IProperty<TContainer>>, IEnumerator, IDisposable
	{
		// Token: 0x0600010D RID: 269 RVA: 0x000053AD File Offset: 0x000035AD
		internal IndexedCollectionPropertyBagEnumerator(IIndexedCollectionPropertyBagEnumerator<TContainer> impl, TContainer container)
		{
			this.m_Impl = impl;
			this.m_Container = container;
			this.m_Previous = impl.GetSharedPropertyState();
			this.m_Position = -1;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000053D1 File Offset: 0x000035D1
		public IProperty<TContainer> Current
		{
			get
			{
				return this.m_Impl.GetSharedProperty();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000053DE File Offset: 0x000035DE
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000053E8 File Offset: 0x000035E8
		public bool MoveNext()
		{
			this.m_Position++;
			bool flag = this.m_Position < this.m_Impl.GetCount(ref this.m_Container);
			bool flag2;
			if (flag)
			{
				this.m_Impl.SetSharedPropertyState(new IndexedCollectionSharedPropertyState
				{
					Index = this.m_Position,
					IsReadOnly = false
				});
				flag2 = true;
			}
			else
			{
				this.m_Impl.SetSharedPropertyState(this.m_Previous);
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005467 File Offset: 0x00003667
		public void Reset()
		{
			this.m_Position = -1;
			this.m_Impl.SetSharedPropertyState(this.m_Previous);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005483 File Offset: 0x00003683
		public void Dispose()
		{
		}

		// Token: 0x0400005D RID: 93
		private readonly IIndexedCollectionPropertyBagEnumerator<TContainer> m_Impl;

		// Token: 0x0400005E RID: 94
		private readonly IndexedCollectionSharedPropertyState m_Previous;

		// Token: 0x0400005F RID: 95
		private TContainer m_Container;

		// Token: 0x04000060 RID: 96
		private int m_Position;
	}
}
