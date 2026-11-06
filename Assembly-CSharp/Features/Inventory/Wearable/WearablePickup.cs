using System;
using Features.Missions;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200112A RID: 4394
	public class WearablePickup : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionHandoffCandidate
	{
		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06008504 RID: 34052 RVA: 0x00363B79 File Offset: 0x00361D79
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x06008505 RID: 34053 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06008506 RID: 34054 RVA: 0x00363B84 File Offset: 0x00361D84
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._playerGameObject != null))
				{
					return float.MaxValue;
				}
				return (base.transform.position - this._playerGameObject.transform.position).sqrMagnitude;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x06008507 RID: 34055 RVA: 0x00363BCD File Offset: 0x00361DCD
		public bool IsCandidateEligible
		{
			get
			{
				return this.CandidateSqrDistance <= this.detectionRadius * this.detectionRadius && this.CanInteract(this._playerGameObject);
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x06008508 RID: 34056 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x06008509 RID: 34057 RVA: 0x00363BF4 File Offset: 0x00361DF4
		public bool CanPrioritizePickup
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanAcceptWorldPickup(this.itemData, null, -1, true);
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x0600850A RID: 34058 RVA: 0x00363C30 File Offset: 0x00361E30
		public bool CanPrepareInteraction
		{
			get
			{
				return this.itemData != null && this.itemData.category == InventoryCategory.Item && this._inventoryManager != null && this._inventoryManager.CanPrepareWorldPickupSwap(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x0600850B RID: 34059 RVA: 0x00363C90 File Offset: 0x00361E90
		private bool CanQuickSwap
		{
			get
			{
				return this.itemData != null && this.itemData.category == InventoryCategory.Item && this._inventoryManager != null && this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x0600850C RID: 34060 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600850D RID: 34061 RVA: 0x00363CF0 File Offset: 0x00361EF0
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

		// Token: 0x0600850E RID: 34062 RVA: 0x00363D5E File Offset: 0x00361F5E
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
		}

		// Token: 0x0600850F RID: 34063 RVA: 0x00363D7B File Offset: 0x00361F7B
		private void Start()
		{
			this.ResolvePlayerReferences();
			if (this.promptUI != null && this.promptUI.gameObject != base.gameObject)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008510 RID: 34064 RVA: 0x00363DBC File Offset: 0x00361FBC
		private void Update()
		{
			if (this._playerGameObject == null || this._inventoryManager == null)
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
			this._sqrDistToPlayer = (base.transform.position - this._playerGameObject.transform.position).sqrMagnitude;
			this._inOwnRange = this._sqrDistToPlayer <= this.detectionRadius * this.detectionRadius;
			bool flag = this._inOwnRange && this.CanInteract(this._playerGameObject) && InteractionCandidateRegistry.IsWinner(this) && (PlayerActivityGate.Allows(PlayerActivity.Interact) || this.CanPrepareInteraction);
			bool flag2 = flag && this.promptUI != null && this.interactionConfig != null && (InteractablePromptUI.Current != this.promptUI || !this.promptUI.IsVisible);
			bool flag3 = flag && this.RefreshSwapHint();
			if (flag && (!this._isPlayerInRange || flag2 || flag3))
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
				if (this.itemData.category == InventoryCategory.Clothing)
				{
					if (this.interactInput.GetButtonDown())
					{
						this.OnInteract(this._playerGameObject);
						return;
					}
				}
				else
				{
					WorldPickupInputAction worldPickupInputAction = this._holdInput.Step(this.interactInput.GetButtonDown(), this.interactInput.GetButtonUp(), Time.deltaTime, this.holdToSwapDuration, this.CanQuickSwap, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
					if (worldPickupInputAction == WorldPickupInputAction.Tap)
					{
						this.OnInteract(this._playerGameObject);
						return;
					}
					if (worldPickupInputAction == WorldPickupInputAction.Hold)
					{
						this.BeginPickup(this._playerGameObject, this._holdInput.CapturedSlot, this._holdInput.CapturedItem);
					}
				}
			}
		}

		// Token: 0x06008511 RID: 34065 RVA: 0x00363FFD File Offset: 0x003621FD
		public bool CanInteract(GameObject player)
		{
			return !this._pickupInProgress && this._inventoryManager != null && this.itemData != null && LongWayHomeLootPolicy.CanPickUp(this.itemData, this, false);
		}

		// Token: 0x06008512 RID: 34066 RVA: 0x00364032 File Offset: 0x00362232
		public void OnInteract(GameObject player)
		{
			this.BeginPickup(player, -1, null);
		}

		// Token: 0x06008513 RID: 34067 RVA: 0x00364040 File Offset: 0x00362240
		private void BeginPickup(GameObject player, int swapSlot, InventoryItemSO expectedItem)
		{
			if (!this.CanInteract(player) || (swapSlot >= 0 && !this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, swapSlot, expectedItem)))
			{
				return;
			}
			if (this.itemData.category == InventoryCategory.Clothing && !this._inventoryManager.HasFreeClothingSlot)
			{
				ToastService.Show("No free clothing slots!", "Wardrobe", null, -1f, "inventory_feedback");
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
				bool flag = this.CompletePickupCore(swapSlot, expectedItem);
				if (!flag)
				{
					this._pickupInProgress = false;
				}
				return flag;
			});
		}

		// Token: 0x06008514 RID: 34068 RVA: 0x0036415C File Offset: 0x0036235C
		private bool CompletePickup()
		{
			return this.CompletePickupCore(-1, null);
		}

		// Token: 0x06008515 RID: 34069 RVA: 0x00364168 File Offset: 0x00362368
		private bool CompletePickupCore(int swapSlot, InventoryItemSO expectedItem)
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
			MissionGameplaySignals.NotifyItemCollected(this.itemData, (this.itemData != null) ? this.itemData.rarity : ItemRarity.Common);
			Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x06008516 RID: 34070 RVA: 0x00364234 File Offset: 0x00362434
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x06008517 RID: 34071 RVA: 0x00364240 File Offset: 0x00362440
		private bool ResolvePlayerReferences()
		{
			if (this._playerGameObject == null)
			{
				this._playerGameObject = PlayerRegistry.PlayerObject;
			}
			if (this._inventoryManager == null && this._playerGameObject != null)
			{
				this._inventoryManager = this._playerGameObject.GetComponentInChildren<PlayerInventoryManager>(true);
			}
			if (this._inventoryManager == null)
			{
				this._inventoryManager = PlayerInventoryManager.Instance;
			}
			if (this._playerGameObject == null && this._inventoryManager != null)
			{
				vThirdPersonController componentInParent = this._inventoryManager.GetComponentInParent<vThirdPersonController>(true);
				this._playerGameObject = ((componentInParent != null) ? componentInParent.gameObject : this._inventoryManager.gameObject);
			}
			return this._playerGameObject != null && this._inventoryManager != null;
		}

		// Token: 0x06008518 RID: 34072 RVA: 0x00364314 File Offset: 0x00362514
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
		}

		// Token: 0x06008519 RID: 34073 RVA: 0x00364384 File Offset: 0x00362584
		private bool RefreshSwapHint()
		{
			if (this.interactionConfig == null || this.itemData == null || this.itemData.category != InventoryCategory.Item)
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

		// Token: 0x0600851A RID: 34074 RVA: 0x0036444B File Offset: 0x0036264B
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

		// Token: 0x04008804 RID: 34820
		[Header("Interaction Config")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04008805 RID: 34821
		[Header("Input")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04008806 RID: 34822
		[SerializeField]
		[Min(0.05f)]
		private float holdToSwapDuration = 0.4f;

		// Token: 0x04008807 RID: 34823
		[Header("Inventory")]
		[SerializeField]
		private WearableItemSO itemData;

		// Token: 0x04008808 RID: 34824
		[Header("Audio")]
		[SerializeField]
		private AudioClip pickupSFX;

		// Token: 0x04008809 RID: 34825
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400880A RID: 34826
		[Header("Detection Settings")]
		[Tooltip("Pickup range from the item. Kept TIGHT on purpose: the player must stand right next to the item to see the prompt / pick it up.")]
		[SerializeField]
		private float detectionRadius = 1.1f;

		// Token: 0x0400880B RID: 34827
		[Header("UI Reference")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x0400880C RID: 34828
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400880D RID: 34829
		private GameObject _playerGameObject;

		// Token: 0x0400880E RID: 34830
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x0400880F RID: 34831
		private bool _isPlayerInRange;

		// Token: 0x04008810 RID: 34832
		private bool _inOwnRange;

		// Token: 0x04008811 RID: 34833
		private bool _pickupInProgress;

		// Token: 0x04008812 RID: 34834
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04008813 RID: 34835
		private int _nextReferenceResolveFrame;

		// Token: 0x04008814 RID: 34836
		private readonly WorldPickupHoldInput _holdInput = new WorldPickupHoldInput();

		// Token: 0x04008815 RID: 34837
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x04008816 RID: 34838
		private const int ReferenceResolveRetryFrames = 30;
	}
}
