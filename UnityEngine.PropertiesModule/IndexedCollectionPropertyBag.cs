using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x0200003D RID: 61
	public class IndexedCollectionPropertyBag<TList, TElement> : PropertyBag<TList>, IListPropertyBag<TList, TElement>, ICollectionPropertyBag<TList, TElement>, IPropertyBag<TList>, IPropertyBag, ICollectionPropertyBagAccept<TList>, IListPropertyBagAccept<TList>, IListPropertyAccept<TList>, IIndexedProperties<TList>, IConstructorWithCount<TList>, IConstructor, IIndexedCollectionPropertyBagEnumerator<TList> where TList : IList<TElement>
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00005488 File Offset: 0x00003688
		public override PropertyCollection<TList> GetProperties()
		{
			return PropertyCollection<TList>.Empty;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000054A0 File Offset: 0x000036A0
		public override PropertyCollection<TList> GetProperties(ref TList container)
		{
			return new PropertyCollection<TList>(new IndexedCollectionPropertyBagEnumerable<TList>(this, container));
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000054C4 File Offset: 0x000036C4
		public bool TryGetProperty(ref TList container, int index, out IProperty<TList> property)
		{
			bool flag = index >= container.Count;
			bool flag2;
			if (flag)
			{
				property = null;
				flag2 = false;
			}
			else
			{
				property = new IndexedCollectionPropertyBag<TList, TElement>.ListElementProperty
				{
					m_Index = index,
					m_IsReadOnly = false
				};
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000550A File Offset: 0x0000370A
		void ICollectionPropertyBagAccept<TList>.Accept(ICollectionPropertyBagVisitor visitor, ref TList container)
		{
			visitor.Visit<TList, TElement>(this, ref container);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005516 File Offset: 0x00003716
		void IListPropertyBagAccept<TList>.Accept(IListPropertyBagVisitor visitor, ref TList list)
		{
			visitor.Visit<TList, TElement>(this, ref list);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005524 File Offset: 0x00003724
		void IListPropertyAccept<TList>.Accept<TContainer>(IListPropertyVisitor visitor, Property<TContainer, TList> property, ref TContainer container, ref TList list)
		{
			using (new AttributesScope(this.m_Property, property))
			{
				visitor.Visit<TContainer, TList, TElement>(property, ref container, ref list);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005570 File Offset: 0x00003770
		TList IConstructorWithCount<TList>.InstantiateWithCount(int count)
		{
			return this.InstantiateWithCount(count);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000558C File Offset: 0x0000378C
		protected virtual TList InstantiateWithCount(int count)
		{
			return default(TList);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000055A8 File Offset: 0x000037A8
		int IIndexedCollectionPropertyBagEnumerator<TList>.GetCount(ref TList container)
		{
			return container.Count;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000055C8 File Offset: 0x000037C8
		IProperty<TList> IIndexedCollectionPropertyBagEnumerator<TList>.GetSharedProperty()
		{
			return this.m_Property;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000055E0 File Offset: 0x000037E0
		IndexedCollectionSharedPropertyState IIndexedCollectionPropertyBagEnumerator<TList>.GetSharedPropertyState()
		{
			return new IndexedCollectionSharedPropertyState
			{
				Index = this.m_Property.m_Index,
				IsReadOnly = this.m_Property.IsReadOnly
			};
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000561F File Offset: 0x0000381F
		void IIndexedCollectionPropertyBagEnumerator<TList>.SetSharedPropertyState(IndexedCollectionSharedPropertyState state)
		{
			this.m_Property.m_Index = state.Index;
			this.m_Property.m_IsReadOnly = state.IsReadOnly;
		}

		// Token: 0x04000063 RID: 99
		private readonly IndexedCollectionPropertyBag<TList, TElement>.ListElementProperty m_Property = new IndexedCollectionPropertyBag<TList, TElement>.ListElementProperty();

		// Token: 0x0200003E RID: 62
		private class ListElementProperty : Property<TList, TElement>, IListElementProperty, ICollectionElementProperty
		{
			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000124 RID: 292 RVA: 0x00005658 File Offset: 0x00003858
			public int Index
			{
				get
				{
					return this.m_Index;
				}
			}

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x06000125 RID: 293 RVA: 0x00005660 File Offset: 0x00003860
			public override string Name
			{
				get
				{
					return this.Index.ToString();
				}
			}

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000126 RID: 294 RVA: 0x0000567B File Offset: 0x0000387B
			public override bool IsReadOnly
			{
				get
				{
					return this.m_IsReadOnly;
				}
			}

			// Token: 0x06000127 RID: 295 RVA: 0x00005683 File Offset: 0x00003883
			public override TElement GetValue(ref TList container)
			{
				return container[this.m_Index];
			}

			// Token: 0x06000128 RID: 296 RVA: 0x00005697 File Offset: 0x00003897
			public override void SetValue(ref TList container, TElement value)
			{
				container[this.m_Index] = value;
			}

			// Token: 0x04000064 RID: 100
			internal int m_Index;

			// Token: 0x04000065 RID: 101
			internal bool m_IsReadOnly;
		}
	}
}
