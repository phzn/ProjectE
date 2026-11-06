using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006BF RID: 1727
	public static class vLockOnHelper
	{
		// Token: 0x06002AAD RID: 10925 RVA: 0x000EF1B8 File Offset: 0x000ED3B8
		public static Vector2 GetScreenPointOffBoundsCenter(this Transform target, Canvas canvas, Camera cam, float _heightOffset)
		{
			Bounds bounds = target.GetComponent<Collider>().bounds;
			Vector3 center = bounds.center;
			float num = Vector3.Distance(bounds.min, bounds.max);
			Vector3 vector = center + new Vector3(0f, num * _heightOffset, 0f);
			RectTransform rectTransform = canvas.transform as RectTransform;
			Vector2 vector2 = cam.WorldToViewportPoint(vector);
			return new Vector2(vector2.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f, vector2.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000EF270 File Offset: 0x000ED470
		public static Vector3 GetPointOffBoundsCenter(this Transform target, float _heightOffset)
		{
			Bounds bounds = target.GetComponent<Collider>().bounds;
			Vector3 center = bounds.center;
			float num = Vector3.Distance(bounds.min, bounds.max);
			return center + new Vector3(0f, num * _heightOffset, 0f);
		}
	}
}
