using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.UniversityUniform
{
	// Token: 0x02000BBC RID: 3004
	public sealed class UniversityUniformFlashBridge : MonoBehaviour
	{
		// Token: 0x0600561A RID: 22042 RVA: 0x00217505 File Offset: 0x00215705
		private void Awake()
		{
			this.ResolveReferences();
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x00217510 File Offset: 0x00215710
		private void OnEnable()
		{
			this.ResolveReferences();
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn += this.HandleItemWorn;
				this._wearController.OnItemUnworn += this.HandleItemUnworn;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveClothChanged += this.HandleActiveClothChanged;
			}
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00217584 File Offset: 0x00215784
		private void Start()
		{
			this.RefreshWornState();
			if (this.flashController != null)
			{
				if (this._worn)
				{
					this.flashController.SetRestored();
					UniversityUniformSkirtFlashCoordinator universityUniformSkirtFlashCoordinator = this.skirtCoordinator;
					if (universityUniformSkirtFlashCoordinator != null)
					{
						universityUniformSkirtFlashCoordinator.OnGarmentRestored();
					}
				}
				else
				{
					this.flashController.SetRemoved();
				}
			}
			WearableItemSO selectedWornProvider = this.GetSelectedWornProvider();
			this.BindProvider(selectedWornProvider);
			bool flag = this._worn && selectedWornProvider != null;
			this._mechanicEnabled = !flag;
			this.SetMechanicEnabled(flag);
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x00217608 File Offset: 0x00215808
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
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x00217678 File Offset: 0x00215878
		private void ResolveReferences()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ArmsFlashController>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<UniversityUniformActionsUI>();
			}
			if (this.skirtCoordinator == null)
			{
				this.skirtCoordinator = base.GetComponent<UniversityUniformSkirtFlashCoordinator>();
			}
			if (this.faceController == null)
			{
				this.faceController = base.GetComponent<UniversityUniformFlashFaceController>();
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
			if (this._styleService == null)
			{
				this._styleService = transform.GetComponentInChildren<MCStyleCustomizationService>(true) ?? Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x00217780 File Offset: 0x00215980
		private bool IsSkirtProvider(WearableItemSO item)
		{
			return !(item == null) && ((this.universityUniformItem != null && item == this.universityUniformItem) || (this.universitySkirtItem != null && item == this.universitySkirtItem) || (!string.IsNullOrEmpty(this.universityUniformItemId) && item.itemId == this.universityUniformItemId) || (!string.IsNullOrEmpty(this.universitySkirtItemId) && item.itemId == this.universitySkirtItemId));
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x00217818 File Offset: 0x00215A18
		private void RefreshWornState()
		{
			this._worn = this._wearController != null && ((this.universityUniformItem != null && this._wearController.IsWorn(this.universityUniformItem)) || (this.universitySkirtItem != null && this._wearController.IsWorn(this.universitySkirtItem)));
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x00217884 File Offset: 0x00215A84
		private WearableItemSO GetSelectedWornProvider()
		{
			if (this._inventory == null || this._wearController == null)
			{
				return null;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			if (!this.IsSkirtProvider(wearableItemSO) || !this._wearController.IsWorn(wearableItemSO))
			{
				return null;
			}
			return wearableItemSO;
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x002178DC File Offset: 0x00215ADC
		private void HandleItemWorn(WearableItemSO item)
		{
			if (!this.IsSkirtProvider(item))
			{
				return;
			}
			this.RefreshWornState();
			if (this.flashController != null)
			{
				this.flashController.SetRestored();
			}
			UniversityUniformSkirtFlashCoordinator universityUniformSkirtFlashCoordinator = this.skirtCoordinator;
			if (universityUniformSkirtFlashCoordinator != null)
			{
				universityUniformSkirtFlashCoordinator.OnGarmentRestored();
			}
			this.ApplyState();
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x0021792C File Offset: 0x00215B2C
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (!this.IsSkirtProvider(item))
			{
				return;
			}
			this.RefreshWornState();
			if (this.flashController != null)
			{
				if (this._worn)
				{
					this.flashController.SetRestored();
				}
				else
				{
					this.flashController.SetRemoved();
				}
			}
			this.ApplyState();
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x0021797D File Offset: 0x00215B7D
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x00217988 File Offset: 0x00215B88
		private void ApplyState()
		{
			WearableItemSO selectedWornProvider = this.GetSelectedWornProvider();
			this.BindProvider(selectedWornProvider);
			this.SetMechanicEnabled(this._worn && selectedWornProvider != null);
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x002179BC File Offset: 0x00215BBC
		private void BindProvider(WearableItemSO provider)
		{
			GameObject gameObject = ((provider != null && this._styleService != null) ? this._styleService.GetSpawnedRoot(provider.mcStylePartId) : null);
			Transform transform = ((gameObject != null) ? gameObject.transform : null);
			ArmsFlashController armsFlashController = this.flashController;
			if (armsFlashController != null)
			{
				armsFlashController.SetGarmentSearchRoot(transform);
			}
			UniversityUniformSkirtFlashCoordinator universityUniformSkirtFlashCoordinator = this.skirtCoordinator;
			if (universityUniformSkirtFlashCoordinator == null)
			{
				return;
			}
			universityUniformSkirtFlashCoordinator.SetGarmentRoot(gameObject);
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x00217A2C File Offset: 0x00215C2C
		private void SetMechanicEnabled(bool enabled)
		{
			if (this._mechanicEnabled == enabled)
			{
				return;
			}
			this._mechanicEnabled = enabled;
			if (enabled)
			{
				if (this.skirtCoordinator != null)
				{
					this.skirtCoordinator.enabled = true;
				}
				if (this.faceController != null)
				{
					this.faceController.enabled = true;
				}
				if (this.actionsUI != null)
				{
					this.actionsUI.enabled = true;
				}
				if (this.flashController != null)
				{
					this.flashController.enabled = true;
					return;
				}
			}
			else
			{
				if (this.flashController != null)
				{
					this.flashController.enabled = false;
				}
				if (this.actionsUI != null)
				{
					this.actionsUI.enabled = false;
				}
				if (this.faceController != null)
				{
					this.faceController.enabled = false;
				}
				if (this.skirtCoordinator != null)
				{
					this.skirtCoordinator.enabled = false;
				}
			}
		}

		// Token: 0x0400569C RID: 22172
		[Header("Identity")]
		[SerializeField]
		private WearableItemSO universityUniformItem;

		// Token: 0x0400569D RID: 22173
		[SerializeField]
		private string universityUniformItemId = "cloth_university_uniform_full";

		// Token: 0x0400569E RID: 22174
		[SerializeField]
		private WearableItemSO universitySkirtItem;

		// Token: 0x0400569F RID: 22175
		[SerializeField]
		private string universitySkirtItemId = "cloth_university_uniform_skirt";

		// Token: 0x040056A0 RID: 22176
		[Header("Mechanic")]
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x040056A1 RID: 22177
		[SerializeField]
		private UniversityUniformActionsUI actionsUI;

		// Token: 0x040056A2 RID: 22178
		[SerializeField]
		private UniversityUniformSkirtFlashCoordinator skirtCoordinator;

		// Token: 0x040056A3 RID: 22179
		[SerializeField]
		private UniversityUniformFlashFaceController faceController;

		// Token: 0x040056A4 RID: 22180
		private WearableUseController _wearController;

		// Token: 0x040056A5 RID: 22181
		private PlayerInventoryManager _inventory;

		// Token: 0x040056A6 RID: 22182
		private MCStyleCustomizationService _styleService;

		// Token: 0x040056A7 RID: 22183
		private bool _worn;

		// Token: 0x040056A8 RID: 22184
		private bool _mechanicEnabled;
	}
}
