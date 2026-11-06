using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E5 RID: 229
	public static class MMColorExtensions
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x00016BDB File Offset: 0x00014DDB
		public static float Sum(this Color color)
		{
			return color.r + color.g + color.b + color.a;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00016BF8 File Offset: 0x00014DF8
		public static float MeanRGB(this Color color)
		{
			return (color.r + color.g + color.b) / 3f;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00016C14 File Offset: 0x00014E14
		public static float Luminance(this Color color)
		{
			return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
		}
	}
}
