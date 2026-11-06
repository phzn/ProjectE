using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Unity.Properties
{
	// Token: 0x02000048 RID: 72
	public class KeyValueCollectionPropertyBag<TDictionary, TKey, TValue> : PropertyBag<TDictionary>, IDictionaryPropertyBag<TDictionary, TKey, TValue>, ICollectionPropertyBag<TDictionary, KeyValuePair<TKey, TValue>>, IPropertyBag<TDictionary>, IPropertyBag, ICollectionPropertyBagAccept<TDictionary>, IDictionaryPropertyBagAccept<TDictionary>, IDictionaryPropertyAccept<TDictionary>, IKeyedProperties<TDictionary, object> where TDictionary : IDictionary<TKey, TValue>
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000056B8 File Offset: 0x000038B8
		public override PropertyCollection<TDictionary> GetProperties()
		{
			return PropertyCollection<TDictionary>.Empty;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000056D0 File Offset: 0x000038D0
		public override PropertyCollection<TDictionary> GetProperties(ref TDictionary container)
		{
			return new PropertyCollection<TDictionary>(new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.Enumerable(container, this.m_KeyValuePairProperty));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000056FD File Offset: 0x000038FD
		void ICollectionPropertyBagAccept<TDictionary>.Accept(ICollectionPropertyBagVisitor visitor, ref TDictionary container)
		{
			visitor.Visit<TDictionary, KeyValuePair<TKey, TValue>>(this, ref container);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005709 File Offset: 0x00003909
		void IDictionaryPropertyBagAccept<TDictionary>.Accept(IDictionaryPropertyBagVisitor visitor, ref TDictionary container)
		{
			visitor.Visit<TDictionary, TKey, TValue>(this, ref container);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005718 File Offset: 0x00003918
		void IDictionaryPropertyAccept<TDictionary>.Accept<TContainer>(IDictionaryPropertyVisitor visitor, Property<TContainer, TDictionary> property, ref TContainer container, ref TDictionary dictionary)
		{
			using (new AttributesScope(this.m_KeyValuePairProperty, property))
			{
				visitor.Visit<TContainer, TDictionary, TKey, TValue>(property, ref container, ref dictionary);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005764 File Offset: 0x00003964
		bool IKeyedProperties<TDictionary, object>.TryGetProperty(ref TDictionary container, object key, out IProperty<TDictionary> property)
		{
			bool flag = container.ContainsKey((TKey)((object)key));
			bool flag2;
			if (flag)
			{
				property = new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty
				{
					Key = (TKey)((object)key)
				};
				flag2 = true;
			}
			else
			{
				property = null;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04000066 RID: 102
		private readonly KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty m_KeyValuePairProperty = new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty();

		// Token: 0x02000049 RID: 73
		private class KeyValuePairProperty : Property<TDictionary, KeyValuePair<TKey, TValue>>, IDictionaryElementProperty<TKey>, IDictionaryElementProperty, ICollectionElementProperty
		{
			// Token: 0x17000034 RID: 52
			// (get) Token: 0x0600013B RID: 315 RVA: 0x000057C0 File Offset: 0x000039C0
			public override string Name
			{
				get
				{
					TKey key = this.Key;
					return key.ToString();
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x0600013C RID: 316 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600013D RID: 317 RVA: 0x000057E4 File Offset: 0x000039E4
			public override KeyValuePair<TKey, TValue> GetValue(ref TDictionary container)
			{
				return new KeyValuePair<TKey, TValue>(this.Key, container[this.Key]);
			}

			// Token: 0x0600013E RID: 318 RVA: 0x00005813 File Offset: 0x00003A13
			public override void SetValue(ref TDictionary container, KeyValuePair<TKey, TValue> value)
			{
				container[value.Key] = value.Value;
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x0600013F RID: 319 RVA: 0x00005831 File Offset: 0x00003A31
			// (set) Token: 0x06000140 RID: 320 RVA: 0x00005839 File Offset: 0x00003A39
			public TKey Key { get; internal set; }

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x06000141 RID: 321 RVA: 0x00005842 File Offset: 0x00003A42
			public object ObjectKey
			{
				get
				{
					return this.Key;
				}
			}
		}

		// Token: 0x0200004A RID: 74
		private readonly struct Enumerable : IEnumerable<IProperty<TDictionary>>, IEnumerable
		{
			// Token: 0x06000143 RID: 323 RVA: 0x00005858 File Offset: 0x00003A58
			public Enumerable(TDictionary dictionary, KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty property)
			{
				this.m_Dictionary = dictionary;
				this.m_Property = property;
			}

			// Token: 0x06000144 RID: 324 RVA: 0x00005869 File Offset: 0x00003A69
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.Enumerable.Enumerator(this.m_Dictionary, this.m_Property);
			}

			// Token: 0x06000145 RID: 325 RVA: 0x00005869 File Offset: 0x00003A69
			IEnumerator<IProperty<TDictionary>> IEnumerable<IProperty<TDictionary>>.GetEnumerator()
			{
				return new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.Enumerable.Enumerator(this.m_Dictionary, this.m_Property);
			}

			// Token: 0x04000068 RID: 104
			private readonly TDictionary m_Dictionary;

			// Token: 0x04000069 RID: 105
			private readonly KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty m_Property;

			// Token: 0x0200004B RID: 75
			private class Enumerator : IEnumerator<IProperty<TDictionary>>, IEnumerator, IDisposable
			{
				// Token: 0x06000146 RID: 326 RVA: 0x0000587C File Offset: 0x00003A7C
				public Enumerator(TDictionary dictionary, KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty property)
				{
					this.m_Dictionary = dictionary;
					this.m_Property = property;
					this.m_Previous = property.Key;
					this.m_Position = -1;
					this.m_Keys = CollectionPool<List<TKey>, TKey>.Get();
					this.m_Keys.AddRange(this.m_Dictionary.Keys);
				}

				// Token: 0x17000038 RID: 56
				// (get) Token: 0x06000147 RID: 327 RVA: 0x000058DA File Offset: 0x00003ADA
				public IProperty<TDictionary> Current
				{
					get
					{
						return this.m_Property;
					}
				}

				// Token: 0x17000039 RID: 57
				// (get) Token: 0x06000148 RID: 328 RVA: 0x000058E2 File Offset: 0x00003AE2
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x06000149 RID: 329 RVA: 0x000058EC File Offset: 0x00003AEC
				public bool MoveNext()
				{
					this.m_Position++;
					int position = this.m_Position;
					TDictionary dictionary = this.m_Dictionary;
					bool flag = position < dictionary.Count;
					bool flag2;
					if (flag)
					{
						this.m_Property.Key = this.m_Keys[this.m_Position];
						flag2 = true;
					}
					else
					{
						this.m_Property.Key = this.m_Previous;
						flag2 = false;
					}
					return flag2;
				}

				// Token: 0x0600014A RID: 330 RVA: 0x00005961 File Offset: 0x00003B61
				public void Reset()
				{
					this.m_Position = -1;
					this.m_Property.Key = this.m_Previous;
				}

				// Token: 0x0600014B RID: 331 RVA: 0x0000597D File Offset: 0x00003B7D
				public void Dispose()
				{
					CollectionPool<List<TKey>, TKey>.Release(this.m_Keys);
				}

				// Token: 0x0400006A RID: 106
				private readonly TDictionary m_Dictionary;

				// Token: 0x0400006B RID: 107
				private readonly KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>.KeyValuePairProperty m_Property;

				// Token: 0x0400006C RID: 108
				private readonly TKey m_Previous;

				// Token: 0x0400006D RID: 109
				private readonly List<TKey> m_Keys;

				// Token: 0x0400006E RID: 110
				private int m_Position;
			}
		}
	}
}
