using System;
using Features.Inventory;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.HideFlash
{
	// Token: 0x020011AC RID: 4524
	public class HideFlashPickup : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionHandoffCandidate
	{
		// Token: 0x06008AA0 RID: 35488 RVA: 0x0038A5F0 File Offset: 0x003887F0
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			if (field == UniversityGameplayField.HideFlashPickup_interactionConfig)
			{
				return this.interactionConfig;
			}
			if (field != UniversityGameplayField.HideFlashPickup_itemData)
			{
				throw new InvalidOperationException("Unsupported HideFlashPickup gameplay binding: " + field.ToString());
			}
			return this.itemData;
		}

		// Token: 0x06008AA1 RID: 35489 RVA: 0x0038A628 File Offset: 0x00388828
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			if (field == UniversityGameplayField.HideFlashPickup_interactionConfig)
			{
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			}
			if (field != UniversityGameplayField.HideFlashPickup_itemData)
			{
				throw new InvalidOperationException("Unsupported HideFlashPickup gameplay binding: " + field.ToString());
			}
			this.itemData = (HideFlashItemSO)asset;
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06008AA2 RID: 35490 RVA: 0x0038A676 File Offset: 0x00388876
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06008AA3 RID: 35491 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06008AA4 RID: 35492 RVA: 0x0038A67E File Offset: 0x0038887E
		public bool HasActiveResidencyInteraction
		{
			get
			{
				return this._pickupInProgress;
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06008AA5 RID: 35493 RVA: 0x0038A688 File Offset: 0x00388888
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._playerTransform != null))
				{
					return float.MaxValue;
				}
				return (base.transform.position - this._playerTransform.position).sqrMagnitude;
			}
		}

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06008AA6 RID: 35494 RVA: 0x0038A6CC File Offset: 0x003888CC
		public bool IsCandidateEligible
		{
			get
			{
				return this.CandidateSqrDistance <= this.detectionRadius * this.detectionRadius && this.CanInteract(this.playerRoot);
			}
		}

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06008AA7 RID: 35495 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06008AA8 RID: 35496 RVA: 0x0038A6F4 File Offset: 0x003888F4
		public bool CanPrioritizePickup
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanAcceptWorldPickup(this.itemData, null, -1, true);
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06008AA9 RID: 35497 RVA: 0x0038A72D File Offset: 0x0038892D
		public bool CanPrepareInteraction
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanPrepareWorldPickupSwap(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06008AAA RID: 35498 RVA: 0x0038A766 File Offset: 0x00388966
		private bool CanQuickSwap
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x06008AAB RID: 35499 RVA: 0x0038A79F File Offset: 0x0038899F
		private void OnEnable()
		{
			if (PoiResidencyPlacement.ReadFlag(this, "pickup-consumed"))
			{
				base.gameObject.SetActive(false);
				Object.Destroy(base.gameObject);
				return;
			}
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06008AAC RID: 35500 RVA: 0x0038A7CC File Offset: 0x003889CC
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._isPlayerInRange = false;
			this._inOwnRange = false;
			this._sqrDistToPlayer = float.MaxValue;
			this._holdInput.Reset();
			if (this.promptUI != null && this.promptUI.gameObject != base.gameObject)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008AAD RID: 35501 RVA: 0x0038A83A File Offset: 0x00388A3A
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
		}

		// Token: 0x06008AAE RID: 35502 RVA: 0x0038A858 File Offset: 0x00388A58
		private void Start()
		{
			this.ResolvePlayerReferences();
			if (this._inventoryManager == null && this.debugLogs)
			{
				Debug.LogWarning("[HideFlashPickup] PlayerInventoryManager not found. Assign playerRoot in the inspector.", this);
			}
			if (this.promptUI != null && this.promptUI.gameObject != base.gameObject)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008AAF RID: 35503 RVA: 0x0038A8C4 File Offset: 0x00388AC4
		private void Update()
		{
			if (this._playerTransform == null || this._inventoryManager == null)
			{
				if (Time.frameCount < this._nextReferenceResolveFrame)
				{
					return;
				}
				this._nextReferenceResolveFrame = Time.frameCount + 30 + (base.GetInstanceID() & 15);
				if (!this.ResolvePlayerReferences())
				{
					return;
				}
			}
			this._sqrDistToPlayer = (base.transform.position - this._playerTransform.position).sqrMagnitude;
			this._inOwnRange = this._sqrDistToPlayer <= this.detectionRadius * this.detectionRadius;
			bool flag = this._inOwnRange && this.CanInteract(this.playerRoot) && InteractionCandidateRegistry.IsWinner(this) && (PlayerActivityGate.Allows(PlayerActivity.Interact) || this.CanPrepareInteraction);
			bool flag2 = flag && this.RefreshSwapHint();
			if (flag && (!this._isPlayerInRange || flag2 || (this.promptUI != null && !this.promptUI.IsVisible)))
			{
				this._isPlayerInRange = true;
				this.ShowPrompt();
			}
			else if (!flag && this._isPlayerInRange)
			{
				this._isPlayerInRange = false;
				this._holdInput.Reset();
				if (this.promptUI != null)
				{
					this.promptUI.gameObject.SetActive(false);
				}
			}
			if (this._isPlayerInRange)
			{
				WorldPickupInputAction worldPickupInputAction = this._holdInput.Step(this.interactInput.GetButtonDown(), this.interactInput.GetButtonUp(), Time.deltaTime, this.holdToSwapDuration, this.CanQuickSwap, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
				if (worldPickupInputAction == WorldPickupInputAction.Tap)
				{
					this.OnInteract(this.playerRoot);
					return;
				}
				if (worldPickupInputAction == WorldPickupInputAction.Hold)
				{
					this.BeginPickup(this._holdInput.CapturedSlot, this._holdInput.CapturedItem);
				}
			}
		}

		// Token: 0x06008AB0 RID: 35504 RVA: 0x0038AA9C File Offset: 0x00388C9C
		public bool CanInteract(GameObject player)
		{
			return !this._pickupInProgress && this._inventoryManager != null && this.itemData != null && (InteractionCandidateRegistry.CanStartWorldInteraction || this.CanPrepareInteraction);
		}

		// Token: 0x06008AB1 RID: 35505 RVA: 0x0038AAD3 File Offset: 0x00388CD3
		public void OnInteract(GameObject player)
		{
			this.BeginPickup(-1, null);
		}

		// Token: 0x06008AB2 RID: 35506 RVA: 0x0038AAE0 File Offset: 0x00388CE0
		private void BeginPickup(int swapSlot, InventoryItemSO expectedItem)
		{
			if (!base.isActiveAndEnabled || !this.CanInteract(this.playerRoot) || this.CandidateSqrDistance > this.detectionRadius * this.detectionRadius || !InteractionCandidateRegistry.IsWinner(this) || (swapSlot >= 0 && !this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, swapSlot, expectedItem)))
			{
				return;
			}
			if (swapSlot >= 0 && !this._inventoryManager.TryPrepareWorldPickupSwap(this.itemData, swapSlot, expectedItem))
			{
				return;
			}
			if (!InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this._isPlayerInRange = false;
			this._holdInput.Reset();
			this._pickupInProgress = true;
			this._inventoryManager.TryPickupWithAnimation(this.itemData, base.transform, delegate
			{
				bool flag = this.CompletePickup(swapSlot, expectedItem);
				if (!flag)
				{
					this._pickupInProgress = false;
				}
				return flag;
			});
		}

		// Token: 0x06008AB3 RID: 35507 RVA: 0x0038ABF0 File Offset: 0x00388DF0
		private bool CompletePickup(int swapSlot, InventoryItemSO expectedItem)
		{
			if (this._inventoryManager == null || this.itemData == null)
			{
				return false;
			}
			if (!((swapSlot >= 0) ? this._inventoryManager.TrySwapWorldPickupIntoSelectedSlot(this.itemData, this.itemData.rarity, -1, 1f, true, swapSlot, expectedItem, true) : this._inventoryManager.AddItem(this.itemData)))
			{
				return false;
			}
			if (this.pickupSFX != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.pickupSFX);
			}
			else if (this.itemData.pickupSFX != null)
			{
				AudioSource.PlayClipAtPoint(this.itemData.pickupSFX, base.transform.position);
			}
			if (this.debugLogs)
			{
				Debug.Log("[HideFlashPickup] Picked up: " + this.itemData.itemName, this);
			}
			PoiResidencyPlacement.WriteFlag(this, "pickup-consumed", true);
			StreamedWorldPropView component = base.GetComponent<StreamedWorldPropView>();
			PoiResidencyStateKey poiResidencyStateKey;
			if (component != null && component.Owner != null && PoiResidencyPlacement.TryGetKey(component.Owner, "slot-consumed", out poiResidencyStateKey))
			{
				PoiResidencyLedger.WriteFlag(poiResidencyStateKey, true);
			}
			Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x06008AB4 RID: 35508 RVA: 0x0038AD30 File Offset: 0x00388F30
		private bool RefreshSwapHint()
		{
			if (this.interactionConfig == null)
			{
				return false;
			}
			if (this._runtimeConfig == null)
			{
				this._runtimeConfig = Object.Instantiate<InteractionConfigSO>(this.interactionConfig);
				this.interactionConfig = this._runtimeConfig;
			}
			string mainActionName = this._runtimeConfig.mainActionName;
			string text = (string.IsNullOrEmpty(mainActionName) ? "Pick Up" : mainActionName);
			text = text.Replace("  (Hold: Swap)", "");
			this._runtimeConfig.mainActionName = text + (this.CanQuickSwap ? "  (Hold: Swap)" : "");
			return mainActionName != this._runtimeConfig.mainActionName;
		}

		// Token: 0x06008AB5 RID: 35509 RVA: 0x0038ADDC File Offset: 0x00388FDC
		private void OnDestroy()
		{
			if (this._runtimeConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._runtimeConfig);
				return;
			}
			Object.DestroyImmediate(this._runtimeConfig);
		}

		// Token: 0x06008AB6 RID: 35510 RVA: 0x0038AE0B File Offset: 0x0038900B
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x06008AB7 RID: 35511 RVA: 0x0038AE14 File Offset: 0x00389014
		private bool ResolvePlayerReferences()
		{
			if (this._inventoryManager == null && this.playerRoot != null)
			{
				this._inventoryManager = this.playerRoot.GetComponentInChildren<PlayerInventoryManager>(true);
			}
			if (this._inventoryManager == null)
			{
				this._inventoryManager = PlayerInventoryManager.Instance;
			}
			if (this.playerRoot == null && this._inventoryManager != null)
			{
				vThirdPersonController componentInParent = this._inventoryManager.GetComponentInParent<vThirdPersonController>(true);
				this.playerRoot = ((componentInParent != null) ? componentInParent.gameObject : this._inventoryManager.gameObject);
			}
			this._playerTransform = ((this.playerRoot != null) ? this.playerRoot.transform : null);
			return this._playerTransform != null && this._inventoryManager != null;
		}

		// Token: 0x06008AB8 RID: 35512 RVA: 0x0038AEF4 File Offset: 0x003890F4
		private void ShowPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			this.RefreshSwapHint();
			if (this.promptUI.gameObject != base.gameObject && !this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			this.promptUI.ShowForHandoff(this.interactionConfig, this);
			this._promptInitialized = true;
		}

		// Token: 0x04008D6C RID: 36204
		[Header("Interaction Config")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04008D6D RID: 36205
		[Header("Input")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04008D6E RID: 36206
		[SerializeField]
		[Min(0.05f)]
		private float holdToSwapDuration = 0.4f;

		// Token: 0x04008D6F RID: 36207
		[Header("Inventory")]
		[SerializeField]
		private HideFlashItemSO itemData;

		// Token: 0x04008D70 RID: 36208
		[Header("Detection Settings")]
		[Tooltip("Pickup range from the item. Kept TIGHT on purpose (same as clothes): the player must stand right next to the item to see the prompt / pick it up.")]
		[SerializeField]
		private float detectionRadius = 1.1f;

		// Token: 0x04008D71 RID: 36209
		[Header("UI Reference")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04008D72 RID: 36210
		[Header("Audio")]
		[SerializeField]
		private AudioClip pickupSFX;

		// Token: 0x04008D73 RID: 36211
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04008D74 RID: 36212
		[Header("References — assign in Inspector")]
		[SerializeField]
		private GameObject playerRoot;

		// Token: 0x04008D75 RID: 36213
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008D76 RID: 36214
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x04008D77 RID: 36215
		private Transform _playerTransform;

		// Token: 0x04008D78 RID: 36216
		private bool _isPlayerInRange;

		// Token: 0x04008D79 RID: 36217
		private bool _inOwnRange;

		// Token: 0x04008D7A RID: 36218
		private bool _promptInitialized;

		// Token: 0x04008D7B RID: 36219
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04008D7C RID: 36220
		private int _nextReferenceResolveFrame;

		// Token: 0x04008D7D RID: 36221
		private readonly WorldPickupHoldInput _holdInput = new WorldPickupHoldInput();

		// Token: 0x04008D7E RID: 36222
		private bool _pickupInProgress;

		// Token: 0x04008D7F RID: 36223
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x04008D80 RID: 36224
		private const int ReferenceResolveRetryFrames = 30;
	}
}
