using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000052 RID: 82
	public readonly struct PropertyCollection<TContainer> : IEnumerable<IProperty<TContainer>>, IEnumerable
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000600A File Offset: 0x0000420A
		public static PropertyCollection<TContainer> Empty { get; } = default(PropertyCollection<TContainer>);

		// Token: 0x0600018A RID: 394 RVA: 0x00006011 File Offset: 0x00004211
		public PropertyCollection(IEnumerable<IProperty<TContainer>> enumerable)
		{
			this.m_Type = PropertyCollection<TContainer>.EnumeratorType.Enumerable;
			this.m_Enumerable = enumerable;
			this.m_Properties = null;
			this.m_IndexedCollectionPropertyBag = default(IndexedCollectionPropertyBagEnumerable<TContainer>);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006035 File Offset: 0x00004235
		public PropertyCollection(List<IProperty<TContainer>> properties)
		{
			this.m_Type = PropertyCollection<TContainer>.EnumeratorType.List;
			this.m_Enumerable = null;
			this.m_Properties = properties;
			this.m_IndexedCollectionPropertyBag = default(IndexedCollectionPropertyBagEnumerable<TContainer>);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006059 File Offset: 0x00004259
		internal PropertyCollection(IndexedCollectionPropertyBagEnumerable<TContainer> enumerable)
		{
			this.m_Type = PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag;
			this.m_Enumerable = null;
			this.m_Properties = null;
			this.m_IndexedCollectionPropertyBag = enumerable;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006078 File Offset: 0x00004278
		public PropertyCollection<TContainer>.Enumerator GetEnumerator()
		{
			PropertyCollection<TContainer>.Enumerator enumerator;
			switch (this.m_Type)
			{
			case PropertyCollection<TContainer>.EnumeratorType.Empty:
				enumerator = default(PropertyCollection<TContainer>.Enumerator);
				break;
			case PropertyCollection<TContainer>.EnumeratorType.Enumerable:
				enumerator = new PropertyCollection<TContainer>.Enumerator(this.m_Enumerable.GetEnumerator());
				break;
			case PropertyCollection<TContainer>.EnumeratorType.List:
				enumerator = new PropertyCollection<TContainer>.Enumerator(this.m_Properties.GetEnumerator());
				break;
			case PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag:
				enumerator = new PropertyCollection<TContainer>.Enumerator(this.m_IndexedCollectionPropertyBag.GetEnumerator());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return enumerator;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000060F3 File Offset: 0x000042F3
		IEnumerator<IProperty<TContainer>> IEnumerable<IProperty<TContainer>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000060F3 File Offset: 0x000042F3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000076 RID: 118
		private readonly PropertyCollection<TContainer>.EnumeratorType m_Type;

		// Token: 0x04000077 RID: 119
		private readonly IEnumerable<IProperty<TContainer>> m_Enumerable;

		// Token: 0x04000078 RID: 120
		private readonly List<IProperty<TContainer>> m_Properties;

		// Token: 0x04000079 RID: 121
		private readonly IndexedCollectionPropertyBagEnumerable<TContainer> m_IndexedCollectionPropertyBag;

		// Token: 0x02000053 RID: 83
		private enum EnumeratorType
		{
			// Token: 0x0400007C RID: 124
			Empty,
			// Token: 0x0400007D RID: 125
			Enumerable,
			// Token: 0x0400007E RID: 126
			List,
			// Token: 0x0400007F RID: 127
			IndexedCollectionPropertyBag
		}

		// Token: 0x02000054 RID: 84
		public struct Enumerator : IEnumerator<IProperty<TContainer>>, IEnumerator, IDisposable
		{
			// Token: 0x17000040 RID: 64
			// (get) Token: 0x06000191 RID: 401 RVA: 0x0000610D File Offset: 0x0000430D
			// (set) Token: 0x06000192 RID: 402 RVA: 0x00006115 File Offset: 0x00004315
			public IProperty<TContainer> Current { readonly get; private set; }

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000193 RID: 403 RVA: 0x0000611E File Offset: 0x0000431E
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000194 RID: 404 RVA: 0x00006126 File Offset: 0x00004326
			internal Enumerator(IEnumerator<IProperty<TContainer>> enumerator)
			{
				this.m_Type = PropertyCollection<TContainer>.EnumeratorType.Enumerable;
				this.m_Enumerator = enumerator;
				this.m_Properties = default(List<IProperty<TContainer>>.Enumerator);
				this.m_IndexedCollectionPropertyBag = default(IndexedCollectionPropertyBagEnumerator<TContainer>);
				this.Current = null;
			}

			// Token: 0x06000195 RID: 405 RVA: 0x00006157 File Offset: 0x00004357
			internal Enumerator(List<IProperty<TContainer>>.Enumerator properties)
			{
				this.m_Type = PropertyCollection<TContainer>.EnumeratorType.List;
				this.m_Enumerator = null;
				this.m_Properties = properties;
				this.m_IndexedCollectionPropertyBag = default(IndexedCollectionPropertyBagEnumerator<TContainer>);
				this.Current = null;
			}

			// Token: 0x06000196 RID: 406 RVA: 0x00006183 File Offset: 0x00004383
			internal Enumerator(IndexedCollectionPropertyBagEnumerator<TContainer> enumerator)
			{
				this.m_Type = PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag;
				this.m_Enumerator = null;
				this.m_Properties = default(List<IProperty<TContainer>>.Enumerator);
				this.m_IndexedCollectionPropertyBag = enumerator;
				this.Current = null;
			}

			// Token: 0x06000197 RID: 407 RVA: 0x000061B0 File Offset: 0x000043B0
			public bool MoveNext()
			{
				bool flag;
				switch (this.m_Type)
				{
				case PropertyCollection<TContainer>.EnumeratorType.Empty:
					return false;
				case PropertyCollection<TContainer>.EnumeratorType.Enumerable:
					flag = this.m_Enumerator.MoveNext();
					this.Current = this.m_Enumerator.Current;
					break;
				case PropertyCollection<TContainer>.EnumeratorType.List:
					flag = this.m_Properties.MoveNext();
					this.Current = this.m_Properties.Current;
					break;
				case PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag:
					flag = this.m_IndexedCollectionPropertyBag.MoveNext();
					this.Current = this.m_IndexedCollectionPropertyBag.Current;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return flag;
			}

			// Token: 0x06000198 RID: 408 RVA: 0x00006250 File Offset: 0x00004450
			public void Reset()
			{
				switch (this.m_Type)
				{
				case PropertyCollection<TContainer>.EnumeratorType.Empty:
					break;
				case PropertyCollection<TContainer>.EnumeratorType.Enumerable:
					this.m_Enumerator.Reset();
					break;
				case PropertyCollection<TContainer>.EnumeratorType.List:
					((IEnumerator)this.m_Properties).Reset();
					break;
				case PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag:
					this.m_IndexedCollectionPropertyBag.Reset();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			// Token: 0x06000199 RID: 409 RVA: 0x000062B8 File Offset: 0x000044B8
			public void Dispose()
			{
				switch (this.m_Type)
				{
				case PropertyCollection<TContainer>.EnumeratorType.Empty:
					break;
				case PropertyCollection<TContainer>.EnumeratorType.Enumerable:
					this.m_Enumerator.Dispose();
					break;
				case PropertyCollection<TContainer>.EnumeratorType.List:
					break;
				case PropertyCollection<TContainer>.EnumeratorType.IndexedCollectionPropertyBag:
					this.m_IndexedCollectionPropertyBag.Dispose();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			// Token: 0x04000080 RID: 128
			private readonly PropertyCollection<TContainer>.EnumeratorType m_Type;

			// Token: 0x04000081 RID: 129
			private IEnumerator<IProperty<TContainer>> m_Enumerator;

			// Token: 0x04000082 RID: 130
			private List<IProperty<TContainer>>.Enumerator m_Properties;

			// Token: 0x04000083 RID: 131
			private IndexedCollectionPropertyBagEnumerator<TContainer> m_IndexedCollectionPropertyBag;
		}
	}
}
