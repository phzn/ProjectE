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
	// Token: 0x02000056 RID: 86
	[DebuggerTypeProxy(typeof(FixedList128BytesDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	[Serializable]
	public struct FixedList128Bytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable, IEquatable<FixedList32Bytes<T>>, IComparable<FixedList32Bytes<T>>, IEquatable<FixedList64Bytes<T>>, IComparable<FixedList64Bytes<T>>, IEquatable<FixedList128Bytes<T>>, IComparable<FixedList128Bytes<T>>, IEquatable<FixedList512Bytes<T>>, IComparable<FixedList512Bytes<T>>, IEquatable<FixedList4096Bytes<T>>, IComparable<FixedList4096Bytes<T>> where T : struct, ValueType
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000084BC File Offset: 0x000066BC
		// (set) Token: 0x060002BF RID: 703 RVA: 0x000084D8 File Offset: 0x000066D8
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (FixedBytes128Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (FixedBytes128Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000084F8 File Offset: 0x000066F8
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (FixedBytes128Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00008516 File Offset: 0x00006716
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000851E File Offset: 0x0000671E
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00008528 File Offset: 0x00006728
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00008530 File Offset: 0x00006730
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000853B File Offset: 0x0000673B
		internal int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00008549 File Offset: 0x00006749
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00008557 File Offset: 0x00006757
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<FixedBytes128Align8, T>();
			}
			set
			{
			}
		}

		// Token: 0x17000063 RID: 99
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

		// Token: 0x060002CB RID: 715 RVA: 0x00008585 File Offset: 0x00006785
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00008593 File Offset: 0x00006793
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000085A6 File Offset: 0x000067A6
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000085AF File Offset: 0x000067AF
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000085BC File Offset: 0x000067BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000085E8 File Offset: 0x000067E8
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00008628 File Offset: 0x00006828
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000866E File Offset: 0x0000686E
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00008678 File Offset: 0x00006878
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

		// Token: 0x060002D4 RID: 724 RVA: 0x000086D6 File Offset: 0x000068D6
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000086E2 File Offset: 0x000068E2
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000086FB File Offset: 0x000068FB
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00008708 File Offset: 0x00006908
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

		// Token: 0x060002D8 RID: 728 RVA: 0x00008766 File Offset: 0x00006966
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00008770 File Offset: 0x00006970
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

		// Token: 0x060002DA RID: 730 RVA: 0x000087CC File Offset: 0x000069CC
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

		// Token: 0x060002DB RID: 731 RVA: 0x00008813 File Offset: 0x00006A13
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000883C File Offset: 0x00006A3C
		public unsafe static bool operator ==(in FixedList128Bytes<T> a, in FixedList32Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList128Bytes<T> fixedList128Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList128Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000887C File Offset: 0x00006A7C
		public static bool operator !=(in FixedList128Bytes<T> a, in FixedList32Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00008888 File Offset: 0x00006A88
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

		// Token: 0x060002DF RID: 735 RVA: 0x00008916 File Offset: 0x00006B16
		public bool Equals(FixedList32Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00008922 File Offset: 0x00006B22
		public FixedList128Bytes(in FixedList32Bytes<T> other)
		{
			this = default(FixedList128Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00008933 File Offset: 0x00006B33
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

		// Token: 0x060002E2 RID: 738 RVA: 0x0000896A File Offset: 0x00006B6A
		public static implicit operator FixedList128Bytes<T>(in FixedList32Bytes<T> other)
		{
			return new FixedList128Bytes<T>(in other);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00008974 File Offset: 0x00006B74
		public unsafe static bool operator ==(in FixedList128Bytes<T> a, in FixedList64Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList128Bytes<T> fixedList128Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList128Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000089B4 File Offset: 0x00006BB4
		public static bool operator !=(in FixedList128Bytes<T> a, in FixedList64Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000089C0 File Offset: 0x00006BC0
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

		// Token: 0x060002E6 RID: 742 RVA: 0x00008A4E File Offset: 0x00006C4E
		public bool Equals(FixedList64Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00008A5A File Offset: 0x00006C5A
		public FixedList128Bytes(in FixedList64Bytes<T> other)
		{
			this = default(FixedList128Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00008A6B File Offset: 0x00006C6B
		internal unsafe int Initialize(in FixedList64Bytes<T> other)
		{
			if (other.Length > this.Capacity)
			{
				return 1;
			}
			this.length = other.length;
			UnsafeUtility.MemCpy((void*)this.Buffer, (void*)other.Buffer, (long)this.LengthInBytes);
			return 0;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00008AA2 File Offset: 0x00006CA2
		public static implicit operator FixedList128Bytes<T>(in FixedList64Bytes<T> other)
		{
			return new FixedList128Bytes<T>(in other);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00008AAC File Offset: 0x00006CAC
		public unsafe static bool operator ==(in FixedList128Bytes<T> a, in FixedList128Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList128Bytes<T> fixedList128Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList128Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00008AEC File Offset: 0x00006CEC
		public static bool operator !=(in FixedList128Bytes<T> a, in FixedList128Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00008AF8 File Offset: 0x00006CF8
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

		// Token: 0x060002ED RID: 749 RVA: 0x00008B86 File Offset: 0x00006D86
		public bool Equals(FixedList128Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00008B94 File Offset: 0x00006D94
		public unsafe static bool operator ==(in FixedList128Bytes<T> a, in FixedList512Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList128Bytes<T> fixedList128Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList128Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00008BD4 File Offset: 0x00006DD4
		public static bool operator !=(in FixedList128Bytes<T> a, in FixedList512Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008BE0 File Offset: 0x00006DE0
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

		// Token: 0x060002F1 RID: 753 RVA: 0x00008C6E File Offset: 0x00006E6E
		public bool Equals(FixedList512Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00008C7A File Offset: 0x00006E7A
		public FixedList128Bytes(in FixedList512Bytes<T> other)
		{
			this = default(FixedList128Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00008C8B File Offset: 0x00006E8B
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

		// Token: 0x060002F4 RID: 756 RVA: 0x00008CC2 File Offset: 0x00006EC2
		public static implicit operator FixedList128Bytes<T>(in FixedList512Bytes<T> other)
		{
			return new FixedList128Bytes<T>(in other);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00008CCC File Offset: 0x00006ECC
		public unsafe static bool operator ==(in FixedList128Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList128Bytes<T> fixedList128Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList128Bytes.LengthInBytes) == 0;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00008D0C File Offset: 0x00006F0C
		public static bool operator !=(in FixedList128Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00008D18 File Offset: 0x00006F18
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

		// Token: 0x060002F8 RID: 760 RVA: 0x00008DA6 File Offset: 0x00006FA6
		public bool Equals(FixedList4096Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00008DB2 File Offset: 0x00006FB2
		public FixedList128Bytes(in FixedList4096Bytes<T> other)
		{
			this = default(FixedList128Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00008DC3 File Offset: 0x00006FC3
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

		// Token: 0x060002FB RID: 763 RVA: 0x00008DFA File Offset: 0x00006FFA
		public static implicit operator FixedList128Bytes<T>(in FixedList4096Bytes<T> other)
		{
			return new FixedList128Bytes<T>(in other);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00008E04 File Offset: 0x00007004
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

		// Token: 0x060002FD RID: 765 RVA: 0x00008E87 File Offset: 0x00007087
		public FixedList128Bytes<T>.Enumerator GetEnumerator()
		{
			return new FixedList128Bytes<T>.Enumerator(ref this);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000CA RID: 202
		[SerializeField]
		internal FixedBytes128Align8 data;

		// Token: 0x02000057 RID: 87
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000300 RID: 768 RVA: 0x00008E8F File Offset: 0x0000708F
			public Enumerator(ref FixedList128Bytes<T> list)
			{
				this.m_List = list;
				this.m_Index = -1;
			}

			// Token: 0x06000301 RID: 769 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000302 RID: 770 RVA: 0x00008EA4 File Offset: 0x000070A4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Length;
			}

			// Token: 0x06000303 RID: 771 RVA: 0x00008EC7 File Offset: 0x000070C7
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000304 RID: 772 RVA: 0x00008ED0 File Offset: 0x000070D0
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000305 RID: 773 RVA: 0x00008EE3 File Offset: 0x000070E3
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040000CB RID: 203
			private FixedList128Bytes<T> m_List;

			// Token: 0x040000CC RID: 204
			private int m_Index;
		}
	}
}
