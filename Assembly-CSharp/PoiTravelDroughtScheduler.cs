using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public sealed class PoiTravelDroughtScheduler
{
	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002E12F File Offset: 0x0002C32F
	public float DistanceSinceAnyPoiMetres
	{
		get
		{
			return this._distanceSinceAnyPoiMetres;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000674 RID: 1652 RVA: 0x0002E137 File Offset: 0x0002C337
	public float DistanceSinceReliefMetres
	{
		get
		{
			return this.DistanceSinceAnyPoiMetres;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000675 RID: 1653 RVA: 0x0002E13F File Offset: 0x0002C33F
	public float DistanceSinceLargePoiMetres
	{
		get
		{
			return this._distanceSinceLargePoiMetres;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000676 RID: 1654 RVA: 0x0002E147 File Offset: 0x0002C347
	public bool PoiPending
	{
		get
		{
			return this._distanceSinceAnyPoiMetres >= 120f;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x0002E159 File Offset: 0x0002C359
	public bool ReliefPending
	{
		get
		{
			return this.PoiPending;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000678 RID: 1656 RVA: 0x0002E161 File Offset: 0x0002C361
	public bool HardLimitReached
	{
		get
		{
			return this._distanceSinceAnyPoiMetres >= 420f;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000679 RID: 1657 RVA: 0x0002E173 File Offset: 0x0002C373
	public bool ShouldBlockLargePoi
	{
		get
		{
			return this._distanceSinceAnyPoiMetres >= 180f || this._distanceSinceLargePoiMetres < 2000f;
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002E194 File Offset: 0x0002C394
	public bool IsPoiPending(float approachLeadMetres)
	{
		float num = Mathf.Max(0f, 300f - Mathf.Max(0f, approachLeadMetres));
		return this._distanceSinceAnyPoiMetres >= num;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002E1CC File Offset: 0x0002C3CC
	public static float CalculateStreamingApproachLeadMetres(int spawnRadius, int minVisibleRadius, float cellSizeMetres)
	{
		int num = Mathf.Max(0, spawnRadius);
		int num2 = Mathf.Min(Mathf.Max(0, minVisibleRadius), num);
		int num3 = ((num > num2) ? num : (num2 + 1));
		return (float)Mathf.Max(1, num3) * Mathf.Max(1f, cellSizeMetres);
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x0002E20E File Offset: 0x0002C40E
	public PoiTypeWeightMultipliers CurrentPoiTypeWeights
	{
		get
		{
			return this.GetPoiTypeWeightsAfterPending(ProceduralPoiKind.None);
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0002E218 File Offset: 0x0002C418
	public PoiTypeWeightMultipliers GetPoiTypeWeightsAfterPending(ProceduralPoiKind pendingKind)
	{
		return new PoiTypeWeightMultipliers(this.GetPoiRecencyMultiplier(ProceduralPoiKind.Kombini, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.Pool, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.ArtGallery, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.Mall, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.Subway, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.University, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.Park, pendingKind), this.GetPoiRecencyMultiplier(ProceduralPoiKind.Stadium, pendingKind));
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0002E26C File Offset: 0x0002C46C
	public bool ObservePosition(Vector2 position, float teleportThresholdMetres)
	{
		if (!PoiTravelDroughtScheduler.IsFinite(position))
		{
			this.Reset();
			return true;
		}
		if (!this._hasPosition)
		{
			this._hasPosition = true;
			this._lastPosition = position;
			return false;
		}
		float num = Vector2.Distance(this._lastPosition, position);
		this._lastPosition = position;
		float num2 = Mathf.Max(1f, teleportThresholdMetres);
		if (!float.IsFinite(num) || num > num2)
		{
			this.ResetAt(position);
			return true;
		}
		if (num <= 0f)
		{
			return false;
		}
		this._distanceSinceAnyPoiMetres = Mathf.Min(1000000f, this._distanceSinceAnyPoiMetres + num);
		this._distanceSinceLargePoiMetres = Mathf.Min(1000000f, this._distanceSinceLargePoiMetres + num);
		return false;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0002E311 File Offset: 0x0002C511
	public void ApplyWorldOriginShift(Vector2 delta)
	{
		if (this._hasPosition)
		{
			this._lastPosition += delta;
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0002E32D File Offset: 0x0002C52D
	public void MarkPoiEncountered()
	{
		this._distanceSinceAnyPoiMetres = 0f;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0002E33A File Offset: 0x0002C53A
	public void MarkReliefEncountered()
	{
		this.MarkPoiEncountered();
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002E342 File Offset: 0x0002C542
	public void MarkLargePoiAccepted()
	{
		this._distanceSinceLargePoiMetres = 0f;
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0002E350 File Offset: 0x0002C550
	public void MarkPoiTypeAccepted(ProceduralPoiKind kind)
	{
		if (kind == ProceduralPoiKind.None || kind <= ProceduralPoiKind.None || (int)kind >= this._lastAcceptedSequenceByKind.Length)
		{
			return;
		}
		if (this._acceptedPoiSequence == 2147483647)
		{
			this.ClearPoiRecency();
		}
		this._acceptedPoiSequence++;
		this._lastAcceptedSequenceByKind[(int)kind] = this._acceptedPoiSequence;
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0002E3A1 File Offset: 0x0002C5A1
	public float GetPoiRecencyMultiplier(ProceduralPoiKind kind)
	{
		return this.GetPoiRecencyMultiplier(kind, ProceduralPoiKind.None);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0002E3AC File Offset: 0x0002C5AC
	private float GetPoiRecencyMultiplier(ProceduralPoiKind kind, ProceduralPoiKind pendingKind)
	{
		if (kind == ProceduralPoiKind.None || kind <= ProceduralPoiKind.None || (int)kind >= this._lastAcceptedSequenceByKind.Length)
		{
			return 1f;
		}
		if (kind == pendingKind)
		{
			return 0.15f;
		}
		int num = this._lastAcceptedSequenceByKind[(int)kind];
		if (num <= 0)
		{
			return 1f;
		}
		int num2 = this._acceptedPoiSequence - num + ((pendingKind == ProceduralPoiKind.None) ? 0 : 1);
		if (num2 <= 0)
		{
			return 0.15f;
		}
		if (num2 == 1)
		{
			return 0.35f;
		}
		if (num2 == 2)
		{
			return 0.65f;
		}
		return 1f;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002E424 File Offset: 0x0002C624
	public void ClearPoiRecency()
	{
		this._acceptedPoiSequence = 0;
		for (int i = 0; i < this._lastAcceptedSequenceByKind.Length; i++)
		{
			this._lastAcceptedSequenceByKind[i] = 0;
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002E454 File Offset: 0x0002C654
	public void Reset()
	{
		this._hasPosition = false;
		this._lastPosition = Vector2.zero;
		this._distanceSinceAnyPoiMetres = 0f;
		this._distanceSinceLargePoiMetres = 2000f;
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0002E47E File Offset: 0x0002C67E
	private void ResetAt(Vector2 position)
	{
		this.Reset();
		this._hasPosition = true;
		this._lastPosition = position;
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x0002E494 File Offset: 0x0002C694
	private static bool IsFinite(Vector2 value)
	{
		return float.IsFinite(value.x) && float.IsFinite(value.y);
	}

	// Token: 0x040004FC RID: 1276
	public const float LargePoiGuardDistanceMetres = 180f;

	// Token: 0x040004FD RID: 1277
	public const float ReliefSoftDistanceMetres = 120f;

	// Token: 0x040004FE RID: 1278
	public const float TargetPoiFirstSightDistanceMetres = 300f;

	// Token: 0x040004FF RID: 1279
	public const float ReliefHardDistanceMetres = 420f;

	// Token: 0x04000500 RID: 1280
	public const float LargePoiCooldownMetres = 2000f;

	// Token: 0x04000501 RID: 1281
	public const float MostRecentPoiMultiplier = 0.15f;

	// Token: 0x04000502 RID: 1282
	public const float SecondRecentPoiMultiplier = 0.35f;

	// Token: 0x04000503 RID: 1283
	public const float ThirdRecentPoiMultiplier = 0.65f;

	// Token: 0x04000504 RID: 1284
	private bool _hasPosition;

	// Token: 0x04000505 RID: 1285
	private Vector2 _lastPosition;

	// Token: 0x04000506 RID: 1286
	private float _distanceSinceAnyPoiMetres;

	// Token: 0x04000507 RID: 1287
	private float _distanceSinceLargePoiMetres = 2000f;

	// Token: 0x04000508 RID: 1288
	private readonly int[] _lastAcceptedSequenceByKind = new int[9];

	// Token: 0x04000509 RID: 1289
	private int _acceptedPoiSequence;
}
