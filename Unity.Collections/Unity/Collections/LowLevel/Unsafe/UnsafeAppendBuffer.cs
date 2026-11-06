using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000102 RID: 258
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeAppendBuffer : INativeDisposable, IDisposable
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x000213D6 File Offset: 0x0001F5D6
		public UnsafeAppendBuffer(int initialCapacity, int alignment, AllocatorManager.AllocatorHandle allocator)
		{
			this.Alignment = alignment;
			this.Allocator = allocator;
			this.Ptr = null;
			this.Length = 0;
			this.Capacity = 0;
			this.SetCapacity(math.max(initialCapacity, 1));
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00021409 File Offset: 0x0001F609
		public unsafe UnsafeAppendBuffer(void* ptr, int length)
		{
			this.Alignment = 0;
			this.Allocator = AllocatorManager.None;
			this.Ptr = (byte*)ptr;
			this.Length = 0;
			this.Capacity = length;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00021432 File Offset: 0x0001F632
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002143D File Offset: 0x0001F63D
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002144C File Offset: 0x0001F64C
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				Memory.Unmanaged.Free<byte>(this.Ptr, this.Allocator);
				this.Allocator = AllocatorManager.Invalid;
			}
			this.Ptr = null;
			this.Length = 0;
			this.Capacity = 0;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000214A4 File Offset: 0x0001F6A4
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

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002150E File Offset: 0x0001F70E
		public void Reset()
		{
			this.Length = 0;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00021518 File Offset: 0x0001F718
		public unsafe void SetCapacity(int capacity)
		{
			if (capacity <= this.Capacity)
			{
				return;
			}
			capacity = math.max(64, math.ceilpow2(capacity));
			byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)capacity, this.Alignment, this.Allocator);
			if (this.Ptr != null)
			{
				UnsafeUtility.MemCpy((void*)ptr, (void*)this.Ptr, (long)this.Length);
				Memory.Unmanaged.Free<byte>(this.Ptr, this.Allocator);
			}
			this.Ptr = ptr;
			this.Capacity = capacity;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002158E File Offset: 0x0001F78E
		public void ResizeUninitialized(int length)
		{
			this.SetCapacity(length);
			this.Length = length;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000215A0 File Offset: 0x0001F7A0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe void Add<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			this.SetCapacity(this.Length + num);
			void* ptr = (void*)(this.Ptr + this.Length);
			if (CollectionHelper.IsAligned(ptr, UnsafeUtility.AlignOf<T>()))
			{
				UnsafeUtility.CopyStructureToPtr<T>(ref value, ptr);
			}
			else
			{
				UnsafeUtility.MemCpy(ptr, (void*)(&value), (long)num);
			}
			this.Length += num;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00021600 File Offset: 0x0001F800
		public unsafe void Add(void* ptr, int structSize)
		{
			this.SetCapacity(this.Length + structSize);
			UnsafeUtility.MemCpy((void*)(this.Ptr + this.Length), ptr, (long)structSize);
			this.Length += structSize;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00021633 File Offset: 0x0001F833
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe void AddArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* ptr, int length) where T : struct, ValueType
		{
			this.Add<int>(length);
			if (length != 0)
			{
				this.Add(ptr, length * UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002164D File Offset: 0x0001F84D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public void Add<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> value) where T : struct, ValueType
		{
			this.Add<int>(value.Length);
			this.Add(value.GetUnsafeReadOnlyPtr<T>(), UnsafeUtility.SizeOf<T>() * value.Length);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00021678 File Offset: 0x0001F878
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe T Pop<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			long num2 = this.Ptr;
			long num3 = (long)this.Length;
			long num4 = num2 + num3 - (long)num;
			T t;
			if (CollectionHelper.IsAligned((ulong)num4, UnsafeUtility.AlignOf<T>()))
			{
				t = UnsafeUtility.ReadArrayElement<T>(num4, 0);
			}
			else
			{
				UnsafeUtility.MemCpy((void*)(&t), num4, (long)num);
			}
			this.Length -= num;
			return t;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000216D4 File Offset: 0x0001F8D4
		public unsafe void Pop(void* ptr, int structSize)
		{
			long num = this.Ptr;
			long num2 = (long)this.Length;
			long num3 = num + num2 - (long)structSize;
			UnsafeUtility.MemCpy(ptr, num3, (long)structSize);
			this.Length -= structSize;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002170E File Offset: 0x0001F90E
		public UnsafeAppendBuffer.Reader AsReader()
		{
			return new UnsafeAppendBuffer.Reader(ref this);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00021718 File Offset: 0x0001F918
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckAlignment(int alignment)
		{
			int num = ((alignment == 0) ? 1 : 0);
			bool flag = ((alignment - 1) & alignment) == 0;
			if (num != 0 || !flag)
			{
				throw new ArgumentException(string.Format("Specified alignment must be non-zero positive power of two. Requested: {0}", alignment));
			}
		}

		// Token: 0x0400049C RID: 1180
		[NativeDisableUnsafePtrRestriction]
		public unsafe byte* Ptr;

		// Token: 0x0400049D RID: 1181
		public int Length;

		// Token: 0x0400049E RID: 1182
		public int Capacity;

		// Token: 0x0400049F RID: 1183
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x040004A0 RID: 1184
		public readonly int Alignment;

		// Token: 0x02000103 RID: 259
		[GenerateTestsForBurstCompatibility]
		public struct Reader
		{
			// Token: 0x06000B01 RID: 2817 RVA: 0x0002174F File Offset: 0x0001F94F
			public Reader(ref UnsafeAppendBuffer buffer)
			{
				this.Ptr = buffer.Ptr;
				this.Size = buffer.Length;
				this.Offset = 0;
			}

			// Token: 0x06000B02 RID: 2818 RVA: 0x00021770 File Offset: 0x0001F970
			public unsafe Reader(void* ptr, int length)
			{
				this.Ptr = (byte*)ptr;
				this.Size = length;
				this.Offset = 0;
			}

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00021787 File Offset: 0x0001F987
			public bool EndOfBuffer
			{
				get
				{
					return this.Offset == this.Size;
				}
			}

			// Token: 0x06000B04 RID: 2820 RVA: 0x00021798 File Offset: 0x0001F998
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void ReadNext<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out T value) where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				void* ptr = (void*)(this.Ptr + this.Offset);
				if (CollectionHelper.IsAligned(ptr, UnsafeUtility.AlignOf<T>()))
				{
					UnsafeUtility.CopyPtrToStructure<T>(ptr, out value);
				}
				else
				{
					fixed (T* ptr2 = &value)
					{
						void* ptr3 = (void*)ptr2;
						UnsafeUtility.MemCpy(ptr3, ptr, (long)num);
					}
				}
				this.Offset += num;
			}

			// Token: 0x06000B05 RID: 2821 RVA: 0x000217F0 File Offset: 0x0001F9F0
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe T ReadNext<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				int num = UnsafeUtility.SizeOf<T>();
				void* ptr = (void*)(this.Ptr + this.Offset);
				T t;
				if (CollectionHelper.IsAligned(ptr, UnsafeUtility.AlignOf<T>()))
				{
					t = UnsafeUtility.ReadArrayElement<T>(ptr, 0);
				}
				else
				{
					UnsafeUtility.MemCpy((void*)(&t), ptr, (long)num);
				}
				this.Offset += num;
				return t;
			}

			// Token: 0x06000B06 RID: 2822 RVA: 0x00021843 File Offset: 0x0001FA43
			public unsafe void* ReadNext(int structSize)
			{
				void* ptr = (void*)((IntPtr)((void*)this.Ptr) + this.Offset);
				this.Offset += structSize;
				return ptr;
			}

			// Token: 0x06000B07 RID: 2823 RVA: 0x00021870 File Offset: 0x0001FA70
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void ReadNext<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out NativeArray<T> value, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
			{
				int num = this.ReadNext<int>();
				value = CollectionHelper.CreateNativeArray<T>(num, allocator, NativeArrayOptions.UninitializedMemory);
				int num2 = num * UnsafeUtility.SizeOf<T>();
				if (num2 > 0)
				{
					void* ptr = this.ReadNext(num2);
					UnsafeUtility.MemCpy(value.GetUnsafePtr<T>(), ptr, (long)num2);
				}
			}

			// Token: 0x06000B08 RID: 2824 RVA: 0x000218B9 File Offset: 0x0001FAB9
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void* ReadNextArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out int length) where T : struct, ValueType
			{
				length = this.ReadNext<int>();
				if (length != 0)
				{
					return this.ReadNext(length * UnsafeUtility.SizeOf<T>());
				}
				return null;
			}

			// Token: 0x06000B09 RID: 2825 RVA: 0x000218D8 File Offset: 0x0001FAD8
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckBounds(int structSize)
			{
				if (this.Offset + structSize > this.Size)
				{
					throw new ArgumentException(string.Format("Requested value outside bounds of UnsafeAppendOnlyBuffer. Remaining bytes: {0} Requested: {1}", this.Size - this.Offset, structSize));
				}
			}

			// Token: 0x040004A1 RID: 1185
			public unsafe readonly byte* Ptr;

			// Token: 0x040004A2 RID: 1186
			public readonly int Size;

			// Token: 0x040004A3 RID: 1187
			public int Offset;
		}
	}
}
