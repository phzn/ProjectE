using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x0200004C RID: 76
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(FixedBytes32Align8)
	})]
	[Serializable]
	internal struct FixedList<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U> : INativeList<T>, IIndexable<T> where T : struct, ValueType where U : struct, ValueType
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00006B44 File Offset: 0x00004D44
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00006B60 File Offset: 0x00004D60
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (U* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (U* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00006B80 File Offset: 0x00004D80
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (U* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00006B9E File Offset: 0x00004D9E
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00006BA6 File Offset: 0x00004DA6
		[CreateProperty]
		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return (int)this.length;
			}
			set
			{
				this.length = (ushort)value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00006BB0 File Offset: 0x00004DB0
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00006BB8 File Offset: 0x00004DB8
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00006BC3 File Offset: 0x00004DC3
		internal readonly int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00006BD1 File Offset: 0x00004DD1
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00006BDF File Offset: 0x00004DDF
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<U, T>();
			}
			set
			{
			}
		}

		// Token: 0x17000042 RID: 66
		public unsafe T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return UnsafeUtility.ReadArrayElement<T>((void*)this.Buffer, CollectionHelper.AssumePositive(index));
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				UnsafeUtility.WriteArrayElement<T>((void*)this.Buffer, CollectionHelper.AssumePositive(index), value);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00006C0D File Offset: 0x00004E0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00006C1B File Offset: 0x00004E1B
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00006C2E File Offset: 0x00004E2E
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00006C37 File Offset: 0x00004E37
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00006C44 File Offset: 0x00004E44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006C70 File Offset: 0x00004E70
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00006CB0 File Offset: 0x00004EB0
		public unsafe void AddReplicate(in T value, int count)
		{
			int length = this.Length;
			this.Length += count;
			fixed (T* ptr = &value)
			{
				T* ptr2 = ptr;
				UnsafeUtility.MemCpyReplicate((void*)(this.Buffer + (IntPtr)length * (IntPtr)sizeof(T)), (void*)ptr2, UnsafeUtility.SizeOf<T>(), count);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00006CF6 File Offset: 0x00004EF6
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00006D00 File Offset: 0x00004F00
		public unsafe void InsertRangeWithBeginEnd(int begin, int end)
		{
			int num = end - begin;
			if (num < 1)
			{
				return;
			}
			int num2 = (int)this.length - begin;
			this.Length += num;
			if (num2 < 1)
			{
				return;
			}
			int num3 = num2 * UnsafeUtility.SizeOf<T>();
			byte* buffer = this.Buffer;
			byte* ptr = buffer + end * UnsafeUtility.SizeOf<T>();
			byte* ptr2 = buffer + begin * UnsafeUtility.SizeOf<T>();
			UnsafeUtility.MemMove((void*)ptr, (void*)ptr2, (long)num3);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00006D5E File Offset: 0x00004F5E
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00006D6A File Offset: 0x00004F6A
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006D83 File Offset: 0x00004F83
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006D90 File Offset: 0x00004F90
		public unsafe void RemoveRangeSwapBack(int index, int count)
		{
			if (count > 0)
			{
				int num = math.max(this.Length - count, index + count);
				int num2 = UnsafeUtility.SizeOf<T>();
				void* ptr = (void*)(this.Buffer + index * num2);
				void* ptr2 = (void*)(this.Buffer + num * num2);
				UnsafeUtility.MemCpy(ptr, ptr2, (long)((this.Length - num) * num2));
				this.Length -= count;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00006DEE File Offset: 0x00004FEE
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00006DF8 File Offset: 0x00004FF8
		public unsafe void RemoveRange(int index, int count)
		{
			if (count > 0)
			{
				int num = math.min(index + count, this.Length);
				int num2 = UnsafeUtility.SizeOf<T>();
				void* ptr = (void*)(this.Buffer + index * num2);
				void* ptr2 = (void*)(this.Buffer + num * num2);
				UnsafeUtility.MemCpy(ptr, ptr2, (long)((this.Length - num) * num2));
				this.Length -= count;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006E54 File Offset: 0x00005054
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public unsafe T[] ToArray()
		{
			T[] array = new T[this.Length];
			byte* buffer = this.Buffer;
			fixed (T[] array2 = array)
			{
				T* ptr;
				if (array == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				UnsafeUtility.MemCpy((void*)ptr, (void*)buffer, (long)this.LengthInBytes);
			}
			return array;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006E9B File Offset: 0x0000509B
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		internal U data;
	}
}
