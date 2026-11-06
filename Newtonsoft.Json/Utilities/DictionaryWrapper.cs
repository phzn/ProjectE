using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	internal class DictionaryWrapper<[Nullable(2)] TKey, [Nullable(2)] TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IWrappedDictionary, IDictionary, ICollection
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x00013BA1 File Offset: 0x00011DA1
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00013BBB File Offset: 0x00011DBB
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00013BD5 File Offset: 0x00011DD5
		public DictionaryWrapper(IReadOnlyDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._readOnlyDictionary = dictionary;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00013BEF File Offset: 0x00011DEF
		internal IDictionary<TKey, TValue> GenericDictionary
		{
			get
			{
				return this._genericDictionary;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00013BF7 File Offset: 0x00011DF7
		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00013C34 File Offset: 0x00011E34
		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey(key);
			}
			return this.GenericDictionary.ContainsKey(key);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00013C74 File Offset: 0x00011E74
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this.GenericDictionary.Keys;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00013CC4 File Offset: 0x00011EC4
		public bool Remove(TKey key)
		{
			if (this._dictionary != null)
			{
				if (this._dictionary.Contains(key))
				{
					this._dictionary.Remove(key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this.GenericDictionary.Remove(key);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00013D1C File Offset: 0x00011F1C
		public bool TryGetValue(TKey key, [Nullable(2)] out TValue value)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(key))
				{
					value = default(TValue);
					return false;
				}
				value = (TValue)((object)this._dictionary[key]);
				return true;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this.GenericDictionary.TryGetValue(key, out value);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00013D88 File Offset: 0x00011F88
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this.GenericDictionary.Values;
			}
		}

		// Token: 0x170000B2 RID: 178
		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)((object)this._dictionary[key]);
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[key];
				}
				return this.GenericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this.GenericDictionary[key] = value;
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00013E64 File Offset: 0x00012064
		public void Add([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			IDictionary<TKey, TValue> genericDictionary = this._genericDictionary;
			if (genericDictionary == null)
			{
				return;
			}
			genericDictionary.Add(item);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00013EB0 File Offset: 0x000120B0
		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this.GenericDictionary.Clear();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00013EE0 File Offset: 0x000120E0
		public bool Contains([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.Contains(item);
			}
			return this.GenericDictionary.Contains(item);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00013F30 File Offset: 0x00012130
		public void CopyTo([Nullable(new byte[] { 1, 0, 1, 1 })] KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary != null)
			{
				using (IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry entry = enumerator.Entry;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)entry.Key), (TValue)((object)entry.Value));
					}
					return;
				}
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this.GenericDictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00013FCC File Offset: 0x000121CC
		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Count;
				}
				return this.GenericDictionary.Count;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00014001 File Offset: 0x00012201
		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				return this._readOnlyDictionary != null || this.GenericDictionary.IsReadOnly;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001402C File Offset: 0x0001222C
		public bool Remove([Nullable(new byte[] { 0, 1, 1 })] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(item.Key))
				{
					return true;
				}
				if (object.Equals(this._dictionary[item.Key], item.Value))
				{
					this._dictionary.Remove(item.Key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this.GenericDictionary.Remove(item);
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000140BC File Offset: 0x000122BC
		[return: Nullable(new byte[] { 1, 0, 1, 1 })]
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return (from DictionaryEntry de in this._dictionary
					select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.GetEnumerator();
			}
			return this.GenericDictionary.GetEnumerator();
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00014125 File Offset: 0x00012325
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001412D File Offset: 0x0001232D
		void IDictionary.Add(object key, [Nullable(2)] object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this.GenericDictionary.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x170000B5 RID: 181
		[Nullable(2)]
		object IDictionary.this[object key]
		{
			[return: Nullable(2)]
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[(TKey)((object)key)];
				}
				return this.GenericDictionary[(TKey)((object)key)];
			}
			[param: Nullable(2)]
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this.GenericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014200 File Offset: 0x00012400
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._readOnlyDictionary.GetEnumerator());
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this.GenericDictionary.GetEnumerator());
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014254 File Offset: 0x00012454
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000142A1 File Offset: 0x000124A1
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && (this._readOnlyDictionary != null || this._dictionary.IsFixedSize);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x000142C2 File Offset: 0x000124C2
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00014301 File Offset: 0x00012501
		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this.GenericDictionary.Remove((TKey)((object)key));
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00014338 File Offset: 0x00012538
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00014377 File Offset: 0x00012577
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this.GenericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x000143AF File Offset: 0x000125AF
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._dictionary != null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000143C6 File Offset: 0x000125C6
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000143E8 File Offset: 0x000125E8
		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary;
				}
				return this.GenericDictionary;
			}
		}

		// Token: 0x040001D5 RID: 469
		[Nullable(2)]
		private readonly IDictionary _dictionary;

		// Token: 0x040001D6 RID: 470
		[Nullable(new byte[] { 2, 1, 1 })]
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x040001D7 RID: 471
		[Nullable(new byte[] { 2, 1, 1 })]
		private readonly IReadOnlyDictionary<TKey, TValue> _readOnlyDictionary;

		// Token: 0x040001D8 RID: 472
		[Nullable(2)]
		private object _syncRoot;

		// Token: 0x02000169 RID: 361
		[Nullable(0)]
		private readonly struct DictionaryEnumerator<[Nullable(2)] TEnumeratorKey, [Nullable(2)] TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000E8C RID: 3724 RVA: 0x00040C13 File Offset: 0x0003EE13
			public DictionaryEnumerator([Nullable(new byte[] { 1, 0, 1, 1 })] IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x1700028E RID: 654
			// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00040C27 File Offset: 0x0003EE27
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x1700028F RID: 655
			// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00040C34 File Offset: 0x0003EE34
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000290 RID: 656
			// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00040C50 File Offset: 0x0003EE50
			[Nullable(2)]
			public object Value
			{
				[NullableContext(2)]
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000291 RID: 657
			// (get) Token: 0x06000E90 RID: 3728 RVA: 0x00040C6C File Offset: 0x0003EE6C
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object obj = keyValuePair.Key;
					keyValuePair = this._e.Current;
					return new DictionaryEntry(obj, keyValuePair.Value);
				}
			}

			// Token: 0x06000E91 RID: 3729 RVA: 0x00040CB3 File Offset: 0x0003EEB3
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x06000E92 RID: 3730 RVA: 0x00040CC0 File Offset: 0x0003EEC0
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x0400069E RID: 1694
			[Nullable(new byte[] { 1, 0, 1, 1 })]
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
