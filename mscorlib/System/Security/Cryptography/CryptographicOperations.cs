using System;
using System.Runtime.CompilerServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000470 RID: 1136
	public static class CryptographicOperations
	{
		// Token: 0x06002DFF RID: 11775 RVA: 0x000A59BC File Offset: 0x000A3BBC
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public unsafe static bool FixedTimeEquals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
		{
			if (left.Length != right.Length)
			{
				return false;
			}
			int length = left.Length;
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num |= (int)(*left[i] - *right[i]);
			}
			return num == 0;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000A5A0B File Offset: 0x000A3C0B
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void ZeroMemory(Span<byte> buffer)
		{
			buffer.Clear();
		}
	}
}
