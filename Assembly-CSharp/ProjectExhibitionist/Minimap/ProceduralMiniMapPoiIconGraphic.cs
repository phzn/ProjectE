using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B5 RID: 2485
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class ProceduralMiniMapPoiIconGraphic : MaskableGraphic
	{
		// Token: 0x060045DF RID: 17887 RVA: 0x001A6B1D File Offset: 0x001A4D1D
		internal void SetKind(MiniMapPoiKind kind)
		{
			this._kind = kind;
			this._unknown = false;
			this._selected = false;
			this._radius = 12f;
			this.SetVerticesDirty();
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x001A6B45 File Offset: 0x001A4D45
		internal void ConfigureMapMarker(MiniMapPoiKind kind, bool unknown, bool selected, float radius)
		{
			this._kind = kind;
			this._unknown = unknown;
			this._selected = selected;
			this._radius = Mathf.Max(6f, radius);
			this.SetVerticesDirty();
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x001A6B74 File Offset: 0x001A4D74
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (this._kind == MiniMapPoiKind.None)
			{
				return;
			}
			Vector2 center = base.rectTransform.rect.center;
			if (this._selected)
			{
				MiniMapPoiPainter.DrawSelectionHalo(vh, center, this._radius);
			}
			if (this._unknown)
			{
				MiniMapPoiPainter.DrawUnknown(vh, center, this._radius);
				return;
			}
			MiniMapPoiPainter.Draw(vh, center, this._radius, this._kind);
		}

		// Token: 0x0400458A RID: 17802
		private MiniMapPoiKind _kind;

		// Token: 0x0400458B RID: 17803
		private float _radius = 12f;

		// Token: 0x0400458C RID: 17804
		private bool _unknown;

		// Token: 0x0400458D RID: 17805
		private bool _selected;
	}
}
