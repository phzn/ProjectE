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

namespace Features.PoolDive
{
	// Token: 0x02000DDE RID: 3550
	public class SwimsuitClothBridge : MonoBehaviour
	{
		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06006917 RID: 26903 RVA: 0x002A0DB4 File Offset: 0x0029EFB4
		public bool AreSwimsuitActionsAvailable
		{
			get
			{
				return this.IsSelected() && (this._worn || (this.flashController != null && this.flashController.IsWorn));
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x002A0DE5 File Offset: 0x0029EFE5
		private void Awake()
		{
			this.ResolveRefs();
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x002A0DF0 File Offset: 0x0029EFF0
		private void ResolveRefs()
		{
			if (this.flashController == null)
			{
				foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
				{
					if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.flashLayerName)
					{
						this.flashController = armsFlashController;
						break;
					}
				}
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController != null)
			{
				this.slipController.SetLandDropItem(this.wornPieceName, this.swimsuitClothItem);
			}
			if (this.diveController == null)
			{
				this.diveController = base.GetComponent<PoolDiveController>();
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<SwimsuitPickupController>();
			}
			if (this.theftController == null)
			{
				this.theftController = base.GetComponent<SwimsuitTheftController>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<SwimsuitActionsUI>();
			}
			if (this.floatToEdge == null)
			{
				this.floatToEdge = base.GetComponent<SwimsuitFloatToEdge>();
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

		// Token: 0x0600691A RID: 26906 RVA: 0x002A0F80 File Offset: 0x0029F180
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

		// Token: 0x0600691B RID: 26907 RVA: 0x002A1054 File Offset: 0x0029F254
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

		// Token: 0x0600691C RID: 26908 RVA: 0x002A1120 File Offset: 0x0029F320
		private void HandleSlipVisualSpawned(string wornName, GameObject visual)
		{
			if (visual == null || wornName != this.wornPieceName || this.swimsuitClothItem == null)
			{
				return;
			}
			int num = this.FindSwimsuitSlot();
			int num2 = ((num >= 0 && this._inventory != null) ? this._inventory.GetClothVariantAt(num) : this._stashedVariant);
			ItemVariantLook.ApplyToWorldInstance(visual, this.swimsuitClothItem, num2);
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x002A1190 File Offset: 0x0029F390
		private void Start()
		{
			bool areSwimsuitActionsAvailable = this.AreSwimsuitActionsAvailable;
			this._enabled = !areSwimsuitActionsAvailable;
			this.SetMechanicsEnabled(areSwimsuitActionsAvailable);
			this.ApplyEnvMechanics();
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x002A11BC File Offset: 0x0029F3BC
		private bool IsOurItem(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.swimsuitClothItem != null)
			{
				return item == this.swimsuitClothItem;
			}
			return !string.IsNullOrEmpty(this.swimsuitItemId) && item.itemId == this.swimsuitItemId;
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x002A120F File Offset: 0x0029F40F
		private void HandleItemWorn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = true;
			if (this.flashController != null)
			{
				this.flashController.SetRestored();
			}
			this.ApplyState();
		}

		// Token: 0x06006920 RID: 26912 RVA: 0x002A1241 File Offset: 0x0029F441
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = false;
			if (this.flashController != null)
			{
				this.flashController.SetRemoved();
			}
			this.ApplyState();
		}

		// Token: 0x06006921 RID: 26913 RVA: 0x002A1273 File Offset: 0x0029F473
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x06006922 RID: 26914 RVA: 0x002A127C File Offset: 0x0029F47C
		private bool IsSelected()
		{
			if (this._inventory == null)
			{
				return false;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			return this.IsOurItem(wearableItemSO);
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x002A12B1 File Offset: 0x0029F4B1
		private void ApplyState()
		{
			this.SetMechanicsEnabled(this.AreSwimsuitActionsAvailable);
			this.ApplyEnvMechanics();
		}

		// Token: 0x06006924 RID: 26916 RVA: 0x002A12C8 File Offset: 0x0029F4C8
		private void SetMechanicsEnabled(bool on)
		{
			if (this.actionsUI != null && !this.actionsUI.enabled)
			{
				this.actionsUI.enabled = true;
			}
			if (on == this._enabled)
			{
				return;
			}
			this._enabled = on;
			if (this.flashController != null)
			{
				this.flashController.enabled = on;
			}
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x002A1328 File Offset: 0x0029F528
		private void ApplyEnvMechanics()
		{
			bool flag = this.slipController != null && this.slipController.AnySlipped;
			if (this.pickupController != null)
			{
				this.pickupController.enabled = flag && this.slipController.IsFloatingPiece(this.wornPieceName);
			}
			if (this.theftController != null)
			{
				this.theftController.enabled = flag;
			}
			if (this.floatToEdge != null)
			{
				this.floatToEdge.enabled = flag && this.slipController.IsFloatingPiece(this.wornPieceName);
			}
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x002A13CC File Offset: 0x0029F5CC
		private void HandleSlipped()
		{
			if (this.slipController == null || this.pickupController == null)
			{
				return;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor(this.wornPieceName);
			if (gameObject == null)
			{
				return;
			}
			MissionGameplaySignals.NotifyWearableAction(this.swimsuitClothItem, "swimsuit_slip", "Slip Off");
			this.pickupController.SetPickupAction(new Func<bool>(this.PickUpToInventory));
			int num = this.FindSwimsuitSlot();
			if (num >= 0 && this._inventory != null)
			{
				this._stashedRarity = this._inventory.GetClothRarityAt(num);
				this._stashedVariant = this._inventory.GetClothVariantAt(num);
				float clothDurabilityBase = this._inventory.GetClothDurabilityBase(num);
				this._stashedDurability = ((clothDurabilityBase > 0f) ? Mathf.Clamp01(this._inventory.GetClothDurability(num) / clothDurabilityBase) : 1f);
				this._inventory.RemoveCloth(num);
			}
			this.ApplyEnvMechanics();
			if (!this.slipController.IsFloatingPiece(this.wornPieceName))
			{
				return;
			}
			InventoryPickup inventoryPickup = gameObject.GetComponentInChildren<InventoryPickup>();
			if (inventoryPickup == null)
			{
				inventoryPickup = gameObject.AddComponent<InventoryPickup>();
			}
			inventoryPickup.enabled = true;
			inventoryPickup.ConfigureAsClothRestore(this.swimsuitClothItem, delegate
			{
				this.pickupController.PickUpNow();
			}, this.slipPickupConfig, new ItemRarity?(this._stashedRarity), this._stashedVariant, this._stashedDurability, false);
		}

		// Token: 0x06006927 RID: 26919 RVA: 0x002A152C File Offset: 0x0029F72C
		private bool PickUpToInventory()
		{
			if (this._inventory == null || this.swimsuitClothItem == null)
			{
				return false;
			}
			if (this._inventory.AddClothingReturnIndex(this.swimsuitClothItem, true, this._stashedRarity, this._stashedVariant, this._stashedDurability, false) < 0)
			{
				return false;
			}
			if (this.slipController != null)
			{
				this.slipController.DiscardSlipped(this.wornPieceName);
			}
			return true;
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x002A15A4 File Offset: 0x0029F7A4
		private int FindSwimsuitSlot()
		{
			if (this._inventory == null || this.swimsuitClothItem == null)
			{
				return -1;
			}
			int maxClothSlots = this._inventory.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this._inventory.GetClothAt(i) == this.swimsuitClothItem)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04006BBD RID: 27581
		[Header("Identity")]
		[Tooltip("Il WearableItemSO del cloth Swimsuit (Cloth_Swimsuit).")]
		[SerializeField]
		private WearableItemSO swimsuitClothItem;

		// Token: 0x04006BBE RID: 27582
		[SerializeField]
		private string swimsuitItemId = "cloth_swimsuit";

		// Token: 0x04006BBF RID: 27583
		[Header("Mechanics (auto-found on this GameObject if empty)")]
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x04006BC0 RID: 27584
		[SerializeField]
		private string flashLayerName = "SwimsuitFlash";

		// Token: 0x04006BC1 RID: 27585
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006BC2 RID: 27586
		[SerializeField]
		private PoolDiveController diveController;

		// Token: 0x04006BC3 RID: 27587
		[SerializeField]
		private SwimsuitPickupController pickupController;

		// Token: 0x04006BC4 RID: 27588
		[SerializeField]
		private SwimsuitTheftController theftController;

		// Token: 0x04006BC5 RID: 27589
		[SerializeField]
		private SwimsuitActionsUI actionsUI;

		// Token: 0x04006BC6 RID: 27590
		[SerializeField]
		private SwimsuitFloatToEdge floatToEdge;

		// Token: 0x04006BC7 RID: 27591
		[Header("Slip ↔ pickup unification")]
		[Tooltip("Worn-mesh name della piece swimsuit nel ClothSlipController.")]
		[SerializeField]
		private string wornPieceName = "School_Swimsuit";

		// Token: 0x04006BC8 RID: 27592
		[Tooltip("Config opzionale per il prompt 'E' sull'env caduto dallo slip (riusa quella del drop se vuoto).")]
		[SerializeField]
		private InteractionConfigSO slipPickupConfig;

		// Token: 0x04006BC9 RID: 27593
		private WearableUseController _wearController;

		// Token: 0x04006BCA RID: 27594
		private PlayerInventoryManager _inventory;

		// Token: 0x04006BCB RID: 27595
		private bool _worn;

		// Token: 0x04006BCC RID: 27596
		private bool _enabled;

		// Token: 0x04006BCD RID: 27597
		private ItemRarity _stashedRarity;

		// Token: 0x04006BCE RID: 27598
		private int _stashedVariant = -1;

		// Token: 0x04006BCF RID: 27599
		private float _stashedDurability = 1f;
	}
}
