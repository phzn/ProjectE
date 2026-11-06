using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000143 RID: 323
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeStream : INativeDisposable, IDisposable
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x00029A8A File Offset: 0x00027C8A
		public UnsafeStream(int bufferCount, AllocatorManager.AllocatorHandle allocator)
		{
			UnsafeStream.AllocateBlock(out this, allocator);
			this.AllocateForEach(bufferCount);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00029A9C File Offset: 0x00027C9C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static JobHandle ScheduleConstruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out UnsafeStream stream, NativeList<T> bufferCount, JobHandle dependency, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			UnsafeStream.AllocateBlock(out stream, allocator);
			return new UnsafeStream.ConstructJobList
			{
				List = (UntypedUnsafeList*)bufferCount.GetUnsafeList(),
				Container = stream
			}.Schedule(dependency);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00029ADC File Offset: 0x00027CDC
		public static JobHandle ScheduleConstruct(out UnsafeStream stream, NativeArray<int> bufferCount, JobHandle dependency, AllocatorManager.AllocatorHandle allocator)
		{
			UnsafeStream.AllocateBlock(out stream, allocator);
			return new UnsafeStream.ConstructJob
			{
				Length = bufferCount,
				Container = stream
			}.Schedule(dependency);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00029B14 File Offset: 0x00027D14
		internal unsafe static void AllocateBlock(out UnsafeStream stream, AllocatorManager.AllocatorHandle allocator)
		{
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			int num = sizeof(UnsafeStreamBlockData) + sizeof(UnsafeStreamBlock*) * threadIndexCount;
			AllocatorManager.Block block = (ref allocator).AllocateBlock<AllocatorManager.AllocatorHandle>(num, 16, 1);
			UnsafeUtility.MemClear((void*)block.Range.Pointer, block.AllocatedBytes);
			stream.m_BlockData = block;
			UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)block.Range.Pointer;
			ptr->Allocator = allocator;
			ptr->BlockCount = threadIndexCount;
			ptr->Blocks = (UnsafeStreamBlock**)(void*)(block.Range.Pointer + sizeof(UnsafeStreamBlockData));
			ptr->Ranges = default(AllocatorManager.Block);
			ptr->RangeCount = 0;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00029BBC File Offset: 0x00027DBC
		internal unsafe void AllocateForEach(int forEachCount)
		{
			UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
			ptr->Ranges = (ref this.m_BlockData.Range.Allocator).AllocateBlock<AllocatorManager.AllocatorHandle>(sizeof(UnsafeStreamRange), 16, forEachCount);
			ptr->RangeCount = forEachCount;
			UnsafeUtility.MemClear((void*)ptr->Ranges.Range.Pointer, ptr->Ranges.AllocatedBytes);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00029C30 File Offset: 0x00027E30
		public unsafe readonly bool IsEmpty()
		{
			if (!this.IsCreated)
			{
				return true;
			}
			UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
			UnsafeStreamRange* ptr2 = (UnsafeStreamRange*)(void*)ptr->Ranges.Range.Pointer;
			for (int num = 0; num != ptr->RangeCount; num++)
			{
				if (ptr2[num].ElementCount > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00029C9A File Offset: 0x00027E9A
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_BlockData.Range.Pointer != IntPtr.Zero;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00029CB6 File Offset: 0x00027EB6
		public unsafe readonly int ForEachCount
		{
			get
			{
				return ((UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer)->RangeCount;
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00029CD2 File Offset: 0x00027ED2
		public UnsafeStream.Reader AsReader()
		{
			return new UnsafeStream.Reader(ref this);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00029CDA File Offset: 0x00027EDA
		public UnsafeStream.Writer AsWriter()
		{
			return new UnsafeStream.Writer(ref this);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00029CE4 File Offset: 0x00027EE4
		public unsafe int Count()
		{
			int num = 0;
			UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
			UnsafeStreamRange* ptr2 = (UnsafeStreamRange*)(void*)ptr->Ranges.Range.Pointer;
			for (int num2 = 0; num2 != ptr->RangeCount; num2++)
			{
				num += ptr2[num2].ElementCount;
			}
			return num;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00029D44 File Offset: 0x00027F44
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe NativeArray<T> ToNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Count(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeStream.Reader reader = this.AsReader();
			int num = 0;
			for (int num2 = 0; num2 != reader.ForEachCount; num2++)
			{
				reader.BeginForEachIndex(num2);
				int remainingItemCount = reader.RemainingItemCount;
				for (int i = 0; i < remainingItemCount; i++)
				{
					nativeArray[num] = *reader.Read<T>();
					num++;
				}
				reader.EndForEachIndex();
			}
			return nativeArray;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00029DC0 File Offset: 0x00027FC0
		private unsafe void Deallocate()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
			for (int num = 0; num != ptr->BlockCount; num++)
			{
				UnsafeStreamBlock* next;
				for (UnsafeStreamBlock* ptr2 = *(IntPtr*)(ptr->Blocks + (IntPtr)num * (IntPtr)sizeof(UnsafeStreamBlock*) / (IntPtr)sizeof(UnsafeStreamBlock*)); ptr2 != null; ptr2 = next)
				{
					next = ptr2->Next;
					ptr->Free(ptr2);
				}
			}
			ptr->Ranges.Dispose();
			this.m_BlockData.Dispose();
			this.m_BlockData = default(AllocatorManager.Block);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00029E46 File Offset: 0x00028046
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.Deallocate();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00029E58 File Offset: 0x00028058
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeStream.DisposeJob
			{
				Container = this
			}.Schedule(inputDeps);
			this.m_BlockData = default(AllocatorManager.Block);
			return jobHandle;
		}

		// Token: 0x04000535 RID: 1333
		[NativeDisableUnsafePtrRestriction]
		internal AllocatorManager.Block m_BlockData;

		// Token: 0x02000144 RID: 324
		[BurstCompile]
		private struct DisposeJob : IJob
		{
			// Token: 0x06000DA5 RID: 3493 RVA: 0x00029E97 File Offset: 0x00028097
			public void Execute()
			{
				this.Container.Deallocate();
			}

			// Token: 0x04000536 RID: 1334
			public UnsafeStream Container;
		}

		// Token: 0x02000145 RID: 325
		[BurstCompile]
		private struct ConstructJobList : IJob
		{
			// Token: 0x06000DA6 RID: 3494 RVA: 0x00029EA4 File Offset: 0x000280A4
			public unsafe void Execute()
			{
				this.Container.AllocateForEach(this.List->m_length);
			}

			// Token: 0x04000537 RID: 1335
			public UnsafeStream Container;

			// Token: 0x04000538 RID: 1336
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe UntypedUnsafeList* List;
		}

		// Token: 0x02000146 RID: 326
		[BurstCompile]
		private struct ConstructJob : IJob
		{
			// Token: 0x06000DA7 RID: 3495 RVA: 0x00029EBC File Offset: 0x000280BC
			public void Execute()
			{
				this.Container.AllocateForEach(this.Length[0]);
			}

			// Token: 0x04000539 RID: 1337
			public UnsafeStream Container;

			// Token: 0x0400053A RID: 1338
			[ReadOnly]
			public NativeArray<int> Length;
		}

		// Token: 0x02000147 RID: 327
		[GenerateTestsForBurstCompatibility]
		public struct Writer
		{
			// Token: 0x06000DA8 RID: 3496 RVA: 0x00029ED8 File Offset: 0x000280D8
			internal Writer(ref UnsafeStream stream)
			{
				this.m_BlockData = stream.m_BlockData;
				this.m_ForeachIndex = int.MinValue;
				this.m_ElementCount = -1;
				this.m_CurrentBlock = null;
				this.m_CurrentBlockEnd = null;
				this.m_CurrentPtr = null;
				this.m_FirstBlock = null;
				this.m_NumberOfBlocks = 0;
				this.m_FirstOffset = 0;
				this.m_ThreadIndex = 0;
			}

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00029F38 File Offset: 0x00028138
			public unsafe int ForEachCount
			{
				get
				{
					return ((UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer)->RangeCount;
				}
			}

			// Token: 0x06000DAA RID: 3498 RVA: 0x00029F54 File Offset: 0x00028154
			public unsafe void BeginForEachIndex(int foreachIndex)
			{
				this.m_ForeachIndex = foreachIndex;
				this.m_ElementCount = 0;
				this.m_NumberOfBlocks = 0;
				this.m_FirstBlock = this.m_CurrentBlock;
				this.m_FirstOffset = (int)((long)((byte*)this.m_CurrentPtr - (byte*)this.m_CurrentBlock));
			}

			// Token: 0x06000DAB RID: 3499 RVA: 0x00029F90 File Offset: 0x00028190
			public unsafe void EndForEachIndex()
			{
				UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
				UnsafeStreamRange* ptr2 = (UnsafeStreamRange*)(void*)ptr->Ranges.Range.Pointer;
				ptr2[this.m_ForeachIndex].ElementCount = this.m_ElementCount;
				ptr2[this.m_ForeachIndex].OffsetInFirstBlock = this.m_FirstOffset;
				ptr2[this.m_ForeachIndex].Block = this.m_FirstBlock;
				ptr2[this.m_ForeachIndex].LastOffset = (int)((long)((byte*)this.m_CurrentPtr - (byte*)this.m_CurrentBlock));
				ptr2[this.m_ForeachIndex].NumberOfBlocks = this.m_NumberOfBlocks;
			}

			// Token: 0x06000DAC RID: 3500 RVA: 0x0002A05B File Offset: 0x0002825B
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void Write<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType
			{
				*this.Allocate<T>() = value;
			}

			// Token: 0x06000DAD RID: 3501 RVA: 0x0002A06C File Offset: 0x0002826C
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe ref T Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				return UnsafeUtility.AsRef<T>((void*)this.Allocate(num));
			}

			// Token: 0x06000DAE RID: 3502 RVA: 0x0002A08C File Offset: 0x0002828C
			public unsafe byte* Allocate(int size)
			{
				byte* ptr = this.m_CurrentPtr;
				this.m_CurrentPtr += size;
				if (this.m_CurrentPtr != this.m_CurrentBlockEnd)
				{
					UnsafeStreamBlock* currentBlock = this.m_CurrentBlock;
					UnsafeStreamBlockData* ptr2 = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
					this.m_CurrentBlock = ptr2->Allocate(currentBlock, this.m_ThreadIndex);
					this.m_CurrentPtr = &this.m_CurrentBlock->Data.FixedElementField;
					if (this.m_FirstBlock == null)
					{
						this.m_FirstOffset = (int)((long)((byte*)this.m_CurrentPtr - (byte*)this.m_CurrentBlock));
						this.m_FirstBlock = this.m_CurrentBlock;
					}
					else
					{
						this.m_NumberOfBlocks++;
					}
					this.m_CurrentBlockEnd = (byte*)(this.m_CurrentBlock + 4096 / sizeof(UnsafeStreamBlock));
					ptr = this.m_CurrentPtr;
					this.m_CurrentPtr += size;
				}
				this.m_ElementCount++;
				return ptr;
			}

			// Token: 0x0400053B RID: 1339
			[NativeDisableUnsafePtrRestriction]
			internal AllocatorManager.Block m_BlockData;

			// Token: 0x0400053C RID: 1340
			[NativeDisableUnsafePtrRestriction]
			private unsafe UnsafeStreamBlock* m_CurrentBlock;

			// Token: 0x0400053D RID: 1341
			[NativeDisableUnsafePtrRestriction]
			private unsafe byte* m_CurrentPtr;

			// Token: 0x0400053E RID: 1342
			[NativeDisableUnsafePtrRestriction]
			private unsafe byte* m_CurrentBlockEnd;

			// Token: 0x0400053F RID: 1343
			internal int m_ForeachIndex;

			// Token: 0x04000540 RID: 1344
			private int m_ElementCount;

			// Token: 0x04000541 RID: 1345
			[NativeDisableUnsafePtrRestriction]
			private unsafe UnsafeStreamBlock* m_FirstBlock;

			// Token: 0x04000542 RID: 1346
			private int m_FirstOffset;

			// Token: 0x04000543 RID: 1347
			private int m_NumberOfBlocks;

			// Token: 0x04000544 RID: 1348
			[NativeSetThreadIndex]
			private int m_ThreadIndex;
		}

		// Token: 0x02000148 RID: 328
		[GenerateTestsForBurstCompatibility]
		public struct Reader
		{
			// Token: 0x06000DAF RID: 3503 RVA: 0x0002A179 File Offset: 0x00028379
			internal Reader(ref UnsafeStream stream)
			{
				this.m_BlockData = stream.m_BlockData;
				this.m_CurrentBlock = null;
				this.m_CurrentPtr = null;
				this.m_CurrentBlockEnd = null;
				this.m_RemainingItemCount = 0;
				this.m_LastBlockSize = 0;
			}

			// Token: 0x06000DB0 RID: 3504 RVA: 0x0002A1B0 File Offset: 0x000283B0
			public unsafe int BeginForEachIndex(int foreachIndex)
			{
				UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
				UnsafeStreamRange* ptr2 = (UnsafeStreamRange*)(void*)ptr->Ranges.Range.Pointer;
				this.m_RemainingItemCount = ptr2[foreachIndex].ElementCount;
				this.m_LastBlockSize = ptr2[foreachIndex].LastOffset;
				this.m_CurrentBlock = ptr2[foreachIndex].Block;
				this.m_CurrentPtr = (byte*)(this.m_CurrentBlock + ptr2[foreachIndex].OffsetInFirstBlock / sizeof(UnsafeStreamBlock));
				this.m_CurrentBlockEnd = (byte*)(this.m_CurrentBlock + 4096 / sizeof(UnsafeStreamBlock));
				return this.m_RemainingItemCount;
			}

			// Token: 0x06000DB1 RID: 3505 RVA: 0x00002C47 File Offset: 0x00000E47
			public void EndForEachIndex()
			{
			}

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0002A260 File Offset: 0x00028460
			public unsafe int ForEachCount
			{
				get
				{
					return ((UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer)->RangeCount;
				}
			}

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0002A27C File Offset: 0x0002847C
			public int RemainingItemCount
			{
				get
				{
					return this.m_RemainingItemCount;
				}
			}

			// Token: 0x06000DB4 RID: 3508 RVA: 0x0002A284 File Offset: 0x00028484
			public unsafe byte* ReadUnsafePtr(int size)
			{
				this.m_RemainingItemCount--;
				byte* ptr = this.m_CurrentPtr;
				this.m_CurrentPtr += size;
				if (this.m_CurrentPtr != this.m_CurrentBlockEnd)
				{
					this.m_CurrentBlock = this.m_CurrentBlock->Next;
					this.m_CurrentPtr = &this.m_CurrentBlock->Data.FixedElementField;
					this.m_CurrentBlockEnd = (byte*)(this.m_CurrentBlock + 4096 / sizeof(UnsafeStreamBlock));
					ptr = this.m_CurrentPtr;
					this.m_CurrentPtr += size;
				}
				return ptr;
			}

			// Token: 0x06000DB5 RID: 3509 RVA: 0x0002A314 File Offset: 0x00028514
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe ref T Read<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				return UnsafeUtility.AsRef<T>((void*)this.ReadUnsafePtr(num));
			}

			// Token: 0x06000DB6 RID: 3510 RVA: 0x0002A334 File Offset: 0x00028534
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe ref T Peek<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				byte* ptr = this.m_CurrentPtr;
				if (ptr + num != this.m_CurrentBlockEnd)
				{
					ptr = &this.m_CurrentBlock->Next->Data.FixedElementField;
				}
				return UnsafeUtility.AsRef<T>((void*)ptr);
			}

			// Token: 0x06000DB7 RID: 3511 RVA: 0x0002A378 File Offset: 0x00028578
			public unsafe int Count()
			{
				UnsafeStreamBlockData* ptr = (UnsafeStreamBlockData*)(void*)this.m_BlockData.Range.Pointer;
				UnsafeStreamRange* ptr2 = (UnsafeStreamRange*)(void*)ptr->Ranges.Range.Pointer;
				int num = 0;
				for (int num2 = 0; num2 != ptr->RangeCount; num2++)
				{
					num += ptr2[num2].ElementCount;
				}
				return num;
			}

			// Token: 0x04000545 RID: 1349
			[NativeDisableUnsafePtrRestriction]
			internal AllocatorManager.Block m_BlockData;

			// Token: 0x04000546 RID: 1350
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeStreamBlock* m_CurrentBlock;

			// Token: 0x04000547 RID: 1351
			[NativeDisableUnsafePtrRestriction]
			internal unsafe byte* m_CurrentPtr;

			// Token: 0x04000548 RID: 1352
			[NativeDisableUnsafePtrRestriction]
			internal unsafe byte* m_CurrentBlockEnd;

			// Token: 0x04000549 RID: 1353
			internal int m_RemainingItemCount;

			// Token: 0x0400054A RID: 1354
			internal int m_LastBlockSize;
		}
	}
}
