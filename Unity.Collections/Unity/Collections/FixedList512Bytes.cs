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
	// Token: 0x0200005A RID: 90
	[DebuggerTypeProxy(typeof(FixedList512BytesDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	[Serializable]
	public struct FixedList512Bytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable, IEquatable<FixedList32Bytes<T>>, IComparable<FixedList32Bytes<T>>, IEquatable<FixedList64Bytes<T>>, IComparable<FixedList64Bytes<T>>, IEquatable<FixedList128Bytes<T>>, IComparable<FixedList128Bytes<T>>, IEquatable<FixedList512Bytes<T>>, IComparable<FixedList512Bytes<T>>, IEquatable<FixedList4096Bytes<T>>, IComparable<FixedList4096Bytes<T>> where T : struct, ValueType
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00008F78 File Offset: 0x00007178
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00008F94 File Offset: 0x00007194
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (FixedBytes512Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (FixedBytes512Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00008FB4 File Offset: 0x000071B4
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (FixedBytes512Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00008FD2 File Offset: 0x000071D2
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00008FDA File Offset: 0x000071DA
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00008FE4 File Offset: 0x000071E4
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00008FEC File Offset: 0x000071EC
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00008FF7 File Offset: 0x000071F7
		internal int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00009005 File Offset: 0x00007205
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00009013 File Offset: 0x00007213
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<FixedBytes512Align8, T>();
			}
			set
			{
			}
		}

		// Token: 0x1700006F RID: 111
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

		// Token: 0x06000319 RID: 793 RVA: 0x00009041 File Offset: 0x00007241
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000904F File Offset: 0x0000724F
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00009062 File Offset: 0x00007262
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000906B File Offset: 0x0000726B
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00009078 File Offset: 0x00007278
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000090A4 File Offset: 0x000072A4
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000090E4 File Offset: 0x000072E4
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

		// Token: 0x06000320 RID: 800 RVA: 0x0000912A File Offset: 0x0000732A
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00009134 File Offset: 0x00007334
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

		// Token: 0x06000322 RID: 802 RVA: 0x00009192 File Offset: 0x00007392
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000919E File Offset: 0x0000739E
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000091B7 File Offset: 0x000073B7
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000091C4 File Offset: 0x000073C4
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

		// Token: 0x06000326 RID: 806 RVA: 0x00009222 File Offset: 0x00007422
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000922C File Offset: 0x0000742C
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

		// Token: 0x06000328 RID: 808 RVA: 0x00009288 File Offset: 0x00007488
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

		// Token: 0x06000329 RID: 809 RVA: 0x000092CF File Offset: 0x000074CF
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000092F8 File Offset: 0x000074F8
		public unsafe static bool operator ==(in FixedList512Bytes<T> a, in FixedList32Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList512Bytes<T> fixedList512Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList512Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00009338 File Offset: 0x00007538
		public static bool operator !=(in FixedList512Bytes<T> a, in FixedList32Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00009344 File Offset: 0x00007544
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

		// Token: 0x0600032D RID: 813 RVA: 0x000093D2 File Offset: 0x000075D2
		public bool Equals(FixedList32Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000093DE File Offset: 0x000075DE
		public FixedList512Bytes(in FixedList32Bytes<T> other)
		{
			this = default(FixedList512Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000093EF File Offset: 0x000075EF
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

		// Token: 0x06000330 RID: 816 RVA: 0x00009426 File Offset: 0x00007626
		public static implicit operator FixedList512Bytes<T>(in FixedList32Bytes<T> other)
		{
			return new FixedList512Bytes<T>(in other);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00009430 File Offset: 0x00007630
		public unsafe static bool operator ==(in FixedList512Bytes<T> a, in FixedList64Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList512Bytes<T> fixedList512Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList512Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00009470 File Offset: 0x00007670
		public static bool operator !=(in FixedList512Bytes<T> a, in FixedList64Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000947C File Offset: 0x0000767C
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

		// Token: 0x06000334 RID: 820 RVA: 0x0000950A File Offset: 0x0000770A
		public bool Equals(FixedList64Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00009516 File Offset: 0x00007716
		public FixedList512Bytes(in FixedList64Bytes<T> other)
		{
			this = default(FixedList512Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00009527 File Offset: 0x00007727
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

		// Token: 0x06000337 RID: 823 RVA: 0x0000955E File Offset: 0x0000775E
		public static implicit operator FixedList512Bytes<T>(in FixedList64Bytes<T> other)
		{
			return new FixedList512Bytes<T>(in other);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00009568 File Offset: 0x00007768
		public unsafe static bool operator ==(in FixedList512Bytes<T> a, in FixedList128Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList512Bytes<T> fixedList512Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList512Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000095A8 File Offset: 0x000077A8
		public static bool operator !=(in FixedList512Bytes<T> a, in FixedList128Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000095B4 File Offset: 0x000077B4
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

		// Token: 0x0600033B RID: 827 RVA: 0x00009642 File Offset: 0x00007842
		public bool Equals(FixedList128Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000964E File Offset: 0x0000784E
		public FixedList512Bytes(in FixedList128Bytes<T> other)
		{
			this = default(FixedList512Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000965F File Offset: 0x0000785F
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

		// Token: 0x0600033E RID: 830 RVA: 0x00009696 File Offset: 0x00007896
		public static implicit operator FixedList512Bytes<T>(in FixedList128Bytes<T> other)
		{
			return new FixedList512Bytes<T>(in other);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000096A0 File Offset: 0x000078A0
		public unsafe static bool operator ==(in FixedList512Bytes<T> a, in FixedList512Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList512Bytes<T> fixedList512Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList512Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000096E0 File Offset: 0x000078E0
		public static bool operator !=(in FixedList512Bytes<T> a, in FixedList512Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000096EC File Offset: 0x000078EC
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

		// Token: 0x06000342 RID: 834 RVA: 0x0000977A File Offset: 0x0000797A
		public bool Equals(FixedList512Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00009788 File Offset: 0x00007988
		public unsafe static bool operator ==(in FixedList512Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList512Bytes<T> fixedList512Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList512Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000097C8 File Offset: 0x000079C8
		public static bool operator !=(in FixedList512Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000097D4 File Offset: 0x000079D4
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

		// Token: 0x06000346 RID: 838 RVA: 0x00009862 File Offset: 0x00007A62
		public bool Equals(FixedList4096Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000986E File Offset: 0x00007A6E
		public FixedList512Bytes(in FixedList4096Bytes<T> other)
		{
			this = default(FixedList512Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000987F File Offset: 0x00007A7F
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

		// Token: 0x06000349 RID: 841 RVA: 0x000098B6 File Offset: 0x00007AB6
		public static implicit operator FixedList512Bytes<T>(in FixedList4096Bytes<T> other)
		{
			return new FixedList512Bytes<T>(in other);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000098C0 File Offset: 0x00007AC0
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

		// Token: 0x0600034B RID: 843 RVA: 0x00009943 File Offset: 0x00007B43
		public FixedList512Bytes<T>.Enumerator GetEnumerator()
		{
			return new FixedList512Bytes<T>.Enumerator(ref this);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000CE RID: 206
		[SerializeField]
		internal FixedBytes512Align8 data;

		// Token: 0x0200005B RID: 91
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x0600034E RID: 846 RVA: 0x0000994B File Offset: 0x00007B4B
			public Enumerator(ref FixedList512Bytes<T> list)
			{
				this.m_List = list;
				this.m_Index = -1;
			}

			// Token: 0x0600034F RID: 847 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000350 RID: 848 RVA: 0x00009960 File Offset: 0x00007B60
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Length;
			}

			// Token: 0x06000351 RID: 849 RVA: 0x00009983 File Offset: 0x00007B83
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000352 RID: 850 RVA: 0x0000998C File Offset: 0x00007B8C
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x06000353 RID: 851 RVA: 0x0000999F File Offset: 0x00007B9F
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040000CF RID: 207
			private FixedList512Bytes<T> m_List;

			// Token: 0x040000D0 RID: 208
			private int m_Index;
		}
	}
}
