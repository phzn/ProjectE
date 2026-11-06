using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Inventory;
using Features.Missions;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.ClothingLoot
{
	// Token: 0x0200134E RID: 4942
	[DisallowMultipleComponent]
	public sealed class ClothingLootTableInteractable : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x060098B8 RID: 39096 RVA: 0x003E9FB0 File Offset: 0x003E81B0
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.ClothingLootTableInteractable_interactionConfig:
				return this.interactionConfig;
			case UniversityGameplayField.ClothingLootTableInteractable_lootPool:
				return this.lootPool;
			case UniversityGameplayField.ClothingLootTableInteractable_promptPrefab:
				return this.promptPrefab;
			default:
				throw new InvalidOperationException("Unsupported ClothingLootTableInteractable POI binding: " + field.ToString());
			}
		}

		// Token: 0x060098B9 RID: 39097 RVA: 0x003EA008 File Offset: 0x003E8208
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.ClothingLootTableInteractable_interactionConfig:
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.ClothingLootTableInteractable_lootPool:
				this.lootPool = (ClothingLootPoolSO)asset;
				return;
			case UniversityGameplayField.ClothingLootTableInteractable_promptPrefab:
				this.promptPrefab = (InteractablePromptUI)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported ClothingLootTableInteractable POI binding: " + field.ToString());
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x060098BA RID: 39098 RVA: 0x003EA06F File Offset: 0x003E826F
		public InteractionConfigSO Config
		{
			get
			{
				if (!(this.interactionConfig != null))
				{
					return this._runtimeInteractionConfig;
				}
				return this.interactionConfig;
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x060098BB RID: 39099 RVA: 0x003EA08C File Offset: 0x003E828C
		public bool IsBeingUsed
		{
			get
			{
				return this._menuOpen;
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x060098BC RID: 39100 RVA: 0x003EA094 File Offset: 0x003E8294
		public bool HasBeenLooted
		{
			get
			{
				PoiResidencyLedger.ClothingState clothingState = this.ResolvePoiLootState();
				if (clothingState != null)
				{
					return clothingState.looted;
				}
				ClothingLootTableInteractable.StoreLootState storeLootState = this.ResolveStoreLootState();
				if (storeLootState == null)
				{
					return this._looted;
				}
				return storeLootState.looted;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x060098BD RID: 39101 RVA: 0x003EA0BC File Offset: 0x003E82BC
		public int CachedOfferCount
		{
			get
			{
				return this.Offers.Count;
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x060098BE RID: 39102 RVA: 0x003EA0C9 File Offset: 0x003E82C9
		public Transform StoreRoot
		{
			get
			{
				return this.ResolveStoreRoot();
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x060098BF RID: 39103 RVA: 0x003EA0D1 File Offset: 0x003E82D1
		public bool IsStorePlacementAllowed
		{
			get
			{
				return !this.mallOnly || this.ResolveStoreRoot() != null;
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x060098C0 RID: 39104 RVA: 0x003EA0E9 File Offset: 0x003E82E9
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x060098C1 RID: 39105 RVA: 0x003EA0F1 File Offset: 0x003E82F1
		public bool IsCandidateEligible
		{
			get
			{
				return base.isActiveAndEnabled && this._placementAllowed && this._inOwnRange && this.CanInteract(this._playerGameObject);
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x060098C2 RID: 39106 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x060098C3 RID: 39107 RVA: 0x003EA119 File Offset: 0x003E8319
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStoreLootState()
		{
			ClothingLootTableInteractable.LootByStore.Clear();
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x060098C4 RID: 39108 RVA: 0x003EA125 File Offset: 0x003E8325
		private List<ClothingLootOffer> Offers
		{
			get
			{
				PoiResidencyLedger.ClothingState clothingState = this.ResolvePoiLootState();
				List<ClothingLootOffer> list;
				if ((list = ((clothingState != null) ? clothingState.offers : null)) == null)
				{
					ClothingLootTableInteractable.StoreLootState storeLootState = this.ResolveStoreLootState();
					list = ((storeLootState != null) ? storeLootState.offers : null) ?? this._offers;
				}
				return list;
			}
		}

		// Token: 0x060098C5 RID: 39109 RVA: 0x003EA15C File Offset: 0x003E835C
		private PoiResidencyLedger.ClothingState ResolvePoiLootState()
		{
			Transform transform = this.ResolveStoreRoot();
			PoiResidencyStateKey poiResidencyStateKey;
			if (!PoiResidencyPlacement.TryGetKey((transform != null) ? transform : this, "clothing-store", out poiResidencyStateKey))
			{
				return null;
			}
			return PoiResidencyLedger.GetClothing(poiResidencyStateKey);
		}

		// Token: 0x060098C6 RID: 39110 RVA: 0x003EA194 File Offset: 0x003E8394
		private ClothingLootTableInteractable.StoreLootState ResolveStoreLootState()
		{
			Transform transform = this.ResolveStoreRoot();
			if (transform == null)
			{
				return null;
			}
			int instanceID = transform.GetInstanceID();
			ClothingLootTableInteractable.StoreLootState storeLootState;
			if (ClothingLootTableInteractable.LootByStore.TryGetValue(instanceID, out storeLootState) && storeLootState.storeRoot == transform)
			{
				return storeLootState;
			}
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, ClothingLootTableInteractable.StoreLootState> keyValuePair in ClothingLootTableInteractable.LootByStore)
			{
				if (keyValuePair.Value.storeRoot == null)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (int num in list)
			{
				ClothingLootTableInteractable.LootByStore.Remove(num);
			}
			storeLootState = new ClothingLootTableInteractable.StoreLootState
			{
				storeRoot = transform
			};
			ClothingLootTableInteractable.LootByStore[instanceID] = storeLootState;
			return storeLootState;
		}

		// Token: 0x060098C7 RID: 39111 RVA: 0x003EA2A0 File Offset: 0x003E84A0
		private void Awake()
		{
			this._interactionCollider = base.GetComponent<Collider>();
			this.ResolveLootPool();
			this.EnsureInteractionConfig();
		}

		// Token: 0x060098C8 RID: 39112 RVA: 0x003EA2BA File Offset: 0x003E84BA
		private void Start()
		{
			this.RefreshPlacementScope();
			this.ResolvePlayerAndInventory();
			this.EnsurePrompt();
		}

		// Token: 0x060098C9 RID: 39113 RVA: 0x003EA2CE File Offset: 0x003E84CE
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
			this.RefreshPlacementScope();
		}

		// Token: 0x060098CA RID: 39114 RVA: 0x003EA2DC File Offset: 0x003E84DC
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.SetPromptVisible(false);
			ClothingLootChoiceUI.CloseIfOwnedBy(this);
			this._menuOpen = false;
			this._inOwnRange = false;
			this._sqrDistance = float.MaxValue;
		}

		// Token: 0x060098CB RID: 39115 RVA: 0x003EA30A File Offset: 0x003E850A
		private void OnDestroy()
		{
			if (this._runtimeInteractionConfig != null)
			{
				Object.Destroy(this._runtimeInteractionConfig);
			}
		}

		// Token: 0x060098CC RID: 39116 RVA: 0x003EA328 File Offset: 0x003E8528
		private void OnTransformParentChanged()
		{
			this._storeRoot = null;
			this.RefreshPlacementScope();
			this._nextPlacementCheckTime = 0f;
			if (this._prompt != null)
			{
				this._prompt.MatchGymPlacement(base.transform, 0.008525f, 0.3f);
			}
		}

		// Token: 0x060098CD RID: 39117 RVA: 0x003EA378 File Offset: 0x003E8578
		private void Update()
		{
			if (Time.unscaledTime >= this._nextPlacementCheckTime)
			{
				this._nextPlacementCheckTime = Time.unscaledTime + 2f;
				this.RefreshPlacementScope();
			}
			if (!this._placementAllowed)
			{
				this.SetPromptVisible(false);
				return;
			}
			if (this._playerGameObject == null || this._inventory == null)
			{
				if (Time.unscaledTime >= this._nextResolveTime)
				{
					this._nextResolveTime = Time.unscaledTime + 0.5f;
					this.ResolvePlayerAndInventory();
				}
				this.SetPromptVisible(false);
				return;
			}
			this.RefreshCandidateDistance();
			bool flag = this._inOwnRange && this.CanInteract(this._playerGameObject) && InteractionCandidateRegistry.IsWinner(this);
			this.SetPromptVisible(flag);
			if (flag && this.interactInput.GetButtonDown())
			{
				this.OnInteract(this._playerGameObject);
			}
		}

		// Token: 0x060098CE RID: 39118 RVA: 0x003EA44C File Offset: 0x003E864C
		public bool CanInteract(GameObject player)
		{
			if (LongWayHomeLootPolicy.RestrictsWorldLoot)
			{
				return false;
			}
			this.ResolveLootPool();
			return this._placementAllowed && !this.HasBeenLooted && !this._menuOpen && !FlyCamera.IsActive && this._inventory != null && this.lootPool != null && this.lootPool.HasValidEntry;
		}

		// Token: 0x060098CF RID: 39119 RVA: 0x003EA4B0 File Offset: 0x003E86B0
		public void OnInteract(GameObject player)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this._playerGameObject == null || this._inventory == null)
			{
				this.ResolvePlayerAndInventory();
			}
			this.RefreshCandidateDistance();
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this))
			{
				return;
			}
			if (this.Offers.Count == 0)
			{
				this.Offers.AddRange(this.lootPool.RollOffers(this.minChoices, this.maxChoices, this._inventory, this.preferUnownedClothes));
			}
			if (this.Offers.Count == 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[ClothingLoot] Table '" + base.name + "' has no eligible clothing.", this);
				}
				return;
			}
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			this._menuOpen = true;
			this.SetPromptVisible(false);
			if (!ClothingLootChoiceUI.TryOpen(this, player, this.Offers))
			{
				this._menuOpen = false;
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[ClothingLoot] Opened '{0}' with {1} cached choices.", base.name, this.Offers.Count), this);
			}
		}

		// Token: 0x060098D0 RID: 39120 RVA: 0x003EA5D6 File Offset: 0x003E87D6
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x060098D1 RID: 39121 RVA: 0x003EA5E0 File Offset: 0x003E87E0
		public bool TryClaimOffer(int index, out ClothingLootOffer claimed, out string error)
		{
			claimed = default(ClothingLootOffer);
			error = string.Empty;
			if (LongWayHomeLootPolicy.RestrictsWorldLoot)
			{
				error = "Clothing finds are limited to the starting alley in Long Way Home.";
				return false;
			}
			if (this.HasBeenLooted || !this._menuOpen)
			{
				error = "This table is no longer available";
				return false;
			}
			if (index < 0 || index >= this.Offers.Count)
			{
				error = "Invalid clothing choice";
				return false;
			}
			ClothingLootOffer clothingLootOffer = this.Offers[index];
			if (!ClothingLootPoolSO.IsEligibleClothing(clothingLootOffer.clothing))
			{
				error = "This choice is not a clothing item";
				Debug.LogError("[ClothingLoot] Rejected non-clothing commit on '" + base.name + "'.", this);
				return false;
			}
			if (this._inventory == null)
			{
				this.ResolvePlayerAndInventory();
			}
			if (this._inventory == null)
			{
				error = "Player inventory is unavailable";
				return false;
			}
			if (!this._inventory.AddItem(clothingLootOffer.clothing, clothingLootOffer.rarity, clothingLootOffer.variantIndex, 1f, true, true))
			{
				error = "No free clothing slots — drop a garment and try again";
				return false;
			}
			MissionGameplaySignals.NotifyItemCollected(clothingLootOffer.clothing, clothingLootOffer.rarity);
			claimed = clothingLootOffer;
			PoiResidencyLedger.ClothingState clothingState = this.ResolvePoiLootState();
			if (clothingState != null)
			{
				clothingState.looted = true;
			}
			else
			{
				ClothingLootTableInteractable.StoreLootState storeLootState = this.ResolveStoreLootState();
				if (storeLootState != null)
				{
					storeLootState.looted = true;
				}
				else
				{
					this._looted = true;
				}
			}
			this._menuOpen = false;
			this.SetPromptVisible(false);
			if (ClothingStoreAlarmController.ShouldTrigger(this.alarmChance, Random.value))
			{
				ClothingStoreAlarmController.TryTrigger(this, this.alarmDuration);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ClothingLoot] Claimed '",
					clothingLootOffer.clothing.itemName,
					"' from '",
					base.name,
					"'."
				}), this);
			}
			return true;
		}

		// Token: 0x060098D2 RID: 39122 RVA: 0x003EA792 File Offset: 0x003E8992
		public void NotifyLootMenuClosed()
		{
			this._menuOpen = false;
		}

		// Token: 0x060098D3 RID: 39123 RVA: 0x003EA79C File Offset: 0x003E899C
		[ContextMenu("Reset Clothing Loot (Runtime)")]
		public void ResetLoot()
		{
			Transform transform = this.ResolveStoreRoot();
			if (transform != null)
			{
				foreach (ClothingLootTableInteractable clothingLootTableInteractable in transform.GetComponentsInChildren<ClothingLootTableInteractable>(true))
				{
					ClothingLootChoiceUI.CloseIfOwnedBy(clothingLootTableInteractable);
					clothingLootTableInteractable._menuOpen = false;
					clothingLootTableInteractable.SetPromptVisible(false);
				}
				PoiResidencyLedger.ClothingState clothingState = this.ResolvePoiLootState();
				if (clothingState != null)
				{
					clothingState.looted = false;
				}
				else
				{
					this.ResolveStoreLootState().looted = false;
				}
			}
			else
			{
				ClothingLootChoiceUI.CloseIfOwnedBy(this);
			}
			this._menuOpen = false;
			this._looted = false;
			PoiResidencyLedger.ClothingState clothingState2 = this.ResolvePoiLootState();
			if (clothingState2 != null)
			{
				clothingState2.looted = false;
			}
			this.Offers.Clear();
		}

		// Token: 0x060098D4 RID: 39124 RVA: 0x003EA83B File Offset: 0x003E8A3B
		private void ResolveLootPool()
		{
			if (this.lootPool == null)
			{
				this.lootPool = ClothingLootPoolSO.LoadDefaultMallPool();
			}
		}

		// Token: 0x060098D5 RID: 39125 RVA: 0x003EA858 File Offset: 0x003E8A58
		private void ResolvePlayerAndInventory()
		{
			if (PlayerRegistry.PlayerObject != null)
			{
				this._playerGameObject = PlayerRegistry.PlayerObject;
			}
			if (this._playerGameObject != null)
			{
				this._inventory = this._playerGameObject.GetComponentInChildren<PlayerInventoryManager>(true);
			}
			if (this._inventory == null)
			{
				this._inventory = PlayerInventoryManager.Instance;
			}
			if (this._playerGameObject == null && this._inventory != null)
			{
				vThirdPersonController componentInParent = this._inventory.GetComponentInParent<vThirdPersonController>();
				this._playerGameObject = ((componentInParent != null) ? componentInParent.gameObject : this._inventory.transform.root.gameObject);
			}
		}

		// Token: 0x060098D6 RID: 39126 RVA: 0x003EA90C File Offset: 0x003E8B0C
		private void RefreshCandidateDistance()
		{
			if (this._playerGameObject == null)
			{
				this._inOwnRange = false;
				this._sqrDistance = float.MaxValue;
				return;
			}
			Vector3 position = this._playerGameObject.transform.position;
			MeshCollider meshCollider = this._interactionCollider as MeshCollider;
			Vector3 vector;
			if (meshCollider != null && !meshCollider.convex)
			{
				vector = this._interactionCollider.bounds.ClosestPoint(position);
			}
			else if (this._interactionCollider != null)
			{
				vector = this._interactionCollider.ClosestPoint(position);
			}
			else
			{
				vector = base.transform.position;
			}
			this._sqrDistance = (vector - position).sqrMagnitude;
			this._inOwnRange = this._sqrDistance <= this.detectionRadius * this.detectionRadius;
		}

		// Token: 0x060098D7 RID: 39127 RVA: 0x003EA9D8 File Offset: 0x003E8BD8
		private void EnsurePrompt()
		{
			if (this._prompt != null || !this._placementAllowed)
			{
				return;
			}
			this._prompt = base.GetComponentInChildren<InteractablePromptUI>(true);
			if (this._prompt == null && this.promptPrefab != null)
			{
				this._prompt = Object.Instantiate<InteractablePromptUI>(this.promptPrefab, base.transform);
				this._prompt.name = "ClothingLootPrompt";
			}
			if (this._prompt == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[ClothingLoot] No interaction prompt prefab on '" + base.name + "'.", this);
				}
				return;
			}
			this._prompt.MatchGymPlacement(base.transform, 0.008525f, 0.3f);
			this._prompt.Show(this.Config);
			this._prompt.Hide();
			this._ownsPrompt = false;
		}

		// Token: 0x060098D8 RID: 39128 RVA: 0x003EAAC0 File Offset: 0x003E8CC0
		private void SetPromptVisible(bool visible)
		{
			if (this._prompt == null && visible)
			{
				this.EnsurePrompt();
			}
			if (this._prompt == null || this._ownsPrompt == visible)
			{
				return;
			}
			this._ownsPrompt = visible;
			if (visible)
			{
				this._prompt.Show(this.Config);
				return;
			}
			this._prompt.Hide();
		}

		// Token: 0x060098D9 RID: 39129 RVA: 0x003EAB24 File Offset: 0x003E8D24
		private void EnsureInteractionConfig()
		{
			if (this.interactionConfig != null || this._runtimeInteractionConfig != null)
			{
				return;
			}
			this._runtimeInteractionConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._runtimeInteractionConfig.hideFlags = HideFlags.HideAndDontSave;
			this._runtimeInteractionConfig.objectName = "Clothes Table";
			this._runtimeInteractionConfig.mainActionName = "Browse Clothes";
			this._runtimeInteractionConfig.mainKeyDisplayLabel = "E";
		}

		// Token: 0x060098DA RID: 39130 RVA: 0x003EAB96 File Offset: 0x003E8D96
		private void RefreshPlacementScope()
		{
			this._placementAllowed = this.IsStorePlacementAllowed;
		}

		// Token: 0x060098DB RID: 39131 RVA: 0x003EABA4 File Offset: 0x003E8DA4
		private Transform ResolveStoreRoot()
		{
			if (this._storeRoot != null)
			{
				return this._storeRoot;
			}
			Transform transform = base.transform;
			while (transform != null)
			{
				if (transform.name.StartsWith("Area_Clothing", StringComparison.OrdinalIgnoreCase))
				{
					this._storeRoot = transform;
					break;
				}
				transform = transform.parent;
			}
			return this._storeRoot;
		}

		// Token: 0x060098DC RID: 39132 RVA: 0x003EAC04 File Offset: 0x003E8E04
		public bool TryGetVisualBounds(out Bounds visualBounds)
		{
			visualBounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer))
				{
					if (!flag)
					{
						visualBounds = renderer.bounds;
						flag = true;
					}
					else
					{
						visualBounds.Encapsulate(renderer.bounds);
					}
				}
			}
			return flag;
		}

		// Token: 0x04009DCD RID: 40397
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04009DCE RID: 40398
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04009DCF RID: 40399
		[SerializeField]
		[Min(0.25f)]
		private float detectionRadius = 1.45f;

		// Token: 0x04009DD0 RID: 40400
		[Header("Mall scope")]
		[Tooltip("The source table prefabs are also used in the stadium. When enabled, this component only activates below a Mall / Area_Clothing hierarchy.")]
		[SerializeField]
		private bool mallOnly = true;

		// Token: 0x04009DD1 RID: 40401
		[Header("Loot")]
		[SerializeField]
		private ClothingLootPoolSO lootPool;

		// Token: 0x04009DD2 RID: 40402
		[SerializeField]
		[Range(1f, 4f)]
		private int minChoices = 1;

		// Token: 0x04009DD3 RID: 40403
		[SerializeField]
		[Range(1f, 4f)]
		private int maxChoices = 4;

		// Token: 0x04009DD4 RID: 40404
		[Tooltip("Prefer garments the player does not currently own. Duplicates remain possible when the complete pool is already owned.")]
		[SerializeField]
		private bool preferUnownedClothes = true;

		// Token: 0x04009DD5 RID: 40405
		[Header("Store alarm")]
		[SerializeField]
		[Range(0f, 1f)]
		private float alarmChance = 0.5f;

		// Token: 0x04009DD6 RID: 40406
		[SerializeField]
		[Min(1f)]
		private float alarmDuration = 30f;

		// Token: 0x04009DD7 RID: 40407
		[Header("Prompt")]
		[Tooltip("Shared vehicle-style world prompt prefab. Instantiated once for this table.")]
		[SerializeField]
		private InteractablePromptUI promptPrefab;

		// Token: 0x04009DD8 RID: 40408
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04009DD9 RID: 40409
		private readonly List<ClothingLootOffer> _offers = new List<ClothingLootOffer>(4);

		// Token: 0x04009DDA RID: 40410
		private static readonly Dictionary<int, ClothingLootTableInteractable.StoreLootState> LootByStore = new Dictionary<int, ClothingLootTableInteractable.StoreLootState>();

		// Token: 0x04009DDB RID: 40411
		private GameObject _playerGameObject;

		// Token: 0x04009DDC RID: 40412
		private PlayerInventoryManager _inventory;

		// Token: 0x04009DDD RID: 40413
		private Collider _interactionCollider;

		// Token: 0x04009DDE RID: 40414
		private InteractablePromptUI _prompt;

		// Token: 0x04009DDF RID: 40415
		private InteractionConfigSO _runtimeInteractionConfig;

		// Token: 0x04009DE0 RID: 40416
		private bool _placementAllowed;

		// Token: 0x04009DE1 RID: 40417
		private bool _inOwnRange;

		// Token: 0x04009DE2 RID: 40418
		private bool _ownsPrompt;

		// Token: 0x04009DE3 RID: 40419
		private bool _menuOpen;

		// Token: 0x04009DE4 RID: 40420
		private bool _looted;

		// Token: 0x04009DE5 RID: 40421
		private float _sqrDistance = float.MaxValue;

		// Token: 0x04009DE6 RID: 40422
		private float _nextResolveTime;

		// Token: 0x04009DE7 RID: 40423
		private float _nextPlacementCheckTime;

		// Token: 0x04009DE8 RID: 40424
		private Transform _storeRoot;

		// Token: 0x0200134F RID: 4943
		private sealed class StoreLootState
		{
			// Token: 0x04009DE9 RID: 40425
			public Transform storeRoot;

			// Token: 0x04009DEA RID: 40426
			public readonly List<ClothingLootOffer> offers = new List<ClothingLootOffer>(4);

			// Token: 0x04009DEB RID: 40427
			public bool looted;
		}
	}
}
