using System;
using System.Runtime.CompilerServices;
using AOT;
using Unity.Burst;

namespace Unity.Collections
{
	// Token: 0x02000034 RID: 52
	[BurstCompile]
	internal struct AutoFreeAllocator : AllocatorManager.IAllocator, IDisposable
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00003EB8 File Offset: 0x000020B8
		public unsafe void Update()
		{
			int length = this.m_tofree.Length;
			while (length-- > 0)
			{
				int length2 = this.m_allocated.Length;
				while (length2-- > 0)
				{
					if (*this.m_allocated[length2] == *this.m_tofree[length])
					{
						Memory.Unmanaged.Free((void*)(*this.m_tofree[length]), this.m_backingAllocatorHandle);
						this.m_allocated.RemoveAtSwapBack(length2);
						break;
					}
				}
			}
			this.m_tofree.Rewind();
			this.m_allocated.TrimExcess();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003F53 File Offset: 0x00002153
		public void Initialize(AllocatorManager.AllocatorHandle backingAllocatorHandle)
		{
			this.m_allocated = new ArrayOfArrays<IntPtr>(1048576, backingAllocatorHandle, 12);
			this.m_tofree = new ArrayOfArrays<IntPtr>(131072, backingAllocatorHandle, 12);
			this.m_backingAllocatorHandle = backingAllocatorHandle;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00003F84 File Offset: 0x00002184
		public unsafe void FreeAll()
		{
			this.Update();
			this.m_handle.Rewind();
			for (int i = 0; i < this.m_allocated.Length; i++)
			{
				Memory.Unmanaged.Free((void*)(*this.m_allocated[i]), this.m_backingAllocatorHandle);
			}
			this.m_allocated.Rewind();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00003FE0 File Offset: 0x000021E0
		public void Dispose()
		{
			this.FreeAll();
			this.m_tofree.Dispose();
			this.m_allocated.Dispose();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003FFE File Offset: 0x000021FE
		public AllocatorManager.TryFunction Function
		{
			get
			{
				return new AllocatorManager.TryFunction(AutoFreeAllocator.Try);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000400C File Offset: 0x0000220C
		public unsafe int Try(ref AllocatorManager.Block block)
		{
			if (block.Range.Pointer == IntPtr.Zero)
			{
				if (block.Bytes == 0L)
				{
					return 0;
				}
				byte* ptr = (byte*)Memory.Unmanaged.Allocate(block.Bytes, block.Alignment, this.m_backingAllocatorHandle);
				block.Range.Pointer = (IntPtr)((void*)ptr);
				block.AllocatedItems = block.Range.Items;
				this.m_allocated.LockfreeAdd(block.Range.Pointer);
				return 0;
			}
			else
			{
				if (block.Range.Items == 0)
				{
					this.m_tofree.LockfreeAdd(block.Range.Pointer);
					block.Range.Pointer = IntPtr.Zero;
					block.AllocatedItems = 0;
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000040C9 File Offset: 0x000022C9
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		internal static int Try(IntPtr state, ref AllocatorManager.Block block)
		{
			return AutoFreeAllocator.Try_000000E3$BurstDirectCall.Invoke(state, ref block);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000040D2 File Offset: 0x000022D2
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000040DA File Offset: 0x000022DA
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000040E3 File Offset: 0x000022E3
		public Allocator ToAllocator
		{
			get
			{
				return this.m_handle.ToAllocator;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000040F0 File Offset: 0x000022F0
		public bool IsCustomAllocator
		{
			get
			{
				return this.m_handle.IsCustomAllocator;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000040FD File Offset: 0x000022FD
		public bool IsAutoDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004100 File Offset: 0x00002300
		[BurstCompile]
		[MonoPInvokeCallback(typeof(AllocatorManager.TryFunction))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Try$BurstManaged(IntPtr state, ref AllocatorManager.Block block)
		{
			return ((AutoFreeAllocator*)(void*)state)->Try(ref block);
		}

		// Token: 0x0400007F RID: 127
		private ArrayOfArrays<IntPtr> m_allocated;

		// Token: 0x04000080 RID: 128
		private ArrayOfArrays<IntPtr> m_tofree;

		// Token: 0x04000081 RID: 129
		private AllocatorManager.AllocatorHandle m_handle;

		// Token: 0x04000082 RID: 130
		private AllocatorManager.AllocatorHandle m_backingAllocatorHandle;

		// Token: 0x02000035 RID: 53
		// (Invoke) Token: 0x06000103 RID: 259
		internal delegate int Try_000000E3$PostfixBurstDelegate(IntPtr state, ref AllocatorManager.Block block);

		// Token: 0x02000036 RID: 54
		internal static class Try_000000E3$BurstDirectCall
		{
			// Token: 0x06000106 RID: 262 RVA: 0x0000410E File Offset: 0x0000230E
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (AutoFreeAllocator.Try_000000E3$BurstDirectCall.Pointer == 0)
				{
					AutoFreeAllocator.Try_000000E3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(AutoFreeAllocator.Try_000000E3$BurstDirectCall.DeferredCompilation, methodof(AutoFreeAllocator.Try$BurstManaged(IntPtr, ref AllocatorManager.Block)).MethodHandle, typeof(AutoFreeAllocator.Try_000000E3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = AutoFreeAllocator.Try_000000E3$BurstDirectCall.Pointer;
			}

			// Token: 0x06000107 RID: 263 RVA: 0x0000413C File Offset: 0x0000233C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				AutoFreeAllocator.Try_000000E3$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000108 RID: 264 RVA: 0x00004154 File Offset: 0x00002354
			public static void Constructor()
			{
				AutoFreeAllocator.Try_000000E3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(AutoFreeAllocator.Try(IntPtr, ref AllocatorManager.Block)).MethodHandle);
			}

			// Token: 0x06000109 RID: 265 RVA: 0x00002C47 File Offset: 0x00000E47
			public static void Initialize()
			{
			}

			// Token: 0x0600010A RID: 266 RVA: 0x00004165 File Offset: 0x00002365
			// Note: this type is marked as 'beforefieldinit'.
			static Try_000000E3$BurstDirectCall()
			{
				AutoFreeAllocator.Try_000000E3$BurstDirectCall.Constructor();
			}

			// Token: 0x0600010B RID: 267 RVA: 0x0000416C File Offset: 0x0000236C
			public static int Invoke(IntPtr state, ref AllocatorManager.Block block)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = AutoFreeAllocator.Try_000000E3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.IntPtr,Unity.Collections.AllocatorManager/Block&), state, ref block, functionPointer);
					}
				}
				return AutoFreeAllocator.Try$BurstManaged(state, ref block);
			}

			// Token: 0x04000083 RID: 131
			private static IntPtr Pointer;

			// Token: 0x04000084 RID: 132
			private static IntPtr DeferredCompilation;
		}
	}
}
