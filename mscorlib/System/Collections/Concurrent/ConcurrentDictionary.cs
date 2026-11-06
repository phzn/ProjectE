using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>Represents a thread-safe collection of key/value pairs that can be accessed by multiple threads concurrently.</summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	// Token: 0x02000A58 RID: 2648
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(IDictionaryDebugView<, >))]
	[Serializable]
	public class ConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06005EF3 RID: 24307 RVA: 0x0013F0E0 File Offset: 0x0013D2E0
		private static bool IsValueWriteAtomic()
		{
			Type typeFromHandle = typeof(TValue);
			if (!typeFromHandle.IsValueType)
			{
				return true;
			}
			switch (Type.GetTypeCode(typeFromHandle))
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Single:
				return true;
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Double:
				return IntPtr.Size == 8;
			default:
				return false;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that is empty, has the default concurrency level, has the default initial capacity, and uses the default comparer for the key type.</summary>
		// Token: 0x06005EF4 RID: 24308 RVA: 0x0013F14F File Offset: 0x0013D34F
		public ConcurrentDictionary()
			: this(ConcurrentDictionary<TKey, TValue>.DefaultConcurrencyLevel, 31, true, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that is empty, has the specified concurrency level and capacity, and uses the default comparer for the key type.</summary>
		/// <param name="concurrencyLevel">The estimated number of threads that will update the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> concurrently.</param>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="concurrencyLevel" /> is less than 1.  
		/// -or-  
		/// <paramref name="capacity" /> is less than 0.</exception>
		// Token: 0x06005EF5 RID: 24309 RVA: 0x0013F160 File Offset: 0x0013D360
		public ConcurrentDictionary(int concurrencyLevel, int capacity)
			: this(concurrencyLevel, capacity, false, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1" />, has the default concurrency level, has the default initial capacity, and uses the default comparer for the key type.</summary>
		/// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> or any of its keys is  <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="collection" /> contains one or more duplicate keys.</exception>
		// Token: 0x06005EF6 RID: 24310 RVA: 0x0013F16C File Offset: 0x0013D36C
		public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
			: this(collection, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that is empty, has the default concurrency level and capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="comparer">The equality comparison implementation to use when comparing keys.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="comparer" /> is <see langword="null" />.</exception>
		// Token: 0x06005EF7 RID: 24311 RVA: 0x0013F176 File Offset: 0x0013D376
		public ConcurrentDictionary(IEqualityComparer<TKey> comparer)
			: this(ConcurrentDictionary<TKey, TValue>.DefaultConcurrencyLevel, 31, true, comparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.IEnumerable" /> has the default concurrency level, has the default initial capacity, and uses the specified  <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> or <paramref name="comparer" /> is <see langword="null" />.</exception>
		// Token: 0x06005EF8 RID: 24312 RVA: 0x0013F187 File Offset: 0x0013D387
		public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
			: this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.IEnumerable" />, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="concurrencyLevel">The estimated number of threads that will update the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> concurrently.</param>
		/// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> or <paramref name="comparer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="concurrencyLevel" /> is less than 1.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="collection" /> contains one or more duplicate keys.</exception>
		// Token: 0x06005EF9 RID: 24313 RVA: 0x0013F1A5 File Offset: 0x0013D3A5
		public ConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
			: this(concurrencyLevel, 31, false, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x0013F1C8 File Offset: 0x0013D3C8
		private void InitializeFromCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in collection)
			{
				if (keyValuePair.Key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				if (!this.TryAddInternal(keyValuePair.Key, this._comparer.GetHashCode(keyValuePair.Key), keyValuePair.Value, false, false, out tvalue))
				{
					throw new ArgumentException("The source argument contains duplicate keys.");
				}
			}
			if (this._budget == 0)
			{
				this._budget = this._tables._buckets.Length / this._tables._locks.Length;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> class that is empty, has the specified concurrency level, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="concurrencyLevel">The estimated number of threads that will update the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> concurrently.</param>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> can contain.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="comparer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="concurrencyLevel" /> or <paramref name="capacity" /> is less than 1.</exception>
		// Token: 0x06005EFB RID: 24315 RVA: 0x0013F280 File Offset: 0x0013D480
		public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
			: this(concurrencyLevel, capacity, false, comparer)
		{
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x0013F28C File Offset: 0x0013D48C
		internal ConcurrentDictionary(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<TKey> comparer)
		{
			if (concurrencyLevel < 1)
			{
				throw new ArgumentOutOfRangeException("concurrencyLevel", "The concurrencyLevel argument must be positive.");
			}
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "The capacity argument must be greater than or equal to zero.");
			}
			if (capacity < concurrencyLevel)
			{
				capacity = concurrencyLevel;
			}
			object[] array = new object[concurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new object();
			}
			int[] array2 = new int[array.Length];
			ConcurrentDictionary<TKey, TValue>.Node[] array3 = new ConcurrentDictionary<TKey, TValue>.Node[capacity];
			this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(array3, array, array2);
			this._comparer = comparer ?? EqualityComparer<TKey>.Default;
			this._growLockArray = growLockArray;
			this._budget = array3.Length / array.Length;
		}

		/// <summary>Attempts to add the specified key and value to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be  <see langword="null" /> for reference types.</param>
		/// <returns>
		///   <see langword="true" /> if the key/value pair was added to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> successfully; <see langword="false" /> if the key already exists.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is  <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005EFD RID: 24317 RVA: 0x0013F330 File Offset: 0x0013D530
		public bool TryAdd(TKey key, TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			TValue tvalue;
			return this.TryAddInternal(key, this._comparer.GetHashCode(key), value, false, true, out tvalue);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06005EFE RID: 24318 RVA: 0x0013F364 File Offset: 0x0013D564
		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			TValue tvalue;
			return this.TryGetValue(key, out tvalue);
		}

		/// <summary>Attempts to remove and return the value that has the specified key from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <param name="key">The key of the element to remove and return.</param>
		/// <param name="value">When this method returns, contains the object removed from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />, or the default value of  the <see langword="TValue" /> type if <paramref name="key" /> does not exist.</param>
		/// <returns>
		///   <see langword="true" /> if the object was removed successfully; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is  <see langword="null" />.</exception>
		// Token: 0x06005EFF RID: 24319 RVA: 0x0013F388 File Offset: 0x0013D588
		public bool TryRemove(TKey key, out TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryRemoveInternal(key, out value, false, default(TValue));
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x0013F3B4 File Offset: 0x0013D5B4
		private bool TryRemoveInternal(TKey key, out TValue value, bool matchValue, TValue oldValue)
		{
			int hashCode = this._comparer.GetHashCode(key);
			for (;;)
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashCode, out num, out num2, tables._buckets.Length, tables._locks.Length);
				object obj = tables._locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentDictionary<TKey, TValue>.Node node = null;
					ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num];
					while (node2 != null)
					{
						if (hashCode == node2._hashcode && this._comparer.Equals(node2._key, key))
						{
							if (matchValue && !EqualityComparer<TValue>.Default.Equals(oldValue, node2._value))
							{
								value = default(TValue);
								return false;
							}
							if (node == null)
							{
								Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node2._next);
							}
							else
							{
								node._next = node2._next;
							}
							value = node2._value;
							tables._countPerLock[num2]--;
							return true;
						}
						else
						{
							node = node2;
							node2 = node2._next;
						}
					}
				}
				break;
			}
			value = default(TValue);
			return false;
		}

		/// <summary>Attempts to get the value associated with the specified key from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the object from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> that has the specified key, or the default value of the type if the operation failed.</param>
		/// <returns>
		///   <see langword="true" /> if the key was found in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is  <see langword="null" />.</exception>
		// Token: 0x06005F01 RID: 24321 RVA: 0x0013F508 File Offset: 0x0013D708
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryGetValueInternal(key, this._comparer.GetHashCode(key), out value);
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x0013F52C File Offset: 0x0013D72C
		private bool TryGetValueInternal(TKey key, int hashcode, out TValue value)
		{
			ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
			int bucket = ConcurrentDictionary<TKey, TValue>.GetBucket(hashcode, tables._buckets.Length);
			for (ConcurrentDictionary<TKey, TValue>.Node node = Volatile.Read<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[bucket]); node != null; node = node._next)
			{
				if (hashcode == node._hashcode && this._comparer.Equals(node._key, key))
				{
					value = node._value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		/// <summary>Updates the value associated with <paramref name="key" /> to <paramref name="newValue" /> if the existing value with <paramref name="key" /> is equal to <paramref name="comparisonValue" />.</summary>
		/// <param name="key">The key of the value that is compared with <paramref name="comparisonValue" /> and possibly replaced.</param>
		/// <param name="newValue">The value that replaces the value of the element that has the specified <paramref name="key" /> if the comparison results in equality.</param>
		/// <param name="comparisonValue">The value that is compared with the value of the element that has the specified <paramref name="key" />.</param>
		/// <returns>
		///   <see langword="true" /> if the value with <paramref name="key" /> was equal to <paramref name="comparisonValue" /> and was replaced with <paramref name="newValue" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06005F03 RID: 24323 RVA: 0x0013F5A4 File Offset: 0x0013D7A4
		public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryUpdateInternal(key, this._comparer.GetHashCode(key), newValue, comparisonValue);
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
		private bool TryUpdateInternal(TKey key, int hashcode, TValue newValue, TValue comparisonValue)
		{
			IEqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			bool flag2;
			for (;;)
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashcode, out num, out num2, tables._buckets.Length, tables._locks.Length);
				object obj = tables._locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentDictionary<TKey, TValue>.Node node = null;
					ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num];
					while (node2 != null)
					{
						if (hashcode == node2._hashcode && this._comparer.Equals(node2._key, key))
						{
							if (@default.Equals(node2._value, comparisonValue))
							{
								if (ConcurrentDictionary<TKey, TValue>.s_isValueWriteAtomic)
								{
									node2._value = newValue;
								}
								else
								{
									ConcurrentDictionary<TKey, TValue>.Node node3 = new ConcurrentDictionary<TKey, TValue>.Node(node2._key, newValue, hashcode, node2._next);
									if (node == null)
									{
										Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node3);
									}
									else
									{
										node._next = node3;
									}
								}
								return true;
							}
							return false;
						}
						else
						{
							node = node2;
							node2 = node2._next;
						}
					}
					flag2 = false;
				}
				break;
			}
			return flag2;
		}

		/// <summary>Removes all keys and values from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		// Token: 0x06005F05 RID: 24325 RVA: 0x0013F6F4 File Offset: 0x0013D8F4
		public void Clear()
		{
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				ConcurrentDictionary<TKey, TValue>.Tables tables = new ConcurrentDictionary<TKey, TValue>.Tables(new ConcurrentDictionary<TKey, TValue>.Node[31], this._tables._locks, new int[this._tables._countPerLock.Length]);
				this._tables = tables;
				this._budget = Math.Max(1, tables._buckets.Length / tables._locks.Length);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x0013F77C File Offset: 0x0013D97C
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index argument is less than zero.");
			}
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				int num2 = 0;
				int num3 = 0;
				while (num3 < this._tables._locks.Length && num2 >= 0)
				{
					num2 += this._tables._countPerLock[num3];
					num3++;
				}
				if (array.Length - num2 < index || num2 < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the dictionary is greater than the available space from index to the end of the destination array.");
				}
				this.CopyToPairs(array, index);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		/// <summary>Copies the key and value pairs stored in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> to a new array.</summary>
		/// <returns>A new array containing a snapshot of key and value pairs copied from the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</returns>
		// Token: 0x06005F07 RID: 24327 RVA: 0x0013F824 File Offset: 0x0013DA24
		public KeyValuePair<TKey, TValue>[] ToArray()
		{
			int num = 0;
			checked
			{
				KeyValuePair<TKey, TValue>[] array;
				try
				{
					this.AcquireAllLocks(ref num);
					int num2 = 0;
					for (int i = 0; i < this._tables._locks.Length; i++)
					{
						num2 += this._tables._countPerLock[i];
					}
					if (num2 == 0)
					{
						array = Array.Empty<KeyValuePair<TKey, TValue>>();
					}
					else
					{
						KeyValuePair<TKey, TValue>[] array2 = new KeyValuePair<TKey, TValue>[num2];
						this.CopyToPairs(array2, 0);
						array = array2;
					}
				}
				finally
				{
					this.ReleaseLocks(0, num);
				}
				return array;
			}
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x0013F8A8 File Offset: 0x0013DAA8
		private void CopyToPairs(KeyValuePair<TKey, TValue>[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new KeyValuePair<TKey, TValue>(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x0013F900 File Offset: 0x0013DB00
		private void CopyToEntries(DictionaryEntry[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new DictionaryEntry(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0013F964 File Offset: 0x0013DB64
		private void CopyToObjects(object[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new KeyValuePair<TKey, TValue>(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</returns>
		// Token: 0x06005F0B RID: 24331 RVA: 0x0013F9BD File Offset: 0x0013DBBD
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			ConcurrentDictionary<TKey, TValue>.Node[] buckets = this._tables._buckets;
			int num;
			for (int i = 0; i < buckets.Length; i = num + 1)
			{
				ConcurrentDictionary<TKey, TValue>.Node current;
				for (current = Volatile.Read<ConcurrentDictionary<TKey, TValue>.Node>(ref buckets[i]); current != null; current = current._next)
				{
					yield return new KeyValuePair<TKey, TValue>(current._key, current._value);
				}
				current = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x06005F0C RID: 24332 RVA: 0x0013F9CC File Offset: 0x0013DBCC
		private bool TryAddInternal(TKey key, int hashcode, TValue value, bool updateIfExists, bool acquireLock, out TValue resultingValue)
		{
			checked
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables;
				bool flag;
				for (;;)
				{
					tables = this._tables;
					int num;
					int num2;
					ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashcode, out num, out num2, tables._buckets.Length, tables._locks.Length);
					flag = false;
					bool flag2 = false;
					try
					{
						if (acquireLock)
						{
							Monitor.Enter(tables._locks[num2], ref flag2);
						}
						if (tables != this._tables)
						{
							continue;
						}
						ConcurrentDictionary<TKey, TValue>.Node node = null;
						for (ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num]; node2 != null; node2 = node2._next)
						{
							if (hashcode == node2._hashcode && this._comparer.Equals(node2._key, key))
							{
								if (updateIfExists)
								{
									if (ConcurrentDictionary<TKey, TValue>.s_isValueWriteAtomic)
									{
										node2._value = value;
									}
									else
									{
										ConcurrentDictionary<TKey, TValue>.Node node3 = new ConcurrentDictionary<TKey, TValue>.Node(node2._key, value, hashcode, node2._next);
										if (node == null)
										{
											Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node3);
										}
										else
										{
											node._next = node3;
										}
									}
									resultingValue = value;
								}
								else
								{
									resultingValue = node2._value;
								}
								return false;
							}
							node = node2;
						}
						Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], new ConcurrentDictionary<TKey, TValue>.Node(key, value, hashcode, tables._buckets[num]));
						tables._countPerLock[num2]++;
						if (tables._countPerLock[num2] > this._budget)
						{
							flag = true;
						}
					}
					finally
					{
						if (flag2)
						{
							Monitor.Exit(tables._locks[num2]);
						}
					}
					break;
				}
				if (flag)
				{
					this.GrowTable(tables);
				}
				resultingValue = value;
				return true;
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value of the key/value pair at the specified index.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is  <see langword="null" />.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
		// Token: 0x170010A8 RID: 4264
		public TValue this[TKey key]
		{
			get
			{
				TValue tvalue;
				if (!this.TryGetValue(key, out tvalue))
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNotFoundException(key);
				}
				return tvalue;
			}
			set
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				this.TryAddInternal(key, this._comparer.GetHashCode(key), value, true, true, out tvalue);
			}
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x0004DFAA File Offset: 0x0004C1AA
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowKeyNotFoundException(object key)
		{
			throw new KeyNotFoundException(SR.Format("The given key '{0}' was not present in the dictionary.", key.ToString()));
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0013FBC3 File Offset: 0x0013DDC3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowKeyNullException()
		{
			throw new ArgumentNullException("key");
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</returns>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06005F11 RID: 24337 RVA: 0x0013FBD0 File Offset: 0x0013DDD0
		public int Count
		{
			get
			{
				int num = 0;
				int countInternal;
				try
				{
					this.AcquireAllLocks(ref num);
					countInternal = this.GetCountInternal();
				}
				finally
				{
					this.ReleaseLocks(0, num);
				}
				return countInternal;
			}
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x0013FC0C File Offset: 0x0013DE0C
		private int GetCountInternal()
		{
			int num = 0;
			for (int i = 0; i < this._tables._countPerLock.Length; i++)
			{
				num += this._tables._countPerLock[i];
			}
			return num;
		}

		/// <summary>Adds a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> by using the specified function if the key does not already exist. Returns the new value, or the existing value if the key exists.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="valueFactory">The function used to generate a value for the key.</param>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> or <paramref name="valueFactory" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005F13 RID: 24339 RVA: 0x0013FC4C File Offset: 0x0013DE4C
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue;
			if (!this.TryGetValueInternal(key, hashCode, out tvalue))
			{
				this.TryAddInternal(key, hashCode, valueFactory(key), false, true, out tvalue);
			}
			return tvalue;
		}

		/// <summary>Adds a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> by using the specified function and an argument if the key does not already exist, or returns the existing value if the key exists.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="valueFactory">The function used to generate a value for the key.</param>
		/// <param name="factoryArgument">An argument value to pass into <paramref name="valueFactory" />.</param>
		/// <typeparam name="TArg">The type of an argument to pass into <paramref name="valueFactory" />.</typeparam>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		// Token: 0x06005F14 RID: 24340 RVA: 0x0013FCA4 File Offset: 0x0013DEA4
		public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue;
			if (!this.TryGetValueInternal(key, hashCode, out tvalue))
			{
				this.TryAddInternal(key, hashCode, valueFactory(key, factoryArgument), false, true, out tvalue);
			}
			return tvalue;
		}

		/// <summary>Adds a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key does not already exist. Returns the new value, or the existing value if the key exists.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value to be added, if the key does not already exist.</param>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005F15 RID: 24341 RVA: 0x0013FCFC File Offset: 0x0013DEFC
		public TValue GetOrAdd(TKey key, TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue;
			if (!this.TryGetValueInternal(key, hashCode, out tvalue))
			{
				this.TryAddInternal(key, hashCode, value, false, true, out tvalue);
			}
			return tvalue;
		}

		/// <summary>Uses the specified functions and argument to add a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key does not already exist, or to update a key/value pair in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key already exists.</summary>
		/// <param name="key">The key to be added or whose value should be updated.</param>
		/// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
		/// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
		/// <param name="factoryArgument">An argument to pass into <paramref name="addValueFactory" /> and <paramref name="updateValueFactory" />.</param>
		/// <typeparam name="TArg">The type of an argument to pass into <paramref name="addValueFactory" /> and <paramref name="updateValueFactory" />.</typeparam>
		/// <returns>The new value for the key. This will be either be the result of <paramref name="addValueFactory" /> (if the key was absent) or the result of <paramref name="updateValueFactory" /> (if the key was present).</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" />, <paramref name="addValueFactory" />, or <paramref name="updateValueFactory" /> is a null reference (Nothing in Visual Basic).</exception>
		// Token: 0x06005F16 RID: 24342 RVA: 0x0013FD40 File Offset: 0x0013DF40
		public TValue AddOrUpdate<TArg>(TKey key, Func<TKey, TArg, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory, TArg factoryArgument)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (addValueFactory == null)
			{
				throw new ArgumentNullException("addValueFactory");
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue tvalue3;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue, factoryArgument);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValueFactory(key, factoryArgument), false, true, out tvalue3))
				{
					return tvalue3;
				}
			}
			return tvalue2;
		}

		/// <summary>Uses the specified functions to add a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key does not already exist, or to update a key/value pair in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key already exists.</summary>
		/// <param name="key">The key to be added or whose value should be updated</param>
		/// <param name="addValueFactory">The function used to generate a value for an absent key</param>
		/// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
		/// <returns>The new value for the key. This will be either be the result of <paramref name="addValueFactory" /> (if the key was absent) or the result of <paramref name="updateValueFactory" /> (if the key was present).</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" />, <paramref name="addValueFactory" />, or <paramref name="updateValueFactory" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005F17 RID: 24343 RVA: 0x0013FDC0 File Offset: 0x0013DFC0
		public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (addValueFactory == null)
			{
				throw new ArgumentNullException("addValueFactory");
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue tvalue3;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValueFactory(key), false, true, out tvalue3))
				{
					return tvalue3;
				}
			}
			return tvalue2;
		}

		/// <summary>Adds a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key does not already exist, or updates a key/value pair in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> by using the specified function if the key already exists.</summary>
		/// <param name="key">The key to be added or whose value should be updated</param>
		/// <param name="addValue">The value to be added for an absent key</param>
		/// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
		/// <returns>The new value for the key. This will be either be <paramref name="addValue" /> (if the key was absent) or the result of <paramref name="updateValueFactory" /> (if the key was present).</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> or <paramref name="updateValueFactory" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005F18 RID: 24344 RVA: 0x0013FE3C File Offset: 0x0013E03C
		public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue tvalue3;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValue, false, true, out tvalue3))
				{
					return tvalue3;
				}
			}
			return tvalue2;
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> is empty.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06005F19 RID: 24345 RVA: 0x0013FEA4 File Offset: 0x0013E0A4
		public bool IsEmpty
		{
			get
			{
				int num = 0;
				try
				{
					this.AcquireAllLocks(ref num);
					for (int i = 0; i < this._tables._countPerLock.Length; i++)
					{
						if (this._tables._countPerLock[i] != 0)
						{
							return false;
						}
					}
				}
				finally
				{
					this.ReleaseLocks(0, num);
				}
				return true;
			}
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0013FF0C File Offset: 0x0013E10C
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			if (!this.TryAdd(key, value))
			{
				throw new ArgumentException("The key already existed in the dictionary.");
			}
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0013FF24 File Offset: 0x0013E124
		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			TValue tvalue;
			return this.TryRemove(key, out tvalue);
		}

		/// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A collection of keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06005F1C RID: 24348 RVA: 0x0013FF3A File Offset: 0x0013E13A
		public ICollection<TKey> Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06005F1D RID: 24349 RVA: 0x0013FF3A File Offset: 0x0013E13A
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		/// <summary>Gets a collection that contains the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A collection that contains the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06005F1E RID: 24350 RVA: 0x0013FF42 File Offset: 0x0013E142
		public ICollection<TValue> Values
		{
			get
			{
				return this.GetValues();
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06005F1F RID: 24351 RVA: 0x0013FF42 File Offset: 0x0013E142
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.GetValues();
			}
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x0013FF4A File Offset: 0x0013E14A
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			((IDictionary<TKey, TValue>)this).Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x0013FF60 File Offset: 0x0013E160
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			TValue tvalue;
			return this.TryGetValue(keyValuePair.Key, out tvalue) && EqualityComparer<TValue>.Default.Equals(tvalue, keyValuePair.Value);
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06005F22 RID: 24354 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005F23 RID: 24355 RVA: 0x0013FF94 File Offset: 0x0013E194
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			if (keyValuePair.Key == null)
			{
				throw new ArgumentNullException("keyValuePair", "TKey is a reference type and item.Key is null.");
			}
			TValue tvalue;
			return this.TryRemoveInternal(keyValuePair.Key, out tvalue, true, keyValuePair.Value);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</returns>
		// Token: 0x06005F24 RID: 24356 RVA: 0x0013FFD6 File Offset: 0x0013E1D6
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">The object to use as the key.</param>
		/// <param name="value">The object to use as the value.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="key" /> is of a type that is not assignable to the key type  of the <see cref="T:System.Collections.Generic.Dictionary`2" />.  
		/// -or-  
		/// <paramref name="value" /> is of a type that is not assignable to the type of values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.  
		/// -or-  
		/// A value with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		/// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
		// Token: 0x06005F25 RID: 24357 RVA: 0x0013FFE0 File Offset: 0x0013E1E0
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (!(key is TKey))
			{
				throw new ArgumentException("The key was of an incorrect type for this dictionary.");
			}
			TValue tvalue;
			try
			{
				tvalue = (TValue)((object)value);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException("The value was of an incorrect type for this dictionary.");
			}
			((IDictionary<TKey, TValue>)this).Add((TKey)((object)key), tvalue);
		}

		/// <summary>Gets a value that indicates the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06005F26 RID: 24358 RVA: 0x0014003C File Offset: 0x0013E23C
		bool IDictionary.Contains(object key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return key is TKey && this.ContainsKey((TKey)((object)key));
		}

		/// <summary>Provides a <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
		// Token: 0x06005F27 RID: 24359 RVA: 0x0014005C File Offset: 0x0013E25C
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new ConcurrentDictionary<TKey, TValue>.DictionaryEnumerator(this);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> has a fixed size.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> has a fixed size; otherwise, <see langword="false" />. For <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06005F28 RID: 24360 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only; otherwise, <see langword="false" />. For <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06005F29 RID: 24361 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> that contains the keys of the  <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
		/// <returns>An interface that contains the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06005F2A RID: 24362 RVA: 0x0013FF3A File Offset: 0x0013E13A
		ICollection IDictionary.Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06005F2B RID: 24363 RVA: 0x00140064 File Offset: 0x0013E264
		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (key is TKey)
			{
				TValue tvalue;
				this.TryRemove((TKey)((object)key), out tvalue);
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> that contains the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An interface that contains the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06005F2C RID: 24364 RVA: 0x0013FF42 File Offset: 0x0013E142
		ICollection IDictionary.Values
		{
			get
			{
				return this.GetValues();
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value associated with the specified key, or  <see langword="null" /> if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is  <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type or the value type of the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</exception>
		// Token: 0x170010B4 RID: 4276
		object IDictionary.this[object key]
		{
			get
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				if (key is TKey && this.TryGetValue((TKey)((object)key), out tvalue))
				{
					return tvalue;
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				if (!(key is TKey))
				{
					throw new ArgumentException("The key was of an incorrect type for this dictionary.");
				}
				if (!(value is TValue))
				{
					throw new ArgumentException("The value was of an incorrect type for this dictionary.");
				}
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.ICollection" />. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" />.  
		/// -or-  
		/// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06005F2F RID: 24367 RVA: 0x00140118 File Offset: 0x0013E318
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index argument is less than zero.");
			}
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num2 = 0;
				int num3 = 0;
				while (num3 < tables._locks.Length && num2 >= 0)
				{
					num2 += tables._countPerLock[num3];
					num3++;
				}
				if (array.Length - num2 < index || num2 < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the dictionary is greater than the available space from index to the end of the destination array.");
				}
				KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
				if (array2 != null)
				{
					this.CopyToPairs(array2, index);
				}
				else
				{
					DictionaryEntry[] array3 = array as DictionaryEntry[];
					if (array3 != null)
					{
						this.CopyToEntries(array3, index);
					}
					else
					{
						object[] array4 = array as object[];
						if (array4 == null)
						{
							throw new ArgumentException("The array is multidimensional, or the type parameter for the set cannot be cast automatically to the type of the destination array.", "array");
						}
						this.CopyToObjects(array4, index);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		/// <summary>Gets a value that indicates whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized with the SyncRoot.</summary>
		/// <returns>
		///   <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />. For <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> this property always returns <see langword="false" />.</returns>
		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06005F30 RID: 24368 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. This property is not supported.</summary>
		/// <returns>Always returns null.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported.</exception>
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005F31 RID: 24369 RVA: 0x0013E2AF File Offset: 0x0013C4AF
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException("The SyncRoot property may not be used for the synchronization of concurrent collections.");
			}
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x0014020C File Offset: 0x0013E40C
		private void GrowTable(ConcurrentDictionary<TKey, TValue>.Tables tables)
		{
			int num = 0;
			try
			{
				this.AcquireLocks(0, 1, ref num);
				if (tables == this._tables)
				{
					long num2 = 0L;
					for (int i = 0; i < tables._countPerLock.Length; i++)
					{
						num2 += (long)tables._countPerLock[i];
					}
					if (num2 < (long)(tables._buckets.Length / 4))
					{
						this._budget = 2 * this._budget;
						if (this._budget < 0)
						{
							this._budget = int.MaxValue;
						}
					}
					else
					{
						int num3 = 0;
						bool flag = false;
						object[] array;
						checked
						{
							try
							{
								num3 = tables._buckets.Length * 2 + 1;
								while (num3 % 3 == 0 || num3 % 5 == 0 || num3 % 7 == 0)
								{
									num3 += 2;
								}
								if (num3 > 2146435071)
								{
									flag = true;
								}
							}
							catch (OverflowException)
							{
								flag = true;
							}
							if (flag)
							{
								num3 = 2146435071;
								this._budget = int.MaxValue;
							}
							this.AcquireLocks(1, tables._locks.Length, ref num);
							array = tables._locks;
						}
						if (this._growLockArray && tables._locks.Length < 1024)
						{
							array = new object[tables._locks.Length * 2];
							Array.Copy(tables._locks, 0, array, 0, tables._locks.Length);
							for (int j = tables._locks.Length; j < array.Length; j++)
							{
								array[j] = new object();
							}
						}
						ConcurrentDictionary<TKey, TValue>.Node[] array2 = new ConcurrentDictionary<TKey, TValue>.Node[num3];
						int[] array3 = new int[array.Length];
						for (int k = 0; k < tables._buckets.Length; k++)
						{
							checked
							{
								ConcurrentDictionary<TKey, TValue>.Node next;
								for (ConcurrentDictionary<TKey, TValue>.Node node = tables._buckets[k]; node != null; node = next)
								{
									next = node._next;
									int num4;
									int num5;
									ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(node._hashcode, out num4, out num5, array2.Length, array.Length);
									array2[num4] = new ConcurrentDictionary<TKey, TValue>.Node(node._key, node._value, node._hashcode, array2[num4]);
									array3[num5]++;
								}
							}
						}
						this._budget = Math.Max(1, array2.Length / array.Length);
						this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(array2, array, array3);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x00140454 File Offset: 0x0013E654
		private static int GetBucket(int hashcode, int bucketCount)
		{
			return (hashcode & int.MaxValue) % bucketCount;
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x0014045F File Offset: 0x0013E65F
		private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
		{
			bucketNo = (hashcode & int.MaxValue) % bucketCount;
			lockNo = bucketNo % lockCount;
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06005F35 RID: 24373 RVA: 0x00140473 File Offset: 0x0013E673
		private static int DefaultConcurrencyLevel
		{
			get
			{
				return PlatformHelper.ProcessorCount;
			}
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x0014047C File Offset: 0x0013E67C
		private void AcquireAllLocks(ref int locksAcquired)
		{
			if (CDSCollectionETWBCLProvider.Log.IsEnabled())
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentDictionary_AcquiringAllLocks(this._tables._buckets.Length);
			}
			this.AcquireLocks(0, 1, ref locksAcquired);
			this.AcquireLocks(1, this._tables._locks.Length, ref locksAcquired);
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x001404D0 File Offset: 0x0013E6D0
		private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
		{
			object[] locks = this._tables._locks;
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				bool flag = false;
				try
				{
					Monitor.Enter(locks[i], ref flag);
				}
				finally
				{
					if (flag)
					{
						locksAcquired++;
					}
				}
			}
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x00140520 File Offset: 0x0013E720
		private void ReleaseLocks(int fromInclusive, int toExclusive)
		{
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				Monitor.Exit(this._tables._locks[i]);
			}
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x00140550 File Offset: 0x0013E750
		private ReadOnlyCollection<TKey> GetKeys()
		{
			int num = 0;
			ReadOnlyCollection<TKey> readOnlyCollection;
			try
			{
				this.AcquireAllLocks(ref num);
				int countInternal = this.GetCountInternal();
				if (countInternal < 0)
				{
					throw new OutOfMemoryException();
				}
				List<TKey> list = new List<TKey>(countInternal);
				for (int i = 0; i < this._tables._buckets.Length; i++)
				{
					for (ConcurrentDictionary<TKey, TValue>.Node node = this._tables._buckets[i]; node != null; node = node._next)
					{
						list.Add(node._key);
					}
				}
				readOnlyCollection = new ReadOnlyCollection<TKey>(list);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
			return readOnlyCollection;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x001405E8 File Offset: 0x0013E7E8
		private ReadOnlyCollection<TValue> GetValues()
		{
			int num = 0;
			ReadOnlyCollection<TValue> readOnlyCollection;
			try
			{
				this.AcquireAllLocks(ref num);
				int countInternal = this.GetCountInternal();
				if (countInternal < 0)
				{
					throw new OutOfMemoryException();
				}
				List<TValue> list = new List<TValue>(countInternal);
				for (int i = 0; i < this._tables._buckets.Length; i++)
				{
					for (ConcurrentDictionary<TKey, TValue>.Node node = this._tables._buckets[i]; node != null; node = node._next)
					{
						list.Add(node._value);
					}
				}
				readOnlyCollection = new ReadOnlyCollection<TValue>(list);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
			return readOnlyCollection;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00140680 File Offset: 0x0013E880
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
			this._serializationArray = this.ToArray();
			this._serializationConcurrencyLevel = tables._locks.Length;
			this._serializationCapacity = tables._buckets.Length;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x001406BE File Offset: 0x0013E8BE
		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			this._serializationArray = null;
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x001406C8 File Offset: 0x0013E8C8
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			KeyValuePair<TKey, TValue>[] serializationArray = this._serializationArray;
			ConcurrentDictionary<TKey, TValue>.Node[] array = new ConcurrentDictionary<TKey, TValue>.Node[this._serializationCapacity];
			int[] array2 = new int[this._serializationConcurrencyLevel];
			object[] array3 = new object[this._serializationConcurrencyLevel];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = new object();
			}
			this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(array, array3, array2);
			this.InitializeFromCollection(serializationArray);
			this._serializationArray = null;
		}

		// Token: 0x0400393D RID: 14653
		[NonSerialized]
		private volatile ConcurrentDictionary<TKey, TValue>.Tables _tables;

		// Token: 0x0400393E RID: 14654
		private IEqualityComparer<TKey> _comparer;

		// Token: 0x0400393F RID: 14655
		[NonSerialized]
		private readonly bool _growLockArray;

		// Token: 0x04003940 RID: 14656
		[NonSerialized]
		private int _budget;

		// Token: 0x04003941 RID: 14657
		private KeyValuePair<TKey, TValue>[] _serializationArray;

		// Token: 0x04003942 RID: 14658
		private int _serializationConcurrencyLevel;

		// Token: 0x04003943 RID: 14659
		private int _serializationCapacity;

		// Token: 0x04003944 RID: 14660
		private const int DefaultCapacity = 31;

		// Token: 0x04003945 RID: 14661
		private const int MaxLockNumber = 1024;

		// Token: 0x04003946 RID: 14662
		private static readonly bool s_isValueWriteAtomic = ConcurrentDictionary<TKey, TValue>.IsValueWriteAtomic();

		// Token: 0x02000A59 RID: 2649
		private sealed class Tables
		{
			// Token: 0x06005F3F RID: 24383 RVA: 0x00140745 File Offset: 0x0013E945
			internal Tables(ConcurrentDictionary<TKey, TValue>.Node[] buckets, object[] locks, int[] countPerLock)
			{
				this._buckets = buckets;
				this._locks = locks;
				this._countPerLock = countPerLock;
			}

			// Token: 0x04003947 RID: 14663
			internal readonly ConcurrentDictionary<TKey, TValue>.Node[] _buckets;

			// Token: 0x04003948 RID: 14664
			internal readonly object[] _locks;

			// Token: 0x04003949 RID: 14665
			internal volatile int[] _countPerLock;
		}

		// Token: 0x02000A5A RID: 2650
		[Serializable]
		private sealed class Node
		{
			// Token: 0x06005F40 RID: 24384 RVA: 0x00140764 File Offset: 0x0013E964
			internal Node(TKey key, TValue value, int hashcode, ConcurrentDictionary<TKey, TValue>.Node next)
			{
				this._key = key;
				this._value = value;
				this._next = next;
				this._hashcode = hashcode;
			}

			// Token: 0x0400394A RID: 14666
			internal readonly TKey _key;

			// Token: 0x0400394B RID: 14667
			internal TValue _value;

			// Token: 0x0400394C RID: 14668
			internal volatile ConcurrentDictionary<TKey, TValue>.Node _next;

			// Token: 0x0400394D RID: 14669
			internal readonly int _hashcode;
		}

		// Token: 0x02000A5B RID: 2651
		[Serializable]
		private sealed class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06005F41 RID: 24385 RVA: 0x0014078B File Offset: 0x0013E98B
			internal DictionaryEnumerator(ConcurrentDictionary<TKey, TValue> dictionary)
			{
				this._enumerator = dictionary.GetEnumerator();
			}

			// Token: 0x170010B8 RID: 4280
			// (get) Token: 0x06005F42 RID: 24386 RVA: 0x001407A0 File Offset: 0x0013E9A0
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

			// Token: 0x170010B9 RID: 4281
			// (get) Token: 0x06005F43 RID: 24387 RVA: 0x001407E4 File Offset: 0x0013E9E4
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x170010BA RID: 4282
			// (get) Token: 0x06005F44 RID: 24388 RVA: 0x0014080C File Offset: 0x0013EA0C
			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x170010BB RID: 4283
			// (get) Token: 0x06005F45 RID: 24389 RVA: 0x00140831 File Offset: 0x0013EA31
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06005F46 RID: 24390 RVA: 0x0014083E File Offset: 0x0013EA3E
			public bool MoveNext()
			{
				return this._enumerator.MoveNext();
			}

			// Token: 0x06005F47 RID: 24391 RVA: 0x0014084B File Offset: 0x0013EA4B
			public void Reset()
			{
				this._enumerator.Reset();
			}

			// Token: 0x0400394E RID: 14670
			private IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;
		}
	}
}
