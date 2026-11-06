using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Provides methods for creating, manipulating, searching, and sorting arrays, thereby serving as the base class for all arrays in the common language runtime.</summary>
	// Token: 0x020001D1 RID: 465
	[Serializable]
	public abstract class Array : ICollection, IEnumerable, IList, IStructuralComparable, IStructuralEquatable, ICloneable
	{
		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 64-bit integers.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">An array of 64-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create. Each integer in the array must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="lengths" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.  
		/// -or-  
		/// The <paramref name="lengths" /> array contains less than one element.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x060013DB RID: 5083 RVA: 0x0004EC90 File Offset: 0x0004CE90
		public static Array CreateInstance(Type elementType, params long[] lengths)
		{
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lengths.Length == 0)
			{
				throw new ArgumentException("Must provide at least one rank.");
			}
			int[] array = new int[lengths.Length];
			for (int i = 0; i < lengths.Length; i++)
			{
				long num = lengths[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("len", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			return Array.CreateInstance(elementType, array);
		}

		/// <summary>Returns a read-only wrapper for the specified array.</summary>
		/// <param name="array">The one-dimensional, zero-based array to wrap in a read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>A read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper for the specified array.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		// Token: 0x060013DC RID: 5084 RVA: 0x0004ED03 File Offset: 0x0004CF03
		public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return new ReadOnlyCollection<T>(array);
		}

		/// <summary>Changes the number of elements of a one-dimensional array to the specified new size.</summary>
		/// <param name="array">The one-dimensional, zero-based array to resize, or <see langword="null" /> to create a new array with the specified size.</param>
		/// <param name="newSize">The size of the new array.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="newSize" /> is less than zero.</exception>
		// Token: 0x060013DD RID: 5085 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		public static void Resize<T>(ref T[] array, int newSize)
		{
			if (newSize < 0)
			{
				throw new ArgumentOutOfRangeException("newSize", "Non-negative number required.");
			}
			T[] array2 = array;
			if (array2 == null)
			{
				array = new T[newSize];
				return;
			}
			if (array2.Length != newSize)
			{
				T[] array3 = new T[newSize];
				Array.Copy(array2, 0, array3, 0, (array2.Length > newSize) ? newSize : array2.Length);
				array = array3;
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Array" />.</summary>
		/// <returns>The number of elements contained in the collection.</returns>
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004ED71 File Offset: 0x0004CF71
		int ICollection.Count
		{
			get
			{
				return this.Length;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ICollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		// Token: 0x170001E1 RID: 481
		object IList.this[int index]
		{
			get
			{
				return this.GetValue(index);
			}
			set
			{
				this.SetValue(value, index);
			}
		}

		/// <summary>Calling this method always throws a <see cref="T:System.NotSupportedException" /> exception.</summary>
		/// <param name="value">The object to be added to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>Adding a value to an array is not supported. No value is returned.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
		// Token: 0x060013E2 RID: 5090 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		int IList.Add(object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="value">The object to locate in the current list. The element to locate can be <see langword="null" /> for reference types.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060013E3 RID: 5091 RVA: 0x0004ED98 File Offset: 0x0004CF98
		bool IList.Contains(object value)
		{
			return Array.IndexOf(this, value) >= 0;
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.IList" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.</exception>
		// Token: 0x060013E4 RID: 5092 RVA: 0x0004EDA7 File Offset: 0x0004CFA7
		void IList.Clear()
		{
			Array.Clear(this, this.GetLowerBound(0), this.Length);
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="value">The object to locate in the current list.</param>
		/// <returns>The index of value if found in the list; otherwise, -1.</returns>
		// Token: 0x060013E5 RID: 5093 RVA: 0x0004EDBC File Offset: 0x0004CFBC
		int IList.IndexOf(object value)
		{
			return Array.IndexOf(this, value);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.</summary>
		/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.  
		///  -or-  
		///  The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="value" /> is null reference in the <see cref="T:System.Collections.IList" />.</exception>
		// Token: 0x060013E6 RID: 5094 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.  
		///  -or-  
		///  The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
		// Token: 0x060013E7 RID: 5095 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		/// <summary>Removes the <see cref="T:System.Collections.IList" /> item at the specified index.</summary>
		/// <param name="index">The index of the element to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.  
		///  -or-  
		///  The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
		// Token: 0x060013E8 RID: 5096 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		/// <summary>Copies all the elements of the current one-dimensional array to the specified one-dimensional array starting at the specified destination array index. The index is specified as a 32-bit integer.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
		/// <param name="index">A 32-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the source array is greater than the available number of elements from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">The source array is multidimensional.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
		// Token: 0x060013E9 RID: 5097 RVA: 0x0004EDC5 File Offset: 0x0004CFC5
		public void CopyTo(Array array, int index)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			Array.Copy(this, this.GetLowerBound(0), array, index, this.Length);
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Array" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Array" />.</returns>
		// Token: 0x060013EA RID: 5098 RVA: 0x000231D1 File Offset: 0x000213D1
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		/// <summary>Determines whether the current collection object precedes, occurs in the same position as, or follows another object in the sort order.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <param name="comparer">An object that compares the current object and <paramref name="other" />.</param>
		/// <returns>An integer that indicates the relationship of the current collection object to other, as shown in the following table.  
		///   Return value  
		///
		///   Description  
		///
		///   -1  
		///
		///   The current instance precedes <paramref name="other" />.  
		///
		///   0  
		///
		///   The current instance and <paramref name="other" /> are equal.  
		///
		///   1  
		///
		///   The current instance follows <paramref name="other" />.</returns>
		// Token: 0x060013EB RID: 5099 RVA: 0x0004EDF4 File Offset: 0x0004CFF4
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Array array = other as Array;
			if (array == null || this.Length != array.Length)
			{
				throw new ArgumentException("Object is not a array with the same number of elements as the array to compare it to.", "other");
			}
			int num = 0;
			int num2 = 0;
			while (num < array.Length && num2 == 0)
			{
				object value = this.GetValue(num);
				object value2 = array.GetValue(num);
				num2 = comparer.Compare(value, value2);
				num++;
			}
			return num2;
		}

		/// <summary>Determines whether an object is equal to the current instance.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <param name="comparer">An object that determines whether the current instance and <paramref name="other" /> are equal.</param>
		/// <returns>
		///   <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060013EC RID: 5100 RVA: 0x0004EE60 File Offset: 0x0004D060
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			Array array = other as Array;
			if (array == null || array.Length != this.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				object value = this.GetValue(i);
				object value2 = array.GetValue(i);
				if (!comparer.Equals(value, value2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00033E4F File Offset: 0x0003204F
		internal static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		/// <summary>Returns a hash code for the current instance.</summary>
		/// <param name="comparer">An object that computes the hash code of the current object.</param>
		/// <returns>The hash code for the current instance.</returns>
		// Token: 0x060013EE RID: 5102 RVA: 0x0004EEC0 File Offset: 0x0004D0C0
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			int num = 0;
			for (int i = ((this.Length >= 8) ? (this.Length - 8) : 0); i < this.Length; i++)
			{
				num = Array.CombineHashCodes(num, comparer.GetHashCode(this.GetValue(i)));
			}
			return num;
		}

		/// <summary>Searches an entire one-dimensional sorted array for a specific element, using the <see cref="T:System.IComparable" /> interface implemented by each element of the array and by the specified object.</summary>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x060013EF RID: 5103 RVA: 0x0004EF16 File Offset: 0x0004D116
		public static int BinarySearch(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, null);
		}

		/// <summary>Converts an array of one type to an array of another type.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to convert to a target type.</param>
		/// <param name="converter">A <see cref="T:System.Converter`2" /> that converts each element from one type to another type.</param>
		/// <typeparam name="TInput">The type of the elements of the source array.</typeparam>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <returns>An array of the target type containing the converted elements from the source array.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="converter" /> is <see langword="null" />.</exception>
		// Token: 0x060013F0 RID: 5104 RVA: 0x0004EF3C File Offset: 0x0004D13C
		public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			TOutput[] array2 = new TOutput[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = converter(array[i]);
			}
			return array2;
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 64-bit integer.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
		// Token: 0x060013F1 RID: 5105 RVA: 0x0004EF91 File Offset: 0x0004D191
		public static void Copy(Array sourceArray, Array destinationArray, long length)
		{
			if (length > 2147483647L || length < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("length", "Arrays larger than 2GB are not supported.");
			}
			Array.Copy(sourceArray, destinationArray, (int)length);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 64-bit integers.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 64-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 64-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is outside the range of valid indexes for the <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="destinationIndex" /> is outside the range of valid indexes for the <paramref name="destinationArray" />.  
		/// -or-  
		/// <paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		// Token: 0x060013F2 RID: 5106 RVA: 0x0004EFC0 File Offset: 0x0004D1C0
		public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
		{
			if (sourceIndex > 2147483647L || sourceIndex < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Arrays larger than 2GB are not supported.");
			}
			if (destinationIndex > 2147483647L || destinationIndex < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Arrays larger than 2GB are not supported.");
			}
			if (length > 2147483647L || length < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("length", "Arrays larger than 2GB are not supported.");
			}
			Array.Copy(sourceArray, (int)sourceIndex, destinationArray, (int)destinationIndex, (int)length);
		}

		/// <summary>Copies all the elements of the current one-dimensional array to the specified one-dimensional array starting at the specified destination array index. The index is specified as a 64-bit integer.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
		/// <param name="index">A 64-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// The number of elements in the source array is greater than the available number of elements from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">The source <see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
		// Token: 0x060013F3 RID: 5107 RVA: 0x0004F043 File Offset: 0x0004D243
		public void CopyTo(Array array, long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			this.CopyTo(array, (int)index);
		}

		/// <summary>Performs the specified action on each element of the specified array.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> on whose elements the action is to be performed.</param>
		/// <param name="action">The <see cref="T:System.Action`1" /> to perform on each element of <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="action" /> is <see langword="null" />.</exception>
		// Token: 0x060013F4 RID: 5108 RVA: 0x0004F070 File Offset: 0x0004D270
		public static void ForEach<T>(T[] array, Action<T> action)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		/// <summary>Gets a 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
		/// <returns>A 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</returns>
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0004F0B4 File Offset: 0x0004D2B4
		public long LongLength
		{
			get
			{
				long num = (long)this.GetLength(0);
				for (int i = 1; i < this.Rank; i++)
				{
					num *= (long)this.GetLength(i);
				}
				return num;
			}
		}

		/// <summary>Gets a 64-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
		/// <returns>A 64-bit integer that represents the number of elements in the specified dimension.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.  
		/// -or-  
		/// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		// Token: 0x060013F6 RID: 5110 RVA: 0x0004F0E7 File Offset: 0x0004D2E7
		public long GetLongLength(int dimension)
		{
			return (long)this.GetLength(dimension);
		}

		/// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
		/// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		// Token: 0x060013F7 RID: 5111 RVA: 0x0004F0F1 File Offset: 0x0004D2F1
		public object GetValue(long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index);
		}

		/// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x060013F8 RID: 5112 RVA: 0x0004F120 File Offset: 0x0004D320
		public object GetValue(long index1, long index2)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index1, (int)index2);
		}

		/// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x060013F9 RID: 5113 RVA: 0x0004F17C File Offset: 0x0004D37C
		public object GetValue(long index1, long index2, long index3)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			if (index3 > 2147483647L || index3 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index3", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index1, (int)index2, (int)index3);
		}

		/// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
		/// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x060013FA RID: 5114 RVA: 0x0004F1FC File Offset: 0x0004D3FC
		public object GetValue(params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			if (this.Rank != indices.Length)
			{
				throw new ArgumentException("Indices length does not match the array rank.");
			}
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				long num = indices[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			return this.GetValue(array);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> has a fixed size.</summary>
		/// <returns>This property is always <see langword="true" /> for all arrays.</returns>
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> is read-only.</summary>
		/// <returns>This property is always <see langword="false" /> for all arrays.</returns>
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Array" /> is synchronized (thread safe).</summary>
		/// <returns>This property is always <see langword="false" /> for all arrays.</returns>
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Array" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Array" />.</returns>
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the <see cref="T:System.IComparable" /> interface implemented by each element of the array and by the specified value.</summary>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x060013FF RID: 5119 RVA: 0x0004F276 File Offset: 0x0004D476
		public static int BinarySearch(Array array, int index, int length, object value)
		{
			return Array.BinarySearch(array, index, length, value, null);
		}

		/// <summary>Searches an entire one-dimensional sorted array for a value using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x06001400 RID: 5120 RVA: 0x0004F282 File Offset: 0x0004D482
		public static int BinarySearch(Array array, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, comparer);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x06001401 RID: 5121 RVA: 0x0004F2A8 File Offset: 0x0004D4A8
		public static int BinarySearch(Array array, int index, int length, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			if (comparer == null)
			{
				comparer = Comparer.Default;
			}
			int i = index;
			int num = index + length - 1;
			object[] array2 = array as object[];
			if (array2 != null)
			{
				while (i <= num)
				{
					int median = Array.GetMedian(i, num);
					int num2;
					try
					{
						num2 = comparer.Compare(array2[median], value);
					}
					catch (Exception ex)
					{
						throw new InvalidOperationException("Failed to compare two elements in the array.", ex);
					}
					if (num2 == 0)
					{
						return median;
					}
					if (num2 < 0)
					{
						i = median + 1;
					}
					else
					{
						num = median - 1;
					}
				}
			}
			else
			{
				while (i <= num)
				{
					int median2 = Array.GetMedian(i, num);
					int num3;
					try
					{
						num3 = comparer.Compare(array.GetValue(median2), value);
					}
					catch (Exception ex2)
					{
						throw new InvalidOperationException("Failed to compare two elements in the array.", ex2);
					}
					if (num3 == 0)
					{
						return median2;
					}
					if (num3 < 0)
					{
						i = median2 + 1;
					}
					else
					{
						num = median2 - 1;
					}
				}
			}
			return ~i;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0004F3DC File Offset: 0x0004D5DC
		private static int GetMedian(int low, int hi)
		{
			return low + (hi - low >> 1);
		}

		/// <summary>Searches an entire one-dimensional sorted array for a specific element, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified object.</summary>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001403 RID: 5123 RVA: 0x0004F3E5 File Offset: 0x0004D5E5
		public static int BinarySearch<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, null);
		}

		/// <summary>Searches an entire one-dimensional sorted array for a value using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface</exception>
		// Token: 0x06001404 RID: 5124 RVA: 0x0004F401 File Offset: 0x0004D601
		public static int BinarySearch<T>(T[] array, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, comparer);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified value.</summary>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001405 RID: 5125 RVA: 0x0004F41D File Offset: 0x0004D61D
		public static int BinarySearch<T>(T[] array, int index, int length, T value)
		{
			return Array.BinarySearch<T>(array, index, length, value, null);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001406 RID: 5126 RVA: 0x0004F42C File Offset: 0x0004D62C
		public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return ArraySortHelper<T>.Default.BinarySearch(array, index, length, value, comparer);
		}

		/// <summary>Searches for the specified object and returns the index of its first occurrence in a one-dimensional array.</summary>
		/// <param name="array">The one-dimensional array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <returns>The index of the first occurrence of <paramref name="value" /> in <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x06001407 RID: 5127 RVA: 0x0004F48D File Offset: 0x0004D68D
		public static int IndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf(array, value, array.GetLowerBound(0), array.Length);
		}

		/// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of its first occurrence. The range extends from a specified index to the end of the array.</summary>
		/// <param name="array">The one-dimensional array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <returns>The index of the first occurrence of <paramref name="value" />, if it's found, within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x06001408 RID: 5128 RVA: 0x0004F4B4 File Offset: 0x0004D6B4
		public static int IndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int lowerBound = array.GetLowerBound(0);
			return Array.IndexOf(array, value, startIndex, array.Length - startIndex + lowerBound);
		}

		/// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of ifs first occurrence. The range extends from a specified index for a specified number of elements.</summary>
		/// <param name="array">The one-dimensional array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <param name="count">The number of elements to search.</param>
		/// <returns>The index of the first occurrence of <paramref name="value" />, if it's found in the <paramref name="array" /> from index <paramref name="startIndex" /> to <paramref name="startIndex" /> + <paramref name="count" /> - 1; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x06001409 RID: 5129 RVA: 0x0004F4EC File Offset: 0x0004D6EC
		public static int IndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			int lowerBound = array.GetLowerBound(0);
			if (startIndex < lowerBound || startIndex > array.Length + lowerBound)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > array.Length - startIndex + lowerBound)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			object[] array2 = array as object[];
			int num = startIndex + count;
			if (array2 != null)
			{
				if (value == null)
				{
					for (int i = startIndex; i < num; i++)
					{
						if (array2[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j < num; j++)
					{
						object obj = array2[j];
						if (obj != null && obj.Equals(value))
						{
							return j;
						}
					}
				}
			}
			else
			{
				for (int k = startIndex; k < num; k++)
				{
					object value2 = array.GetValue(k);
					if (value2 == null)
					{
						if (value == null)
						{
							return k;
						}
					}
					else if (value2.Equals(value))
					{
						return k;
					}
				}
			}
			return lowerBound - 1;
		}

		/// <summary>Searches for the specified object and returns the index of its first occurrence in a one-dimensional array.</summary>
		/// <param name="array">The one-dimensional, zero-based array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> in the entire <paramref name="array" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		// Token: 0x0600140A RID: 5130 RVA: 0x0004F5E6 File Offset: 0x0004D7E6
		public static int IndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOfImpl<T>(array, value, 0, array.Length);
		}

		/// <summary>Searches for the specified object in a range of elements of a one dimensional array, and returns the index of its first occurrence. The range extends from a specified index to the end of the array.</summary>
		/// <param name="array">The one-dimensional, zero-based array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		// Token: 0x0600140B RID: 5131 RVA: 0x0004F601 File Offset: 0x0004D801
		public static int IndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf<T>(array, value, startIndex, array.Length - startIndex);
		}

		/// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of its first occurrence. The range extends from a specified index for a specified number of elements.</summary>
		/// <param name="array">The one-dimensional, zero-based array to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that starts at <paramref name="startIndex" /> and contains the number of elements specified in <paramref name="count" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		// Token: 0x0600140C RID: 5132 RVA: 0x0004F620 File Offset: 0x0004D820
		public static int IndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > array.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return Array.IndexOfImpl<T>(array, value, startIndex, count);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the entire one-dimensional <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x0600140D RID: 5133 RVA: 0x0004F67A File Offset: 0x0004D87A
		public static int LastIndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf(array, value, array.Length - 1, array.Length);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the backward search.</param>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x0600140E RID: 5134 RVA: 0x0004F69F File Offset: 0x0004D89F
		public static int LastIndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf(array, value, startIndex, startIndex + 1);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x0600140F RID: 5135 RVA: 0x0004F6BC File Offset: 0x0004D8BC
		public static int LastIndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (startIndex < 0 || startIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException("endIndex", "endIndex cannot be greater than startIndex.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			object[] array2 = array as object[];
			int num = startIndex - count + 1;
			if (array2 != null)
			{
				if (value == null)
				{
					for (int i = startIndex; i >= num; i--)
					{
						if (array2[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j >= num; j--)
					{
						object obj = array2[j];
						if (obj != null && obj.Equals(value))
						{
							return j;
						}
					}
				}
			}
			else
			{
				for (int k = startIndex; k >= num; k--)
				{
					object value2 = array.GetValue(k);
					if (value2 == null)
					{
						if (value == null)
						{
							return k;
						}
					}
					else if (value2.Equals(value))
					{
						return k;
					}
				}
			}
			return -1;
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		// Token: 0x06001410 RID: 5136 RVA: 0x0004F7B9 File Offset: 0x0004D9B9
		public static int LastIndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf<T>(array, value, array.Length - 1, array.Length);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		// Token: 0x06001411 RID: 5137 RVA: 0x0004F7D8 File Offset: 0x0004D9D8
		public static int LastIndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf<T>(array, value, startIndex, (array.Length == 0) ? 0 : (startIndex + 1));
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		// Token: 0x06001412 RID: 5138 RVA: 0x0004F7FC File Offset: 0x0004D9FC
		public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				if (startIndex != -1 && startIndex != 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (count != 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				return -1;
			}
			else
			{
				if (startIndex < 0 || startIndex >= array.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				return Array.LastIndexOfImpl<T>(array, value, startIndex, count);
			}
		}

		/// <summary>Reverses the sequence of the elements in the entire one-dimensional <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		// Token: 0x06001413 RID: 5139 RVA: 0x0004F886 File Offset: 0x0004DA86
		public static void Reverse(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Reverse(array, array.GetLowerBound(0), array.Length);
		}

		/// <summary>Reverses the sequence of the elements in a range of elements in the one-dimensional <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
		/// <param name="index">The starting index of the section to reverse.</param>
		/// <param name="length">The number of elements in the section to reverse.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		// Token: 0x06001414 RID: 5140 RVA: 0x0004F8AC File Offset: 0x0004DAAC
		public static void Reverse(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int lowerBound = array.GetLowerBound(0);
			if (index < lowerBound || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < lowerBound) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - (index - lowerBound) < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			object[] array2 = array as object[];
			if (array2 != null)
			{
				Array.Reverse<object>(array2, index, length);
				return;
			}
			int i = index;
			int num = index + length - 1;
			while (i < num)
			{
				object value = array.GetValue(i);
				array.SetValue(array.GetValue(num), i);
				array.SetValue(value, num);
				i++;
				num--;
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004F967 File Offset: 0x0004DB67
		public static void Reverse<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Reverse<T>(array, 0, array.Length);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004F984 File Offset: 0x0004DB84
		public static void Reverse<T>(T[] array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length <= 1)
			{
				return;
			}
			ref T ptr = ref Unsafe.Add<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), index);
			ref T ptr2 = ref Unsafe.Add<T>(Unsafe.Add<T>(ref ptr, length), -1);
			do
			{
				T t = ptr;
				ptr = ptr2;
				ptr2 = t;
				ptr = Unsafe.Add<T>(ref ptr, 1);
				ptr2 = Unsafe.Add<T>(ref ptr2, -1);
			}
			while (Unsafe.IsAddressLessThan<T>(ref ptr, ref ptr2));
		}

		/// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001417 RID: 5143 RVA: 0x0004FA2D File Offset: 0x0004DC2D
		public void SetValue(object value, long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index);
		}

		/// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001418 RID: 5144 RVA: 0x0004FA5C File Offset: 0x0004DC5C
		public void SetValue(object value, long index1, long index2)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index1, (int)index2);
		}

		/// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001419 RID: 5145 RVA: 0x0004FAB8 File Offset: 0x0004DCB8
		public void SetValue(object value, long index1, long index2, long index3)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			if (index3 > 2147483647L || index3 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index3", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index1, (int)index2, (int)index3);
		}

		/// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the element to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600141A RID: 5146 RVA: 0x0004FB3C File Offset: 0x0004DD3C
		public void SetValue(object value, params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			if (this.Rank != indices.Length)
			{
				throw new ArgumentException("Indices length does not match the array rank.");
			}
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				long num = indices[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			this.SetValue(value, array);
		}

		/// <summary>Sorts the elements in an entire one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x0600141B RID: 5147 RVA: 0x0004FBB7 File Offset: 0x0004DDB7
		public static void Sort(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.Length, null);
		}

		/// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x0600141C RID: 5148 RVA: 0x0004FBDC File Offset: 0x0004DDDC
		public static void Sort(Array array, int index, int length)
		{
			Array.Sort(array, null, index, length, null);
		}

		/// <summary>Sorts the elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="array">The one-dimensional array to sort.</param>
		/// <param name="comparer">The implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		// Token: 0x0600141D RID: 5149 RVA: 0x0004FBE8 File Offset: 0x0004DDE8
		public static void Sort(Array array, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.Length, comparer);
		}

		/// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x0600141E RID: 5150 RVA: 0x0004FC0D File Offset: 0x0004DE0D
		public static void Sort(Array array, int index, int length, IComparer comparer)
		{
			Array.Sort(array, null, index, length, comparer);
		}

		/// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.  
		///  -or-  
		///  <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.  
		///  -or-  
		///  The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x0600141F RID: 5151 RVA: 0x0004FC19 File Offset: 0x0004DE19
		public static void Sort(Array keys, Array items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, null);
		}

		/// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.  
		///  -or-  
		///  <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.  
		///  -or-  
		///  The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x06001420 RID: 5152 RVA: 0x0004FC3E File Offset: 0x0004DE3E
		public static void Sort(Array keys, Array items, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, comparer);
		}

		/// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.  
		///  -or-  
		///  <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.  
		///  -or-  
		///  The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x06001421 RID: 5153 RVA: 0x0004FC63 File Offset: 0x0004DE63
		public static void Sort(Array keys, Array items, int index, int length)
		{
			Array.Sort(keys, items, index, length, null);
		}

		/// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.  
		///  -or-  
		///  <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.  
		///  -or-  
		///  <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.  
		///  -or-  
		///  The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		// Token: 0x06001422 RID: 5154 RVA: 0x0004FC70 File Offset: 0x0004DE70
		public static void Sort(Array keys, Array items, int index, int length, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (keys.Rank != 1 || (items != null && items.Rank != 1))
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			int lowerBound = keys.GetLowerBound(0);
			if (items != null && lowerBound != items.GetLowerBound(0))
			{
				throw new ArgumentException("The arrays' lower bounds must be identical.");
			}
			if (index < lowerBound || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (keys.Length - (index - lowerBound) < length || (items != null && index - lowerBound > items.Length - length))
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				Array.SortImpl(keys, items, index, length, comparer);
			}
		}

		/// <summary>Sorts the elements in an entire <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001423 RID: 5155 RVA: 0x0004FD25 File Offset: 0x0004DF25
		public static void Sort<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T>(array, 0, array.Length, null);
		}

		/// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001424 RID: 5156 RVA: 0x0004FD40 File Offset: 0x0004DF40
		public static void Sort<T>(T[] array, int index, int length)
		{
			Array.Sort<T>(array, index, length, null);
		}

		/// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The one-dimensional, zero-base <see cref="T:System.Array" /> to sort</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		// Token: 0x06001425 RID: 5157 RVA: 0x0004FD4B File Offset: 0x0004DF4B
		public static void Sort<T>(T[] array, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T>(array, 0, array.Length, comparer);
		}

		/// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001426 RID: 5158 RVA: 0x0004FD68 File Offset: 0x0004DF68
		public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				ArraySortHelper<T>.Default.Sort(array, index, length, comparer);
			}
		}

		/// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Comparison`1" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort</param>
		/// <param name="comparison">The <see cref="T:System.Comparison`1" /> to use when comparing elements.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="comparison" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparison" /> caused an error during the sort. For example, <paramref name="comparison" /> might not return 0 when comparing an item with itself.</exception>
		// Token: 0x06001427 RID: 5159 RVA: 0x0004FDCB File Offset: 0x0004DFCB
		public static void Sort<T>(T[] array, Comparison<T> comparison)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			ArraySortHelper<T>.Sort(array, 0, array.Length, comparison);
		}

		/// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001428 RID: 5160 RVA: 0x0004FDF4 File Offset: 0x0004DFF4
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, null);
		}

		/// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x06001429 RID: 5161 RVA: 0x0004FE10 File Offset: 0x0004E010
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length)
		{
			Array.Sort<TKey, TValue>(keys, items, index, length, null);
		}

		/// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x0600142A RID: 5162 RVA: 0x0004FE1C File Offset: 0x0004E01C
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, comparer);
		}

		/// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.  
		/// -or-  
		/// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.  
		/// -or-  
		/// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		// Token: 0x0600142B RID: 5163 RVA: 0x0004FE38 File Offset: 0x0004E038
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (keys.Length - index < length || (items != null && index > items.Length - length))
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				if (items == null)
				{
					Array.Sort<TKey>(keys, index, length, comparer);
					return;
				}
				ArraySortHelper<TKey, TValue>.Default.Sort(keys, items, index, length, comparer);
			}
		}

		/// <summary>Determines whether the specified array contains elements that match the conditions defined by the specified predicate.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>
		///   <see langword="true" /> if <paramref name="array" /> contains one or more elements that match the conditions defined by the specified predicate; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x0600142C RID: 5164 RVA: 0x0004FEB6 File Offset: 0x0004E0B6
		public static bool Exists<T>(T[] array, Predicate<T> match)
		{
			return Array.FindIndex<T>(array, match) != -1;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		public static void Fill<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004FEFC File Offset: 0x0004E0FC
		public static void Fill<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				array[i] = value;
			}
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based array to search.</param>
		/// <param name="match">The predicate that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x0600142F RID: 5167 RVA: 0x0004FF64 File Offset: 0x0004E164
		public static T Find<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		/// <summary>Retrieves all the elements that match the conditions defined by the specified predicate.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>An <see cref="T:System.Array" /> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x06001430 RID: 5168 RVA: 0x0004FFBC File Offset: 0x0004E1BC
		public static T[] FindAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num = 0;
			T[] array2 = Array.Empty<T>();
			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					if (num == array2.Length)
					{
						Array.Resize<T>(ref array2, Math.Min((num == 0) ? 4 : (num * 2), array.Length));
					}
					array2[num++] = array[i];
				}
			}
			if (num != array2.Length)
			{
				Array.Resize<T>(ref array2, num);
			}
			return array2;
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x06001431 RID: 5169 RVA: 0x00050049 File Offset: 0x0004E249
		public static int FindIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, 0, array.Length, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the specified index to the last element.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		// Token: 0x06001432 RID: 5170 RVA: 0x00050064 File Offset: 0x0004E264
		public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, startIndex, array.Length - startIndex, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that starts at the specified index and contains the specified number of elements.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		// Token: 0x06001433 RID: 5171 RVA: 0x00050084 File Offset: 0x0004E284
		public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x06001434 RID: 5172 RVA: 0x00050108 File Offset: 0x0004E308
		public static T FindLast<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x06001435 RID: 5173 RVA: 0x00050161 File Offset: 0x0004E361
		public static int FindLastIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindLastIndex<T>(array, array.Length - 1, array.Length, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		// Token: 0x06001436 RID: 5174 RVA: 0x00050180 File Offset: 0x0004E380
		public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindLastIndex<T>(array, startIndex, startIndex + 1, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.  
		/// -or-  
		/// <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		// Token: 0x06001437 RID: 5175 RVA: 0x0005019C File Offset: 0x0004E39C
		public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			if (array.Length == 0)
			{
				if (startIndex != -1)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
			}
			else if (startIndex < 0 || startIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Determines whether every element in the array matches the conditions defined by the specified predicate.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to check against the conditions.</param>
		/// <param name="match">The predicate that defines the conditions to check against the elements.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>
		///   <see langword="true" /> if every element in <paramref name="array" /> matches the conditions defined by the specified predicate; otherwise, <see langword="false" />. If there are no elements in the array, the return value is <see langword="true" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="match" /> is <see langword="null" />.</exception>
		// Token: 0x06001438 RID: 5176 RVA: 0x00050238 File Offset: 0x0004E438
		public static bool TrueForAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!match(array[i]))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</returns>
		// Token: 0x06001439 RID: 5177 RVA: 0x00050281 File Offset: 0x0004E481
		public IEnumerator GetEnumerator()
		{
			return new Array.ArrayEnumerator(this);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0000259F File Offset: 0x0000079F
		private Array()
		{
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004ED71 File Offset: 0x0004CF71
		internal int InternalArray__ICollection_get_Count()
		{
			return this.Length;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x000040F7 File Offset: 0x000022F7
		internal bool InternalArray__ICollection_get_IsReadOnly()
		{
			return true;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00050289 File Offset: 0x0004E489
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ref byte GetRawSzArrayData()
		{
			return ref Unsafe.As<Array.RawData>(this).Data;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00050296 File Offset: 0x0004E496
		internal IEnumerator<T> InternalArray__IEnumerable_GetEnumerator<T>()
		{
			if (this.Length == 0)
			{
				return Array.EmptyInternalEnumerator<T>.Value;
			}
			return new Array.InternalEnumerator<T>(this);
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x000502B1 File Offset: 0x0004E4B1
		internal void InternalArray__ICollection_Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__ICollection_Add<T>(T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal bool InternalArray__ICollection_Remove<T>(T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x000502CC File Offset: 0x0004E4CC
		internal bool InternalArray__ICollection_Contains<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException("Only single dimension arrays are supported.");
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					if (t == null)
					{
						return true;
					}
				}
				else if (item.Equals(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00050333 File Offset: 0x0004E533
		internal void InternalArray__ICollection_CopyTo<T>(T[] array, int arrayIndex)
		{
			Array.Copy(this, this.GetLowerBound(0), array, arrayIndex, this.Length);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005034C File Offset: 0x0004E54C
		internal T InternalArray__IReadOnlyList_get_Item<T>(int index)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T t;
			this.GetGenericValueImpl<T>(index, out t);
			return t;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004ED71 File Offset: 0x0004CF71
		internal int InternalArray__IReadOnlyCollection_get_Count()
		{
			return this.Length;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__Insert<T>(int index, T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00050378 File Offset: 0x0004E578
		internal int InternalArray__IndexOf<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException("Only single dimension arrays are supported.");
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					if (t == null)
					{
						return i + this.GetLowerBound(0);
					}
				}
				else if (t.Equals(item))
				{
					return i + this.GetLowerBound(0);
				}
			}
			return this.GetLowerBound(0) - 1;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000503F8 File Offset: 0x0004E5F8
		internal T InternalArray__get_Item<T>(int index)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T t;
			this.GetGenericValueImpl<T>(index, out t);
			return t;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00050424 File Offset: 0x0004E624
		internal void InternalArray__set_Item<T>(int index, T item)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object[] array = this as object[];
			if (array != null)
			{
				array[index] = item;
				return;
			}
			this.SetGenericValueImpl<T>(index, ref item);
		}

		// Token: 0x0600144B RID: 5195
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGenericValue_icall<T>(ref Array self, int pos, out T value);

		// Token: 0x0600144C RID: 5196
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGenericValue_icall<T>(ref Array self, int pos, ref T value);

		// Token: 0x0600144D RID: 5197 RVA: 0x00050464 File Offset: 0x0004E664
		internal void GetGenericValueImpl<T>(int pos, out T value)
		{
			Array array = this;
			Array.GetGenericValue_icall<T>(ref array, pos, out value);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005047C File Offset: 0x0004E67C
		internal void SetGenericValueImpl<T>(int pos, ref T value)
		{
			Array array = this;
			Array.SetGenericValue_icall<T>(ref array, pos, ref value);
		}

		/// <summary>Gets the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
		/// <returns>The total number of elements in all the dimensions of the <see cref="T:System.Array" />; zero if there are no elements in the array.</returns>
		/// <exception cref="T:System.OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x00050494 File Offset: 0x0004E694
		public int Length
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				int num = this.GetLength(0);
				for (int i = 1; i < this.Rank; i++)
				{
					num *= this.GetLength(i);
				}
				return num;
			}
		}

		/// <summary>Gets the rank (number of dimensions) of the <see cref="T:System.Array" />. For example, a one-dimensional array returns 1, a two-dimensional array returns 2, and so on.</summary>
		/// <returns>The rank (number of dimensions) of the <see cref="T:System.Array" />.</returns>
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x000504C5 File Offset: 0x0004E6C5
		public int Rank
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.GetRank();
			}
		}

		// Token: 0x06001451 RID: 5201
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetRank();

		/// <summary>Gets a 32-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
		/// <returns>A 32-bit integer that represents the number of elements in the specified dimension.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.  
		/// -or-  
		/// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		// Token: 0x06001452 RID: 5202
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLength(int dimension);

		/// <summary>Gets the index of the first element of the specified dimension in the array.</summary>
		/// <param name="dimension">A zero-based dimension of the array whose starting index needs to be determined.</param>
		/// <returns>The index of the first element of the specified dimension in the array.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.  
		/// -or-  
		/// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		// Token: 0x06001453 RID: 5203
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLowerBound(int dimension);

		/// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
		/// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001454 RID: 5204
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetValue(params int[] indices);

		/// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the element to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001455 RID: 5205
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(object value, params int[] indices);

		// Token: 0x06001456 RID: 5206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValueImpl(int pos);

		// Token: 0x06001457 RID: 5207
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValueImpl(object value, int pos);

		// Token: 0x06001458 RID: 5208
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool FastCopy(Array source, int source_idx, Array dest, int dest_idx, int length);

		// Token: 0x06001459 RID: 5209
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Array CreateInstanceImpl(Type elementType, int[] lengths, int[] bounds);

		/// <summary>Gets the index of the last element of the specified dimension in the array.</summary>
		/// <param name="dimension">A zero-based dimension of the array whose upper bound needs to be determined.</param>
		/// <returns>The index of the last element of the specified dimension in the array, or -1 if the specified dimension is empty.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.  
		/// -or-  
		/// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		// Token: 0x0600145A RID: 5210 RVA: 0x000504CD File Offset: 0x0004E6CD
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetUpperBound(int dimension)
		{
			return this.GetLowerBound(dimension) + this.GetLength(dimension) - 1;
		}

		/// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
		/// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600145B RID: 5211 RVA: 0x000504E0 File Offset: 0x0004E6E0
		public object GetValue(int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			int lowerBound = this.GetLowerBound(0);
			if (index < lowerBound || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException("Index has to be between upper and lower bound of the array.");
			}
			if (base.GetType().GetElementType().IsPointer)
			{
				throw new NotSupportedException("Type is not supported.");
			}
			return this.GetValueImpl(index - lowerBound);
		}

		/// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600145C RID: 5212 RVA: 0x00050548 File Offset: 0x0004E748
		public object GetValue(int index1, int index2)
		{
			int[] array = new int[] { index1, index2 };
			return this.GetValue(array);
		}

		/// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600145D RID: 5213 RVA: 0x0005056C File Offset: 0x0004E76C
		public object GetValue(int index1, int index2, int index3)
		{
			int[] array = new int[] { index1, index2, index3 };
			return this.GetValue(array);
		}

		/// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600145E RID: 5214 RVA: 0x00050594 File Offset: 0x0004E794
		public void SetValue(object value, int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			int lowerBound = this.GetLowerBound(0);
			if (index < lowerBound || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException("Index has to be >= lower bound and <= upper bound of the array.");
			}
			if (base.GetType().GetElementType().IsPointer)
			{
				throw new NotSupportedException("Type is not supported.");
			}
			this.SetValueImpl(value, index - lowerBound);
		}

		/// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x0600145F RID: 5215 RVA: 0x00050600 File Offset: 0x0004E800
		public void SetValue(object value, int index1, int index2)
		{
			int[] array = new int[] { index1, index2 };
			this.SetValue(value, array);
		}

		/// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		// Token: 0x06001460 RID: 5216 RVA: 0x00050624 File Offset: 0x0004E824
		public void SetValue(object value, int index1, int index2, int index3)
		{
			int[] array = new int[] { index1, index2, index3 };
			this.SetValue(value, array);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005064D File Offset: 0x0004E84D
		internal static Array UnsafeCreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
		{
			return Array.CreateInstance(elementType, lengths, lowerBounds);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00050657 File Offset: 0x0004E857
		internal static Array UnsafeCreateInstance(Type elementType, int length1, int length2)
		{
			return Array.CreateInstance(elementType, length1, length2);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00050661 File Offset: 0x0004E861
		internal static Array UnsafeCreateInstance(Type elementType, params int[] lengths)
		{
			return Array.CreateInstance(elementType, lengths);
		}

		/// <summary>Creates a one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and length, with zero-based indexing.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length">The size of the <see cref="T:System.Array" /> to create.</param>
		/// <returns>A new one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length, using zero-based indexing.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero.</exception>
		// Token: 0x06001464 RID: 5220 RVA: 0x0005066C File Offset: 0x0004E86C
		public static Array CreateInstance(Type elementType, int length)
		{
			int[] array = new int[] { length };
			return Array.CreateInstance(elementType, array);
		}

		/// <summary>Creates a two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <returns>A new two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length1" /> is less than zero.  
		/// -or-  
		/// <paramref name="length2" /> is less than zero.</exception>
		// Token: 0x06001465 RID: 5221 RVA: 0x0005068C File Offset: 0x0004E88C
		public static Array CreateInstance(Type elementType, int length1, int length2)
		{
			int[] array = new int[] { length1, length2 };
			return Array.CreateInstance(elementType, array);
		}

		/// <summary>Creates a three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length3">The size of the third dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <returns>A new three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length1" /> is less than zero.  
		/// -or-  
		/// <paramref name="length2" /> is less than zero.  
		/// -or-  
		/// <paramref name="length3" /> is less than zero.</exception>
		// Token: 0x06001466 RID: 5222 RVA: 0x000506B0 File Offset: 0x0004E8B0
		public static Array CreateInstance(Type elementType, int length1, int length2, int length3)
		{
			int[] array = new int[] { length1, length2, length3 };
			return Array.CreateInstance(elementType, array);
		}

		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 32-bit integers.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">An array of 32-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="lengths" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.  
		/// -or-  
		/// The <paramref name="lengths" /> array contains less than one element.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.</exception>
		// Token: 0x06001467 RID: 5223 RVA: 0x000506D8 File Offset: 0x0004E8D8
		public static Array CreateInstance(Type elementType, params int[] lengths)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			int[] array = null;
			elementType = elementType.UnderlyingSystemType as RuntimeType;
			if (elementType == null)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			return Array.CreateInstanceImpl(elementType, lengths, array);
		}

		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with the specified lower bounds.</summary>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">A one-dimensional array that contains the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lowerBounds">A one-dimensional array that contains the lower bound (starting index) of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length and lower bound for each dimension.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="lengths" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="lowerBounds" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.  
		/// -or-  
		/// The <paramref name="lengths" /> array contains less than one element.  
		/// -or-  
		/// The <paramref name="lengths" /> and <paramref name="lowerBounds" /> arrays do not contain the same number of elements.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.  
		/// -or-  
		/// <paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.  
		///  -or-  
		///  Any value in <paramref name="lowerBounds" /> is very large, such that the sum of a dimension's lower bound and length is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06001468 RID: 5224 RVA: 0x00050778 File Offset: 0x0004E978
		public static Array CreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lowerBounds == null)
			{
				throw new ArgumentNullException("lowerBounds");
			}
			elementType = elementType.UnderlyingSystemType as RuntimeType;
			if (elementType == null)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			if (lengths.Length < 1)
			{
				throw new ArgumentException("Arrays must contain >= 1 elements.");
			}
			if (lengths.Length != lowerBounds.Length)
			{
				throw new ArgumentException("Arrays must be of same size.");
			}
			for (int i = 0; i < lowerBounds.Length; i++)
			{
				if (lengths[i] < 0)
				{
					throw new ArgumentOutOfRangeException("lengths", "Each value has to be >= 0.");
				}
				if ((long)lowerBounds[i] + (long)lengths[i] > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("lengths", "Length + bound must not exceed Int32.MaxValue.");
				}
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			return Array.CreateInstanceImpl(elementType, lengths, lowerBounds);
		}

		/// <summary>Sets a range of elements in an array to the default value of each element type.</summary>
		/// <param name="array">The array whose elements need to be cleared.</param>
		/// <param name="index">The starting index of the range of elements to clear.</param>
		/// <param name="length">The number of elements to clear.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.  
		/// -or-  
		/// The sum of <paramref name="index" /> and <paramref name="length" /> is greater than the size of <paramref name="array" />.</exception>
		// Token: 0x06001469 RID: 5225 RVA: 0x0005088C File Offset: 0x0004EA8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Clear(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (length < 0)
			{
				throw new IndexOutOfRangeException("length < 0");
			}
			int lowerBound = array.GetLowerBound(0);
			if (index < lowerBound)
			{
				throw new IndexOutOfRangeException("index < lower bound");
			}
			index -= lowerBound;
			if (index > array.Length - length)
			{
				throw new IndexOutOfRangeException("index + length > size");
			}
			Array.ClearInternal(array, index, length);
		}

		// Token: 0x0600146A RID: 5226
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearInternal(Array a, int index, int count);

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 32-bit integer.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
		// Token: 0x0600146B RID: 5227 RVA: 0x000508F0 File Offset: 0x0004EAF0
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, Array destinationArray, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			Array.Copy(sourceArray, sourceArray.GetLowerBound(0), destinationArray, destinationArray.GetLowerBound(0), length);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 32-bit integers.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		// Token: 0x0600146C RID: 5228 RVA: 0x00050924 File Offset: 0x0004EB24
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Value has to be >= 0.");
			}
			if (sourceArray.Rank != destinationArray.Rank)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Value has to be >= 0.");
			}
			if (destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Value has to be >= 0.");
			}
			if (Array.FastCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length))
			{
				return;
			}
			int num = sourceIndex - sourceArray.GetLowerBound(0);
			int num2 = destinationIndex - destinationArray.GetLowerBound(0);
			if (num2 < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Index was less than the array's lower bound in the first dimension.");
			}
			if (num > sourceArray.Length - length)
			{
				throw new ArgumentException("length");
			}
			if (num2 > destinationArray.Length - length)
			{
				throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds", "destinationArray");
			}
			Type elementType = sourceArray.GetType().GetElementType();
			Type elementType2 = destinationArray.GetType().GetElementType();
			bool isValueType = elementType2.IsValueType;
			if (sourceArray != destinationArray || num > num2)
			{
				for (int i = 0; i < length; i++)
				{
					object valueImpl = sourceArray.GetValueImpl(num + i);
					if (valueImpl == null && isValueType)
					{
						throw new InvalidCastException();
					}
					try
					{
						destinationArray.SetValueImpl(valueImpl, num2 + i);
					}
					catch (ArgumentException)
					{
						throw Array.CreateArrayTypeMismatchException();
					}
					catch (InvalidCastException)
					{
						if (Array.CanAssignArrayElement(elementType, elementType2))
						{
							throw;
						}
						throw Array.CreateArrayTypeMismatchException();
					}
				}
				return;
			}
			for (int j = length - 1; j >= 0; j--)
			{
				object valueImpl2 = sourceArray.GetValueImpl(num + j);
				try
				{
					destinationArray.SetValueImpl(valueImpl2, num2 + j);
				}
				catch (ArgumentException)
				{
					throw Array.CreateArrayTypeMismatchException();
				}
				catch
				{
					if (Array.CanAssignArrayElement(elementType, elementType2))
					{
						throw;
					}
					throw Array.CreateArrayTypeMismatchException();
				}
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004DBDB File Offset: 0x0004BDDB
		private static ArrayTypeMismatchException CreateArrayTypeMismatchException()
		{
			return new ArrayTypeMismatchException();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00050B04 File Offset: 0x0004ED04
		private static bool CanAssignArrayElement(Type source, Type target)
		{
			if (source.IsValueType)
			{
				return source.IsAssignableFrom(target);
			}
			if (source.IsInterface)
			{
				return !target.IsValueType;
			}
			if (target.IsInterface)
			{
				return !source.IsValueType;
			}
			return source.IsAssignableFrom(target) || target.IsAssignableFrom(source);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index.  Guarantees that all changes are undone if the copy does not succeed completely.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The <paramref name="sourceArray" /> type is neither the same as nor derived from the <paramref name="destinationArray" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.  
		/// -or-  
		/// <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.  
		/// -or-  
		/// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		// Token: 0x0600146F RID: 5231 RVA: 0x00050B57 File Offset: 0x0004ED57
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void ConstrainedCopy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
		}

		/// <summary>Returns an empty array.</summary>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <returns>An empty array.</returns>
		// Token: 0x06001470 RID: 5232 RVA: 0x00050B64 File Offset: 0x0004ED64
		public static T[] Empty<T>()
		{
			return EmptyArray<T>.Value;
		}

		/// <summary>Initializes every element of the value-type <see cref="T:System.Array" /> by calling the default constructor of the value type.</summary>
		// Token: 0x06001471 RID: 5233 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Initialize()
		{
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00050B6B File Offset: 0x0004ED6B
		private static int IndexOfImpl<T>(T[] array, T value, int startIndex, int count)
		{
			return EqualityComparer<T>.Default.IndexOf(array, value, startIndex, count);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00050B7B File Offset: 0x0004ED7B
		private static int LastIndexOfImpl<T>(T[] array, T value, int startIndex, int count)
		{
			return EqualityComparer<T>.Default.LastIndexOf(array, value, startIndex, count);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00050B8C File Offset: 0x0004ED8C
		private static void SortImpl(Array keys, Array items, int index, int length, IComparer comparer)
		{
			object[] array = keys as object[];
			object[] array2 = null;
			if (array != null)
			{
				array2 = items as object[];
			}
			if (array != null && (items == null || array2 != null))
			{
				Array.SorterObjectArray sorterObjectArray = new Array.SorterObjectArray(array, array2, comparer);
				sorterObjectArray.Sort(index, length);
				return;
			}
			Array.SorterGenericArray sorterGenericArray = new Array.SorterGenericArray(keys, items, comparer);
			sorterGenericArray.Sort(index, length);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00050BDE File Offset: 0x0004EDDE
		internal static T UnsafeLoad<T>(T[] array, int index)
		{
			return array[index];
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00050BE7 File Offset: 0x0004EDE7
		internal static void UnsafeStore<T>(T[] array, int index, T value)
		{
			array[index] = value;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00050BF1 File Offset: 0x0004EDF1
		internal static R UnsafeMov<S, R>(S instance)
		{
			return (R)((object)instance);
		}

		// Token: 0x020001D2 RID: 466
		private sealed class ArrayEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x06001478 RID: 5240 RVA: 0x00050BFE File Offset: 0x0004EDFE
			internal ArrayEnumerator(Array array)
			{
				this._array = array;
				this._index = -1;
				this._endIndex = array.Length;
			}

			// Token: 0x06001479 RID: 5241 RVA: 0x00050C20 File Offset: 0x0004EE20
			public bool MoveNext()
			{
				if (this._index < this._endIndex)
				{
					this._index++;
					return this._index < this._endIndex;
				}
				return false;
			}

			// Token: 0x0600147A RID: 5242 RVA: 0x00050C4E File Offset: 0x0004EE4E
			public void Reset()
			{
				this._index = -1;
			}

			// Token: 0x0600147B RID: 5243 RVA: 0x000231D1 File Offset: 0x000213D1
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x0600147C RID: 5244 RVA: 0x00050C58 File Offset: 0x0004EE58
			public object Current
			{
				get
				{
					if (this._index < 0)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
					}
					if (this._index >= this._endIndex)
					{
						throw new InvalidOperationException("Enumeration already finished.");
					}
					if (this._index == 0 && this._array.GetType().GetElementType().IsPointer)
					{
						throw new NotSupportedException("Type is not supported.");
					}
					return this._array.GetValueImpl(this._index);
				}
			}

			// Token: 0x0400145C RID: 5212
			private Array _array;

			// Token: 0x0400145D RID: 5213
			private int _index;

			// Token: 0x0400145E RID: 5214
			private int _endIndex;
		}

		// Token: 0x020001D3 RID: 467
		[StructLayout(LayoutKind.Sequential)]
		private class RawData
		{
			// Token: 0x0400145F RID: 5215
			public IntPtr Bounds;

			// Token: 0x04001460 RID: 5216
			public IntPtr Count;

			// Token: 0x04001461 RID: 5217
			public byte Data;
		}

		// Token: 0x020001D4 RID: 468
		internal struct InternalEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600147E RID: 5246 RVA: 0x00050CCD File Offset: 0x0004EECD
			internal InternalEnumerator(Array array)
			{
				this.array = array;
				this.idx = -2;
			}

			// Token: 0x0600147F RID: 5247 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x06001480 RID: 5248 RVA: 0x00050CE0 File Offset: 0x0004EEE0
			public bool MoveNext()
			{
				if (this.idx == -2)
				{
					this.idx = this.array.Length;
				}
				if (this.idx != -1)
				{
					int num = this.idx - 1;
					this.idx = num;
					return num != -1;
				}
				return false;
			}

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x06001481 RID: 5249 RVA: 0x00050D2C File Offset: 0x0004EF2C
			public T Current
			{
				get
				{
					if (this.idx == -2)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext");
					}
					if (this.idx == -1)
					{
						throw new InvalidOperationException("Enumeration already finished");
					}
					return this.array.InternalArray__get_Item<T>(this.array.Length - 1 - this.idx);
				}
			}

			// Token: 0x06001482 RID: 5250 RVA: 0x00050D81 File Offset: 0x0004EF81
			void IEnumerator.Reset()
			{
				this.idx = -2;
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x06001483 RID: 5251 RVA: 0x00050D8B File Offset: 0x0004EF8B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04001462 RID: 5218
			private const int NOT_STARTED = -2;

			// Token: 0x04001463 RID: 5219
			private const int FINISHED = -1;

			// Token: 0x04001464 RID: 5220
			private readonly Array array;

			// Token: 0x04001465 RID: 5221
			private int idx;
		}

		// Token: 0x020001D5 RID: 469
		internal class EmptyInternalEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06001484 RID: 5252 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x06001485 RID: 5253 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public bool MoveNext()
			{
				return false;
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x06001486 RID: 5254 RVA: 0x00050D98 File Offset: 0x0004EF98
			public T Current
			{
				get
				{
					throw new InvalidOperationException("Enumeration has not started. Call MoveNext");
				}
			}

			// Token: 0x170001ED RID: 493
			// (get) Token: 0x06001487 RID: 5255 RVA: 0x00050DA4 File Offset: 0x0004EFA4
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001488 RID: 5256 RVA: 0x00004BF9 File Offset: 0x00002DF9
			void IEnumerator.Reset()
			{
			}

			// Token: 0x04001466 RID: 5222
			public static readonly Array.EmptyInternalEnumerator<T> Value = new Array.EmptyInternalEnumerator<T>();
		}

		// Token: 0x020001D6 RID: 470
		internal sealed class FunctorComparer<T> : IComparer<T>
		{
			// Token: 0x0600148B RID: 5259 RVA: 0x00050DBD File Offset: 0x0004EFBD
			public FunctorComparer(Comparison<T> comparison)
			{
				this.comparison = comparison;
			}

			// Token: 0x0600148C RID: 5260 RVA: 0x00050DCC File Offset: 0x0004EFCC
			public int Compare(T x, T y)
			{
				return this.comparison(x, y);
			}

			// Token: 0x04001467 RID: 5223
			private Comparison<T> comparison;
		}

		// Token: 0x020001D7 RID: 471
		private struct SorterObjectArray
		{
			// Token: 0x0600148D RID: 5261 RVA: 0x00050DDB File Offset: 0x0004EFDB
			internal SorterObjectArray(object[] keys, object[] items, IComparer comparer)
			{
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				this.keys = keys;
				this.items = items;
				this.comparer = comparer;
			}

			// Token: 0x0600148E RID: 5262 RVA: 0x00050DFC File Offset: 0x0004EFFC
			internal void SwapIfGreaterWithItems(int a, int b)
			{
				if (a != b && this.comparer.Compare(this.keys[a], this.keys[b]) > 0)
				{
					object obj = this.keys[a];
					this.keys[a] = this.keys[b];
					this.keys[b] = obj;
					if (this.items != null)
					{
						object obj2 = this.items[a];
						this.items[a] = this.items[b];
						this.items[b] = obj2;
					}
				}
			}

			// Token: 0x0600148F RID: 5263 RVA: 0x00050E78 File Offset: 0x0004F078
			private void Swap(int i, int j)
			{
				object obj = this.keys[i];
				this.keys[i] = this.keys[j];
				this.keys[j] = obj;
				if (this.items != null)
				{
					object obj2 = this.items[i];
					this.items[i] = this.items[j];
					this.items[j] = obj2;
				}
			}

			// Token: 0x06001490 RID: 5264 RVA: 0x00050ED1 File Offset: 0x0004F0D1
			internal void Sort(int left, int length)
			{
				this.IntrospectiveSort(left, length);
			}

			// Token: 0x06001491 RID: 5265 RVA: 0x00050EDC File Offset: 0x0004F0DC
			private void IntrospectiveSort(int left, int length)
			{
				if (length < 2)
				{
					return;
				}
				try
				{
					this.IntroSort(left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(this.keys.Length));
				}
				catch (IndexOutOfRangeException)
				{
					IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(this.comparer);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("Failed to compare two elements in the array.", ex);
				}
			}

			// Token: 0x06001492 RID: 5266 RVA: 0x00050F44 File Offset: 0x0004F144
			private void IntroSort(int lo, int hi, int depthLimit)
			{
				while (hi > lo)
				{
					int num = hi - lo + 1;
					if (num <= 16)
					{
						if (num == 1)
						{
							return;
						}
						if (num == 2)
						{
							this.SwapIfGreaterWithItems(lo, hi);
							return;
						}
						if (num == 3)
						{
							this.SwapIfGreaterWithItems(lo, hi - 1);
							this.SwapIfGreaterWithItems(lo, hi);
							this.SwapIfGreaterWithItems(hi - 1, hi);
							return;
						}
						this.InsertionSort(lo, hi);
						return;
					}
					else
					{
						if (depthLimit == 0)
						{
							this.Heapsort(lo, hi);
							return;
						}
						depthLimit--;
						int num2 = this.PickPivotAndPartition(lo, hi);
						this.IntroSort(num2 + 1, hi, depthLimit);
						hi = num2 - 1;
					}
				}
			}

			// Token: 0x06001493 RID: 5267 RVA: 0x00050FC8 File Offset: 0x0004F1C8
			private int PickPivotAndPartition(int lo, int hi)
			{
				int num = lo + (hi - lo) / 2;
				this.SwapIfGreaterWithItems(lo, num);
				this.SwapIfGreaterWithItems(lo, hi);
				this.SwapIfGreaterWithItems(num, hi);
				object obj = this.keys[num];
				this.Swap(num, hi - 1);
				int i = lo;
				int num2 = hi - 1;
				while (i < num2)
				{
					while (this.comparer.Compare(this.keys[++i], obj) < 0)
					{
					}
					while (this.comparer.Compare(obj, this.keys[--num2]) < 0)
					{
					}
					if (i >= num2)
					{
						break;
					}
					this.Swap(i, num2);
				}
				this.Swap(i, hi - 1);
				return i;
			}

			// Token: 0x06001494 RID: 5268 RVA: 0x00051064 File Offset: 0x0004F264
			private void Heapsort(int lo, int hi)
			{
				int num = hi - lo + 1;
				for (int i = num / 2; i >= 1; i--)
				{
					this.DownHeap(i, num, lo);
				}
				for (int j = num; j > 1; j--)
				{
					this.Swap(lo, lo + j - 1);
					this.DownHeap(1, j - 1, lo);
				}
			}

			// Token: 0x06001495 RID: 5269 RVA: 0x000510B4 File Offset: 0x0004F2B4
			private void DownHeap(int i, int n, int lo)
			{
				object obj = this.keys[lo + i - 1];
				object obj2 = ((this.items != null) ? this.items[lo + i - 1] : null);
				while (i <= n / 2)
				{
					int num = 2 * i;
					if (num < n && this.comparer.Compare(this.keys[lo + num - 1], this.keys[lo + num]) < 0)
					{
						num++;
					}
					if (this.comparer.Compare(obj, this.keys[lo + num - 1]) >= 0)
					{
						break;
					}
					this.keys[lo + i - 1] = this.keys[lo + num - 1];
					if (this.items != null)
					{
						this.items[lo + i - 1] = this.items[lo + num - 1];
					}
					i = num;
				}
				this.keys[lo + i - 1] = obj;
				if (this.items != null)
				{
					this.items[lo + i - 1] = obj2;
				}
			}

			// Token: 0x06001496 RID: 5270 RVA: 0x0005119C File Offset: 0x0004F39C
			private void InsertionSort(int lo, int hi)
			{
				for (int i = lo; i < hi; i++)
				{
					int num = i;
					object obj = this.keys[i + 1];
					object obj2 = ((this.items != null) ? this.items[i + 1] : null);
					while (num >= lo && this.comparer.Compare(obj, this.keys[num]) < 0)
					{
						this.keys[num + 1] = this.keys[num];
						if (this.items != null)
						{
							this.items[num + 1] = this.items[num];
						}
						num--;
					}
					this.keys[num + 1] = obj;
					if (this.items != null)
					{
						this.items[num + 1] = obj2;
					}
				}
			}

			// Token: 0x04001468 RID: 5224
			private object[] keys;

			// Token: 0x04001469 RID: 5225
			private object[] items;

			// Token: 0x0400146A RID: 5226
			private IComparer comparer;
		}

		// Token: 0x020001D8 RID: 472
		private struct SorterGenericArray
		{
			// Token: 0x06001497 RID: 5271 RVA: 0x00051249 File Offset: 0x0004F449
			internal SorterGenericArray(Array keys, Array items, IComparer comparer)
			{
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				this.keys = keys;
				this.items = items;
				this.comparer = comparer;
			}

			// Token: 0x06001498 RID: 5272 RVA: 0x0005126C File Offset: 0x0004F46C
			internal void SwapIfGreaterWithItems(int a, int b)
			{
				if (a != b && this.comparer.Compare(this.keys.GetValue(a), this.keys.GetValue(b)) > 0)
				{
					object value = this.keys.GetValue(a);
					this.keys.SetValue(this.keys.GetValue(b), a);
					this.keys.SetValue(value, b);
					if (this.items != null)
					{
						object value2 = this.items.GetValue(a);
						this.items.SetValue(this.items.GetValue(b), a);
						this.items.SetValue(value2, b);
					}
				}
			}

			// Token: 0x06001499 RID: 5273 RVA: 0x00051314 File Offset: 0x0004F514
			private void Swap(int i, int j)
			{
				object value = this.keys.GetValue(i);
				this.keys.SetValue(this.keys.GetValue(j), i);
				this.keys.SetValue(value, j);
				if (this.items != null)
				{
					object value2 = this.items.GetValue(i);
					this.items.SetValue(this.items.GetValue(j), i);
					this.items.SetValue(value2, j);
				}
			}

			// Token: 0x0600149A RID: 5274 RVA: 0x0005138D File Offset: 0x0004F58D
			internal void Sort(int left, int length)
			{
				this.IntrospectiveSort(left, length);
			}

			// Token: 0x0600149B RID: 5275 RVA: 0x00051398 File Offset: 0x0004F598
			private void IntrospectiveSort(int left, int length)
			{
				if (length < 2)
				{
					return;
				}
				try
				{
					this.IntroSort(left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(this.keys.Length));
				}
				catch (IndexOutOfRangeException)
				{
					IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(this.comparer);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("Failed to compare two elements in the array.", ex);
				}
			}

			// Token: 0x0600149C RID: 5276 RVA: 0x00051404 File Offset: 0x0004F604
			private void IntroSort(int lo, int hi, int depthLimit)
			{
				while (hi > lo)
				{
					int num = hi - lo + 1;
					if (num <= 16)
					{
						if (num == 1)
						{
							return;
						}
						if (num == 2)
						{
							this.SwapIfGreaterWithItems(lo, hi);
							return;
						}
						if (num == 3)
						{
							this.SwapIfGreaterWithItems(lo, hi - 1);
							this.SwapIfGreaterWithItems(lo, hi);
							this.SwapIfGreaterWithItems(hi - 1, hi);
							return;
						}
						this.InsertionSort(lo, hi);
						return;
					}
					else
					{
						if (depthLimit == 0)
						{
							this.Heapsort(lo, hi);
							return;
						}
						depthLimit--;
						int num2 = this.PickPivotAndPartition(lo, hi);
						this.IntroSort(num2 + 1, hi, depthLimit);
						hi = num2 - 1;
					}
				}
			}

			// Token: 0x0600149D RID: 5277 RVA: 0x00051488 File Offset: 0x0004F688
			private int PickPivotAndPartition(int lo, int hi)
			{
				int num = lo + (hi - lo) / 2;
				this.SwapIfGreaterWithItems(lo, num);
				this.SwapIfGreaterWithItems(lo, hi);
				this.SwapIfGreaterWithItems(num, hi);
				object value = this.keys.GetValue(num);
				this.Swap(num, hi - 1);
				int i = lo;
				int num2 = hi - 1;
				while (i < num2)
				{
					while (this.comparer.Compare(this.keys.GetValue(++i), value) < 0)
					{
					}
					while (this.comparer.Compare(value, this.keys.GetValue(--num2)) < 0)
					{
					}
					if (i >= num2)
					{
						break;
					}
					this.Swap(i, num2);
				}
				this.Swap(i, hi - 1);
				return i;
			}

			// Token: 0x0600149E RID: 5278 RVA: 0x00051530 File Offset: 0x0004F730
			private void Heapsort(int lo, int hi)
			{
				int num = hi - lo + 1;
				for (int i = num / 2; i >= 1; i--)
				{
					this.DownHeap(i, num, lo);
				}
				for (int j = num; j > 1; j--)
				{
					this.Swap(lo, lo + j - 1);
					this.DownHeap(1, j - 1, lo);
				}
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x00051580 File Offset: 0x0004F780
			private void DownHeap(int i, int n, int lo)
			{
				object value = this.keys.GetValue(lo + i - 1);
				object obj = ((this.items != null) ? this.items.GetValue(lo + i - 1) : null);
				while (i <= n / 2)
				{
					int num = 2 * i;
					if (num < n && this.comparer.Compare(this.keys.GetValue(lo + num - 1), this.keys.GetValue(lo + num)) < 0)
					{
						num++;
					}
					if (this.comparer.Compare(value, this.keys.GetValue(lo + num - 1)) >= 0)
					{
						break;
					}
					this.keys.SetValue(this.keys.GetValue(lo + num - 1), lo + i - 1);
					if (this.items != null)
					{
						this.items.SetValue(this.items.GetValue(lo + num - 1), lo + i - 1);
					}
					i = num;
				}
				this.keys.SetValue(value, lo + i - 1);
				if (this.items != null)
				{
					this.items.SetValue(obj, lo + i - 1);
				}
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x00051694 File Offset: 0x0004F894
			private void InsertionSort(int lo, int hi)
			{
				for (int i = lo; i < hi; i++)
				{
					int num = i;
					object value = this.keys.GetValue(i + 1);
					object obj = ((this.items != null) ? this.items.GetValue(i + 1) : null);
					while (num >= lo && this.comparer.Compare(value, this.keys.GetValue(num)) < 0)
					{
						this.keys.SetValue(this.keys.GetValue(num), num + 1);
						if (this.items != null)
						{
							this.items.SetValue(this.items.GetValue(num), num + 1);
						}
						num--;
					}
					this.keys.SetValue(value, num + 1);
					if (this.items != null)
					{
						this.items.SetValue(obj, num + 1);
					}
				}
			}

			// Token: 0x0400146B RID: 5227
			private Array keys;

			// Token: 0x0400146C RID: 5228
			private Array items;

			// Token: 0x0400146D RID: 5229
			private IComparer comparer;
		}
	}
}
