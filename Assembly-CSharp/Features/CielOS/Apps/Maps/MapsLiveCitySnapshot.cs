using System;
using System.Collections.Generic;
using ProjectExhibitionist.Minimap;
using UnityEngine;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x0200147D RID: 5245
	internal sealed class MapsLiveCitySnapshot
	{
		// Token: 0x0600A1A1 RID: 41377 RVA: 0x00430C9C File Offset: 0x0042EE9C
		internal void Rebuild(ChunkManager manager, ProceduralMiniMapStyleAtlas atlas, Vector3 center, Vector2 worldSize, bool loadedOnly = false)
		{
			for (int i = 0; i < this.Chunks.Count; i++)
			{
				this.Chunks[i].zones.Clear();
				this._pool.Push(this.Chunks[i]);
			}
			for (int j = 0; j < this.PreviewChunks.Count; j++)
			{
				this.PreviewChunks[j].zones.Clear();
				this._pool.Push(this.PreviewChunks[j]);
			}
			this.Chunks.Clear();
			this.PreviewChunks.Clear();
			this.Pois.Clear();
			this._occupied.Clear();
			if (manager == null)
			{
				return;
			}
			float num = Mathf.Max(1f, manager.CellSize);
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in manager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (value != null && !(value.chunkRoot == null) && (!loadedOnly || (value.chunkRoot.activeInHierarchy && manager.IsChunkPresentationVisible(value))))
				{
					ChunkData chunkData = value.chunkData;
					Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : this.ResolveAuthoredSize(value.chunkRoot.transform, num));
					for (int k = 0; k < vector2Int.x; k++)
					{
						for (int l = 0; l < vector2Int.y; l++)
						{
							this._occupied.Add(keyValuePair.Key + new Vector2Int(k, l));
						}
					}
					ChunkTemplate chunkTemplate = ((chunkData != null) ? chunkData.template : null);
					string text = ((chunkData != null) ? chunkData.name : value.chunkRoot.name);
					MiniMapChunkVisual miniMapChunkVisual = this.Rent(value.worldPosition, new Vector2((float)vector2Int.x, (float)vector2Int.y) * num, value.rotation.eulerAngles.y, (chunkTemplate != null) ? chunkTemplate.shape : MapsLiveCitySnapshot.ResolveShape(text), keyValuePair.Key.GetHashCode(), false);
					if (value.spawnedZones != null)
					{
						for (int m = 0; m < value.spawnedZones.Count; m++)
						{
							GameObject gameObject = value.spawnedZones[m];
							if (!(gameObject == null) && (!loadedOnly || MapsLiveCitySnapshot.IsZonePresented(gameObject)))
							{
								ZoneSlot zoneSlot = ((chunkTemplate != null && chunkTemplate.zoneSlots != null && m < chunkTemplate.zoneSlots.Count) ? chunkTemplate.zoneSlots[m] : null);
								this.AddZone(miniMapChunkVisual, gameObject, MapsLiveCitySnapshot.ZoneSize(zoneSlot, num, miniMapChunkVisual.size), atlas, m);
							}
						}
					}
					if (chunkData == null && miniMapChunkVisual.zones.Count == 0)
					{
						foreach (object obj in value.chunkRoot.transform)
						{
							Transform transform = (Transform)obj;
							ProceduralMiniMapStyleAtlas.Entry entry;
							if ((!loadedOnly || MapsLiveCitySnapshot.IsZonePresented(transform.gameObject)) && atlas != null && atlas.TryGet(null, transform.name, out entry))
							{
								this.AddZone(miniMapChunkVisual, transform.gameObject, entry.worldSize, atlas, miniMapChunkVisual.zones.Count);
							}
						}
					}
				}
			}
			foreach (MiniMapPoiMarker miniMapPoiMarker in MiniMapPoiMarker.Active)
			{
				if (!(miniMapPoiMarker == null) && miniMapPoiMarker.isActiveAndEnabled && miniMapPoiMarker.IsOperationallyReady && miniMapPoiMarker.Kind != MiniMapPoiKind.Home && miniMapPoiMarker.Kind != MiniMapPoiKind.None && (!loadedOnly || (this._occupied.Contains(manager.WorldToCell(miniMapPoiMarker.transform.position)) && MapsLiveCitySnapshot.IsZonePresented(miniMapPoiMarker.gameObject))))
				{
					this.AddPoi(miniMapPoiMarker.transform.position, miniMapPoiMarker.Kind);
				}
			}
			if (loadedOnly)
			{
				return;
			}
			Vector2Int vector2Int2 = manager.WorldToCell(center - new Vector3(worldSize.x, 0f, worldSize.y) * 0.5f);
			Vector2Int vector2Int3 = manager.WorldToCell(center + new Vector3(worldSize.x, 0f, worldSize.y) * 0.5f);
			int num2 = 0;
			int num3 = vector2Int2.y;
			while (num3 <= vector2Int3.y && num2 < 1024)
			{
				int num4 = vector2Int2.x;
				while (num4 <= vector2Int3.x && num2 < 1024)
				{
					Vector2Int vector2Int4 = new Vector2Int(num4, num3);
					ChunkSpawnResolver.SpawnEntry spawnEntry;
					if (!this._occupied.Contains(vector2Int4) && manager.TryResolveMiniMapMetadataPreview(vector2Int4, out spawnEntry) && spawnEntry.IsValid)
					{
						ChunkData chunkData2 = spawnEntry.chunkData;
						if (!(chunkData2 == null))
						{
							this.Rent(manager.CellToWorld(vector2Int4, Vector2Int.one), Vector2.one * num, (float)spawnEntry.rotation * 90f, (chunkData2.template != null) ? chunkData2.template.shape : MapsLiveCitySnapshot.ResolveShape(chunkData2.name), vector2Int4.GetHashCode(), true);
						}
					}
					num4++;
					num2++;
				}
				num3++;
			}
		}

		// Token: 0x0600A1A2 RID: 41378 RVA: 0x004312D4 File Offset: 0x0042F4D4
		private static bool IsZonePresented(GameObject zone)
		{
			if (zone == null || !zone.activeInHierarchy)
			{
				return false;
			}
			HeavyZoneStreamingProxy componentInParent = zone.GetComponentInParent<HeavyZoneStreamingProxy>(true);
			if (componentInParent != null && !componentInParent.IsOperationalReady)
			{
				return false;
			}
			IncrementalPooledZoneActivation componentInParent2 = zone.GetComponentInParent<IncrementalPooledZoneActivation>(true);
			return componentInParent2 == null || componentInParent2.IsOperationalReady;
		}

		// Token: 0x0600A1A3 RID: 41379 RVA: 0x00431328 File Offset: 0x0042F528
		private MiniMapChunkVisual Rent(Vector3 center, Vector2 size, float yaw, ChunkShape shape, int hash, bool preview)
		{
			MiniMapChunkVisual miniMapChunkVisual = ((this._pool.Count > 0) ? this._pool.Pop() : new MiniMapChunkVisual());
			miniMapChunkVisual.center = center;
			miniMapChunkVisual.size = size;
			miniMapChunkVisual.yaw = yaw;
			miniMapChunkVisual.shape = shape;
			miniMapChunkVisual.stableHash = hash;
			miniMapChunkVisual.previewOnly = preview;
			miniMapChunkVisual.tile = default(MiniMapTileReference);
			if (preview)
			{
				this.PreviewChunks.Add(miniMapChunkVisual);
			}
			else
			{
				this.Chunks.Add(miniMapChunkVisual);
			}
			return miniMapChunkVisual;
		}

		// Token: 0x0600A1A4 RID: 41380 RVA: 0x004313B0 File Offset: 0x0042F5B0
		private Vector2Int ResolveAuthoredSize(Transform root, float cellSize)
		{
			Vector2Int one;
			if (this._authoredSizes.TryGetValue(root, out one))
			{
				return one;
			}
			PreSpawnedChunk component = root.GetComponent<PreSpawnedChunk>();
			if (component != null)
			{
				one = new Vector2Int(Mathf.Max(1, component.gridSize.x), Mathf.Max(1, component.gridSize.y));
			}
			else
			{
				SnapPoint[] componentsInChildren = root.GetComponentsInChildren<SnapPoint>(true);
				one = Vector2Int.one;
				if (componentsInChildren.Length >= 2)
				{
					Vector3 vector = Vector3.positiveInfinity;
					Vector3 vector2 = Vector3.negativeInfinity;
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						Vector3 vector3 = root.InverseTransformPoint(componentsInChildren[i].transform.position);
						vector = Vector3.Min(vector, vector3);
						vector2 = Vector3.Max(vector2, vector3);
					}
					one = new Vector2Int(Mathf.Max(1, Mathf.RoundToInt((vector2.x - vector.x) / cellSize)), Mathf.Max(1, Mathf.RoundToInt((vector2.z - vector.z) / cellSize)));
				}
			}
			this._authoredSizes[root] = one;
			return one;
		}

		// Token: 0x0600A1A5 RID: 41381 RVA: 0x004314B8 File Offset: 0x0042F6B8
		private void AddZone(MiniMapChunkVisual chunk, GameObject zone, Vector2 size, ProceduralMiniMapStyleAtlas atlas, int index)
		{
			MiniMapTileReference miniMapTileReference = default(MiniMapTileReference);
			ProceduralMiniMapStyleAtlas.Entry entry;
			if (atlas != null && atlas.TryGet(null, zone.name, out entry))
			{
				miniMapTileReference = MiniMapTileReference.From(entry);
				size = entry.worldSize;
			}
			MiniMapPoiKind miniMapPoiKind = MapsLiveCitySnapshot.ResolvePoi(zone.name);
			chunk.zones.Add(new MiniMapZoneVisual
			{
				center = zone.transform.position,
				yaw = zone.transform.eulerAngles.y,
				size = size,
				tile = miniMapTileReference,
				kind = MapsLiveCitySnapshot.ZoneKind(zone.name, miniMapPoiKind),
				stableHash = (chunk.stableHash ^ index)
			});
			IncrementalPooledZoneActivation incrementalPooledZoneActivation;
			if (miniMapPoiKind != MiniMapPoiKind.None && MiniMapPoiMarker.IsSourceOperational(zone.transform, miniMapPoiKind, out incrementalPooledZoneActivation))
			{
				this.AddPoi(zone.transform.position, miniMapPoiKind);
			}
		}

		// Token: 0x0600A1A6 RID: 41382 RVA: 0x0043159C File Offset: 0x0042F79C
		private void AddPoi(Vector3 position, MiniMapPoiKind kind)
		{
			for (int i = 0; i < this.Pois.Count; i++)
			{
				if (this.Pois[i].kind == kind && (this.Pois[i].center - position).sqrMagnitude < 900f)
				{
					return;
				}
			}
			this.Pois.Add(new MiniMapPoiVisual
			{
				center = position,
				kind = kind,
				discovered = true
			});
		}

		// Token: 0x0600A1A7 RID: 41383 RVA: 0x00431628 File Offset: 0x0042F828
		private static Vector2 ZoneSize(ZoneSlot slot, float cellSize, Vector2 fallback)
		{
			if (slot == null)
			{
				return fallback * 0.78f;
			}
			Vector2 vector = slot.parentChunkSize.GetPhysicalSize(cellSize) - Vector2.one * 9f;
			if (slot.occupancy == ZoneOccupancy.Quarter)
			{
				return vector * 0.5f;
			}
			if (slot.occupancy == ZoneOccupancy.Half)
			{
				return new Vector2(vector.x * 0.5f, vector.y);
			}
			return vector;
		}

		// Token: 0x0600A1A8 RID: 41384 RVA: 0x0043169C File Offset: 0x0042F89C
		private static ChunkShape ResolveShape(string name)
		{
			if (MapsLiveCitySnapshot.Contains(name, "startchunk") || MapsLiveCitySnapshot.Contains(name, "start chunk") || MapsLiveCitySnapshot.Contains(name, "xchunk") || MapsLiveCitySnapshot.Contains(name, "x chunk"))
			{
				return ChunkShape.XShape;
			}
			if (MapsLiveCitySnapshot.Contains(name, "tchunk") || MapsLiveCitySnapshot.Contains(name, "t chunk"))
			{
				return ChunkShape.TShape;
			}
			return ChunkShape.MonoShape;
		}

		// Token: 0x0600A1A9 RID: 41385 RVA: 0x004316FC File Offset: 0x0042F8FC
		private static MiniMapPoiKind ResolvePoi(string name)
		{
			if (MapsLiveCitySnapshot.Contains(name, "university") || MapsLiveCitySnapshot.Contains(name, "campus"))
			{
				return MiniMapPoiKind.University;
			}
			if (MapsLiveCitySnapshot.Contains(name, "subway") || MapsLiveCitySnapshot.Contains(name, "metro"))
			{
				return MiniMapPoiKind.Metro;
			}
			if (MapsLiveCitySnapshot.Contains(name, "kombini"))
			{
				return MiniMapPoiKind.Kombini;
			}
			if (MapsLiveCitySnapshot.Contains(name, "stadium") || MapsLiveCitySnapshot.Contains(name, "football"))
			{
				return MiniMapPoiKind.Stadium;
			}
			if (MapsLiveCitySnapshot.Contains(name, "park") || MapsLiveCitySnapshot.Contains(name, "garden"))
			{
				return MiniMapPoiKind.Park;
			}
			if (MapsLiveCitySnapshot.Contains(name, "mall"))
			{
				return MiniMapPoiKind.Mall;
			}
			if (MapsLiveCitySnapshot.Contains(name, "gallery") || MapsLiveCitySnapshot.Contains(name, "museum"))
			{
				return MiniMapPoiKind.Gallery;
			}
			if (MapsLiveCitySnapshot.Contains(name, "pool") || MapsLiveCitySnapshot.Contains(name, "swimming"))
			{
				return MiniMapPoiKind.Pool;
			}
			return MiniMapPoiKind.None;
		}

		// Token: 0x0600A1AA RID: 41386 RVA: 0x004317D4 File Offset: 0x0042F9D4
		private static MiniMapZoneKind ZoneKind(string name, MiniMapPoiKind poi)
		{
			switch (poi)
			{
			case MiniMapPoiKind.Park:
				return MiniMapZoneKind.Park;
			case MiniMapPoiKind.Metro:
				return MiniMapZoneKind.Subway;
			case MiniMapPoiKind.University:
				return MiniMapZoneKind.University;
			case MiniMapPoiKind.Stadium:
				return MiniMapZoneKind.Stadium;
			case MiniMapPoiKind.Mall:
				return MiniMapZoneKind.Mall;
			case MiniMapPoiKind.Gallery:
				return MiniMapZoneKind.Gallery;
			case MiniMapPoiKind.Pool:
				return MiniMapZoneKind.Pool;
			case MiniMapPoiKind.Kombini:
				return MiniMapZoneKind.Commercial;
			default:
				if (MapsLiveCitySnapshot.Contains(name, "suburban"))
				{
					return MiniMapZoneKind.Suburban;
				}
				if (MapsLiveCitySnapshot.Contains(name, "residential"))
				{
					return MiniMapZoneKind.Residential;
				}
				if (MapsLiveCitySnapshot.Contains(name, "city"))
				{
					return MiniMapZoneKind.City;
				}
				return MiniMapZoneKind.Generic;
			}
		}

		// Token: 0x0600A1AB RID: 41387 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool Contains(string value, string token)
		{
			return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x0400A549 RID: 42313
		internal readonly List<MiniMapChunkVisual> Chunks = new List<MiniMapChunkVisual>(128);

		// Token: 0x0400A54A RID: 42314
		internal readonly List<MiniMapChunkVisual> PreviewChunks = new List<MiniMapChunkVisual>(128);

		// Token: 0x0400A54B RID: 42315
		internal readonly List<MiniMapPoiVisual> Pois = new List<MiniMapPoiVisual>(24);

		// Token: 0x0400A54C RID: 42316
		private readonly HashSet<Vector2Int> _occupied = new HashSet<Vector2Int>();

		// Token: 0x0400A54D RID: 42317
		private readonly Stack<MiniMapChunkVisual> _pool = new Stack<MiniMapChunkVisual>();

		// Token: 0x0400A54E RID: 42318
		private readonly Dictionary<Transform, Vector2Int> _authoredSizes = new Dictionary<Transform, Vector2Int>();
	}
}
