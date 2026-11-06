using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000032 RID: 50
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
	public struct AllocatorHelper<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType, AllocatorManager.IAllocator
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003AC9 File Offset: 0x00001CC9
		public unsafe ref T Allocator
		{
			get
			{
				return UnsafeUtility.AsRef<T>((void*)this.m_allocator);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003AD8 File Offset: 0x00001CD8
		[ExcludeFromBurstCompatTesting("CreateAllocator is unburstable")]
		public unsafe AllocatorHelper(AllocatorManager.AllocatorHandle backingAllocator, bool isGlobal = false, int globalIndex = 0)
		{
			ref T ptr = ref AllocatorManager.CreateAllocator<T>(backingAllocator, isGlobal, globalIndex);
			this.m_allocator = (T*)UnsafeUtility.AddressOf<T>(ref ptr);
			this.m_backingAllocator = backingAllocator;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003B01 File Offset: 0x00001D01
		[ExcludeFromBurstCompatTesting("DestroyAllocator is unburstable")]
		public unsafe void Dispose()
		{
			UnsafeUtility.AsRef<T>((void*)this.m_allocator).DestroyAllocator<T>(this.m_backingAllocator);
		}

		// Token: 0x04000077 RID: 119
		private unsafe readonly T* m_allocator;

		// Token: 0x04000078 RID: 120
		private AllocatorManager.AllocatorHandle m_backingAllocator;
	}
}
