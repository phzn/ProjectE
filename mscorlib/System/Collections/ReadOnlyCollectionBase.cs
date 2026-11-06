using System;

namespace System.Collections
{
	/// <summary>Provides the <see langword="abstract" /> base class for a strongly typed non-generic read-only collection.</summary>
	// Token: 0x02000A2B RID: 2603
	[Serializable]
	public abstract class ReadOnlyCollectionBase : ICollection, IEnumerable
	{
		/// <summary>Gets the list of elements contained in the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> representing the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance itself.</returns>
		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x0013662B File Offset: 0x0013482B
		protected ArrayList InnerList
		{
			get
			{
				if (this._list == null)
				{
					this._list = new ArrayList();
				}
				return this._list;
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance.  
		///  Retrieving the value of this property is an O(1) operation.</returns>
		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x00136646 File Offset: 0x00134846
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to a <see cref="T:System.Collections.ReadOnlyCollectionBase" /> object is synchronized (thread safe).</summary>
		/// <returns>
		///   <see langword="true" /> if access to the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> object is synchronized (thread safe); otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x00136653 File Offset: 0x00134853
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to a <see cref="T:System.Collections.ReadOnlyCollectionBase" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> object.</returns>
		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x00136660 File Offset: 0x00134860
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.ReadOnlyCollectionBase" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ReadOnlyCollectionBase" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the source <see cref="T:System.Collections.ReadOnlyCollectionBase" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ReadOnlyCollectionBase" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x06005C42 RID: 23618 RVA: 0x000BCA7D File Offset: 0x000BAC7D
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.ReadOnlyCollectionBase" /> instance.</returns>
		// Token: 0x06005C43 RID: 23619 RVA: 0x0013666D File Offset: 0x0013486D
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x0400389A RID: 14490
		private ArrayList _list;
	}
}
