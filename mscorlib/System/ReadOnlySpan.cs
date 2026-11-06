using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
	// Token: 0x02000176 RID: 374
	[NonVersionable]
	[DebuggerTypeProxy(typeof(SpanDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	public readonly ref struct ReadOnlySpan<T>
	{
		// Token: 0x06000EC8 RID: 3784 RVA: 0x0003C660 File Offset: 0x0003A860
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan(T[] array)
		{
			if (array == null)
			{
				this = default(ReadOnlySpan<T>);
				return;
			}
			this._pointer = new ByReference<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()));
			this._length = array.Length;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0003C68C File Offset: 0x0003A88C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this = default(ReadOnlySpan<T>);
				return;
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._pointer = new ByReference<T>(Unsafe.Add<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), start));
			this._length = length;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0003C6E5 File Offset: 0x0003A8E5
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan(void* pointer, int length)
		{
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof(T));
			}
			if (length < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._pointer = new ByReference<T>(Unsafe.As<byte, T>(ref *(byte*)pointer));
			this._length = length;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0003C71E File Offset: 0x0003A91E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan(ref T ptr, int length)
		{
			this._pointer = new ByReference<T>(ref ptr);
			this._length = length;
		}

		// Token: 0x1700010D RID: 269
		public ref T this[int index]
		{
			[NonVersionable]
			[Intrinsic]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= this._length)
				{
					ThrowHelper.ThrowIndexOutOfRangeException();
				}
				return Unsafe.Add<T>(this._pointer.Value, index);
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0003C764 File Offset: 0x0003A964
		public readonly ref T GetPinnableReference()
		{
			if (this._length == 0)
			{
				return Unsafe.AsRef<T>(null);
			}
			return this._pointer.Value;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0003C790 File Offset: 0x0003A990
		public void CopyTo(Span<T> destination)
		{
			if (this._length <= destination.Length)
			{
				Buffer.Memmove<T>(destination._pointer.Value, this._pointer.Value, (ulong)((long)this._length));
				return;
			}
			ThrowHelper.ThrowArgumentException_DestinationTooShort();
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0003C7DC File Offset: 0x0003A9DC
		public bool TryCopyTo(Span<T> destination)
		{
			bool flag = false;
			if (this._length <= destination.Length)
			{
				Buffer.Memmove<T>(destination._pointer.Value, this._pointer.Value, (ulong)((long)this._length));
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0003C828 File Offset: 0x0003AA28
		public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
		{
			return left._length == right._length && Unsafe.AreSame<T>(left._pointer.Value, right._pointer.Value);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0003C868 File Offset: 0x0003AA68
		public unsafe override string ToString()
		{
			if (typeof(T) == typeof(char))
			{
				fixed (char* ptr = Unsafe.As<T, char>(this._pointer.Value))
				{
					return new string(ptr, 0, this._length);
				}
			}
			return string.Format("System.ReadOnlySpan<{0}>[{1}]", typeof(T).Name, this._length);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0003C8D8 File Offset: 0x0003AAD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> Slice(int start)
		{
			if (start > this._length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return new ReadOnlySpan<T>(Unsafe.Add<T>(this._pointer.Value, start), this._length - start);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0003C914 File Offset: 0x0003AB14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> Slice(int start, int length)
		{
			if (start > this._length || length > this._length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return new ReadOnlySpan<T>(Unsafe.Add<T>(this._pointer.Value, start), length);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0003C954 File Offset: 0x0003AB54
		public T[] ToArray()
		{
			if (this._length == 0)
			{
				return Array.Empty<T>();
			}
			T[] array = new T[this._length];
			Buffer.Memmove<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), this._pointer.Value, (ulong)((long)this._length));
			return array;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public int Length
		{
			[NonVersionable]
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0003C9A7 File Offset: 0x0003ABA7
		public bool IsEmpty
		{
			[NonVersionable]
			get
			{
				return this._length == 0;
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0003C9B2 File Offset: 0x0003ABB2
		public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
		{
			return !(left == right);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003C9BE File Offset: 0x0003ABBE
		[Obsolete("Equals() on ReadOnlySpan will always throw an exception. Use == instead.")]
		public override bool Equals(object obj)
		{
			throw new NotSupportedException("Equals() on Span and ReadOnlySpan is not supported. Use operator== instead.");
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003C9CA File Offset: 0x0003ABCA
		[Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
		public override int GetHashCode()
		{
			throw new NotSupportedException("GetHashCode() on Span and ReadOnlySpan is not supported.");
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003C9D6 File Offset: 0x0003ABD6
		public static implicit operator ReadOnlySpan<T>(T[] array)
		{
			return new ReadOnlySpan<T>(array);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0003C9DE File Offset: 0x0003ABDE
		public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment)
		{
			return new ReadOnlySpan<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x0003C9FC File Offset: 0x0003ABFC
		public static ReadOnlySpan<T> Empty
		{
			get
			{
				return default(ReadOnlySpan<T>);
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0003CA12 File Offset: 0x0003AC12
		public ReadOnlySpan<T>.Enumerator GetEnumerator()
		{
			return new ReadOnlySpan<T>.Enumerator(this);
		}

		// Token: 0x040012D2 RID: 4818
		internal readonly ByReference<T> _pointer;

		// Token: 0x040012D3 RID: 4819
		private readonly int _length;

		// Token: 0x02000177 RID: 375
		[Obsolete("Types with embedded references are not supported in this version of your compiler.", true)]
		public ref struct Enumerator
		{
			// Token: 0x06000EDE RID: 3806 RVA: 0x0003CA1F File Offset: 0x0003AC1F
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal Enumerator(ReadOnlySpan<T> span)
			{
				this._span = span;
				this._index = -1;
			}

			// Token: 0x06000EDF RID: 3807 RVA: 0x0003CA30 File Offset: 0x0003AC30
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				int num = this._index + 1;
				if (num < this._span.Length)
				{
					this._index = num;
					return true;
				}
				return false;
			}

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0003CA5E File Offset: 0x0003AC5E
			public readonly ref T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._span[this._index];
				}
			}

			// Token: 0x040012D4 RID: 4820
			private readonly ReadOnlySpan<T> _span;

			// Token: 0x040012D5 RID: 4821
			private int _index;
		}
	}
}
