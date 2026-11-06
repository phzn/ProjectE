using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000149 RID: 329
	internal static class UnsafeTextExtensions
	{
		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002A3D8 File Offset: 0x000285D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref UnsafeList<byte> AsUnsafeListOfBytes(this UnsafeText text)
		{
			return UnsafeUtility.As<UntypedUnsafeList, UnsafeList<byte>>(ref text.m_UntypedListData);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002A3E5 File Offset: 0x000285E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeList<byte> AsUnsafeListOfBytesRO(this UnsafeText text)
		{
			return *UnsafeUtility.As<UntypedUnsafeList, UnsafeList<byte>>(ref text.m_UntypedListData);
		}
	}
}
