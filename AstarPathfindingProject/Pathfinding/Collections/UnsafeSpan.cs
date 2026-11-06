using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x02000275 RID: 629
	public readonly struct UnsafeSpan<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0005FB81 File Offset: 0x0005DD81
		public int Length
		{
			get
			{
				return (int)this.length;
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0005FB89 File Offset: 0x0005DD89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan(void* ptr, int length, Allocator allocator = Allocator.None)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (length > 0 && ptr == null)
			{
				throw new ArgumentNullException();
			}
			this.ptr = (T*)ptr;
			this.length = (uint)length;
			this.Allocator = allocator;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0005FBB9 File Offset: 0x0005DDB9
		public UnsafeSpan(T[] data, Allocator allocator)
		{
			this = new UnsafeSpan<T>(allocator, data.Length);
			this.CopyFrom<T>(data);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0005FBD1 File Offset: 0x0005DDD1
		public unsafe UnsafeSpan(T[] data, out ulong gcHandle)
		{
			this.ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.length = (uint)data.Length;
			this.Allocator = Allocator.None;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0005FBF0 File Offset: 0x0005DDF0
		public unsafe UnsafeSpan(T[,] data, out ulong gcHandle)
		{
			this.ptr = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.length = (uint)data.Length;
			this.Allocator = Allocator.None;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0005FC14 File Offset: 0x0005DE14
		public unsafe UnsafeSpan(Allocator allocator, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (length > 0)
			{
				this.ptr = (T*)UnsafeUtility.MallocTracked((long)length * (long)UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator, 1);
			}
			else
			{
				this.ptr = null;
			}
			this.length = (uint)length;
			this.Allocator = allocator;
		}

		// Token: 0x17000251 RID: 593
		public unsafe ref T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= (int)this.length)
				{
					throw new IndexOutOfRangeException();
				}
				Hint.Assume(this.ptr != null);
				return ref this.ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x17000252 RID: 594
		public unsafe ref T this[uint index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= this.length)
				{
					throw new IndexOutOfRangeException();
				}
				Hint.Assume(this.ptr != null);
				Hint.Assume(this.ptr + (ulong)index * (ulong)((long)sizeof(T)) / (ulong)sizeof(T) != null);
				return ref this.ptr[(ulong)index * (ulong)((long)sizeof(T)) / (ulong)sizeof(T)];
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0005FCF6 File Offset: 0x0005DEF6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan<U> Reinterpret<[IsUnmanaged] U>() where U : struct, ValueType
		{
			if (sizeof(T) != sizeof(U))
			{
				throw new InvalidOperationException("Cannot reinterpret span because the size of the types do not match");
			}
			return new UnsafeSpan<U>((void*)this.ptr, (int)this.length, this.Allocator);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0005FD28 File Offset: 0x0005DF28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe UnsafeSpan<U> Reinterpret<[IsUnmanaged] U>(int expectedOriginalTypeSize) where U : struct, ValueType
		{
			return new UnsafeSpan<U>((void*)this.ptr, (int)(this.length * (uint)sizeof(T) / (uint)sizeof(U)), this.Allocator);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0005FD4F File Offset: 0x0005DF4F
		public unsafe UnsafeSpan<T> Slice(int start, int length)
		{
			if (start < 0 || length < 0 || (long)(start + length) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			return new UnsafeSpan<T>((void*)(this.ptr + (IntPtr)start * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), length, this.Allocator);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0005FD88 File Offset: 0x0005DF88
		public UnsafeSpan<T> Slice(int start)
		{
			return this.Slice(start, (int)(this.length - (uint)start));
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0005FD9C File Offset: 0x0005DF9C
		public unsafe void Move(int startIndex, int toIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (startIndex < 0 || (long)(startIndex + count) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			if (toIndex < 0 || (long)(toIndex + count) > (long)((ulong)this.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			if (count == 0)
			{
				return;
			}
			UnsafeUtility.MemMove((void*)(this.ptr + (IntPtr)toIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)(this.ptr + (IntPtr)startIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)sizeof(T) * (long)count);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0005FE14 File Offset: 0x0005E014
		public static void RemoveAt(ref UnsafeSpan<T> span, int index)
		{
			if (index < 0 || (long)index >= (long)((ulong)span.length))
			{
				throw new ArgumentOutOfRangeException();
			}
			span.Move(index + 1, index, (int)(span.length - (uint)index - 1U));
			span = span.Slice(0, (int)(span.length - 1U));
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0005FE54 File Offset: 0x0005E054
		public unsafe void CopyTo(UnsafeSpan<T> other)
		{
			if (other.length < this.length)
			{
				throw new ArgumentException();
			}
			if (this.length > 0U)
			{
				UnsafeUtility.MemCpy((void*)other.ptr, (void*)this.ptr, (long)sizeof(T) * (long)((ulong)this.length));
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0005FE94 File Offset: 0x0005E094
		public unsafe void CopyTo(List<T> buffer)
		{
			if (buffer.Capacity < buffer.Count + this.Length)
			{
				buffer.Capacity = buffer.Count + this.Length;
			}
			for (int i = 0; i < this.Length; i++)
			{
				buffer.Add(*this[i]);
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0005FEEC File Offset: 0x0005E0EC
		public UnsafeSpan<T> Clone(Allocator allocator)
		{
			UnsafeSpan<T> unsafeSpan = new UnsafeSpan<T>(allocator, (int)this.length);
			this.CopyTo(unsafeSpan);
			return unsafeSpan;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0005FF10 File Offset: 0x0005E110
		public unsafe T[] ToArray()
		{
			T[] array = new T[this.length];
			if (this.length > 0U)
			{
				T[] array2;
				T* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				UnsafeUtility.MemCpy((void*)ptr, (void*)this.ptr, (long)sizeof(T) * (long)((ulong)this.length));
				array2 = null;
			}
			return array;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0005FF68 File Offset: 0x0005E168
		public unsafe NativeArray<T> MoveToNativeArray(Allocator allocator)
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.ptr, this.Length, allocator);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0005FF7C File Offset: 0x0005E17C
		public unsafe void Free(Allocator expectedAllocator)
		{
			if (this.length > 0U)
			{
				UnsafeUtility.FreeTracked((void*)this.ptr, expectedAllocator);
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0005FF93 File Offset: 0x0005E193
		public void Free()
		{
			this.Free(this.Allocator);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0005FFA4 File Offset: 0x0005E1A4
		public UnsafeSpan<T> Reallocate(Allocator allocator, int newSize)
		{
			uint num = this.length;
			if (newSize < 0)
			{
				throw new ArgumentOutOfRangeException("newSize", "New size must be non-negative");
			}
			if (newSize == this.Length)
			{
				return this;
			}
			UnsafeSpan<T> unsafeSpan = new UnsafeSpan<T>(allocator, newSize);
			this.Slice(0, Math.Min(newSize, this.Length)).CopyTo(unsafeSpan);
			this.Free(allocator);
			return unsafeSpan;
		}

		// Token: 0x04000B9A RID: 2970
		[NativeDisableUnsafePtrRestriction]
		internal unsafe readonly T* ptr;

		// Token: 0x04000B9B RID: 2971
		internal readonly uint length;

		// Token: 0x04000B9C RID: 2972
		public readonly Allocator Allocator;
	}
}
