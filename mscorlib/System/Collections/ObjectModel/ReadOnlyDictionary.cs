using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity;

namespace System.Collections.ObjectModel
{
	/// <summary>Represents a read-only, generic collection of key/value pairs.</summary>
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	// Token: 0x02000A83 RID: 2691
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(DictionaryDebugView<, >))]
	[Serializable]
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" /> class that is a wrapper around the specified dictionary.</summary>
		/// <param name="dictionary">The dictionary to wrap.</param>
		// Token: 0x06006054 RID: 24660 RVA: 0x00142E2E File Offset: 0x0014102E
		public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.m_dictionary = dictionary;
		}

		/// <summary>Gets the dictionary that is wrapped by this <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" /> object.</summary>
		/// <returns>The dictionary that is wrapped by this object.</returns>
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06006055 RID: 24661 RVA: 0x00142E4B File Offset: 0x0014104B
		protected IDictionary<TKey, TValue> Dictionary
		{
			get
			{
				return this.m_dictionary;
			}
		}

		/// <summary>Gets a key collection that contains the keys of the dictionary.</summary>
		/// <returns>A key collection that contains the keys of the dictionary.</returns>
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06006056 RID: 24662 RVA: 0x00142E53 File Offset: 0x00141053
		public ReadOnlyDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new ReadOnlyDictionary<TKey, TValue>.KeyCollection(this.m_dictionary.Keys);
				}
				return this._keys;
			}
		}

		/// <summary>Gets a collection that contains the values in the dictionary.</summary>
		/// <returns>A collection that contains the values in the object that implements <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" />.</returns>
		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06006057 RID: 24663 RVA: 0x00142E79 File Offset: 0x00141079
		public ReadOnlyDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new ReadOnlyDictionary<TKey, TValue>.ValueCollection(this.m_dictionary.Values);
				}
				return this._values;
			}
		}

		/// <summary>Determines whether the dictionary contains an element that has the specified key.</summary>
		/// <param name="key">The key to locate in the dictionary.</param>
		/// <returns>
		///   <see langword="true" /> if the dictionary contains an element that has the specified key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006058 RID: 24664 RVA: 0x00142E9F File Offset: 0x0014109F
		public bool ContainsKey(TKey key)
		{
			return this.m_dictionary.ContainsKey(key);
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06006059 RID: 24665 RVA: 0x00142EAD File Offset: 0x001410AD
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		/// <summary>Retrieves the value that is associated with the specified key.</summary>
		/// <param name="key">The key whose value will be retrieved.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
		/// <returns>
		///   <see langword="true" /> if the object that implements <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600605A RID: 24666 RVA: 0x00142EB5 File Offset: 0x001410B5
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.m_dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x00142EC4 File Offset: 0x001410C4
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		/// <summary>Gets the element that has the specified key.</summary>
		/// <param name="key">The key of the element to get.</param>
		/// <returns>The element that has the specified key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> is not found.</exception>
		// Token: 0x17001100 RID: 4352
		public TValue this[TKey key]
		{
			get
			{
				return this.m_dictionary[key];
			}
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x0013B59D File Offset: 0x0013979D
		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x17001101 RID: 4353
		TValue IDictionary<TKey, TValue>.this[TKey key]
		{
			get
			{
				return this.m_dictionary[key];
			}
			set
			{
				throw new NotSupportedException("Collection is read-only.");
			}
		}

		/// <summary>Gets the number of items in the dictionary.</summary>
		/// <returns>The number of items in the dictionary.</returns>
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x00142EDA File Offset: 0x001410DA
		public int Count
		{
			get
			{
				return this.m_dictionary.Count;
			}
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x00142EE7 File Offset: 0x001410E7
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.m_dictionary.Contains(item);
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x00142EF5 File Offset: 0x001410F5
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.m_dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x0013B59D File Offset: 0x0013979D
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x0013B59D File Offset: 0x0013979D
		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x0013B59D File Offset: 0x0013979D
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" />.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		// Token: 0x06006068 RID: 24680 RVA: 0x00142F04 File Offset: 0x00141104
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		// Token: 0x06006069 RID: 24681 RVA: 0x00142F11 File Offset: 0x00141111
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x00142F1E File Offset: 0x0014111E
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return key is TKey;
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		// Token: 0x0600606B RID: 24683 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Add(object key, object value)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		// Token: 0x0600606C RID: 24684 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Clear()
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		/// <summary>Determines whether the dictionary contains an element that has the specified key.</summary>
		/// <param name="key">The key to locate in the dictionary.</param>
		/// <returns>
		///   <see langword="true" /> if the dictionary contains an element that has the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x0600606D RID: 24685 RVA: 0x00142F37 File Offset: 0x00141137
		bool IDictionary.Contains(object key)
		{
			return ReadOnlyDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		/// <summary>Returns an enumerator for the dictionary.</summary>
		/// <returns>An enumerator for the dictionary.</returns>
		// Token: 0x0600606E RID: 24686 RVA: 0x00142F50 File Offset: 0x00141150
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			IDictionary dictionary = this.m_dictionary as IDictionary;
			if (dictionary != null)
			{
				return dictionary.GetEnumerator();
			}
			return new ReadOnlyDictionary<TKey, TValue>.DictionaryEnumerator(this.m_dictionary);
		}

		/// <summary>Gets a value that indicates whether the dictionary has a fixed size.</summary>
		/// <returns>
		///   <see langword="true" /> if the dictionary has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600606F RID: 24687 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that indicates whether the dictionary is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> in all cases.</returns>
		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a collection that contains the keys of the dictionary.</summary>
		/// <returns>A collection that contains the keys of the dictionary.</returns>
		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06006071 RID: 24689 RVA: 0x00142EAD File Offset: 0x001410AD
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		// Token: 0x06006072 RID: 24690 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Remove(object key)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		/// <summary>Gets a collection that contains the values in the dictionary.</summary>
		/// <returns>A collection that contains the values in the dictionary.</returns>
		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06006073 RID: 24691 RVA: 0x00142EC4 File Offset: 0x001410C4
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		/// <summary>Gets the element that has the specified key.</summary>
		/// <param name="key">The key of the element to get or set.</param>
		/// <returns>The element that has the specified key.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set.  
		///  -or-  
		///  The property is set, <paramref name="key" /> does not exist in the collection, and the dictionary has a fixed size.</exception>
		// Token: 0x17001108 RID: 4360
		object IDictionary.this[object key]
		{
			get
			{
				if (ReadOnlyDictionary<TKey, TValue>.IsCompatibleKey(key))
				{
					return this[(TKey)((object)key)];
				}
				return null;
			}
			set
			{
				throw new NotSupportedException("Collection is read-only.");
			}
		}

		/// <summary>Copies the elements of the dictionary to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the dictionary. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the source dictionary is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.  
		/// -or-  
		/// The type of the source dictionary cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x06006076 RID: 24694 RVA: 0x00142FA0 File Offset: 0x001411A0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException("The lower bound of target array must be zero.");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.m_dictionary.CopyTo(array2, index);
				return;
			}
			DictionaryEntry[] array3 = array as DictionaryEntry[];
			if (array3 != null)
			{
				using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = this.m_dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						array3[index++] = new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
					}
					return;
				}
			}
			object[] array4 = array as object[];
			if (array4 == null)
			{
				throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
			}
			try
			{
				foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this.m_dictionary)
				{
					array4[index++] = new KeyValuePair<TKey, TValue>(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
			}
		}

		/// <summary>Gets a value that indicates whether access to the dictionary is synchronized (thread safe).</summary>
		/// <returns>
		///   <see langword="true" /> if access to the dictionary is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06006077 RID: 24695 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the dictionary.</summary>
		/// <returns>An object that can be used to synchronize access to the dictionary.</returns>
		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x00143128 File Offset: 0x00141328
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.m_dictionary as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06006079 RID: 24697 RVA: 0x00142EAD File Offset: 0x001410AD
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x00142EC4 File Offset: 0x001410C4
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x040039B1 RID: 14769
		private readonly IDictionary<TKey, TValue> m_dictionary;

		// Token: 0x040039B2 RID: 14770
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040039B3 RID: 14771
		[NonSerialized]
		private ReadOnlyDictionary<TKey, TValue>.KeyCollection _keys;

		// Token: 0x040039B4 RID: 14772
		[NonSerialized]
		private ReadOnlyDictionary<TKey, TValue>.ValueCollection _values;

		// Token: 0x02000A84 RID: 2692
		[Serializable]
		private struct DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600607B RID: 24699 RVA: 0x00143172 File Offset: 0x00141372
			public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
			{
				this._dictionary = dictionary;
				this._enumerator = this._dictionary.GetEnumerator();
			}

			// Token: 0x1700110D RID: 4365
			// (get) Token: 0x0600607C RID: 24700 RVA: 0x0014318C File Offset: 0x0014138C
			public DictionaryEntry Entry
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					object obj = keyValuePair.Key;
					keyValuePair = this._enumerator.Current;
					return new DictionaryEntry(obj, keyValuePair.Value);
				}
			}

			// Token: 0x1700110E RID: 4366
			// (get) Token: 0x0600607D RID: 24701 RVA: 0x001431D0 File Offset: 0x001413D0
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x1700110F RID: 4367
			// (get) Token: 0x0600607E RID: 24702 RVA: 0x001431F8 File Offset: 0x001413F8
			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x17001110 RID: 4368
			// (get) Token: 0x0600607F RID: 24703 RVA: 0x0014321D File Offset: 0x0014141D
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06006080 RID: 24704 RVA: 0x0014322A File Offset: 0x0014142A
			public bool MoveNext()
			{
				return this._enumerator.MoveNext();
			}

			// Token: 0x06006081 RID: 24705 RVA: 0x00143237 File Offset: 0x00141437
			public void Reset()
			{
				this._enumerator.Reset();
			}

			// Token: 0x040039B5 RID: 14773
			private readonly IDictionary<TKey, TValue> _dictionary;

			// Token: 0x040039B6 RID: 14774
			private IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;
		}

		/// <summary>Represents a read-only collection of the keys of a <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" /> object.</summary>
		/// <typeparam name="TKey" />
		/// <typeparam name="TValue" />
		// Token: 0x02000A85 RID: 2693
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
		{
			// Token: 0x06006082 RID: 24706 RVA: 0x00143244 File Offset: 0x00141444
			internal KeyCollection(ICollection<TKey> collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection");
				}
				this._collection = collection;
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <param name="item">The object to add to the collection.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006083 RID: 24707 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<TKey>.Add(TKey item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006084 RID: 24708 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Determines whether the collection contains a specific value.</summary>
			/// <param name="item">The object to locate in the collection.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> is found in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006085 RID: 24709 RVA: 0x00143261 File Offset: 0x00141461
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this._collection.Contains(item);
			}

			/// <summary>Copies the elements of the collection to an array, starting at a specific array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="arrayIndex" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.  
			/// -or-  
			/// Type <paramref name="T" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			// Token: 0x06006086 RID: 24710 RVA: 0x0014326F File Offset: 0x0014146F
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				this._collection.CopyTo(array, arrayIndex);
			}

			/// <summary>Gets the number of elements in the collection.</summary>
			/// <returns>The number of elements in the collection.</returns>
			// Token: 0x17001111 RID: 4369
			// (get) Token: 0x06006087 RID: 24711 RVA: 0x0014327E File Offset: 0x0014147E
			public int Count
			{
				get
				{
					return this._collection.Count;
				}
			}

			/// <summary>Gets a value that indicates whether the dictionary is read-only.</summary>
			/// <returns>
			///   <see langword="true" /> in all cases.</returns>
			// Token: 0x17001112 RID: 4370
			// (get) Token: 0x06006088 RID: 24712 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <param name="item">The object to remove from the collection.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the collection; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original collection.</returns>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006089 RID: 24713 RVA: 0x0013B59D File Offset: 0x0013979D
			bool ICollection<TKey>.Remove(TKey item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			// Token: 0x0600608A RID: 24714 RVA: 0x0014328B File Offset: 0x0014148B
			public IEnumerator<TKey> GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			// Token: 0x0600608B RID: 24715 RVA: 0x00143298 File Offset: 0x00141498
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			/// <summary>Copies the elements of the collection to an array, starting at a specific array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// The number of elements in the source collection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			// Token: 0x0600608C RID: 24716 RVA: 0x001432A5 File Offset: 0x001414A5
			void ICollection.CopyTo(Array array, int index)
			{
				ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TKey>(this._collection, array, index);
			}

			/// <summary>Gets a value that indicates whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///   <see langword="true" /> if access to the collection is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
			// Token: 0x17001113 RID: 4371
			// (get) Token: 0x0600608D RID: 24717 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>An object that can be used to synchronize access to the collection.</returns>
			// Token: 0x17001114 RID: 4372
			// (get) Token: 0x0600608E RID: 24718 RVA: 0x001432B4 File Offset: 0x001414B4
			object ICollection.SyncRoot
			{
				get
				{
					if (this._syncRoot == null)
					{
						ICollection collection = this._collection as ICollection;
						if (collection != null)
						{
							this._syncRoot = collection.SyncRoot;
						}
						else
						{
							Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
						}
					}
					return this._syncRoot;
				}
			}

			// Token: 0x0600608F RID: 24719 RVA: 0x000173AD File Offset: 0x000155AD
			internal KeyCollection()
			{
				ThrowStub.ThrowNotSupportedException();
			}

			// Token: 0x040039B7 RID: 14775
			private readonly ICollection<TKey> _collection;

			// Token: 0x040039B8 RID: 14776
			[NonSerialized]
			private object _syncRoot;
		}

		/// <summary>Represents a read-only collection of the values of a <see cref="T:System.Collections.ObjectModel.ReadOnlyDictionary`2" /> object.</summary>
		/// <typeparam name="TKey" />
		/// <typeparam name="TValue" />
		// Token: 0x02000A86 RID: 2694
		[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
		{
			// Token: 0x06006090 RID: 24720 RVA: 0x001432FE File Offset: 0x001414FE
			internal ValueCollection(ICollection<TValue> collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection");
				}
				this._collection = collection;
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <param name="item">The object to add to the collection.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006091 RID: 24721 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006092 RID: 24722 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Determines whether the collection contains a specific value.</summary>
			/// <param name="item">The object to locate in the collection.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> is found in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006093 RID: 24723 RVA: 0x0014331B File Offset: 0x0014151B
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this._collection.Contains(item);
			}

			/// <summary>Copies the elements of the collection to an array, starting at a specific array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="arrayIndex" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.  
			/// -or-  
			/// Type <paramref name="T" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			// Token: 0x06006094 RID: 24724 RVA: 0x00143329 File Offset: 0x00141529
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				this._collection.CopyTo(array, arrayIndex);
			}

			/// <summary>Gets the number of elements in the collection.</summary>
			/// <returns>The number of elements in the collection.</returns>
			// Token: 0x17001115 RID: 4373
			// (get) Token: 0x06006095 RID: 24725 RVA: 0x00143338 File Offset: 0x00141538
			public int Count
			{
				get
				{
					return this._collection.Count;
				}
			}

			/// <summary>Gets a value that indicates whether the collection is read-only.</summary>
			/// <returns>
			///   <see langword="true" /> in all cases.</returns>
			// Token: 0x17001116 RID: 4374
			// (get) Token: 0x06006096 RID: 24726 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Throws a <see cref="T:System.NotSupportedException" /> exception in all cases.</summary>
			/// <param name="item">The object to remove from the collection.</param>
			/// <returns>
			///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the collection; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if item is not found in the original collection.</returns>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06006097 RID: 24727 RVA: 0x0013B59D File Offset: 0x0013979D
			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			// Token: 0x06006098 RID: 24728 RVA: 0x00143345 File Offset: 0x00141545
			public IEnumerator<TValue> GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			// Token: 0x06006099 RID: 24729 RVA: 0x00143352 File Offset: 0x00141552
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			/// <summary>Copies the elements of the collection to an array, starting at a specific array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.  
			/// -or-  
			/// The number of elements in the source collection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			// Token: 0x0600609A RID: 24730 RVA: 0x0014335F File Offset: 0x0014155F
			void ICollection.CopyTo(Array array, int index)
			{
				ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TValue>(this._collection, array, index);
			}

			/// <summary>Gets a value that indicates whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///   <see langword="true" /> if access to the collection is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
			// Token: 0x17001117 RID: 4375
			// (get) Token: 0x0600609B RID: 24731 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>An object that can be used to synchronize access to the collection.</returns>
			// Token: 0x17001118 RID: 4376
			// (get) Token: 0x0600609C RID: 24732 RVA: 0x00143370 File Offset: 0x00141570
			object ICollection.SyncRoot
			{
				get
				{
					if (this._syncRoot == null)
					{
						ICollection collection = this._collection as ICollection;
						if (collection != null)
						{
							this._syncRoot = collection.SyncRoot;
						}
						else
						{
							Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
						}
					}
					return this._syncRoot;
				}
			}

			// Token: 0x0600609D RID: 24733 RVA: 0x000173AD File Offset: 0x000155AD
			internal ValueCollection()
			{
				ThrowStub.ThrowNotSupportedException();
			}

			// Token: 0x040039B9 RID: 14777
			private readonly ICollection<TValue> _collection;

			// Token: 0x040039BA RID: 14778
			[NonSerialized]
			private object _syncRoot;
		}
	}
}
