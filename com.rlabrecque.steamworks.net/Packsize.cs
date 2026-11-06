using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000181 RID: 385
	public static class Packsize
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		public static bool Test()
		{
			int num = Marshal.SizeOf<Packsize.ValvePackingSentinel_t>();
			int num2 = Marshal.SizeOf<RemoteStorageEnumerateUserSubscribedFilesResult_t>();
			return num == 32 && num2 == 616;
		}

		// Token: 0x04000B31 RID: 2865
		public const int value = 8;

		// Token: 0x020001E7 RID: 487
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			// Token: 0x04000C20 RID: 3104
			private uint m_u32;

			// Token: 0x04000C21 RID: 3105
			private ulong m_u64;

			// Token: 0x04000C22 RID: 3106
			private ushort m_u16;

			// Token: 0x04000C23 RID: 3107
			private double m_d;
		}
	}
}
