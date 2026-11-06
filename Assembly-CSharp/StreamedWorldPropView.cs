using System;
using System.Collections.Generic;
using Features.DumpsterHide;
using Features.HideFlash;
using Features.LockerHide;
using Features.TrashcanLoot;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[DisallowMultipleComponent]
public sealed class StreamedWorldPropView : MonoBehaviour
{
	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0002EC84 File Offset: 0x0002CE84
	// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0002EC8C File Offset: 0x0002CE8C
	public StreamedWorldPropSlot Owner { get; private set; }

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002EC95 File Offset: 0x0002CE95
	// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0002EC9D File Offset: 0x0002CE9D
	public GameObject Template { get; internal set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0002ECA6 File Offset: 0x0002CEA6
	public bool ResumingSamePlacement
	{
		get
		{
			return this.resuming && this.BelongsToCurrentPlacement;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0002ECB8 File Offset: 0x0002CEB8
	public bool IsConsumed
	{
		get
		{
			if (this.pickups == null || this.pickups.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.pickups.Length; i++)
			{
				if (this.pickups[i] != null)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x0002ECFE File Offset: 0x0002CEFE
	public bool HasPersistentState
	{
		get
		{
			return (this.can != null && this.can.IsLooted) || this.IsConsumed || this.HasMissingScopedPickups;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x0002ED2C File Offset: 0x0002CF2C
	public bool HasMissingScopedPickups
	{
		get
		{
			if (!this.scopedPlacement || this.pickups == null)
			{
				return false;
			}
			HideFlashPickup[] array = this.pickups;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x0002ED70 File Offset: 0x0002CF70
	public bool IsBusy
	{
		get
		{
			PoiResidencyStateKey poiResidencyStateKey;
			bool flag = this.Owner != null && PoiResidencyPlacement.TryGetKey(this.Owner, "slot-consumed", out poiResidencyStateKey);
			if (this.can != null && (flag ? this.can.HasActiveResidencyInteraction : this.can.IsRummaging))
			{
				return true;
			}
			if (this.hide != null && this.hide.IsBusy)
			{
				return true;
			}
			if (flag && this.pickups != null)
			{
				foreach (HideFlashPickup hideFlashPickup in this.pickups)
				{
					if (hideFlashPickup != null && hideFlashPickup.HasActiveResidencyInteraction)
					{
						return true;
					}
				}
			}
			if (flag && this.dumpsters != null)
			{
				foreach (DumpsterHideInteractable dumpsterHideInteractable in this.dumpsters)
				{
					if (dumpsterHideInteractable != null && dumpsterHideInteractable.HasActiveResidencyInteraction)
					{
						return true;
					}
				}
			}
			if (flag && this.lockers != null)
			{
				foreach (LockerHideInteractable lockerHideInteractable in this.lockers)
				{
					if (lockerHideInteractable != null && lockerHideInteractable.HasActiveResidencyInteraction)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x0002EEA0 File Offset: 0x0002D0A0
	internal bool HasAddressablePayloadLease
	{
		get
		{
			return this.addressablePayloadLease != null;
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0002EEAB File Offset: 0x0002D0AB
	internal void BindAddressablePayload(HeavyZoneAddressableResidency.Lease lease)
	{
		if (this.addressablePayloadLease != null)
		{
			throw new InvalidOperationException("A world prop already owns a payload lease.");
		}
		this.addressablePayloadLease = lease;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002EEC8 File Offset: 0x0002D0C8
	internal void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.can = base.GetComponent<TrashcanLootInteractable>();
		this.hide = base.GetComponent<WasteCanHideInteractable>();
		this.pickups = base.GetComponentsInChildren<HideFlashPickup>(true);
		this.dumpsters = base.GetComponentsInChildren<DumpsterHideInteractable>(true);
		this.lockers = base.GetComponentsInChildren<LockerHideInteractable>(true);
		foreach (Component component in base.GetComponentsInChildren<Component>(true))
		{
			if (PoiResidencyStateBindings.IsGameplayStateTarget(component))
			{
				this.CacheStateId(component);
			}
		}
		this.solidColliders = base.GetComponentsInChildren<Collider>(true);
		this.animators = base.GetComponentsInChildren<Animator>(true);
		this.foliageBox = base.GetComponent<BoxCollider>();
		foreach (Animator animator in this.animators)
		{
			if (animator != null)
			{
				animator.keepAnimatorStateOnDisable = true;
			}
		}
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002EF9F File Offset: 0x0002D19F
	private void CacheStateId(Component component)
	{
		if (component != null)
		{
			this.stateIds.Add(component, PoiResidencyStateBindings.SemanticStateId(base.transform, component));
		}
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002EFC2 File Offset: 0x0002D1C2
	internal bool TryGetStateId(Component component, out string id)
	{
		return this.stateIds.TryGetValue(component, out id);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002EFD4 File Offset: 0x0002D1D4
	internal void Wake(StreamedWorldPropSlot owner)
	{
		this.Initialize();
		this.resuming = this.Owner == owner && this.generation == owner.Generation;
		this.Owner = owner;
		PoiResidencyStateKey poiResidencyStateKey;
		this.scopedPlacement = PoiResidencyPlacement.TryGetKey(owner, "slot-consumed", out poiResidencyStateKey);
		this.generation = owner.Generation;
		owner.View = this;
		base.transform.SetParent(owner.transform, false);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		if (owner.HasFoliageBox && this.foliageBox != null)
		{
			this.foliageBox.center = owner.FoliageCenter;
			this.foliageBox.size = owner.FoliageSize;
		}
		this.SuppressDuplicatePhysics();
		base.gameObject.SetActive(true);
		this.SuppressDuplicatePhysics();
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002F0CC File Offset: 0x0002D2CC
	private void SuppressDuplicatePhysics()
	{
		foreach (Collider collider in this.solidColliders)
		{
			if (collider != null && !collider.isTrigger)
			{
				collider.enabled = false;
			}
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002F10A File Offset: 0x0002D30A
	internal bool BelongsToCurrentPlacement
	{
		get
		{
			return this.Owner != null && this.generation == this.Owner.Generation;
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002F12F File Offset: 0x0002D32F
	internal void ForgetOwner()
	{
		if (this.Owner != null && this.Owner.View == this)
		{
			this.Owner.View = null;
		}
		this.Owner = null;
		this.resuming = false;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002F16C File Offset: 0x0002D36C
	internal void ReleaseAddressablePayloadAfterDestruction()
	{
		HeavyZoneAddressableResidency.Lease lease = this.addressablePayloadLease;
		this.addressablePayloadLease = null;
		if (lease != null)
		{
			HeavyZoneAddressableResidency.ReleaseAfterDestroyed(lease, new Object[] { base.gameObject });
		}
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0002F1A0 File Offset: 0x0002D3A0
	private void OnDestroy()
	{
		this.ReleaseAddressablePayloadAfterDestruction();
		if (this.Owner != null && this.Owner.View == this)
		{
			PoiResidencyStateKey poiResidencyStateKey;
			if (!PoiResidencyPlacement.TryGetKey(this.Owner, "slot-consumed", out poiResidencyStateKey) || this.IsConsumed)
			{
				this.Owner.Consumed = true;
			}
			this.Owner.View = null;
		}
	}

	// Token: 0x0400053F RID: 1343
	private TrashcanLootInteractable can;

	// Token: 0x04000540 RID: 1344
	private WasteCanHideInteractable hide;

	// Token: 0x04000541 RID: 1345
	private HideFlashPickup[] pickups;

	// Token: 0x04000542 RID: 1346
	private DumpsterHideInteractable[] dumpsters;

	// Token: 0x04000543 RID: 1347
	private LockerHideInteractable[] lockers;

	// Token: 0x04000544 RID: 1348
	private Collider[] solidColliders;

	// Token: 0x04000545 RID: 1349
	private Animator[] animators;

	// Token: 0x04000546 RID: 1350
	private BoxCollider foliageBox;

	// Token: 0x04000547 RID: 1351
	private bool initialized;

	// Token: 0x04000548 RID: 1352
	private int generation;

	// Token: 0x04000549 RID: 1353
	private HeavyZoneAddressableResidency.Lease addressablePayloadLease;

	// Token: 0x0400054A RID: 1354
	private readonly Dictionary<Component, string> stateIds = new Dictionary<Component, string>();

	// Token: 0x0400054B RID: 1355
	private bool scopedPlacement;

	// Token: 0x0400054E RID: 1358
	private bool resuming;

	// Token: 0x0400054F RID: 1359
	internal bool RetirementQueued;
}
