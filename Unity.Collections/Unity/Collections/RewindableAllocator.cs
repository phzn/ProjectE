using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x020000D8 RID: 216
	[BurstCompile]
	public struct RewindableAllocator : AllocatorManager.IAllocator, IDisposable
	{
		// Token: 0x060009F7 RID: 2551 RVA: 0x0001D2AC File Offset: 0x0001B4AC
		public unsafe void Initialize(int initialSizeInBytes, bool enableBlockFree = false)
		{
			this.m_spinner = default(Spinner);
			this.m_block = new UnmanagedArray<RewindableAllocator.MemoryBlock>(64, Allocator.Persistent);
			long num = (((long)initialSizeInBytes > 131072L) ? ((long)initialSizeInBytes) : 131072L);
			*this.m_block[0] = new RewindableAllocator.MemoryBlock(num);
			this.m_last = (this.m_used = 0);
			this.m_enableBlockFree = (enableBlockFree ? 1 : 0);
			this.m_reachMaxBlockSize = (((long)initialSizeInBytes >= 67108864L) ? 1 : 0);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0001D335 File Offset: 0x0001B535
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0001D340 File Offset: 0x0001B540
		public bool EnableBlockFree
		{
			get
			{
				return this.m_enableBlockFree > 0;
			}
			set
			{
				this.m_enableBlockFree = (value ? 1 : 0);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0001D34F File Offset: 0x0001B54F
		public int BlocksAllocated
		{
			get
			{
				return this.m_last + 1;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0001D359 File Offset: 0x0001B559
		public int InitialSizeInBytes
		{
			get
			{
				return (int)this.m_block[0].m_bytes;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0001D36D File Offset: 0x0001B56D
		internal long MaxMemoryBlockSize
		{
			get
			{
				return 67108864L;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0001D378 File Offset: 0x0001B578
		internal long BytesAllocated
		{
			get
			{
				long num = 0L;
				for (int i = 0; i <= this.m_last; i++)
				{
					num += this.m_block[i].m_bytes;
				}
				return num;
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001D3B0 File Offset: 0x0001B5B0
		public void Rewind()
		{
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Rewind a RewindableAllocator from a Job.");
			}
			this.m_handle.Rewind();
			while (this.m_last > this.m_used)
			{
				int num = this.m_last;
				this.m_last = num - 1;
				this.m_block[num].Dispose();
			}
			while (this.m_used > 0)
			{
				int num = this.m_used;
				this.m_used = num - 1;
				this.m_block[num].Rewind();
			}
			this.m_block[0].Rewind();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001D448 File Offset: 0x0001B648
		public void Dispose()
		{
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Dispose a RewindableAllocator from a Job.");
			}
			this.m_used = 0;
			this.Rewind();
			this.m_block[0].Dispose();
			this.m_block.Dispose();
			this.m_last = (this.m_used = 0);
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0001D4A0 File Offset: 0x0001B6A0
		[ExcludeFromBurstCompatTesting("Uses managed delegate")]
		public AllocatorManager.TryFunction Function
		{
			get
			{
				return new AllocatorManager.TryFunction(RewindableAllocator.Try);
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001D4B0 File Offset: 0x0001B6B0
		private unsafe int TryAllocate(ref AllocatorManager.Block block, int startIndex, int lastIndex, long alignedSize, long alignmentMask)
		{
			int i = startIndex;
			while (i <= lastIndex)
			{
				RewindableAllocator.Union union = default(RewindableAllocator.Union);
				bool flag = false;
				union.m_long = Interlocked.Read(ref this.m_block[i].m_union.m_long);
				long num;
				RewindableAllocator.Union union2;
				do
				{
					num = (union.m_current + alignmentMask) & ~alignmentMask;
					if (num + block.Bytes > this.m_block[i].m_bytes)
					{
						goto Block_1;
					}
					union2 = union;
					RewindableAllocator.Union union3 = default(RewindableAllocator.Union);
					union3.m_current = ((num + alignedSize > this.m_block[i].m_bytes) ? this.m_block[i].m_bytes : (num + alignedSize));
					union3.m_allocCount = union.m_allocCount + 1L;
					union.m_long = Interlocked.CompareExchange(ref this.m_block[i].m_union.m_long, union3.m_long, union2.m_long);
				}
				while (union.m_long != union2.m_long);
				IL_00F9:
				if (!flag)
				{
					block.Range.Pointer = (IntPtr)((void*)(this.m_block[i].m_pointer + num));
					block.AllocatedItems = block.Range.Items;
					Interlocked.MemoryBarrier();
					int num2 = this.m_used;
					int num3;
					int num4;
					do
					{
						num3 = num2;
						num4 = ((i > num3) ? i : num3);
						num2 = Interlocked.CompareExchange(ref this.m_used, num4, num3);
					}
					while (num4 != num3);
					return 0;
				}
				i++;
				continue;
				Block_1:
				flag = true;
				goto IL_00F9;
			}
			return -1;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001D634 File Offset: 0x0001B834
		public unsafe int Try(ref AllocatorManager.Block block)
		{
			if (block.Range.Pointer == IntPtr.Zero)
			{
				int num = math.max(64, block.Alignment);
				int num2 = ((num != 64) ? 1 : 0);
				int num3 = 63;
				if (num2 == 1)
				{
					num = (num + num3) & ~num3;
				}
				long num4 = (long)num - 1L;
				long num5 = (block.Bytes + (long)(num2 * num) + num4) & ~num4;
				int last = this.m_last;
				int num6 = this.TryAllocate(ref block, 0, this.m_last, num5, num4);
				if (num6 == 0)
				{
					return num6;
				}
				this.m_spinner.Acquire();
				num6 = this.TryAllocate(ref block, last, this.m_last, num5, num4);
				if (num6 == 0)
				{
					this.m_spinner.Release();
					return num6;
				}
				long num7;
				if (this.m_reachMaxBlockSize == 0)
				{
					num7 = this.m_block[this.m_last].m_bytes << 1;
				}
				else
				{
					num7 = this.m_block[this.m_last].m_bytes + 67108864L;
				}
				num7 = math.max(num7, num5);
				this.m_reachMaxBlockSize = ((num7 >= 67108864L) ? 1 : 0);
				*this.m_block[this.m_last + 1] = new RewindableAllocator.MemoryBlock(num7);
				Interlocked.Increment(ref this.m_last);
				num6 = this.TryAllocate(ref block, this.m_last, this.m_last, num5, num4);
				this.m_spinner.Release();
				return num6;
			}
			else
			{
				if (block.Range.Items == 0)
				{
					if (this.m_enableBlockFree != 0)
					{
						for (int i = 0; i <= this.m_last; i++)
						{
							if (this.m_block[i].Contains(block.Range.Pointer))
							{
								RewindableAllocator.Union union = default(RewindableAllocator.Union);
								union.m_long = Interlocked.Read(ref this.m_block[i].m_union.m_long);
								RewindableAllocator.Union union2;
								do
								{
									union2 = union;
									RewindableAllocator.Union union3 = union;
									long allocCount = union3.m_allocCount;
									union3.m_allocCount = allocCount - 1L;
									if (union3.m_allocCount == 0L)
									{
										union3.m_current = 0L;
									}
									union.m_long = Interlocked.CompareExchange(ref this.m_block[i].m_union.m_long, union3.m_long, union2.m_long);
								}
								while (union.m_long != union2.m_long);
							}
						}
					}
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001D894 File Offset: 0x0001BA94
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		internal static int Try(IntPtr state, ref AllocatorManager.Block block)
		{
			return RewindableAllocator.Try_000009E0$BurstDirectCall.Invoke(state, ref block);
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0001D89D File Offset: 0x0001BA9D
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001D8A5 File Offset: 0x0001BAA5
		public AllocatorManager.AllocatorHandle Handle
		{
			get
			{
				return this.m_handle;
			}
			set
			{
				this.m_handle = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0001D8AE File Offset: 0x0001BAAE
		public Allocator ToAllocator
		{
			get
			{
				return this.m_handle.ToAllocator;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0001D8BB File Offset: 0x0001BABB
		public bool IsCustomAllocator
		{
			get
			{
				return this.m_handle.IsCustomAllocator;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x000040FD File Offset: 0x000022FD
		public bool IsAutoDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public NativeArray<T> AllocateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int length) where T : struct, ValueType
		{
			return new NativeArray<T>
			{
				m_Buffer = (ref this).AllocateStruct(default(T), length),
				m_Length = length,
				m_AllocatorLabel = Allocator.None
			};
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001D908 File Offset: 0x0001BB08
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe NativeList<T> AllocateNativeList<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int capacity) where T : struct, ValueType
		{
			NativeList<T> nativeList = default(NativeList<T>);
			nativeList.m_ListData = (ref this).Allocate(default(UnsafeList<T>), 1);
			nativeList.m_ListData->Ptr = (ref this).Allocate(default(T), capacity);
			nativeList.m_ListData->m_length = 0;
			nativeList.m_ListData->m_capacity = capacity;
			nativeList.m_ListData->Allocator = Allocator.None;
			return nativeList;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001D979 File Offset: 0x0001BB79
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Try$BurstManaged(IntPtr state, ref AllocatorManager.Block block)
		{
			return ((RewindableAllocator*)(void*)state)->Try(ref block);
		}

		// Token: 0x04000419 RID: 1049
		private const int kLog2MaxMemoryBlockSize = 26;

		// Token: 0x0400041A RID: 1050
		private const long kMaxMemoryBlockSize = 67108864L;

		// Token: 0x0400041B RID: 1051
		private const long kMinMemoryBlockSize = 131072L;

		// Token: 0x0400041C RID: 1052
		private const int kMaxNumBlocks = 64;

		// Token: 0x0400041D RID: 1053
		private const int kBlockBusyRewindMask = -2147483648;

		// Token: 0x0400041E RID: 1054
		private const int kBlockBusyAllocateMask = 2147483647;

		// Token: 0x0400041F RID: 1055
		private Spinner m_spinner;

		// Token: 0x04000420 RID: 1056
		private AllocatorManager.AllocatorHandle m_handle;

		// Token: 0x04000421 RID: 1057
		private UnmanagedArray<RewindableAllocator.MemoryBlock> m_block;

		// Token: 0x04000422 RID: 1058
		private int m_last;

		// Token: 0x04000423 RID: 1059
		private int m_used;

		// Token: 0x04000424 RID: 1060
		private byte m_enableBlockFree;

		// Token: 0x04000425 RID: 1061
		private byte m_reachMaxBlockSize;

		// Token: 0x020000D9 RID: 217
		internal struct Union
		{
			// Token: 0x1700012A RID: 298
			// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0001D987 File Offset: 0x0001BB87
			// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0001D999 File Offset: 0x0001BB99
			internal long m_current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_long & 1099511627775L;
				}
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
					this.m_long &= -1099511627776L;
					this.m_long |= value & 1099511627775L;
				}
			}

			// Token: 0x1700012B RID: 299
			// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0001D9C9 File Offset: 0x0001BBC9
			// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0001D9DB File Offset: 0x0001BBDB
			internal long m_allocCount
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (this.m_long >> 40) & 16777215L;
				}
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
					this.m_long &= 1099511627775L;
					this.m_long |= (value & 16777215L) << 40;
				}
			}

			// Token: 0x04000426 RID: 1062
			internal long m_long;

			// Token: 0x04000427 RID: 1063
			private const int currentBits = 40;

			// Token: 0x04000428 RID: 1064
			private const int currentOffset = 0;

			// Token: 0x04000429 RID: 1065
			private const long currentMask = 1099511627775L;

			// Token: 0x0400042A RID: 1066
			private const int allocCountBits = 24;

			// Token: 0x0400042B RID: 1067
			private const int allocCountOffset = 40;

			// Token: 0x0400042C RID: 1068
			private const long allocCountMask = 16777215L;
		}

		// Token: 0x020000DA RID: 218
		[GenerateTestsForBurstCompatibility]
		internal struct MemoryBlock : IDisposable
		{
			// Token: 0x06000A10 RID: 2576 RVA: 0x0001DA0B File Offset: 0x0001BC0B
			public unsafe MemoryBlock(long bytes)
			{
				this.m_pointer = (byte*)Memory.Unmanaged.Allocate(bytes, 16384, Allocator.Persistent);
				this.m_bytes = bytes;
				this.m_union = default(RewindableAllocator.Union);
			}

			// Token: 0x06000A11 RID: 2577 RVA: 0x0001DA37 File Offset: 0x0001BC37
			public void Rewind()
			{
				this.m_union = default(RewindableAllocator.Union);
			}

			// Token: 0x06000A12 RID: 2578 RVA: 0x0001DA45 File Offset: 0x0001BC45
			public void Dispose()
			{
				Memory.Unmanaged.Free<byte>(this.m_pointer, Allocator.Persistent);
				this.m_pointer = null;
				this.m_bytes = 0L;
				this.m_union = default(RewindableAllocator.Union);
			}

			// Token: 0x06000A13 RID: 2579 RVA: 0x0001DA74 File Offset: 0x0001BC74
			public unsafe bool Contains(IntPtr ptr)
			{
				void* ptr2 = (void*)ptr;
				return ptr2 >= (void*)this.m_pointer && ptr2 < (void*)(this.m_pointer + this.m_union.m_current);
			}

			// Token: 0x0400042D RID: 1069
			public const int kMaximumAlignment = 16384;

			// Token: 0x0400042E RID: 1070
			public unsafe byte* m_pointer;

			// Token: 0x0400042F RID: 1071
			public long m_bytes;

			// Token: 0x04000430 RID: 1072
			public RewindableAllocator.Union m_union;
		}

		// Token: 0x020000DB RID: 219
		// (Invoke) Token: 0x06000A15 RID: 2581
		internal delegate int Try_000009E0$PostfixBurstDelegate(IntPtr state, ref AllocatorManager.Block block);

		// Token: 0x020000DC RID: 220
		internal static class Try_000009E0$BurstDirectCall
		{
			// Token: 0x06000A18 RID: 2584 RVA: 0x0001DAA9 File Offset: 0x0001BCA9
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RewindableAllocator.Try_000009E0$BurstDirectCall.Pointer == 0)
				{
					RewindableAllocator.Try_000009E0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RewindableAllocator.Try_000009E0$BurstDirectCall.DeferredCompilation, methodof(RewindableAllocator.Try$BurstManaged(IntPtr, ref AllocatorManager.Block)).MethodHandle, typeof(RewindableAllocator.Try_000009E0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RewindableAllocator.Try_000009E0$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A19 RID: 2585 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RewindableAllocator.Try_000009E0$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000A1A RID: 2586 RVA: 0x0001DAF0 File Offset: 0x0001BCF0
			public static void Constructor()
			{
				RewindableAllocator.Try_000009E0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RewindableAllocator.Try(IntPtr, ref AllocatorManager.Block)).MethodHandle);
			}

			// Token: 0x06000A1B RID: 2587 RVA: 0x00002C47 File Offset: 0x00000E47
			public static void Initialize()
			{
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x0001DB01 File Offset: 0x0001BD01
			// Note: this type is marked as 'beforefieldinit'.
			static Try_000009E0$BurstDirectCall()
			{
				RewindableAllocator.Try_000009E0$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A1D RID: 2589 RVA: 0x0001DB08 File Offset: 0x0001BD08
			public static int Invoke(IntPtr state, ref AllocatorManager.Block block)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RewindableAllocator.Try_000009E0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.IntPtr,Unity.Collections.AllocatorManager/Block&), state, ref block, functionPointer);
					}
				}
				return RewindableAllocator.Try$BurstManaged(state, ref block);
			}

			// Token: 0x04000431 RID: 1073
			private static IntPtr Pointer;

			// Token: 0x04000432 RID: 1074
			private static IntPtr DeferredCompilation;
		}
	}
}
