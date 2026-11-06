using System;
using System.Collections.Generic;

// Token: 0x02000090 RID: 144
public static class PoiResidencyCatalog
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600062B RID: 1579 RVA: 0x0002C6CC File Offset: 0x0002A8CC
	public static IReadOnlyList<string> Families
	{
		get
		{
			return Array.AsReadOnly<string>(PoiResidencyCatalog.FamilyNames);
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
	public static IReadOnlyList<string> VariantIds
	{
		get
		{
			return Array.AsReadOnly<string>(PoiResidencyCatalog.VariantNames);
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0002C6E4 File Offset: 0x0002A8E4
	private static Dictionary<string, PoiResidencyCatalog.Entry> BuildIndex(bool address)
	{
		Dictionary<string, PoiResidencyCatalog.Entry> dictionary = new Dictionary<string, PoiResidencyCatalog.Entry>(StringComparer.Ordinal);
		foreach (PoiResidencyCatalog.Entry entry in PoiResidencyCatalog.Entries)
		{
			dictionary.Add(address ? entry.address : entry.resourceKey, entry);
		}
		return dictionary;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002C72D File Offset: 0x0002A92D
	public static IEnumerable<PoiResidencyCatalog.Entry> GetEntries(string family)
	{
		if (!PoiResidencyCatalog.IsFamily(family))
		{
			throw new ArgumentException("Unknown POI family.", "family");
		}
		foreach (PoiResidencyCatalog.Entry entry in PoiResidencyCatalog.Entries)
		{
			if (entry.family == family)
			{
				yield return entry;
			}
		}
		PoiResidencyCatalog.Entry[] array = null;
		yield break;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0002C73D File Offset: 0x0002A93D
	public static bool IsFamily(string family)
	{
		return family != null && Array.IndexOf<string>(PoiResidencyCatalog.FamilyNames, family) >= 0;
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0002C755 File Offset: 0x0002A955
	public static bool TryGetByResourcePath(string path, out PoiResidencyCatalog.Entry entry)
	{
		entry = null;
		return path != null && PoiResidencyCatalog.ByResource.TryGetValue(path, out entry);
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002C76B File Offset: 0x0002A96B
	public static bool TryGetByAddress(string address, out PoiResidencyCatalog.Entry entry)
	{
		entry = null;
		return address != null && PoiResidencyCatalog.ByAddress.TryGetValue(address, out entry);
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002C784 File Offset: 0x0002A984
	public static string FamilyFor(ProceduralPoiKind kind)
	{
		switch (kind)
		{
		case ProceduralPoiKind.Kombini:
			return "kombini";
		case ProceduralPoiKind.Pool:
			return "pool";
		case ProceduralPoiKind.ArtGallery:
			return "art-gallery";
		case ProceduralPoiKind.Mall:
			return "mall";
		case ProceduralPoiKind.Subway:
			return "subway";
		case ProceduralPoiKind.University:
			return "university";
		case ProceduralPoiKind.Park:
			return "park";
		case ProceduralPoiKind.Stadium:
			return "stadium";
		default:
			return null;
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002C7EC File Offset: 0x0002A9EC
	public static ProceduralPoiKind KindFor(string family)
	{
		foreach (object obj in Enum.GetValues(typeof(ProceduralPoiKind)))
		{
			ProceduralPoiKind proceduralPoiKind = (ProceduralPoiKind)obj;
			if (PoiResidencyCatalog.FamilyFor(proceduralPoiKind) == family && family != null)
			{
				return proceduralPoiKind;
			}
		}
		throw new ArgumentException("Unknown POI family.", "family");
	}

	// Token: 0x040004CF RID: 1231
	private static readonly string[] FamilyNames = new string[] { "university", "art-gallery", "pool", "mall", "stadium", "park", "kombini", "subway" };

	// Token: 0x040004D0 RID: 1232
	private static readonly string[] VariantNames = new string[] { "standard-original", "performance", "standard-performance", "standard-balanced", "performance-balanced", "performance-original" };

	// Token: 0x040004D1 RID: 1233
	private static readonly PoiResidencyCatalog.Entry[] Entries = new PoiResidencyCatalog.Entry[]
	{
		new PoiResidencyCatalog.Entry("university", "standard-original", "ShellRuntime/3x3 - University Shell/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("university", "performance", "PerformanceMap/b56524a566a600245aa57170bee68dbb/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("university", "standard-performance", "MapLightsS0/b56524a566a600245aa57170bee68dbb/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("university", "standard-balanced", "MapLightsS1/b56524a566a600245aa57170bee68dbb/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("university", "performance-balanced", "MapLightsP1/b56524a566a600245aa57170bee68dbb/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("university", "performance-original", "MapLightsP2/b56524a566a600245aa57170bee68dbb/3x3 - University Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "standard-original", "ShellRuntime/1x1 - Art Gallery Shell/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "performance", "PerformanceMap/ab0dae2956a39bc49a12ff11d8771c68/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "standard-performance", "MapLightsS0/ab0dae2956a39bc49a12ff11d8771c68/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "standard-balanced", "MapLightsS1/ab0dae2956a39bc49a12ff11d8771c68/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "performance-balanced", "MapLightsP1/ab0dae2956a39bc49a12ff11d8771c68/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("art-gallery", "performance-original", "MapLightsP2/ab0dae2956a39bc49a12ff11d8771c68/1x1 - Art Gallery Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "standard-original", "ShellRuntime/1x1 - Pool Shell/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "performance", "PerformanceMap/4585eac2dd625b24eae0ae02e57c08ec/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "standard-performance", "MapLightsS0/4585eac2dd625b24eae0ae02e57c08ec/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "standard-balanced", "MapLightsS1/4585eac2dd625b24eae0ae02e57c08ec/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "performance-balanced", "MapLightsP1/4585eac2dd625b24eae0ae02e57c08ec/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("pool", "performance-original", "MapLightsP2/4585eac2dd625b24eae0ae02e57c08ec/1x1 - Pool Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "standard-original", "ShellRuntime/3x3 - Mall Shell/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "performance", "PerformanceMap/46834c5258eff8f4590262b924b14d79/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "standard-performance", "MapLightsS0/46834c5258eff8f4590262b924b14d79/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "standard-balanced", "MapLightsS1/46834c5258eff8f4590262b924b14d79/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "performance-balanced", "MapLightsP1/46834c5258eff8f4590262b924b14d79/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("mall", "performance-original", "MapLightsP2/46834c5258eff8f4590262b924b14d79/3x3 - Mall Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "standard-original", "ShellRuntime/4x4 - Stadium Shell/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "performance", "PerformanceMap/9a563c2ac2ce0494f8aeec6f138de019/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "standard-performance", "MapLightsS0/9a563c2ac2ce0494f8aeec6f138de019/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "standard-balanced", "MapLightsS1/9a563c2ac2ce0494f8aeec6f138de019/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "performance-balanced", "MapLightsP1/9a563c2ac2ce0494f8aeec6f138de019/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("stadium", "performance-original", "MapLightsP2/9a563c2ac2ce0494f8aeec6f138de019/4x4 - Stadium Shell [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "standard-original", "ParkRuntime/PARK - 3x3/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "performance", "PerformanceMap/cbb4e89cd61d3ef4a808807cda7e3453/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "standard-performance", "MapLightsS0/cbb4e89cd61d3ef4a808807cda7e3453/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "standard-balanced", "MapLightsS1/cbb4e89cd61d3ef4a808807cda7e3453/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "performance-balanced", "MapLightsP1/cbb4e89cd61d3ef4a808807cda7e3453/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("park", "performance-original", "MapLightsP2/cbb4e89cd61d3ef4a808807cda7e3453/PARK - 3x3 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "standard-original", "PoiResidencyBaseline/kombini/standard-original/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "performance", "PoiResidencyBaseline/kombini/performance/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "standard-performance", "PoiResidencyBaseline/kombini/standard-performance/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "standard-balanced", "PoiResidencyBaseline/kombini/standard-balanced/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "performance-balanced", "PoiResidencyBaseline/kombini/performance-balanced/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("kombini", "performance-original", "PoiResidencyBaseline/kombini/performance-original/Suburban_Kombini - Full - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "standard-original", "PoiResidencyBaseline/subway/standard-original/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "performance", "PoiResidencyBaseline/subway/performance/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "standard-performance", "PoiResidencyBaseline/subway/standard-performance/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "standard-balanced", "PoiResidencyBaseline/subway/standard-balanced/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "performance-balanced", "PoiResidencyBaseline/subway/performance-balanced/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]"),
		new PoiResidencyCatalog.Entry("subway", "performance-original", "PoiResidencyBaseline/subway/performance-original/Suburban_Subway_1 - Half - Chunk1x1 [Optimized]")
	};

	// Token: 0x040004D2 RID: 1234
	private static readonly Dictionary<string, PoiResidencyCatalog.Entry> ByResource = PoiResidencyCatalog.BuildIndex(false);

	// Token: 0x040004D3 RID: 1235
	private static readonly Dictionary<string, PoiResidencyCatalog.Entry> ByAddress = PoiResidencyCatalog.BuildIndex(true);

	// Token: 0x02000091 RID: 145
	public sealed class Entry
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x0002CD9C File Offset: 0x0002AF9C
		internal Entry(string family, string variant, string resourceKey)
		{
			this.family = family;
			this.variant = variant;
			this.resourceKey = resourceKey;
			this.address = "map-residency/" + family + "/" + variant;
			this.manifestResourceKey = "MapResidencyGameplay/" + family + "/" + variant;
		}

		// Token: 0x040004D4 RID: 1236
		public readonly string family;

		// Token: 0x040004D5 RID: 1237
		public readonly string variant;

		// Token: 0x040004D6 RID: 1238
		public readonly string resourceKey;

		// Token: 0x040004D7 RID: 1239
		public readonly string address;

		// Token: 0x040004D8 RID: 1240
		public readonly string manifestResourceKey;
	}
}
