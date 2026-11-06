using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x0200007B RID: 123
	internal class SerializedArrayViewPropertyBag : PropertyBag<SerializedArrayView>, IListPropertyBag<SerializedArrayView, SerializedValueView>, ICollectionPropertyBag<SerializedArrayView, SerializedValueView>, IPropertyBag<SerializedArrayView>, IPropertyBag, ICollectionPropertyBagAccept<SerializedArrayView>, IListPropertyBagAccept<SerializedArrayView>, IListPropertyAccept<SerializedArrayView>, IIndexedProperties<SerializedArrayView>
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000D805 File Offset: 0x0000BA05
		public override PropertyCollection<SerializedArrayView> GetProperties()
		{
			return PropertyCollection<SerializedArrayView>.Empty;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000D80C File Offset: 0x0000BA0C
		public override PropertyCollection<SerializedArrayView> GetProperties(ref SerializedArrayView container)
		{
			return new PropertyCollection<SerializedArrayView>(new SerializedArrayViewPropertyBag.Enumerable(container));
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000D824 File Offset: 0x0000BA24
		bool IIndexedProperties<SerializedArrayView>.TryGetProperty(ref SerializedArrayView container, int index, out IProperty<SerializedArrayView> property)
		{
			SerializedArrayView.Enumerator enumerator = container.GetEnumerator();
			int num = -1;
			while (enumerator.MoveNext())
			{
				num++;
				if (num == index)
				{
					property = new SerializedArrayViewPropertyBag.Property
					{
						m_Index = index,
						m_View = enumerator.Current
					};
					return true;
				}
			}
			property = null;
			return false;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D86E File Offset: 0x0000BA6E
		void ICollectionPropertyBagAccept<SerializedArrayView>.Accept(ICollectionPropertyBagVisitor visitor, ref SerializedArrayView container)
		{
			visitor.Visit<SerializedArrayView, SerializedValueView>(this, ref container);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D878 File Offset: 0x0000BA78
		void IListPropertyBagAccept<SerializedArrayView>.Accept(IListPropertyBagVisitor visitor, ref SerializedArrayView container)
		{
			visitor.Visit<SerializedArrayView, SerializedValueView>(this, ref container);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000D882 File Offset: 0x0000BA82
		void IListPropertyAccept<SerializedArrayView>.Accept<TContainer>(IListPropertyVisitor visitor, Property<TContainer, SerializedArrayView> property, ref TContainer container, ref SerializedArrayView list)
		{
			visitor.Visit<TContainer, SerializedArrayView, SerializedValueView>(property, ref container, ref list);
		}

		// Token: 0x040001A5 RID: 421
		private static readonly SerializedArrayViewPropertyBag.Property k_Property = new SerializedArrayViewPropertyBag.Property();

		// Token: 0x0200007C RID: 124
		private class Property : Property<SerializedArrayView, SerializedValueView>, IListElementProperty, ICollectionElementProperty
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000D8A2 File Offset: 0x0000BAA2
			public int Index
			{
				get
				{
					return this.m_Index;
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000D8AA File Offset: 0x0000BAAA
			public override string Name
			{
				get
				{
					return this.m_Index.ToString();
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060002D3 RID: 723 RVA: 0x00003204 File Offset: 0x00001404
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060002D4 RID: 724 RVA: 0x0000D8B7 File Offset: 0x0000BAB7
			public override SerializedValueView GetValue(ref SerializedArrayView container)
			{
				return this.m_View;
			}

			// Token: 0x060002D5 RID: 725 RVA: 0x0000B071 File Offset: 0x00009271
			public override void SetValue(ref SerializedArrayView container, SerializedValueView value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}

			// Token: 0x040001A6 RID: 422
			internal int m_Index;

			// Token: 0x040001A7 RID: 423
			internal SerializedValueView m_View;
		}

		// Token: 0x0200007D RID: 125
		private struct Enumerator : IEnumerator<IProperty<SerializedArrayView>>, IEnumerator, IDisposable
		{
			// Token: 0x060002D7 RID: 727 RVA: 0x0000D8C7 File Offset: 0x0000BAC7
			public Enumerator(SerializedArrayView.Enumerator enumerator)
			{
				this.m_Index = -1;
				this.m_Enumerator = enumerator;
			}

			// Token: 0x060002D8 RID: 728 RVA: 0x0000D8D7 File Offset: 0x0000BAD7
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x060002D9 RID: 729 RVA: 0x0000D8F2 File Offset: 0x0000BAF2
			public void Reset()
			{
				this.m_Index = -1;
				this.m_Enumerator.Reset();
			}

			// Token: 0x060002DA RID: 730 RVA: 0x0000D906 File Offset: 0x0000BB06
			public void Dispose()
			{
				this.m_Enumerator.Dispose();
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060002DB RID: 731 RVA: 0x0000D913 File Offset: 0x0000BB13
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060002DC RID: 732 RVA: 0x0000D91B File Offset: 0x0000BB1B
			public IProperty<SerializedArrayView> Current
			{
				get
				{
					SerializedArrayViewPropertyBag.k_Property.m_Index = this.m_Index;
					SerializedArrayViewPropertyBag.k_Property.m_View = this.m_Enumerator.Current;
					return SerializedArrayViewPropertyBag.k_Property;
				}
			}

			// Token: 0x040001A8 RID: 424
			private int m_Index;

			// Token: 0x040001A9 RID: 425
			private SerializedArrayView.Enumerator m_Enumerator;
		}

		// Token: 0x0200007E RID: 126
		private readonly struct Enumerable : IEnumerable<IProperty<SerializedArrayView>>, IEnumerable
		{
			// Token: 0x060002DD RID: 733 RVA: 0x0000D947 File Offset: 0x0000BB47
			public Enumerable(SerializedArrayView container)
			{
				this.m_Container = container;
			}

			// Token: 0x060002DE RID: 734 RVA: 0x0000D950 File Offset: 0x0000BB50
			public IEnumerator<IProperty<SerializedArrayView>> GetEnumerator()
			{
				return new SerializedArrayViewPropertyBag.Enumerator(this.m_Container.GetEnumerator());
			}

			// Token: 0x060002DF RID: 735 RVA: 0x0000D950 File Offset: 0x0000BB50
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SerializedArrayViewPropertyBag.Enumerator(this.m_Container.GetEnumerator());
			}

			// Token: 0x040001AA RID: 426
			private readonly SerializedArrayView m_Container;
		}
	}
}
