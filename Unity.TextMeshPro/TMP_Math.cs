using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000013 RID: 19
	public static class TMP_Math
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0001706E File Offset: 0x0001526E
		public static bool Approximately(float a, float b)
		{
			return b - 0.0001f < a && a < b + 0.0001f;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00017088 File Offset: 0x00015288
		public static int Mod(int a, int b)
		{
			int num = a % b;
			if (num >= 0)
			{
				return num;
			}
			return num + b;
		}

		// Token: 0x04000090 RID: 144
		public const float FLOAT_MAX = 32767f;

		// Token: 0x04000091 RID: 145
		public const float FLOAT_MIN = -32767f;

		// Token: 0x04000092 RID: 146
		public const int INT_MAX = 2147483647;

		// Token: 0x04000093 RID: 147
		public const int INT_MIN = -2147483647;

		// Token: 0x04000094 RID: 148
		public const float FLOAT_UNSET = -32767f;

		// Token: 0x04000095 RID: 149
		public const int INT_UNSET = -32767;

		// Token: 0x04000096 RID: 150
		public static Vector2 MAX_16BIT = new Vector2(32767f, 32767f);

		// Token: 0x04000097 RID: 151
		public static Vector2 MIN_16BIT = new Vector2(-32767f, -32767f);
	}
}
