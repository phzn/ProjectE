using System;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Missions;
using Features.Slip;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TowelSystem
{
	// Token: 0x02000C04 RID: 3076
	public class TowelClothBridge : MonoBehaviour
	{
		// Token: 0x060058D2 RID: 22738 RVA: 0x0022BF6C File Offset: 0x0022A16C
		private void Awake()
		{
			this.ResolveRefs();
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x0022BF74 File Offset: 0x0022A174
		private void ResolveRefs()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ClothFullFlashController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController != null)
			{
				this.slipController.SetLandDropItem(this.wornPieceName, this.towelClothItem);
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<TowelPickupController>();
			}
			if (this.theftController == null)
			{
				this.theftController = base.GetComponent<TowelTheftController>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<TowelActionsUI>();
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

		// Token: 0x060058D4 RID: 22740 RVA: 0x0022C098 File Offset: 0x0022A298
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

		// Token: 0x060058D5 RID: 22741 RVA: 0x0022C16C File Offset: 0x0022A36C
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

		// Token: 0x060058D6 RID: 22742 RVA: 0x0022C238 File Offset: 0x0022A438
		private void HandleSlipVisualSpawned(string wornName, GameObject visual)
		{
			if (visual == null || wornName != this.wornPieceName || this.towelClothItem == null)
			{
				return;
			}
			int num = this.FindTowelSlot();
			int num2 = ((num >= 0 && this._inventory != null) ? this._inventory.GetClothVariantAt(num) : this._stashedVariant);
			ItemVariantLook.ApplyToWorldInstance(visual, this.towelClothItem, num2);
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x0022C2A8 File Offset: 0x0022A4A8
		private void Start()
		{
			bool flag = this._worn && this.IsSelected();
			this._enabled = !flag;
			this.SetMechanicsEnabled(flag);
			this.ApplyEnvMechanics();
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x0022C2E0 File Offset: 0x0022A4E0
		private bool IsOurItem(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.towelClothItem != null)
			{
				return item == this.towelClothItem;
			}
			return !string.IsNullOrEmpty(this.towelItemId) && item.itemId == this.towelItemId;
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x0022C333 File Offset: 0x0022A533
		private void HandleItemWorn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = true;
			this.ApplyState();
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x0022C34C File Offset: 0x0022A54C
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = false;
			this.ApplyState();
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x0022C365 File Offset: 0x0022A565
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x0022C370 File Offset: 0x0022A570
		private bool IsSelected()
		{
			if (this._inventory == null)
			{
				return false;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			return this.IsOurItem(wearableItemSO);
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x0022C3A5 File Offset: 0x0022A5A5
		private void ApplyState()
		{
			this.SetMechanicsEnabled(this._worn && this.IsSelected());
			this.ApplyEnvMechanics();
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x0022C3C4 File Offset: 0x0022A5C4
		private void SetMechanicsEnabled(bool on)
		{
			if (on == this._enabled)
			{
				return;
			}
			this._enabled = on;
			if (this.actionsUI != null)
			{
				this.actionsUI.enabled = on;
			}
			if (this.flashController != null)
			{
				this.flashController.enabled = on;
			}
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x0022C418 File Offset: 0x0022A618
		private void ApplyEnvMechanics()
		{
			bool flag = this.slipController != null && this.slipController.AnySlipped;
			if (this.pickupController != null)
			{
				this.pickupController.enabled = false;
			}
			if (this.theftController != null)
			{
				this.theftController.enabled = flag;
			}
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x0022C478 File Offset: 0x0022A678
		private void HandleSlipped()
		{
			if (this.slipController == null || this.pickupController == null)
			{
				return;
			}
			if (this.slipController.EnvInstanceFor(this.wornPieceName) == null)
			{
				return;
			}
			MissionGameplaySignals.NotifyWearableAction(this.towelClothItem, "towel_slip", "Drop the Towel");
			this.pickupController.SetPickupAction(new Func<bool>(this.PickUpToInventory));
			int num = this.FindTowelSlot();
			if (num >= 0 && this._inventory != null)
			{
				this._stashedRarity = this._inventory.GetClothRarityAt(num);
				this._stashedVariant = this._inventory.GetClothVariantAt(num);
				float clothDurabilityBase = this._inventory.GetClothDurabilityBase(num);
				this._stashedDurability = ((clothDurabilityBase > 0f) ? Mathf.Clamp01(this._inventory.GetClothDurability(num) / clothDurabilityBase) : 1f);
				this._inventory.RemoveCloth(num);
			}
			this.ApplyEnvMechanics();
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x0022C56C File Offset: 0x0022A76C
		private bool PickUpToInventory()
		{
			if (this._inventory == null || this.towelClothItem == null)
			{
				return false;
			}
			if (this._inventory.AddClothingReturnIndex(this.towelClothItem, true, this._stashedRarity, this._stashedVariant, this._stashedDurability, false) < 0)
			{
				return false;
			}
			if (this.slipController != null)
			{
				this.slipController.DiscardSlipped(this.wornPieceName);
			}
			return true;
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x0022C5E4 File Offset: 0x0022A7E4
		private int FindTowelSlot()
		{
			if (this._inventory == null || this.towelClothItem == null)
			{
				return -1;
			}
			int maxClothSlots = this._inventory.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this._inventory.GetClothAt(i) == this.towelClothItem)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04005962 RID: 22882
		[Header("Identity")]
		[Tooltip("Il WearableItemSO del cloth Towel (Cloth_Towel).")]
		[SerializeField]
		private WearableItemSO towelClothItem;

		// Token: 0x04005963 RID: 22883
		[SerializeField]
		private string towelItemId = "cloth_towel";

		// Token: 0x04005964 RID: 22884
		[Header("Mechanics (auto-found on this GameObject if empty)")]
		[Tooltip("Towel-flash full-body sul layer dedicato ClothFlashFull (gemello di ClothFlashArms). Il bridge abilita il controller quando il towel è worn+selected; il flash B fa il toggle.")]
		[SerializeField]
		private ClothFullFlashController flashController;

		// Token: 0x04005965 RID: 22885
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04005966 RID: 22886
		[SerializeField]
		private TowelPickupController pickupController;

		// Token: 0x04005967 RID: 22887
		[SerializeField]
		private TowelTheftController theftController;

		// Token: 0x04005968 RID: 22888
		[SerializeField]
		private TowelActionsUI actionsUI;

		// Token: 0x04005969 RID: 22889
		[Header("Slip ↔ pickup unification")]
		[Tooltip("Worn-mesh name della piece towel nel ClothSlipController.")]
		[SerializeField]
		private string wornPieceName = "Bath_Towel";

		// Token: 0x0400596A RID: 22890
		[Tooltip("Config opzionale per il prompt 'E' sull'env caduto dallo slip.")]
		[SerializeField]
		private InteractionConfigSO slipPickupConfig;

		// Token: 0x0400596B RID: 22891
		private WearableUseController _wearController;

		// Token: 0x0400596C RID: 22892
		private PlayerInventoryManager _inventory;

		// Token: 0x0400596D RID: 22893
		private bool _worn;

		// Token: 0x0400596E RID: 22894
		private bool _enabled;

		// Token: 0x0400596F RID: 22895
		private ItemRarity _stashedRarity;

		// Token: 0x04005970 RID: 22896
		private int _stashedVariant = -1;

		// Token: 0x04005971 RID: 22897
		private float _stashedDurability = 1f;
	}
}
