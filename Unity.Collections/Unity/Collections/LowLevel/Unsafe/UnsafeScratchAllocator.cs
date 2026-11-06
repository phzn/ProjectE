using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200013E RID: 318
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeScratchAllocator
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x00029932 File Offset: 0x00027B32
		public unsafe UnsafeScratchAllocator(void* ptr, int capacityInBytes)
		{
			this.m_Pointer = ptr;
			this.m_LengthInBytes = 0;
			this.m_CapacityInBytes = capacityInBytes;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00029949 File Offset: 0x00027B49
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckAllocationDoesNotExceedCapacity(ulong requestedSize)
		{
			if (requestedSize > (ulong)((long)this.m_CapacityInBytes))
			{
				throw new ArgumentException(string.Format("Cannot allocate more than provided size in UnsafeScratchAllocator. Requested: {0} Size: {1} Capacity: {2}", requestedSize, this.m_LengthInBytes, this.m_CapacityInBytes));
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00029984 File Offset: 0x00027B84
		public unsafe void* Allocate(int sizeInBytes, int alignmentInBytes)
		{
			if (sizeInBytes == 0)
			{
				return null;
			}
			ulong num = (ulong)((long)(alignmentInBytes - 1));
			long num2 = ((long)((IntPtr)this.m_Pointer) + (long)this.m_LengthInBytes + (long)num) & (long)(~(long)num);
			long num3 = (long)((byte*)(void*)((IntPtr)num2) - (byte*)this.m_Pointer);
			num3 += (long)sizeInBytes;
			this.m_LengthInBytes = (int)num3;
			return (void*)((IntPtr)num2);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000299E6 File Offset: 0x00027BE6
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe void* Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int count = 1) where T : struct, ValueType
		{
			return this.Allocate(UnsafeUtility.SizeOf<T>() * count, UnsafeUtility.AlignOf<T>());
		}

		// Token: 0x04000524 RID: 1316
		private unsafe void* m_Pointer;

		// Token: 0x04000525 RID: 1317
		private int m_LengthInBytes;

		// Token: 0x04000526 RID: 1318
		private readonly int m_CapacityInBytes;
	}
}
