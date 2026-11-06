using System;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000027 RID: 39
	public static class ColorBlenderExtensions
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static Color Difference(this Color c1, Color c2, float intensity)
		{
			float num = Mathf.Abs(c1.r - c2.r) * intensity;
			float num2 = Mathf.Abs(c1.g - c2.g) * intensity;
			float num3 = Mathf.Abs(c1.b - c2.b) * intensity;
			return new Color(num, num2, num3, c1.a);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D2A File Offset: 0x00001F2A
		public static Color Multiply(this Color c1, Color c2, float intensity)
		{
			return c1.Lerp(c1 * c2, intensity);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003D3A File Offset: 0x00001F3A
		public static Color Overlay(this Color c1, Color c2, float intensity)
		{
			return c1.Lerp(c2, intensity);
		}
	}
}
