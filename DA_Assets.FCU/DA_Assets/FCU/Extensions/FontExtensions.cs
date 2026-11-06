using System;
using DA_Assets.Extensions;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004B RID: 75
	public static class FontExtensions
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x000053B0 File Offset: 0x000035B0
		public static string FormatFontName(this string value)
		{
			if (value.IsEmpty())
			{
				return "null";
			}
			return value.Replace("SDF", "").ToLower().Replace(" ", "")
				.Replace("-", "")
				.Replace("_", "");
		}
	}
}
