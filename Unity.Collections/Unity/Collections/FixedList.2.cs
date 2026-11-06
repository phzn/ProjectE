using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x0200004D RID: 77
	[GenerateTestsForBurstCompatibility]
	internal struct FixedList
	{
		// Token: 0x0600021E RID: 542 RVA: 0x00006EC2 File Offset: 0x000050C2
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int PaddingBytes<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			return math.max(0, math.min(6, (1 << math.tzcnt(UnsafeUtility.SizeOf<T>())) - 2));
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006EE1 File Offset: 0x000050E1
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int StorageBytes<[global::System.Runtime.CompilerServices.IsUnmanaged] BUFFER, [global::System.Runtime.CompilerServices.IsUnmanaged] T>() where BUFFER : struct, ValueType where T : struct, ValueType
		{
			return UnsafeUtility.SizeOf<BUFFER>() - UnsafeUtility.SizeOf<ushort>() - FixedList.PaddingBytes<T>();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006EF4 File Offset: 0x000050F4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int Capacity<[global::System.Runtime.CompilerServices.IsUnmanaged] BUFFER, [global::System.Runtime.CompilerServices.IsUnmanaged] T>() where BUFFER : struct, ValueType where T : struct, ValueType
		{
			return FixedList.StorageBytes<BUFFER, T>() / UnsafeUtility.SizeOf<T>();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006F04 File Offset: 0x00005104
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckResize<[global::System.Runtime.CompilerServices.IsUnmanaged] BUFFER, [global::System.Runtime.CompilerServices.IsUnmanaged] T>(int newLength) where BUFFER : struct, ValueType where T : struct, ValueType
		{
			int num = FixedList.Capacity<BUFFER, T>();
			if (newLength < 0 || newLength > num)
			{
				throw new IndexOutOfRangeException(string.Format("NewLength {0} is out of range of '{1}' Capacity.", newLength, num));
			}
		}
	}
}
