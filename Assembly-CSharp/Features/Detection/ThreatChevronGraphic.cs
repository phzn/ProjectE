using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x02001278 RID: 4728
	internal sealed class ThreatChevronGraphic : MaskableGraphic
	{
		// Token: 0x060092F6 RID: 37622 RVA: 0x003C57A8 File Offset: 0x003C39A8
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.color;
			for (int i = 0; i < 12; i++)
			{
				float num = Mathf.Lerp(-15f, 15f, (float)i / 12f) * 0.017453292f;
				float num2 = Mathf.Lerp(-15f, 15f, (float)(i + 1) / 12f) * 0.017453292f;
				Vector2 vector = new Vector2(Mathf.Sin(num), Mathf.Cos(num));
				Vector2 vector2 = new Vector2(Mathf.Sin(num2), Mathf.Cos(num2));
				ThreatChevronGraphic.AddQuad(simpleVert, vector * 109f, vector * 113f, vector2 * 113f, vector2 * 109f, vh);
			}
			ThreatChevronGraphic.AddQuad(simpleVert, new Vector2(-11f, 125f), new Vector2(0f, 135f), new Vector2(0f, 130f), new Vector2(-8f, 122f), vh);
			ThreatChevronGraphic.AddQuad(simpleVert, new Vector2(0f, 135f), new Vector2(11f, 125f), new Vector2(8f, 122f), new Vector2(0f, 130f), vh);
		}

		// Token: 0x060092F7 RID: 37623 RVA: 0x003C5908 File Offset: 0x003C3B08
		private static void AddQuad(UIVertex vertex, Vector2 a, Vector2 b, Vector2 c, Vector2 d, VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			ThreatChevronGraphic.Add(vertex, a, vh);
			ThreatChevronGraphic.Add(vertex, b, vh);
			ThreatChevronGraphic.Add(vertex, c, vh);
			ThreatChevronGraphic.Add(vertex, d, vh);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x003C595E File Offset: 0x003C3B5E
		private static void Add(UIVertex vertex, Vector2 position, VertexHelper vh)
		{
			vertex.position = position;
			vh.AddVert(vertex);
		}
	}
}
