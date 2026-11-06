using System;
using Features.ClothingLoot;
using Features.Detection;
using Features.DumpsterHide;
using Features.HideFlash;
using Features.LockerHide;
using Features.NPC;
using Features.TrashcanLoot;
using Features.WantedPoster;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.Subway;
using UnityEngine;

// Token: 0x02000097 RID: 151
[DisallowMultipleComponent]
public sealed class PoiResidencyPlacement : MonoBehaviour
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x0002D342 File Offset: 0x0002B542
	// (set) Token: 0x06000655 RID: 1621 RVA: 0x0002D34A File Offset: 0x0002B54A
	public PoiResidencyContext Context { get; private set; }

	// Token: 0x06000656 RID: 1622 RVA: 0x0002D354 File Offset: 0x0002B554
	public static PoiResidencyPlacement Configure(GameObject inactiveZoneRoot, PoiResidencyContext context)
	{
		if (inactiveZoneRoot == null || inactiveZoneRoot.activeInHierarchy)
		{
			throw new InvalidOperationException("Configure POI placement before activation.");
		}
		if (!context.IsValid)
		{
			throw new ArgumentException("Invalid POI placement context.", "context");
		}
		PoiResidencyPlacement poiResidencyPlacement = inactiveZoneRoot.GetComponent<PoiResidencyPlacement>();
		if (poiResidencyPlacement == null)
		{
			poiResidencyPlacement = inactiveZoneRoot.AddComponent<PoiResidencyPlacement>();
		}
		poiResidencyPlacement.Context = context;
		poiResidencyPlacement.bindings = null;
		poiResidencyPlacement.authoredNpcs = Array.Empty<NPCSenses>();
		poiResidencyPlacement.authoredPhoneNpcs = Array.Empty<StaticPhoneNPC>();
		return poiResidencyPlacement;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0002D3D4 File Offset: 0x0002B5D4
	public void BindPayload(GameObject inactivePayload)
	{
		if (inactivePayload == null || inactivePayload.activeInHierarchy)
		{
			throw new InvalidOperationException("Bind POI state before payload activation.");
		}
		if (!inactivePayload.transform.IsChildOf(base.transform))
		{
			throw new InvalidOperationException("POI state must belong to this placement.");
		}
		PoiResidencyStateBindings[] components = inactivePayload.GetComponents<PoiResidencyStateBindings>();
		if (components.Length != 1)
		{
			throw new InvalidOperationException("Expected one authored POI state identity map.");
		}
		components[0].ValidateAndIndex(this.Context.family);
		this.bindings = components[0];
		this.authoredNpcs = inactivePayload.GetComponentsInChildren<NPCSenses>(true);
		this.authoredPhoneNpcs = inactivePayload.GetComponentsInChildren<StaticPhoneNPC>(true);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002D46C File Offset: 0x0002B66C
	public static bool TryGetKey(Component target, string channel, out PoiResidencyStateKey key)
	{
		key = default(PoiResidencyStateKey);
		if (target == null)
		{
			return false;
		}
		StreamedWorldPropView componentInParent = target.GetComponentInParent<StreamedWorldPropView>(true);
		PoiResidencyStateKey poiResidencyStateKey;
		if (!(componentInParent != null) || !(componentInParent.Owner != null) || !PoiResidencyPlacement.TryGetDirectKey(componentInParent.Owner, channel, out poiResidencyStateKey))
		{
			return PoiResidencyPlacement.TryGetDirectKey(target, channel, out key);
		}
		string text;
		if (!componentInParent.TryGetStateId(target, out text))
		{
			return false;
		}
		key = new PoiResidencyStateKey(poiResidencyStateKey.context, PoiResidencyStateBindings.ProjectViewStateId(poiResidencyStateKey.id, text), channel);
		return true;
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
	public void PrepareForActivation(GameObject inactivePayload, Transform loadingGate = null)
	{
		if (inactivePayload == null || inactivePayload.activeInHierarchy || this.bindings == null || this.bindings.gameObject != inactivePayload)
		{
			throw new InvalidOperationException("Prepare the bound POI payload before activation.");
		}
		if (loadingGate != null && (inactivePayload.transform.parent != loadingGate || loadingGate.parent != base.transform))
		{
			throw new InvalidOperationException("The sampling gate must be owned by this POI placement.");
		}
		TrashcanTierRandomizer.PrepareScopedPayload(inactivePayload, loadingGate);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0002D57C File Offset: 0x0002B77C
	private static bool TryGetDirectKey(Component target, string channel, out PoiResidencyStateKey key)
	{
		key = default(PoiResidencyStateKey);
		PoiResidencyPlacement componentInParent = target.GetComponentInParent<PoiResidencyPlacement>(true);
		string text;
		if (componentInParent == null || !PoiResidencyLedger.IsCurrent(componentInParent.Context) || componentInParent.bindings == null || !componentInParent.bindings.TryGetId(target, out text))
		{
			return false;
		}
		key = new PoiResidencyStateKey(componentInParent.Context, text, channel);
		return true;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0002D5E4 File Offset: 0x0002B7E4
	public static bool ReadFlag(Component target, string channel)
	{
		PoiResidencyStateKey poiResidencyStateKey;
		return PoiResidencyPlacement.TryGetKey(target, channel, out poiResidencyStateKey) && PoiResidencyLedger.ReadFlag(poiResidencyStateKey);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002D604 File Offset: 0x0002B804
	public static void WriteFlag(Component target, string channel, bool value)
	{
		PoiResidencyStateKey poiResidencyStateKey;
		if (PoiResidencyPlacement.TryGetKey(target, channel, out poiResidencyStateKey))
		{
			PoiResidencyLedger.WriteFlag(poiResidencyStateKey, value);
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0002D624 File Offset: 0x0002B824
	public static bool TryGetWorldPropTierKey(Component target, out PoiResidencyStateKey key)
	{
		if (!PoiResidencyPlacement.TryGetKey(target, "slot-tier", out key))
		{
			return false;
		}
		int num = key.id.IndexOf("/view/", StringComparison.Ordinal);
		if (num >= 0)
		{
			key = new PoiResidencyStateKey(key.context, key.id.Substring(0, num), key.channel);
		}
		return true;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0002D67C File Offset: 0x0002B87C
	public static void ReplaceWorldProp(GameObject original, GameObject inactiveReplacement)
	{
		if (inactiveReplacement == null || inactiveReplacement.activeInHierarchy)
		{
			throw new InvalidOperationException("Restore tier identity before activation.");
		}
		PoiResidencyPlacement componentInParent = original.GetComponentInParent<PoiResidencyPlacement>(true);
		if (componentInParent == null || componentInParent.bindings == null)
		{
			throw new InvalidOperationException("Missing POI replacement ownership.");
		}
		componentInParent.bindings.ReplaceWorldProp(original, inactiveReplacement);
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x0002D6DC File Offset: 0x0002B8DC
	public bool HasActiveGameplayUse
	{
		get
		{
			if (this.bindings == null)
			{
				return false;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			NPCSenses npcsenses = ((instance != null) ? instance.DetectingNPC : null);
			Transform transform = ((instance != null && instance.IsEventActive) ? instance.ActiveInitiatorTransform : null);
			if (instance != null && instance.IsEventActive && ((npcsenses != null && npcsenses.transform.IsChildOf(base.transform)) || (transform != null && transform.IsChildOf(base.transform))))
			{
				return true;
			}
			foreach (NPCSenses npcsenses2 in this.authoredNpcs)
			{
				if (!(npcsenses2 == null))
				{
					if (instance != null && (instance.HasPendingDetection(npcsenses2) || (instance.IsEventActive && (npcsenses == npcsenses2 || transform == npcsenses2.transform))))
					{
						return true;
					}
					if (npcsenses2.gameObject.activeInHierarchy && (npcsenses2.CurrentAttention > 0f || npcsenses2.IsDetected || npcsenses2.IsInCooldown || npcsenses2.IsGhostFleeing))
					{
						return true;
					}
				}
			}
			foreach (StaticPhoneNPC staticPhoneNPC in this.authoredPhoneNpcs)
			{
				if (staticPhoneNPC != null && staticPhoneNPC.gameObject.activeInHierarchy && (staticPhoneNPC.IsPhotoReactionActive || (staticPhoneNPC.IsEncounterPrepared && staticPhoneNPC.OwnsPresentation)))
				{
					return true;
				}
			}
			PoiResidencyStateBindings.Record[] records = this.bindings.records;
			for (int i = 0; i < records.Length; i++)
			{
				Component target = records[i].target;
				if (!(target == null) && target.gameObject.activeInHierarchy)
				{
					ClothingStoreAlarmController clothingStoreAlarmController;
					if (target is Transform && target.TryGetComponent<ClothingStoreAlarmController>(out clothingStoreAlarmController) && clothingStoreAlarmController.IsActive)
					{
						return true;
					}
					IInteractable interactable = target as IInteractable;
					if (interactable != null && interactable.IsBeingUsed)
					{
						return true;
					}
					BenchSeat benchSeat = target as BenchSeat;
					if (benchSeat != null && benchSeat.OwnerId != 0)
					{
						return true;
					}
					StreamedWorldPropSlot streamedWorldPropSlot = target as StreamedWorldPropSlot;
					if (streamedWorldPropSlot != null && streamedWorldPropSlot.View != null && streamedWorldPropSlot.View.IsBusy)
					{
						return true;
					}
					SubwayRideDirector subwayRideDirector = target as SubwayRideDirector;
					if (subwayRideDirector != null && subwayRideDirector.HasActiveResidencyRide)
					{
						return true;
					}
					DumpsterHideInteractable dumpsterHideInteractable = target as DumpsterHideInteractable;
					if (dumpsterHideInteractable != null && dumpsterHideInteractable.HasActiveResidencyInteraction)
					{
						return true;
					}
					LockerHideInteractable lockerHideInteractable = target as LockerHideInteractable;
					if (lockerHideInteractable != null && lockerHideInteractable.HasActiveResidencyInteraction)
					{
						return true;
					}
					TrashcanLootInteractable trashcanLootInteractable = target as TrashcanLootInteractable;
					if (trashcanLootInteractable != null && trashcanLootInteractable.HasActiveResidencyInteraction)
					{
						return true;
					}
					WasteCanHideInteractable wasteCanHideInteractable = target as WasteCanHideInteractable;
					if (wasteCanHideInteractable != null && wasteCanHideInteractable.IsBusy)
					{
						return true;
					}
					HideFlashPickup hideFlashPickup = target as HideFlashPickup;
					if (hideFlashPickup != null && hideFlashPickup.HasActiveResidencyInteraction)
					{
						return true;
					}
					WantedPosterPickup wantedPosterPickup = target as WantedPosterPickup;
					if (wantedPosterPickup != null && wantedPosterPickup.HasActiveResidencyInteraction)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x040004F1 RID: 1265
	private PoiResidencyStateBindings bindings;

	// Token: 0x040004F2 RID: 1266
	private NPCSenses[] authoredNpcs = Array.Empty<NPCSenses>();

	// Token: 0x040004F3 RID: 1267
	private StaticPhoneNPC[] authoredPhoneNpcs = Array.Empty<StaticPhoneNPC>();
}
