using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory.Actions;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x02001144 RID: 4420
	public class PlacedDildoInteractable : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionInputPriority
	{
		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06008627 RID: 34343 RVA: 0x0036BCAE File Offset: 0x00369EAE
		public InteractionConfigSO Config
		{
			get
			{
				return this._runtimeConfig;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06008628 RID: 34344 RVA: 0x0036BCB6 File Offset: 0x00369EB6
		public bool IsBeingUsed
		{
			get
			{
				return this._isBeingUsed;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06008629 RID: 34345 RVA: 0x0036BCBE File Offset: 0x00369EBE
		public InventoryItemSO ItemData
		{
			get
			{
				return this.itemData;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x0600862A RID: 34346 RVA: 0x0036BCC8 File Offset: 0x00369EC8
		public float CandidateSqrDistance
		{
			get
			{
				if (this._isActionHUDOpen)
				{
					return 0f;
				}
				if (!(this._playerTransform != null))
				{
					return float.MaxValue;
				}
				return (this.GetInteractionWorldCenter() - this._playerTransform.position).sqrMagnitude;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x0600862B RID: 34347 RVA: 0x0036BD15 File Offset: 0x00369F15
		public bool IsCandidateEligible
		{
			get
			{
				return (this.IsPlayerInRange() || this._isActionHUDOpen) && !this._isBeingUsed && this._playerTransform != null && this.CanInteract(this._playerTransform.gameObject);
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x0600862C RID: 34348 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x0600862D RID: 34349 RVA: 0x0036BD50 File Offset: 0x00369F50
		public bool CanPrioritizePickup
		{
			get
			{
				return this.itemData != null && this.itemData.canBePickedUpAfterPlacement && this._inventoryManager != null && this._inventoryManager.CanAcceptWorldPickup(this.itemData, null, -1, true);
			}
		}

		// Token: 0x0600862E RID: 34350 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600862F RID: 34351 RVA: 0x0036BDA4 File Offset: 0x00369FA4
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.CloseAllUI();
		}

		// Token: 0x06008630 RID: 34352 RVA: 0x0036BDB4 File Offset: 0x00369FB4
		public bool ReservesInventoryBinding(string binding)
		{
			if (!base.isActiveAndEnabled || !this._isActionHUDOpen || this._isBeingUsed || this._playerTransform == null || !this.IsPlayerInRange() || !this.CanInteract(this._playerTransform.gameObject))
			{
				return false;
			}
			for (int i = 0; i < this.actionInputs.Length; i++)
			{
				if (this.IsPoseActionIndex(i) && string.Equals(binding, InteractionCandidateRegistry.ActiveBinding(this.actionInputs[i]), StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008631 RID: 34353 RVA: 0x0036BE3C File Offset: 0x0036A03C
		private bool IsPoseActionIndex(int index)
		{
			return this._runtimeConfig != null && index >= 0 && index < this._runtimeConfig.usableActions.Count && this._runtimeConfig.usableActions[index] != null && this._runtimeConfig.usableActions[index].actionName != "Pick Up";
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06008632 RID: 34354 RVA: 0x0036BEA3 File Offset: 0x0036A0A3
		// (set) Token: 0x06008633 RID: 34355 RVA: 0x0036BEAB File Offset: 0x0036A0AB
		public PlacementSurfaceType PlacementType
		{
			get
			{
				return this.placementType;
			}
			set
			{
				this.placementType = value;
				this.RebuildRuntimeConfig();
			}
		}

		// Token: 0x06008634 RID: 34356 RVA: 0x0036BEBC File Offset: 0x0036A0BC
		public InventoryItemSO DetachItemDataForExternalPickup()
		{
			if (this._isBeingUsed || this.itemData == null)
			{
				return null;
			}
			this._isBeingUsed = true;
			this.CloseAllUI();
			InventoryItemSO inventoryItemSO = this.itemData;
			this.itemData = null;
			this.NotifyPersistentPickup();
			base.enabled = false;
			return inventoryItemSO;
		}

		// Token: 0x06008635 RID: 34357 RVA: 0x0036BF08 File Offset: 0x0036A108
		private void Awake()
		{
			string[] array = new string[] { "DPadLeft", "DPadUp", "DPadRight", "DPadDown", "RB" };
			int num = 0;
			while (num < this.actionInputs.Length && num < array.Length)
			{
				ControllerContextBindings.Set(this.actionInputs[num], array[num]);
				num++;
			}
			this.FindPromptUI();
		}

		// Token: 0x06008636 RID: 34358 RVA: 0x0036BF74 File Offset: 0x0036A174
		private void FindPromptUI()
		{
			if (this.promptUI != null)
			{
				return;
			}
			InteractablePromptUI[] componentsInChildren = base.GetComponentsInChildren<InteractablePromptUI>(true);
			if (componentsInChildren.Length != 0)
			{
				this.promptUI = componentsInChildren[0];
				if (this.debugLogs)
				{
					Debug.Log("[PlacedDildoInteractable] Found promptUI in child: " + this.promptUI.name);
				}
			}
		}

		// Token: 0x06008637 RID: 34359 RVA: 0x0036BFC8 File Offset: 0x0036A1C8
		private void Start()
		{
			this.CachePlayerReference();
			this.AutoConfigureLOSLayers();
			this.RebuildRuntimeConfig();
			if (this.promptUI != null)
			{
				this.promptUI.Show(this._runtimeConfig);
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008638 RID: 34360 RVA: 0x0036C018 File Offset: 0x0036A218
		private void Update()
		{
			if (this._controllerLabels != ControllerInput.IsActive)
			{
				this.RefreshActionInputLabels();
				if (this._isActionHUDOpen)
				{
					InteractionHUDController instance = InteractionHUDController.Instance;
					if (instance != null)
					{
						instance.Show(this._runtimeConfig);
					}
				}
			}
			if (this._playerTransform == null)
			{
				this.CachePlayerReference();
				if (this._playerTransform == null)
				{
					return;
				}
			}
			if (this._isBeingUsed)
			{
				return;
			}
			this.CheckPlayerProximity();
		}

		// Token: 0x06008639 RID: 34361 RVA: 0x0036C088 File Offset: 0x0036A288
		private void CheckPlayerProximity()
		{
			bool isPlayerInRange = this._isPlayerInRange;
			this._isPlayerInRange = this.IsPlayerInRange();
			this._sqrDistToPlayer = ((this._playerTransform != null) ? (this.GetInteractionWorldCenter() - this._playerTransform.position).sqrMagnitude : float.MaxValue);
			bool flag = InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			if (this.promptUI == null)
			{
				this.FindPromptUI();
			}
			if (this._isPlayerInRange && flag)
			{
				if (!isPlayerInRange && this.debugLogs)
				{
					Debug.Log("[PlacedDildoInteractable] Player entered range.");
				}
				if (this._runtimeConfig == null)
				{
					this.RebuildRuntimeConfig();
				}
				if (this.CanInteract(this._playerTransform.gameObject))
				{
					if (!this._isActionHUDOpen)
					{
						this.ShowWorldPrompt();
					}
				}
				else if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
				{
					this.promptUI.gameObject.SetActive(false);
				}
			}
			else if ((isPlayerInRange || (!flag && this.promptUI != null && this.promptUI.gameObject.activeSelf)) && (!this._isActionHUDOpen || !this._isPlayerInRange))
			{
				this.CloseAllUI();
			}
			if (!this._isPlayerInRange || !flag)
			{
				return;
			}
			if (this.interactInput.GetButtonDown() && !this._isActionHUDOpen)
			{
				if (this.CanInteract(this._playerTransform.gameObject))
				{
					this.OnInteract(this._playerTransform.gameObject);
				}
				return;
			}
			if (this._inventoryManager != null && this._inventoryManager.pickupInput.GetButtonDown() && this.itemData != null && this.itemData.canBePickedUpAfterPlacement && this.CanInteract(this._playerTransform.gameObject))
			{
				this.PickUpToInventory(this._playerTransform.gameObject);
				return;
			}
			if (this._isActionHUDOpen)
			{
				if (this.interactInput.GetButtonDown())
				{
					this.CloseActionHUD();
					return;
				}
				for (int i = 0; i < this.actionInputs.Length; i++)
				{
					if (this.IsPoseActionIndex(i) && this.actionInputs[i].GetButtonDown())
					{
						InteractionCandidateRegistry.MarkContextInputConsumed(this.actionInputs[i]);
						this.TriggerActionByIndex(i);
						return;
					}
				}
			}
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x0036C2D0 File Offset: 0x0036A4D0
		private bool IsPlayerInRange()
		{
			if (this._playerTransform == null)
			{
				return false;
			}
			Vector3 vector = this.GetInteractionWorldCenter() - this._playerTransform.position;
			float num = vector.x * vector.x + vector.z * vector.z;
			float num2 = Mathf.Abs(vector.y);
			float num3 = ((this.itemData != null && !this.itemData.supportsPlacedActions) ? Mathf.Max(5f, this.maxVerticalDistance) : this.maxVerticalDistance);
			return num <= this.interactionDistance * this.interactionDistance && num2 <= num3;
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x0036C378 File Offset: 0x0036A578
		public bool CanInteract(GameObject player)
		{
			if (this._isBeingUsed)
			{
				return false;
			}
			if (player == null)
			{
				return false;
			}
			if (!this.enableLOS)
			{
				return true;
			}
			Vector3 vector = player.transform.position + Vector3.up * 1.5f;
			Vector3 vector2 = this.GetInteractionWorldCenter() - vector;
			float magnitude = vector2.magnitude;
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, vector2.normalized, out raycastHit, magnitude, this.losBlockingLayers, QueryTriggerInteraction.Ignore) && raycastHit.collider != null)
			{
				Transform transform = raycastHit.collider.transform;
				bool flag = transform == player.transform || transform.IsChildOf(player.transform.root);
				bool flag2 = transform == base.transform || transform.IsChildOf(base.transform);
				if (!flag && !flag2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600863C RID: 34364 RVA: 0x0036C464 File Offset: 0x0036A664
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			if (this.itemData != null && !this.itemData.supportsPlacedActions && this.itemData.canBePickedUpAfterPlacement)
			{
				this.PickUpToInventory(player);
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this.RefreshActionInputLabels();
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.Show(this._runtimeConfig);
			}
			this._isActionHUDOpen = true;
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x0036C4F4 File Offset: 0x0036A6F4
		public void OnActionExecuted(string actionName, GameObject player)
		{
			DildoActionDefinition dildoActionDefinition = this.FindActionById(actionName);
			if (dildoActionDefinition == null)
			{
				return;
			}
			if (this.actionExecutor == null || this.actionExecutor.IsExecuting)
			{
				return;
			}
			this._isBeingUsed = true;
			this.CloseAllUI();
			this.actionExecutor.Execute(dildoActionDefinition, base.transform);
			if (this._waitRoutine != null)
			{
				base.StopCoroutine(this._waitRoutine);
			}
			this._waitRoutine = base.StartCoroutine(this.WaitForExecutorFinish());
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x0036C574 File Offset: 0x0036A774
		private void PickUpToInventory(GameObject player)
		{
			if (this._inventoryManager == null || this.itemData == null)
			{
				return;
			}
			if (this._inventoryManager.AddItem(this.itemData))
			{
				InteractionCandidateRegistry.MarkInteract();
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance != null)
				{
					instance.Hide();
				}
				if (this.keepInWorldOnPickup)
				{
					RunModeService.DisqualifyLeaderboardRun("An item was duplicated with an infinite pickup debug setting.");
					if (this.promptUI != null)
					{
						this.promptUI.gameObject.SetActive(false);
					}
					return;
				}
				this.NotifyPersistentPickup();
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0600863F RID: 34367 RVA: 0x0036C60C File Offset: 0x0036A80C
		private void NotifyPersistentPickup()
		{
			IPlacedItemPersistence[] componentsInParent = base.GetComponentsInParent<IPlacedItemPersistence>(true);
			for (int i = 0; i < componentsInParent.Length; i++)
			{
				componentsInParent[i].OnPlacedItemPickedUp();
			}
		}

		// Token: 0x06008640 RID: 34368 RVA: 0x0036C638 File Offset: 0x0036A838
		public void Initialize(IEnumerable<DildoActionDefinition> actions, PlacementSurfaceType surface, InteractionConfigSO configTemplate, DildoActionExecutor executor, InventoryItemSO data = null, InteractablePromptUI prompt = null, Transform player = null)
		{
			this.allActions.Clear();
			if (actions != null)
			{
				foreach (DildoActionDefinition dildoActionDefinition in actions)
				{
					if (dildoActionDefinition != null)
					{
						this.allActions.Add(dildoActionDefinition);
					}
				}
			}
			this.placementType = surface;
			this.interactionConfigTemplate = configTemplate;
			if (executor != null)
			{
				this.actionExecutor = executor;
			}
			this.itemData = data;
			if (prompt != null)
			{
				this.promptUI = prompt;
			}
			if (player != null)
			{
				this._playerTransform = player;
				this._inventoryManager = player.GetComponentInChildren<PlayerInventoryManager>();
				if (this._inventoryManager == null)
				{
					this._inventoryManager = player.GetComponentInParent<PlayerInventoryManager>();
				}
				if (this._inventoryManager == null)
				{
					this._inventoryManager = Object.FindObjectOfType<PlayerInventoryManager>();
				}
			}
			if (this.promptUI == null)
			{
				this.EnsureRuntimePrompt();
			}
			this.RebuildRuntimeConfig();
			if (this.promptUI != null)
			{
				this.promptUI.Show(this._runtimeConfig);
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008641 RID: 34369 RVA: 0x0036C774 File Offset: 0x0036A974
		private Vector3 GetInteractionWorldCenter()
		{
			Collider componentInChildren = base.GetComponentInChildren<Collider>(true);
			if (componentInChildren != null)
			{
				return componentInChildren.bounds.center;
			}
			Renderer componentInChildren2 = base.GetComponentInChildren<Renderer>(true);
			if (!(componentInChildren2 != null))
			{
				return base.transform.position;
			}
			return componentInChildren2.bounds.center;
		}

		// Token: 0x06008642 RID: 34370 RVA: 0x0036C7CC File Offset: 0x0036A9CC
		private void EnsureRuntimePrompt()
		{
			this.FindPromptUI();
			if (this.promptUI != null)
			{
				return;
			}
			InteractablePromptUI interactablePromptUI = null;
			foreach (InteractablePromptUI interactablePromptUI2 in Object.FindObjectsOfType<InteractablePromptUI>(true))
			{
				if (!(interactablePromptUI2 == null) && !interactablePromptUI2.transform.IsChildOf(base.transform))
				{
					interactablePromptUI = interactablePromptUI2;
					if (interactablePromptUI2.name.IndexOf("InteractPrompt", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						break;
					}
				}
			}
			if (interactablePromptUI == null)
			{
				return;
			}
			Vector3 lossyScale = interactablePromptUI.transform.lossyScale;
			this.promptUI = Object.Instantiate<InteractablePromptUI>(interactablePromptUI, base.transform);
			this.promptUI.name = "PlacedItem_InteractPrompt";
			Renderer componentInChildren = base.GetComponentInChildren<Renderer>(true);
			Bounds bounds = ((componentInChildren != null) ? componentInChildren.bounds : new Bounds(base.transform.position, Vector3.one * 0.1f));
			this.promptUI.transform.position = bounds.center + Vector3.up * Mathf.Max(0.14f, bounds.extents.y + 0.1f);
			this.promptUI.transform.rotation = Quaternion.identity;
			PlacedDildoInteractable.SetWorldScale(this.promptUI.transform, lossyScale);
			this.promptUI.gameObject.SetActive(false);
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x0036C934 File Offset: 0x0036AB34
		private static void SetWorldScale(Transform target, Vector3 worldScale)
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(worldScale.x / Mathf.Max(0.0001f, Mathf.Abs(vector.x)), worldScale.y / Mathf.Max(0.0001f, Mathf.Abs(vector.y)), worldScale.z / Mathf.Max(0.0001f, Mathf.Abs(vector.z)));
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x0036C9CC File Offset: 0x0036ABCC
		private void TriggerActionByIndex(int index)
		{
			if (this._playerTransform == null)
			{
				return;
			}
			List<UsableAction> list = ((this._runtimeConfig != null) ? this._runtimeConfig.usableActions : null);
			if (list == null || index < 0 || index >= list.Count)
			{
				return;
			}
			string actionName = list[index].actionName;
			string description = list[index].description;
			if (actionName == "Pick Up")
			{
				this.PickUpToInventory(this._playerTransform.gameObject);
				return;
			}
			if (!string.IsNullOrEmpty(description))
			{
				this.OnActionExecuted(description, this._playerTransform.gameObject);
				return;
			}
			this.OnActionExecuted(actionName, this._playerTransform.gameObject);
		}

		// Token: 0x06008645 RID: 34373 RVA: 0x0036CA7C File Offset: 0x0036AC7C
		private void CloseActionHUD()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._runtimeConfig);
			}
			this._isActionHUDOpen = false;
			if (this._isPlayerInRange)
			{
				this.ShowWorldPrompt();
			}
		}

		// Token: 0x06008646 RID: 34374 RVA: 0x0036CAAA File Offset: 0x0036ACAA
		private void CloseAllUI()
		{
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._runtimeConfig);
			}
			this._isActionHUDOpen = false;
		}

		// Token: 0x06008647 RID: 34375 RVA: 0x0036CAEC File Offset: 0x0036ACEC
		private void ShowWorldPrompt()
		{
			if (this.promptUI == null || this._runtimeConfig == null)
			{
				return;
			}
			if (!this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			if (InteractablePromptUI.Current != this.promptUI || !this.promptUI.IsVisible)
			{
				this.promptUI.Show(this._runtimeConfig);
			}
		}

		// Token: 0x06008648 RID: 34376 RVA: 0x0036CB6C File Offset: 0x0036AD6C
		private void CachePlayerReference()
		{
			vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
			if (vThirdPersonController != null)
			{
				this._playerTransform = vThirdPersonController.transform;
				this._inventoryManager = this._playerTransform.GetComponentInChildren<PlayerInventoryManager>();
				if (this._inventoryManager == null)
				{
					this._inventoryManager = this._playerTransform.GetComponentInParent<PlayerInventoryManager>();
				}
				if (this._inventoryManager == null)
				{
					this._inventoryManager = Object.FindObjectOfType<PlayerInventoryManager>();
				}
			}
		}

		// Token: 0x06008649 RID: 34377 RVA: 0x0036CBE0 File Offset: 0x0036ADE0
		private void AutoConfigureLOSLayers()
		{
			int num = LayerMask.NameToLayer("Player");
			int num2 = LayerMask.NameToLayer("Ignore Raycast");
			int num3 = 0;
			if (num >= 0)
			{
				num3 |= 1 << num;
			}
			if (num2 >= 0)
			{
				num3 |= 1 << num2;
			}
			this.losBlockingLayers = ~num3;
		}

		// Token: 0x0600864A RID: 34378 RVA: 0x0036CC2C File Offset: 0x0036AE2C
		private void RebuildRuntimeConfig()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
			this._runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			if (this.interactionConfigTemplate != null)
			{
				this._runtimeConfig.objectName = this.interactionConfigTemplate.objectName;
				this._runtimeConfig.mainActionName = this.interactionConfigTemplate.mainActionName;
				this._runtimeConfig.mainKeyDisplayLabel = this.interactionConfigTemplate.mainKeyDisplayLabel;
			}
			else
			{
				this._runtimeConfig.objectName = ((this.itemData != null) ? this.itemData.itemName : "Dildo");
				this._runtimeConfig.mainActionName = "Use";
				this._runtimeConfig.mainKeyDisplayLabel = this.interactInput.keyboard;
			}
			bool flag = this.itemData != null && !this.itemData.supportsPlacedActions && this.itemData.canBePickedUpAfterPlacement;
			if (flag)
			{
				this._runtimeConfig.objectName = this.itemData.itemName;
				this._runtimeConfig.mainActionName = "PICK UP";
				this._runtimeConfig.mainKeyDisplayLabel = this.interactInput.keyboard;
			}
			this._runtimeConfig.usableActions.Clear();
			List<DildoActionDefinition> compatibleActions = this.GetCompatibleActions();
			for (int i = 0; i < compatibleActions.Count; i++)
			{
				string text = ((i < this.actionInputs.Length) ? (ControllerInput.IsActive ? InteractionCandidateRegistry.ActiveBinding(this.actionInputs[i]) : this.actionInputs[i].keyboard.Replace("Alpha", "")) : (i + 1).ToString());
				this._runtimeConfig.usableActions.Add(new UsableAction
				{
					actionName = compatibleActions[i].DisplayName,
					keyDisplayLabel = text,
					description = compatibleActions[i].ActionId
				});
			}
			if (this.itemData != null && this.itemData.canBePickedUpAfterPlacement && !flag)
			{
				string text2 = ((this._inventoryManager != null) ? this._inventoryManager.PickupKey.ToString() : "F");
				this._runtimeConfig.usableActions.Add(new UsableAction
				{
					actionName = "Pick Up",
					keyDisplayLabel = text2,
					description = "Return to inventory"
				});
			}
			this.RefreshActionInputLabels();
		}

		// Token: 0x0600864B RID: 34379 RVA: 0x0036CEA4 File Offset: 0x0036B0A4
		private void RefreshActionInputLabels()
		{
			this._controllerLabels = ControllerInput.IsActive;
			if (this._runtimeConfig == null)
			{
				return;
			}
			ControllerContextBindings.MarkControllerLabels(this._runtimeConfig, this._controllerLabels);
			this._runtimeConfig.mainKeyDisplayLabel = (this._controllerLabels ? this.interactInput.buttonName : ((this.interactionConfigTemplate != null) ? this.interactionConfigTemplate.mainKeyDisplayLabel : this.interactInput.keyboard));
			int num = 0;
			while (num < this.actionInputs.Length && num < this._runtimeConfig.usableActions.Count)
			{
				if (this.IsPoseActionIndex(num) && this.actionInputs[num] != null)
				{
					this._runtimeConfig.usableActions[num].keyDisplayLabel = (this._controllerLabels ? InteractionCandidateRegistry.ActiveBinding(this.actionInputs[num]) : this.actionInputs[num].keyboard.Replace("Alpha", ""));
				}
				num++;
			}
			foreach (UsableAction usableAction in this._runtimeConfig.usableActions)
			{
				if (usableAction.description == "Return to inventory")
				{
					usableAction.keyDisplayLabel = (this._controllerLabels ? ((this._inventoryManager != null) ? this._inventoryManager.pickupInput.buttonName : "RT") : ((this._inventoryManager != null) ? this._inventoryManager.PickupKey : "F"));
				}
			}
		}

		// Token: 0x0600864C RID: 34380 RVA: 0x0036D050 File Offset: 0x0036B250
		private List<DildoActionDefinition> GetCompatibleActions()
		{
			List<DildoActionDefinition> list = new List<DildoActionDefinition>();
			foreach (DildoActionDefinition dildoActionDefinition in this.allActions)
			{
				if (dildoActionDefinition != null && dildoActionDefinition.IsCompatibleWith(this.placementType))
				{
					list.Add(dildoActionDefinition);
				}
			}
			return list;
		}

		// Token: 0x0600864D RID: 34381 RVA: 0x0036D0C4 File Offset: 0x0036B2C4
		private DildoActionDefinition FindActionById(string id)
		{
			foreach (DildoActionDefinition dildoActionDefinition in this.allActions)
			{
				if (dildoActionDefinition != null && dildoActionDefinition.ActionId == id)
				{
					return dildoActionDefinition;
				}
			}
			return null;
		}

		// Token: 0x0600864E RID: 34382 RVA: 0x0036D130 File Offset: 0x0036B330
		private IEnumerator WaitForExecutorFinish()
		{
			yield return new WaitUntil(() => this.actionExecutor == null || !this.actionExecutor.IsExecuting);
			this._isBeingUsed = false;
			if (this._isPlayerInRange)
			{
				this.ShowWorldPrompt();
			}
			yield break;
		}

		// Token: 0x0600864F RID: 34383 RVA: 0x0036D13F File Offset: 0x0036B33F
		private void OnDestroy()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x040088CC RID: 35020
		[Header("Interaction Config Template")]
		[SerializeField]
		private InteractionConfigSO interactionConfigTemplate;

		// Token: 0x040088CD RID: 35021
		[Header("Item Data")]
		[SerializeField]
		private InventoryItemSO itemData;

		// Token: 0x040088CE RID: 35022
		[Header("Surface")]
		[SerializeField]
		private PlacementSurfaceType placementType = PlacementSurfaceType.Floor;

		// Token: 0x040088CF RID: 35023
		[Header("Actions")]
		[SerializeField]
		private List<DildoActionDefinition> allActions = new List<DildoActionDefinition>();

		// Token: 0x040088D0 RID: 35024
		[Header("UI References")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040088D1 RID: 35025
		[Header("Player References")]
		[SerializeField]
		private DildoActionExecutor actionExecutor;

		// Token: 0x040088D2 RID: 35026
		[Header("Input")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x040088D3 RID: 35027
		[SerializeField]
		private GenericInput[] actionInputs = new GenericInput[]
		{
			new GenericInput("Alpha1", "", ""),
			new GenericInput("Alpha2", "", ""),
			new GenericInput("Alpha3", "", ""),
			new GenericInput("Alpha4", "", ""),
			new GenericInput("Alpha5", "", "")
		};

		// Token: 0x040088D4 RID: 35028
		[Header("Detection")]
		[Tooltip("Horizontal interaction range. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		[SerializeField]
		private float interactionDistance = 1.1f;

		// Token: 0x040088D5 RID: 35029
		[SerializeField]
		private float maxVerticalDistance = 2.5f;

		// Token: 0x040088D6 RID: 35030
		[Header("Line of Sight")]
		[SerializeField]
		private LayerMask losBlockingLayers = -1;

		// Token: 0x040088D7 RID: 35031
		[SerializeField]
		private bool enableLOS = true;

		// Token: 0x040088D8 RID: 35032
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040088D9 RID: 35033
		[Tooltip("DEBUG: se true, raccogliendo l'oggetto NON viene distrutto dal mondo — resta in scena e può essere raccolto di nuovo all'infinito (utile per testare la rottura del wanted poster). Lascia false per il comportamento normale.")]
		[SerializeField]
		private bool keepInWorldOnPickup;

		// Token: 0x040088DA RID: 35034
		private bool _isBeingUsed;

		// Token: 0x040088DB RID: 35035
		private bool _isPlayerInRange;

		// Token: 0x040088DC RID: 35036
		private bool _isActionHUDOpen;

		// Token: 0x040088DD RID: 35037
		private bool _controllerLabels;

		// Token: 0x040088DE RID: 35038
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x040088DF RID: 35039
		private Transform _playerTransform;

		// Token: 0x040088E0 RID: 35040
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x040088E1 RID: 35041
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x040088E2 RID: 35042
		private Coroutine _waitRoutine;
	}
}
