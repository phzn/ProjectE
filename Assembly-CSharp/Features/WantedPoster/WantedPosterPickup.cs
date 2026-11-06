using System;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B80 RID: 2944
	public class WantedPosterPickup : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionHandoffCandidate
	{
		// Token: 0x060053B9 RID: 21433 RVA: 0x00207CE1 File Offset: 0x00205EE1
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			if (field == UniversityGameplayField.WantedPosterPickup_interactionConfig)
			{
				return this.interactionConfig;
			}
			if (field != UniversityGameplayField.WantedPosterPickup_itemData)
			{
				throw new InvalidOperationException("Unsupported WantedPosterPickup gameplay binding: " + field.ToString());
			}
			return this.itemData;
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x00207D1C File Offset: 0x00205F1C
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			if (field == UniversityGameplayField.WantedPosterPickup_interactionConfig)
			{
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			}
			if (field != UniversityGameplayField.WantedPosterPickup_itemData)
			{
				throw new InvalidOperationException("Unsupported WantedPosterPickup gameplay binding: " + field.ToString());
			}
			this.itemData = (InventoryItemSO)asset;
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x00207D6B File Offset: 0x00205F6B
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x060053BC RID: 21436 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x060053BD RID: 21437 RVA: 0x00207D73 File Offset: 0x00205F73
		public bool HasActiveResidencyInteraction
		{
			get
			{
				return this._picking;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x060053BE RID: 21438 RVA: 0x00207D7B File Offset: 0x00205F7B
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._playerGameObject != null))
				{
					return float.MaxValue;
				}
				return this.GetPickupSqrDistance(this._playerGameObject.transform.position);
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x060053BF RID: 21439 RVA: 0x00207DA7 File Offset: 0x00205FA7
		public bool IsCandidateEligible
		{
			get
			{
				return this.CandidateSqrDistance <= this.detectionRadius * this.detectionRadius && !this._picking && this.CanInteract(this._playerGameObject);
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x00207DD4 File Offset: 0x00205FD4
		public bool CanPrioritizePickup
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanAcceptWorldPickup(this.itemData, null, -1, true);
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x00207E10 File Offset: 0x00206010
		public bool CanPrepareInteraction
		{
			get
			{
				return this.itemData != null && this.itemData.category == InventoryCategory.Item && this._inventoryManager != null && this._inventoryManager.CanPrepareWorldPickupSwap(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x060053C3 RID: 21443 RVA: 0x00207E6F File Offset: 0x0020606F
		private bool CanQuickSwap
		{
			get
			{
				return this._inventoryManager != null && this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x00207EA8 File Offset: 0x002060A8
		private void OnEnable()
		{
			this._picking = false;
			this._isPlayerInRange = false;
			this._inOwnRange = false;
			this._sqrDistToPlayer = float.MaxValue;
			this._holdInput.Reset();
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			this.ResolvePlayerDependencies();
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x00207F05 File Offset: 0x00206105
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._isPlayerInRange = false;
			this._holdInput.Reset();
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x00207F38 File Offset: 0x00206138
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			this._posterRenderer = base.GetComponent<Renderer>();
			if (this._posterRenderer == null)
			{
				WantedPoster componentInChildren = base.GetComponentInChildren<WantedPoster>(true);
				if (componentInChildren != null)
				{
					this._posterRenderer = componentInChildren.Renderer;
				}
			}
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x00207F98 File Offset: 0x00206198
		private void Start()
		{
			PlacedDildoInteractable component = base.GetComponent<PlacedDildoInteractable>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			this.ResolvePlayerDependencies();
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x00207FC4 File Offset: 0x002061C4
		private void Update()
		{
			if (this._playerGameObject == null || this._inventoryManager == null)
			{
				this.ResolvePlayerDependencies();
			}
			if (this._playerGameObject == null || this._picking)
			{
				this.UpdatePrompt(false);
				return;
			}
			this._sqrDistToPlayer = this.GetPickupSqrDistance(this._playerGameObject.transform.position);
			this._inOwnRange = this._sqrDistToPlayer <= this.detectionRadius * this.detectionRadius;
			bool flag = this._inOwnRange && this.CanInteract(this._playerGameObject) && InteractionCandidateRegistry.IsWinner(this) && (PlayerActivityGate.Allows(PlayerActivity.Interact) || this.CanPrepareInteraction);
			this.UpdatePrompt(flag);
			if (this._isPlayerInRange)
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

		// Token: 0x060053C9 RID: 21449 RVA: 0x00208114 File Offset: 0x00206314
		private float GetPickupSqrDistance(Vector3 playerFeet)
		{
			Vector3 vector = playerFeet + Vector3.up * this.interactionHeight;
			return (((this._posterRenderer != null) ? this._posterRenderer.bounds.ClosestPoint(vector) : base.transform.position) - vector).sqrMagnitude;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x00208178 File Offset: 0x00206378
		private void UpdatePrompt(bool visible)
		{
			this._isPlayerInRange = visible;
			if (!visible)
			{
				this._holdInput.Reset();
			}
			if (this.promptUI == null)
			{
				return;
			}
			if (!visible)
			{
				if (this.promptUI.IsVisible)
				{
					this.promptUI.Hide();
				}
				return;
			}
			bool flag = this.RefreshSwapHint();
			if (!this.promptUI.IsVisible || InteractablePromptUI.Current != this.promptUI || flag)
			{
				this.promptUI.gameObject.SetActive(true);
				this.promptUI.ShowForHandoff(this.interactionConfig, this);
			}
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x00208214 File Offset: 0x00206414
		public bool CanInteract(GameObject player)
		{
			if (this._inventoryManager == null || !GameSystemsBootstrapper.IsRunReady(false))
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			return instance == null || !instance.IsTransitioning;
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x00208254 File Offset: 0x00206454
		private void ResolvePlayerDependencies()
		{
			GameObject playerObject = PlayerRegistry.PlayerObject;
			if (playerObject != null)
			{
				this._playerGameObject = playerObject;
			}
			if (this._playerGameObject == null)
			{
				vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
				if (vThirdPersonController != null)
				{
					this._playerGameObject = vThirdPersonController.gameObject;
				}
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance != null)
			{
				this._inventoryManager = instance;
				return;
			}
			if (this._playerGameObject != null)
			{
				this._inventoryManager = this._playerGameObject.GetComponentInChildren<PlayerInventoryManager>(true);
			}
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x002082D7 File Offset: 0x002064D7
		public void OnInteract(GameObject player)
		{
			this.BeginPickup(player, -1, null);
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x002082E4 File Offset: 0x002064E4
		private void BeginPickup(GameObject player, int swapSlot, InventoryItemSO expectedItem)
		{
			if (!this.CanInteract(player) || this._picking || (swapSlot >= 0 && !this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, swapSlot, expectedItem)) || (swapSlot >= 0 && !this._inventoryManager.TryPrepareWorldPickupSwap(this.itemData, swapSlot, expectedItem)) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			this._picking = true;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			this._isPlayerInRange = false;
			this._holdInput.Reset();
			this._originalScale = base.transform.localScale;
			this._inventoryManager.TryPickupWithAnimation(this.itemData, base.transform, delegate
			{
				bool flag = this.CompletePickupCore(swapSlot, expectedItem);
				if (!flag)
				{
					this._picking = false;
				}
				return flag;
			});
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x002083DC File Offset: 0x002065DC
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
			if (this.keepInWorldOnPickup)
			{
				RunModeService.DisqualifyLeaderboardRun("An item was duplicated with an infinite pickup debug setting.");
			}
			if (this.pickupSFX != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.pickupSFX);
			}
			if (this.keepInWorldOnPickup)
			{
				this.RestoreAfterDebugPickup();
				return true;
			}
			base.transform.localScale = this._originalScale;
			WantedPoster componentInChildren = base.GetComponentInChildren<WantedPoster>(true);
			if (componentInChildren != null && componentInChildren.ConsumeAuthoredWorldInstance())
			{
				PoiResidencyStateKey poiResidencyStateKey;
				if (PoiResidencyPlacement.TryGetKey(this, "poster-pickup", out poiResidencyStateKey))
				{
					this._picking = false;
				}
				return true;
			}
			Object.Destroy(base.gameObject);
			return true;
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x002084E5 File Offset: 0x002066E5
		private void RestoreAfterDebugPickup()
		{
			base.transform.localScale = this._originalScale;
			this._picking = false;
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x00208500 File Offset: 0x00206700
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

		// Token: 0x060053D2 RID: 21458 RVA: 0x002085AC File Offset: 0x002067AC
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

		// Token: 0x060053D3 RID: 21459 RVA: 0x002085DB File Offset: 0x002067DB
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x04005429 RID: 21545
		[Header("Interaction Config")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x0400542A RID: 21546
		[Header("Inventory")]
		[SerializeField]
		private InventoryItemSO itemData;

		// Token: 0x0400542B RID: 21547
		[Header("Audio")]
		[SerializeField]
		private AudioClip pickupSFX;

		// Token: 0x0400542C RID: 21548
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400542D RID: 21549
		[Header("Detection")]
		[Tooltip("Pickup range. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		[SerializeField]
		private float detectionRadius = 1.1f;

		// Token: 0x0400542E RID: 21550
		[Tooltip("Height above the player's feet used to reach a wall-mounted poster.")]
		[Min(0f)]
		[SerializeField]
		private float interactionHeight = 1f;

		// Token: 0x0400542F RID: 21551
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04005430 RID: 21552
		[SerializeField]
		[Min(0.05f)]
		private float holdToSwapDuration = 0.4f;

		// Token: 0x04005431 RID: 21553
		[Header("UI")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04005432 RID: 21554
		[Header("Debug")]
		[Tooltip("Se true, alla raccolta NON distrugge il poster a muro (per debug, riprendibile all'infinito).")]
		[SerializeField]
		private bool keepInWorldOnPickup;

		// Token: 0x04005433 RID: 21555
		private GameObject _playerGameObject;

		// Token: 0x04005434 RID: 21556
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x04005435 RID: 21557
		private bool _isPlayerInRange;

		// Token: 0x04005436 RID: 21558
		private bool _inOwnRange;

		// Token: 0x04005437 RID: 21559
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04005438 RID: 21560
		private bool _picking;

		// Token: 0x04005439 RID: 21561
		private Vector3 _originalScale = Vector3.one;

		// Token: 0x0400543A RID: 21562
		private Renderer _posterRenderer;

		// Token: 0x0400543B RID: 21563
		private readonly WorldPickupHoldInput _holdInput = new WorldPickupHoldInput();

		// Token: 0x0400543C RID: 21564
		private InteractionConfigSO _runtimeConfig;
	}
}
