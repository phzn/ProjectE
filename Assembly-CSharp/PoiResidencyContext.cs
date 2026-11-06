using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public readonly struct PoiResidencyContext : IEquatable<PoiResidencyContext>
{
	// Token: 0x0600063E RID: 1598 RVA: 0x0002CF13 File Offset: 0x0002B113
	public PoiResidencyContext(string runId, int sessionGeneration, int seed, Vector2Int originCell, Vector2Int capturedTravelOffset, int zoneSlot, string family)
	{
		this.runId = runId ?? string.Empty;
		this.sessionGeneration = sessionGeneration;
		this.seed = seed;
		this.originCell = originCell;
		this.capturedTravelOffset = capturedTravelOffset;
		this.zoneSlot = zoneSlot;
		this.family = family;
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0002CF53 File Offset: 0x0002B153
	public bool IsValid
	{
		get
		{
			return this.sessionGeneration > 0 && this.zoneSlot >= 0 && !string.IsNullOrEmpty(this.family);
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x0002CF77 File Offset: 0x0002B177
	public Vector2Int SampleOrigin
	{
		get
		{
			return this.originCell + this.capturedTravelOffset;
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0002CF8C File Offset: 0x0002B18C
	public bool Equals(PoiResidencyContext other)
	{
		return this.sessionGeneration == other.sessionGeneration && this.seed == other.seed && this.zoneSlot == other.zoneSlot && this.SampleOrigin == other.SampleOrigin && string.Equals(this.runId, other.runId, StringComparison.Ordinal) && string.Equals(this.family, other.family, StringComparison.Ordinal);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002D000 File Offset: 0x0002B200
	public override bool Equals(object obj)
	{
		if (obj is PoiResidencyContext)
		{
			PoiResidencyContext poiResidencyContext = (PoiResidencyContext)obj;
			return this.Equals(poiResidencyContext);
		}
		return false;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0002D028 File Offset: 0x0002B228
	public override int GetHashCode()
	{
		int num = ((((((this.sessionGeneration * 397) ^ this.seed) * 397) ^ this.zoneSlot) * 397) ^ this.SampleOrigin.GetHashCode()) * 397;
		string text = this.runId;
		int num2 = (num ^ ((text != null) ? text.GetHashCode() : 0)) * 397;
		string text2 = this.family;
		return num2 ^ ((text2 != null) ? text2.GetHashCode() : 0);
	}

	// Token: 0x040004E0 RID: 1248
	public readonly string runId;

	// Token: 0x040004E1 RID: 1249
	public readonly int sessionGeneration;

	// Token: 0x040004E2 RID: 1250
	public readonly int seed;

	// Token: 0x040004E3 RID: 1251
	public readonly int zoneSlot;

	// Token: 0x040004E4 RID: 1252
	public readonly Vector2Int originCell;

	// Token: 0x040004E5 RID: 1253
	public readonly Vector2Int capturedTravelOffset;

	// Token: 0x040004E6 RID: 1254
	public readonly string family;
}
