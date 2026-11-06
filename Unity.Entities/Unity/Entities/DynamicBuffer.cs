using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000209 RID: 521
	[NativeContainer]
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}")]
	[DebuggerTypeProxy(typeof(DynamicBufferDebugView<>))]
	public struct DynamicBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IQueryTypeParameter, IEnumerable<T>, IEnumerable, INativeList<T>, IIndexable<T> where T : struct, ValueType
	{
		// Token: 0x06000C1D RID: 3101 RVA: 0x0003820D File Offset: 0x0003640D
		internal unsafe DynamicBuffer(BufferHeader* header, int internalCapacity)
		{
			this.m_Buffer = header;
			this.m_InternalCapacity = internalCapacity;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0003821D File Offset: 0x0003641D
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003822A File Offset: 0x0003642A
		public unsafe int Length
		{
			get
			{
				return this.m_Buffer->Length;
			}
			set
			{
				this.ResizeUninitialized(value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00038233 File Offset: 0x00036433
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x00038240 File Offset: 0x00036440
		public unsafe int Capacity
		{
			get
			{
				return this.m_Buffer->Capacity;
			}
			set
			{
				BufferHeader.SetCapacity(this.m_Buffer, value, UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), BufferHeader.TrashMode.RetainOldData, false, 0, this.m_InternalCapacity);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00038261 File Offset: 0x00036461
		public bool IsEmpty
		{
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00038276 File Offset: 0x00036476
		public bool IsCreated
		{
			get
			{
				return this.m_Buffer != null;
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00038285 File Offset: 0x00036485
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckBounds(int index)
		{
			if (index >= this.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in DynamicBuffer of '{1}' Length.", index, this.Length));
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReadAccess()
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckWriteAccess()
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckWriteAccessAndInvalidateArrayAliases()
		{
		}

		// Token: 0x170000E5 RID: 229
		public unsafe T this[int index]
		{
			get
			{
				return UnsafeUtility.ReadArrayElement<T>((void*)BufferHeader.GetElementPointer(this.m_Buffer), index);
			}
			set
			{
				UnsafeUtility.WriteArrayElement<T>((void*)BufferHeader.GetElementPointer(this.m_Buffer), index, value);
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000382D8 File Offset: 0x000364D8
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)BufferHeader.GetElementPointer(this.m_Buffer), index);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000382EB File Offset: 0x000364EB
		public unsafe void ResizeUninitialized(int length)
		{
			this.EnsureCapacity(length);
			this.m_Buffer->Length = length;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00038300 File Offset: 0x00036500
		public unsafe void Resize(int length, NativeArrayOptions options)
		{
			this.EnsureCapacity(length);
			int length2 = this.m_Buffer->Length;
			this.m_Buffer->Length = length;
			if (options == NativeArrayOptions.ClearMemory && length2 < length)
			{
				int num = length - length2;
				byte* elementPointer = BufferHeader.GetElementPointer(this.m_Buffer);
				int num2 = UnsafeUtility.SizeOf<T>();
				UnsafeUtility.MemClear((void*)((byte*)elementPointer + length2 * num2), (long)(num * num2));
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00038356 File Offset: 0x00036556
		public void EnsureCapacity(int length)
		{
			BufferHeader.EnsureCapacity(this.m_Buffer, length, UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), BufferHeader.TrashMode.RetainOldData, false, 0);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00038371 File Offset: 0x00036571
		public unsafe void Clear()
		{
			this.m_Buffer->Length = 0;
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00038380 File Offset: 0x00036580
		public unsafe void TrimExcess()
		{
			byte* pointer = this.m_Buffer->Pointer;
			int length = this.m_Buffer->Length;
			if (length == this.Capacity || pointer == null)
			{
				return;
			}
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.AlignOf<T>();
			byte* ptr;
			bool flag;
			if (length <= this.m_InternalCapacity)
			{
				ptr = (byte*)(this.m_Buffer + 1);
				flag = true;
			}
			else
			{
				ptr = (byte*)Memory.Unmanaged.Allocate((long)num * (long)length, num2, Allocator.Persistent);
				flag = false;
			}
			UnsafeUtility.MemCpy((void*)ptr, (void*)pointer, (long)num * (long)length);
			this.m_Buffer->Capacity = Math.Max(length, this.m_InternalCapacity);
			this.m_Buffer->Pointer = (flag ? null : ptr);
			Memory.Unmanaged.Free<byte>(pointer, Allocator.Persistent);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00038438 File Offset: 0x00036638
		public int Add(T elem)
		{
			int length = this.Length;
			this.ResizeUninitialized(length + 1);
			this[length] = elem;
			return length;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00038460 File Offset: 0x00036660
		public unsafe void Insert(int index, T elem)
		{
			int length = this.Length;
			this.ResizeUninitialized(length + 1);
			int num = UnsafeUtility.SizeOf<T>();
			byte* elementPointer = BufferHeader.GetElementPointer(this.m_Buffer);
			UnsafeUtility.MemMove((void*)(elementPointer + (index + 1) * num), (void*)(elementPointer + index * num), (long)num * (long)(length - index));
			this[index] = elem;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000384B0 File Offset: 0x000366B0
		public unsafe void AddRange(NativeArray<T> newElems)
		{
			int num = UnsafeUtility.SizeOf<T>();
			int length = this.Length;
			this.ResizeUninitialized(length + newElems.Length);
			UnsafeUtility.MemCpy((void*)(BufferHeader.GetElementPointer(this.m_Buffer) + (long)length * (long)num), newElems.GetUnsafeReadOnlyPtr<T>(), (long)num * (long)newElems.Length);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00038504 File Offset: 0x00036704
		public unsafe void RemoveRange(int index, int count)
		{
			if (count == 0)
			{
				return;
			}
			int num = UnsafeUtility.SizeOf<T>();
			byte* elementPointer = BufferHeader.GetElementPointer(this.m_Buffer);
			UnsafeUtility.MemMove((void*)(elementPointer + index * num), (void*)(elementPointer + (index + count) * num), (long)num * (long)(this.Length - count - index));
			this.m_Buffer->Length -= count;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00038558 File Offset: 0x00036758
		public unsafe void RemoveRangeSwapBack(int index, int count)
		{
			if (count == 0)
			{
				return;
			}
			ref int ptr = ref this.m_Buffer->Length;
			byte* elementPointer = BufferHeader.GetElementPointer(this.m_Buffer);
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = math.max(ptr - count, index + count);
			void* ptr2 = (void*)(elementPointer + index * num);
			void* ptr3 = (void*)(elementPointer + num2 * num);
			UnsafeUtility.MemMove(ptr2, ptr3, (long)((ptr - num2) * num));
			ptr -= count;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000385B4 File Offset: 0x000367B4
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000385C0 File Offset: 0x000367C0
		public unsafe void RemoveAtSwapBack(int index)
		{
			BufferHeader* buffer = this.m_Buffer;
			buffer->Length--;
			int length = buffer->Length;
			if (index != length)
			{
				byte* elementPointer = BufferHeader.GetElementPointer(this.m_Buffer);
				UnsafeUtility.WriteArrayElement<T>((void*)elementPointer, index, UnsafeUtility.ReadArrayElement<T>((void*)elementPointer, length));
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000385FE File Offset: 0x000367FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void* GetUnsafePtr()
		{
			return (void*)BufferHeader.GetElementPointer(this.m_Buffer);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x000385FE File Offset: 0x000367FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void* GetUnsafeReadOnlyPtr()
		{
			return (void*)BufferHeader.GetElementPointer(this.m_Buffer);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0003860B File Offset: 0x0003680B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void AssertReinterpretSizesMatch<U>() where U : struct
		{
			if (UnsafeUtility.SizeOf<U>() != UnsafeUtility.SizeOf<T>())
			{
				throw new InvalidOperationException(string.Format("Types {0} and {1} are of different sizes; cannot reinterpret", typeof(U), typeof(T)));
			}
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0003863D File Offset: 0x0003683D
		public DynamicBuffer<U> Reinterpret<[global::System.Runtime.CompilerServices.IsUnmanaged] U>() where U : struct, ValueType
		{
			return new DynamicBuffer<U>(this.m_Buffer, this.m_InternalCapacity);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00038650 File Offset: 0x00036850
		public unsafe NativeArray<T> AsNativeArray()
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)BufferHeader.GetElementPointer(this.m_Buffer), this.Length, Allocator.None);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0003866C File Offset: 0x0003686C
		public NativeArray<T>.Enumerator GetEnumerator()
		{
			NativeArray<T> nativeArray = this.AsNativeArray();
			return new NativeArray<T>.Enumerator(ref nativeArray);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00038687 File Offset: 0x00036887
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00038687 File Offset: 0x00036887
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0003868E File Offset: 0x0003688E
		public NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			return CollectionHelper.CreateNativeArray<T>(this.AsNativeArray(), allocator);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003869C File Offset: 0x0003689C
		public void CopyFrom(NativeArray<T> v)
		{
			this.ResizeUninitialized(v.Length);
			NativeSlice<T> nativeSlice = new NativeSlice<T>(v);
			nativeSlice.CopyTo(this.AsNativeArray());
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000386CB File Offset: 0x000368CB
		public void CopyFrom(NativeSlice<T> v)
		{
			this.ResizeUninitialized(v.Length);
			v.CopyTo(this.AsNativeArray());
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000386E7 File Offset: 0x000368E7
		public unsafe void CopyFrom(DynamicBuffer<T> v)
		{
			this.ResizeUninitialized(v.Length);
			UnsafeUtility.MemCpy((void*)BufferHeader.GetElementPointer(this.m_Buffer), (void*)BufferHeader.GetElementPointer(v.m_Buffer), (long)(this.Length * UnsafeUtility.SizeOf<T>()));
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00038720 File Offset: 0x00036920
		public unsafe void CopyFrom(T[] v)
		{
			if (v == null)
			{
				throw new ArgumentNullException("v");
			}
			this.ResizeUninitialized(v.Length);
			GCHandle gchandle = GCHandle.Alloc(v, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			UnsafeUtility.MemCpy((void*)BufferHeader.GetElementPointer(this.m_Buffer), (void*)intPtr, (long)(this.Length * UnsafeUtility.SizeOf<T>()));
			gchandle.Free();
		}

		// Token: 0x0400055B RID: 1371
		[NativeDisableUnsafePtrRestriction]
		[NoAlias]
		private unsafe BufferHeader* m_Buffer;

		// Token: 0x0400055C RID: 1372
		private int m_InternalCapacity;
	}
}
