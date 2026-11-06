using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000120 RID: 288
	[GenerateTestsForBurstCompatibility]
	internal static class UnsafePtrListExtensions
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x000247B8 File Offset: 0x000229B8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref UnsafeList<IntPtr> ListData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafePtrList<T> from) where T : struct, ValueType
		{
			return UnsafeUtility.As<UnsafePtrList<T>, UnsafeList<IntPtr>>(ref from);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x000247C0 File Offset: 0x000229C0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeList<IntPtr> ListDataRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafePtrList<T> from) where T : struct, ValueType
		{
			return *UnsafeUtility.As<UnsafePtrList<T>, UnsafeList<IntPtr>>(ref from);
		}
	}
}
