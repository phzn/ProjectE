using System;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000106 RID: 262
	[DebuggerDisplay("Length = {Length}, IsCreated = {IsCreated}")]
	[DebuggerTypeProxy(typeof(UnsafeBitArrayDebugView))]
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeBitArray : INativeDisposable, IDisposable
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x00021A88 File Offset: 0x0001FC88
		public unsafe UnsafeBitArray(void* ptr, int sizeInBytes, AllocatorManager.AllocatorHandle allocator = default(AllocatorManager.AllocatorHandle))
		{
			this.Ptr = (ulong*)ptr;
			this.Length = sizeInBytes * 8;
			this.Capacity = sizeInBytes * 8;
			this.Allocator = allocator;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00021AAA File Offset: 0x0001FCAA
		public UnsafeBitArray(int numBits, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			this.Allocator = allocator;
			this.Ptr = null;
			this.Length = 0;
			this.Capacity = 0;
			this.Resize(numBits, options);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00021AD4 File Offset: 0x0001FCD4
		internal unsafe static UnsafeBitArray* Alloc(AllocatorManager.AllocatorHandle allocator)
		{
			return (UnsafeBitArray*)Memory.Unmanaged.Allocate((long)sizeof(UnsafeBitArray), UnsafeUtility.AlignOf<UnsafeBitArray>(), allocator);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00021AF5 File Offset: 0x0001FCF5
		internal unsafe static void Free(UnsafeBitArray* data, AllocatorManager.AllocatorHandle allocator)
		{
			if (data == null)
			{
				throw new InvalidOperationException("UnsafeBitArray has yet to be created or has been destroyed!");
			}
			data->Dispose();
			Memory.Unmanaged.Free<UnsafeBitArray>(data, allocator);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00021B14 File Offset: 0x0001FD14
		public readonly bool IsCreated
		{
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00021B23 File Offset: 0x0001FD23
		public readonly bool IsEmpty
		{
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00021B38 File Offset: 0x0001FD38
		private unsafe void Realloc(int capacityInBits)
		{
			int num = Bitwise.AlignUp(capacityInBits, 64);
			int num2 = num / 8;
			ulong* ptr = null;
			if (num2 > 0)
			{
				ptr = (ulong*)Memory.Unmanaged.Allocate((long)num2, 16, this.Allocator);
				if (this.Capacity > 0)
				{
					int num3 = math.min(num, this.Capacity) / 8;
					UnsafeUtility.MemCpy((void*)ptr, (void*)this.Ptr, (long)num3);
				}
			}
			Memory.Unmanaged.Free<ulong>(this.Ptr, this.Allocator);
			this.Ptr = ptr;
			this.Capacity = num;
			this.Length = math.min(this.Length, num);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		public void Resize(int numBits, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			int num = math.max(numBits, 1);
			if (num > this.Capacity)
			{
				this.SetCapacity(num);
			}
			int length = this.Length;
			this.Length = numBits;
			if (options == NativeArrayOptions.ClearMemory && length < this.Length)
			{
				this.SetBits(length, false, this.Length - length);
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00021C10 File Offset: 0x0001FE10
		public void SetCapacity(int capacityInBits)
		{
			if (this.Capacity == capacityInBits)
			{
				return;
			}
			this.Realloc(capacityInBits);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00021C23 File Offset: 0x0001FE23
		public void TrimExcess()
		{
			this.SetCapacity(this.Length);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00021C34 File Offset: 0x0001FE34
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				Memory.Unmanaged.Free<ulong>(this.Ptr, this.Allocator);
				this.Allocator = AllocatorManager.Invalid;
			}
			this.Ptr = null;
			this.Length = 0;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00021C84 File Offset: 0x0001FE84
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				JobHandle jobHandle = new UnsafeDisposeJob
				{
					Ptr = (void*)this.Ptr,
					Allocator = this.Allocator
				}.Schedule(inputDeps);
				this.Ptr = null;
				this.Allocator = AllocatorManager.Invalid;
				return jobHandle;
			}
			this.Ptr = null;
			return inputDeps;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00021CF0 File Offset: 0x0001FEF0
		public unsafe void Clear()
		{
			int num = Bitwise.AlignUp(this.Length, 64) / 8;
			UnsafeUtility.MemClear((void*)this.Ptr, (long)num);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00021D1C File Offset: 0x0001FF1C
		public unsafe static void Set(ulong* ptr, int pos, bool value)
		{
			int num = pos >> 6;
			int num2 = pos & 63;
			ulong num3 = 1UL << num2;
			ulong num4 = (ptr[num] & ~num3) | (ulong)((long)(-(long)Bitwise.FromBool(value)) & (long)num3);
			ptr[num] = num4;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00021D58 File Offset: 0x0001FF58
		public void Set(int pos, bool value)
		{
			UnsafeBitArray.Set(this.Ptr, pos, value);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00021D68 File Offset: 0x0001FF68
		public unsafe void SetBits(int pos, bool value, int numBits)
		{
			int num = math.min(pos + numBits, this.Length);
			int num2 = pos >> 6;
			int num3 = pos & 63;
			int num4 = num - 1 >> 6;
			int num5 = num & 63;
			ulong num6 = ulong.MaxValue << num3;
			ulong num7 = ulong.MaxValue >> 64 - num5;
			ulong num8 = (ulong)((long)(-(long)Bitwise.FromBool(value)));
			ulong num9 = num6 & num8;
			ulong num10 = num7 & num8;
			ulong num11 = ~num6;
			ulong num12 = ~num7;
			if (num2 == num4)
			{
				ulong num13 = ~(num6 & num7);
				ulong num14 = num9 & num10;
				this.Ptr[num2] = (this.Ptr[num2] & num13) | num14;
				return;
			}
			this.Ptr[num2] = (this.Ptr[num2] & num11) | num9;
			for (int i = num2 + 1; i < num4; i++)
			{
				this.Ptr[i] = num8;
			}
			this.Ptr[num4] = (this.Ptr[num4] & num12) | num10;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00021E5C File Offset: 0x0002005C
		public unsafe void SetBits(int pos, ulong value, int numBits = 1)
		{
			int num = pos >> 6;
			int num2 = pos & 63;
			if (num2 + numBits <= 64)
			{
				ulong num3 = ulong.MaxValue >> 64 - numBits;
				this.Ptr[num] = Bitwise.ReplaceBits(this.Ptr[num], num2, num3, value);
				return;
			}
			int num4 = math.min(pos + numBits, this.Length);
			int num5 = num4 - 1 >> 6;
			int num6 = num4 & 63;
			ulong num7 = ulong.MaxValue >> num2;
			this.Ptr[num] = Bitwise.ReplaceBits(this.Ptr[num], num2, num7, value);
			ulong num8 = value >> 64 - num2;
			ulong num9 = ulong.MaxValue >> 64 - num6;
			this.Ptr[num5] = Bitwise.ReplaceBits(this.Ptr[num5], 0, num9, num8);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00021F22 File Offset: 0x00020122
		public ulong GetBits(int pos, int numBits = 1)
		{
			return Bitwise.GetBits(this.Ptr, this.Length, pos, numBits);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00021F37 File Offset: 0x00020137
		public bool IsSet(int pos)
		{
			return Bitwise.IsSet(this.Ptr, pos);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00021F45 File Offset: 0x00020145
		internal void CopyUlong(int dstPos, ref UnsafeBitArray srcBitArray, int srcPos, int numBits)
		{
			this.SetBits(dstPos, srcBitArray.GetBits(srcPos, numBits), numBits);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00021F59 File Offset: 0x00020159
		public void Copy(int dstPos, int srcPos, int numBits)
		{
			if (dstPos == srcPos)
			{
				return;
			}
			this.Copy(dstPos, ref this, srcPos, numBits);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00021F6C File Offset: 0x0002016C
		public unsafe void Copy(int dstPos, ref UnsafeBitArray srcBitArray, int srcPos, int numBits)
		{
			if (numBits == 0)
			{
				return;
			}
			if (numBits <= 64)
			{
				this.CopyUlong(dstPos, ref srcBitArray, srcPos, numBits);
				return;
			}
			if (numBits <= 128)
			{
				this.CopyUlong(dstPos, ref srcBitArray, srcPos, 64);
				numBits -= 64;
				if (numBits > 0)
				{
					this.CopyUlong(dstPos + 64, ref srcBitArray, srcPos + 64, numBits);
					return;
				}
			}
			else if ((dstPos & 7) == (srcPos & 7))
			{
				int num = CollectionHelper.Align(dstPos, 8) >> 3;
				int num2 = CollectionHelper.Align(srcPos, 8) >> 3;
				int num3 = num * 8 - dstPos;
				if (num3 > 0)
				{
					this.CopyUlong(dstPos, ref srcBitArray, srcPos, num3);
				}
				int num4 = numBits - num3;
				int num5 = num4 / 8;
				if (num5 > 0)
				{
					UnsafeUtility.MemMove((void*)(this.Ptr + num / 8), (void*)(srcBitArray.Ptr + num2 / 8), (long)num5);
				}
				int num6 = num4 & 7;
				if (num6 > 0)
				{
					this.CopyUlong((num + num5) * 8, ref srcBitArray, (num2 + num5) * 8, num6);
					return;
				}
			}
			else
			{
				int num7 = CollectionHelper.Align(dstPos, 64) - dstPos;
				if (num7 > 0)
				{
					this.CopyUlong(dstPos, ref srcBitArray, srcPos, num7);
					numBits -= num7;
					dstPos += num7;
					srcPos += num7;
				}
				while (numBits >= 64)
				{
					this.Ptr[dstPos >> 6] = srcBitArray.GetBits(srcPos, 64);
					numBits -= 64;
					dstPos += 64;
					srcPos += 64;
				}
				if (numBits > 0)
				{
					this.CopyUlong(dstPos, ref srcBitArray, srcPos, numBits);
				}
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x000220AC File Offset: 0x000202AC
		public int Find(int pos, int numBits)
		{
			int num = this.Length - pos;
			return Bitwise.Find(this.Ptr, pos, num, numBits);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000220D0 File Offset: 0x000202D0
		public int Find(int pos, int count, int numBits)
		{
			return Bitwise.Find(this.Ptr, pos, count, numBits);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000220E0 File Offset: 0x000202E0
		public bool TestNone(int pos, int numBits = 1)
		{
			return Bitwise.TestNone(this.Ptr, this.Length, pos, numBits);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000220F5 File Offset: 0x000202F5
		public bool TestAny(int pos, int numBits = 1)
		{
			return Bitwise.TestAny(this.Ptr, this.Length, pos, numBits);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002210A File Offset: 0x0002030A
		public bool TestAll(int pos, int numBits = 1)
		{
			return Bitwise.TestAll(this.Ptr, this.Length, pos, numBits);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002211F File Offset: 0x0002031F
		public int CountBits(int pos, int numBits = 1)
		{
			return Bitwise.CountBits(this.Ptr, this.Length, pos, numBits);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00022134 File Offset: 0x00020334
		public UnsafeBitArray.ReadOnly AsReadOnly()
		{
			return new UnsafeBitArray.ReadOnly(this.Ptr, this.Length);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00022147 File Offset: 0x00020347
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckSizeMultipleOf8(int sizeInBytes)
		{
			if ((sizeInBytes & 7) != 0)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: sizeInBytes {0} (must be multiple of 8-bytes, sizeInBytes: {1}).", sizeInBytes, sizeInBytes));
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002216A File Offset: 0x0002036A
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckArgs(int pos, int numBits)
		{
			if (pos < 0 || pos >= this.Length || numBits < 1)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: pos {0} (must be 0-{1}), numBits {2} (must be greater than 0).", pos, this.Length - 1, numBits));
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000221A8 File Offset: 0x000203A8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckArgsPosCount(int begin, int count, int numBits)
		{
			if (begin < 0 || begin >= this.Length)
			{
				throw new ArgumentException(string.Format("BitArray invalid argument: begin {0} (must be 0-{1}).", begin, this.Length - 1));
			}
			if (count < 0 || count > this.Length)
			{
				throw new ArgumentException(string.Format("BitArray invalid argument: count {0} (must be 0-{1}).", count, this.Length));
			}
			if (numBits < 1 || count < numBits)
			{
				throw new ArgumentException(string.Format("BitArray invalid argument: numBits {0} (must be greater than 0).", numBits));
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00022234 File Offset: 0x00020434
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckArgsUlong(int pos, int numBits)
		{
			if (numBits < 1 || numBits > 64)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: numBits {0} (must be 1-64).", numBits));
			}
			if (pos + numBits > this.Length)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: Out of bounds pos {0}, numBits {1}, Length {2}.", pos, numBits, this.Length));
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00022294 File Offset: 0x00020494
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckArgsCopy(ref UnsafeBitArray dstBitArray, int dstPos, ref UnsafeBitArray srcBitArray, int srcPos, int numBits)
		{
			if (srcPos + numBits > srcBitArray.Length)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: Out of bounds - source position {0}, numBits {1}, source bit array Length {2}.", srcPos, numBits, srcBitArray.Length));
			}
			if (dstPos + numBits > dstBitArray.Length)
			{
				throw new ArgumentException(string.Format("BitArray invalid arguments: Out of bounds - destination position {0}, numBits {1}, destination bit array Length {2}.", dstPos, numBits, dstBitArray.Length));
			}
		}

		// Token: 0x040004A6 RID: 1190
		[NativeDisableUnsafePtrRestriction]
		public unsafe ulong* Ptr;

		// Token: 0x040004A7 RID: 1191
		public int Length;

		// Token: 0x040004A8 RID: 1192
		public int Capacity;

		// Token: 0x040004A9 RID: 1193
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x02000107 RID: 263
		public struct ReadOnly
		{
			// Token: 0x1700013E RID: 318
			// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00022309 File Offset: 0x00020509
			public readonly bool IsCreated
			{
				get
				{
					return this.Ptr != null;
				}
			}

			// Token: 0x1700013F RID: 319
			// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00022318 File Offset: 0x00020518
			public readonly bool IsEmpty
			{
				get
				{
					return !this.IsCreated || this.Length == 0;
				}
			}

			// Token: 0x06000B3A RID: 2874 RVA: 0x0002232D File Offset: 0x0002052D
			internal unsafe ReadOnly(ulong* ptr, int length)
			{
				this.Ptr = ptr;
				this.Length = length;
			}

			// Token: 0x06000B3B RID: 2875 RVA: 0x0002233D File Offset: 0x0002053D
			public readonly ulong GetBits(int pos, int numBits = 1)
			{
				return Bitwise.GetBits(this.Ptr, this.Length, pos, numBits);
			}

			// Token: 0x06000B3C RID: 2876 RVA: 0x00022352 File Offset: 0x00020552
			public readonly bool IsSet(int pos)
			{
				return Bitwise.IsSet(this.Ptr, pos);
			}

			// Token: 0x06000B3D RID: 2877 RVA: 0x00022360 File Offset: 0x00020560
			public readonly int Find(int pos, int numBits)
			{
				int num = this.Length - pos;
				return Bitwise.Find(this.Ptr, pos, num, numBits);
			}

			// Token: 0x06000B3E RID: 2878 RVA: 0x00022384 File Offset: 0x00020584
			public readonly int Find(int pos, int count, int numBits)
			{
				return Bitwise.Find(this.Ptr, pos, count, numBits);
			}

			// Token: 0x06000B3F RID: 2879 RVA: 0x00022394 File Offset: 0x00020594
			public readonly bool TestNone(int pos, int numBits = 1)
			{
				return Bitwise.TestNone(this.Ptr, this.Length, pos, numBits);
			}

			// Token: 0x06000B40 RID: 2880 RVA: 0x000223A9 File Offset: 0x000205A9
			public readonly bool TestAny(int pos, int numBits = 1)
			{
				return Bitwise.TestAny(this.Ptr, this.Length, pos, numBits);
			}

			// Token: 0x06000B41 RID: 2881 RVA: 0x000223BE File Offset: 0x000205BE
			public readonly bool TestAll(int pos, int numBits = 1)
			{
				return Bitwise.TestAll(this.Ptr, this.Length, pos, numBits);
			}

			// Token: 0x06000B42 RID: 2882 RVA: 0x000223D3 File Offset: 0x000205D3
			public readonly int CountBits(int pos, int numBits = 1)
			{
				return Bitwise.CountBits(this.Ptr, this.Length, pos, numBits);
			}

			// Token: 0x06000B43 RID: 2883 RVA: 0x000223E8 File Offset: 0x000205E8
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void CheckArgs(int pos, int numBits)
			{
				if (pos < 0 || pos >= this.Length || numBits < 1)
				{
					throw new ArgumentException(string.Format("BitArray invalid arguments: pos {0} (must be 0-{1}), numBits {2} (must be greater than 0).", pos, this.Length - 1, numBits));
				}
			}

			// Token: 0x06000B44 RID: 2884 RVA: 0x00022424 File Offset: 0x00020624
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void CheckArgsPosCount(int begin, int count, int numBits)
			{
				if (begin < 0 || begin >= this.Length)
				{
					throw new ArgumentException(string.Format("BitArray invalid argument: begin {0} (must be 0-{1}).", begin, this.Length - 1));
				}
				if (count < 0 || count > this.Length)
				{
					throw new ArgumentException(string.Format("BitArray invalid argument: count {0} (must be 0-{1}).", count, this.Length));
				}
				if (numBits < 1 || count < numBits)
				{
					throw new ArgumentException(string.Format("BitArray invalid argument: numBits {0} (must be greater than 0).", numBits));
				}
			}

			// Token: 0x06000B45 RID: 2885 RVA: 0x000224B0 File Offset: 0x000206B0
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private readonly void CheckArgsUlong(int pos, int numBits)
			{
				if (numBits < 1 || numBits > 64)
				{
					throw new ArgumentException(string.Format("BitArray invalid arguments: numBits {0} (must be 1-64).", numBits));
				}
				if (pos + numBits > this.Length)
				{
					throw new ArgumentException(string.Format("BitArray invalid arguments: Out of bounds pos {0}, numBits {1}, Length {2}.", pos, numBits, this.Length));
				}
			}

			// Token: 0x040004AA RID: 1194
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly ulong* Ptr;

			// Token: 0x040004AB RID: 1195
			public readonly int Length;
		}
	}
}
