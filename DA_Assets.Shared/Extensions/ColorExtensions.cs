using System;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000028 RID: 40
	public static class ColorExtensions
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003D44 File Offset: 0x00001F44
		public static Color Lerp(this Color a, Color b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003DB6 File Offset: 0x00001FB6
		public static Color SetAlpha(this Color color, float? alpha)
		{
			return new Color(color.r, color.g, color.b, (alpha == null) ? 1f : alpha.ToFloat());
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003DE8 File Offset: 0x00001FE8
		public static Color ToColor(this string color)
		{
			if (color.StartsWith("#", StringComparison.InvariantCulture))
			{
				color = color.Substring(1);
			}
			if (color.Length == 6)
			{
				color += "FF";
			}
			uint num = Convert.ToUInt32(color, 16);
			float num2 = ((num & 4278190080U) >> 24) / 255f;
			float num3 = ((num & 16711680U) >> 16) / 255f;
			float num4 = ((num & 65280U) >> 8) / 255f;
			float num5 = (num & 255U) / 255f;
			return new Color(num2, num3, num4, num5);
		}
	}
}
