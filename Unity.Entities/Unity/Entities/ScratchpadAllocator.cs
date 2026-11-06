using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000277 RID: 631
	[BurstCompile]
	[GenerateTestsForBurstCompatibility]
	public struct ScratchpadAllocator : AllocatorManager.IAllocator, IDisposable
	{
		// Token: 0x060010EA RID: 4330 RVA: 0x000480EC File Offset: 0x000462EC
		public unsafe int Try(ref AllocatorManager.Block block)
		{
			if (block.Range.Pointer == IntPtr.Zero)
			{
				int next = this.m_next;
				int num = 63;
				long num2 = (block.Bytes + (long)num) & (long)(~(long)num);
				if ((long)this.m_next + num2 > (long)this.m_bytes)
				{
					throw new InvalidOperationException(string.Format("ScratchpadAllocator: Request {0} bytes on top of current {1} bytes which exceeds max available {2}.", block.Bytes, this.m_next, this.m_bytes));
				}
				this.m_next += (int)num2;
				block.Range.Pointer = (IntPtr)((void*)(this.m_pointer + next));
				return 0;
			}
			else
			{
				if (block.Range.Items == 0)
				{
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x000481A7 File Offset: 0x000463A7
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		internal static int Try(IntPtr state, ref AllocatorManager.Block block)
		{
			return ScratchpadAllocator.Try_00000F31$BurstDirectCall.Invoke(state, ref block);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000481B0 File Offset: 0x000463B0
		[BurstCompile]
		public void Rewind()
		{
			this.m_handle.Rewind();
			this.m_next = 0;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x000481C4 File Offset: 0x000463C4
		[ExcludeFromBurstCompatTesting("Returns managed delegate")]
		public AllocatorManager.TryFunction Function
		{
			get
			{
				return new AllocatorManager.TryFunction(ScratchpadAllocator.Try);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x000481D2 File Offset: 0x000463D2
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x000481DA File Offset: 0x000463DA
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x000481E3 File Offset: 0x000463E3
		public Allocator ToAllocator
		{
			get
			{
				return this.m_handle.ToAllocator;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x000481F0 File Offset: 0x000463F0
		public bool IsCustomAllocator
		{
			get
			{
				return this.m_handle.IsCustomAllocator;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x000106DF File Offset: 0x0000E8DF
		public bool IsAutoDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x000481FD File Offset: 0x000463FD
		public void Dispose()
		{
			this.Rewind();
			Memory.Unmanaged.Free<byte>(this.m_pointer, Allocator.Persistent);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00048216 File Offset: 0x00046416
		public unsafe void Initialize(int bytes)
		{
			this.m_pointer = (byte*)Memory.Unmanaged.Allocate((long)bytes, 16384, Allocator.Persistent);
			this.m_bytes = bytes;
			this.m_next = 0;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004823E File Offset: 0x0004643E
		public int GetAvailableBytes()
		{
			return this.m_bytes - this.m_next;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00048250 File Offset: 0x00046450
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe NativeArray<T> AllocateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int length) where T : struct, ValueType
		{
			return new NativeArray<T>
			{
				m_Buffer = (void*)(ref this).Allocate(default(T), length),
				m_Length = length,
				m_AllocatorLabel = this.ToAllocator
			};
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00048294 File Offset: 0x00046494
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe NativeList<T> AllocateNativeList<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int capacity) where T : struct, ValueType
		{
			NativeList<T> nativeList = default(NativeList<T>);
			nativeList.m_ListData = (ref this).Allocate(default(UnsafeList<T>), 1);
			nativeList.m_ListData->Ptr = (ref this).Allocate(default(T), capacity);
			nativeList.m_ListData->m_length = 0;
			nativeList.m_ListData->m_capacity = capacity;
			nativeList.m_ListData->Allocator = this.Handle;
			return nativeList;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00048305 File Offset: 0x00046505
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Try$BurstManaged(IntPtr state, ref AllocatorManager.Block block)
		{
			return ((ScratchpadAllocator*)(void*)state)->Try(ref block);
		}

		// Token: 0x0400067D RID: 1661
		private const int kMaximumAlignment = 16384;

		// Token: 0x0400067E RID: 1662
		private unsafe byte* m_pointer;

		// Token: 0x0400067F RID: 1663
		internal AllocatorManager.AllocatorHandle m_handle;

		// Token: 0x04000680 RID: 1664
		internal int m_bytes;

		// Token: 0x04000681 RID: 1665
		internal int m_next;

		// Token: 0x02000278 RID: 632
		// (Invoke) Token: 0x060010FA RID: 4346
		internal delegate int Try_00000F31$PostfixBurstDelegate(IntPtr state, ref AllocatorManager.Block block);

		// Token: 0x02000279 RID: 633
		internal static class Try_00000F31$BurstDirectCall
		{
			// Token: 0x060010FD RID: 4349 RVA: 0x00048313 File Offset: 0x00046513
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ScratchpadAllocator.Try_00000F31$BurstDirectCall.Pointer == 0)
				{
					ScratchpadAllocator.Try_00000F31$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ScratchpadAllocator.Try_00000F31$BurstDirectCall.DeferredCompilation, methodof(ScratchpadAllocator.Try$BurstManaged(IntPtr, ref AllocatorManager.Block)).MethodHandle, typeof(ScratchpadAllocator.Try_00000F31$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ScratchpadAllocator.Try_00000F31$BurstDirectCall.Pointer;
			}

			// Token: 0x060010FE RID: 4350 RVA: 0x00048340 File Offset: 0x00046540
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ScratchpadAllocator.Try_00000F31$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060010FF RID: 4351 RVA: 0x00048358 File Offset: 0x00046558
			public static void Constructor()
			{
				ScratchpadAllocator.Try_00000F31$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ScratchpadAllocator.Try(IntPtr, ref AllocatorManager.Block)).MethodHandle);
			}

			// Token: 0x06001100 RID: 4352 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001101 RID: 4353 RVA: 0x00048369 File Offset: 0x00046569
			// Note: this type is marked as 'beforefieldinit'.
			static Try_00000F31$BurstDirectCall()
			{
				ScratchpadAllocator.Try_00000F31$BurstDirectCall.Constructor();
			}

			// Token: 0x06001102 RID: 4354 RVA: 0x00048370 File Offset: 0x00046570
			public static int Invoke(IntPtr state, ref AllocatorManager.Block block)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ScratchpadAllocator.Try_00000F31$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.IntPtr,Unity.Collections.AllocatorManager/Block&), state, ref block, functionPointer);
					}
				}
				return ScratchpadAllocator.Try$BurstManaged(state, ref block);
			}

			// Token: 0x04000682 RID: 1666
			private static IntPtr Pointer;

			// Token: 0x04000683 RID: 1667
			private static IntPtr DeferredCompilation;
		}
	}
}
