using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E7 RID: 231
	public static class MMFloatExtensions
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public static float MMNormalizeAngle(this float angleInDegrees)
		{
			angleInDegrees %= 360f;
			if (angleInDegrees < 0f)
			{
				angleInDegrees += 360f;
			}
			return angleInDegrees;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00016CD1 File Offset: 0x00014ED1
		public static float RoundDown(this float number, int decimalPlaces)
		{
			return Mathf.Floor(number * Mathf.Pow(10f, (float)decimalPlaces)) / Mathf.Pow(10f, (float)decimalPlaces);
		}
	}
}
