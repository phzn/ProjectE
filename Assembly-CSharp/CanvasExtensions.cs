using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public static class CanvasExtensions
{
	// Token: 0x060009F6 RID: 2550 RVA: 0x000458E8 File Offset: 0x00043AE8
	public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 world_position, Camera camera = null)
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
		Vector3 vector = camera.WorldToViewportPoint(world_position);
		RectTransform component = canvas.GetComponent<RectTransform>();
		return new Vector2(vector.x * component.sizeDelta.x - component.sizeDelta.x * 0.5f, vector.y * component.sizeDelta.y - component.sizeDelta.y * 0.5f);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00045964 File Offset: 0x00043B64
	public static Vector2 ClampInsideRectagle(this Vector2 pos, RectTransform container, Vector2 margin)
	{
		Vector2 vector = pos;
		vector.x = Mathf.Clamp(vector.x, -container.rect.width / 2f + margin.x, container.rect.width / 2f - margin.x);
		vector.y = Mathf.Clamp(vector.y, -container.rect.height / 2f + margin.y, container.rect.height / 2f - margin.y);
		return vector;
	}
}
