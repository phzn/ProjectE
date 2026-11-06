using System;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x020002AA RID: 682
	public static class Misc
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x0007D1F4 File Offset: 0x0007B3F4
		public static Rect GetScreenRect(this RectTransform o)
		{
			Vector2 vector = Vector2.Scale(o.rect.size, o.lossyScale);
			Rect rect = new Rect(o.position.x, o.position.y, vector.x, vector.y);
			rect.x -= o.pivot.x * vector.x;
			rect.y -= o.pivot.y * vector.y;
			return rect;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0007D28C File Offset: 0x0007B48C
		public static Rect GetScreenRect(this RectTransform o, Camera camera)
		{
			o.GetWorldCorners(Misc.wc);
			return new Rect(Misc.wc[0].x, Misc.wc[0].y, Misc.wc[2].x - Misc.wc[0].x, Misc.wc[2].y - Misc.wc[0].y);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0007D30C File Offset: 0x0007B50C
		public static Rect GetViewportRect(this RectTransform o, Camera camera)
		{
			Rect screenRect = o.GetScreenRect(camera);
			screenRect.x /= (float)camera.pixelWidth;
			screenRect.y /= (float)camera.pixelHeight;
			screenRect.width /= (float)camera.pixelWidth;
			screenRect.height /= (float)camera.pixelHeight;
			return screenRect;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0007D376 File Offset: 0x0007B576
		public static T FindObjectOfType<T>(bool includeInactive = false) where T : Object
		{
			return Object.FindObjectOfType<T>(includeInactive);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0007D37E File Offset: 0x0007B57E
		public static Object[] FindObjectsOfType(Type type, bool includeInactive = false)
		{
			return Object.FindObjectsOfType(type, includeInactive);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0007D387 File Offset: 0x0007B587
		public static T[] FindObjectsOfType<T>(bool includeInactive = false) where T : Object
		{
			return Object.FindObjectsOfType<T>(includeInactive);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0007D38F File Offset: 0x0007B58F
		public static void DestroySafe(Object obj)
		{
			Object.Destroy(obj);
		}

		// Token: 0x040014BB RID: 5307
		public static Vector4 Vector4back = new Vector4(0f, 0f, -1f, 0f);

		// Token: 0x040014BC RID: 5308
		public static Vector3 Vector3one = Vector3.one;

		// Token: 0x040014BD RID: 5309
		public static Vector3 Vector3zero = Vector3.zero;

		// Token: 0x040014BE RID: 5310
		public static Vector3 Vector3back = Vector3.back;

		// Token: 0x040014BF RID: 5311
		public static Vector3 Vector3left = Vector3.left;

		// Token: 0x040014C0 RID: 5312
		public static Vector3 Vector3right = Vector3.right;

		// Token: 0x040014C1 RID: 5313
		public static Vector3 Vector3up = Vector3.up;

		// Token: 0x040014C2 RID: 5314
		public static Vector3 Vector3down = Vector3.down;

		// Token: 0x040014C3 RID: 5315
		public static Vector3 Vector3half = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x040014C4 RID: 5316
		public static Vector2 Vector2left = Vector2.left;

		// Token: 0x040014C5 RID: 5317
		public static Vector2 Vector2right = Vector2.right;

		// Token: 0x040014C6 RID: 5318
		public static Vector2 Vector2one = Vector2.one;

		// Token: 0x040014C7 RID: 5319
		public static Vector2 Vector2zero = Vector2.zero;

		// Token: 0x040014C8 RID: 5320
		public static Vector2 Vector2down = Vector2.down;

		// Token: 0x040014C9 RID: 5321
		public static Vector2 Vector2up = Vector2.up;

		// Token: 0x040014CA RID: 5322
		public static Vector2 Vector2max = new Vector2(100000f, 100000f);

		// Token: 0x040014CB RID: 5323
		public static Vector2 Vector2half = new Vector2(0.5f, 0.5f);

		// Token: 0x040014CC RID: 5324
		public static Vector3 ViewportCenter = new Vector3(0.5f, 0.5f, 0f);

		// Token: 0x040014CD RID: 5325
		public static Color ColorTransparent = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040014CE RID: 5326
		public static Color ColorWhite = Color.white;

		// Token: 0x040014CF RID: 5327
		public static Quaternion QuaternionZero = Quaternion.Euler(0f, 0f, 0f);

		// Token: 0x040014D0 RID: 5328
		private static readonly Vector3[] wc = new Vector3[4];

		// Token: 0x040014D1 RID: 5329
		public static WaitForSeconds WaitForOneSecond = new WaitForSeconds(1f);
	}
}
