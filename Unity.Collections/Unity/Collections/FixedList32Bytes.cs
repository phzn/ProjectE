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
	// Token: 0x0200004E RID: 78
	[DebuggerTypeProxy(typeof(FixedList32BytesDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	[Serializable]
	public struct FixedList32Bytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable, IEquatable<FixedList32Bytes<T>>, IComparable<FixedList32Bytes<T>>, IEquatable<FixedList64Bytes<T>>, IComparable<FixedList64Bytes<T>>, IEquatable<FixedList128Bytes<T>>, IComparable<FixedList128Bytes<T>>, IEquatable<FixedList512Bytes<T>>, IComparable<FixedList512Bytes<T>>, IEquatable<FixedList4096Bytes<T>>, IComparable<FixedList4096Bytes<T>> where T : struct, ValueType
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00006F3C File Offset: 0x0000513C
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00006F58 File Offset: 0x00005158
		internal unsafe ushort length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				fixed (FixedBytes32Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return *(ushort*)ptr2;
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				fixed (FixedBytes32Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					*(short*)ptr2 = (short)value;
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00006F78 File Offset: 0x00005178
		internal unsafe readonly byte* buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				fixed (FixedBytes32Align8* ptr = &this.data)
				{
					void* ptr2 = (void*)ptr;
					return (byte*)ptr2 + UnsafeUtility.SizeOf<ushort>();
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00006F96 File Offset: 0x00005196
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00006F9E File Offset: 0x0000519E
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00006FA8 File Offset: 0x000051A8
		[CreateProperty]
		private IEnumerable<T> Elements
		{
			get
			{
				return this.ToArray();
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00006FB0 File Offset: 0x000051B0
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00006FBB File Offset: 0x000051BB
		internal int LengthInBytes
		{
			get
			{
				return this.Length * UnsafeUtility.SizeOf<T>();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00006FC9 File Offset: 0x000051C9
		internal unsafe readonly byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.buffer + FixedList.PaddingBytes<T>();
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00006FD7 File Offset: 0x000051D7
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return FixedList.Capacity<FixedBytes32Align8, T>();
			}
			set
			{
			}
		}

		// Token: 0x1700004B RID: 75
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

		// Token: 0x0600022F RID: 559 RVA: 0x00007005 File Offset: 0x00005205
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<T>((void*)this.Buffer, index);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00007013 File Offset: 0x00005213
		public unsafe override int GetHashCode()
		{
			return (int)CollectionHelper.Hash((void*)this.Buffer, this.LengthInBytes);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007026 File Offset: 0x00005226
		public void Add(in T item)
		{
			this.AddNoResize(in item);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000702F File Offset: 0x0000522F
		public unsafe void AddRange(void* ptr, int length)
		{
			this.AddRangeNoResize(ptr, length);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000703C File Offset: 0x0000523C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNoResize(in T item)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = item;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00007068 File Offset: 0x00005268
		public unsafe void AddRangeNoResize(void* ptr, int length)
		{
			int length2 = this.Length;
			this.Length += length;
			UnsafeUtility.MemCpy((void*)(this.Buffer + (IntPtr)length2 * (IntPtr)sizeof(T)), ptr, (long)(UnsafeUtility.SizeOf<T>() * length));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000070A8 File Offset: 0x000052A8
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

		// Token: 0x06000236 RID: 566 RVA: 0x000070EE File Offset: 0x000052EE
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000070F8 File Offset: 0x000052F8
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

		// Token: 0x06000238 RID: 568 RVA: 0x00007156 File Offset: 0x00005356
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00007162 File Offset: 0x00005362
		public void Insert(int index, in T item)
		{
			this.InsertRangeWithBeginEnd(index, index + 1);
			this[index] = item;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000717B File Offset: 0x0000537B
		public void RemoveAtSwapBack(int index)
		{
			this.RemoveRangeSwapBack(index, 1);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00007188 File Offset: 0x00005388
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

		// Token: 0x0600023C RID: 572 RVA: 0x000071E6 File Offset: 0x000053E6
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000071F0 File Offset: 0x000053F0
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

		// Token: 0x0600023E RID: 574 RVA: 0x0000724C File Offset: 0x0000544C
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

		// Token: 0x0600023F RID: 575 RVA: 0x00007293 File Offset: 0x00005493
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<T>(), (void*)this.Buffer, (long)this.LengthInBytes);
			return nativeArray;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000072BC File Offset: 0x000054BC
		public unsafe static bool operator ==(in FixedList32Bytes<T> a, in FixedList32Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList32Bytes<T> fixedList32Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList32Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000072FC File Offset: 0x000054FC
		public static bool operator !=(in FixedList32Bytes<T> a, in FixedList32Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00007308 File Offset: 0x00005508
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

		// Token: 0x06000243 RID: 579 RVA: 0x00007396 File Offset: 0x00005596
		public bool Equals(FixedList32Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000073A4 File Offset: 0x000055A4
		public unsafe static bool operator ==(in FixedList32Bytes<T> a, in FixedList64Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList32Bytes<T> fixedList32Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList32Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000073E4 File Offset: 0x000055E4
		public static bool operator !=(in FixedList32Bytes<T> a, in FixedList64Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000073F0 File Offset: 0x000055F0
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

		// Token: 0x06000247 RID: 583 RVA: 0x0000747E File Offset: 0x0000567E
		public bool Equals(FixedList64Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000748A File Offset: 0x0000568A
		public FixedList32Bytes(in FixedList64Bytes<T> other)
		{
			this = default(FixedList32Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000749B File Offset: 0x0000569B
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

		// Token: 0x0600024A RID: 586 RVA: 0x000074D2 File Offset: 0x000056D2
		public static implicit operator FixedList32Bytes<T>(in FixedList64Bytes<T> other)
		{
			return new FixedList32Bytes<T>(in other);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000074DC File Offset: 0x000056DC
		public unsafe static bool operator ==(in FixedList32Bytes<T> a, in FixedList128Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList32Bytes<T> fixedList32Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList32Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000751C File Offset: 0x0000571C
		public static bool operator !=(in FixedList32Bytes<T> a, in FixedList128Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00007528 File Offset: 0x00005728
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

		// Token: 0x0600024E RID: 590 RVA: 0x000075B6 File Offset: 0x000057B6
		public bool Equals(FixedList128Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000075C2 File Offset: 0x000057C2
		public FixedList32Bytes(in FixedList128Bytes<T> other)
		{
			this = default(FixedList32Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000075D3 File Offset: 0x000057D3
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

		// Token: 0x06000251 RID: 593 RVA: 0x0000760A File Offset: 0x0000580A
		public static implicit operator FixedList32Bytes<T>(in FixedList128Bytes<T> other)
		{
			return new FixedList32Bytes<T>(in other);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00007614 File Offset: 0x00005814
		public unsafe static bool operator ==(in FixedList32Bytes<T> a, in FixedList512Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList32Bytes<T> fixedList32Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList32Bytes.LengthInBytes) == 0;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00007654 File Offset: 0x00005854
		public static bool operator !=(in FixedList32Bytes<T> a, in FixedList512Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00007660 File Offset: 0x00005860
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

		// Token: 0x06000255 RID: 597 RVA: 0x000076EE File Offset: 0x000058EE
		public bool Equals(FixedList512Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000076FA File Offset: 0x000058FA
		public FixedList32Bytes(in FixedList512Bytes<T> other)
		{
			this = default(FixedList32Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000770B File Offset: 0x0000590B
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

		// Token: 0x06000258 RID: 600 RVA: 0x00007742 File Offset: 0x00005942
		public static implicit operator FixedList32Bytes<T>(in FixedList512Bytes<T> other)
		{
			return new FixedList32Bytes<T>(in other);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000774C File Offset: 0x0000594C
		public unsafe static bool operator ==(in FixedList32Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			if (a.length != b.length)
			{
				return false;
			}
			void* buffer = (void*)a.Buffer;
			void* buffer2 = (void*)b.Buffer;
			FixedList32Bytes<T> fixedList32Bytes = a;
			return UnsafeUtility.MemCmp(buffer, buffer2, (long)fixedList32Bytes.LengthInBytes) == 0;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000778C File Offset: 0x0000598C
		public static bool operator !=(in FixedList32Bytes<T> a, in FixedList4096Bytes<T> b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00007798 File Offset: 0x00005998
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

		// Token: 0x0600025C RID: 604 RVA: 0x00007826 File Offset: 0x00005A26
		public bool Equals(FixedList4096Bytes<T> other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00007832 File Offset: 0x00005A32
		public FixedList32Bytes(in FixedList4096Bytes<T> other)
		{
			this = default(FixedList32Bytes<T>);
			this.Initialize(in other);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00007843 File Offset: 0x00005A43
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

		// Token: 0x0600025F RID: 607 RVA: 0x0000787A File Offset: 0x00005A7A
		public static implicit operator FixedList32Bytes<T>(in FixedList4096Bytes<T> other)
		{
			return new FixedList32Bytes<T>(in other);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00007884 File Offset: 0x00005A84
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

		// Token: 0x06000261 RID: 609 RVA: 0x00007907 File Offset: 0x00005B07
		public FixedList32Bytes<T>.Enumerator GetEnumerator()
		{
			return new FixedList32Bytes<T>.Enumerator(ref this);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		internal FixedBytes32Align8 data;

		// Token: 0x0200004F RID: 79
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000264 RID: 612 RVA: 0x00007916 File Offset: 0x00005B16
			public Enumerator(ref FixedList32Bytes<T> list)
			{
				this.m_List = list;
				this.m_Index = -1;
			}

			// Token: 0x06000265 RID: 613 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000266 RID: 614 RVA: 0x0000792B File Offset: 0x00005B2B
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Length;
			}

			// Token: 0x06000267 RID: 615 RVA: 0x0000794E File Offset: 0x00005B4E
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000268 RID: 616 RVA: 0x00007957 File Offset: 0x00005B57
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000269 RID: 617 RVA: 0x0000796A File Offset: 0x00005B6A
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040000C3 RID: 195
			private FixedList32Bytes<T> m_List;

			// Token: 0x040000C4 RID: 196
			private int m_Index;
		}
	}
}
