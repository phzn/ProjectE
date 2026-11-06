using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public readonly struct PoiTypeWeightMultipliers
{
	// Token: 0x0600068D RID: 1677 RVA: 0x0002E594 File Offset: 0x0002C794
	public PoiTypeWeightMultipliers(float kombini, float pool, float artGallery, float mall, float subway, float university, float park, float stadium)
	{
		this.kombini = PoiTypeWeightMultipliers.Positive(kombini);
		this.pool = PoiTypeWeightMultipliers.Positive(pool);
		this.artGallery = PoiTypeWeightMultipliers.Positive(artGallery);
		this.mall = PoiTypeWeightMultipliers.Positive(mall);
		this.subway = PoiTypeWeightMultipliers.Positive(subway);
		this.university = PoiTypeWeightMultipliers.Positive(university);
		this.park = PoiTypeWeightMultipliers.Positive(park);
		this.stadium = PoiTypeWeightMultipliers.Positive(stadium);
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0002E608 File Offset: 0x0002C808
	public float For(ProceduralPoiKind kind)
	{
		switch (kind)
		{
		case ProceduralPoiKind.Kombini:
			return this.kombini;
		case ProceduralPoiKind.Pool:
			return this.pool;
		case ProceduralPoiKind.ArtGallery:
			return this.artGallery;
		case ProceduralPoiKind.Mall:
			return this.mall;
		case ProceduralPoiKind.Subway:
			return this.subway;
		case ProceduralPoiKind.University:
			return this.university;
		case ProceduralPoiKind.Park:
			return this.park;
		case ProceduralPoiKind.Stadium:
			return this.stadium;
		default:
			return 1f;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x0002E67C File Offset: 0x0002C87C
	public static PoiTypeWeightMultipliers Uniform
	{
		get
		{
			return new PoiTypeWeightMultipliers(1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
		}
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002E6AB File Offset: 0x0002C8AB
	private static float Positive(float value)
	{
		return Mathf.Max(0.0001f, value);
	}

	// Token: 0x04000517 RID: 1303
	public readonly float kombini;

	// Token: 0x04000518 RID: 1304
	public readonly float pool;

	// Token: 0x04000519 RID: 1305
	public readonly float artGallery;

	// Token: 0x0400051A RID: 1306
	public readonly float mall;

	// Token: 0x0400051B RID: 1307
	public readonly float subway;

	// Token: 0x0400051C RID: 1308
	public readonly float university;

	// Token: 0x0400051D RID: 1309
	public readonly float park;

	// Token: 0x0400051E RID: 1310
	public readonly float stadium;
}
