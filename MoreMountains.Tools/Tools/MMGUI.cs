using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x020000FD RID: 253
	public static class MMGUI
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x0001B09C File Offset: 0x0001929C
		public static void SetSize(RectTransform rectTransform, Vector2 newSize)
		{
			Vector2 size = rectTransform.rect.size;
			Vector2 vector = newSize - size;
			rectTransform.offsetMin -= new Vector2(vector.x * rectTransform.pivot.x, vector.y * rectTransform.pivot.y);
			rectTransform.offsetMax += new Vector2(vector.x * (1f - rectTransform.pivot.x), vector.y * (1f - rectTransform.pivot.y));
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001B140 File Offset: 0x00019340
		public static bool PointOrTouchBlockedByUI()
		{
			return EventSystem.current.IsPointerOverGameObject() || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId));
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001B194 File Offset: 0x00019394
		public static Texture2D MakeTex(int width, int height, Color color)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}
	}
}
