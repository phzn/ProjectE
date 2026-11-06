using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009AE RID: 2478
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class ProceduralMiniMapGraphic : MaskableGraphic
	{
		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x060045A1 RID: 17825 RVA: 0x001A4B63 File Offset: 0x001A2D63
		public override Texture mainTexture
		{
			get
			{
				if (!(this._styleAtlas != null) || !(this._styleAtlas.Atlas != null))
				{
					return base.mainTexture;
				}
				return this._styleAtlas.Atlas;
			}
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x001A4B98 File Offset: 0x001A2D98
		internal void SetStyleAtlas(ProceduralMiniMapStyleAtlas styleAtlas)
		{
			this._styleAtlas = styleAtlas;
			this.SetMaterialDirty();
			this.SetVerticesDirty();
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x001A4BAD File Offset: 0x001A2DAD
		internal void SetChunks(IReadOnlyList<MiniMapChunkVisual> chunks)
		{
			this._chunks = chunks ?? Array.Empty<MiniMapChunkVisual>();
			this.SetVerticesDirty();
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001A4BC5 File Offset: 0x001A2DC5
		public void SetExpanded(bool expanded)
		{
			if (this._expanded == expanded)
			{
				return;
			}
			this._expanded = expanded;
			this.SetVerticesDirty();
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x001A4BDE File Offset: 0x001A2DDE
		public void SetView(Vector3 worldCenter, float heading, float pixelsPerMeter, float groundCompression)
		{
			this._fitToViewport = false;
			this._worldCenter = worldCenter;
			this._heading = heading;
			this._pixelsPerMeter = pixelsPerMeter;
			this._groundCompression = groundCompression;
			this.SetVerticesDirty();
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x001A4C0C File Offset: 0x001A2E0C
		internal void SetFittedView(Vector3 worldCenter, float heading, Vector2 worldSize, float groundCompression, float padding)
		{
			this._fitToViewport = true;
			this._worldCenter = worldCenter;
			this._heading = heading;
			this._fitWorldSize = new Vector2(Mathf.Max(1f, worldSize.x), Mathf.Max(1f, worldSize.y));
			this._groundCompression = groundCompression;
			this._fitPadding = Mathf.Max(0f, padding);
			this.UpdateFittedScale();
			this.SetVerticesDirty();
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x001A4C7F File Offset: 0x001A2E7F
		internal Vector2 ProjectWorldToLocal(Vector3 worldPosition)
		{
			this.UpdateFittedScale();
			return this.Project(worldPosition);
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x001A4C90 File Offset: 0x001A2E90
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (this._chunks == null || this._chunks.Count == 0)
			{
				return;
			}
			this.UpdateFittedScale();
			this.CollectVisibleChunks();
			if (this._visibleChunksScratch.Count == 0)
			{
				return;
			}
			this.DrawZones(vh);
			this.DrawRoadNetworkLayer(vh, false);
			this.DrawRoadNetworkLayer(vh, true);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x001A4CEC File Offset: 0x001A2EEC
		private void CollectVisibleChunks()
		{
			this._visibleChunksScratch.Clear();
			for (int i = 0; i < this._chunks.Count; i++)
			{
				MiniMapChunkVisual miniMapChunkVisual = this._chunks[i];
				if (miniMapChunkVisual != null && this.IsChunkInsideViewport(miniMapChunkVisual))
				{
					this._visibleChunksScratch.Add(miniMapChunkVisual);
				}
			}
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x001A4D40 File Offset: 0x001A2F40
		private bool IsChunkInsideViewport(MiniMapChunkVisual chunk)
		{
			Rect rect = base.rectTransform.rect;
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return true;
			}
			float num = chunk.size.magnitude * 0.5f + 10f;
			for (int i = 0; i < chunk.zones.Count; i++)
			{
				MiniMapZoneVisual miniMapZoneVisual = chunk.zones[i];
				float num2 = miniMapZoneVisual.center.x - chunk.center.x;
				float num3 = miniMapZoneVisual.center.z - chunk.center.z;
				float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3) + miniMapZoneVisual.size.magnitude * 0.5f;
				if (miniMapZoneVisual.tile.valid)
				{
					num4 += miniMapZoneVisual.tile.centerOffset.magnitude;
				}
				num = Mathf.Max(num, num4);
			}
			float num5 = Mathf.Abs(this._pixelsPerMeter) * Mathf.Max(1f, Mathf.Abs(this._groundCompression));
			return ProceduralMiniMapGraphic.CircleIntersectsRect(rect, this.Project(chunk.center), num * num5);
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x001A4E74 File Offset: 0x001A3074
		private static bool CircleIntersectsRect(Rect rect, Vector2 center, float radius)
		{
			radius = Mathf.Max(0f, radius);
			return center.x + radius >= rect.xMin && center.x - radius <= rect.xMax && center.y + radius >= rect.yMin && center.y - radius <= rect.yMax;
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x001A4ED8 File Offset: 0x001A30D8
		private void DrawRoadNetworkLayer(VertexHelper vh, bool asphalt)
		{
			for (int i = 0; i < this._visibleChunksScratch.Count; i++)
			{
				MiniMapChunkVisual miniMapChunkVisual = this._visibleChunksScratch[i];
				this.DrawRoadNetwork(vh, miniMapChunkVisual, asphalt);
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x001A4F14 File Offset: 0x001A3114
		private void DrawRoadNetwork(VertexHelper vh, MiniMapChunkVisual chunk, bool asphalt)
		{
			float num = chunk.size.x * 0.5f;
			float num2 = chunk.size.y * 0.5f;
			Vector2 vector = new Vector2(chunk.size.x + 9f, 9f);
			Vector2 vector2 = new Vector2(9f, chunk.size.y + 9f);
			this.DrawRoad(vh, ProceduralMiniMapGraphic.Offset(chunk.center, chunk.yaw, 0f, num2), vector, chunk.yaw, asphalt);
			this.DrawRoad(vh, ProceduralMiniMapGraphic.Offset(chunk.center, chunk.yaw, 0f, -num2), vector, chunk.yaw, asphalt);
			this.DrawRoad(vh, ProceduralMiniMapGraphic.Offset(chunk.center, chunk.yaw, num, 0f), vector2, chunk.yaw, asphalt);
			this.DrawRoad(vh, ProceduralMiniMapGraphic.Offset(chunk.center, chunk.yaw, -num, 0f), vector2, chunk.yaw, asphalt);
			ChunkShape shape = chunk.shape;
			if (shape == ChunkShape.XShape)
			{
				this.DrawRoad(vh, chunk.center, new Vector2(chunk.size.x, 9f), chunk.yaw, asphalt);
				this.DrawRoad(vh, chunk.center, new Vector2(9f, chunk.size.y), chunk.yaw, asphalt);
				return;
			}
			if (shape != ChunkShape.TShape)
			{
				return;
			}
			this.DrawRoad(vh, chunk.center, new Vector2(9f, chunk.size.y), chunk.yaw, asphalt);
			float num3 = chunk.size.x * 0.5f;
			Vector3 vector3 = ProceduralMiniMapGraphic.Offset(chunk.center, chunk.yaw, chunk.size.x * 0.25f, 0f);
			this.DrawRoad(vh, vector3, new Vector2(num3, 9f), chunk.yaw, asphalt);
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x001A5100 File Offset: 0x001A3300
		private void DrawRoad(VertexHelper vh, Vector3 center, Vector2 size, float yaw, bool asphalt)
		{
			bool flag = size.x >= size.y;
			Vector2 vector = size;
			if (flag)
			{
				vector.x += 1.5f;
			}
			else
			{
				vector.y += 1.5f;
			}
			if (asphalt)
			{
				if (flag)
				{
					vector.y = Mathf.Max(1f, vector.y - 2.4f);
				}
				else
				{
					vector.x = Mathf.Max(1f, vector.x - 2.4f);
				}
			}
			this.AddWorldRect(vh, center, vector, yaw, asphalt ? ProceduralMiniMapGraphic.Asphalt : ProceduralMiniMapGraphic.Sidewalk);
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x001A51A4 File Offset: 0x001A33A4
		private void DrawZones(VertexHelper vh)
		{
			for (int i = 0; i < this._visibleChunksScratch.Count; i++)
			{
				MiniMapChunkVisual miniMapChunkVisual = this._visibleChunksScratch[i];
				for (int j = 0; j < miniMapChunkVisual.zones.Count; j++)
				{
					this.DrawZone(vh, miniMapChunkVisual.zones[j], miniMapChunkVisual.previewOnly ? ProceduralMiniMapGraphic.PreviewTint : Color.white);
				}
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x001A5214 File Offset: 0x001A3414
		private void DrawZone(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			if (zone.tile.valid)
			{
				Vector3 vector = ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, zone.tile.centerOffset.x, zone.tile.centerOffset.y);
				this.AddWorldTexturedRect(vh, vector, zone.tile.worldSize, zone.yaw, zone.tile.uv, tint);
				if (zone.kind == MiniMapZoneKind.Pool)
				{
					this.DrawPool(vh, zone, tint);
				}
				return;
			}
			switch (zone.kind)
			{
			case MiniMapZoneKind.University:
				this.DrawUniversity(vh, zone, tint);
				return;
			case MiniMapZoneKind.Stadium:
				this.DrawStadium(vh, zone, tint);
				return;
			case MiniMapZoneKind.Mall:
				this.DrawMall(vh, zone, tint);
				return;
			case MiniMapZoneKind.Gallery:
				this.DrawGallery(vh, zone, tint);
				return;
			case MiniMapZoneKind.Pool:
				this.DrawPool(vh, zone, tint);
				return;
			default:
			{
				if (zone.kind == MiniMapZoneKind.Park || zone.kind == MiniMapZoneKind.Sports)
				{
					Color color = ProceduralMiniMapGraphic.Tint(ProceduralMiniMapGraphic.GetZonePadColor(zone.kind), tint);
					this.AddWorldRect(vh, zone.center, zone.size * 0.94f, zone.yaw, color);
				}
				uint stableHash = (uint)zone.stableHash;
				MiniMapZoneKind kind = zone.kind;
				if (kind == MiniMapZoneKind.Park)
				{
					this.DrawPark(vh, zone, tint, ref stableHash);
					return;
				}
				if (kind != MiniMapZoneKind.Sports)
				{
					return;
				}
				this.DrawSports(vh, zone, tint);
				return;
			}
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x001A5364 File Offset: 0x001A3564
		private void DrawPark(VertexHelper vh, MiniMapZoneVisual zone, Color tint, ref uint random)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.64f, 0.59f, 0.46f, 0.82f), tint);
			this.AddWorldRect(vh, zone.center, new Vector2(zone.size.x * 0.72f, 1.6f), zone.yaw + 18f, color);
			this.AddWorldRect(vh, zone.center, new Vector2(1.5f, zone.size.y * 0.66f), zone.yaw - 12f, color);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.2f, 0.43f, 0.29f, 1f), tint);
			int num = Mathf.Clamp(Mathf.RoundToInt((zone.size.x + zone.size.y) / 13f), 4, 18);
			for (int i = 0; i < num; i++)
			{
				float num2 = (ProceduralMiniMapGraphic.Next01(ref random) - 0.5f) * zone.size.x * 0.76f;
				float num3 = (ProceduralMiniMapGraphic.Next01(ref random) - 0.5f) * zone.size.y * 0.76f;
				Vector3 vector = ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, num2, num3);
				float num4 = Mathf.Lerp(1.2f, 2.6f, ProceduralMiniMapGraphic.Next01(ref random));
				this.AddWorldDisc(vh, vector, num4, color2);
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x001A54D8 File Offset: 0x001A36D8
		private void DrawSports(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Vector2 vector = zone.size * 0.72f;
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.88f, 0.9f, 0.79f, 0.82f), tint);
			this.DrawRectOutline(vh, zone.center, vector, zone.yaw, 0.65f, color);
			this.AddWorldRect(vh, zone.center, new Vector2(0.55f, vector.y), zone.yaw, color);
			this.AddWorldDisc(vh, zone.center, Mathf.Min(vector.x, vector.y) * 0.1f, new Color(color.r, color.g, color.b, color.a * 0.36f));
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x001A559C File Offset: 0x001A379C
		private void DrawUniversity(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.25f, 0.43f, 0.31f, 1f), tint);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.73f, 0.76f, 0.7f, 1f), tint);
			Color color3 = ProceduralMiniMapGraphic.Tint(new Color(0.31f, 0.35f, 0.37f, 1f), tint);
			this.AddWorldRect(vh, zone.center, zone.size * 0.94f, zone.yaw, color);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, 0f, 11f), new Vector2(34f, 12f), zone.yaw, color2);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, -13f, -7f), new Vector2(10f, 25f), zone.yaw, color2);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, 13f, -7f), new Vector2(10f, 25f), zone.yaw, color2);
			this.AddWorldRect(vh, zone.center, new Vector2(7f, 7f), zone.yaw + 45f, color3);
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x001A5700 File Offset: 0x001A3900
		private void DrawStadium(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.68f, 0.7f, 0.7f, 1f), tint);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.25f, 0.29f, 0.32f, 1f), tint);
			Color color3 = ProceduralMiniMapGraphic.Tint(new Color(0.26f, 0.49f, 0.31f, 1f), tint);
			Color color4 = ProceduralMiniMapGraphic.Tint(new Color(0.89f, 0.91f, 0.83f, 0.9f), tint);
			this.AddWorldCapsule(vh, zone.center, zone.size * 0.96f, zone.yaw, color);
			this.AddWorldCapsule(vh, zone.center, zone.size * 0.77f, zone.yaw, color2);
			this.AddWorldCapsule(vh, zone.center, zone.size * 0.57f, zone.yaw, color3);
			this.AddWorldRect(vh, zone.center, new Vector2(0.7f, zone.size.y * 0.5f), zone.yaw, color4);
			this.AddWorldDisc(vh, zone.center, 3.3f, new Color(color4.r, color4.g, color4.b, 0.34f));
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x001A5858 File Offset: 0x001A3A58
		private void DrawMall(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.72f, 0.6f, 0.4f, 1f), tint);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.34f, 0.32f, 0.3f, 1f), tint);
			Color color3 = ProceduralMiniMapGraphic.Tint(new Color(0.43f, 0.65f, 0.7f, 1f), tint);
			this.AddWorldRect(vh, zone.center, zone.size * 0.92f, zone.yaw, color);
			this.AddWorldRect(vh, zone.center, zone.size * 0.48f, zone.yaw, color2);
			this.AddWorldRect(vh, zone.center, new Vector2(zone.size.x * 0.13f, zone.size.y * 0.56f), zone.yaw, color3);
			this.AddWorldRect(vh, zone.center, new Vector2(zone.size.x * 0.56f, zone.size.y * 0.13f), zone.yaw, color3);
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x001A5984 File Offset: 0x001A3B84
		private void DrawGallery(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.57f, 0.56f, 0.53f, 1f), tint);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.79f, 0.8f, 0.76f, 1f), tint);
			Color color3 = ProceduralMiniMapGraphic.Tint(new Color(0.35f, 0.57f, 0.65f, 1f), tint);
			this.AddWorldRect(vh, zone.center, zone.size * 0.94f, zone.yaw, color);
			this.AddWorldRect(vh, zone.center, zone.size * 0.66f, zone.yaw + 45f, color2);
			this.AddWorldRect(vh, zone.center, zone.size * 0.3f, zone.yaw + 45f, color3);
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x001A5A6C File Offset: 0x001A3C6C
		private void DrawPool(VertexHelper vh, MiniMapZoneVisual zone, Color tint)
		{
			Color color = ProceduralMiniMapGraphic.Tint(new Color(0.65f, 0.7f, 0.69f, 1f), tint);
			Color color2 = ProceduralMiniMapGraphic.Tint(new Color(0.28f, 0.62f, 0.72f, 1f), tint);
			Color color3 = ProceduralMiniMapGraphic.Tint(new Color(0.82f, 0.91f, 0.91f, 0.82f), tint);
			this.AddWorldRect(vh, zone.center, zone.size * 0.94f, zone.yaw, color);
			Vector2 vector = new Vector2(zone.size.x * 0.76f, zone.size.y * 0.66f);
			this.AddWorldRect(vh, zone.center, vector, zone.yaw, color2);
			for (int i = -2; i <= 2; i++)
			{
				this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(zone.center, zone.yaw, (float)i * vector.x / 6f, 0f), new Vector2(0.45f, vector.y * 0.92f), zone.yaw, color3);
			}
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x001A5B98 File Offset: 0x001A3D98
		private void UpdateFittedScale()
		{
			if (!this._fitToViewport)
			{
				return;
			}
			Rect rect = base.rectTransform.rect;
			float num = Mathf.Max(1f, rect.width - this._fitPadding * 2f);
			float num2 = Mathf.Max(1f, rect.height - this._fitPadding * 2f);
			float num3 = num / this._fitWorldSize.x;
			float num4 = num2 / (this._fitWorldSize.y * Mathf.Max(0.01f, Mathf.Abs(this._groundCompression)));
			this._pixelsPerMeter = Mathf.Max(0.01f, Mathf.Min(num3, num4));
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x001A5C3F File Offset: 0x001A3E3F
		private static Color Tint(Color color, Color tint)
		{
			return new Color(color.r * tint.r, color.g * tint.g, color.b * tint.b, color.a * tint.a);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x001A5C7C File Offset: 0x001A3E7C
		private void AddWorldRect(VertexHelper vh, Vector3 center, Vector2 size, float yaw, Color color)
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			ProceduralMiniMapGraphic.GetWorldCorners(center, size, yaw, out vector, out vector2, out vector3, out vector4);
			ProceduralMiniMapGraphic.AddQuad(vh, this.Project(vector), this.Project(vector2), this.Project(vector3), this.Project(vector4), color);
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x001A5CC0 File Offset: 0x001A3EC0
		private void AddWorldCapsule(VertexHelper vh, Vector3 center, Vector2 size, float yaw, Color color)
		{
			bool flag = size.x >= size.y;
			float num = (flag ? size.y : size.x);
			float num2 = num * 0.5f;
			float num3 = Mathf.Max(0f, (flag ? size.x : size.y) - num);
			Vector2 vector = (flag ? new Vector2(num3, num) : new Vector2(num, num3));
			this.AddWorldRect(vh, center, vector, yaw, color);
			float num4 = num3 * 0.5f;
			Vector3 vector2 = (flag ? ProceduralMiniMapGraphic.Offset(center, yaw, -num4, 0f) : ProceduralMiniMapGraphic.Offset(center, yaw, 0f, -num4));
			Vector3 vector3 = (flag ? ProceduralMiniMapGraphic.Offset(center, yaw, num4, 0f) : ProceduralMiniMapGraphic.Offset(center, yaw, 0f, num4));
			this.AddWorldDisc(vh, vector2, num2, color);
			this.AddWorldDisc(vh, vector3, num2, color);
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x001A5DA8 File Offset: 0x001A3FA8
		private void AddWorldTexturedRect(VertexHelper vh, Vector3 center, Vector2 size, float yaw, Rect uv, Color color)
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			ProceduralMiniMapGraphic.GetWorldCorners(center, size, yaw, out vector, out vector2, out vector3, out vector4);
			ProceduralMiniMapGraphic.AddTexturedQuad(vh, this.Project(vector), this.Project(vector2), this.Project(vector3), this.Project(vector4), new Vector2(uv.xMin, uv.yMin), new Vector2(uv.xMax, uv.yMin), new Vector2(uv.xMax, uv.yMax), new Vector2(uv.xMin, uv.yMax), color);
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x001A5E38 File Offset: 0x001A4038
		private void DrawRectOutline(VertexHelper vh, Vector3 center, Vector2 size, float yaw, float width, Color color)
		{
			float num = size.x * 0.5f;
			float num2 = size.y * 0.5f;
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(center, yaw, 0f, num2), new Vector2(size.x, width), yaw, color);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(center, yaw, 0f, -num2), new Vector2(size.x, width), yaw, color);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(center, yaw, num, 0f), new Vector2(width, size.y), yaw, color);
			this.AddWorldRect(vh, ProceduralMiniMapGraphic.Offset(center, yaw, -num, 0f), new Vector2(width, size.y), yaw, color);
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x001A5EFC File Offset: 0x001A40FC
		private void AddWorldDisc(VertexHelper vh, Vector3 center, float radius, Color color)
		{
			Vector2 vector = this.Project(center);
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapGraphic.AddVertex(vh, vector, color);
			for (int i = 0; i <= 10; i++)
			{
				float num = (float)i * 3.1415927f * 2f / 10f;
				Vector3 vector2 = center + new Vector3(Mathf.Cos(num) * radius, 0f, Mathf.Sin(num) * radius);
				ProceduralMiniMapGraphic.AddVertex(vh, this.Project(vector2), color);
			}
			for (int j = 0; j < 10; j++)
			{
				vh.AddTriangle(currentVertCount, currentVertCount + j + 1, currentVertCount + j + 2);
			}
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x001A5F9C File Offset: 0x001A419C
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

		// Token: 0x060045C0 RID: 17856 RVA: 0x001A6014 File Offset: 0x001A4214
		private static void GetWorldCorners(Vector3 center, Vector2 size, float yaw, out Vector3 world0, out Vector3 world1, out Vector3 world2, out Vector3 world3)
		{
			float num = size.x * 0.5f;
			float num2 = size.y * 0.5f;
			world0 = ProceduralMiniMapGraphic.Offset(center, yaw, -num, -num2);
			world1 = ProceduralMiniMapGraphic.Offset(center, yaw, num, -num2);
			world2 = ProceduralMiniMapGraphic.Offset(center, yaw, num, num2);
			world3 = ProceduralMiniMapGraphic.Offset(center, yaw, -num, num2);
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x001A6080 File Offset: 0x001A4280
		private static Vector3 Offset(Vector3 center, float yaw, float localX, float localZ)
		{
			float num = yaw * 0.017453292f;
			float num2 = Mathf.Cos(num);
			float num3 = Mathf.Sin(num);
			center.x += localX * num2 + localZ * num3;
			center.z += -localX * num3 + localZ * num2;
			return center;
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x001A60C8 File Offset: 0x001A42C8
		private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapGraphic.AddVertex(vh, a, color);
			ProceduralMiniMapGraphic.AddVertex(vh, b, color);
			ProceduralMiniMapGraphic.AddVertex(vh, c, color);
			ProceduralMiniMapGraphic.AddVertex(vh, d, color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x001A611C File Offset: 0x001A431C
		private static void AddTexturedQuad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, Color color)
		{
			int currentVertCount = vh.currentVertCount;
			ProceduralMiniMapGraphic.AddVertex(vh, a, uvA, color);
			ProceduralMiniMapGraphic.AddVertex(vh, b, uvB, color);
			ProceduralMiniMapGraphic.AddVertex(vh, c, uvC, color);
			ProceduralMiniMapGraphic.AddVertex(vh, d, uvD, color);
			vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x001A6177 File Offset: 0x001A4377
		private static void AddVertex(VertexHelper vh, Vector2 position, Color color)
		{
			ProceduralMiniMapGraphic.AddVertex(vh, position, new Vector2(0.9375f, 0.9375f), color);
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x001A6190 File Offset: 0x001A4390
		private static void AddVertex(VertexHelper vh, Vector2 position, Vector2 uv, Color color)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.position = position;
			simpleVert.color = color;
			simpleVert.uv0 = uv;
			vh.AddVert(simpleVert);
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x001A61D4 File Offset: 0x001A43D4
		private static Color GetZonePadColor(MiniMapZoneKind kind)
		{
			switch (kind)
			{
			case MiniMapZoneKind.City:
				return new Color(0.3f, 0.31f, 0.34f, 1f);
			case MiniMapZoneKind.Residential:
				return new Color(0.34f, 0.35f, 0.37f, 1f);
			case MiniMapZoneKind.Suburban:
				return new Color(0.32f, 0.39f, 0.34f, 1f);
			case MiniMapZoneKind.Commercial:
				return new Color(0.42f, 0.35f, 0.29f, 1f);
			case MiniMapZoneKind.Park:
				return new Color(0.255f, 0.46f, 0.315f, 1f);
			case MiniMapZoneKind.Sports:
				return new Color(0.3f, 0.49f, 0.32f, 1f);
			case MiniMapZoneKind.Subway:
				return new Color(0.27f, 0.37f, 0.43f, 1f);
			default:
				return new Color(0.31f, 0.34f, 0.35f, 1f);
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x001A62D9 File Offset: 0x001A44D9
		private static float Next01(ref uint state)
		{
			state ^= state << 13;
			state ^= state >> 17;
			state ^= state << 5;
			return (state & 16777215U) / 16777215f;
		}

		// Token: 0x04004559 RID: 17753
		private static readonly Color Sidewalk = new Color(0.47f, 0.5f, 0.5f, 1f);

		// Token: 0x0400455A RID: 17754
		private static readonly Color Asphalt = new Color(0.3f, 0.33f, 0.36f, 1f);

		// Token: 0x0400455B RID: 17755
		private static readonly Color PreviewTint = new Color(0.8f, 0.82f, 0.86f, 0.62f);

		// Token: 0x0400455C RID: 17756
		private IReadOnlyList<MiniMapChunkVisual> _chunks = Array.Empty<MiniMapChunkVisual>();

		// Token: 0x0400455D RID: 17757
		private readonly List<MiniMapChunkVisual> _visibleChunksScratch = new List<MiniMapChunkVisual>(64);

		// Token: 0x0400455E RID: 17758
		private Vector3 _worldCenter;

		// Token: 0x0400455F RID: 17759
		private float _heading;

		// Token: 0x04004560 RID: 17760
		private float _pixelsPerMeter = 1.4f;

		// Token: 0x04004561 RID: 17761
		private float _groundCompression = 0.62f;

		// Token: 0x04004562 RID: 17762
		private bool _expanded;

		// Token: 0x04004563 RID: 17763
		private ProceduralMiniMapStyleAtlas _styleAtlas;

		// Token: 0x04004564 RID: 17764
		private bool _fitToViewport;

		// Token: 0x04004565 RID: 17765
		private Vector2 _fitWorldSize;

		// Token: 0x04004566 RID: 17766
		private float _fitPadding;
	}
}
