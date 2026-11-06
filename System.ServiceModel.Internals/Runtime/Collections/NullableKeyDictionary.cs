using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Runtime.Collections
{
	// Token: 0x02000051 RID: 81
	internal class NullableKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		public NullableKeyDictionary()
		{
			this.innerDictionary = new Dictionary<TKey, TValue>();
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000FDCF File Offset: 0x0000DFCF
		public int Count
		{
			get
			{
				return this.innerDictionary.Count + (this.isNullKeyPresent ? 1 : 0);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000FDE9 File Offset: 0x0000DFE9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000FDEC File Offset: 0x0000DFEC
		public ICollection<TKey> Keys
		{
			get
			{
				return new NullableKeyDictionary<TKey, TValue>.NullKeyDictionaryKeyCollection<TKey, TValue>(this);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		public ICollection<TValue> Values
		{
			get
			{
				return new NullableKeyDictionary<TKey, TValue>.NullKeyDictionaryValueCollection<TKey, TValue>(this);
			}
		}

		// Token: 0x17000063 RID: 99
		public TValue this[TKey key]
		{
			get
			{
				if (key != null)
				{
					return this.innerDictionary[key];
				}
				if (this.isNullKeyPresent)
				{
					return this.nullKeyValue;
				}
				throw Fx.Exception.AsError(new KeyNotFoundException());
			}
			set
			{
				if (key == null)
				{
					this.isNullKeyPresent = true;
					this.nullKeyValue = value;
					return;
				}
				this.innerDictionary[key] = value;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FE58 File Offset: 0x0000E058
		public void Add(TKey key, TValue value)
		{
			if (key != null)
			{
				this.innerDictionary.Add(key, value);
				return;
			}
			if (this.isNullKeyPresent)
			{
				throw Fx.Exception.Argument("key", "Null Key Already Present");
			}
			this.isNullKeyPresent = true;
			this.nullKeyValue = value;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FEA6 File Offset: 0x0000E0A6
		public bool ContainsKey(TKey key)
		{
			if (key != null)
			{
				return this.innerDictionary.ContainsKey(key);
			}
			return this.isNullKeyPresent;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FEC3 File Offset: 0x0000E0C3
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				bool flag = this.isNullKeyPresent;
				this.isNullKeyPresent = false;
				this.nullKeyValue = default(TValue);
				return flag;
			}
			return this.innerDictionary.Remove(key);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FEF3 File Offset: 0x0000E0F3
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key != null)
			{
				return this.innerDictionary.TryGetValue(key, out value);
			}
			if (this.isNullKeyPresent)
			{
				value = this.nullKeyValue;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000FF29 File Offset: 0x0000E129
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FF3F File Offset: 0x0000E13F
		public void Clear()
		{
			this.isNullKeyPresent = false;
			this.nullKeyValue = default(TValue);
			this.innerDictionary.Clear();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FF60 File Offset: 0x0000E160
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (item.Key != null)
			{
				return this.innerDictionary.Contains(item);
			}
			if (!this.isNullKeyPresent)
			{
				return false;
			}
			if (item.Value != null)
			{
				TValue value = item.Value;
				return value.Equals(this.nullKeyValue);
			}
			return this.nullKeyValue == null;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.innerDictionary.CopyTo(array, arrayIndex);
			if (this.isNullKeyPresent)
			{
				array[arrayIndex + this.innerDictionary.Count] = new KeyValuePair<TKey, TValue>(default(TKey), this.nullKeyValue);
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010019 File Offset: 0x0000E219
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (item.Key != null)
			{
				return this.innerDictionary.Remove(item);
			}
			if (this.Contains(item))
			{
				this.isNullKeyPresent = false;
				this.nullKeyValue = default(TValue);
				return true;
			}
			return false;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00010055 File Offset: 0x0000E255
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.innerDictionary)
			{
				yield return keyValuePair;
			}
			if (this.isNullKeyPresent)
			{
				yield return new KeyValuePair<TKey, TValue>(default(TKey), this.nullKeyValue);
			}
			yield break;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010064 File Offset: 0x0000E264
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
		}

		// Token: 0x040001F0 RID: 496
		private bool isNullKeyPresent;

		// Token: 0x040001F1 RID: 497
		private TValue nullKeyValue;

		// Token: 0x040001F2 RID: 498
		private IDictionary<TKey, TValue> innerDictionary;

		// Token: 0x02000094 RID: 148
		private class NullKeyDictionaryKeyCollection<TypeKey, TypeValue> : ICollection<TypeKey>, IEnumerable<TypeKey>, IEnumerable
		{
			// Token: 0x06000415 RID: 1045 RVA: 0x00012FEC File Offset: 0x000111EC
			public NullKeyDictionaryKeyCollection(NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary)
			{
				this.nullKeyDictionary = nullKeyDictionary;
			}

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x06000416 RID: 1046 RVA: 0x00012FFC File Offset: 0x000111FC
			public int Count
			{
				get
				{
					int num = this.nullKeyDictionary.innerDictionary.Keys.Count;
					if (this.nullKeyDictionary.isNullKeyPresent)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000417 RID: 1047 RVA: 0x00013031 File Offset: 0x00011231
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000418 RID: 1048 RVA: 0x00013034 File Offset: 0x00011234
			public void Add(TypeKey item)
			{
				throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
			}

			// Token: 0x06000419 RID: 1049 RVA: 0x0001304A File Offset: 0x0001124A
			public void Clear()
			{
				throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
			}

			// Token: 0x0600041A RID: 1050 RVA: 0x00013060 File Offset: 0x00011260
			public bool Contains(TypeKey item)
			{
				if (item != null)
				{
					return this.nullKeyDictionary.innerDictionary.Keys.Contains(item);
				}
				return this.nullKeyDictionary.isNullKeyPresent;
			}

			// Token: 0x0600041B RID: 1051 RVA: 0x0001308C File Offset: 0x0001128C
			public void CopyTo(TypeKey[] array, int arrayIndex)
			{
				this.nullKeyDictionary.innerDictionary.Keys.CopyTo(array, arrayIndex);
				if (this.nullKeyDictionary.isNullKeyPresent)
				{
					array[arrayIndex + this.nullKeyDictionary.innerDictionary.Keys.Count] = default(TypeKey);
				}
			}

			// Token: 0x0600041C RID: 1052 RVA: 0x000130E3 File Offset: 0x000112E3
			public bool Remove(TypeKey item)
			{
				throw Fx.Exception.AsError(new NotSupportedException("Key Collection Updates Not Allowed"));
			}

			// Token: 0x0600041D RID: 1053 RVA: 0x000130F9 File Offset: 0x000112F9
			public IEnumerator<TypeKey> GetEnumerator()
			{
				foreach (TypeKey typeKey in this.nullKeyDictionary.innerDictionary.Keys)
				{
					yield return typeKey;
				}
				IEnumerator<TypeKey> enumerator = null;
				if (this.nullKeyDictionary.isNullKeyPresent)
				{
					TypeKey typeKey2 = default(TypeKey);
				}
				yield break;
				yield break;
			}

			// Token: 0x0600041E RID: 1054 RVA: 0x00013108 File Offset: 0x00011308
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<TypeKey>)this).GetEnumerator();
			}

			// Token: 0x040002F9 RID: 761
			private NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary;
		}

		// Token: 0x02000095 RID: 149
		private class NullKeyDictionaryValueCollection<TypeKey, TypeValue> : ICollection<TypeValue>, IEnumerable<TypeValue>, IEnumerable
		{
			// Token: 0x0600041F RID: 1055 RVA: 0x00013110 File Offset: 0x00011310
			public NullKeyDictionaryValueCollection(NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary)
			{
				this.nullKeyDictionary = nullKeyDictionary;
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000420 RID: 1056 RVA: 0x00013120 File Offset: 0x00011320
			public int Count
			{
				get
				{
					int num = this.nullKeyDictionary.innerDictionary.Values.Count;
					if (this.nullKeyDictionary.isNullKeyPresent)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000421 RID: 1057 RVA: 0x00013155 File Offset: 0x00011355
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x00013158 File Offset: 0x00011358
			public void Add(TypeValue item)
			{
				throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
			}

			// Token: 0x06000423 RID: 1059 RVA: 0x0001316E File Offset: 0x0001136E
			public void Clear()
			{
				throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
			}

			// Token: 0x06000424 RID: 1060 RVA: 0x00013184 File Offset: 0x00011384
			public bool Contains(TypeValue item)
			{
				return this.nullKeyDictionary.innerDictionary.Values.Contains(item) || (this.nullKeyDictionary.isNullKeyPresent && this.nullKeyDictionary.nullKeyValue.Equals(item));
			}

			// Token: 0x06000425 RID: 1061 RVA: 0x000131D8 File Offset: 0x000113D8
			public void CopyTo(TypeValue[] array, int arrayIndex)
			{
				this.nullKeyDictionary.innerDictionary.Values.CopyTo(array, arrayIndex);
				if (this.nullKeyDictionary.isNullKeyPresent)
				{
					array[arrayIndex + this.nullKeyDictionary.innerDictionary.Values.Count] = this.nullKeyDictionary.nullKeyValue;
				}
			}

			// Token: 0x06000426 RID: 1062 RVA: 0x00013231 File Offset: 0x00011431
			public bool Remove(TypeValue item)
			{
				throw Fx.Exception.AsError(new NotSupportedException("Value Collection Updates Not Allowed"));
			}

			// Token: 0x06000427 RID: 1063 RVA: 0x00013247 File Offset: 0x00011447
			public IEnumerator<TypeValue> GetEnumerator()
			{
				foreach (TypeValue typeValue in this.nullKeyDictionary.innerDictionary.Values)
				{
					yield return typeValue;
				}
				IEnumerator<TypeValue> enumerator = null;
				if (this.nullKeyDictionary.isNullKeyPresent)
				{
					yield return this.nullKeyDictionary.nullKeyValue;
				}
				yield break;
				yield break;
			}

			// Token: 0x06000428 RID: 1064 RVA: 0x00013256 File Offset: 0x00011456
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<TypeValue>)this).GetEnumerator();
			}

			// Token: 0x040002FA RID: 762
			private NullableKeyDictionary<TypeKey, TypeValue> nullKeyDictionary;
		}
	}
}
