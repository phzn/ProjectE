using System;
using Features.TrashcanLoot;
using UnityEngine;

// Token: 0x020000A4 RID: 164
[DisallowMultipleComponent]
public sealed class StreamedWorldPropSlot : MonoBehaviour
{
	// Token: 0x060006A1 RID: 1697 RVA: 0x0002EA13 File Offset: 0x0002CC13
	internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
	{
		if (field == UniversityGameplayField.StreamedWorldPropSlot_viewPrefab)
		{
			return this.viewPrefab;
		}
		throw new InvalidOperationException("Unsupported StreamedWorldPropSlot gameplay binding: " + field.ToString());
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0002EA3C File Offset: 0x0002CC3C
	internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
	{
		if (field == UniversityGameplayField.StreamedWorldPropSlot_viewPrefab)
		{
			this.viewPrefab = (GameObject)asset;
			this.UsesCanonicalGameplayView = false;
			return;
		}
		throw new InvalidOperationException("Unsupported StreamedWorldPropSlot gameplay binding: " + field.ToString());
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002EA72 File Offset: 0x0002CC72
	internal void MarkCanonicalGameplayView(GameObject canonicalPrefab)
	{
		if (canonicalPrefab == null || this.viewPrefab != canonicalPrefab)
		{
			throw new InvalidOperationException("Canonical view proof must match the assigned original prefab.");
		}
		this.UsesCanonicalGameplayView = true;
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0002EA9D File Offset: 0x0002CC9D
	public GameObject ViewPrefab
	{
		get
		{
			if (!(this.selectedPrefab != null))
			{
				return this.viewPrefab;
			}
			return this.selectedPrefab;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0002EABA File Offset: 0x0002CCBA
	// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0002EAC2 File Offset: 0x0002CCC2
	public bool UsesCanonicalGameplayView { get; private set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0002EACB File Offset: 0x0002CCCB
	public Bounds WorldBounds
	{
		get
		{
			return PerformanceMapDetailController.TransformBounds(this.localBounds, base.transform.localToWorldMatrix);
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0002EAE3 File Offset: 0x0002CCE3
	// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0002EAEB File Offset: 0x0002CCEB
	public StreamedWorldPropView View { get; internal set; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060006AA RID: 1706 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
	// (set) Token: 0x060006AB RID: 1707 RVA: 0x0002EAFC File Offset: 0x0002CCFC
	public bool Consumed
	{
		get
		{
			return this.consumed;
		}
		internal set
		{
			this.consumed = value;
			PoiResidencyPlacement.WriteFlag(this, "slot-consumed", value);
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060006AC RID: 1708 RVA: 0x0002EB11 File Offset: 0x0002CD11
	// (set) Token: 0x060006AD RID: 1709 RVA: 0x0002EB19 File Offset: 0x0002CD19
	public int Generation { get; private set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x0002EB22 File Offset: 0x0002CD22
	public bool HasFoliageBox
	{
		get
		{
			return this.foliageBox;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002EB2A File Offset: 0x0002CD2A
	public Vector3 FoliageCenter
	{
		get
		{
			return this.foliageCenter;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0002EB32 File Offset: 0x0002CD32
	public Vector3 FoliageSize
	{
		get
		{
			return this.foliageSize;
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002EB3C File Offset: 0x0002CD3C
	private void OnEnable()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		int generation = this.Generation;
		this.Generation = generation + 1;
		this.consumed = PoiResidencyPlacement.ReadFlag(this, "slot-consumed");
		PoiResidencyStateKey poiResidencyStateKey;
		if (this.UsesCanonicalGameplayView && PoiResidencyPlacement.TryGetKey(this, "slot-tier", out poiResidencyStateKey))
		{
			if (!PoiResidencyLedger.TryGetTier(poiResidencyStateKey, out this.selectedPrefab))
			{
				this.selectedPrefab = (this.randomizeTrashcanTier ? TrashcanTierRandomizer.SelectStreamedPrefab(base.transform, this.viewPrefab) : this.viewPrefab);
				PoiResidencyLedger.RememberCanonicalTier(poiResidencyStateKey, this.randomizeTrashcanTier ? TrashcanTierRandomizer.CanonicalTierForStreamedChoice(this.selectedPrefab, this.viewPrefab) : this.selectedPrefab);
			}
			else if (this.randomizeTrashcanTier)
			{
				this.selectedPrefab = TrashcanTierRandomizer.StreamedViewForCanonicalTier(this.selectedPrefab, this.viewPrefab);
			}
			this.prepared = true;
		}
		else if (!this.prepared)
		{
			this.selectedPrefab = (this.randomizeTrashcanTier ? TrashcanTierRandomizer.SelectStreamedPrefab(base.transform, this.viewPrefab) : this.viewPrefab);
			this.prepared = true;
		}
		WorldPropStreamingService.Register(this);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002EC51 File Offset: 0x0002CE51
	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			WorldPropStreamingService.Unregister(this);
		}
	}

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private GameObject viewPrefab;

	// Token: 0x04000531 RID: 1329
	[SerializeField]
	private Bounds localBounds = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private bool randomizeTrashcanTier;

	// Token: 0x04000533 RID: 1331
	[SerializeField]
	private bool foliageBox;

	// Token: 0x04000534 RID: 1332
	[SerializeField]
	private Vector3 foliageCenter;

	// Token: 0x04000535 RID: 1333
	[SerializeField]
	private Vector3 foliageSize;

	// Token: 0x04000536 RID: 1334
	private GameObject selectedPrefab;

	// Token: 0x04000537 RID: 1335
	private bool prepared;

	// Token: 0x0400053A RID: 1338
	private bool consumed;

	// Token: 0x0400053C RID: 1340
	internal int RegistryIndex = -1;

	// Token: 0x0400053D RID: 1341
	internal bool Queued;

	// Token: 0x0400053E RID: 1342
	internal float LastRelevantTime;
}
