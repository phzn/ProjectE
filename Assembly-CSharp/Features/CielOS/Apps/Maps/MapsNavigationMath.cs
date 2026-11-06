using System;
using UnityEngine;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x0200147E RID: 5246
	public static class MapsNavigationMath
	{
		// Token: 0x0600A1AD RID: 41389 RVA: 0x004318B1 File Offset: 0x0042FAB1
		public static Vector2 Project(Vector3 position, Vector3 center, float pixelsPerMetre)
		{
			return new Vector2(position.x - center.x, position.z - center.z) * pixelsPerMetre;
		}

		// Token: 0x0600A1AE RID: 41390 RVA: 0x004318D8 File Offset: 0x0042FAD8
		public static float FitScale(Vector2 viewport, Vector2 worldSize, float padding = 36f)
		{
			return Mathf.Min(Mathf.Max(1f, viewport.x - padding * 2f) / Mathf.Max(1f, worldSize.x), Mathf.Max(1f, viewport.y - padding * 2f) / Mathf.Max(1f, worldSize.y));
		}

		// Token: 0x0600A1AF RID: 41391 RVA: 0x0043193C File Offset: 0x0042FB3C
		public static float LocalWalkingScale(Vector2 viewport)
		{
			return Mathf.Max(1f, viewport.x) / 120f;
		}

		// Token: 0x0600A1B0 RID: 41392 RVA: 0x00431954 File Offset: 0x0042FB54
		public static float Heading(Vector3 from, Vector3 to)
		{
			return Mathf.Atan2(to.x - from.x, to.z - from.z) * 57.29578f;
		}

		// Token: 0x0600A1B1 RID: 41393 RVA: 0x0043197C File Offset: 0x0042FB7C
		public static Vector2 ClampMarker(Vector2 local, Vector2 viewport, float inset = 22f)
		{
			Vector2 vector = new Vector2(Mathf.Max(1f, viewport.x * 0.5f - inset), Mathf.Max(1f, viewport.y * 0.5f - inset));
			float num = Mathf.Max(Mathf.Abs(local.x) / vector.x, Mathf.Abs(local.y) / vector.y);
			if (num <= 1f)
			{
				return local;
			}
			return local / num;
		}

		// Token: 0x0600A1B2 RID: 41394 RVA: 0x004319FC File Offset: 0x0042FBFC
		public static string Bearing(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			if (vector.x * vector.x + vector.z * vector.z < 1f)
			{
				return "HERE";
			}
			switch (Mathf.RoundToInt(Mathf.Repeat(Mathf.Atan2(vector.x, vector.z) * 57.29578f, 360f) / 45f) % 8)
			{
			case 0:
				return "N";
			case 1:
				return "NE";
			case 2:
				return "E";
			case 3:
				return "SE";
			case 4:
				return "S";
			case 5:
				return "SW";
			case 6:
				return "W";
			default:
				return "NW";
			}
		}
	}
}
