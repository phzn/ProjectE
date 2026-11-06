using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Inventory;
using Features.NPC;
using Features.RenderingPrecision;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.InsertableToys.Vegetables
{
	// Token: 0x02001183 RID: 4483
	[DisallowMultipleComponent]
	public sealed class VegetableTheftSource : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x06008918 RID: 35096 RVA: 0x00381A38 File Offset: 0x0037FC38
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.VegetableTheftSource_interactionConfig:
				return this.interactionConfig;
			case UniversityGameplayField.VegetableTheftSource_promptPrefab:
				return this.promptPrefab;
			case UniversityGameplayField.VegetableTheftSource_vegetables:
				return this.vegetables[elementIndex];
			default:
				throw new InvalidOperationException("Unsupported VegetableTheftSource POI binding: " + field.ToString());
			}
		}

		// Token: 0x06008919 RID: 35097 RVA: 0x00381A94 File Offset: 0x0037FC94
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.VegetableTheftSource_interactionConfig:
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.VegetableTheftSource_promptPrefab:
				this.promptPrefab = (InteractablePromptUI)asset;
				return;
			case UniversityGameplayField.VegetableTheftSource_vegetables:
				this.vegetables[elementIndex] = (InsertableToyItemSO)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported VegetableTheftSource POI binding: " + field.ToString());
			}
		}

		// Token: 0x0600891A RID: 35098 RVA: 0x00381B01 File Offset: 0x0037FD01
		internal int GetPoiGameplayElementCount(UniversityGameplayField field)
		{
			if (field != UniversityGameplayField.VegetableTheftSource_vegetables)
			{
				throw new InvalidOperationException("Unsupported VegetableTheftSource indexed POI binding: " + field.ToString());
			}
			if (this.vegetables == null)
			{
				return 0;
			}
			return this.vegetables.Count;
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x0600891B RID: 35099 RVA: 0x00381B3A File Offset: 0x0037FD3A
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

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x0600891C RID: 35100 RVA: 0x00381B57 File Offset: 0x0037FD57
		public bool IsBeingUsed
		{
			get
			{
				return this._menuOpen;
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x0600891D RID: 35101 RVA: 0x00381B5F File Offset: 0x0037FD5F
		public bool HasBeenLooted
		{
			get
			{
				return this._looted;
			}
		}

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x0600891E RID: 35102 RVA: 0x00381B67 File Offset: 0x0037FD67
		public int ChoiceCount
		{
			get
			{
				if (this.vegetables == null)
				{
					return 0;
				}
				return this.vegetables.Count;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x0600891F RID: 35103 RVA: 0x00381B7E File Offset: 0x0037FD7E
		public IReadOnlyList<InsertableToyItemSO> Vegetables
		{
			get
			{
				return this.vegetables;
			}
		}

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06008920 RID: 35104 RVA: 0x00381B86 File Offset: 0x0037FD86
		public BoxCollider KombiniInteriorVolume
		{
			get
			{
				return this.kombiniInteriorVolume;
			}
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06008921 RID: 35105 RVA: 0x00381B8E File Offset: 0x0037FD8E
		public string PromptLayerName
		{
			get
			{
				return this.promptLayerName;
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06008922 RID: 35106 RVA: 0x00381B96 File Offset: 0x0037FD96
		public int PromptSortingOrder
		{
			get
			{
				return this.promptSortingOrder;
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06008923 RID: 35107 RVA: 0x00381B9E File Offset: 0x0037FD9E
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06008924 RID: 35108 RVA: 0x00381BA6 File Offset: 0x0037FDA6
		public bool IsCandidateEligible
		{
			get
			{
				return this._inOwnRange && this.CanInteract(this._playerGameObject);
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06008925 RID: 35109 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06008926 RID: 35110 RVA: 0x00381BBE File Offset: 0x0037FDBE
		private void Awake()
		{
			this._interactionCollider = base.GetComponent<Collider>();
			this.EnsureInteractionConfig();
		}

		// Token: 0x06008927 RID: 35111 RVA: 0x00381BD2 File Offset: 0x0037FDD2
		private void Start()
		{
			this.ResolvePlayerAndInventory();
			this.EnsurePrompt();
		}

		// Token: 0x06008928 RID: 35112 RVA: 0x00381BE0 File Offset: 0x0037FDE0
		private void OnEnable()
		{
			PoiResidencyStateKey poiResidencyStateKey;
			if (PoiResidencyPlacement.TryGetKey(this, "vegetable-looted", out poiResidencyStateKey))
			{
				this._looted = PoiResidencyLedger.ReadFlag(poiResidencyStateKey);
				if (this._looted)
				{
					this.RememberLootedPlacement();
				}
			}
			else if (this._looted && this.HasLootedPlacementChanged())
			{
				this.ResetTheft();
			}
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06008929 RID: 35113 RVA: 0x00381C34 File Offset: 0x0037FE34
		private Vector3 CurrentPlacementPosition
		{
			get
			{
				return WorldOrigin.ToLogical((this.kombiniInteriorVolume != null) ? this.kombiniInteriorVolume.transform.position : base.transform.position);
			}
		}

		// Token: 0x0600892A RID: 35114 RVA: 0x00381C68 File Offset: 0x0037FE68
		private bool HasLootedPlacementChanged()
		{
			if (this._lootedScene == base.gameObject.scene.handle && (this.CurrentPlacementPosition - this._lootedPlacement).sqrMagnitude <= 0.01f)
			{
				string lootedRunId = this._lootedRunId;
				NPCRosterSnapshot currentNpcRoster = RunModeService.CurrentNpcRoster;
				if (!(lootedRunId != ((currentNpcRoster != null) ? currentNpcRoster.runId : null)))
				{
					ChunkManager instance = ChunkManager.Instance;
					int num;
					return this._lootedHasMapSeed && instance != null && instance.TryGetSessionSeed(out num) && (num != this._lootedMapSeed || instance.TravelOffset != this._lootedTravelOffset);
				}
			}
			return true;
		}

		// Token: 0x0600892B RID: 35115 RVA: 0x00381D10 File Offset: 0x0037FF10
		private void RememberLootedPlacement()
		{
			PoiResidencyPlacement.WriteFlag(this, "vegetable-looted", true);
			this._lootedPlacement = this.CurrentPlacementPosition;
			this._lootedScene = base.gameObject.scene.handle;
			NPCRosterSnapshot currentNpcRoster = RunModeService.CurrentNpcRoster;
			this._lootedRunId = ((currentNpcRoster != null) ? currentNpcRoster.runId : null);
			ChunkManager instance = ChunkManager.Instance;
			this._lootedHasMapSeed = instance != null && instance.TryGetSessionSeed(out this._lootedMapSeed);
			this._lootedTravelOffset = ((instance != null) ? instance.TravelOffset : Vector2Int.zero);
		}

		// Token: 0x0600892C RID: 35116 RVA: 0x00381DA5 File Offset: 0x0037FFA5
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.SetPromptVisible(false);
			VegetableTheftChoiceUI.CloseIfOwnedBy(this);
			this._menuOpen = false;
			this._inOwnRange = false;
			this._sqrDistance = float.MaxValue;
		}

		// Token: 0x0600892D RID: 35117 RVA: 0x00381DD3 File Offset: 0x0037FFD3
		private void OnDestroy()
		{
			if (this._runtimeInteractionConfig != null)
			{
				Object.Destroy(this._runtimeInteractionConfig);
			}
		}

		// Token: 0x0600892E RID: 35118 RVA: 0x00381DEE File Offset: 0x0037FFEE
		private void OnTransformParentChanged()
		{
			if (this._prompt != null)
			{
				this._prompt.MatchGymPlacement(base.transform, 0.008525f, 0.3f);
			}
		}

		// Token: 0x0600892F RID: 35119 RVA: 0x00381E1C File Offset: 0x0038001C
		private void Update()
		{
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

		// Token: 0x06008930 RID: 35120 RVA: 0x00381EBA File Offset: 0x003800BA
		public bool CanInteract(GameObject player)
		{
			return !this._looted && !this._menuOpen && !FlyCamera.IsActive && this._inventory != null && this.HasValidVegetable() && this.IsPlayerInsideKombini(player);
		}

		// Token: 0x06008931 RID: 35121 RVA: 0x00381EF4 File Offset: 0x003800F4
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
			GameObject gameObject = ((player != null) ? player : this._playerGameObject);
			if (!this.IsPlayerInsideKombini(gameObject))
			{
				this.SetPromptVisible(false);
				return;
			}
			this.RefreshCandidateDistance();
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			this._menuOpen = true;
			this.SetPromptVisible(false);
			try
			{
				if (!VegetableTheftChoiceUI.TryOpen(this, gameObject, this.vegetables))
				{
					this._menuOpen = false;
					if (this.debugLogs)
					{
						Debug.LogWarning("[VegetableTheft] The choice menu could not be opened.", this);
					}
				}
			}
			catch
			{
				this._menuOpen = false;
				throw;
			}
		}

		// Token: 0x06008932 RID: 35122 RVA: 0x00381FC4 File Offset: 0x003801C4
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x06008933 RID: 35123 RVA: 0x00381FD0 File Offset: 0x003801D0
		public bool TryClaimVegetable(int index, out InsertableToyItemSO claimed, out string error)
		{
			claimed = null;
			error = string.Empty;
			if (this._looted || !this._menuOpen)
			{
				error = "This vegetable crate is no longer available";
				return false;
			}
			if (this.vegetables == null || index < 0 || index >= this.vegetables.Count)
			{
				error = "Invalid vegetable choice";
				return false;
			}
			InsertableToyItemSO insertableToyItemSO = this.vegetables[index];
			if (insertableToyItemSO == null)
			{
				error = "This vegetable is unavailable";
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
			if (this._inventory.IsSlotInputLocked)
			{
				error = "Remove the active item before stealing a vegetable";
				return false;
			}
			if (!this._inventory.AddItem(insertableToyItemSO))
			{
				error = "No free item slots - drop an item and try again";
				return false;
			}
			claimed = insertableToyItemSO;
			this._looted = true;
			this.RememberLootedPlacement();
			this._menuOpen = false;
			this.SetPromptVisible(false);
			if (this.debugLogs)
			{
				Debug.Log("[VegetableTheft] Stole '" + insertableToyItemSO.itemName + "'.", this);
			}
			return true;
		}

		// Token: 0x06008934 RID: 35124 RVA: 0x003820DC File Offset: 0x003802DC
		public void NotifyTheftMenuClosed()
		{
			this._menuOpen = false;
		}

		// Token: 0x06008935 RID: 35125 RVA: 0x003820E5 File Offset: 0x003802E5
		[ContextMenu("Reset Vegetable Theft (Runtime)")]
		public void ResetTheft()
		{
			PoiResidencyPlacement.WriteFlag(this, "vegetable-looted", false);
			VegetableTheftChoiceUI.CloseIfOwnedBy(this);
			this._menuOpen = false;
			this._looted = false;
		}

		// Token: 0x06008936 RID: 35126 RVA: 0x00382108 File Offset: 0x00380308
		private bool HasValidVegetable()
		{
			if (this.vegetables == null)
			{
				return false;
			}
			for (int i = 0; i < this.vegetables.Count; i++)
			{
				if (this.vegetables[i] != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x0038214C File Offset: 0x0038034C
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

		// Token: 0x06008938 RID: 35128 RVA: 0x00382200 File Offset: 0x00380400
		private void RefreshCandidateDistance()
		{
			if (this._playerGameObject == null)
			{
				this._inOwnRange = false;
				this._sqrDistance = float.MaxValue;
				return;
			}
			Vector3 position = this._playerGameObject.transform.position;
			if (!this.IsWorldPositionInsideKombini(position))
			{
				this._inOwnRange = false;
				this._sqrDistance = float.MaxValue;
				return;
			}
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

		// Token: 0x06008939 RID: 35129 RVA: 0x003822E8 File Offset: 0x003804E8
		public bool IsWorldPositionInsideKombini(Vector3 worldPosition)
		{
			if (this.kombiniInteriorVolume == null || !this.kombiniInteriorVolume.enabled)
			{
				return false;
			}
			Vector3 vector = this.kombiniInteriorVolume.transform.InverseTransformPoint(worldPosition) - this.kombiniInteriorVolume.center;
			Vector3 vector2 = this.kombiniInteriorVolume.size * 0.5f;
			vector2 = new Vector3(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y), Mathf.Abs(vector2.z));
			return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.y) <= vector2.y && Mathf.Abs(vector.z) <= vector2.z;
		}

		// Token: 0x0600893A RID: 35130 RVA: 0x003823B0 File Offset: 0x003805B0
		private bool IsPlayerInsideKombini(GameObject player)
		{
			if (player == null)
			{
				return false;
			}
			vThirdPersonController componentInParent = player.GetComponentInParent<vThirdPersonController>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : player.transform);
			return this.IsWorldPositionInsideKombini(transform.position);
		}

		// Token: 0x0600893B RID: 35131 RVA: 0x003823F4 File Offset: 0x003805F4
		private void EnsurePrompt()
		{
			if (this._prompt != null)
			{
				return;
			}
			this._prompt = base.GetComponentInChildren<InteractablePromptUI>(true);
			if (this._prompt == null && this.promptPrefab != null)
			{
				this._prompt = Object.Instantiate<InteractablePromptUI>(this.promptPrefab, base.transform);
				this._prompt.name = "VegetableTheftPrompt";
			}
			if (this._prompt == null)
			{
				return;
			}
			this._prompt.MatchGymPlacement(base.transform, 0.008525f, 0.3f);
			this._prompt.ConfigureRenderingLayer(this.promptLayerName, this.promptSortingOrder);
			this._prompt.Show(this.Config);
			this._prompt.Hide();
			this._promptVisible = false;
		}

		// Token: 0x0600893C RID: 35132 RVA: 0x003824C8 File Offset: 0x003806C8
		private void SetPromptVisible(bool visible)
		{
			if (this._prompt == null && visible)
			{
				this.EnsurePrompt();
			}
			if (this._prompt == null || this._promptVisible == visible)
			{
				return;
			}
			this._promptVisible = visible;
			if (visible)
			{
				this._prompt.Show(this.Config);
				return;
			}
			this._prompt.Hide();
		}

		// Token: 0x0600893D RID: 35133 RVA: 0x0038252C File Offset: 0x0038072C
		private void EnsureInteractionConfig()
		{
			if (this.interactionConfig != null || this._runtimeInteractionConfig != null)
			{
				return;
			}
			this._runtimeInteractionConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._runtimeInteractionConfig.hideFlags = HideFlags.HideAndDontSave;
			this._runtimeInteractionConfig.objectName = "Vegetable Crate";
			this._runtimeInteractionConfig.mainActionName = "Steal a vegetable";
			this._runtimeInteractionConfig.mainKeyDisplayLabel = "E";
		}

		// Token: 0x04008BB6 RID: 35766
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04008BB7 RID: 35767
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04008BB8 RID: 35768
		[SerializeField]
		[Min(0.25f)]
		private float detectionRadius = 1.45f;

		// Token: 0x04008BB9 RID: 35769
		[Header("Kombini Interior Gate")]
		[Tooltip("Authored box volume that contains the Kombini interior. The source fails closed when this reference is missing, disabled or inactive.")]
		[SerializeField]
		private BoxCollider kombiniInteriorVolume;

		// Token: 0x04008BBA RID: 35770
		[Header("Vegetables")]
		[SerializeField]
		private List<InsertableToyItemSO> vegetables = new List<InsertableToyItemSO>();

		// Token: 0x04008BBB RID: 35771
		[Header("Prompt")]
		[Tooltip("Shared world-space interaction prompt instantiated once for this source.")]
		[SerializeField]
		private InteractablePromptUI promptPrefab;

		// Token: 0x04008BBC RID: 35772
		[SerializeField]
		private string promptLayerName = "UI";

		// Token: 0x04008BBD RID: 35773
		[SerializeField]
		private int promptSortingOrder = 500;

		// Token: 0x04008BBE RID: 35774
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008BBF RID: 35775
		private GameObject _playerGameObject;

		// Token: 0x04008BC0 RID: 35776
		private PlayerInventoryManager _inventory;

		// Token: 0x04008BC1 RID: 35777
		private Collider _interactionCollider;

		// Token: 0x04008BC2 RID: 35778
		private InteractablePromptUI _prompt;

		// Token: 0x04008BC3 RID: 35779
		private InteractionConfigSO _runtimeInteractionConfig;

		// Token: 0x04008BC4 RID: 35780
		private bool _promptVisible;

		// Token: 0x04008BC5 RID: 35781
		private bool _menuOpen;

		// Token: 0x04008BC6 RID: 35782
		private bool _looted;

		// Token: 0x04008BC7 RID: 35783
		private Vector3 _lootedPlacement;

		// Token: 0x04008BC8 RID: 35784
		private int _lootedScene;

		// Token: 0x04008BC9 RID: 35785
		private string _lootedRunId;

		// Token: 0x04008BCA RID: 35786
		private bool _lootedHasMapSeed;

		// Token: 0x04008BCB RID: 35787
		private int _lootedMapSeed;

		// Token: 0x04008BCC RID: 35788
		private Vector2Int _lootedTravelOffset;

		// Token: 0x04008BCD RID: 35789
		private bool _inOwnRange;

		// Token: 0x04008BCE RID: 35790
		private float _sqrDistance = float.MaxValue;

		// Token: 0x04008BCF RID: 35791
		private float _nextResolveTime;
	}
}
