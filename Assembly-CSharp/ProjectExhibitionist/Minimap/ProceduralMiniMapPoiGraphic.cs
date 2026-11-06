using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B4 RID: 2484
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class ProceduralMiniMapPoiGraphic : MaskableGraphic
	{
		// Token: 0x060045D8 RID: 17880 RVA: 0x001A6814 File Offset: 0x001A4A14
		internal void SetPois(IReadOnlyList<MiniMapPoiVisual> pois)
		{
			this._pois = pois ?? Array.Empty<MiniMapPoiVisual>();
			this.SetVerticesDirty();
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x001A682C File Offset: 0x001A4A2C
		internal void SetExpanded(bool expanded)
		{
			if (this._expanded == expanded)
			{
				return;
			}
			this._expanded = expanded;
			this.SetVerticesDirty();
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x001A6845 File Offset: 0x001A4A45
		internal void SetView(Vector3 worldCenter, float heading, float pixelsPerMeter, float groundCompression)
		{
			this._worldCenter = worldCenter;
			this._heading = heading;
			this._pixelsPerMeter = pixelsPerMeter;
			this._groundCompression = groundCompression;
			this.SetVerticesDirty();
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x001A686C File Offset: 0x001A4A6C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			float num = (this._expanded ? 12f : 8.5f);
			Rect rect = base.rectTransform.rect;
			for (int i = 0; i < this._pois.Count; i++)
			{
				MiniMapPoiVisual miniMapPoiVisual = this._pois[i];
				if (miniMapPoiVisual.kind != MiniMapPoiKind.None && miniMapPoiVisual.kind != MiniMapPoiKind.Home)
				{
					Vector2 vector = this.Project(miniMapPoiVisual.center);
					float num2 = num + 2f;
					if (rect.width <= 0f || rect.height <= 0f || (vector.x >= rect.xMin - num2 && vector.x <= rect.xMax + num2 && vector.y >= rect.yMin - num2 && vector.y <= rect.yMax + num2))
					{
						if (miniMapPoiVisual.discovered)
						{
							MiniMapPoiPainter.Draw(vh, vector, num, miniMapPoiVisual.kind);
						}
						else
						{
							MiniMapPoiPainter.DrawUnknown(vh, vector, num);
						}
					}
				}
			}
			for (int j = 0; j < this._pois.Count; j++)
			{
				MiniMapPoiVisual miniMapPoiVisual2 = this._pois[j];
				if (miniMapPoiVisual2.kind == MiniMapPoiKind.Home)
				{
					Vector2 vector3;
					Vector2 vector2 = ProceduralMiniMapPoiGraphic.ResolveHomeMarkerPosition(this.Project(miniMapPoiVisual2.center), rect, num, out vector3);
					MiniMapPoiPainter.DrawHome(vh, vector2, num, vector3);
				}
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x001A69D8 File Offset: 0x001A4BD8
		internal static Vector2 ResolveHomeMarkerPosition(Vector2 projectedCenter, Rect viewport, float iconRadius, out Vector2 outwardDirection)
		{
			outwardDirection = Vector2.zero;
			if (viewport.width <= 0f || viewport.height <= 0f)
			{
				return projectedCenter;
			}
			float num = Mathf.Max(0f, Mathf.Min(viewport.width, viewport.height) * 0.5f - iconRadius - 12f);
			Vector2 vector = projectedCenter - viewport.center;
			if (vector.sqrMagnitude <= num * num)
			{
				return projectedCenter;
			}
			outwardDirection = vector.normalized;
			return viewport.center + outwardDirection * num;
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x001A6A7C File Offset: 0x001A4C7C
		private Vector2 Project(Vector3 world)
		{
			float num = world.x - this._worldCenter.x;
			float num2 = world.z - this._worldCenter.z;
			float num3 = this._heading * 0.017453292f;
			float num4 = Mathf.Cos(num3);
			float num5 = Mathf.Sin(num3);
			float num6 = num * num4 - num2 * num5;
			float num7 = num * num5 + num2 * num4;
			return new Vector2(num6 * this._pixelsPerMeter, num7 * this._pixelsPerMeter * this._groundCompression);
		}

		// Token: 0x04004584 RID: 17796
		private IReadOnlyList<MiniMapPoiVisual> _pois = Array.Empty<MiniMapPoiVisual>();

		// Token: 0x04004585 RID: 17797
		private Vector3 _worldCenter;

		// Token: 0x04004586 RID: 17798
		private float _heading;

		// Token: 0x04004587 RID: 17799
		private float _pixelsPerMeter = 1f;

		// Token: 0x04004588 RID: 17800
		private float _groundCompression = 0.7f;

		// Token: 0x04004589 RID: 17801
		private bool _expanded;
	}
}
