using System;

namespace Unity.Burst.CompilerServices
{
	// Token: 0x02000056 RID: 86
	public static class Aliasing
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectAliased(void* a, void* b)
		{
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00002551 File Offset: 0x00000751
		public static void ExpectAliased<A, B>(in A a, in B b) where A : struct where B : struct
		{
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectAliased<B>(void* a, in B b) where B : struct
		{
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectAliased<A>(in A a, void* b) where A : struct
		{
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectNotAliased(void* a, void* b)
		{
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00002551 File Offset: 0x00000751
		public static void ExpectNotAliased<A, B>(in A a, in B b) where A : struct where B : struct
		{
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectNotAliased<B>(void* a, in B b) where B : struct
		{
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00002551 File Offset: 0x00000751
		public unsafe static void ExpectNotAliased<A>(in A a, void* b) where A : struct
		{
		}
	}
}
