using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RapidActions
{
	// Token: 0x02000D87 RID: 3463
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class RadialCategoryMarker : MaskableGraphic
	{
		// Token: 0x060065ED RID: 26093 RVA: 0x0028AFC8 File Offset: 0x002891C8
		public static void SetVisible(UltimateRadialMenu.UltimateRadialButton button, bool visible)
		{
			if (button == null)
			{
				return;
			}
			RectTransform rectTransform = ((button.text != null) ? button.text.rectTransform : button.buttonTransform);
			if (rectTransform == null)
			{
				return;
			}
			Transform transform = rectTransform.Find("SpecialCategoryMarker");
			RadialCategoryMarker radialCategoryMarker = ((transform != null) ? transform.GetComponent<RadialCategoryMarker>() : null);
			if (radialCategoryMarker == null && visible)
			{
				radialCategoryMarker = new GameObject("SpecialCategoryMarker", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(RadialCategoryMarker)
				})
				{
					layer = rectTransform.gameObject.layer
				}.GetComponent<RadialCategoryMarker>();
				radialCategoryMarker.rectTransform.SetParent(rectTransform, false);
				RectTransform rectTransform2 = radialCategoryMarker.rectTransform;
				RectTransform rectTransform3 = radialCategoryMarker.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform3.anchorMax = vector;
				rectTransform2.anchorMin = vector;
				radialCategoryMarker.rectTransform.pivot = new Vector2(0.5f, 0f);
				radialCategoryMarker.rectTransform.anchoredPosition = new Vector2(0f, 3f);
				radialCategoryMarker.rectTransform.sizeDelta = new Vector2(16f, 16f);
				radialCategoryMarker.color = new Color(1f, 0.72f, 0.84f, 1f);
				radialCategoryMarker.raycastTarget = false;
			}
			if (radialCategoryMarker != null)
			{
				radialCategoryMarker.gameObject.SetActive(visible);
			}
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x0028B140 File Offset: 0x00289340
		protected override void OnPopulateMesh(VertexHelper vertices)
		{
			vertices.Clear();
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float num = Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height) * 0.5f;
			RadialCategoryMarker.DrawStar(vertices, pixelAdjustedRect.center, num, new Color(0.12f, 0.07f, 0.12f, this.color.a));
			RadialCategoryMarker.DrawStar(vertices, pixelAdjustedRect.center, num * 0.78f, this.color);
		}

		// Token: 0x060065EF RID: 26095 RVA: 0x0028B1BC File Offset: 0x002893BC
		private static void DrawStar(VertexHelper vertices, Vector2 center, float radius, Color tint)
		{
			int currentVertCount = vertices.currentVertCount;
			vertices.AddVert(center, tint, Vector2.zero);
			for (int i = 0; i < 10; i++)
			{
				float num = (90f + (float)i * 36f) * 0.017453292f;
				float num2 = ((i % 2 == 0) ? 1f : 0.46f) * radius;
				Vector2 vector = center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * num2;
				vertices.AddVert(vector, tint, Vector2.zero);
			}
			for (int j = 0; j < 10; j++)
			{
				vertices.AddTriangle(currentVertCount, currentVertCount + 1 + j, currentVertCount + 1 + (j + 1) % 10);
			}
		}

		// Token: 0x0400686F RID: 26735
		private const string MarkerName = "SpecialCategoryMarker";
	}
}
