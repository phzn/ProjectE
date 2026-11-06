using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Collections.Generic
{
	/// <summary>Represents a collection of keys and values.</summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	// Token: 0x02000A89 RID: 2697
	[DebuggerTypeProxy(typeof(IDictionaryDebugView<, >))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, ISerializable, IDeserializationCallback
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
		// Token: 0x0600609F RID: 24735 RVA: 0x001434C8 File Offset: 0x001416C8
		public Dictionary()
			: this(0, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than 0.</exception>
		// Token: 0x060060A0 RID: 24736 RVA: 0x001434D2 File Offset: 0x001416D2
		public Dictionary(int capacity)
			: this(capacity, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		// Token: 0x060060A1 RID: 24737 RVA: 0x001434DC File Offset: 0x001416DC
		public Dictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than 0.</exception>
		// Token: 0x060060A2 RID: 24738 RVA: 0x001434E6 File Offset: 0x001416E6
		public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			if (capacity > 0)
			{
				this.Initialize(capacity);
			}
			if (comparer != EqualityComparer<TKey>.Default)
			{
				this._comparer = comparer;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the default equality comparer for the key type.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		// Token: 0x060060A3 RID: 24739 RVA: 0x00143514 File Offset: 0x00141714
		public Dictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		// Token: 0x060060A4 RID: 24740 RVA: 0x00143520 File Offset: 0x00141720
		public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			if (dictionary.GetType() == typeof(Dictionary<TKey, TValue>))
			{
				Dictionary<TKey, TValue> dictionary2 = (Dictionary<TKey, TValue>)dictionary;
				int count = dictionary2._count;
				Dictionary<TKey, TValue>.Entry[] entries = dictionary2._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						this.Add(entries[i].key, entries[i].value);
					}
				}
				return;
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x001435F8 File Offset: 0x001417F8
		public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
			: this(collection, null)
		{
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x00143604 File Offset: 0x00141804
		public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
		{
			ICollection<KeyValuePair<TKey, TValue>> collection2 = collection as ICollection<KeyValuePair<TKey, TValue>>;
			this..ctor((collection2 != null) ? collection2.Count : 0, comparer);
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in collection)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class with serialized data.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		// Token: 0x060060A7 RID: 24743 RVA: 0x0014367C File Offset: 0x0014187C
		protected Dictionary(SerializationInfo info, StreamingContext context)
		{
			HashHelpers.SerializationInfoTable.Add(this, info);
		}

		/// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> that is used to determine equality of keys for the dictionary.</summary>
		/// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2" /> and to provide hash values for the keys.</returns>
		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060060A8 RID: 24744 RVA: 0x00143690 File Offset: 0x00141890
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				if (this._comparer != null)
				{
					return this._comparer;
				}
				return EqualityComparer<TKey>.Default;
			}
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060060A9 RID: 24745 RVA: 0x001436B3 File Offset: 0x001418B3
		public int Count
		{
			get
			{
				return this._count - this._freeCount;
			}
		}

		/// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060060AA RID: 24746 RVA: 0x001436C2 File Offset: 0x001418C2
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this._keys;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060060AB RID: 24747 RVA: 0x001436C2 File Offset: 0x001418C2
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this._keys;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060060AC RID: 24748 RVA: 0x001436C2 File Offset: 0x001418C2
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this._keys;
			}
		}

		/// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x060060AD RID: 24749 RVA: 0x001436DE File Offset: 0x001418DE
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this._values;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x060060AE RID: 24750 RVA: 0x001436DE File Offset: 0x001418DE
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this._values;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x001436DE File Offset: 0x001418DE
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this._values;
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
		// Token: 0x17001121 RID: 4385
		public TValue this[TKey key]
		{
			get
			{
				int num = this.FindEntry(key);
				if (num >= 0)
				{
					return this._entries[num].value;
				}
				ThrowHelper.ThrowKeyNotFoundException(key);
				return default(TValue);
			}
			set
			{
				this.TryInsert(key, value, InsertionBehavior.OverwriteExisting);
			}
		}

		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be <see langword="null" /> for reference types.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		// Token: 0x060060B2 RID: 24754 RVA: 0x00143747 File Offset: 0x00141947
		public void Add(TKey key, TValue value)
		{
			this.TryInsert(key, value, InsertionBehavior.ThrowOnExisting);
		}

		// Token: 0x060060B3 RID: 24755 RVA: 0x00143753 File Offset: 0x00141953
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x060060B4 RID: 24756 RVA: 0x0014376C File Offset: 0x0014196C
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this._entries[num].value, keyValuePair.Value);
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x001437B4 File Offset: 0x001419B4
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this._entries[num].value, keyValuePair.Value))
			{
				this.Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		/// <summary>Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		// Token: 0x060060B6 RID: 24758 RVA: 0x00143808 File Offset: 0x00141A08
		public void Clear()
		{
			int count = this._count;
			if (count > 0)
			{
				Array.Clear(this._buckets, 0, this._buckets.Length);
				this._count = 0;
				this._freeList = -1;
				this._freeCount = 0;
				Array.Clear(this._entries, 0, count);
			}
			this._version++;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x060060B7 RID: 24759 RVA: 0x00143864 File Offset: 0x00141A64
		public bool ContainsKey(TKey key)
		{
			return this.FindEntry(key) >= 0;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains a specific value.</summary>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />. The value can be <see langword="null" /> for reference types.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified value; otherwise, <see langword="false" />.</returns>
		// Token: 0x060060B8 RID: 24760 RVA: 0x00143874 File Offset: 0x00141A74
		public bool ContainsValue(TValue value)
		{
			Dictionary<TKey, TValue>.Entry[] entries = this._entries;
			if (value == null)
			{
				for (int i = 0; i < this._count; i++)
				{
					if (entries[i].hashCode >= 0 && entries[i].value == null)
					{
						return true;
					}
				}
			}
			else if (default(TValue) != null)
			{
				for (int j = 0; j < this._count; j++)
				{
					if (entries[j].hashCode >= 0 && EqualityComparer<TValue>.Default.Equals(entries[j].value, value))
					{
						return true;
					}
				}
			}
			else
			{
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				for (int k = 0; k < this._count; k++)
				{
					if (entries[k].hashCode >= 0 && @default.Equals(entries[k].value, value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x00143960 File Offset: 0x00141B60
		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (index > array.Length)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			int count = this._count;
			Dictionary<TKey, TValue>.Entry[] entries = this._entries;
			for (int i = 0; i < count; i++)
			{
				if (entries[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
				}
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x060060BA RID: 24762 RVA: 0x001439E6 File Offset: 0x00141BE6
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x001439EF File Offset: 0x00141BEF
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x060060BC RID: 24764 RVA: 0x00143A00 File Offset: 0x00141C00
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
			}
			info.AddValue("Version", this._version);
			info.AddValue("Comparer", this._comparer ?? EqualityComparer<TKey>.Default, typeof(IEqualityComparer<TKey>));
			info.AddValue("HashSize", (this._buckets == null) ? 0 : this._buckets.Length);
			if (this._buckets != null)
			{
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
			}
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x00143A9C File Offset: 0x00141C9C
		private int FindEntry(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			int num = -1;
			int[] buckets = this._buckets;
			Dictionary<TKey, TValue>.Entry[] entries = this._entries;
			int num2 = 0;
			if (buckets != null)
			{
				IEqualityComparer<TKey> comparer = this._comparer;
				if (comparer == null)
				{
					int num3 = key.GetHashCode() & int.MaxValue;
					num = buckets[num3 % buckets.Length] - 1;
					if (default(TKey) != null)
					{
						while (num < entries.Length && (entries[num].hashCode != num3 || !EqualityComparer<TKey>.Default.Equals(entries[num].key, key)))
						{
							num = entries[num].next;
							if (num2 >= entries.Length)
							{
								ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
							}
							num2++;
						}
					}
					else
					{
						EqualityComparer<TKey> @default = EqualityComparer<TKey>.Default;
						while (num < entries.Length && (entries[num].hashCode != num3 || !@default.Equals(entries[num].key, key)))
						{
							num = entries[num].next;
							if (num2 >= entries.Length)
							{
								ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
							}
							num2++;
						}
					}
				}
				else
				{
					int num4 = comparer.GetHashCode(key) & int.MaxValue;
					num = buckets[num4 % buckets.Length] - 1;
					while (num < entries.Length && (entries[num].hashCode != num4 || !comparer.Equals(entries[num].key, key)))
					{
						num = entries[num].next;
						if (num2 >= entries.Length)
						{
							ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
						}
						num2++;
					}
				}
			}
			return num;
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x00143C20 File Offset: 0x00141E20
		private int Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			this._freeList = -1;
			this._buckets = new int[prime];
			this._entries = new Dictionary<TKey, TValue>.Entry[prime];
			return prime;
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x00143C54 File Offset: 0x00141E54
		private bool TryInsert(TKey key, TValue value, InsertionBehavior behavior)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			this._version++;
			if (this._buckets == null)
			{
				this.Initialize(0);
			}
			Dictionary<TKey, TValue>.Entry[] array = this._entries;
			IEqualityComparer<TKey> comparer = this._comparer;
			int num = ((comparer == null) ? key.GetHashCode() : comparer.GetHashCode(key)) & int.MaxValue;
			int num2 = 0;
			ref int ptr = ref this._buckets[num % this._buckets.Length];
			int i = ptr - 1;
			if (comparer == null)
			{
				if (default(TKey) != null)
				{
					while (i < array.Length)
					{
						if (array[i].hashCode == num && EqualityComparer<TKey>.Default.Equals(array[i].key, key))
						{
							if (behavior == InsertionBehavior.OverwriteExisting)
							{
								array[i].value = value;
								return true;
							}
							if (behavior == InsertionBehavior.ThrowOnExisting)
							{
								ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
							}
							return false;
						}
						else
						{
							i = array[i].next;
							if (num2 >= array.Length)
							{
								ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
							}
							num2++;
						}
					}
				}
				else
				{
					EqualityComparer<TKey> @default = EqualityComparer<TKey>.Default;
					while (i < array.Length)
					{
						if (array[i].hashCode == num && @default.Equals(array[i].key, key))
						{
							if (behavior == InsertionBehavior.OverwriteExisting)
							{
								array[i].value = value;
								return true;
							}
							if (behavior == InsertionBehavior.ThrowOnExisting)
							{
								ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
							}
							return false;
						}
						else
						{
							i = array[i].next;
							if (num2 >= array.Length)
							{
								ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
							}
							num2++;
						}
					}
				}
			}
			else
			{
				while (i < array.Length)
				{
					if (array[i].hashCode == num && comparer.Equals(array[i].key, key))
					{
						if (behavior == InsertionBehavior.OverwriteExisting)
						{
							array[i].value = value;
							return true;
						}
						if (behavior == InsertionBehavior.ThrowOnExisting)
						{
							ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
						}
						return false;
					}
					else
					{
						i = array[i].next;
						if (num2 >= array.Length)
						{
							ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
						}
						num2++;
					}
				}
			}
			bool flag = false;
			bool flag2 = false;
			int num3;
			if (this._freeCount > 0)
			{
				num3 = this._freeList;
				flag2 = true;
				this._freeCount--;
			}
			else
			{
				int count = this._count;
				if (count == array.Length)
				{
					this.Resize();
					flag = true;
				}
				num3 = count;
				this._count = count + 1;
				array = this._entries;
			}
			ref int ptr2 = (ref flag ? ref this._buckets[num % this._buckets.Length] : ref ptr);
			ref Dictionary<TKey, TValue>.Entry ptr3 = ref array[num3];
			if (flag2)
			{
				this._freeList = ptr3.next;
			}
			ptr3.hashCode = num;
			ptr3.next = ptr2 - 1;
			ptr3.key = key;
			ptr3.value = value;
			ptr2 = num3 + 1;
			return true;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2" /> instance is invalid.</exception>
		// Token: 0x060060C0 RID: 24768 RVA: 0x00143F18 File Offset: 0x00142118
		public virtual void OnDeserialization(object sender)
		{
			SerializationInfo serializationInfo;
			HashHelpers.SerializationInfoTable.TryGetValue(this, out serializationInfo);
			if (serializationInfo == null)
			{
				return;
			}
			int @int = serializationInfo.GetInt32("Version");
			int int2 = serializationInfo.GetInt32("HashSize");
			this._comparer = (IEqualityComparer<TKey>)serializationInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			if (int2 != 0)
			{
				this.Initialize(int2);
				KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])serializationInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
				if (array == null)
				{
					ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeys);
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Key == null)
					{
						ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
					}
					this.Add(array[i].Key, array[i].Value);
				}
			}
			else
			{
				this._buckets = null;
			}
			this._version = @int;
			HashHelpers.SerializationInfoTable.Remove(this);
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x00144008 File Offset: 0x00142208
		private void Resize()
		{
			this.Resize(HashHelpers.ExpandPrime(this._count), false);
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x0014401C File Offset: 0x0014221C
		private void Resize(int newSize, bool forceNewHashCodes)
		{
			int[] array = new int[newSize];
			Dictionary<TKey, TValue>.Entry[] array2 = new Dictionary<TKey, TValue>.Entry[newSize];
			int count = this._count;
			Array.Copy(this._entries, 0, array2, 0, count);
			if (default(TKey) == null && forceNewHashCodes)
			{
				for (int i = 0; i < count; i++)
				{
					if (array2[i].hashCode >= 0)
					{
						array2[i].hashCode = array2[i].key.GetHashCode() & int.MaxValue;
					}
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (array2[j].hashCode >= 0)
				{
					int num = array2[j].hashCode % newSize;
					array2[j].next = array[num] - 1;
					array[num] = j + 1;
				}
			}
			this._buckets = array;
			this._entries = array2;
		}

		/// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		///   <see langword="true" /> if the element is successfully found and removed; otherwise, <see langword="false" />.  This method returns <see langword="false" /> if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x060060C3 RID: 24771 RVA: 0x00144108 File Offset: 0x00142308
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this._buckets != null)
			{
				IEqualityComparer<TKey> comparer = this._comparer;
				int num = ((comparer != null) ? comparer.GetHashCode(key) : key.GetHashCode()) & int.MaxValue;
				int num2 = num % this._buckets.Length;
				int num3 = -1;
				ref Dictionary<TKey, TValue>.Entry ptr;
				for (int i = this._buckets[num2] - 1; i >= 0; i = ptr.next)
				{
					ptr = ref this._entries[i];
					if (ptr.hashCode == num)
					{
						IEqualityComparer<TKey> comparer2 = this._comparer;
						if ((comparer2 != null) ? comparer2.Equals(ptr.key, key) : EqualityComparer<TKey>.Default.Equals(ptr.key, key))
						{
							if (num3 < 0)
							{
								this._buckets[num2] = ptr.next + 1;
							}
							else
							{
								this._entries[num3].next = ptr.next;
							}
							ptr.hashCode = -1;
							ptr.next = this._freeList;
							if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
							{
								ptr.key = default(TKey);
							}
							if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
							{
								ptr.value = default(TValue);
							}
							this._freeList = i;
							this._freeCount++;
							this._version++;
							return true;
						}
					}
					num3 = i;
				}
			}
			return false;
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x00144260 File Offset: 0x00142460
		public bool Remove(TKey key, out TValue value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this._buckets != null)
			{
				IEqualityComparer<TKey> comparer = this._comparer;
				int num = ((comparer != null) ? comparer.GetHashCode(key) : key.GetHashCode()) & int.MaxValue;
				int num2 = num % this._buckets.Length;
				int num3 = -1;
				ref Dictionary<TKey, TValue>.Entry ptr;
				for (int i = this._buckets[num2] - 1; i >= 0; i = ptr.next)
				{
					ptr = ref this._entries[i];
					if (ptr.hashCode == num)
					{
						IEqualityComparer<TKey> comparer2 = this._comparer;
						if ((comparer2 != null) ? comparer2.Equals(ptr.key, key) : EqualityComparer<TKey>.Default.Equals(ptr.key, key))
						{
							if (num3 < 0)
							{
								this._buckets[num2] = ptr.next + 1;
							}
							else
							{
								this._entries[num3].next = ptr.next;
							}
							value = ptr.value;
							ptr.hashCode = -1;
							ptr.next = this._freeList;
							if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
							{
								ptr.key = default(TKey);
							}
							if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
							{
								ptr.value = default(TValue);
							}
							this._freeList = i;
							this._freeCount++;
							this._version++;
							return true;
						}
					}
					num3 = i;
				}
			}
			value = default(TValue);
			return false;
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x060060C5 RID: 24773 RVA: 0x001443CC File Offset: 0x001425CC
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.FindEntry(key);
			if (num >= 0)
			{
				value = this._entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x00144406 File Offset: 0x00142606
		public bool TryAdd(TKey key, TValue value)
		{
			return this.TryInsert(key, value, InsertionBehavior.None);
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x060060C7 RID: 24775 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x00144411 File Offset: 0x00142611
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyTo(array, index);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// <paramref name="array" /> does not have zero-based indexing.  
		/// -or-  
		/// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.  
		/// -or-  
		/// The type of the source <see cref="T:System.Collections.Generic.ICollection`1" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x060060C9 RID: 24777 RVA: 0x0014441C File Offset: 0x0014261C
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index > array.Length)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			DictionaryEntry[] array3 = array as DictionaryEntry[];
			if (array3 != null)
			{
				Dictionary<TKey, TValue>.Entry[] entries = this._entries;
				for (int i = 0; i < this._count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
					}
				}
				return;
			}
			object[] array4 = array as object[];
			if (array4 == null)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
			try
			{
				int count = this._count;
				Dictionary<TKey, TValue>.Entry[] entries2 = this._entries;
				for (int j = 0; j < count; j++)
				{
					if (entries2[j].hashCode >= 0)
					{
						array4[index++] = new KeyValuePair<TKey, TValue>(entries2[j].key, entries2[j].value);
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x060060CA RID: 24778 RVA: 0x001439EF File Offset: 0x00141BEF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0014457C File Offset: 0x0014277C
		public int EnsureCapacity(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			int num = ((this._entries == null) ? 0 : this._entries.Length);
			if (num >= capacity)
			{
				return num;
			}
			if (this._buckets == null)
			{
				return this.Initialize(capacity);
			}
			int prime = HashHelpers.GetPrime(capacity);
			this.Resize(prime, false);
			return prime;
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x001445CE File Offset: 0x001427CE
		public void TrimExcess()
		{
			this.TrimExcess(this.Count);
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x001445DC File Offset: 0x001427DC
		public void TrimExcess(int capacity)
		{
			if (capacity < this.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			int prime = HashHelpers.GetPrime(capacity);
			Dictionary<TKey, TValue>.Entry[] entries = this._entries;
			int num = ((entries == null) ? 0 : entries.Length);
			if (prime >= num)
			{
				return;
			}
			int count = this._count;
			this.Initialize(prime);
			Dictionary<TKey, TValue>.Entry[] entries2 = this._entries;
			int[] buckets = this._buckets;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				int hashCode = entries[i].hashCode;
				if (hashCode >= 0)
				{
					Dictionary<TKey, TValue>.Entry[] array = entries2;
					int num3 = num2;
					array[num3] = entries[i];
					int num4 = hashCode % prime;
					array[num3].next = buckets[num4] - 1;
					buckets[num4] = num2 + 1;
					num2++;
				}
			}
			this._count = num2;
			this._freeCount = 0;
		}

		/// <summary>Gets a value that indicates whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///   <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x060060CE RID: 24782 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x060060CF RID: 24783 RVA: 0x001446A3 File Offset: 0x001428A3
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x060060D0 RID: 24784 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x060060D1 RID: 24785 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060060D2 RID: 24786 RVA: 0x001446C5 File Offset: 0x001428C5
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x001446CD File Offset: 0x001428CD
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		/// <summary>Gets or sets the value with the specified key.</summary>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key, or <see langword="null" /> if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.  
		///  -or-  
		///  A value is being assigned, and <paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		// Token: 0x17001129 RID: 4393
		object IDictionary.this[object key]
		{
			get
			{
				if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
				{
					int num = this.FindEntry((TKey)((object)key));
					if (num >= 0)
					{
						return this._entries[num].value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
				try
				{
					TKey tkey = (TKey)((object)key);
					try
					{
						this[tkey] = (TValue)((object)value);
					}
					catch (InvalidCastException)
					{
						ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
					}
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
				}
			}
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x00144790 File Offset: 0x00142990
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">The object to use as the key.</param>
		/// <param name="value">The object to use as the value.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.  
		/// -or-  
		/// <paramref name="value" /> is of a type that is not assignable to <paramref name="TValue" />, the type of values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.  
		/// -or-  
		/// A value with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		// Token: 0x060060D7 RID: 24791 RVA: 0x001447A4 File Offset: 0x001429A4
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
			try
			{
				TKey tkey = (TKey)((object)key);
				try
				{
					this.Add(tkey, (TValue)((object)value));
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
				}
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x060060D8 RID: 24792 RVA: 0x0014481C File Offset: 0x00142A1C
		bool IDictionary.Contains(object key)
		{
			return Dictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
		// Token: 0x060060D9 RID: 24793 RVA: 0x00144834 File Offset: 0x00142A34
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 1);
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x060060DA RID: 24794 RVA: 0x00144842 File Offset: 0x00142A42
		void IDictionary.Remove(object key)
		{
			if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x040039BF RID: 14783
		private int[] _buckets;

		// Token: 0x040039C0 RID: 14784
		private Dictionary<TKey, TValue>.Entry[] _entries;

		// Token: 0x040039C1 RID: 14785
		private int _count;

		// Token: 0x040039C2 RID: 14786
		private int _freeList;

		// Token: 0x040039C3 RID: 14787
		private int _freeCount;

		// Token: 0x040039C4 RID: 14788
		private int _version;

		// Token: 0x040039C5 RID: 14789
		private IEqualityComparer<TKey> _comparer;

		// Token: 0x040039C6 RID: 14790
		private Dictionary<TKey, TValue>.KeyCollection _keys;

		// Token: 0x040039C7 RID: 14791
		private Dictionary<TKey, TValue>.ValueCollection _values;

		// Token: 0x040039C8 RID: 14792
		private object _syncRoot;

		// Token: 0x040039C9 RID: 14793
		private const string VersionName = "Version";

		// Token: 0x040039CA RID: 14794
		private const string HashSizeName = "HashSize";

		// Token: 0x040039CB RID: 14795
		private const string KeyValuePairsName = "KeyValuePairs";

		// Token: 0x040039CC RID: 14796
		private const string ComparerName = "Comparer";

		// Token: 0x02000A8A RID: 2698
		private struct Entry
		{
			// Token: 0x040039CD RID: 14797
			public int hashCode;

			// Token: 0x040039CE RID: 14798
			public int next;

			// Token: 0x040039CF RID: 14799
			public TKey key;

			// Token: 0x040039D0 RID: 14800
			public TValue value;
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <typeparam name="TKey" />
		/// <typeparam name="TValue" />
		// Token: 0x02000A8B RID: 2699
		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator, IDictionaryEnumerator
		{
			// Token: 0x060060DB RID: 24795 RVA: 0x00144859 File Offset: 0x00142A59
			internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this._dictionary = dictionary;
				this._version = dictionary._version;
				this._index = 0;
				this._getEnumeratorRetType = getEnumeratorRetType;
				this._current = default(KeyValuePair<TKey, TValue>);
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <returns>
			///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			// Token: 0x060060DC RID: 24796 RVA: 0x00144888 File Offset: 0x00142A88
			public bool MoveNext()
			{
				if (this._version != this._dictionary._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}
				while (this._index < this._dictionary._count)
				{
					Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
					int index = this._index;
					this._index = index + 1;
					ref Dictionary<TKey, TValue>.Entry ptr = ref entries[index];
					if (ptr.hashCode >= 0)
					{
						this._current = new KeyValuePair<TKey, TValue>(ptr.key, ptr.value);
						return true;
					}
				}
				this._index = this._dictionary._count + 1;
				this._current = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2" /> at the current position of the enumerator.</returns>
			// Token: 0x1700112A RID: 4394
			// (get) Token: 0x060060DD RID: 24797 RVA: 0x00144926 File Offset: 0x00142B26
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this._current;
				}
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" />.</summary>
			// Token: 0x060060DE RID: 24798 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator, as an <see cref="T:System.Object" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
			// Token: 0x1700112B RID: 4395
			// (get) Token: 0x060060DF RID: 24799 RVA: 0x00144930 File Offset: 0x00142B30
			object IEnumerator.Current
			{
				get
				{
					if (this._index == 0 || this._index == this._dictionary._count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}
					if (this._getEnumeratorRetType == 1)
					{
						return new DictionaryEntry(this._current.Key, this._current.Value);
					}
					return new KeyValuePair<TKey, TValue>(this._current.Key, this._current.Value);
				}
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			// Token: 0x060060E0 RID: 24800 RVA: 0x001449B3 File Offset: 0x00142BB3
			void IEnumerator.Reset()
			{
				if (this._version != this._dictionary._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}
				this._index = 0;
				this._current = default(KeyValuePair<TKey, TValue>);
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the dictionary at the current position of the enumerator, as a <see cref="T:System.Collections.DictionaryEntry" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
			// Token: 0x1700112C RID: 4396
			// (get) Token: 0x060060E1 RID: 24801 RVA: 0x001449E0 File Offset: 0x00142BE0
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this._index == 0 || this._index == this._dictionary._count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}
					return new DictionaryEntry(this._current.Key, this._current.Value);
				}
			}

			/// <summary>Gets the key of the element at the current position of the enumerator.</summary>
			/// <returns>The key of the element in the dictionary at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
			// Token: 0x1700112D RID: 4397
			// (get) Token: 0x060060E2 RID: 24802 RVA: 0x00144A34 File Offset: 0x00142C34
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this._index == 0 || this._index == this._dictionary._count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}
					return this._current.Key;
				}
			}

			/// <summary>Gets the value of the element at the current position of the enumerator.</summary>
			/// <returns>The value of the element in the dictionary at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
			// Token: 0x1700112E RID: 4398
			// (get) Token: 0x060060E3 RID: 24803 RVA: 0x00144A68 File Offset: 0x00142C68
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this._index == 0 || this._index == this._dictionary._count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}
					return this._current.Value;
				}
			}

			// Token: 0x040039D1 RID: 14801
			private Dictionary<TKey, TValue> _dictionary;

			// Token: 0x040039D2 RID: 14802
			private int _version;

			// Token: 0x040039D3 RID: 14803
			private int _index;

			// Token: 0x040039D4 RID: 14804
			private KeyValuePair<TKey, TValue> _current;

			// Token: 0x040039D5 RID: 14805
			private int _getEnumeratorRetType;

			// Token: 0x040039D6 RID: 14806
			internal const int DictEntry = 1;

			// Token: 0x040039D7 RID: 14807
			internal const int KeyValuePair = 2;
		}

		/// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited.</summary>
		/// <typeparam name="TKey" />
		/// <typeparam name="TValue" />
		// Token: 0x02000A8C RID: 2700
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(DictionaryKeyCollectionDebugView<, >))]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose keys are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is <see langword="null" />.</exception>
			// Token: 0x060060E4 RID: 24804 RVA: 0x00144A9C File Offset: 0x00142C9C
			public KeyCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this._dictionary = dictionary;
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</returns>
			// Token: 0x060060E5 RID: 24805 RVA: 0x00144AB4 File Offset: 0x00142CB4
			public Dictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			// Token: 0x060060E6 RID: 24806 RVA: 0x00144AC4 File Offset: 0x00142CC4
			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
				}
				if (array.Length - index < this._dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this._dictionary._count;
				Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.  
			///  Retrieving the value of this property is an O(1) operation.</returns>
			// Token: 0x1700112F RID: 4399
			// (get) Token: 0x060060E7 RID: 24807 RVA: 0x00144B4C File Offset: 0x00142D4C
			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
			/// <returns>
			///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns <see langword="true" />.</returns>
			// Token: 0x17001130 RID: 4400
			// (get) Token: 0x060060E8 RID: 24808 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060E9 RID: 24809 RVA: 0x00144B59 File Offset: 0x00142D59
			void ICollection<TKey>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060EA RID: 24810 RVA: 0x00144B59 File Offset: 0x00142D59
			void ICollection<TKey>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
			/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x060060EB RID: 24811 RVA: 0x00144B62 File Offset: 0x00142D62
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this._dictionary.ContainsKey(item);
			}

			/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if item was not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060EC RID: 24812 RVA: 0x00144B70 File Offset: 0x00142D70
			bool ICollection<TKey>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
			// Token: 0x060060ED RID: 24813 RVA: 0x00144B7A File Offset: 0x00142D7A
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			// Token: 0x060060EE RID: 24814 RVA: 0x00144B7A File Offset: 0x00142D7A
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// <paramref name="array" /> does not have zero-based indexing.  
			/// -or-  
			/// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.  
			/// -or-  
			/// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			// Token: 0x060060EF RID: 24815 RVA: 0x00144B8C File Offset: 0x00142D8C
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if (array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if (index > array.Length)
				{
					ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
				}
				if (array.Length - index < this._dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
				}
				int count = this._dictionary._count;
				Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///   <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns <see langword="false" />.</returns>
			// Token: 0x17001131 RID: 4401
			// (get) Token: 0x060060F0 RID: 24816 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns the current instance.</returns>
			// Token: 0x17001132 RID: 4402
			// (get) Token: 0x060060F1 RID: 24817 RVA: 0x00144C78 File Offset: 0x00142E78
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dictionary).SyncRoot;
				}
			}

			// Token: 0x040039D8 RID: 14808
			private Dictionary<TKey, TValue> _dictionary;

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			/// <typeparam name="TKey" />
			/// <typeparam name="TValue" />
			// Token: 0x02000A8D RID: 2701
			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				// Token: 0x060060F2 RID: 24818 RVA: 0x00144C85 File Offset: 0x00142E85
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this._dictionary = dictionary;
					this._version = dictionary._version;
					this._index = 0;
					this._currentKey = default(TKey);
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" />.</summary>
				// Token: 0x060060F3 RID: 24819 RVA: 0x00004BF9 File Offset: 0x00002DF9
				public void Dispose()
				{
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
				/// <returns>
				///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
				// Token: 0x060060F4 RID: 24820 RVA: 0x00144CB0 File Offset: 0x00142EB0
				public bool MoveNext()
				{
					if (this._version != this._dictionary._version)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
					}
					while (this._index < this._dictionary._count)
					{
						Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
						int index = this._index;
						this._index = index + 1;
						ref Dictionary<TKey, TValue>.Entry ptr = ref entries[index];
						if (ptr.hashCode >= 0)
						{
							this._currentKey = ptr.key;
							return true;
						}
					}
					this._index = this._dictionary._count + 1;
					this._currentKey = default(TKey);
					return false;
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> at the current position of the enumerator.</returns>
				// Token: 0x17001133 RID: 4403
				// (get) Token: 0x060060F5 RID: 24821 RVA: 0x00144D43 File Offset: 0x00142F43
				public TKey Current
				{
					get
					{
						return this._currentKey;
					}
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
				// Token: 0x17001134 RID: 4404
				// (get) Token: 0x060060F6 RID: 24822 RVA: 0x00144D4B File Offset: 0x00142F4B
				object IEnumerator.Current
				{
					get
					{
						if (this._index == 0 || this._index == this._dictionary._count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
						}
						return this._currentKey;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
				// Token: 0x060060F7 RID: 24823 RVA: 0x00144D7A File Offset: 0x00142F7A
				void IEnumerator.Reset()
				{
					if (this._version != this._dictionary._version)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
					}
					this._index = 0;
					this._currentKey = default(TKey);
				}

				// Token: 0x040039D9 RID: 14809
				private Dictionary<TKey, TValue> _dictionary;

				// Token: 0x040039DA RID: 14810
				private int _index;

				// Token: 0x040039DB RID: 14811
				private int _version;

				// Token: 0x040039DC RID: 14812
				private TKey _currentKey;
			}
		}

		/// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited.</summary>
		/// <typeparam name="TKey" />
		/// <typeparam name="TValue" />
		// Token: 0x02000A8E RID: 2702
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(DictionaryValueCollectionDebugView<, >))]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> class that reflects the values in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose values are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is <see langword="null" />.</exception>
			// Token: 0x060060F8 RID: 24824 RVA: 0x00144DA7 File Offset: 0x00142FA7
			public ValueCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this._dictionary = dictionary;
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
			// Token: 0x060060F9 RID: 24825 RVA: 0x00144DBF File Offset: 0x00142FBF
			public Dictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			// Token: 0x060060FA RID: 24826 RVA: 0x00144DCC File Offset: 0x00142FCC
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
				}
				if (array.Length - index < this._dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this._dictionary._count;
				Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
			// Token: 0x17001135 RID: 4405
			// (get) Token: 0x060060FB RID: 24827 RVA: 0x00144E54 File Offset: 0x00143054
			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
			/// <returns>
			///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns <see langword="true" />.</returns>
			// Token: 0x17001136 RID: 4406
			// (get) Token: 0x060060FC RID: 24828 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060FD RID: 24829 RVA: 0x00144E61 File Offset: 0x00143061
			void ICollection<TValue>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> was not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060FE RID: 24830 RVA: 0x00144E6A File Offset: 0x0014306A
			bool ICollection<TValue>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x060060FF RID: 24831 RVA: 0x00144E61 File Offset: 0x00143061
			void ICollection<TValue>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
			/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006100 RID: 24832 RVA: 0x00144E74 File Offset: 0x00143074
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this._dictionary.ContainsValue(item);
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
			// Token: 0x06006101 RID: 24833 RVA: 0x00144E82 File Offset: 0x00143082
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			// Token: 0x06006102 RID: 24834 RVA: 0x00144E82 File Offset: 0x00143082
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// <paramref name="array" /> does not have zero-based indexing.  
			/// -or-  
			/// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.  
			/// -or-  
			/// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			// Token: 0x06006103 RID: 24835 RVA: 0x00144E94 File Offset: 0x00143094
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if (array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if (index > array.Length)
				{
					ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
				}
				if (array.Length - index < this._dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
				}
				int count = this._dictionary._count;
				Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///   <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns <see langword="false" />.</returns>
			// Token: 0x17001137 RID: 4407
			// (get) Token: 0x06006104 RID: 24836 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns the current instance.</returns>
			// Token: 0x17001138 RID: 4408
			// (get) Token: 0x06006105 RID: 24837 RVA: 0x00144F80 File Offset: 0x00143180
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dictionary).SyncRoot;
				}
			}

			// Token: 0x040039DD RID: 14813
			private Dictionary<TKey, TValue> _dictionary;

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			/// <typeparam name="TKey" />
			/// <typeparam name="TValue" />
			// Token: 0x02000A8F RID: 2703
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				// Token: 0x06006106 RID: 24838 RVA: 0x00144F8D File Offset: 0x0014318D
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this._dictionary = dictionary;
					this._version = dictionary._version;
					this._index = 0;
					this._currentValue = default(TValue);
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" />.</summary>
				// Token: 0x06006107 RID: 24839 RVA: 0x00004BF9 File Offset: 0x00002DF9
				public void Dispose()
				{
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
				/// <returns>
				///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
				// Token: 0x06006108 RID: 24840 RVA: 0x00144FB8 File Offset: 0x001431B8
				public bool MoveNext()
				{
					if (this._version != this._dictionary._version)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
					}
					while (this._index < this._dictionary._count)
					{
						Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
						int index = this._index;
						this._index = index + 1;
						ref Dictionary<TKey, TValue>.Entry ptr = ref entries[index];
						if (ptr.hashCode >= 0)
						{
							this._currentValue = ptr.value;
							return true;
						}
					}
					this._index = this._dictionary._count + 1;
					this._currentValue = default(TValue);
					return false;
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> at the current position of the enumerator.</returns>
				// Token: 0x17001139 RID: 4409
				// (get) Token: 0x06006109 RID: 24841 RVA: 0x0014504B File Offset: 0x0014324B
				public TValue Current
				{
					get
					{
						return this._currentValue;
					}
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
				// Token: 0x1700113A RID: 4410
				// (get) Token: 0x0600610A RID: 24842 RVA: 0x00145053 File Offset: 0x00143253
				object IEnumerator.Current
				{
					get
					{
						if (this._index == 0 || this._index == this._dictionary._count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
						}
						return this._currentValue;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
				// Token: 0x0600610B RID: 24843 RVA: 0x00145082 File Offset: 0x00143282
				void IEnumerator.Reset()
				{
					if (this._version != this._dictionary._version)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
					}
					this._index = 0;
					this._currentValue = default(TValue);
				}

				// Token: 0x040039DE RID: 14814
				private Dictionary<TKey, TValue> _dictionary;

				// Token: 0x040039DF RID: 14815
				private int _index;

				// Token: 0x040039E0 RID: 14816
				private int _version;

				// Token: 0x040039E1 RID: 14817
				private TValue _currentValue;
			}
		}
	}
}
