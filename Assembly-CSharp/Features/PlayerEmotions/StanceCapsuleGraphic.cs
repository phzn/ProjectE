using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E18 RID: 3608
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class StanceCapsuleGraphic : MaskableGraphic
	{
		// Token: 0x06006B5F RID: 27487 RVA: 0x002AFDB7 File Offset: 0x002ADFB7
		public void SetBorderWidth(float value)
		{
			this.borderWidth = Mathf.Max(0f, value);
			this.SetVerticesDirty();
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x002AFDD0 File Offset: 0x002ADFD0
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			if (pixelAdjustedRect.width <= 0f || pixelAdjustedRect.height <= 0f)
			{
				return;
			}
			float num = Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height) * 0.5f;
			float num2 = Mathf.Min(this.borderWidth, num);
			if (num2 <= 0f)
			{
				StanceCapsuleGraphic.AddVertex(vh, pixelAdjustedRect.center, this.color);
				for (int i = 0; i < 50; i++)
				{
					StanceCapsuleGraphic.AddVertex(vh, StanceCapsuleGraphic.Point(pixelAdjustedRect, num, i), this.color);
				}
				for (int j = 0; j < 50; j++)
				{
					vh.AddTriangle(0, j + 1, (j + 1) % 50 + 1);
				}
				return;
			}
			Rect rect = new Rect(pixelAdjustedRect.x + num2, pixelAdjustedRect.y + num2, pixelAdjustedRect.width - num2 * 2f, pixelAdjustedRect.height - num2 * 2f);
			for (int k = 0; k < 50; k++)
			{
				StanceCapsuleGraphic.AddVertex(vh, StanceCapsuleGraphic.Point(pixelAdjustedRect, num, k), this.color);
				StanceCapsuleGraphic.AddVertex(vh, StanceCapsuleGraphic.Point(rect, num - num2, k), this.color);
			}
			for (int l = 0; l < 50; l++)
			{
				int num3 = l * 2;
				int num4 = (l + 1) % 50 * 2;
				vh.AddTriangle(num3, num4, num3 + 1);
				vh.AddTriangle(num4, num4 + 1, num3 + 1);
			}
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x002AFF48 File Offset: 0x002AE148
		private static Vector2 Point(Rect rect, float radius, int index)
		{
			bool flag = index > 24;
			int num = (flag ? (index - 24 - 1) : index);
			float num2 = (-90f + (flag ? 180f : 0f) + 180f * (float)num / 24f) * 0.017453292f;
			return new Vector2(flag ? (rect.xMin + radius) : (rect.xMax - radius), rect.center.y) + new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * radius;
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x002AFFD8 File Offset: 0x002AE1D8
		private static void AddVertex(VertexHelper vh, Vector2 position, Color color)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.position = position;
			simpleVert.color = color;
			simpleVert.uv0 = new Vector2(0.5f, 0.5f);
			vh.AddVert(simpleVert);
		}

		// Token: 0x04006E16 RID: 28182
		[SerializeField]
		[Min(0f)]
		private float borderWidth;

		// Token: 0x04006E17 RID: 28183
		private const int ArcSegments = 24;
	}
}
