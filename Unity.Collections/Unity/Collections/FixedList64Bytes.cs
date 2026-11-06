using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x02000052 RID: 82
	[DebuggerTypeProxy(typeof(FixedList64BytesDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	[Serializable]
	public struct FixedList64Bytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable, IEquatable<FixedList32Bytes<T>>, IComparable<FixedList32Bytes<T>>, IEquatable<FixedList64Bytes<T>>, IComparable<FixedList64Bytes<T>>, IEquatable<FixedList128Bytes<T>>, IComparable<FixedList128Bytes<T>>, IEquatable<FixedList512Bytes<T>>, IComparable<FixedList512Bytes<T>>, IEquatable<FixedList4096Bytes<T>>, IComparable<FixedList4096Bytes<T>> where T : struct, ValueType
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00007A00 File Offset: 0x00005C00
		// (set) Token: 0x06000271 RID: 625 RVA: 0x00007A1C File Offset: 0x00005C1C
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (FixedBytes64Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (FixedBytes64Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00007A3C File Offset: 0x00005C3C
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (FixedBytes64Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00007A5A File Offset: 0x00005C5A
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00007A62 File Offset: 0x00005C62
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00007A6C File Offset: 0x00005C6C
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00007A74 File Offset: 0x00005C74
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00007A7F File Offset: 0x00005C7F
		internal int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00007A8D File Offset: 0x00005C8D
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007A9B File Offset: 0x00005C9B
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<FixedBytes64Align8, T>();
			}
			set
			{
			}
		}

		// Token: 0x17000057 RID: 87
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

		// Token: 0x0600027D RID: 637 RVA: 0x00007AC9 File Offset: 0x00005CC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007AD7 File Offset: 0x00005CD7
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00007AEA File Offset: 0x00005CEA
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00007AF3 File Offset: 0x00005CF3
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00007B00 File Offset: 0x00005D00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00007B2C File Offset: 0x00005D2C
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00007B6C File Offset: 0x00005D6C
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

		// Token: 0x06000284 RID: 644 RVA: 0x00007BB2 File Offset: 0x00005DB2
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007BBC File Offset: 0x00005DBC
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

		// Token: 0x06000286 RID: 646 RVA: 0x00007C1A File Offset: 0x00005E1A
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00007C26 File Offset: 0x00005E26
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00007C3F File Offset: 0x00005E3F
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00007C4C File Offset: 0x00005E4C
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

		// Token: 0x0600028A RID: 650 RVA: 0x00007CAA File Offset: 0x00005EAA
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00007CB4 File Offset: 0x00005EB4
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

		// Token: 0x0600028C RID: 652 RVA: 0x00007D10 File Offset: 0x00005F10
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

		// Token: 0x0600028D RID: 653 RVA: 0x00007D57 File Offset: 0x00005F57
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00007D80 File Offset: 0x00005F80
		public unsafe static bool operator ==(in FixedList64Bytes<T> a, in FixedList32Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList64Bytes<T> fixedList64Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList64Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public static bool operator !=(in FixedList64Bytes<T> a, in FixedList32Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00007DCC File Offset: 0x00005FCC
		public unsafe int CompareTo(FixedList32Bytes<T> other)
		{
			byte* buffer = this.buffer;
			byte* buffer2 = other.buffer;
			byte* ptr = buffer + FixedList.PaddingBytes<T>();
			byte* ptr2 = buffer2 + FixedList.PaddingBytes<T>();
			int num = math.min(this.Length, other.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnsafeUtility.MemCmp((void*)(ptr + sizeof(T) * i), (void*)(ptr2 + sizeof(T) * i), (long)sizeof(T));
				if (num2 != 0)
				{
					return num2;
				}
			}
			return this.Length.CompareTo(other.Length);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00007E5A File Offset: 0x0000605A
		public bool Equals(FixedList32Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00007E66 File Offset: 0x00006066
		public FixedList64Bytes(in FixedList32Bytes<T> other)
		{
			this = default(FixedList64Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00007E77 File Offset: 0x00006077
		internal unsafe int Initialize(in FixedList32Bytes<T> other)
		{
			if (other.Length > this.Capacity)
			{
				return 1;
			}
			this.length = other.length;
			UnsafeUtility.MemCpy((void*)this.Buffer, (void*)other.Buffer, (long)this.LengthInBytes);
			return 0;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00007EAE File Offset: 0x000060AE
		public static implicit operator FixedList64Bytes<T>(in FixedList32Bytes<T> other)
		{
			return new FixedList64Bytes<T>(in other);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00007EB8 File Offset: 0x000060B8
		public unsafe static bool operator ==(in FixedList64Bytes<T> a, in FixedList64Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList64Bytes<T> fixedList64Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList64Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00007EF8 File Offset: 0x000060F8
		public static bool operator !=(in FixedList64Bytes<T> a, in FixedList64Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00007F04 File Offset: 0x00006104
		public unsafe int CompareTo(FixedList64Bytes<T> other)
		{
			byte* buffer = this.buffer;
			byte* buffer2 = other.buffer;
			byte* ptr = buffer + FixedList.PaddingBytes<T>();
			byte* ptr2 = buffer2 + FixedList.PaddingBytes<T>();
			int num = math.min(this.Length, other.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnsafeUtility.MemCmp((void*)(ptr + sizeof(T) * i), (void*)(ptr2 + sizeof(T) * i), (long)sizeof(T));
				if (num2 != 0)
				{
					return num2;
				}
			}
			return this.Length.CompareTo(other.Length);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00007F92 File Offset: 0x00006192
		public bool Equals(FixedList64Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00007FA0 File Offset: 0x000061A0
		public unsafe static bool operator ==(in FixedList64Bytes<T> a, in FixedList128Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList64Bytes<T> fixedList64Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList64Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00007FE0 File Offset: 0x000061E0
		public static bool operator !=(in FixedList64Bytes<T> a, in FixedList128Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00007FEC File Offset: 0x000061EC
		public unsafe int CompareTo(FixedList128Bytes<T> other)
		{
			byte* buffer = this.buffer;
			byte* buffer2 = other.buffer;
			byte* ptr = buffer + FixedList.PaddingBytes<T>();
			byte* ptr2 = buffer2 + FixedList.PaddingBytes<T>();
			int num = math.min(this.Length, other.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnsafeUtility.MemCmp((void*)(ptr + sizeof(T) * i), (void*)(ptr2 + sizeof(T) * i), (long)sizeof(T));
				if (num2 != 0)
				{
					return num2;
				}
			}
			return this.Length.CompareTo(other.Length);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000807A File Offset: 0x0000627A
		public bool Equals(FixedList128Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008086 File Offset: 0x00006286
		public FixedList64Bytes(in FixedList128Bytes<T> other)
		{
			this = default(FixedList64Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008097 File Offset: 0x00006297
		internal unsafe int Initialize(in FixedList128Bytes<T> other)
		{
			if (other.Length > this.Capacity)
			{
				return 1;
			}
			this.length = other.length;
			UnsafeUtility.MemCpy((void*)this.Buffer, (void*)other.Buffer, (long)this.LengthInBytes);
			return 0;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000080CE File Offset: 0x000062CE
		public static implicit operator FixedList64Bytes<T>(in FixedList128Bytes<T> other)
		{
			return new FixedList64Bytes<T>(in other);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000080D8 File Offset: 0x000062D8
		public unsafe static bool operator ==(in FixedList64Bytes<T> a, in FixedList512Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList64Bytes<T> fixedList64Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList64Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008118 File Offset: 0x00006318
		public static bool operator !=(in FixedList64Bytes<T> a, in FixedList512Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008124 File Offset: 0x00006324
		public unsafe int CompareTo(FixedList512Bytes<T> other)
		{
			byte* buffer = this.buffer;
			byte* buffer2 = other.buffer;
			byte* ptr = buffer + FixedList.PaddingBytes<T>();
			byte* ptr2 = buffer2 + FixedList.PaddingBytes<T>();
			int num = math.min(this.Length, other.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnsafeUtility.MemCmp((void*)(ptr + sizeof(T) * i), (void*)(ptr2 + sizeof(T) * i), (long)sizeof(T));
				if (num2 != 0)
				{
					return num2;
				}
			}
			return this.Length.CompareTo(other.Length);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000081B2 File Offset: 0x000063B2
		public bool Equals(FixedList512Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000081BE File Offset: 0x000063BE
		public FixedList64Bytes(in FixedList512Bytes<T> other)
		{
			this = default(FixedList64Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000081CF File Offset: 0x000063CF
		internal unsafe int Initialize(in FixedList512Bytes<T> other)
		{
			if (other.Length > this.Capacity)
			{
				return 1;
			}
			this.length = other.length;
			UnsafeUtility.MemCpy((void*)this.Buffer, (void*)other.Buffer, (long)this.LengthInBytes);
			return 0;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00008206 File Offset: 0x00006406
		public static implicit operator FixedList64Bytes<T>(in FixedList512Bytes<T> other)
		{
			return new FixedList64Bytes<T>(in other);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00008210 File Offset: 0x00006410
		public unsafe static bool operator ==(in FixedList64Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList64Bytes<T> fixedList64Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList64Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008250 File Offset: 0x00006450
		public static bool operator !=(in FixedList64Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000825C File Offset: 0x0000645C
		public unsafe int CompareTo(FixedList4096Bytes<T> other)
		{
			byte* buffer = this.buffer;
			byte* buffer2 = other.buffer;
			byte* ptr = buffer + FixedList.PaddingBytes<T>();
			byte* ptr2 = buffer2 + FixedList.PaddingBytes<T>();
			int num = math.min(this.Length, other.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnsafeUtility.MemCmp((void*)(ptr + sizeof(T) * i), (void*)(ptr2 + sizeof(T) * i), (long)sizeof(T));
				if (num2 != 0)
				{
					return num2;
				}
			}
			return this.Length.CompareTo(other.Length);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000082EA File Offset: 0x000064EA
		public bool Equals(FixedList4096Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000082F6 File Offset: 0x000064F6
		public FixedList64Bytes(in FixedList4096Bytes<T> other)
		{
			this = default(FixedList64Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00008307 File Offset: 0x00006507
		internal unsafe int Initialize(in FixedList4096Bytes<T> other)
		{
			if (other.Length > this.Capacity)
			{
				return 1;
			}
			this.length = other.length;
			UnsafeUtility.MemCpy((void*)this.Buffer, (void*)other.Buffer, (long)this.LengthInBytes);
			return 0;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000833E File Offset: 0x0000653E
		public static implicit operator FixedList64Bytes<T>(in FixedList4096Bytes<T> other)
		{
			return new FixedList64Bytes<T>(in other);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00008348 File Offset: 0x00006548
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			if (obj is FixedList32Bytes<T>)
			{
				FixedList32Bytes<T> fixedList32Bytes = (FixedList32Bytes<T>)obj;
				return this.Equals(fixedList32Bytes);
			}
			if (obj is FixedList64Bytes<T>)
			{
				FixedList64Bytes<T> fixedList64Bytes = (FixedList64Bytes<T>)obj;
				return this.Equals(fixedList64Bytes);
			}
			if (obj is FixedList128Bytes<T>)
			{
				FixedList128Bytes<T> fixedList128Bytes = (FixedList128Bytes<T>)obj;
				return this.Equals(fixedList128Bytes);
			}
			if (obj is FixedList512Bytes<T>)
			{
				FixedList512Bytes<T> fixedList512Bytes = (FixedList512Bytes<T>)obj;
				return this.Equals(fixedList512Bytes);
			}
			if (obj is FixedList4096Bytes<T>)
			{
				FixedList4096Bytes<T> fixedList4096Bytes = (FixedList4096Bytes<T>)obj;
				return this.Equals(fixedList4096Bytes);
			}
			return false;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000083CB File Offset: 0x000065CB
		public FixedList64Bytes<T>.Enumerator GetEnumerator()
		{
			return new FixedList64Bytes<T>.Enumerator(ref this);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		internal FixedBytes64Align8 data;

		// Token: 0x02000053 RID: 83
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x060002B2 RID: 690 RVA: 0x000083D3 File Offset: 0x000065D3
			public Enumerator(ref FixedList64Bytes<T> list)
			{
				this.m_List = list;
				this.m_Index = -1;
			}

			// Token: 0x060002B3 RID: 691 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060002B4 RID: 692 RVA: 0x000083E8 File Offset: 0x000065E8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Length;
			}

			// Token: 0x060002B5 RID: 693 RVA: 0x0000840B File Offset: 0x0000660B
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060002B6 RID: 694 RVA: 0x00008414 File Offset: 0x00006614
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060002B7 RID: 695 RVA: 0x00008427 File Offset: 0x00006627
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040000C7 RID: 199
			private FixedList64Bytes<T> m_List;

			// Token: 0x040000C8 RID: 200
			private int m_Index;
		}
	}
}
