using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A35 RID: 2613
	[RequireComponent(typeof(CanvasRenderer))]
	internal sealed class ControllerFocusGraphic : MaskableGraphic
	{
		// Token: 0x06004A3C RID: 19004 RVA: 0x001CC7D0 File Offset: 0x001CA9D0
		protected override void OnPopulateMesh(VertexHelper mesh)
		{
			mesh.Clear();
			Rect rect = base.rectTransform.rect;
			if (rect.width < 4f || rect.height < 4f)
			{
				return;
			}
			ControllerFocusGraphic.Ring(mesh, rect, 4.5f, new Color32(91, 38, 96, 140));
			Rect rect2 = new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 2f);
			ControllerFocusGraphic.Ring(mesh, rect2, 2f, new Color32(244, 91, 157, byte.MaxValue));
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x001CC888 File Offset: 0x001CAA88
		private static void Ring(VertexHelper mesh, Rect rect, float width, Color32 tint)
		{
			float num = Mathf.Min(10f, Mathf.Min(rect.width, rect.height) * 0.25f);
			int currentVertCount = mesh.currentVertCount;
			for (int i = 0; i < 4; i++)
			{
				Vector2 vector = new Vector2((i == 0 || i == 3) ? (rect.xMax - num) : (rect.xMin + num), (i < 2) ? (rect.yMax - num) : (rect.yMin + num));
				for (int j = 0; j <= 8; j++)
				{
					float num2 = ((float)i * 90f + (float)j * 90f / 8f) * 0.017453292f;
					Vector2 vector2 = new Vector2(Mathf.Cos(num2), Mathf.Sin(num2));
					mesh.AddVert(vector + vector2 * num, tint, Vector2.zero);
					mesh.AddVert(vector + vector2 * Mathf.Max(0f, num - width), tint, Vector2.zero);
				}
			}
			int num3 = 36;
			for (int k = 0; k < num3; k++)
			{
				int num4 = currentVertCount + k * 2;
				int num5 = currentVertCount + (k + 1) % num3 * 2;
				mesh.AddTriangle(num4, num5, num4 + 1);
				mesh.AddTriangle(num4 + 1, num5, num5 + 1);
			}
		}
	}
}
