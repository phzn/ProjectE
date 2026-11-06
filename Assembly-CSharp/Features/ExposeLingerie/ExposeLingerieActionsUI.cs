using System;
using Features.DebugTools;
using Features.Inventory;
using Features.Inventory.Wearable;
using Invector.vCharacterController;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.ExposeLingerie
{
	// Token: 0x02001212 RID: 4626
	public class ExposeLingerieActionsUI : MonoBehaviour
	{
		// Token: 0x06008EFA RID: 36602 RVA: 0x003A7C78 File Offset: 0x003A5E78
		private void Awake()
		{
			if (this.exposeController == null)
			{
				this.exposeController = base.GetComponent<ExposeLingerieController>();
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			this._runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._runtimeConfig.objectName = this.objectName;
			this._runtimeConfig.hideFlags = HideFlags.HideAndDontSave;
			this._runtimeConfig.usableActions = this._actions.Actions;
		}

		// Token: 0x06008EFB RID: 36603 RVA: 0x003A7D2F File Offset: 0x003A5F2F
		private void OnEnable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemUnworn += this.HandleItemUnworn;
				this._wearController.OnItemWorn += this.HandleItemWorn;
			}
		}

		// Token: 0x06008EFC RID: 36604 RVA: 0x003A7D70 File Offset: 0x003A5F70
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (this.exposeController == null || item == null)
			{
				return;
			}
			if (ExposeLingerieActionsUI.IsItem(item, this.braClothItem, this.braItemId))
			{
				this.exposeController.NotifyGarmentRemoved(true);
				return;
			}
			if (ExposeLingerieActionsUI.IsItem(item, this.pantiesClothItem, this.pantiesItemId))
			{
				this.exposeController.NotifyGarmentRemoved(false);
			}
		}

		// Token: 0x06008EFD RID: 36605 RVA: 0x003A7DD8 File Offset: 0x003A5FD8
		private void HandleItemWorn(WearableItemSO item)
		{
			if (this.exposeController == null || item == null)
			{
				return;
			}
			if (ExposeLingerieActionsUI.IsItem(item, this.braClothItem, this.braItemId))
			{
				this.exposeController.ClearOpenLatch(true);
				return;
			}
			if (ExposeLingerieActionsUI.IsItem(item, this.pantiesClothItem, this.pantiesItemId))
			{
				this.exposeController.ClearOpenLatch(false);
			}
		}

		// Token: 0x06008EFE RID: 36606 RVA: 0x003A7E3E File Offset: 0x003A603E
		private void OnDestroy()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x06008EFF RID: 36607 RVA: 0x003A7E5C File Offset: 0x003A605C
		private void OnDisable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemUnworn -= this.HandleItemUnworn;
				this._wearController.OnItemWorn -= this.HandleItemWorn;
			}
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._runtimeConfig);
			}
		}

		// Token: 0x06008F00 RID: 36608 RVA: 0x003A7EC4 File Offset: 0x003A60C4
		private ExposeLingerieActionsUI.Piece ActivePiece(out WearableItemSO selected)
		{
			selected = ((this._inventory != null) ? (this._inventory.GetActiveCloth() as WearableItemSO) : null);
			if (selected == null)
			{
				return ExposeLingerieActionsUI.Piece.None;
			}
			if (ExposeLingerieActionsUI.IsItem(selected, this.braClothItem, this.braItemId))
			{
				return ExposeLingerieActionsUI.Piece.Bra;
			}
			if (ExposeLingerieActionsUI.IsItem(selected, this.pantiesClothItem, this.pantiesItemId))
			{
				return ExposeLingerieActionsUI.Piece.Panties;
			}
			return ExposeLingerieActionsUI.Piece.None;
		}

		// Token: 0x06008F01 RID: 36609 RVA: 0x0038E3AA File Offset: 0x0038C5AA
		private static bool IsItem(WearableItemSO sel, WearableItemSO item, string id)
		{
			if (item != null)
			{
				return sel == item;
			}
			return !string.IsNullOrEmpty(id) && sel.itemId == id;
		}

		// Token: 0x06008F02 RID: 36610 RVA: 0x003A7F2F File Offset: 0x003A612F
		private bool IsWorn(WearableItemSO item)
		{
			return this._wearController != null && this._wearController.IsWorn(item);
		}

		// Token: 0x06008F03 RID: 36611 RVA: 0x003A7F50 File Offset: 0x003A6150
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this.exposeController == null || this.exposeController.IsPlaying || this.exposeController.IsOnCooldown)
			{
				return;
			}
			if (this.exposeInput == null || !this.exposeInput.GetButtonDown())
			{
				return;
			}
			WearableItemSO wearableItemSO;
			ExposeLingerieActionsUI.Piece piece = this.ActivePiece(out wearableItemSO);
			if (piece == ExposeLingerieActionsUI.Piece.None || !this.IsWorn(wearableItemSO))
			{
				return;
			}
			if (piece == ExposeLingerieActionsUI.Piece.Bra)
			{
				this.exposeController.ToggleBra();
				return;
			}
			this.exposeController.TogglePanties();
		}

		// Token: 0x06008F04 RID: 36612 RVA: 0x003A7FD8 File Offset: 0x003A61D8
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			this._actions.Begin();
			this.BuildActions();
			bool flag = this._actions.End();
			if (this._actions.Actions.Count == 0)
			{
				instance.HideIfCurrent(this._runtimeConfig);
				return;
			}
			if (flag || instance.CurrentConfig == null)
			{
				this._runtimeConfig.objectName = this.objectName;
				instance.Show(this._runtimeConfig);
			}
		}

		// Token: 0x06008F05 RID: 36613 RVA: 0x003A8060 File Offset: 0x003A6260
		private void BuildActions()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return;
			}
			if (this.exposeController == null || this.exposeController.IsPlaying)
			{
				return;
			}
			WearableItemSO wearableItemSO;
			ExposeLingerieActionsUI.Piece piece = this.ActivePiece(out wearableItemSO);
			if (piece == ExposeLingerieActionsUI.Piece.None)
			{
				return;
			}
			if (this._wearController != null)
			{
				this._wearController.AppendVisibleWearActions(wearableItemSO, this._actions);
			}
			if (this.IsWorn(wearableItemSO))
			{
				bool flag = ((piece == ExposeLingerieActionsUI.Piece.Bra) ? this.exposeController.IsBraOpen : this.exposeController.IsPantiesOpen);
				string text = ((piece == ExposeLingerieActionsUI.Piece.Bra) ? (flag ? this.braCoverLabel : this.braActionLabel) : (flag ? this.pantiesCoverLabel : this.pantiesActionLabel));
				this._actions.Add(this.exposeKey, text, "");
			}
		}

		// Token: 0x0400927B RID: 37499
		[Header("Controller (auto-found if empty)")]
		[SerializeField]
		private ExposeLingerieController exposeController;

		// Token: 0x0400927C RID: 37500
		[Header("Inventory selection (active piece = selected cloth)")]
		[SerializeField]
		private WearableItemSO braClothItem;

		// Token: 0x0400927D RID: 37501
		[SerializeField]
		private WearableItemSO pantiesClothItem;

		// Token: 0x0400927E RID: 37502
		[SerializeField]
		private string braItemId = "cloth_expose_bra";

		// Token: 0x0400927F RID: 37503
		[SerializeField]
		private string pantiesItemId = "cloth_expose_panties";

		// Token: 0x04009280 RID: 37504
		[Header("Display")]
		[SerializeField]
		private string objectName = "Lingerie";

		// Token: 0x04009281 RID: 37505
		[SerializeField]
		private string exposeKey = "B";

		// Token: 0x04009282 RID: 37506
		[SerializeField]
		private string braActionLabel = "Expose Breasts";

		// Token: 0x04009283 RID: 37507
		[SerializeField]
		private string pantiesActionLabel = "Expose Pussy";

		// Token: 0x04009284 RID: 37508
		[SerializeField]
		private string braCoverLabel = "Cover Breasts";

		// Token: 0x04009285 RID: 37509
		[SerializeField]
		private string pantiesCoverLabel = "Cover Pussy";

		// Token: 0x04009286 RID: 37510
		[Header("Input")]
		[SerializeField]
		private GenericInput exposeInput = new GenericInput("B", "Y", "B");

		// Token: 0x04009287 RID: 37511
		private PlayerInventoryManager _inventory;

		// Token: 0x04009288 RID: 37512
		private WearableUseController _wearController;

		// Token: 0x04009289 RID: 37513
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x0400928A RID: 37514
		private readonly InteractionActionBuffer _actions = new InteractionActionBuffer();

		// Token: 0x02001213 RID: 4627
		private enum Piece
		{
			// Token: 0x0400928C RID: 37516
			None,
			// Token: 0x0400928D RID: 37517
			Bra,
			// Token: 0x0400928E RID: 37518
			Panties
		}
	}
}
