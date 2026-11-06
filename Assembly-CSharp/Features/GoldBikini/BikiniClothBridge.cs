using System;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.Inventory.Wearable;
using Features.Missions;
using Features.Slip;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.GoldBikini
{
	// Token: 0x020011BC RID: 4540
	public class BikiniClothBridge : MonoBehaviour
	{
		// Token: 0x06008B4E RID: 35662 RVA: 0x0038ED18 File Offset: 0x0038CF18
		private void Awake()
		{
			this.ResolveRefs();
		}

		// Token: 0x06008B4F RID: 35663 RVA: 0x0038ED20 File Offset: 0x0038CF20
		private void ResolveRefs()
		{
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController != null)
			{
				this.slipController.SetLandDropItem(this.bra.wornPieceName, this.bra.clothItem);
				this.slipController.SetLandDropItem(this.panties.wornPieceName, this.panties.clothItem);
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<BikiniPickupController>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<BikiniBraActionsUI>();
			}
			if (this.slipDiveController == null)
			{
				this.slipDiveController = base.GetComponent<BikiniSlipDiveController>();
			}
			if (this.bra.flashController == null)
			{
				this.bra.flashController = this.ResolveFlash(this.bra.flashLayerName);
			}
			if (this.panties.flashController == null)
			{
				this.panties.flashController = this.ResolveFlash(this.panties.flashLayerName);
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			if (this._wearController == null)
			{
				this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			}
			if (this._inventory == null)
			{
				this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			}
		}

		// Token: 0x06008B50 RID: 35664 RVA: 0x0038EEB0 File Offset: 0x0038D0B0
		private void OnEnable()
		{
			this.ResolveRefs();
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn += this.HandleItemWorn;
				this._wearController.OnItemUnworn += this.HandleItemUnworn;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveClothChanged += this.HandleActiveClothChanged;
			}
			if (this.slipController != null)
			{
				this.slipController.OnSlipped.AddListener(new UnityAction(this.HandleSlipped));
				this.slipController.OnRestored.AddListener(new UnityAction(this.ApplyEnvMechanics));
				this.slipController.OnSlipVisualSpawned += this.HandleSlipVisualSpawned;
			}
		}

		// Token: 0x06008B51 RID: 35665 RVA: 0x0038EF84 File Offset: 0x0038D184
		private void OnDisable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn -= this.HandleItemWorn;
				this._wearController.OnItemUnworn -= this.HandleItemUnworn;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveClothChanged -= this.HandleActiveClothChanged;
			}
			if (this.slipController != null)
			{
				this.slipController.OnSlipped.RemoveListener(new UnityAction(this.HandleSlipped));
				this.slipController.OnRestored.RemoveListener(new UnityAction(this.ApplyEnvMechanics));
				this.slipController.OnSlipVisualSpawned -= this.HandleSlipVisualSpawned;
			}
		}

		// Token: 0x06008B52 RID: 35666 RVA: 0x0038F050 File Offset: 0x0038D250
		private void HandleSlipVisualSpawned(string wornName, GameObject visual)
		{
			BikiniClothBridge.PieceLink pieceLink = ((wornName == this.bra.wornPieceName) ? this.bra : ((wornName == this.panties.wornPieceName) ? this.panties : null));
			if (pieceLink == null || pieceLink.clothItem == null || visual == null)
			{
				return;
			}
			int num = this.FindSlot(pieceLink);
			int num2 = ((num >= 0 && this._inventory != null) ? this._inventory.GetClothVariantAt(num) : pieceLink.stashedVariant);
			ItemVariantLook.ApplyToWorldInstance(visual, pieceLink.clothItem, num2);
		}

		// Token: 0x06008B53 RID: 35667 RVA: 0x0038F0EC File Offset: 0x0038D2EC
		private void Start()
		{
			if (this._wearController != null)
			{
				this._braWorn = this._wearController.IsWorn(this.bra.itemId, true);
				this._pantiesWorn = this._wearController.IsWorn(this.panties.itemId, true);
			}
			this.ApplyState();
			this.ApplyEnvMechanics();
		}

		// Token: 0x06008B54 RID: 35668 RVA: 0x0038F150 File Offset: 0x0038D350
		private bool IsItem(BikiniClothBridge.PieceLink p, WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (p.clothItem != null)
			{
				return item == p.clothItem;
			}
			return !string.IsNullOrEmpty(p.itemId) && item.itemId == p.itemId;
		}

		// Token: 0x06008B55 RID: 35669 RVA: 0x0038F1A4 File Offset: 0x0038D3A4
		private void HandleItemWorn(WearableItemSO item)
		{
			if (this.IsItem(this.bra, item))
			{
				this.ResetPieceSlips(this.bra);
				this._braWorn = true;
				this.SetFlashRestored(this.bra);
			}
			else
			{
				if (!this.IsItem(this.panties, item))
				{
					return;
				}
				this.ResetPieceSlips(this.panties);
				this._pantiesWorn = true;
				this.SetFlashRestored(this.panties);
			}
			this.ApplyState();
		}

		// Token: 0x06008B56 RID: 35670 RVA: 0x0038F218 File Offset: 0x0038D418
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (this.IsItem(this.bra, item))
			{
				this.ResetPieceSlips(this.bra);
				this._braWorn = false;
				this.SetFlashRemoved(this.bra);
			}
			else
			{
				if (!this.IsItem(this.panties, item))
				{
					return;
				}
				this.ResetPieceSlips(this.panties);
				this._pantiesWorn = false;
				this.SetFlashRemoved(this.panties);
			}
			this.ApplyState();
		}

		// Token: 0x06008B57 RID: 35671 RVA: 0x0038F28C File Offset: 0x0038D48C
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x06008B58 RID: 35672 RVA: 0x0038F294 File Offset: 0x0038D494
		private bool IsSelected(BikiniClothBridge.PieceLink p)
		{
			if (this._inventory == null)
			{
				return false;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			return this.IsItem(p, wearableItemSO);
		}

		// Token: 0x06008B59 RID: 35673 RVA: 0x0038F2CC File Offset: 0x0038D4CC
		private void ApplyState()
		{
			this.SetFlashEnabled(this.bra, this._braWorn && this.IsSelected(this.bra));
			this.SetFlashEnabled(this.panties, this._pantiesWorn && this.IsSelected(this.panties));
			bool flag = this._braWorn || this._pantiesWorn;
			if (this.actionsUI != null)
			{
				this.actionsUI.enabled = flag;
			}
			if (this.slipDiveController != null)
			{
				this.slipDiveController.enabled = flag;
			}
			this.ApplyEnvMechanics();
		}

		// Token: 0x06008B5A RID: 35674 RVA: 0x0038F36B File Offset: 0x0038D56B
		private void ResetPieceSlips(BikiniClothBridge.PieceLink piece)
		{
			if (this.slipDiveController != null)
			{
				this.slipDiveController.ResetPieceSlips(piece.wornPieceName);
			}
		}

		// Token: 0x06008B5B RID: 35675 RVA: 0x0038F38C File Offset: 0x0038D58C
		private void ApplyEnvMechanics()
		{
			bool flag = this.slipController != null && this.slipController.AnySlipped;
			if (this.pickupController != null)
			{
				this.pickupController.enabled = flag && (this.slipController.IsFloatingPiece(this.bra.wornPieceName) || this.slipController.IsFloatingPiece(this.panties.wornPieceName));
			}
			if (this.theftController != null)
			{
				this.theftController.enabled = flag;
			}
		}

		// Token: 0x06008B5C RID: 35676 RVA: 0x0038F420 File Offset: 0x0038D620
		private void HandleSlipped()
		{
			if (this.slipController == null || this.pickupController == null)
			{
				return;
			}
			this.TryDropPiece(this.bra);
			this.TryDropPiece(this.panties);
			this.pickupController.SetPickupAction(new Func<bool>(this.PickUpToInventory));
			this.ApplyEnvMechanics();
			this.ConfigurePiecePickup(this.bra);
			this.ConfigurePiecePickup(this.panties);
		}

		// Token: 0x06008B5D RID: 35677 RVA: 0x0038F498 File Offset: 0x0038D698
		private void ConfigurePiecePickup(BikiniClothBridge.PieceLink piece)
		{
			if (!this.slipController.IsFloatingPiece(piece.wornPieceName))
			{
				return;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor(piece.wornPieceName);
			if (gameObject == null || piece.clothItem == null)
			{
				return;
			}
			InventoryPickup inventoryPickup = gameObject.GetComponentInChildren<InventoryPickup>();
			if (inventoryPickup == null)
			{
				inventoryPickup = gameObject.AddComponent<InventoryPickup>();
			}
			inventoryPickup.enabled = true;
			inventoryPickup.ConfigureAsClothRestore(piece.clothItem, delegate
			{
				this.pickupController.PickUpNow();
			}, this.slipPickupConfig, new ItemRarity?(piece.stashedRarity), piece.stashedVariant, piece.stashedDurability, false);
		}

		// Token: 0x06008B5E RID: 35678 RVA: 0x0038F538 File Offset: 0x0038D738
		private void TryDropPiece(BikiniClothBridge.PieceLink p)
		{
			if (this.slipController == null || !this.slipController.HasSlippedPiece(p.wornPieceName))
			{
				return;
			}
			MissionGameplaySignals.NotifyWearableAction(p.clothItem, "bikini_slip", "Slip Off");
			int num = this.FindSlot(p);
			if (num >= 0 && this._inventory != null)
			{
				p.stashedRarity = this._inventory.GetClothRarityAt(num);
				p.stashedVariant = this._inventory.GetClothVariantAt(num);
				float clothDurabilityBase = this._inventory.GetClothDurabilityBase(num);
				p.stashedDurability = ((clothDurabilityBase > 0f) ? Mathf.Clamp01(this._inventory.GetClothDurability(num) / clothDurabilityBase) : 1f);
				this._inventory.RemoveCloth(num);
			}
		}

		// Token: 0x06008B5F RID: 35679 RVA: 0x0038F5FB File Offset: 0x0038D7FB
		private bool PickUpToInventory()
		{
			return false | this.PickUpPiece(this.bra) | this.PickUpPiece(this.panties);
		}

		// Token: 0x06008B60 RID: 35680 RVA: 0x0038F618 File Offset: 0x0038D818
		private bool PickUpPiece(BikiniClothBridge.PieceLink p)
		{
			if (this._inventory == null || this.slipController == null)
			{
				return false;
			}
			if (!this.slipController.IsFloatingPiece(p.wornPieceName))
			{
				return false;
			}
			WearableItemSO clothItem = p.clothItem;
			if (clothItem == null)
			{
				return false;
			}
			if (this._inventory.AddClothingReturnIndex(clothItem, true, p.stashedRarity, p.stashedVariant, p.stashedDurability, false) < 0)
			{
				return false;
			}
			this.slipController.DiscardSlipped(p.wornPieceName);
			return true;
		}

		// Token: 0x06008B61 RID: 35681 RVA: 0x0038F6A4 File Offset: 0x0038D8A4
		private int FindSlot(BikiniClothBridge.PieceLink p)
		{
			if (this._inventory == null || p.clothItem == null)
			{
				return -1;
			}
			int maxClothSlots = this._inventory.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this._inventory.GetClothAt(i) == p.clothItem)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06008B62 RID: 35682 RVA: 0x0038F704 File Offset: 0x0038D904
		private ArmsFlashController ResolveFlash(string layerName)
		{
			if (string.IsNullOrEmpty(layerName))
			{
				return null;
			}
			foreach (ArmsFlashController armsFlashController in base.GetComponentsInChildren<ArmsFlashController>(true))
			{
				if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == layerName)
				{
					return armsFlashController;
				}
			}
			return null;
		}

		// Token: 0x06008B63 RID: 35683 RVA: 0x0038F753 File Offset: 0x0038D953
		private void SetFlashEnabled(BikiniClothBridge.PieceLink p, bool on)
		{
			if (p.flashController != null)
			{
				p.flashController.enabled = on;
			}
		}

		// Token: 0x06008B64 RID: 35684 RVA: 0x0038F76F File Offset: 0x0038D96F
		private void SetFlashRestored(BikiniClothBridge.PieceLink p)
		{
			if (p.flashController != null)
			{
				p.flashController.SetRestored();
			}
		}

		// Token: 0x06008B65 RID: 35685 RVA: 0x0038F78A File Offset: 0x0038D98A
		private void SetFlashRemoved(BikiniClothBridge.PieceLink p)
		{
			if (p.flashController != null)
			{
				p.flashController.SetRemoved();
			}
		}

		// Token: 0x04008E36 RID: 36406
		[Header("Pieces")]
		[SerializeField]
		private BikiniClothBridge.PieceLink bra = new BikiniClothBridge.PieceLink
		{
			wornPieceName = "EPBikini_Bra",
			itemId = "cloth_bikini_bra",
			flashLayerName = "BikiniBraFlash"
		};

		// Token: 0x04008E37 RID: 36407
		[SerializeField]
		private BikiniClothBridge.PieceLink panties = new BikiniClothBridge.PieceLink
		{
			wornPieceName = "EPBikini_Panties",
			itemId = "cloth_bikini_panties",
			flashLayerName = "BikiniPantiesFlash"
		};

		// Token: 0x04008E38 RID: 36408
		[Header("Mechanics (auto-found on this GameObject if empty)")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04008E39 RID: 36409
		[SerializeField]
		private BikiniPickupController pickupController;

		// Token: 0x04008E3A RID: 36410
		[SerializeField]
		private MonoBehaviour theftController;

		// Token: 0x04008E3B RID: 36411
		[SerializeField]
		private MonoBehaviour actionsUI;

		// Token: 0x04008E3C RID: 36412
		[SerializeField]
		private BikiniSlipDiveController slipDiveController;

		// Token: 0x04008E3D RID: 36413
		[Header("Slip ↔ pickup")]
		[SerializeField]
		private InteractionConfigSO slipPickupConfig;

		// Token: 0x04008E3E RID: 36414
		private WearableUseController _wearController;

		// Token: 0x04008E3F RID: 36415
		private PlayerInventoryManager _inventory;

		// Token: 0x04008E40 RID: 36416
		private bool _braWorn;

		// Token: 0x04008E41 RID: 36417
		private bool _pantiesWorn;

		// Token: 0x020011BD RID: 4541
		[Serializable]
		public class PieceLink
		{
			// Token: 0x04008E42 RID: 36418
			[Tooltip("Worn-mesh name nel ClothSlipController (es. EPBikini_Bra / EPBikini_Panties).")]
			public string wornPieceName;

			// Token: 0x04008E43 RID: 36419
			[Tooltip("Il WearableItemSO del pezzo (Cloth_BikiniBra / Cloth_BikiniPanties).")]
			public WearableItemSO clothItem;

			// Token: 0x04008E44 RID: 36420
			[Tooltip("Fallback itemId se clothItem è null.")]
			public string itemId;

			// Token: 0x04008E45 RID: 36421
			[Tooltip("Flash GENERICO del pezzo (ArmsFlashController, IFlashGate). Layer bra/panties.")]
			public ArmsFlashController flashController;

			// Token: 0x04008E46 RID: 36422
			[Tooltip("Layer name per auto-risolvere il flashController giusto fra quelli sul GameObject.")]
			public string flashLayerName;

			// Token: 0x04008E47 RID: 36423
			[NonSerialized]
			public ItemRarity stashedRarity;

			// Token: 0x04008E48 RID: 36424
			[NonSerialized]
			public int stashedVariant;

			// Token: 0x04008E49 RID: 36425
			[NonSerialized]
			public float stashedDurability = 1f;
		}
	}
}
