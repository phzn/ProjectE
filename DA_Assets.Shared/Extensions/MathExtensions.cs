using System;
using System.Globalization;
using System.Numerics;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002F RID: 47
	public static class MathExtensions
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004B35 File Offset: 0x00002D35
		public static bool TryParseWithDot(this string str, out float value)
		{
			return float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004B48 File Offset: 0x00002D48
		public static string ToDotString(this float value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004B56 File Offset: 0x00002D56
		public static float ToFloat(this float? value)
		{
			if (value == null)
			{
				return 0f;
			}
			return value.Value;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004B6E File Offset: 0x00002D6E
		public static BigInteger ToBigInteger(this byte[] data)
		{
			return new BigInteger(data);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004B78 File Offset: 0x00002D78
		public static int NormalizeAngleToSize(this float val, float width, float height)
		{
			if (val <= 0f)
			{
				val = 0f;
				return (int)val;
			}
			float num = width / val;
			float num2 = height / val;
			if (num < 2f || num2 < 2f)
			{
				val = (float)Mathf.Min((int)(width / 2f), (int)(height / 2f));
			}
			if (val <= 0f)
			{
				val = 0f;
			}
			return (int)val;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004BD8 File Offset: 0x00002DD8
		public static bool TryParseFloat(this string input, out float result)
		{
			bool flag = float.TryParse(input, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.GetCultureInfo("fr-FR"), out result);
			if (!flag)
			{
				flag = float.TryParse(input, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out result);
			}
			return flag;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004C14 File Offset: 0x00002E14
		public static float CalculateIntersectionFactor(this Vector3 lineStart, Vector2 lineEnd, Vector2 origin, Vector2 direction)
		{
			float num = (lineEnd.x - lineStart.x) * direction.y - (lineEnd.y - lineStart.y) * direction.x;
			if (Mathf.Approximately(num, 0f))
			{
				return -1f;
			}
			return ((origin.x - lineStart.x) * direction.y - (origin.y - lineStart.y) * direction.x) / num;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004C8C File Offset: 0x00002E8C
		public static UIVertex Lerp(this UIVertex vertex1, UIVertex vertex2, float interpolationFactor)
		{
			return new UIVertex
			{
				position = Vector3.Lerp(vertex1.position, vertex2.position, interpolationFactor),
				color = Color.Lerp(vertex1.color, vertex2.color, interpolationFactor),
				uv0 = Vector2.Lerp(vertex1.uv0, vertex2.uv0, interpolationFactor),
				uv1 = Vector2.Lerp(vertex1.uv1, vertex2.uv1, interpolationFactor),
				uv2 = Vector2.Lerp(vertex1.uv2, vertex2.uv2, interpolationFactor),
				uv3 = Vector2.Lerp(vertex1.uv3, vertex2.uv3, interpolationFactor)
			};
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004D84 File Offset: 0x00002F84
		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			float x = v.x;
			float y = v.y;
			v.x = num2 * x - num * y;
			v.y = num * x + num2 * y;
			return v;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004DD6 File Offset: 0x00002FD6
		public static float NormalizeAngle360(this float angle)
		{
			if (angle < 0f)
			{
				angle = angle % 360f + 360f;
			}
			else
			{
				angle %= 360f;
			}
			return angle;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004DFC File Offset: 0x00002FFC
		public static int GetDeterministicHashCode(this string str)
		{
			int num = 352654597;
			int num2 = num;
			for (int i = 0; i < str.Length; i += 2)
			{
				num = ((num << 5) + num) ^ (int)str[i];
				if (i == str.Length - 1)
				{
					break;
				}
				num2 = ((num2 << 5) + num2) ^ (int)str[i + 1];
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004E54 File Offset: 0x00003054
		public static float Remap(this float value, float sourceMin, float sourceMax, float targetMin, float targetMax)
		{
			return (value - sourceMin) / (sourceMax - sourceMin) * (targetMax - targetMin) + targetMin;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004E64 File Offset: 0x00003064
		public static bool RandomBool
		{
			get
			{
				return Random.value > 0.5f;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004E74 File Offset: 0x00003074
		public static Vector4 Round(this Vector4 vector4, int dp = 0)
		{
			float num = (float)Math.Round((double)vector4.x, dp);
			float num2 = (float)Math.Round((double)vector4.y, dp);
			float num3 = (float)Math.Round((double)vector4.z, dp);
			float num4 = (float)Math.Round((double)vector4.w, dp);
			return new Vector4(num, num2, num3, num4);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004EC4 File Offset: 0x000030C4
		public static Vector2 Round(this Vector2 vector2, int dp = 0)
		{
			float num = (float)Math.Round((double)vector2.x, dp);
			float num2 = (float)Math.Round((double)vector2.y, dp);
			return new Vector2(num, num2);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004EF4 File Offset: 0x000030F4
		public static float RoundToNearest025(this float value)
		{
			return Mathf.Round(value * 4f) / 4f;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004F08 File Offset: 0x00003108
		public static float RoundToNearest005(this float value)
		{
			return Mathf.Round(value * 20f) / 20f;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004F1C File Offset: 0x0000311C
		public static float Round(this float value, int digits)
		{
			return (float)Math.Round((double)value, digits);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004F27 File Offset: 0x00003127
		public static Vector2Int ToVector2Int(this Vector2 vector)
		{
			return new Vector2Int((int)vector.x, (int)vector.y);
		}
	}
}
