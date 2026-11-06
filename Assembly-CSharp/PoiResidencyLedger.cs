using System;
using System.Collections.Generic;
using Features.ClothingLoot;
using UnityEngine;

// Token: 0x02000095 RID: 149
public static class PoiResidencyLedger
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x0002D170 File Offset: 0x0002B370
	// (set) Token: 0x06000649 RID: 1609 RVA: 0x0002D177 File Offset: 0x0002B377
	public static int SessionGeneration { get; private set; }

	// Token: 0x0600064A RID: 1610 RVA: 0x0002D17F File Offset: 0x0002B37F
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Reset()
	{
		PoiResidencyLedger.SessionGeneration = 0;
		PoiResidencyLedger.Flags.Clear();
		PoiResidencyLedger.CanonicalTiers.Clear();
		PoiResidencyLedger.Clothing.Clear();
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0002D1A5 File Offset: 0x0002B3A5
	public static void BeginSession(int generation)
	{
		if (generation <= 0)
		{
			throw new ArgumentOutOfRangeException("generation");
		}
		if (generation == PoiResidencyLedger.SessionGeneration)
		{
			return;
		}
		PoiResidencyLedger.SessionGeneration = generation;
		PoiResidencyLedger.Flags.Clear();
		PoiResidencyLedger.CanonicalTiers.Clear();
		PoiResidencyLedger.Clothing.Clear();
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0002D1E3 File Offset: 0x0002B3E3
	public static bool IsCurrent(PoiResidencyContext context)
	{
		return context.IsValid && context.sessionGeneration == PoiResidencyLedger.SessionGeneration;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0002D200 File Offset: 0x0002B400
	public static bool ReadFlag(PoiResidencyStateKey key)
	{
		bool flag;
		return PoiResidencyLedger.IsCurrent(key.context) && PoiResidencyLedger.Flags.TryGetValue(key, out flag) && flag;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0002D22C File Offset: 0x0002B42C
	public static void WriteFlag(PoiResidencyStateKey key, bool value)
	{
		if (!PoiResidencyLedger.IsCurrent(key.context))
		{
			return;
		}
		if (value)
		{
			PoiResidencyLedger.Flags[key] = true;
			return;
		}
		PoiResidencyLedger.Flags.Remove(key);
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0002D258 File Offset: 0x0002B458
	public static bool TryGetTier(PoiResidencyStateKey key, out GameObject canonical)
	{
		canonical = null;
		return PoiResidencyLedger.IsCurrent(key.context) && PoiResidencyLedger.CanonicalTiers.TryGetValue(key, out canonical) && canonical != null;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002D284 File Offset: 0x0002B484
	public static void RememberCanonicalTier(PoiResidencyStateKey key, GameObject canonical)
	{
		if (!PoiResidencyLedger.IsCurrent(key.context) || canonical == null)
		{
			return;
		}
		if (canonical.scene.IsValid())
		{
			throw new InvalidOperationException("A POI tier must be a canonical prefab, never a scene instance.");
		}
		PoiResidencyLedger.CanonicalTiers[key] = canonical;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002D2D0 File Offset: 0x0002B4D0
	public static PoiResidencyLedger.ClothingState GetClothing(PoiResidencyStateKey key)
	{
		if (!PoiResidencyLedger.IsCurrent(key.context))
		{
			return null;
		}
		PoiResidencyLedger.ClothingState clothingState;
		if (!PoiResidencyLedger.Clothing.TryGetValue(key, out clothingState))
		{
			clothingState = new PoiResidencyLedger.ClothingState();
			PoiResidencyLedger.Clothing.Add(key, clothingState);
		}
		return clothingState;
	}

	// Token: 0x040004EA RID: 1258
	private static readonly Dictionary<PoiResidencyStateKey, bool> Flags = new Dictionary<PoiResidencyStateKey, bool>();

	// Token: 0x040004EB RID: 1259
	private static readonly Dictionary<PoiResidencyStateKey, GameObject> CanonicalTiers = new Dictionary<PoiResidencyStateKey, GameObject>();

	// Token: 0x040004EC RID: 1260
	private static readonly Dictionary<PoiResidencyStateKey, PoiResidencyLedger.ClothingState> Clothing = new Dictionary<PoiResidencyStateKey, PoiResidencyLedger.ClothingState>();

	// Token: 0x02000096 RID: 150
	public sealed class ClothingState
	{
		// Token: 0x040004EE RID: 1262
		public readonly List<ClothingLootOffer> offers = new List<ClothingLootOffer>(4);

		// Token: 0x040004EF RID: 1263
		public bool looted;
	}
}
