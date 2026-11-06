using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B0 RID: 2480
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class ProceduralMiniMapMarkerGraphic : MaskableGraphic
	{
		// Token: 0x060045CC RID: 17868 RVA: 0x001A63AF File Offset: 0x001A45AF
		public void SetHeading(float heading)
		{
			if (Mathf.Abs(Mathf.DeltaAngle(this._heading, heading)) < 0.1f)
			{
				return;
			}
			this._heading = heading;
			base.rectTransform.localEulerAngles = new Vector3(0f, 0f, heading);
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x001A63EC File Offset: 0x001A45EC
		public void SetExpanded(bool expanded)
		{
			this.SetVerticesDirty();
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x001A63F4 File Offset: 0x001A45F4
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Color color = new Color(0.035f, 0.045f, 0.06f, 0.98f);
			ProceduralMiniMapMarkerGraphic.AddTriangle(vh, new Vector2(0f, 10f), new Vector2(-6.5f, -8f), new Vector2(0f, -5f), color);
			ProceduralMiniMapMarkerGraphic.AddTriangle(vh, new Vector2(0f, 10f), new Vector2(0f, -5f), new Vector2(6.5f, -8f), color);
			Color color2 = new Color(0.97f, 0.39f, 0.52f, 1f);
			ProceduralMiniMapMarkerGraphic.AddTriangle(vh, new Vector2(0f, 7.5f), new Vector2(-3.8f, -5f), new Vector2(0f, -3f), color2);
			ProceduralMiniMapMarkerGraphic.AddTriangle(vh, new Vector2(0f, 7.5f), new Vector2(0f, -3f), new Vector2(3.8f, -5f), color2);
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x001A6510 File Offset: 0x001A4710
		private static void AddTriangle(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapMarkerGraphic.AddVertex(vh, a, color);
			ProceduralMiniMapMarkerGraphic.AddVertex(vh, b, color);
			ProceduralMiniMapMarkerGraphic.AddVertex(vh, c, color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x001A654C File Offset: 0x001A474C
		private static void AddVertex(VertexHelper vh, Vector2 position, Color color)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.position = position;
			simpleVert.color = color;
			simpleVert.uv0 = new Vector2(0.5f, 0.5f);
			vh.AddVert(simpleVert);
		}

		// Token: 0x04004568 RID: 17768
		private float _heading;
	}
}
