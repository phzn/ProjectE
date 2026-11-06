using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014BF RID: 5311
	public static class StreamerCensorProjection
	{
		// Token: 0x0600A356 RID: 41814 RVA: 0x0043DD04 File Offset: 0x0043BF04
		public static Bounds TransformBounds(Transform transform, Bounds local)
		{
			Bounds bounds = new Bounds(transform.TransformPoint(local.center), Vector3.zero);
			for (int i = 0; i < 8; i++)
			{
				bounds.Encapsulate(transform.TransformPoint(StreamerCensorProjection.Corner(local, i)));
			}
			return bounds;
		}

		// Token: 0x0600A357 RID: 41815 RVA: 0x0043DD4B File Offset: 0x0043BF4B
		private static Vector3 Corner(Bounds b, int i)
		{
			return b.center + Vector3.Scale(b.extents, new Vector3((float)(((i & 1) == 0) ? (-1) : 1), (float)(((i & 2) == 0) ? (-1) : 1), (float)(((i & 4) == 0) ? (-1) : 1)));
		}

		// Token: 0x0600A358 RID: 41816 RVA: 0x0043DD88 File Offset: 0x0043BF88
		public static bool TryProject(Camera camera, Bounds bounds, out Rect rect)
		{
			rect = default(Rect);
			Vector2 vector = Vector2.one * float.PositiveInfinity;
			Vector2 vector2 = Vector2.one * float.NegativeInfinity;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector3 = camera.WorldToViewportPoint(StreamerCensorProjection.Corner(bounds, i));
				if (vector3.z <= camera.nearClipPlane)
				{
					num++;
				}
				else
				{
					vector = Vector2.Min(vector, vector3);
					vector2 = Vector2.Max(vector2, vector3);
				}
			}
			if (num == 8)
			{
				return false;
			}
			if (num > 0)
			{
				rect = new Rect(0f, 0f, 1f, 1f);
				return true;
			}
			if (vector2.x < 0f || vector.x > 1f || vector2.y < 0f || vector.y > 1f)
			{
				return false;
			}
			rect = Rect.MinMaxRect(Mathf.Clamp01(vector.x), Mathf.Clamp01(vector.y), Mathf.Clamp01(vector2.x), Mathf.Clamp01(vector2.y));
			return rect.width > 0f && rect.height > 0f;
		}

		// Token: 0x0600A359 RID: 41817 RVA: 0x0043DEC0 File Offset: 0x0043C0C0
		internal static void Add(Camera camera, Bounds bounds, List<Rect> regions)
		{
			Rect rect;
			if (StreamerCensorProjection.TryProject(camera, bounds, out rect))
			{
				regions.Add(rect);
			}
		}

		// Token: 0x0600A35A RID: 41818 RVA: 0x0043DEDF File Offset: 0x0043C0DF
		public static float BlockPixels(Rect rect, int width, int height, float setting)
		{
			return Mathf.Max(2f, Mathf.Max(setting * (float)height / 1080f, Mathf.Min(rect.width * (float)width, rect.height * (float)height) * setting / 128f));
		}
	}
}
