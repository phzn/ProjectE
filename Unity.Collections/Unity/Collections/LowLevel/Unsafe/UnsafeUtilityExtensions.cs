using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200014B RID: 331
	[GenerateTestsForBurstCompatibility]
	public static class UnsafeUtilityExtensions
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x0002A674 File Offset: 0x00028874
		internal unsafe static void MemSwap(void* ptr, void* otherPtr, long size)
		{
			byte* ptr2 = (byte*)ptr;
			byte* ptr3 = (byte*)otherPtr;
			byte* ptr4 = stackalloc byte[(UIntPtr)1024];
			while (size > 0L)
			{
				long num = math.min(size, 1024L);
				UnsafeUtility.MemCpy((void*)ptr4, (void*)ptr2, num);
				UnsafeUtility.MemCpy((void*)ptr2, (void*)ptr3, num);
				UnsafeUtility.MemCpy((void*)ptr3, (void*)ptr4, num);
				size -= num;
				ptr3 += num;
				ptr2 += num;
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002A6C9 File Offset: 0x000288C9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T ReadArrayElementBoundsChecked<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* source, int index, int capacity) where T : struct, ValueType
		{
			return UnsafeUtility.ReadArrayElement<T>(source, index);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002A6D2 File Offset: 0x000288D2
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void WriteArrayElementBoundsChecked<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* destination, int index, T value, int capacity) where T : struct, ValueType
		{
			UnsafeUtility.WriteArrayElement<T>(destination, index, value);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002A6DC File Offset: 0x000288DC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* AddressOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in T value) where T : struct, ValueType
		{
			return ILSupport.AddressOf<T>(in value);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002A6E4 File Offset: 0x000288E4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T AsRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in T value) where T : struct, ValueType
		{
			return ILSupport.AsRef<T>(in value);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002A6EC File Offset: 0x000288EC
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe static void CheckMemSwapOverlap(byte* dst, byte* src, long size)
		{
			if (dst + size != src && src + size != dst)
			{
				throw new InvalidOperationException("MemSwap memory blocks are overlapped.");
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002A707 File Offset: 0x00028907
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckIndexRange(int index, int capacity)
		{
			if (index > capacity - 1 || index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Attempt to read or write from array index {0}, which is out of bounds. Array capacity is {1}. ", index, capacity) + "This may lead to a crash, data corruption, or reading invalid data.");
			}
		}
	}
}
