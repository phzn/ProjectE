using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000CB RID: 203
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	public struct NativeStream : INativeDisposable, IDisposable
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0001C1FA File Offset: 0x0001A3FA
		public NativeStream(int bufferCount, AllocatorManager.AllocatorHandle allocator)
		{
			NativeStream.AllocateBlock(out this, allocator);
			this.m_Stream.AllocateForEach(bufferCount);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001C210 File Offset: 0x0001A410
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static JobHandle ScheduleConstruct<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out NativeStream stream, NativeList<T> bufferCount, JobHandle dependency, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeStream.AllocateBlock(out stream, allocator);
			return new NativeStream.ConstructJobList
			{
				List = (UntypedUnsafeList*)bufferCount.GetUnsafeList(),
				Container = stream
			}.Schedule(dependency);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001C250 File Offset: 0x0001A450
		public static JobHandle ScheduleConstruct(out NativeStream stream, NativeArray<int> bufferCount, JobHandle dependency, AllocatorManager.AllocatorHandle allocator)
		{
			NativeStream.AllocateBlock(out stream, allocator);
			return new NativeStream.ConstructJob
			{
				Length = bufferCount,
				Container = stream
			}.Schedule(dependency);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001C288 File Offset: 0x0001A488
		public readonly bool IsEmpty()
		{
			return this.m_Stream.IsEmpty();
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0001C295 File Offset: 0x0001A495
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Stream.IsCreated;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0001C2A2 File Offset: 0x0001A4A2
		public readonly int ForEachCount
		{
			get
			{
				return this.m_Stream.ForEachCount;
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001C2AF File Offset: 0x0001A4AF
		public NativeStream.Reader AsReader()
		{
			return new NativeStream.Reader(ref this);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001C2B7 File Offset: 0x0001A4B7
		public NativeStream.Writer AsWriter()
		{
			return new NativeStream.Writer(ref this);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001C2BF File Offset: 0x0001A4BF
		public int Count()
		{
			return this.m_Stream.Count();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001C2CC File Offset: 0x0001A4CC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public NativeArray<T> ToNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			return this.m_Stream.ToNativeArray<T>(allocator);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001C2DA File Offset: 0x0001A4DA
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.m_Stream.Dispose();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeStreamDisposeJob
			{
				Data = new NativeStreamDispose
				{
					m_StreamData = this.m_Stream
				}
			}.Schedule(inputDeps);
			this.m_Stream = default(UnsafeStream);
			return jobHandle;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001C33F File Offset: 0x0001A53F
		private static void AllocateBlock(out NativeStream stream, AllocatorManager.AllocatorHandle allocator)
		{
			UnsafeStream.AllocateBlock(out stream.m_Stream, allocator);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001C34D File Offset: 0x0001A54D
		private void AllocateForEach(int forEachCount)
		{
			this.m_Stream.AllocateForEach(forEachCount);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001C35B File Offset: 0x0001A55B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckForEachCountGreaterThanZero(int forEachCount)
		{
			if (forEachCount <= 0)
			{
				throw new ArgumentException("foreachCount must be > 0", "foreachCount");
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private readonly void CheckRead()
		{
		}

		// Token: 0x04000406 RID: 1030
		private UnsafeStream m_Stream;

		// Token: 0x020000CC RID: 204
		[BurstCompile]
		private struct ConstructJobList : IJob
		{
			// Token: 0x06000958 RID: 2392 RVA: 0x0001C371 File Offset: 0x0001A571
			public unsafe void Execute()
			{
				this.Container.AllocateForEach(this.List->m_length);
			}

			// Token: 0x04000407 RID: 1031
			public NativeStream Container;

			// Token: 0x04000408 RID: 1032
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe UntypedUnsafeList* List;
		}

		// Token: 0x020000CD RID: 205
		[BurstCompile]
		private struct ConstructJob : IJob
		{
			// Token: 0x06000959 RID: 2393 RVA: 0x0001C389 File Offset: 0x0001A589
			public void Execute()
			{
				this.Container.AllocateForEach(this.Length[0]);
			}

			// Token: 0x04000409 RID: 1033
			public NativeStream Container;

			// Token: 0x0400040A RID: 1034
			[ReadOnly]
			public NativeArray<int> Length;
		}

		// Token: 0x020000CE RID: 206
		[NativeContainer]
		[NativeContainerSupportsMinMaxWriteRestriction]
		[GenerateTestsForBurstCompatibility]
		public struct Writer
		{
			// Token: 0x0600095A RID: 2394 RVA: 0x0001C3A2 File Offset: 0x0001A5A2
			internal Writer(ref NativeStream stream)
			{
				this.m_Writer = stream.m_Stream.AsWriter();
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x0600095B RID: 2395 RVA: 0x0001C3B5 File Offset: 0x0001A5B5
			public int ForEachCount
			{
				get
				{
					return this.m_Writer.ForEachCount;
				}
			}

			// Token: 0x0600095C RID: 2396 RVA: 0x00002C47 File Offset: 0x00000E47
			public void PatchMinMaxRange(int foreEachIndex)
			{
			}

			// Token: 0x0600095D RID: 2397 RVA: 0x0001C3C2 File Offset: 0x0001A5C2
			public void BeginForEachIndex(int foreachIndex)
			{
				this.m_Writer.BeginForEachIndex(foreachIndex);
			}

			// Token: 0x0600095E RID: 2398 RVA: 0x0001C3D0 File Offset: 0x0001A5D0
			public void EndForEachIndex()
			{
				this.m_Writer.EndForEachIndex();
			}

			// Token: 0x0600095F RID: 2399 RVA: 0x0001C3DD File Offset: 0x0001A5DD
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void Write<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType
			{
				*this.Allocate<T>() = value;
			}

			// Token: 0x06000960 RID: 2400 RVA: 0x0001C3EC File Offset: 0x0001A5EC
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe ref T Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				return UnsafeUtility.AsRef<T>((void*)this.Allocate(num));
			}

			// Token: 0x06000961 RID: 2401 RVA: 0x0001C40B File Offset: 0x0001A60B
			public unsafe byte* Allocate(int size)
			{
				return this.m_Writer.Allocate(size);
			}

			// Token: 0x06000962 RID: 2402 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckBeginForEachIndex(int foreachIndex)
			{
			}

			// Token: 0x06000963 RID: 2403 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckEndForEachIndex()
			{
			}

			// Token: 0x06000964 RID: 2404 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckAllocateSize(int size)
			{
			}

			// Token: 0x0400040B RID: 1035
			private UnsafeStream.Writer m_Writer;
		}

		// Token: 0x020000CF RID: 207
		[NativeContainer]
		[NativeContainerIsReadOnly]
		[GenerateTestsForBurstCompatibility]
		public struct Reader
		{
			// Token: 0x06000965 RID: 2405 RVA: 0x0001C419 File Offset: 0x0001A619
			internal Reader(ref NativeStream stream)
			{
				this.m_Reader = stream.m_Stream.AsReader();
			}

			// Token: 0x06000966 RID: 2406 RVA: 0x0001C42C File Offset: 0x0001A62C
			public int BeginForEachIndex(int foreachIndex)
			{
				return this.m_Reader.BeginForEachIndex(foreachIndex);
			}

			// Token: 0x06000967 RID: 2407 RVA: 0x0001C43A File Offset: 0x0001A63A
			public void EndForEachIndex()
			{
				this.m_Reader.EndForEachIndex();
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000968 RID: 2408 RVA: 0x0001C447 File Offset: 0x0001A647
			public int ForEachCount
			{
				get
				{
					return this.m_Reader.ForEachCount;
				}
			}

			// Token: 0x17000110 RID: 272
			// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001C454 File Offset: 0x0001A654
			public int RemainingItemCount
			{
				get
				{
					return this.m_Reader.RemainingItemCount;
				}
			}

			// Token: 0x0600096A RID: 2410 RVA: 0x0001C464 File Offset: 0x0001A664
			public unsafe byte* ReadUnsafePtr(int size)
			{
				this.m_Reader.m_RemainingItemCount = this.m_Reader.m_RemainingItemCount - 1;
				byte* ptr = this.m_Reader.m_CurrentPtr;
				this.m_Reader.m_CurrentPtr = this.m_Reader.m_CurrentPtr + size;
				if (this.m_Reader.m_CurrentPtr != this.m_Reader.m_CurrentBlockEnd)
				{
					this.m_Reader.m_CurrentBlock = this.m_Reader.m_CurrentBlock->Next;
					this.m_Reader.m_CurrentPtr = &this.m_Reader.m_CurrentBlock->Data.FixedElementField;
					this.m_Reader.m_CurrentBlockEnd = (byte*)(this.m_Reader.m_CurrentBlock + 4096 / sizeof(UnsafeStreamBlock));
					ptr = this.m_Reader.m_CurrentPtr;
					this.m_Reader.m_CurrentPtr = this.m_Reader.m_CurrentPtr + size;
				}
				return ptr;
			}

			// Token: 0x0600096B RID: 2411 RVA: 0x0001C52C File Offset: 0x0001A72C
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe ref T Read<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				return UnsafeUtility.AsRef<T>((void*)this.ReadUnsafePtr(num));
			}

			// Token: 0x0600096C RID: 2412 RVA: 0x0001C54B File Offset: 0x0001A74B
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public ref T Peek<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				UnsafeUtility.SizeOf<T>();
				return this.m_Reader.Peek<T>();
			}

			// Token: 0x0600096D RID: 2413 RVA: 0x0001C55E File Offset: 0x0001A75E
			public int Count()
			{
				return this.m_Reader.Count();
			}

			// Token: 0x0600096E RID: 2414 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckNotReadingOutOfBounds(int size)
			{
			}

			// Token: 0x0600096F RID: 2415 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private void CheckRead()
			{
			}

			// Token: 0x06000970 RID: 2416 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckReadSize(int size)
			{
			}

			// Token: 0x06000971 RID: 2417 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckBeginForEachIndex(int forEachIndex)
			{
			}

			// Token: 0x06000972 RID: 2418 RVA: 0x0001C56B File Offset: 0x0001A76B
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckEndForEachIndex()
			{
				if (this.m_Reader.m_RemainingItemCount != 0)
				{
					throw new ArgumentException("Not all elements (Count) have been read. If this is intentional, simply skip calling EndForEachIndex();");
				}
				if (this.m_Reader.m_CurrentBlockEnd != this.m_Reader.m_CurrentPtr)
				{
					throw new ArgumentException("Not all data (Data Size) has been read. If this is intentional, simply skip calling EndForEachIndex();");
				}
			}

			// Token: 0x0400040C RID: 1036
			private UnsafeStream.Reader m_Reader;
		}
	}
}
