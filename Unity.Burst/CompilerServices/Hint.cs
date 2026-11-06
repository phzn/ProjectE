using System;

namespace Unity.Burst.CompilerServices
{
	// Token: 0x02000059 RID: 89
	public static class Hint
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x000061CE File Offset: 0x000043CE
		public static bool Likely(bool condition)
		{
			return condition;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000061CE File Offset: 0x000043CE
		public static bool Unlikely(bool condition)
		{
			return condition;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00002551 File Offset: 0x00000751
		public static void Assume(bool condition)
		{
		}
	}
}
