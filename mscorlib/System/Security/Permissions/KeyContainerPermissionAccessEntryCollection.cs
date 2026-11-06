using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Represents a collection of <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> objects. This class cannot be inherited.</summary>
	// Token: 0x02000447 RID: 1095
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06002C6A RID: 11370 RVA: 0x0009FA9D File Offset: 0x0009DC9D
		internal KeyContainerPermissionAccessEntryCollection()
		{
			this._list = new ArrayList();
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x0009FAB0 File Offset: 0x0009DCB0
		internal KeyContainerPermissionAccessEntryCollection(KeyContainerPermissionAccessEntry[] entries)
		{
			if (entries != null)
			{
				foreach (KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry in entries)
				{
					this.Add(keyContainerPermissionAccessEntry);
				}
			}
		}

		/// <summary>Gets the number of items in the collection.</summary>
		/// <returns>The number of <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> objects in the collection.</returns>
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x0009FAE2 File Offset: 0x0009DCE2
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		/// <summary>Gets a value indicating whether the collection is synchronized (thread safe).</summary>
		/// <returns>
		///   <see langword="false" /> in all cases.</returns>
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the item at the specified index in the collection.</summary>
		/// <param name="index">The zero-based index of the element to access.</param>
		/// <returns>The <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object at the specified index in the collection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is greater than or equal to the collection count.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="index" /> is negative.</exception>
		// Token: 0x170005A2 RID: 1442
		public KeyContainerPermissionAccessEntry this[int index]
		{
			get
			{
				return (KeyContainerPermissionAccessEntry)this._list[index];
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Adds a <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object to the collection.</summary>
		/// <param name="accessEntry">The <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object to add.</param>
		/// <returns>The index at which the new element was inserted.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="accessEntry" /> is <see langword="null" />.</exception>
		// Token: 0x06002C70 RID: 11376 RVA: 0x0009FB02 File Offset: 0x0009DD02
		public int Add(KeyContainerPermissionAccessEntry accessEntry)
		{
			return this._list.Add(accessEntry);
		}

		/// <summary>Removes all the <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> objects from the collection.</summary>
		// Token: 0x06002C71 RID: 11377 RVA: 0x0009FB10 File Offset: 0x0009DD10
		public void Clear()
		{
			this._list.Clear();
		}

		/// <summary>Copies the elements of the collection to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> array that is the destination of the elements copied from the current collection.</param>
		/// <param name="index">The index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the collection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06002C72 RID: 11378 RVA: 0x0009FB1D File Offset: 0x0009DD1D
		public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Copies the elements of the collection to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the current collection.</param>
		/// <param name="index">The index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the collection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06002C73 RID: 11379 RVA: 0x0009FB1D File Offset: 0x0009DD1D
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Returns a <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntryEnumerator" /> object that can be used to iterate through the objects in the collection.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntryEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06002C74 RID: 11380 RVA: 0x0009FB2C File Offset: 0x0009DD2C
		public KeyContainerPermissionAccessEntryEnumerator GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this._list);
		}

		/// <summary>Returns a <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntryEnumerator" /> object that can be used to iterate through the objects in the collection.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntryEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06002C75 RID: 11381 RVA: 0x0009FB2C File Offset: 0x0009DD2C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this._list);
		}

		/// <summary>Gets the index in the collection of the specified <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object, if it exists in the collection.</summary>
		/// <param name="accessEntry">The <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object to locate.</param>
		/// <returns>The index of the specified <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object in the collection, or -1 if no match is found.</returns>
		// Token: 0x06002C76 RID: 11382 RVA: 0x0009FB3C File Offset: 0x0009DD3C
		public int IndexOf(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				if (accessEntry.Equals(this._list[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Removes the specified <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object from thecollection.</summary>
		/// <param name="accessEntry">The <see cref="T:System.Security.Permissions.KeyContainerPermissionAccessEntry" /> object to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="accessEntry" /> is <see langword="null" />.</exception>
		// Token: 0x06002C77 RID: 11383 RVA: 0x0009FB84 File Offset: 0x0009DD84
		public void Remove(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				if (accessEntry.Equals(this._list[i]))
				{
					this._list.RemoveAt(i);
				}
			}
		}

		// Token: 0x04002053 RID: 8275
		private ArrayList _list;
	}
}
