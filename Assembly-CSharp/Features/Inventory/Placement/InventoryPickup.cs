using System;
using Features.Inventory.Wearable;
using Features.Localization;
using Features.Missions;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x0200113D RID: 4413
	public class InventoryPickup : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionHandoffCandidate
	{
		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x060085EB RID: 34283 RVA: 0x0036A76C File Offset: 0x0036896C
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x060085EC RID: 34284 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x060085ED RID: 34285 RVA: 0x0036A774 File Offset: 0x00368974
		public InventoryItemSO ItemData
		{
			get
			{
				return this.itemData;
			}
		}

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x060085EE RID: 34286 RVA: 0x0036A77C File Offset: 0x0036897C
		// (remove) Token: 0x060085EF RID: 34287 RVA: 0x0036A7B4 File Offset: 0x003689B4
		public event Action Collected;

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x060085F0 RID: 34288 RVA: 0x0036A7E9 File Offset: 0x003689E9
		public ItemRarity InstanceRarity
		{
			get
			{
				if (this.hasInstanceRarity)
				{
					return this.instanceRarity;
				}
				if (!(this.itemData != null))
				{
					return ItemRarity.Common;
				}
				return this.itemData.rarity;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x060085F1 RID: 34289 RVA: 0x0036A815 File Offset: 0x00368A15
		public int InstanceVariant
		{
			get
			{
				return this.instanceVariant;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x060085F2 RID: 34290 RVA: 0x0036A81D File Offset: 0x00368A1D
		public float InstanceDurability01
		{
			get
			{
				return this.instanceDurability01;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x060085F3 RID: 34291 RVA: 0x0036A825 File Offset: 0x00368A25
		public static float LastInteractTime
		{
			get
			{
				return InteractionCandidateRegistry.LastInteractTime;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x060085F4 RID: 34292 RVA: 0x0036A82C File Offset: 0x00368A2C
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._playerGameObject != null))
				{
					return float.MaxValue;
				}
				return this.GetInteractionSqrDistance(this._playerGameObject.transform.position);
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x060085F5 RID: 34293 RVA: 0x0036A858 File Offset: 0x00368A58
		public bool IsCandidateEligible
		{
			get
			{
				return base.isActiveAndEnabled && this.CandidateSqrDistance <= this.detectionRadius * this.detectionRadius && this.CanInteract(this._playerGameObject) && this.HasUnobstructedReach(this._playerGameObject, false);
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x060085F6 RID: 34294 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x060085F7 RID: 34295 RVA: 0x0036A894 File Offset: 0x00368A94
		public bool CanPrioritizePickup
		{
			get
			{
				return this.restoreMode || (this._inventoryManager != null && this._inventoryManager.CanAcceptWorldPickup(this.itemData, new ItemRarity?(this.InstanceRarity), this.instanceVariant, this.registerAsDiscovery));
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x060085F8 RID: 34296 RVA: 0x0036A8E4 File Offset: 0x00368AE4
		public bool CanPrepareInteraction
		{
			get
			{
				return !this.restoreMode && this._inventoryManager != null && this._inventoryManager.CanPrepareWorldPickupSwap(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x060085F9 RID: 34297 RVA: 0x0036A930 File Offset: 0x00368B30
		private bool CanQuickSwap
		{
			get
			{
				return !this.restoreMode && this._inventoryManager != null && this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			}
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x0036A97C File Offset: 0x00368B7C
		public void Configure(InventoryItemSO item, InteractionConfigSO configTemplate = null, AudioClip sfx = null, ItemRarity? rarity = null, int variantIndex = -1, float durability01 = 1f, bool registersDiscovery = true)
		{
			this.restoreMode = false;
			this._restoreCallback = null;
			this._holdInput.Reset();
			this.detectionRadius = Mathf.Max(this.detectionRadius, this.runtimeDropDetectionRadius);
			this.itemData = item;
			if (item != null && !string.IsNullOrWhiteSpace(item.itemName))
			{
				base.gameObject.name = item.itemName;
			}
			if (sfx != null)
			{
				this.pickupSFX = sfx;
			}
			if (rarity != null)
			{
				this.hasInstanceRarity = true;
				this.instanceRarity = rarity.Value;
			}
			this.instanceVariant = variantIndex;
			this.instanceDurability01 = Mathf.Clamp01(durability01);
			this.registerAsDiscovery = registersDiscovery;
			InteractionConfigSO interactionConfigSO = ((configTemplate != null) ? configTemplate : this.interactionConfig);
			if (interactionConfigSO != null && item != null)
			{
				if (this._runtimeConfig == null)
				{
					this._runtimeConfig = Object.Instantiate<InteractionConfigSO>(interactionConfigSO);
				}
				this._runtimeConfig.objectName = ((!string.IsNullOrEmpty(item.itemName)) ? item.itemName : interactionConfigSO.objectName);
				this.ApplyRarityLabelColor(this._runtimeConfig);
				this.AddQuickWearHint(this._runtimeConfig);
				this.interactionConfig = this._runtimeConfig;
			}
			else if (configTemplate != null)
			{
				this.interactionConfig = configTemplate;
			}
			if (Application.isPlaying)
			{
				LootRarityAura.EnsureOn(base.gameObject, this.itemData, this.InstanceRarity);
			}
			this.CacheInteractionGeometry();
		}

		// Token: 0x060085FB RID: 34299 RVA: 0x0036AAF4 File Offset: 0x00368CF4
		private void ApplyRarityLabelColor(InteractionConfigSO config)
		{
			if (config == null || this.itemData == null)
			{
				return;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			ItemRarity itemRarity = this.InstanceRarity;
			if (instance == null || !instance.ColorLabel(itemRarity))
			{
				return;
			}
			config.objectNameColor = instance.GetColor(itemRarity);
		}

		// Token: 0x060085FC RID: 34300 RVA: 0x0036AB46 File Offset: 0x00368D46
		public void ConfigureAsClothRestore(InventoryItemSO item, Action onRestore, InteractionConfigSO configTemplate = null, ItemRarity? rarity = null, int variantIndex = -1, float durability01 = 1f, bool registersDiscovery = true)
		{
			this.Configure(item, configTemplate, null, rarity, variantIndex, durability01, registersDiscovery);
			this.restoreMode = true;
			this._restoreCallback = onRestore;
			if (this._runtimeConfig != null)
			{
				this.AddQuickWearHint(this._runtimeConfig);
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x060085FD RID: 34301 RVA: 0x0036AB82 File Offset: 0x00368D82
		private bool IsWearableCloth
		{
			get
			{
				return !this.restoreMode && this.itemData is WearableItemSO && this.itemData.category == InventoryCategory.Clothing;
			}
		}

		// Token: 0x060085FE RID: 34302 RVA: 0x0036ABAC File Offset: 0x00368DAC
		private bool AddQuickWearHint(InteractionConfigSO config)
		{
			if (config == null)
			{
				return false;
			}
			string mainActionName = config.mainActionName;
			string text = (string.IsNullOrEmpty(mainActionName) ? "Pick Up" : mainActionName);
			text = text.Replace("  (Hold: Wear)", "").Replace("  (Hold: Swap)", "");
			config.mainActionName = text + (this.IsWearableCloth ? "  (Hold: Wear)" : (this.CanQuickSwap ? "  (Hold: Swap)" : ""));
			return config.mainActionName != mainActionName;
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x0036AC37 File Offset: 0x00368E37
		private void OnDestroy()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x0036AC52 File Offset: 0x00368E52
		private void OnEnable()
		{
			this._nextReferenceResolveFrame = 0;
			this._reachCheckFrame = -1;
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06008601 RID: 34305 RVA: 0x0036AC68 File Offset: 0x00368E68
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._isPlayerInRange = false;
			this._inOwnRange = false;
			this._sqrDistToPlayer = float.MaxValue;
			this._holdInput.Reset();
			this.HidePrompt();
		}

		// Token: 0x06008602 RID: 34306 RVA: 0x0036AC9A File Offset: 0x00368E9A
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
		}

		// Token: 0x06008603 RID: 34307 RVA: 0x0036ACB8 File Offset: 0x00368EB8
		private void Start()
		{
			this.ResolvePlayerReferences();
			if (this.interactionConfig != null && this.itemData != null)
			{
				if (this._runtimeConfig == null)
				{
					this._runtimeConfig = Object.Instantiate<InteractionConfigSO>(this.interactionConfig);
				}
				this._runtimeConfig.objectName = ((!string.IsNullOrEmpty(this.itemData.itemName)) ? this.itemData.itemName : this.interactionConfig.objectName);
				this.ApplyRarityLabelColor(this._runtimeConfig);
				this.AddQuickWearHint(this._runtimeConfig);
				this.interactionConfig = this._runtimeConfig;
			}
			if (this.itemData != null && !string.IsNullOrWhiteSpace(this.itemData.itemName))
			{
				base.gameObject.name = this.itemData.itemName;
			}
			LootRarityAura.EnsureOn(base.gameObject, this.itemData, this.InstanceRarity);
			this.CacheInteractionGeometry();
			this.HidePrompt();
		}

		// Token: 0x06008604 RID: 34308 RVA: 0x0036ADC0 File Offset: 0x00368FC0
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
			this._sqrDistToPlayer = this.GetInteractionSqrDistance(this._playerGameObject.transform.position);
			this._inOwnRange = this._sqrDistToPlayer <= this.detectionRadius * this.detectionRadius;
			this._reachCheckFrame = -1;
			bool flag = this._inOwnRange && this.CanInteract(this._playerGameObject) && InteractionCandidateRegistry.IsWinner(this);
			bool flag2 = flag && this._runtimeConfig != null && this.AddQuickWearHint(this._runtimeConfig);
			bool flag3 = flag && this.promptUI != null && this.interactionConfig != null && (flag2 || InteractablePromptUI.Current != this.promptUI || !this.promptUI.IsVisible);
			if (flag && (!this._isPlayerInRange || flag3))
			{
				this._isPlayerInRange = true;
				this.ShowPrompt();
			}
			else if (!flag && this._isPlayerInRange)
			{
				this._isPlayerInRange = false;
				this._holdInput.Reset();
				this.HidePrompt();
			}
			if (this._isPlayerInRange)
			{
				this.HandleHoldInput();
			}
		}

		// Token: 0x06008605 RID: 34309 RVA: 0x0036AF34 File Offset: 0x00369134
		private float GetInteractionSqrDistance(Vector3 playerPosition)
		{
			float num = (base.transform.position - playerPosition).sqrMagnitude;
			bool flag = false;
			if (this._interactionColliders != null)
			{
				for (int i = 0; i < this._interactionColliders.Length; i++)
				{
					Collider collider = this._interactionColliders[i];
					if (!(collider == null) && collider.enabled && collider.gameObject.activeInHierarchy)
					{
						float sqrMagnitude = (collider.bounds.ClosestPoint(playerPosition) - playerPosition).sqrMagnitude;
						if (!flag || sqrMagnitude < num)
						{
							num = sqrMagnitude;
						}
						flag = true;
					}
				}
			}
			if (!flag && this._interactionRenderers != null)
			{
				for (int j = 0; j < this._interactionRenderers.Length; j++)
				{
					Renderer renderer = this._interactionRenderers[j];
					if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
					{
						float sqrMagnitude2 = (renderer.bounds.ClosestPoint(playerPosition) - playerPosition).sqrMagnitude;
						if (!flag || sqrMagnitude2 < num)
						{
							num = sqrMagnitude2;
						}
						flag = true;
					}
				}
			}
			return num;
		}

		// Token: 0x06008606 RID: 34310 RVA: 0x0036B050 File Offset: 0x00369250
		private void CacheInteractionGeometry()
		{
			this._reachCheckFrame = -1;
			this._interactionColliders = Array.FindAll<Collider>(base.GetComponentsInChildren<Collider>(true), new Predicate<Collider>(InventoryPickup.IsInteractionGeometry));
			this._interactionRenderers = Array.FindAll<Renderer>(base.GetComponentsInChildren<Renderer>(true), (Renderer renderer) => (renderer is MeshRenderer || renderer is SkinnedMeshRenderer) && InventoryPickup.IsInteractionGeometry(renderer));
		}

		// Token: 0x06008607 RID: 34311 RVA: 0x0036B0B3 File Offset: 0x003692B3
		private static bool IsInteractionGeometry(Component component)
		{
			return component != null && component.GetComponentInParent<LootRarityAura>() == null;
		}

		// Token: 0x06008608 RID: 34312 RVA: 0x0036B0CC File Offset: 0x003692CC
		private bool HasUnobstructedReach(GameObject player, bool forceRefresh = false)
		{
			if (player == null)
			{
				return false;
			}
			if (this._interactionColliders == null || this._interactionRenderers == null)
			{
				this.CacheInteractionGeometry();
			}
			Vector3 vector = player.transform.TransformPoint(Vector3.up * this.reachOriginHeight);
			Vector3 reachTarget = this.GetReachTarget(vector);
			if (!forceRefresh && this._reachCheckFrame == Time.frameCount && vector == this._reachCheckOrigin && reachTarget == this._reachCheckTarget)
			{
				return this._hasClearReach;
			}
			this._reachCheckFrame = Time.frameCount;
			this._reachCheckOrigin = vector;
			this._reachCheckTarget = reachTarget;
			Vector3 vector2 = reachTarget - vector;
			float magnitude = vector2.magnitude;
			if (magnitude <= 0.01f)
			{
				return this._hasClearReach = true;
			}
			Ray ray = new Ray(vector, vector2 / magnitude);
			int num = Physics.RaycastNonAlloc(ray, InventoryPickup.ReachHits, magnitude - 0.005f, -1, QueryTriggerInteraction.Ignore);
			if (num == InventoryPickup.ReachHits.Length)
			{
				RaycastHit[] array = Physics.RaycastAll(ray, magnitude - 0.005f, -1, QueryTriggerInteraction.Ignore);
				return this._hasClearReach = this.HasNoReachBlocker(array, array.Length, player.transform);
			}
			return this._hasClearReach = this.HasNoReachBlocker(InventoryPickup.ReachHits, num, player.transform);
		}

		// Token: 0x06008609 RID: 34313 RVA: 0x0036B210 File Offset: 0x00369410
		private Vector3 GetReachTarget(Vector3 origin)
		{
			Vector3 vector = base.transform.position;
			float num = float.PositiveInfinity;
			foreach (Renderer renderer in this._interactionRenderers)
			{
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					Vector3 vector2 = renderer.bounds.ClosestPoint(origin);
					float sqrMagnitude = (vector2 - origin).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						vector = vector2;
					}
				}
			}
			if (!float.IsPositiveInfinity(num))
			{
				return vector;
			}
			foreach (Collider collider in this._interactionColliders)
			{
				if (!(collider == null) && collider.enabled && collider.gameObject.activeInHierarchy)
				{
					Vector3 vector3 = collider.bounds.ClosestPoint(origin);
					float sqrMagnitude2 = (vector3 - origin).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
						vector = vector3;
					}
				}
			}
			return vector;
		}

		// Token: 0x0600860A RID: 34314 RVA: 0x0036B318 File Offset: 0x00369518
		private bool HasNoReachBlocker(RaycastHit[] hits, int count, Transform player)
		{
			for (int i = 0; i < count; i++)
			{
				Collider collider = hits[i].collider;
				if (!(collider == null) && !collider.transform.IsChildOf(player) && !collider.transform.IsChildOf(base.transform))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600860B RID: 34315 RVA: 0x0036B36C File Offset: 0x0036956C
		private void HandleHoldInput()
		{
			WorldPickupInputAction worldPickupInputAction = this._holdInput.Step(this.interactInput.GetButtonDown(), this.interactInput.GetButtonUp(), Time.deltaTime, this.holdToWearDuration, this.IsWearableCloth || this.CanQuickSwap, this._inventoryManager.ActiveSlotIndex, this._inventoryManager.GetActiveSlotItem());
			if (worldPickupInputAction == WorldPickupInputAction.Tap)
			{
				this.DoInteract(false);
				return;
			}
			if (worldPickupInputAction == WorldPickupInputAction.Hold)
			{
				if (this.IsWearableCloth)
				{
					this.DoInteract(true);
					return;
				}
				this.BeginPickup(false, this._holdInput.CapturedSlot, this._holdInput.CapturedItem);
			}
		}

		// Token: 0x0600860C RID: 34316 RVA: 0x0036B40C File Offset: 0x0036960C
		public bool CanInteract(GameObject player)
		{
			return !this._pickupInProgress && this._inventoryManager != null && this.itemData != null && CollarAvailabilityPolicy.IsAvailable(this.itemData) && LongWayHomeLootPolicy.CanPickUp(this.itemData, this, !this.registerAsDiscovery || this.restoreMode);
		}

		// Token: 0x0600860D RID: 34317 RVA: 0x0036B469 File Offset: 0x00369669
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			this.DoInteract(false);
		}

		// Token: 0x0600860E RID: 34318 RVA: 0x0036B47C File Offset: 0x0036967C
		private void DoInteract(bool wear)
		{
			this.BeginPickup(wear, -1, null);
		}

		// Token: 0x0600860F RID: 34319 RVA: 0x0036B488 File Offset: 0x00369688
		private void BeginPickup(bool wear, int swapSlot, InventoryItemSO expectedItem)
		{
			if (!base.isActiveAndEnabled || !this.CanInteract(this._playerGameObject) || (swapSlot >= 0 && !this._inventoryManager.CanSwapWorldPickupIntoSelectedSlot(this.itemData, swapSlot, expectedItem)) || this.CandidateSqrDistance > this.detectionRadius * this.detectionRadius || !this.HasUnobstructedReach(this._playerGameObject, true) || !InteractionCandidateRegistry.IsWinner(this))
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
			if (!this.restoreMode && this.itemData.category == InventoryCategory.Clothing && !this._inventoryManager.HasFreeClothingSlot)
			{
				ToastService.Show("No free clothing slots!", "Wardrobe", null, -1f, "inventory_feedback");
				return;
			}
			this.HidePrompt();
			this._isPlayerInRange = false;
			this._holdInput.Reset();
			if (!this.restoreMode)
			{
				this._pickupInProgress = true;
				this._inventoryManager.TryPickupWithAnimation(this.itemData, base.transform, delegate
				{
					bool flag = this.CompletePickupCore(wear, swapSlot, expectedItem);
					if (!flag)
					{
						this._pickupInProgress = false;
					}
					return flag;
				});
				return;
			}
			Action restoreCallback = this._restoreCallback;
			if (restoreCallback == null)
			{
				return;
			}
			restoreCallback();
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x0036B5EC File Offset: 0x003697EC
		private bool CompletePickup(bool wear)
		{
			return this.CompletePickupCore(wear, -1, null);
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x0036B5F8 File Offset: 0x003697F8
		private bool CompletePickupCore(bool wear, int swapSlot, InventoryItemSO expectedItem)
		{
			if (this._inventoryManager == null || this.itemData == null)
			{
				return false;
			}
			if (!LongWayHomeLootPolicy.CanPickUp(this.itemData, this, !this.registerAsDiscovery || this.restoreMode))
			{
				return false;
			}
			bool flag;
			using (LongWayHomeLootPolicy.BeginPickupGrant(this, this.itemData))
			{
				if (wear && this.IsWearableCloth)
				{
					int num = this._inventoryManager.AddClothingReturnIndex(this.itemData, true, this.InstanceRarity, this.instanceVariant, this.instanceDurability01, this.registerAsDiscovery);
					if (num < 0)
					{
						return false;
					}
					WearableItemSO wearableItemSO = (WearableItemSO)this.itemData;
					if (this._wearController != null && this._wearController.IsWorn(wearableItemSO))
					{
						string pickedName = this.itemData.itemName;
						ToastService.ShowComputed(() => GameLocalization.FormatSource("inventory_feedback", "You are already wearing a {0}!", new object[] { GameLocalization.GetSource("items", pickedName) }), "Wardrobe", -1f, "inventory_feedback", null);
					}
					else if (this._wearController != null)
					{
						this._wearController.WearClothFromSlot(wearableItemSO, num);
					}
				}
				else if (swapSlot >= 0)
				{
					if (!this._inventoryManager.TrySwapWorldPickupIntoSelectedSlot(this.itemData, this.InstanceRarity, this.instanceVariant, this.instanceDurability01, this.registerAsDiscovery, swapSlot, expectedItem, true))
					{
						return false;
					}
				}
				else if (!this._inventoryManager.AddItem(this.itemData, this.InstanceRarity, this.instanceVariant, this.instanceDurability01, this.registerAsDiscovery, true))
				{
					return false;
				}
				if (this.pickupSFX != null && this.audioSource != null)
				{
					this.audioSource.PlayOneShot(this.pickupSFX);
				}
				if (this.registerAsDiscovery)
				{
					MissionGameplaySignals.NotifyItemCollected(this.itemData, this.InstanceRarity);
				}
				Action collected = this.Collected;
				if (collected != null)
				{
					collected();
				}
				Object.Destroy(base.gameObject);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06008612 RID: 34322 RVA: 0x0036B810 File Offset: 0x00369A10
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x06008613 RID: 34323 RVA: 0x0036B81C File Offset: 0x00369A1C
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
			if (this._wearController == null && this._playerGameObject != null)
			{
				this._wearController = this._playerGameObject.GetComponentInChildren<WearableUseController>(true);
			}
			if (this._wearController == null)
			{
				this._wearController = Object.FindObjectOfType<WearableUseController>();
			}
			return this._playerGameObject != null && this._inventoryManager != null;
		}

		// Token: 0x06008614 RID: 34324 RVA: 0x0036B938 File Offset: 0x00369B38
		private void ShowPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this.promptUI.gameObject != base.gameObject && !this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			this.promptUI.ShowForHandoff(this.interactionConfig, this);
		}

		// Token: 0x06008615 RID: 34325 RVA: 0x0036B9A4 File Offset: 0x00369BA4
		private void HidePrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			this.promptUI.Hide();
			if (this.promptUI.gameObject != base.gameObject)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x0400889D RID: 34973
		[Header("Interaction Config")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x0400889E RID: 34974
		[Header("Input")]
		[Tooltip("Pickup key. Tap = pick; hold = quick-wear clothes or swap the selected item slot.")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x0400889F RID: 34975
		[Tooltip("Seconds the key must be held to trigger quick-wear or a selected-slot item swap.")]
		[SerializeField]
		private float holdToWearDuration = 0.4f;

		// Token: 0x040088A0 RID: 34976
		[Header("Inventory")]
		[SerializeField]
		private InventoryItemSO itemData;

		// Token: 0x040088A1 RID: 34977
		[Header("Instance rarity (per-drop roll)")]
		[Tooltip("Instance rarity carried by THIS world object (rolled loot / dropped instance). Only used when 'hasInstanceRarity' is set (runtime Configure); editor-placed pickups fall back to the item's base rarity.")]
		[SerializeField]
		private ItemRarity instanceRarity;

		// Token: 0x040088A2 RID: 34978
		[SerializeField]
		private bool hasInstanceRarity;

		// Token: 0x040088A3 RID: 34979
		[Tooltip("Surface-variant index carried by this instance (-1 = no explicit look).")]
		[SerializeField]
		private int instanceVariant = -1;

		// Token: 0x040088A4 RID: 34980
		[Tooltip("True for authored/loot-spawned pickups. False for objects the player just dropped, so drop + re-pick cannot manufacture a permanent Collection unlock.")]
		[SerializeField]
		private bool registerAsDiscovery = true;

		// Token: 0x040088A5 RID: 34981
		[Tooltip("Remaining durability carried by this instance, 0..1 of the full bar (1 = fresh). Dropped items keep their wear through the drop+repickup round-trip.")]
		[SerializeField]
		private float instanceDurability01 = 1f;

		// Token: 0x040088A6 RID: 34982
		[Header("Cloth Restore mode (slip env)")]
		[Tooltip("When set, this pickup is in RESTORE mode: the world object is a garment that SLIPPED off the body but is STILL in the cloth slot (not removed from inventory). Pressing E restores it onto the body via the restore callback (animated) instead of adding it to the inventory. Set via ConfigureAsClothRestore (the slip bridge wires this on the slip env).")]
		[SerializeField]
		private bool restoreMode;

		// Token: 0x040088A7 RID: 34983
		private Action _restoreCallback;

		// Token: 0x040088A8 RID: 34984
		[Header("Audio")]
		[SerializeField]
		private AudioClip pickupSFX;

		// Token: 0x040088A9 RID: 34985
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040088AA RID: 34986
		[Header("Detection Settings")]
		[Tooltip("Pickup range from the item. Kept TIGHT on purpose: the player must stand right next to the item to see the prompt / pick it up.")]
		[SerializeField]
		private float detectionRadius = 1.1f;

		// Token: 0x040088AB RID: 34987
		[Tooltip("Minimum pickup range used for objects spawned from an inventory drop. Authored world pickups keep detectionRadius unchanged.")]
		[SerializeField]
		private float runtimeDropDetectionRadius = 1.45f;

		// Token: 0x040088AC RID: 34988
		[Tooltip("Height of the reach check above the player's origin. Walls between the player and the visible item block its prompt and pickup.")]
		[SerializeField]
		[Min(0f)]
		private float reachOriginHeight = 1.2f;

		// Token: 0x040088AD RID: 34989
		[Header("UI Reference")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040088AE RID: 34990
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040088AF RID: 34991
		private GameObject _playerGameObject;

		// Token: 0x040088B0 RID: 34992
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x040088B1 RID: 34993
		private WearableUseController _wearController;

		// Token: 0x040088B2 RID: 34994
		private bool _isPlayerInRange;

		// Token: 0x040088B3 RID: 34995
		private bool _inOwnRange;

		// Token: 0x040088B4 RID: 34996
		private readonly WorldPickupHoldInput _holdInput = new WorldPickupHoldInput();

		// Token: 0x040088B5 RID: 34997
		private bool _pickupInProgress;

		// Token: 0x040088B6 RID: 34998
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x040088B7 RID: 34999
		private Collider[] _interactionColliders;

		// Token: 0x040088B8 RID: 35000
		private Renderer[] _interactionRenderers;

		// Token: 0x040088B9 RID: 35001
		private int _nextReferenceResolveFrame;

		// Token: 0x040088BA RID: 35002
		private int _reachCheckFrame = -1;

		// Token: 0x040088BB RID: 35003
		private Vector3 _reachCheckOrigin;

		// Token: 0x040088BC RID: 35004
		private Vector3 _reachCheckTarget;

		// Token: 0x040088BD RID: 35005
		private bool _hasClearReach;

		// Token: 0x040088BE RID: 35006
		private static readonly RaycastHit[] ReachHits = new RaycastHit[32];

		// Token: 0x040088BF RID: 35007
		private const int ReferenceResolveRetryFrames = 30;

		// Token: 0x040088C0 RID: 35008
		private InteractionConfigSO _runtimeConfig;
	}
}
