using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Slip;
using Invector.vCharacterController;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.GoldBikini
{
	// Token: 0x020011B9 RID: 4537
	public class BikiniBraActionsUI : MonoBehaviour
	{
		// Token: 0x06008B37 RID: 35639 RVA: 0x0038E24C File Offset: 0x0038C44C
		private void Awake()
		{
			if (this.braController == null || this.pantiesController == null)
			{
				this.ResolveFlashControllers();
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<BikiniPickupController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipDiveController == null)
			{
				this.slipDiveController = base.GetComponent<BikiniSlipDiveController>();
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			this._runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._runtimeConfig.objectName = this.objectName;
			this._runtimeConfig.hideFlags = HideFlags.HideAndDontSave;
			this._runtimeConfig.usableActions = this._actions.Actions;
		}

		// Token: 0x06008B38 RID: 35640 RVA: 0x0038E344 File Offset: 0x0038C544
		private BikiniBraActionsUI.Piece ActivePiece()
		{
			if (this._inventory == null)
			{
				return BikiniBraActionsUI.Piece.None;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			if (wearableItemSO == null)
			{
				return BikiniBraActionsUI.Piece.None;
			}
			if (BikiniBraActionsUI.IsItem(wearableItemSO, this.braClothItem, this.braItemId))
			{
				return BikiniBraActionsUI.Piece.Bra;
			}
			if (BikiniBraActionsUI.IsItem(wearableItemSO, this.pantiesClothItem, this.pantiesItemId))
			{
				return BikiniBraActionsUI.Piece.Panties;
			}
			return BikiniBraActionsUI.Piece.None;
		}

		// Token: 0x06008B39 RID: 35641 RVA: 0x0038E3AA File Offset: 0x0038C5AA
		private static bool IsItem(WearableItemSO sel, WearableItemSO item, string id)
		{
			if (item != null)
			{
				return sel == item;
			}
			return !string.IsNullOrEmpty(id) && sel.itemId == id;
		}

		// Token: 0x06008B3A RID: 35642 RVA: 0x0038E3D3 File Offset: 0x0038C5D3
		private void OnDestroy()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x06008B3B RID: 35643 RVA: 0x0038E3EE File Offset: 0x0038C5EE
		private void OnDisable()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._runtimeConfig);
			}
		}

		// Token: 0x06008B3C RID: 35644 RVA: 0x0038E410 File Offset: 0x0038C610
		private void ResolveFlashControllers()
		{
			base.GetComponentsInChildren<ArmsFlashController>(true, this._flashControllerCandidates);
			foreach (ArmsFlashController armsFlashController in this._flashControllerCandidates)
			{
				string text = ((armsFlashController.Config != null) ? armsFlashController.Config.flashLayerName : null);
				if (this.braController == null && text == this.braLayerName)
				{
					this.braController = armsFlashController;
				}
				else if (this.pantiesController == null && text == this.pantiesLayerName)
				{
					this.pantiesController = armsFlashController;
				}
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06008B3D RID: 35645 RVA: 0x0038E4C8 File Offset: 0x0038C6C8
		private bool BraWorn
		{
			get
			{
				return this.braController != null && this.braController.IsWorn;
			}
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x06008B3E RID: 35646 RVA: 0x0038E4E5 File Offset: 0x0038C6E5
		private bool PantiesWorn
		{
			get
			{
				return this.pantiesController != null && this.pantiesController.IsWorn;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x06008B3F RID: 35647 RVA: 0x0038E504 File Offset: 0x0038C704
		private bool ActiveBusy
		{
			get
			{
				BikiniBraActionsUI.Piece piece = this.ActivePiece();
				if (piece == BikiniBraActionsUI.Piece.Bra)
				{
					return this.braController != null && (this.braController.IsOpen || this.braController.IsBusy);
				}
				return piece == BikiniBraActionsUI.Piece.Panties && this.pantiesController != null && (this.pantiesController.IsOpen || this.pantiesController.IsBusy);
			}
		}

		// Token: 0x06008B40 RID: 35648 RVA: 0x0038E577 File Offset: 0x0038C777
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this.flashInput != null && this.flashInput.GetButtonDown())
			{
				this.ToggleActiveFlash();
			}
			if (this.slipInput != null && this.slipInput.GetButtonDown())
			{
				this.SlipActive();
			}
		}

		// Token: 0x06008B41 RID: 35649 RVA: 0x0038E5B8 File Offset: 0x0038C7B8
		private void ToggleActiveFlash()
		{
			if (this.slipDiveController != null && this.slipDiveController.ReadjustOwnsAction)
			{
				return;
			}
			if (this.braController == null || this.pantiesController == null)
			{
				this.ResolveFlashControllers();
			}
			BikiniBraActionsUI.Piece piece = this.ActivePiece();
			if (piece == BikiniBraActionsUI.Piece.Bra)
			{
				if (this.braController != null && !this.braController.IsOnCooldown)
				{
					this.braController.Toggle();
					return;
				}
			}
			else if (piece == BikiniBraActionsUI.Piece.Panties && this.pantiesController != null && !this.pantiesController.IsOnCooldown)
			{
				this.pantiesController.Toggle();
			}
		}

		// Token: 0x06008B42 RID: 35650 RVA: 0x0038E660 File Offset: 0x0038C860
		private void SlipActive()
		{
			if (this.slipController == null || this.ActiveBusy)
			{
				return;
			}
			BikiniBraActionsUI.Piece piece = this.ActivePiece();
			if (piece == BikiniBraActionsUI.Piece.Bra)
			{
				this.slipController.TrySlipLandPiece(this.braWornName);
				return;
			}
			if (piece == BikiniBraActionsUI.Piece.Panties)
			{
				this.slipController.TrySlipLandPiece(this.pantiesWornName);
			}
		}

		// Token: 0x06008B43 RID: 35651 RVA: 0x0038E6B8 File Offset: 0x0038C8B8
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (this.braController == null || this.pantiesController == null)
			{
				this.ResolveFlashControllers();
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

		// Token: 0x06008B44 RID: 35652 RVA: 0x0038E764 File Offset: 0x0038C964
		private void BuildActions()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return;
			}
			bool flag = this.slipController != null && this.slipController.IsSlipping;
			BikiniBraActionsUI.Piece piece = this.ActivePiece();
			if (this.slipDiveController != null && this.slipDiveController.HasSelectedSlip)
			{
				this._actions.Add(this.slipDiveController.ReadjustKey, "Re-adjust", "");
			}
			else if (piece == BikiniBraActionsUI.Piece.Bra && this.braController != null)
			{
				if (this.braController.IsOpen || this.braController.IsBusy)
				{
					this._actions.Add(this.flashKey, "Cover Up", "");
				}
				else if (this.braController.IsWorn)
				{
					this._actions.Add(this.flashKey, "Flash Top", "");
					if (this.slipController != null && !flag)
					{
						this._actions.Add(this.slipKey, "Slip Top", "");
					}
				}
			}
			else if (piece == BikiniBraActionsUI.Piece.Panties && this.pantiesController != null)
			{
				if (this.pantiesController.IsOpen || this.pantiesController.IsBusy)
				{
					this._actions.Add(this.flashKey, "Pull Up", "");
				}
				else if (this.pantiesController.IsWorn)
				{
					this._actions.Add(this.flashKey, "Flash Bottom", "");
					if (this.slipController != null && !flag)
					{
						this._actions.Add(this.slipKey, "Slip Bottom", "");
					}
				}
			}
			if (this.pickupController != null && this.pickupController.CanPickUp)
			{
				this._actions.Add(this.pickupKey, "Pick Up Bikini", "");
			}
		}

		// Token: 0x04008E12 RID: 36370
		[Header("Controllers (auto-found if empty)")]
		[Tooltip("Generic arms-flash controller for the BRA (its config.flashLayerName == braLayerName).")]
		[SerializeField]
		private ArmsFlashController braController;

		// Token: 0x04008E13 RID: 36371
		[Tooltip("Generic arms-flash controller for the PANTIES (its config.flashLayerName == pantiesLayerName).")]
		[SerializeField]
		private ArmsFlashController pantiesController;

		// Token: 0x04008E14 RID: 36372
		[SerializeField]
		private BikiniPickupController pickupController;

		// Token: 0x04008E15 RID: 36373
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04008E16 RID: 36374
		[Tooltip("Dive blendShape-slip controller (nipple/pussy wardrobe slip). Used to surface the B 're-adjust' prompt for the selected garment. Auto-found.")]
		[SerializeField]
		private BikiniSlipDiveController slipDiveController;

		// Token: 0x04008E17 RID: 36375
		[Header("Inventory selection (active piece = selected cloth)")]
		[Tooltip("Cloth_BikiniBra: quando è il cloth selezionato, B/J agiscono sul bra.")]
		[SerializeField]
		private WearableItemSO braClothItem;

		// Token: 0x04008E18 RID: 36376
		[Tooltip("Cloth_BikiniPanties: quando è il cloth selezionato, B/J agiscono sulle panties.")]
		[SerializeField]
		private WearableItemSO pantiesClothItem;

		// Token: 0x04008E19 RID: 36377
		[SerializeField]
		private string braItemId = "cloth_bikini_bra";

		// Token: 0x04008E1A RID: 36378
		[SerializeField]
		private string pantiesItemId = "cloth_bikini_panties";

		// Token: 0x04008E1B RID: 36379
		[Header("Auto-find (which ArmsFlashController is which)")]
		[SerializeField]
		private string braLayerName = "BikiniBraFlash";

		// Token: 0x04008E1C RID: 36380
		[SerializeField]
		private string pantiesLayerName = "BikiniPantiesFlash";

		// Token: 0x04008E1D RID: 36381
		[Tooltip("Worn-mesh names of the bra/panties pieces inside the generic ClothSlipController.")]
		[SerializeField]
		private string braWornName = "EPBikini_Bra";

		// Token: 0x04008E1E RID: 36382
		[SerializeField]
		private string pantiesWornName = "EPBikini_Panties";

		// Token: 0x04008E1F RID: 36383
		[Header("Display")]
		[SerializeField]
		private string objectName = "Bikini";

		// Token: 0x04008E20 RID: 36384
		[SerializeField]
		private string flashKey = "B";

		// Token: 0x04008E21 RID: 36385
		[SerializeField]
		private string slipKey = "J";

		// Token: 0x04008E22 RID: 36386
		[SerializeField]
		private string pickupKey = "E";

		// Token: 0x04008E23 RID: 36387
		[SerializeField]
		private string readjustKey = "B";

		// Token: 0x04008E24 RID: 36388
		[Header("Input")]
		[SerializeField]
		private GenericInput flashInput = new GenericInput("B", "Y", "B");

		// Token: 0x04008E25 RID: 36389
		[SerializeField]
		private GenericInput slipInput = new GenericInput("J", "X", "J");

		// Token: 0x04008E26 RID: 36390
		private PlayerInventoryManager _inventory;

		// Token: 0x04008E27 RID: 36391
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x04008E28 RID: 36392
		private readonly InteractionActionBuffer _actions = new InteractionActionBuffer();

		// Token: 0x04008E29 RID: 36393
		private readonly List<ArmsFlashController> _flashControllerCandidates = new List<ArmsFlashController>(4);

		// Token: 0x020011BA RID: 4538
		private enum Piece
		{
			// Token: 0x04008E2B RID: 36395
			None,
			// Token: 0x04008E2C RID: 36396
			Bra,
			// Token: 0x04008E2D RID: 36397
			Panties
		}
	}
}
