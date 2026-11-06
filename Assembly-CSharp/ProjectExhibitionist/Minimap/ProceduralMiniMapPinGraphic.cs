using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B1 RID: 2481
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class ProceduralMiniMapPinGraphic : MaskableGraphic
	{
		// Token: 0x060045D2 RID: 17874 RVA: 0x001A65A3 File Offset: 0x001A47A3
		public void SetPalette(Color fill, Color rim, Color core)
		{
			this._fill = fill;
			this._rim = rim;
			this._core = core;
			this.SetVerticesDirty();
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x001A65C0 File Offset: 0x001A47C0
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			ProceduralMiniMapPinGraphic.AddDisc(vh, new Vector2(0f, 0.5f), 4.2f, 1.6f, new Color(0f, 0f, 0f, 0.35f));
			ProceduralMiniMapPinGraphic.AddDrop(vh, 9.8f, 19f, 1.8f, this._rim);
			ProceduralMiniMapPinGraphic.AddDrop(vh, 8f, 19f, 0f, this._fill);
			ProceduralMiniMapPinGraphic.AddDisc(vh, new Vector2(0f, 19f), 3.2f, 3.2f, this._core);
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x001A6668 File Offset: 0x001A4868
		private static void AddDrop(VertexHelper vh, float radius, float headY, float tipPadding, Color color)
		{
			Vector2 vector = new Vector2(0f, headY);
			ProceduralMiniMapPinGraphic.AddDisc(vh, vector, radius, radius, color);
			float num = headY - radius * 0.55f;
			float num2 = radius * 0.84f;
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapPinGraphic.AddVertex(vh, new Vector2(-num2, num), color);
			ProceduralMiniMapPinGraphic.AddVertex(vh, new Vector2(num2, num), color);
			ProceduralMiniMapPinGraphic.AddVertex(vh, new Vector2(0f, -tipPadding), color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x001A66E8 File Offset: 0x001A48E8
		private static void AddDisc(VertexHelper vh, Vector2 center, float radiusX, float radiusY, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapPinGraphic.AddVertex(vh, center, color);
			for (int i = 0; i <= 28; i++)
			{
				float num = (float)i * 3.1415927f * 2f / 28f;
				ProceduralMiniMapPinGraphic.AddVertex(vh, center + new Vector2(Mathf.Cos(num) * radiusX, Mathf.Sin(num) * radiusY), color);
			}
			for (int j = 0; j < 28; j++)
			{
				vh.AddTriangle(currentVertCount, currentVertCount + j + 1, currentVertCount + j + 2);
			}
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x001A6768 File Offset: 0x001A4968
		private static void AddVertex(VertexHelper vh, Vector2 position, Color color)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.position = position;
			simpleVert.color = color;
			simpleVert.uv0 = new Vector2(0.5f, 0.5f);
			vh.AddVert(simpleVert);
		}

		// Token: 0x04004569 RID: 17769
		private const int Segments = 28;

		// Token: 0x0400456A RID: 17770
		private const float HeadRadius = 8f;

		// Token: 0x0400456B RID: 17771
		private const float HeadHeight = 19f;

		// Token: 0x0400456C RID: 17772
		private Color _rim = new Color(0.08f, 0.04f, 0.07f, 0.95f);

		// Token: 0x0400456D RID: 17773
		private Color _fill = MapPinController.ThemePink;

		// Token: 0x0400456E RID: 17774
		private Color _core = new Color(1f, 0.96f, 0.98f, 1f);
	}
}
