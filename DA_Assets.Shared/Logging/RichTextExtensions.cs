using System;

namespace DA_Assets.Logging
{
	// Token: 0x02000022 RID: 34
	internal static class RichTextExtensions
	{
		// Token: 0x06000073 RID: 115 RVA: 0x000034A9 File Offset: 0x000016A9
		public static string TextBold(this string str)
		{
			return "<b>" + str + "</b>";
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000034BB File Offset: 0x000016BB
		public static string TextColor(this string str, string clr)
		{
			return string.Format("<color={0}>{1}</color>", clr, str);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000034C9 File Offset: 0x000016C9
		public static string TextItalic(this string str)
		{
			return "<i>" + str + "</i>";
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034DB File Offset: 0x000016DB
		public static string TextSize(this string str, int size)
		{
			return string.Format("<size={0}>{1}</size>", size, str);
		}
	}
}
