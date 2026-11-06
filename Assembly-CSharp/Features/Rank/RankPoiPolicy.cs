using System;
using Features.RunModes;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DAC RID: 3500
	public static class RankPoiPolicy
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06006709 RID: 26377 RVA: 0x00293ACB File Offset: 0x00291CCB
		public static bool RestrictionsActive
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return RankPoiPolicy.ShouldRestrict(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x00293AF6 File Offset: 0x00291CF6
		public static bool ShouldRestrict(RunModeKind mode, string rankId, bool admin)
		{
			return !admin && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && !string.Equals(rankId, "hornyslut_declared", StringComparison.Ordinal);
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x00293B13 File Offset: 0x00291D13
		public static bool IsPoiAllowed(ProceduralPoiKind kind)
		{
			return RankPoiPolicy.IsPoiAllowed(kind, RankPoiPolicy.RestrictionsActive);
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x00293B20 File Offset: 0x00291D20
		public static bool IsPoiAllowed(ProceduralPoiKind kind, bool restricted)
		{
			return !restricted || kind == ProceduralPoiKind.None || kind == ProceduralPoiKind.Kombini || kind == ProceduralPoiKind.ArtGallery || kind == ProceduralPoiKind.University || kind == ProceduralPoiKind.Stadium;
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x00293B3C File Offset: 0x00291D3C
		public static bool IsZoneAllowed(ZoneData zone)
		{
			ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(zone);
			if (poiKind == ProceduralPoiKind.None && ProceduralZonePolicy.IsSpecialZone(zone))
			{
				return !RankPoiPolicy.RestrictionsActive;
			}
			return RankPoiPolicy.IsPoiAllowed(poiKind);
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x00293B6C File Offset: 0x00291D6C
		public static bool IsChunkAllowed(ChunkData chunk)
		{
			if (chunk == null || !RankPoiPolicy.RestrictionsActive)
			{
				return true;
			}
			if (chunk.containsSubway)
			{
				return false;
			}
			if (chunk.availableZones == null || chunk.availableZones.Count == 0)
			{
				return true;
			}
			bool flag = false;
			foreach (ZoneData zoneData in chunk.availableZones)
			{
				if (!(zoneData == null))
				{
					flag = true;
					if (RankPoiPolicy.IsZoneAllowed(zoneData))
					{
						return true;
					}
				}
			}
			return !flag;
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x00293C0C File Offset: 0x00291E0C
		public static bool IsSpawnAllowed(ChunkSpawnResolver.SpawnEntry entry)
		{
			if (!RankPoiPolicy.RestrictionsActive)
			{
				return true;
			}
			if (!RankPoiPolicy.IsChunkAllowed(entry.chunkData))
			{
				return false;
			}
			if (entry.zoneIndices != null)
			{
				ChunkData chunkData = entry.chunkData;
				if (((chunkData != null) ? chunkData.availableZones : null) != null)
				{
					foreach (int num in entry.zoneIndices)
					{
						if (num >= 0 && num < entry.chunkData.availableZones.Count && !RankPoiPolicy.IsZoneAllowed(entry.chunkData.availableZones[num]))
						{
							return false;
						}
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x00293C9A File Offset: 0x00291E9A
		public static bool IsDestinationAllowed(string sceneName)
		{
			return RankPoiPolicy.IsDestinationAllowed(sceneName, RankPoiPolicy.RestrictionsActive);
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x00293CA8 File Offset: 0x00291EA8
		public static bool IsDestinationAllowed(string sceneName, bool restricted)
		{
			return !restricted || string.IsNullOrWhiteSpace(sceneName) || !sceneName.StartsWith("POI - ", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "POI - Art Gallery", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "POI - University", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "POI - Kombini", StringComparison.OrdinalIgnoreCase) || string.Equals(sceneName, "POI - Stadium", StringComparison.OrdinalIgnoreCase);
		}
	}
}
