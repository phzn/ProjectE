using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000098 RID: 152
public sealed class PoiResidencyPrefabResolver
{
	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x0002D9FA File Offset: 0x0002BBFA
	public string Mode { get; }

	// Token: 0x06000662 RID: 1634 RVA: 0x0002DA04 File Offset: 0x0002BC04
	public PoiResidencyPrefabResolver(string mode, Func<string, GameObject> load)
	{
		if (mode != "off" && mode != "all" && !PoiResidencyCatalog.IsFamily(mode))
		{
			throw new ArgumentException("Unknown residency mode.", "mode");
		}
		this.Mode = mode;
		if (load == null)
		{
			throw new ArgumentNullException("load");
		}
		this.load = load;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0002DA76 File Offset: 0x0002BC76
	public static bool IsDirectBaselineFamily(string family)
	{
		return family == "kombini" || family == "subway";
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0002DA94 File Offset: 0x0002BC94
	public GameObject Resolve(GameObject authoredPrefab)
	{
		PoiResidencyCatalog.Entry entry;
		if (!this.TryGetBaselineRoute(authoredPrefab, out entry))
		{
			return authoredPrefab;
		}
		GameObject gameObject;
		if (this.baselines.TryGetValue(entry.resourceKey, out gameObject))
		{
			if (gameObject == null)
			{
				throw new InvalidOperationException("A live baseline prefab was destroyed: " + entry.resourceKey);
			}
			return gameObject;
		}
		else
		{
			gameObject = this.load(entry.resourceKey);
			PoiResidencyStateBindings poiResidencyStateBindings = ((gameObject != null) ? gameObject.GetComponent<PoiResidencyStateBindings>() : null);
			if (gameObject == null || gameObject.GetComponent<HeavyZoneStreamingProxy>() != null || poiResidencyStateBindings == null || poiResidencyStateBindings.family != entry.family)
			{
				throw new InvalidOperationException("Missing or invalid direct pooled baseline: " + entry.resourceKey);
			}
			this.baselines.Add(entry.resourceKey, gameObject);
			return gameObject;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0002DB68 File Offset: 0x0002BD68
	public bool TryGetResolved(GameObject authoredPrefab, out GameObject resolved)
	{
		resolved = authoredPrefab;
		PoiResidencyCatalog.Entry entry;
		if (!this.TryGetBaselineRoute(authoredPrefab, out entry))
		{
			return resolved != null;
		}
		return this.baselines.TryGetValue(entry.resourceKey, out resolved) && resolved != null;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0002DBAC File Offset: 0x0002BDAC
	private bool TryGetBaselineRoute(GameObject authoredPrefab, out PoiResidencyCatalog.Entry route)
	{
		route = null;
		HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((authoredPrefab != null) ? authoredPrefab.GetComponent<HeavyZoneStreamingProxy>() : null);
		return heavyZoneStreamingProxy != null && PoiResidencyCatalog.TryGetByResourcePath(heavyZoneStreamingProxy.ResourcesPrefabPath, out route) && PoiResidencyPrefabResolver.IsDirectBaselineFamily(route.family) && !MapResidencyPilotSelection.IsFamilyEnabled(route.family, this.Mode);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0002DC0B File Offset: 0x0002BE0B
	public void Clear()
	{
		this.baselines.Clear();
	}

	// Token: 0x040004F4 RID: 1268
	private readonly Func<string, GameObject> load;

	// Token: 0x040004F5 RID: 1269
	private readonly Dictionary<string, GameObject> baselines = new Dictionary<string, GameObject>(StringComparer.Ordinal);
}
