using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x0200147C RID: 5244
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class MapsDirectionGraphic : MaskableGraphic
	{
		// Token: 0x0600A19E RID: 41374 RVA: 0x00430B91 File Offset: 0x0042ED91
		protected override void OnPopulateMesh(VertexHelper vertices)
		{
			vertices.Clear();
			MapsDirectionGraphic.AddArrow(vertices, 1.25f, new Color(0.035f, 0.06f, 0.09f));
			MapsDirectionGraphic.AddArrow(vertices, 1f, this.color);
		}

		// Token: 0x0600A19F RID: 41375 RVA: 0x00430BCC File Offset: 0x0042EDCC
		private static void AddArrow(VertexHelper vertices, float scale, Color tint)
		{
			int currentVertCount = vertices.currentVertCount;
			Vector2[] array = new Vector2[]
			{
				new Vector2(0f, 12f),
				new Vector2(-9f, -7f),
				new Vector2(0f, -3f),
				new Vector2(9f, -7f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.position = array[i] * scale;
				simpleVert.color = tint;
				vertices.AddVert(simpleVert);
			}
			vertices.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertices.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 3);
		}
	}
}
