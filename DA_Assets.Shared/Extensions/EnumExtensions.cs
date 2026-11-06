using System;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002B RID: 43
	public static class EnumExtensions
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00004151 File Offset: 0x00002351
		public static bool Contains(this Enum value, string str)
		{
			return value.ToString().Contains(str);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000415F File Offset: 0x0000235F
		public static string ToLower(this Enum value)
		{
			return value.ToString().ToLower();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000416C File Offset: 0x0000236C
		public static string ToUpper(this Enum value)
		{
			return value.ToString().ToUpper();
		}
	}
}
