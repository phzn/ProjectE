using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200015C RID: 348
	public class MMTweenDefinitions
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x000260B0 File Offset: 0x000242B0
		public static float Linear_Tween(float t)
		{
			return t;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000260B3 File Offset: 0x000242B3
		public static float LinearAnti_Tween(float t)
		{
			return 1f - t;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000260BC File Offset: 0x000242BC
		public static float AlmostIdentity(float t)
		{
			return t * t * (2f - t);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000260C9 File Offset: 0x000242C9
		public static float EaseIn_Quadratic(float t)
		{
			return t * t;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000260CE File Offset: 0x000242CE
		public static float EaseOut_Quadratic(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Quadratic(1f - t);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000260E2 File Offset: 0x000242E2
		public static float EaseInOut_Quadratic(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Quadratic(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Quadratic((1f - t) * 2f) / 2f;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002611D File Offset: 0x0002431D
		public static float EaseIn_Cubic(float t)
		{
			return t * t * t;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00026124 File Offset: 0x00024324
		public static float EaseOut_Cubic(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Cubic(1f - t);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00026138 File Offset: 0x00024338
		public static float EaseInOut_Cubic(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Cubic(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Cubic((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00026173 File Offset: 0x00024373
		public static float EaseIn_Quartic(float t)
		{
			return Mathf.Pow(t, 4f);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00026180 File Offset: 0x00024380
		public static float EaseOut_Quartic(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Quartic(1f - t);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00026194 File Offset: 0x00024394
		public static float EaseInOut_Quartic(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Quartic(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Quartic((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000261CF File Offset: 0x000243CF
		public static float EaseIn_Quintic(float t)
		{
			return Mathf.Pow(t, 5f);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000261DC File Offset: 0x000243DC
		public static float EaseOut_Quintic(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Quintic(1f - t);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000261F0 File Offset: 0x000243F0
		public static float EaseInOut_Quintic(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Quintic(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Quintic((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002622C File Offset: 0x0002442C
		public static float EaseIn_Bounce(float t)
		{
			float num = 0.3f;
			return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num / 4f) * 6.2831855f / num) + 1f;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002626D File Offset: 0x0002446D
		public static float EaseOut_Bounce(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Bounce(1f - t);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00026281 File Offset: 0x00024481
		public static float EaseInOut_Bounce(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Bounce(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Bounce((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000262BC File Offset: 0x000244BC
		public static float EaseIn_Sinusoidal(float t)
		{
			return 1f + Mathf.Sin(1.5707964f * t - 1.5707964f);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000262D6 File Offset: 0x000244D6
		public static float EaseOut_Sinusoidal(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Sinusoidal(1f - t);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000262EA File Offset: 0x000244EA
		public static float EaseInOut_Sinusoidal(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Sinusoidal(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Sinusoidal((1f - t) * 2f) / 2f;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00026328 File Offset: 0x00024528
		public static float EaseIn_Overhead(float t)
		{
			float num = 1.6f;
			return t * t * ((num + 1f) * t - num);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002634A File Offset: 0x0002454A
		public static float EaseOut_Overhead(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Overhead(1f - t);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002635E File Offset: 0x0002455E
		public static float EaseInOut_Overhead(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Overhead(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Overhead((1f - t) * 2f) / 2f;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00026399 File Offset: 0x00024599
		public static float EaseIn_Exponential(float t)
		{
			if (t != 0f)
			{
				return Mathf.Pow(1024f, t - 1f);
			}
			return 0f;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000263BA File Offset: 0x000245BA
		public static float EaseOut_Exponential(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Exponential(1f - t);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000263CE File Offset: 0x000245CE
		public static float EaseInOut_Exponential(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Exponential(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Exponential((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002640C File Offset: 0x0002460C
		public static float EaseIn_Elastic(float t)
		{
			if (t == 0f)
			{
				return 0f;
			}
			if (t == 1f)
			{
				return 1f;
			}
			return -Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - 0.1f) * 6.2831855f / 0.4f);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00026469 File Offset: 0x00024669
		public static float EaseOut_Elastic(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Elastic(1f - t);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002647D File Offset: 0x0002467D
		public static float EaseInOut_Elastic(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Elastic(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Elastic((1f - t) * 2f) / 2f;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000264B8 File Offset: 0x000246B8
		public static float EaseIn_Circular(float t)
		{
			return 1f - Mathf.Sqrt(1f - t * t);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000264CE File Offset: 0x000246CE
		public static float EaseOut_Circular(float t)
		{
			return 1f - MMTweenDefinitions.EaseIn_Circular(1f - t);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000264E2 File Offset: 0x000246E2
		public static float EaseInOut_Circular(float t)
		{
			if (t < 0.5f)
			{
				return MMTweenDefinitions.EaseIn_Circular(t * 2f) / 2f;
			}
			return 1f - MMTweenDefinitions.EaseIn_Circular((1f - t) * 2f) / 2f;
		}
	}
}
