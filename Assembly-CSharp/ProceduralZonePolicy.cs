using System;
using System.Collections.Generic;
using Features.Rank;
using UnityEngine;

// Token: 0x0200009F RID: 159
public static class ProceduralZonePolicy
{
	// Token: 0x06000691 RID: 1681 RVA: 0x0002E6B8 File Offset: 0x0002C8B8
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetRuntimeCache()
	{
		ProceduralZonePolicy.FlagsByZone.Clear();
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0002E6C4 File Offset: 0x0002C8C4
	public static bool IsExcludedZone(ZoneData zone)
	{
		return (ProceduralZonePolicy.GetFlags(zone) & 1) != 0 || !RankPoiPolicy.IsZoneAllowed(zone);
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0002E6DB File Offset: 0x0002C8DB
	public static bool IsSpecialZone(ZoneData zone)
	{
		return (ProceduralZonePolicy.GetFlags(zone) & 2) > 0;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002E6E8 File Offset: 0x0002C8E8
	public static bool IsKombiniZone(ZoneData zone)
	{
		return ProceduralZonePolicy.MatchesIdentity(zone, "Kombini");
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002E6F8 File Offset: 0x0002C8F8
	public static bool IsDroughtReliefZone(ZoneData zone)
	{
		ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(zone);
		return poiKind == ProceduralPoiKind.Kombini || poiKind == ProceduralPoiKind.Pool || poiKind == ProceduralPoiKind.ArtGallery;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002E71C File Offset: 0x0002C91C
	public static bool IsLargePoiZone(ZoneData zone)
	{
		ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(zone);
		return poiKind == ProceduralPoiKind.Mall || poiKind == ProceduralPoiKind.Subway || poiKind == ProceduralPoiKind.University || poiKind == ProceduralPoiKind.Park || poiKind == ProceduralPoiKind.Stadium;
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002E748 File Offset: 0x0002C948
	public static ProceduralPoiKind GetPoiKind(ZoneData zone)
	{
		if (!ProceduralZonePolicy.IsSpecialZone(zone))
		{
			return ProceduralPoiKind.None;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Kombini"))
		{
			return ProceduralPoiKind.Kombini;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Pool"))
		{
			return ProceduralPoiKind.Pool;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Art Gallery"))
		{
			return ProceduralPoiKind.ArtGallery;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Mall"))
		{
			return ProceduralPoiKind.Mall;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Subway"))
		{
			return ProceduralPoiKind.Subway;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "University"))
		{
			return ProceduralPoiKind.University;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "PARK - 3x3") || ProceduralZonePolicy.MatchesIdentity(zone, "Park"))
		{
			return ProceduralPoiKind.Park;
		}
		if (ProceduralZonePolicy.MatchesIdentity(zone, "Stadium"))
		{
			return ProceduralPoiKind.Stadium;
		}
		return ProceduralPoiKind.None;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002E7E8 File Offset: 0x0002C9E8
	public static bool IsLargePoiChunk(ChunkData chunk)
	{
		if (chunk == null)
		{
			return false;
		}
		if (chunk.containsSubway)
		{
			return true;
		}
		if (chunk.availableZones == null)
		{
			return false;
		}
		for (int i = 0; i < chunk.availableZones.Count; i++)
		{
			if (ProceduralZonePolicy.IsLargePoiZone(chunk.availableZones[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002E840 File Offset: 0x0002CA40
	public static bool RequiresExactRuntimeIdentity(ZoneData zone)
	{
		return ProceduralZonePolicy.IsSpecialZone(zone);
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002E848 File Offset: 0x0002CA48
	public static bool IsAlwaysSpecialChunk(ChunkData chunk)
	{
		if (chunk == null)
		{
			return false;
		}
		if (chunk.containsSubway)
		{
			return true;
		}
		if (chunk.availableZones == null || chunk.availableZones.Count == 0)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < chunk.availableZones.Count; i++)
		{
			ZoneData zoneData = chunk.availableZones[i];
			if (!(zoneData == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData))
			{
				if (!ProceduralZonePolicy.IsSpecialZone(zoneData))
				{
					return false;
				}
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
	private static bool MatchesIdentity(ZoneData zone, string token)
	{
		return !(zone == null) && !string.IsNullOrEmpty(token) && (ProceduralZonePolicy.Contains(zone.name, token) || ProceduralZonePolicy.Contains(zone.zoneName, token) || (zone.zonePrefab != null && ProceduralZonePolicy.Contains(zone.zonePrefab.name, token)));
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002E924 File Offset: 0x0002CB24
	private static byte GetFlags(ZoneData zone)
	{
		if (zone == null)
		{
			return 0;
		}
		int instanceID = zone.GetInstanceID();
		byte b;
		if (ProceduralZonePolicy.FlagsByZone.TryGetValue(instanceID, out b))
		{
			return b;
		}
		bool flag = ProceduralZonePolicy.MatchesIdentity(zone, "CityCross") || ProceduralZonePolicy.MatchesIdentity(zone, "SuburbanMisc");
		bool flag2 = !flag && (ProceduralZonePolicy.MatchesIdentity(zone, "PARK - 3x3") || ProceduralZonePolicy.MatchesIdentity(zone, "Kombini") || ProceduralZonePolicy.MatchesIdentity(zone, "Subway") || ProceduralZonePolicy.MatchesIdentity(zone, " Shell"));
		b = (byte)((flag ? 1 : 0) | (flag2 ? 2 : 0));
		ProceduralZonePolicy.FlagsByZone[instanceID] = b;
		return b;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
	private static bool Contains(string value, string token)
	{
		return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	// Token: 0x0400051F RID: 1311
	private const byte ExcludedFlag = 1;

	// Token: 0x04000520 RID: 1312
	private const byte SpecialFlag = 2;

	// Token: 0x04000521 RID: 1313
	public const int KombiniSpawnGateDenominator = 3;

	// Token: 0x04000522 RID: 1314
	public const float KombiniAdmissionMultiplier = 0.8f;

	// Token: 0x04000523 RID: 1315
	private static readonly Dictionary<int, byte> FlagsByZone = new Dictionary<int, byte>(64);
}
