using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding.Collections
{
	// Token: 0x02000267 RID: 615
	[BurstCompile]
	public struct HierarchicalBitset
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x0005E7C0 File Offset: 0x0005C9C0
		public HierarchicalBitset(int size, Allocator allocator)
		{
			this.allocator = allocator;
			this.l1 = new UnsafeSpan<ulong>(allocator, size + 64 - 1 >> 6);
			this.l2 = new UnsafeSpan<ulong>(allocator, size + 4095 >> 6 >> 6);
			this.l3 = new UnsafeSpan<ulong>(allocator, size + 262143 >> 6 >> 6 >> 6);
			this.l1.FillZeros<ulong>();
			this.l2.FillZeros<ulong>();
			this.l3.FillZeros<ulong>();
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0005E839 File Offset: 0x0005CA39
		public bool IsCreated
		{
			get
			{
				return this.Capacity > 0;
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0005E844 File Offset: 0x0005CA44
		public void Dispose()
		{
			this.l1.Free(this.allocator);
			this.l2.Free(this.allocator);
			this.l3.Free(this.allocator);
			this = default(HierarchicalBitset);
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0005E880 File Offset: 0x0005CA80
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x0005E890 File Offset: 0x0005CA90
		public int Capacity
		{
			get
			{
				return this.l1.Length << 6;
			}
			set
			{
				if (value < this.Capacity)
				{
					throw new ArgumentException("Shrinking the bitset is not supported");
				}
				if (value == this.Capacity)
				{
					return;
				}
				HierarchicalBitset hierarchicalBitset = new HierarchicalBitset(value, this.allocator);
				this.l1.CopyTo(hierarchicalBitset.l1);
				this.l2.CopyTo(hierarchicalBitset.l2);
				this.l3.CopyTo(hierarchicalBitset.l3);
				this.Dispose();
				this = hierarchicalBitset;
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0005E90C File Offset: 0x0005CB0C
		public unsafe int Count()
		{
			int num = 0;
			for (int i = 0; i < this.l1.Length; i++)
			{
				num += math.countbits(*this.l1[i]);
			}
			return num;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0005E948 File Offset: 0x0005CB48
		public unsafe bool IsEmpty
		{
			get
			{
				for (int i = 0; i < this.l3.Length; i++)
				{
					if (*this.l3[i] != 0UL)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0005E97D File Offset: 0x0005CB7D
		public void Clear()
		{
			this.l1.FillZeros<ulong>();
			this.l2.FillZeros<ulong>();
			this.l3.FillZeros<ulong>();
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0005E9A0 File Offset: 0x0005CBA0
		public void GetIndices(NativeList<int> result)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(256, Allocator.Temp, NativeArrayOptions.ClearMemory);
			HierarchicalBitset.Iterator iterator = this.GetIterator(nativeArray.AsUnsafeSpan<int>());
			while (iterator.MoveNext())
			{
				UnsafeSpan<int> unsafeSpan = iterator.Current;
				for (int i = 0; i < unsafeSpan.Length; i++)
				{
					result.Add(unsafeSpan[i]);
				}
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0005E9FC File Offset: 0x0005CBFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool SetAtomic(ref UnsafeSpan<ulong> span, int index)
		{
			int num = index >> 6;
			ulong num2 = *span[num];
			if ((num2 & (1UL << index)) != 0UL)
			{
				return true;
			}
			for (;;)
			{
				ulong num3 = (ulong)Interlocked.CompareExchange(UnsafeUtility.As<ulong, long>(span[num]), (long)(num2 | (1UL << index)), (long)num2);
				if (num3 == num2)
				{
					break;
				}
				num2 = num3;
			}
			return false;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0005EA48 File Offset: 0x0005CC48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool ResetAtomic(ref UnsafeSpan<ulong> span, int index)
		{
			int num = index >> 6;
			ulong num2 = *span[num];
			if ((num2 & (1UL << index)) == 0UL)
			{
				return true;
			}
			for (;;)
			{
				ulong num3 = (ulong)Interlocked.CompareExchange(UnsafeUtility.As<ulong, long>(span[num]), (long)(num2 & ~(1UL << index)), (long)num2);
				if (num3 == num2)
				{
					break;
				}
				num2 = num3;
			}
			return false;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0005EA95 File Offset: 0x0005CC95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool Get(int index)
		{
			return (*this.l1[index >> 6] & (1UL << index)) > 0UL;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0005EAB2 File Offset: 0x0005CCB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int index)
		{
			if (HierarchicalBitset.SetAtomic(ref this.l1, index))
			{
				return;
			}
			HierarchicalBitset.SetAtomic(ref this.l2, index >> 6);
			HierarchicalBitset.SetAtomic(ref this.l3, index >> 12);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0005EAE4 File Offset: 0x0005CCE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Reset(int index)
		{
			if (HierarchicalBitset.ResetAtomic(ref this.l1, index))
			{
				return;
			}
			if (*this.l1[index >> 6] == 0UL)
			{
				HierarchicalBitset.ResetAtomic(ref this.l2, index >> 6);
			}
			if (*this.l2[index >> 12] == 0UL)
			{
				HierarchicalBitset.ResetAtomic(ref this.l3, index >> 12);
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0005EB42 File Offset: 0x0005CD42
		public HierarchicalBitset.Iterator GetIterator(UnsafeSpan<int> scratchBuffer)
		{
			return new HierarchicalBitset.Iterator(this, scratchBuffer);
		}

		// Token: 0x04000B70 RID: 2928
		private UnsafeSpan<ulong> l1;

		// Token: 0x04000B71 RID: 2929
		private UnsafeSpan<ulong> l2;

		// Token: 0x04000B72 RID: 2930
		private UnsafeSpan<ulong> l3;

		// Token: 0x04000B73 RID: 2931
		private Allocator allocator;

		// Token: 0x04000B74 RID: 2932
		private const int Log64 = 6;

		// Token: 0x02000268 RID: 616
		[BurstCompile]
		public struct Iterator : IEnumerator<UnsafeSpan<int>>, IEnumerator, IDisposable, IEnumerable<UnsafeSpan<int>>, IEnumerable
		{
			// Token: 0x1700023D RID: 573
			// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0005EB50 File Offset: 0x0005CD50
			public UnsafeSpan<int> Current
			{
				get
				{
					return this.result.Slice(0, this.resultCount);
				}
			}

			// Token: 0x1700023E RID: 574
			// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0001C8BA File Offset: 0x0001AABA
			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x06000F5A RID: 3930 RVA: 0x0001C8BA File Offset: 0x0001AABA
			public void Reset()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000F5B RID: 3931 RVA: 0x00003788 File Offset: 0x00001988
			public void Dispose()
			{
			}

			// Token: 0x06000F5C RID: 3932 RVA: 0x0005EB64 File Offset: 0x0005CD64
			public IEnumerator<UnsafeSpan<int>> GetEnumerator()
			{
				return this;
			}

			// Token: 0x06000F5D RID: 3933 RVA: 0x0001C8BA File Offset: 0x0001AABA
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000F5E RID: 3934 RVA: 0x0005EB71 File Offset: 0x0005CD71
			private static int l2index(int l3index, int l3bitIndex)
			{
				return (l3index << 6) + l3bitIndex;
			}

			// Token: 0x06000F5F RID: 3935 RVA: 0x0005EB71 File Offset: 0x0005CD71
			private static int l1index(int l2index, int l2bitIndex)
			{
				return (l2index << 6) + l2bitIndex;
			}

			// Token: 0x06000F60 RID: 3936 RVA: 0x0005EB78 File Offset: 0x0005CD78
			public Iterator(HierarchicalBitset bitSet, UnsafeSpan<int> result)
			{
				this.bitSet = bitSet;
				this.result = result;
				this.resultCount = 0;
				this.l3index = 0;
				this.l3bitIndex = 0;
				this.l2bitIndex = 0;
				if (result.Length < 128)
				{
					throw new ArgumentException("Result array must be at least 128 elements long");
				}
			}

			// Token: 0x06000F61 RID: 3937 RVA: 0x0005EBC8 File Offset: 0x0005CDC8
			public bool MoveNext()
			{
				return HierarchicalBitset.Iterator.MoveNextBurst(ref this);
			}

			// Token: 0x06000F62 RID: 3938 RVA: 0x0005EBD0 File Offset: 0x0005CDD0
			[BurstCompile]
			public static bool MoveNextBurst(ref HierarchicalBitset.Iterator iter)
			{
				return HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Invoke(ref iter);
			}

			// Token: 0x06000F63 RID: 3939 RVA: 0x0005EBD8 File Offset: 0x0005CDD8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private unsafe bool MoveNextInternal()
			{
				uint num = 0U;
				int num2 = this.l3index;
				int num3 = this.l3bitIndex;
				int num4 = this.l2bitIndex;
				while ((long)num2 < (long)((ulong)this.bitSet.l3.length))
				{
					ulong num5 = *this.bitSet.l3[num2] & (ulong.MaxValue << num3);
					if (num5 != 0UL)
					{
						while (num5 != 0UL)
						{
							num3 = math.tzcnt(num5);
							int num6 = HierarchicalBitset.Iterator.l2index(num2, num3);
							for (ulong num7 = *this.bitSet.l2[num6] & (ulong.MaxValue << num4); num7 != 0UL; num7 &= num7 - 1UL)
							{
								num4 = math.tzcnt(num7);
								if ((ulong)(num + 64U) > (ulong)((long)this.result.Length))
								{
									this.resultCount = (int)num;
									this.l3index = num2;
									this.l3bitIndex = num3;
									this.l2bitIndex = num4;
									return true;
								}
								int num8 = HierarchicalBitset.Iterator.l1index(num6, num4);
								ulong num9 = *this.bitSet.l1[num8];
								int num10 = num8 << 6;
								while (num9 != 0UL)
								{
									int num11 = math.tzcnt(num9);
									num9 &= num9 - 1UL;
									int num12 = num10 + num11;
									Hint.Assume(num < (uint)this.result.Length);
									*this.result[num++] = num12;
								}
							}
							num5 &= num5 - 1UL;
							num4 = 0;
						}
						num4 = 0;
						num3 = 0;
					}
					num2++;
				}
				this.resultCount = (int)num;
				this.l3index = num2;
				this.l3bitIndex = num3;
				this.l2bitIndex = num4;
				return num > 0U;
			}

			// Token: 0x06000F64 RID: 3940 RVA: 0x0005ED64 File Offset: 0x0005CF64
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool MoveNextBurst$BurstManaged(ref HierarchicalBitset.Iterator iter)
			{
				return iter.MoveNextInternal();
			}

			// Token: 0x04000B75 RID: 2933
			private HierarchicalBitset bitSet;

			// Token: 0x04000B76 RID: 2934
			private UnsafeSpan<int> result;

			// Token: 0x04000B77 RID: 2935
			private int resultCount;

			// Token: 0x04000B78 RID: 2936
			private int l3index;

			// Token: 0x04000B79 RID: 2937
			private int l3bitIndex;

			// Token: 0x04000B7A RID: 2938
			private int l2bitIndex;

			// Token: 0x02000269 RID: 617
			// (Invoke) Token: 0x06000F66 RID: 3942
			internal delegate bool MoveNextBurst_00000E4F$PostfixBurstDelegate(ref HierarchicalBitset.Iterator iter);

			// Token: 0x0200026A RID: 618
			internal static class MoveNextBurst_00000E4F$BurstDirectCall
			{
				// Token: 0x06000F69 RID: 3945 RVA: 0x0005ED6C File Offset: 0x0005CF6C
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Pointer == 0)
					{
						HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.DeferredCompilation, methodof(HierarchicalBitset.Iterator.MoveNextBurst$BurstManaged(ref HierarchicalBitset.Iterator)).MethodHandle, typeof(HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Pointer;
				}

				// Token: 0x06000F6A RID: 3946 RVA: 0x0005ED98 File Offset: 0x0005CF98
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x06000F6B RID: 3947 RVA: 0x0005EDB0 File Offset: 0x0005CFB0
				public static void Constructor()
				{
					HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(HierarchicalBitset.Iterator.MoveNextBurst(ref HierarchicalBitset.Iterator)).MethodHandle);
				}

				// Token: 0x06000F6C RID: 3948 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x06000F6D RID: 3949 RVA: 0x0005EDC1 File Offset: 0x0005CFC1
				// Note: this type is marked as 'beforefieldinit'.
				static MoveNextBurst_00000E4F$BurstDirectCall()
				{
					HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.Constructor();
				}

				// Token: 0x06000F6E RID: 3950 RVA: 0x0005EDC8 File Offset: 0x0005CFC8
				public static bool Invoke(ref HierarchicalBitset.Iterator iter)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = HierarchicalBitset.Iterator.MoveNextBurst_00000E4F$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Pathfinding.Collections.HierarchicalBitset/Iterator&), ref iter, functionPointer);
						}
					}
					return HierarchicalBitset.Iterator.MoveNextBurst$BurstManaged(ref iter);
				}

				// Token: 0x04000B7B RID: 2939
				private static IntPtr Pointer;

				// Token: 0x04000B7C RID: 2940
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
