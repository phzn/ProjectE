using System;
using UnityEngine;

namespace DA_Assets.DAG
{
	// Token: 0x02000007 RID: 7
	internal static class Extensions
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002850 File Offset: 0x00000A50
		public static Color Difference(this Color c1, Color c2, float intensity)
		{
			float num = Mathf.Abs(c1.r - c2.r) * intensity;
			float num2 = Mathf.Abs(c1.g - c2.g) * intensity;
			float num3 = Mathf.Abs(c1.b - c2.b) * intensity;
			return new Color(num, num2, num3, c1.a);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028AA File Offset: 0x00000AAA
		public static Color Multiply(this Color c1, Color c2, float intensity)
		{
			return c1.Lerp(c1 * c2, intensity);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028BA File Offset: 0x00000ABA
		public static Color Overlay(this Color c1, Color c2, float intensity)
		{
			return c1.Lerp(c2, intensity);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028C4 File Offset: 0x00000AC4
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002918 File Offset: 0x00000B18
		public static float CalculateIntersectionFactor(this Vector3 lineStart, Vector2 lineEnd, Vector2 origin, Vector2 direction)
		{
			float num = (lineEnd.x - lineStart.x) * direction.y - (lineEnd.y - lineStart.y) * direction.x;
			if (Mathf.Approximately(num, 0f))
			{
				return -1f;
			}
			return ((origin.x - lineStart.x) * direction.y - (origin.y - lineStart.y) * direction.x) / num;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000298D File Offset: 0x00000B8D
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

		// Token: 0x0600001A RID: 26 RVA: 0x000029B4 File Offset: 0x00000BB4
		public static Color Lerp(this Color a, Color b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A28 File Offset: 0x00000C28
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
	}
}
