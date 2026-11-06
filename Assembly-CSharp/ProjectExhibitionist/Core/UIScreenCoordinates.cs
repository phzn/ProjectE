using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A77 RID: 2679
	public static class UIScreenCoordinates
	{
		// Token: 0x06004C3C RID: 19516 RVA: 0x001DA684 File Offset: 0x001D8884
		public static Camera ResolveEventCamera(Canvas canvas)
		{
			if (canvas == null)
			{
				return null;
			}
			Canvas rootCanvas = canvas.rootCanvas;
			if (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return null;
			}
			if (!(rootCanvas.worldCamera != null))
			{
				return canvas.worldCamera;
			}
			return rootCanvas.worldCamera;
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001DA6C8 File Offset: 0x001D88C8
		public static bool TryScreenPointToLocalPoint(RectTransform rect, Canvas canvas, Vector2 screenPoint, out Vector2 localPoint)
		{
			localPoint = Vector2.zero;
			if (rect == null)
			{
				return false;
			}
			if (canvas == null)
			{
				canvas = rect.GetComponentInParent<Canvas>();
			}
			Canvas canvas2 = ((canvas != null) ? canvas.rootCanvas : null);
			if (canvas2 != null && canvas2.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				return RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, UIScreenCoordinates.ResolveEventCamera(canvas), out localPoint);
			}
			Vector2 vector = rect.TransformVector(Vector3.right);
			Vector2 vector2 = rect.TransformVector(Vector3.up);
			float num = vector.x * vector2.y - vector.y * vector2.x;
			if (Mathf.Abs(num) < 1E-06f)
			{
				return false;
			}
			Vector2 vector3 = screenPoint - rect.position;
			localPoint = new Vector2((vector3.x * vector2.y - vector3.y * vector2.x) / num, (vector.x * vector3.y - vector.y * vector3.x) / num);
			return true;
		}
	}
}
