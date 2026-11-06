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
	// Token: 0x0200005E RID: 94
	[DebuggerTypeProxy(typeof(FixedList4096BytesDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	[Serializable]
	public struct FixedList4096Bytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable, IEquatable<FixedList32Bytes<T>>, IComparable<FixedList32Bytes<T>>, IEquatable<FixedList64Bytes<T>>, IComparable<FixedList64Bytes<T>>, IEquatable<FixedList128Bytes<T>>, IComparable<FixedList128Bytes<T>>, IEquatable<FixedList512Bytes<T>>, IComparable<FixedList512Bytes<T>>, IEquatable<FixedList4096Bytes<T>>, IComparable<FixedList4096Bytes<T>> where T : struct, ValueType
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00009A34 File Offset: 0x00007C34
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00009A50 File Offset: 0x00007C50
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (FixedBytes4096Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (FixedBytes4096Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00009A70 File Offset: 0x00007C70
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (FixedBytes4096Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00009A8E File Offset: 0x00007C8E
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00009A96 File Offset: 0x00007C96
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00009AA0 File Offset: 0x00007CA0
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00009AA8 File Offset: 0x00007CA8
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00009AB3 File Offset: 0x00007CB3
		internal int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00009AC1 File Offset: 0x00007CC1
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00009ACF File Offset: 0x00007CCF
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<FixedBytes4096Align8, T>();
			}
			set
			{
			}
		}

		// Token: 0x1700007B RID: 123
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

		// Token: 0x06000367 RID: 871 RVA: 0x00009AFD File Offset: 0x00007CFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00009B0B File Offset: 0x00007D0B
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00009B1E File Offset: 0x00007D1E
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00009B27 File Offset: 0x00007D27
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00009B34 File Offset: 0x00007D34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009B60 File Offset: 0x00007D60
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00009BA0 File Offset: 0x00007DA0
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

		// Token: 0x0600036E RID: 878 RVA: 0x00009BE6 File Offset: 0x00007DE6
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00009BF0 File Offset: 0x00007DF0
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

		// Token: 0x06000370 RID: 880 RVA: 0x00009C4E File Offset: 0x00007E4E
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00009C5A File Offset: 0x00007E5A
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00009C73 File Offset: 0x00007E73
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00009C80 File Offset: 0x00007E80
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

		// Token: 0x06000374 RID: 884 RVA: 0x00009CDE File Offset: 0x00007EDE
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00009CE8 File Offset: 0x00007EE8
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

		// Token: 0x06000376 RID: 886 RVA: 0x00009D44 File Offset: 0x00007F44
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

		// Token: 0x06000377 RID: 887 RVA: 0x00009D8B File Offset: 0x00007F8B
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00009DB4 File Offset: 0x00007FB4
		public unsafe static bool operator ==(in FixedList4096Bytes<T> a, in FixedList32Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList4096Bytes<T> fixedList4096Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList4096Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00009DF4 File Offset: 0x00007FF4
		public static bool operator !=(in FixedList4096Bytes<T> a, in FixedList32Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00009E00 File Offset: 0x00008000
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

		// Token: 0x0600037B RID: 891 RVA: 0x00009E8E File Offset: 0x0000808E
		public bool Equals(FixedList32Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00009E9A File Offset: 0x0000809A
		public FixedList4096Bytes(in FixedList32Bytes<T> other)
		{
			this = default(FixedList4096Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00009EAB File Offset: 0x000080AB
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

		// Token: 0x0600037E RID: 894 RVA: 0x00009EE2 File Offset: 0x000080E2
		public static implicit operator FixedList4096Bytes<T>(in FixedList32Bytes<T> other)
		{
			return new FixedList4096Bytes<T>(in other);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00009EEC File Offset: 0x000080EC
		public unsafe static bool operator ==(in FixedList4096Bytes<T> a, in FixedList64Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList4096Bytes<T> fixedList4096Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList4096Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00009F2C File Offset: 0x0000812C
		public static bool operator !=(in FixedList4096Bytes<T> a, in FixedList64Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00009F38 File Offset: 0x00008138
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

		// Token: 0x06000382 RID: 898 RVA: 0x00009FC6 File Offset: 0x000081C6
		public bool Equals(FixedList64Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00009FD2 File Offset: 0x000081D2
		public FixedList4096Bytes(in FixedList64Bytes<T> other)
		{
			this = default(FixedList4096Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00009FE3 File Offset: 0x000081E3
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

		// Token: 0x06000385 RID: 901 RVA: 0x0000A01A File Offset: 0x0000821A
		public static implicit operator FixedList4096Bytes<T>(in FixedList64Bytes<T> other)
		{
			return new FixedList4096Bytes<T>(in other);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000A024 File Offset: 0x00008224
		public unsafe static bool operator ==(in FixedList4096Bytes<T> a, in FixedList128Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList4096Bytes<T> fixedList4096Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList4096Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000A064 File Offset: 0x00008264
		public static bool operator !=(in FixedList4096Bytes<T> a, in FixedList128Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000A070 File Offset: 0x00008270
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

		// Token: 0x06000389 RID: 905 RVA: 0x0000A0FE File Offset: 0x000082FE
		public bool Equals(FixedList128Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000A10A File Offset: 0x0000830A
		public FixedList4096Bytes(in FixedList128Bytes<T> other)
		{
			this = default(FixedList4096Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000A11B File Offset: 0x0000831B
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

		// Token: 0x0600038C RID: 908 RVA: 0x0000A152 File Offset: 0x00008352
		public static implicit operator FixedList4096Bytes<T>(in FixedList128Bytes<T> other)
		{
			return new FixedList4096Bytes<T>(in other);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000A15C File Offset: 0x0000835C
		public unsafe static bool operator ==(in FixedList4096Bytes<T> a, in FixedList512Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList4096Bytes<T> fixedList4096Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList4096Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000A19C File Offset: 0x0000839C
		public static bool operator !=(in FixedList4096Bytes<T> a, in FixedList512Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000A1A8 File Offset: 0x000083A8
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

		// Token: 0x06000390 RID: 912 RVA: 0x0000A236 File Offset: 0x00008436
		public bool Equals(FixedList512Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000A242 File Offset: 0x00008442
		public FixedList4096Bytes(in FixedList512Bytes<T> other)
		{
			this = default(FixedList4096Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000A253 File Offset: 0x00008453
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

		// Token: 0x06000393 RID: 915 RVA: 0x0000A28A File Offset: 0x0000848A
		public static implicit operator FixedList4096Bytes<T>(in FixedList512Bytes<T> other)
		{
			return new FixedList4096Bytes<T>(in other);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000A294 File Offset: 0x00008494
		public unsafe static bool operator ==(in FixedList4096Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList4096Bytes<T> fixedList4096Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList4096Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000A2D4 File Offset: 0x000084D4
		public static bool operator !=(in FixedList4096Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000A2E0 File Offset: 0x000084E0
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

		// Token: 0x06000397 RID: 919 RVA: 0x0000A36E File Offset: 0x0000856E
		public bool Equals(FixedList4096Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000A37C File Offset: 0x0000857C
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

		// Token: 0x06000399 RID: 921 RVA: 0x0000A3FF File Offset: 0x000085FF
		public FixedList4096Bytes<T>.Enumerator GetEnumerator()
		{
			return new FixedList4096Bytes<T>.Enumerator(ref this);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		internal FixedBytes4096Align8 data;

		// Token: 0x0200005F RID: 95
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x0600039C RID: 924 RVA: 0x0000A407 File Offset: 0x00008607
			public Enumerator(ref FixedList4096Bytes<T> list)
			{
				this.m_List = list;
				this.m_Index = -1;
			}

			// Token: 0x0600039D RID: 925 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x0600039E RID: 926 RVA: 0x0000A41C File Offset: 0x0000861C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Length;
			}

			// Token: 0x0600039F RID: 927 RVA: 0x0000A43F File Offset: 0x0000863F
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000A448 File Offset: 0x00008648
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000A45B File Offset: 0x0000865B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040000D3 RID: 211
			private FixedList4096Bytes<T> m_List;

			// Token: 0x040000D4 RID: 212
			private int m_Index;
		}
	}
}
