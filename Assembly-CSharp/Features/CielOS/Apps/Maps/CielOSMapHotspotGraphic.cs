using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001465 RID: 5221
	[RequireComponent(typeof(CanvasRenderer))]
	internal sealed class CielOSMapHotspotGraphic : MaskableGraphic
	{
		// Token: 0x0600A11D RID: 41245 RVA: 0x0042BA45 File Offset: 0x00429C45
		internal void Configure(bool selected, Color accent)
		{
			this._selected = selected;
			this._accent = accent;
			this.SetVerticesDirty();
		}

		// Token: 0x0600A11E RID: 41246 RVA: 0x0042BA5C File Offset: 0x00429C5C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Vector2 center = base.rectTransform.rect.center;
			CielOSMapHotspotGraphic.AddDisc(vh, center + new Vector2(0f, -2f), 23f, CielOSMapHotspotGraphic.Shadow, 32);
			if (this._selected)
			{
				Color color = new Color(this._accent.r, this._accent.g, this._accent.b, 0.98f);
				CielOSMapHotspotGraphic.AddDisc(vh, center, 25f, color, 36);
				CielOSMapHotspotGraphic.AddDisc(vh, center, 21.5f, CielOSMapHotspotGraphic.Core, 36);
				CielOSMapHotspotGraphic.AddRing(vh, center, 19.5f, 2f, CielOSMapHotspotGraphic.Rim, 36);
				return;
			}
			CielOSMapHotspotGraphic.AddDisc(vh, center, 21.5f, CielOSMapHotspotGraphic.Rim, 32);
			CielOSMapHotspotGraphic.AddDisc(vh, center, 18.5f, CielOSMapHotspotGraphic.Core, 32);
		}

		// Token: 0x0600A11F RID: 41247 RVA: 0x0042BB40 File Offset: 0x00429D40
		private static void AddRing(VertexHelper vh, Vector2 center, float radius, float thickness, Color color, int segments)
		{
			float num = Mathf.Max(0f, radius - thickness);
			for (int i = 0; i < segments; i++)
			{
				float num2 = (float)i * 3.1415927f * 2f / (float)segments;
				float num3 = (float)(i + 1) * 3.1415927f * 2f / (float)segments;
				Vector2 vector = center + new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * radius;
				Vector2 vector2 = center + new Vector2(Mathf.Cos(num3), Mathf.Sin(num3)) * radius;
				Vector2 vector3 = center + new Vector2(Mathf.Cos(num3), Mathf.Sin(num3)) * num;
				Vector2 vector4 = center + new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * num;
				CielOSMapHotspotGraphic.AddQuad(vh, vector, vector2, vector3, vector4, color);
			}
		}

		// Token: 0x0600A120 RID: 41248 RVA: 0x0042BC24 File Offset: 0x00429E24
		private static void AddDisc(VertexHelper vh, Vector2 center, float radius, Color color, int segments)
		{
			int currentVertCount = vh.currentVertCount;
			vh.AddVert(center, color, Vector2.zero);
			for (int i = 0; i <= segments; i++)
			{
				float num = (float)i * 3.1415927f * 2f / (float)segments;
				vh.AddVert(center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * radius, color, Vector2.zero);
			}
			for (int j = 0; j < segments; j++)
			{
				vh.AddTriangle(currentVertCount, currentVertCount + j + 1, currentVertCount + j + 2);
			}
		}

		// Token: 0x0600A121 RID: 41249 RVA: 0x0042BCCC File Offset: 0x00429ECC
		private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			vh.AddVert(a, color, Vector2.zero);
			vh.AddVert(b, color, Vector2.zero);
			vh.AddVert(c, color, Vector2.zero);
			vh.AddVert(d, color, Vector2.zero);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 3);
		}

		// Token: 0x0400A4DD RID: 42205
		private static readonly Color Shadow = new Color(0.01f, 0.015f, 0.02f, 0.58f);

		// Token: 0x0400A4DE RID: 42206
		private static readonly Color Rim = new Color(0.93f, 0.93f, 0.9f, 0.96f);

		// Token: 0x0400A4DF RID: 42207
		private static readonly Color Core = new Color(0.075f, 0.09f, 0.105f, 0.98f);

		// Token: 0x0400A4E0 RID: 42208
		private bool _selected;

		// Token: 0x0400A4E1 RID: 42209
		private Color _accent = new Color(0.67f, 0.38f, 0.79f, 1f);
	}
}
