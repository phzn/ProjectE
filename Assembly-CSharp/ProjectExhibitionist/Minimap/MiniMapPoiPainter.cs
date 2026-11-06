using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B6 RID: 2486
	internal static class MiniMapPoiPainter
	{
		// Token: 0x060045E3 RID: 17891 RVA: 0x001A6BF8 File Offset: 0x001A4DF8
		internal static void Draw(VertexHelper vh, Vector2 center, float radius, MiniMapPoiKind kind)
		{
			MiniMapPoiPainter.AddDisc(vh, center, radius + 1.6f, MiniMapPoiPainter.Border, 16);
			MiniMapPoiPainter.AddDisc(vh, center, radius, MiniMapPoiPainter.ColorFor(kind), 16);
			float num = radius / 12f;
			switch (kind)
			{
			case MiniMapPoiKind.Home:
				MiniMapPoiPainter.AddTriangle(vh, center + new Vector2(0f, 7f) * num, center + new Vector2(-7f, 0f) * num, center + new Vector2(7f, 0f) * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -3f) * num, new Vector2(10f, 7f) * num, MiniMapPoiPainter.Ink);
				return;
			case MiniMapPoiKind.Park:
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -5f) * num, new Vector2(2.4f, 7f) * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddDisc(vh, center + new Vector2(0f, 2f) * num, 5.2f * num, MiniMapPoiPainter.Ink, 10);
				return;
			case MiniMapPoiKind.Metro:
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(-6f, -5f) * num, center + new Vector2(-6f, 6f) * num, 2.1f * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(-6f, 6f) * num, center, 2.1f * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddLine(vh, center, center + new Vector2(6f, 6f) * num, 2.1f * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(6f, 6f) * num, center + new Vector2(6f, -5f) * num, 2.1f * num, MiniMapPoiPainter.Ink);
				return;
			case MiniMapPoiKind.University:
			{
				MiniMapPoiPainter.AddTriangle(vh, center + new Vector2(0f, 7f) * num, center + new Vector2(-8f, 2f) * num, center + new Vector2(8f, 2f) * num, MiniMapPoiPainter.Ink);
				for (int i = -5; i <= 5; i += 5)
				{
					MiniMapPoiPainter.AddRect(vh, center + new Vector2((float)i, -2f) * num, new Vector2(2f, 8f) * num, MiniMapPoiPainter.Ink);
				}
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -6f) * num, new Vector2(17f, 2f) * num, MiniMapPoiPainter.Ink);
				return;
			}
			case MiniMapPoiKind.Stadium:
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(-5f, -7f) * num, center + new Vector2(-5f, 7f) * num, 2f * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddTriangle(vh, center + new Vector2(-4f, 6f) * num, center + new Vector2(7f, 3f) * num, center + new Vector2(-4f, 0f) * num, MiniMapPoiPainter.Ink);
				return;
			case MiniMapPoiKind.Mall:
				MiniMapPoiPainter.DrawBag(vh, center, num);
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(-4f, 1f) * num, center + new Vector2(0f, -3f) * num, 1.5f * num, MiniMapPoiPainter.Border);
				MiniMapPoiPainter.AddLine(vh, center + new Vector2(0f, -3f) * num, center + new Vector2(4f, 1f) * num, 1.5f * num, MiniMapPoiPainter.Border);
				return;
			case MiniMapPoiKind.Gallery:
				MiniMapPoiPainter.AddDiamond(vh, center, 7f * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddDiamond(vh, center, 3.2f * num, MiniMapPoiPainter.ColorFor(kind));
				return;
			case MiniMapPoiKind.Pool:
			{
				for (int j = -4; j <= 4; j += 4)
				{
					MiniMapPoiPainter.AddLine(vh, center + new Vector2(-7f, (float)j) * num, center + new Vector2(7f, (float)j) * num, 1.7f * num, MiniMapPoiPainter.Ink);
				}
				return;
			}
			case MiniMapPoiKind.Kombini:
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -1f) * num, new Vector2(13f, 11f) * num, MiniMapPoiPainter.Ink);
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, 4.5f) * num, new Vector2(15f, 2.5f) * num, MiniMapPoiPainter.Border);
				MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -2f) * num, new Vector2(4f, 7f) * num, MiniMapPoiPainter.ColorFor(kind));
				return;
			default:
				return;
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x001A7194 File Offset: 0x001A5394
		internal static void DrawUnknown(VertexHelper vh, Vector2 center, float radius)
		{
			Color color = new Color(0.2f, 0.22f, 0.25f, 0.92f);
			Color color2 = new Color(0.62f, 0.66f, 0.69f, 0.88f);
			MiniMapPoiPainter.AddDisc(vh, center, radius + 1.4f, MiniMapPoiPainter.Border, 16);
			MiniMapPoiPainter.AddDisc(vh, center, radius, color, 16);
			float num = radius / 12f;
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(-3.2f, 4.2f) * num, center + new Vector2(0f, 6.2f) * num, 2.2f * num, color2);
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(0f, 6.2f) * num, center + new Vector2(3.4f, 3.3f) * num, 2.2f * num, color2);
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(3.4f, 3.3f) * num, center + new Vector2(0.2f, -0.4f) * num, 2.2f * num, color2);
			MiniMapPoiPainter.AddRect(vh, center + new Vector2(0.2f, -5.2f) * num, new Vector2(2.3f, 2.3f) * num, color2);
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x001A7302 File Offset: 0x001A5502
		internal static void DrawSelectionHalo(VertexHelper vh, Vector2 center, float radius)
		{
			MiniMapPoiPainter.AddDisc(vh, center, radius + 5.5f, new Color(1f, 0.91f, 1f, 0.96f), 20);
			MiniMapPoiPainter.AddDisc(vh, center, radius + 2.8f, MiniMapPoiPainter.Border, 20);
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x001A7344 File Offset: 0x001A5544
		internal static void DrawHome(VertexHelper vh, Vector2 center, float radius, Vector2 outwardDirection)
		{
			Color color = new Color(1f, 0.91f, 0.94f, 0.96f);
			MiniMapPoiPainter.AddDisc(vh, center, radius + 3.8f, color, 20);
			MiniMapPoiPainter.Draw(vh, center, radius, MiniMapPoiKind.Home);
			if (outwardDirection.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Vector2 vector = new Vector2(-outwardDirection.y, outwardDirection.x);
			Vector2 vector2 = center + outwardDirection * (radius + 9f);
			Vector2 vector3 = center + outwardDirection * (radius + 5.5f);
			Vector2 vector4 = vector3 - vector * 3.5f;
			Vector2 vector5 = vector3 + vector * 3.5f;
			MiniMapPoiPainter.AddLine(vh, vector4, vector2, 4f, MiniMapPoiPainter.Border);
			MiniMapPoiPainter.AddLine(vh, vector2, vector5, 4f, MiniMapPoiPainter.Border);
			MiniMapPoiPainter.AddLine(vh, vector4, vector2, 2f, color);
			MiniMapPoiPainter.AddLine(vh, vector2, vector5, 2f, color);
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x001A7438 File Offset: 0x001A5638
		private static void DrawBag(VertexHelper vh, Vector2 center, float scale)
		{
			MiniMapPoiPainter.AddRect(vh, center + new Vector2(0f, -2f) * scale, new Vector2(13f, 11f) * scale, MiniMapPoiPainter.Ink);
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(-4f, 4f) * scale, center + new Vector2(-2f, 7f) * scale, 1.8f * scale, MiniMapPoiPainter.Ink);
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(-2f, 7f) * scale, center + new Vector2(2f, 7f) * scale, 1.8f * scale, MiniMapPoiPainter.Ink);
			MiniMapPoiPainter.AddLine(vh, center + new Vector2(2f, 7f) * scale, center + new Vector2(4f, 4f) * scale, 1.8f * scale, MiniMapPoiPainter.Ink);
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x001A7558 File Offset: 0x001A5758
		private static Color ColorFor(MiniMapPoiKind kind)
		{
			switch (kind)
			{
			case MiniMapPoiKind.Home:
				return new Color(0.9f, 0.35f, 0.42f, 1f);
			case MiniMapPoiKind.Park:
				return new Color(0.2f, 0.58f, 0.34f, 1f);
			case MiniMapPoiKind.Metro:
				return new Color(0.18f, 0.48f, 0.78f, 1f);
			case MiniMapPoiKind.University:
				return new Color(0.8f, 0.57f, 0.2f, 1f);
			case MiniMapPoiKind.Stadium:
				return new Color(0.72f, 0.26f, 0.28f, 1f);
			case MiniMapPoiKind.Mall:
				return new Color(0.78f, 0.4f, 0.18f, 1f);
			case MiniMapPoiKind.Gallery:
				return new Color(0.55f, 0.34f, 0.72f, 1f);
			case MiniMapPoiKind.Pool:
				return new Color(0.16f, 0.6f, 0.72f, 1f);
			case MiniMapPoiKind.Kombini:
				return new Color(0.2f, 0.66f, 0.48f, 1f);
			default:
				return new Color(0.45f, 0.5f, 0.54f, 1f);
			}
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x001A769C File Offset: 0x001A589C
		private static void AddDisc(VertexHelper vh, Vector2 center, float radius, Color color, int segments)
		{
			int currentVertCount = vh.currentVertCount;
			MiniMapPoiPainter.AddVertex(vh, center, color);
			for (int i = 0; i <= segments; i++)
			{
				float num = (float)i * 3.1415927f * 2f / (float)segments;
				MiniMapPoiPainter.AddVertex(vh, center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * radius, color);
			}
			for (int j = 0; j < segments; j++)
			{
				vh.AddTriangle(currentVertCount, currentVertCount + j + 1, currentVertCount + j + 2);
			}
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x001A771C File Offset: 0x001A591C
		private static void AddRect(VertexHelper vh, Vector2 center, Vector2 size, Color color)
		{
			Vector2 vector = size * 0.5f;
			MiniMapPoiPainter.AddQuad(vh, center + new Vector2(-vector.x, -vector.y), center + new Vector2(vector.x, -vector.y), center + new Vector2(vector.x, vector.y), center + new Vector2(-vector.x, vector.y), color);
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x001A779C File Offset: 0x001A599C
		private static void AddDiamond(VertexHelper vh, Vector2 center, float radius, Color color)
		{
			MiniMapPoiPainter.AddQuad(vh, center + Vector2.down * radius, center + Vector2.right * radius, center + Vector2.up * radius, center + Vector2.left * radius, color);
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x001A77F4 File Offset: 0x001A59F4
		private static void AddTriangle(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			MiniMapPoiPainter.AddVertex(vh, a, color);
			MiniMapPoiPainter.AddVertex(vh, b, color);
			MiniMapPoiPainter.AddVertex(vh, c, color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x001A7830 File Offset: 0x001A5A30
		private static void AddLine(VertexHelper vh, Vector2 a, Vector2 b, float width, Color color)
		{
			Vector2 vector = b - a;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Vector2 vector2 = new Vector2(-vector.y, vector.x).normalized * width * 0.5f;
			MiniMapPoiPainter.AddQuad(vh, a - vector2, b - vector2, b + vector2, a + vector2, color);
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x001A78A4 File Offset: 0x001A5AA4
		private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			MiniMapPoiPainter.AddVertex(vh, a, color);
			MiniMapPoiPainter.AddVertex(vh, b, color);
			MiniMapPoiPainter.AddVertex(vh, c, color);
			MiniMapPoiPainter.AddVertex(vh, d, color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x001A78F8 File Offset: 0x001A5AF8
		private static void AddVertex(VertexHelper vh, Vector2 position, Color color)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.position = position;
			simpleVert.color = color;
			simpleVert.uv0 = new Vector2(0.5f, 0.5f);
			vh.AddVert(simpleVert);
		}

		// Token: 0x0400458E RID: 17806
		private static readonly Color Ink = new Color(0.96f, 0.98f, 0.97f, 1f);

		// Token: 0x0400458F RID: 17807
		private static readonly Color Border = new Color(0.025f, 0.035f, 0.045f, 0.96f);
	}
}
